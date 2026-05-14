using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TaskManager.API.Data;


var builder = WebApplication.CreateBuilder(args);
var jwt = builder.Configuration.GetSection("JwtSettings");

// Add services to the container.

//Base de datos
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSQLconnection")));


// JWT

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt => opt.TokenValidationParameters = new()
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidIssuer = jwt["Issuer"],
      ValidAudience = jwt["Audience"],
      IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwt["SecretKey"]!))
    });


// CORS para Angular en desarrollo
builder.Services.AddCors(opt => opt.AddPolicy("AllowAngular",
    p => p.WithOrigins("http://localhost:4200")
           .AllowAnyMethod().AllowAnyHeader()));


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}


//METODO GET: obtener listado de tasks

//app.MapGet("/listado_tasks", async (AppDbContext dbContext) =>
//await dbContext.Tasks.ToListAsync());

//Metodo

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
