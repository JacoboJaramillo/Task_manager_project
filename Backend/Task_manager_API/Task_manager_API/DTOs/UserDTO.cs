using Task_manager.Core.Entities;
using Task_manager_API.DTOs;
using TaskManager.API.Data;
namespace Task_manager_API.DTOs
{
  public class UserDTO
  {
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }

    public static UserDTO userSimple(User usuario)
    {
      return new UserDTO
      {
        Id = usuario.Id,
        Name = usuario.Name,
        Email = usuario.Email,
      };
    }
  }
}
