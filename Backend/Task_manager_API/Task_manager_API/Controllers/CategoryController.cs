using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_manager.Core.Entities;
using Task_manager_API.DTOs;
using TaskManager.API.Data;
namespace Task_manager_API.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class CategoryController : Controller
  {
    private readonly AppDbContext _context;
    public CategoryController(AppDbContext context)
    {
      _context = context;
    }
    //Metodo GET
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryDTO>>> GetCategories()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      if (Guid.TryParse(userId, out var categoryId))
      {
        var categorias = await _context.Categories.Where(c => c.userId == categoryId).ToListAsync();
        return categorias.Select(c => CategoryDTO.categoriaSimple(c)).ToList();
      }
      else
      {
        return Unauthorized();
      }
    }
    //Metodo POST
    [HttpPost]
    public async Task<ActionResult<CategoryDTO>> PostCategories(CreateCategoryDTO categoria)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      if (Guid.TryParse(userId, out var categoryId))
      {
        var categoriaUsuario = new Category
        {
          userId = categoryId,
          name = categoria.Name,
        };
        _context.Add(categoriaUsuario);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategories), new { id = CategoryDTO.categoriaSimple(categoriaUsuario).Id }, CategoryDTO.categoriaSimple(categoriaUsuario));
      }
      return Unauthorized();
    }
    //metodo DELETE
    [HttpDelete("${id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
      var buscarId = await _context.Categories.FindAsync(id);
      if (buscarId == null) return NotFound();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (Guid.TryParse(userId, out var categoryId))
      {
        if (buscarId.userId != categoryId)
        {
          return NotFound();
        }
        _context.Categories.Remove(buscarId);
        await _context.SaveChangesAsync();
        return NoContent();
      }
      return Unauthorized();
    }
  }
}
