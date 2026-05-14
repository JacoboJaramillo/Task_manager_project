using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Task_manager.Core.Entities;
using TaskManager.API.Data;

namespace Task_manager_API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TaskController : Controller
  {
    private readonly AppDbContext _context;

    public TaskController(AppDbContext context)
    {
      _context = context;
    }

    //GET API TASKS

    [HttpGet]

    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTasks()
    {
      return await _context.Tasks.ToListAsync();
    }

    //GET

    [HttpGet("{id}")]

    public async Task<ActionResult<IEnumerable<TaskItem>>> getTasks(int id)
    {
      var tarea = await _context.Tasks.FindAsync(id);
      if (tarea == null)
      {
        return NotFound(new { mensaje = "Tarea no encontrada" });
      }
      return Ok(tarea);
    }

    //POST: API TAREA

    [HttpPost]
    public async Task<ActionResult<TaskItem>> postTarea(TaskItem tarea)
    {
      _context.Tasks.Add(tarea);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(getTasks), new { id = tarea.Id }, tarea);

    }

    //UPDATE: API TAREA

    [HttpPut("{id}")]
    public async Task<ActionResult> putTarea(Guid id, TaskItem tarea)
    {
      if (id != tarea.Id)
      {
        return BadRequest(new { mensaje = "Los ID's no coinciden" });
      }

      _context.Entry(tarea).State = EntityState.Modified;

      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException)
      {
        if (!tareaExists(id))
        {
          return NotFound();
        }
        else
        {
          throw;
        }
      }
      return NoContent();
    }

    //DELETE: API TAREAS

    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteTarea(Guid id)
    {
      var tarea = await _context.Tasks.FindAsync(id);

      if(tarea == null) {  return NotFound(); }
      _context.Tasks.Remove(tarea);
      await _context.SaveChangesAsync();
      return Ok(new { mensaje = "Eliminado correctamente" });
    }

    private bool tareaExists(Guid id)
    {
      return _context.Tasks.Any(t => t.Id == id);
    }
  }
}
