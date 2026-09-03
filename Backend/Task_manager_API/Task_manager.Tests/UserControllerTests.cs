using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Security.Cryptography.X509Certificates;
using Task_manager.Core.Entities;
using Task_manager_API.Controllers;
using Task_manager_API.DTOs;
using TaskManager.API.Data;

namespace Task_manager.Tests
{
  public class UserControllerTests
  {
    private AppDbContext GetInMemoryContext()
    {
      var options = new DbContextOptionsBuilder<AppDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;
      return new AppDbContext(options);
    }

    [Fact]
    public async Task Register_EmailYaExiste_Retorna409()
    {
      // Arrange

      var usuarioPrueba = new User
      {
        Name = "Prueba",
        Email = "Test@Test.com",
        PasswordHash = "hashcualquiera"
      };
      var context = GetInMemoryContext();
      var mockConfig = new Mock<IConfiguration>();
      // TODO: agregar un User al context con email "test@test.com"

      context.Users.Add(usuarioPrueba);
      await context.SaveChangesAsync();

      // TODO: instanciar UserController

      var controller = new UserController(context, mockConfig.Object);

      // TODO: crear RegisterUserDTO con el mismo email

      var registroPrueba = new registerUserDTO
      {
        Name = "Prueba",
        Email = "Test@Test.com",
        password = "hashcualquiera"
      };

      // Act
      // TODO: llamar al método postUser


      var registrar = await controller.postUser(registroPrueba);


      // Assert
      // TODO: verificar que retorna 409

      Assert.IsType<ConflictObjectResult>(registrar.Result);
    }

    [Fact]
    public async Task Login_credencialesIncorrectas_Retorna401()
    {
      //Arrange

      var usuarioPrueba = new User
      {
        Name = "Prueba",
        Email = "Test@Test.com",
        PasswordHash = BCrypt.Net.BCrypt.EnhancedHashPassword("contraseña123")
      };
      var context = GetInMemoryContext();
      var mockConfig = new Mock<IConfiguration>();

      //se Agrega el user al context (base de datos falsa) con los datos de usuario prueba

      context.Users.Add(usuarioPrueba);
      await context.SaveChangesAsync();

      //instanciamos userController

      var controller = new UserController(context, mockConfig.Object);

      //se crea el loginDTO con una credencial invalida

      var loginPrueba = new loginDTO
      {
        Email = "Test@Test.com",
        Password = "holamiamor123",
      };

      //Act

      //se llama el metodo que valida las credenciales desde userController

      var validar = await controller.JWT_User(loginPrueba);

      //Assert

      //verificar que retorna error 401 unauthorized

      Assert.IsType<UnauthorizedObjectResult>(validar.Result);
    }

    [Fact]

    public async Task Register_CreaCorrectamente_Retorna201()
    {
      //Arrange


      var context = GetInMemoryContext();
      var mockConfig = new Mock<IConfiguration>();

      //instanciar UserController

      var controller = new UserController(context, mockConfig.Object);

      //se crea el registerUserDTO

      var registroPrueba = new registerUserDTO
      {
        Name = "Test",
        Email = "Test@Test.com",
        password = "hashcualquiera"

      };

      //Act

      //llamar metodo para validar usuario

      var validar = await controller.postUser(registroPrueba);

      //Assert

      //verificar que el comportamiento del test de la respuesta esperada

      Assert.IsType<CreatedAtActionResult>(validar.Result);


    }
  }
}
