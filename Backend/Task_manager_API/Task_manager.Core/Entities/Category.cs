namespace Task_manager.Core.Entities
{
  public class Category
  {
    public Guid id { get; set; } = Guid.NewGuid();
    public string name { get; set; } = string.Empty;
    public ICollection<TaskItem> Task { get; set; } = [];
    public Guid userId { get; set; }
    public User? User { get; set; }

  }
}
