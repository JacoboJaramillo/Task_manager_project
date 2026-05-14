namespace Task_manager.Core.Entities
{
  public class Category
  {
    public Guid id { get; set; } = Guid.NewGuid();
    public string name { get; set; } = string.Empty;

  }
}
