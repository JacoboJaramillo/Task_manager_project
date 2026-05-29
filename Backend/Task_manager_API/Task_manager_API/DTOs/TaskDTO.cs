using Task_manager.Core.Entities;

namespace Task_manager_API.DTOs
{
  public class TaskDTO
  {
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public required string Priority { get; set; }
    public DateTime? DueDate { get; set; }

    public static TaskDTO TareaSimple(TaskItem tarea)
    {
      return new TaskDTO
      {
        Id = tarea.Id,
        Title = tarea.Title,
        Description = tarea.Description,
        IsCompleted = tarea.IsCompleted,
        Priority = tarea.Priority,
        DueDate = tarea.DueDate,
      };
    }

  }
}

