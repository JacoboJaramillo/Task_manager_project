using Task_manager.Core.Entities;

namespace Task_manager_API.DTOs
{
  public class CategoryDTO
  {
    public required Guid Id { get; set; }
    public required string Name { get; set; }

    public static CategoryDTO categoriaSimple(Category categoria)
    {
      return new CategoryDTO
      {
        Id = categoria.id,
        Name = categoria.name
      };
    }
  }
}
