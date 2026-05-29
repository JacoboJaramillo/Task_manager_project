namespace Task_manager.Core.Entities;


public class TaskItem
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Title { get; set; } = string.Empty;
  public string? Description { get; set; }
  public bool IsCompleted { get; set; } = false;
  public string Priority { get; set; } = "Medium"; // Low, Medium, High
  public DateTime? DueDate { get; set; }
  public Guid UserId { get; set; }
  public User? User { get; set; }
  public Guid? CategoryId { get; set; }
  public Category? Category { get; set; }
}

