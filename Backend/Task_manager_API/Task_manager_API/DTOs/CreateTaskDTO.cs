namespace Task_manager_API.DTOs
{
  public class CreateTaskDTO
  {
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public required string Priority { get; set; }
    public DateTime? DueDate { get; set; }
  }
}
