using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_manager.Core.Entities;
using TaskManager.API.Data;
using BCrypt.Net;
using Task_manager_API.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
namespace Task_manager_API.Controllers
{
  [Route("api/[Controller]")]
  [ApiController]
  public class UserController : Controller
  {
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    public UserController(AppDbContext context, IConfiguration configuration)
    {
      _context = context;
      _configuration = configuration;
    }
    //METODO: POST USUARIOS
    [HttpPost(Name = "Post_users")]
    public async Task<ActionResult<UserDTO>> postUser([FromBody] registerUserDTO dto)
    {
      //verificar que no haya un usuario con el mismo correo
      var correo = await _context.Users.FirstOrDefaultAsync(c => c.Email == dto.Email);
      if(correo != null)
      {
        return Conflict("Este correo ya esta asociado a otra cuenta");
      }

      //Encriptar contraseña pa seguridad
      string contraHasheada = BCrypt.Net.BCrypt.EnhancedHashPassword(dto.password);
      //Mapear Manualmente DTO de entrada y se manda a la DB
      var nuevoUsuario = new User
      {
        Name = dto.Name,
        Email = dto.Email,
        PasswordHash = contraHasheada,
      };
      //se guarda el nuevo usuario en la BD
      _context.Users.Add(nuevoUsuario);
      await _context.SaveChangesAsync();
      //Mapeo de salida, para mostrar al cliente solo la informacion necesario y no cosas sensibles
      return CreatedAtAction(nameof(getUsers), new { id = UserDTO.userSimple(nuevoUsuario).Id }, UserDTO.userSimple(nuevoUsuario));
    }
    //METODO: GET USUARIOS
    [HttpGet(Name = "Get_Users")]
    public async Task<ActionResult<IEnumerable<UserDTO>>> getUsers()
    {
      var usuarios = await _context.Users.ToListAsync();
      return usuarios.Select(u => UserDTO.userSimple(u)).ToList();
    }
    //Metodo User/Login
    [HttpPost("Login")]
    public async Task<ActionResult<string>> JWT_User([FromBody]loginDTO dto)
    {
      //va buscando en la base de datos al usuario
      var usuario = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
      //si no hay usuario manda error
      if (usuario == null)
      {
        return Unauthorized("Contraseña o correo invalidos");
      }
      //verifica la contraseña hasheada con el verify de BCRYPT
      bool contraValida = BCrypt.Net.BCrypt.EnhancedVerify(dto.Password, usuario.PasswordHash);
      //si la contraseña es invalida manda error
      if (!contraValida)
      {
        return Unauthorized("Contraseña o correo invalidos");
      }
      var tokenHandler = new JwtSecurityTokenHandler();
      //obtener clave secreta del appsettings.json
      var llaveSecreta = _configuration.GetSection("JwtSettings:SecretKey").Value ?? throw new InvalidOperationException("JWT key no configurada");
      var key = Encoding.UTF8.GetBytes(llaveSecreta);
      var tokenDescriptor = new SecurityTokenDescriptor()
      {
        Subject = new ClaimsIdentity(new[]
        {
          new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
          new Claim(ClaimTypes.Email, usuario.Email),
          new Claim(ClaimTypes.Name, usuario.Name),
        }),
        Expires = DateTime.UtcNow.AddMinutes(
          double.TryParse(_configuration.GetSection("JwtSettings:ExpirationMinutes").Value, out var mins) ? mins : 60),
        Issuer = _configuration.GetSection("JwtSettings:Issuer").Value,
        Audience = _configuration.GetSection("JwtSettings:Audience").Value,
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);
      var tokenJwt = tokenHandler.WriteToken(token);
      return Ok(new { token = tokenJwt });
    }
  }
}
