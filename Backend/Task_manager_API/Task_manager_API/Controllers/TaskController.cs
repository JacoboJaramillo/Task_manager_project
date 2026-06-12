using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Task_manager.Core.Entities;
using Task_manager_API.DTOs;
using TaskManager.API.Data;
namespace Task_manager_API.Controllers
{
  [Authorize]
  [ApiController]
  [Route("api/[controller]")]
  public class TaskController : Controller
  {
    private readonly AppDbContext _context;
    public TaskController(AppDbContext context)
    {
      _context = context;
    }
    //GET API TASKS
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDTO>>> GetTasks()
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
     if(Guid.TryParse(userId, out Guid result))
      {
        var tareas = await _context.Tasks.Where(i => i.UserId == result).ToListAsync();
         return tareas.Select(t => TaskDTO.TareaSimple(t)).ToList();
      }
      else
      {
        return NotFound(new { mensaje = "Usuario no encontrado" });
      }
    }

    //GET{ID} API TASKS

    [HttpGet("{id}")]

    public async Task<ActionResult<TaskDTO>> GetTask(Guid id)
    {

      
      var userid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if(Guid.TryParse(userid, out Guid result))
      {
        var tareas = await _context.Tasks.Where(i => i.UserId == result && i.Id == id).FirstOrDefaultAsync();

        if (tareas == null)
        {
          return NotFound();
        }

        return Ok(new TaskDTO
        {
          Id = id,
          Title = tareas.Title,
          Description = tareas.Description,
          IsCompleted = tareas.IsCompleted,
          Priority = tareas.Priority,
          DueDate = tareas.DueDate,
        });

      }
      return NotFound();
    }


    //POST API TAREA
    [HttpPost]
    public async Task<ActionResult<TaskDTO>> PostTasks(CreateTaskDTO Task)
    {
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      if (Guid.TryParse(userId, out Guid result))
      {
        if (Task.DueDate.HasValue)
        {
          Task.DueDate = DateTime.SpecifyKind(Task.DueDate.Value, DateTimeKind.Utc);
        }
        var tareaUsuario = new TaskItem
        {
          UserId = result,
          Title = Task.Title,
          Description = Task.Description,
          IsCompleted = Task.IsCompleted,
          Priority = Task.Priority,
          DueDate = Task.DueDate
        };
        _context.Tasks.Add(tareaUsuario);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetTasks), new { id = TaskDTO.TareaSimple(tareaUsuario).Id }, TaskDTO.TareaSimple(tareaUsuario));

      }
      return BadRequest("Peticion no encontrada");
    }
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDTO>> putTarea(Guid id, CreateTaskDTO Task)
    {
      var buscarId = await _context.Tasks.FindAsync(id);
      if (buscarId == null) return NotFound();
      var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (Guid.TryParse(userId, out Guid result))
      {
        if (buscarId.UserId != result)
        {
          return NotFound();
        }
        if (Task.DueDate.HasValue)
        {
          Task.DueDate = DateTime.SpecifyKind(Task.DueDate.Value, DateTimeKind.Utc);
        }
        buscarId.Title = Task.Title;
        buscarId.Description = Task.Description;
        buscarId.IsCompleted = Task.IsCompleted;
        buscarId.Priority = Task.Priority;
        buscarId.DueDate = Task.DueDate;
        await _context.SaveChangesAsync();
        return Ok(TaskDTO.TareaSimple(buscarId));
      }
      return BadRequest();
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> deleteTarea(Guid id)
    {
      var buscarId = await _context.Tasks.FindAsync(id);
      if (buscarId == null) return NotFound();
      var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      if (Guid.TryParse(UserId, out Guid result))
      {
        if(buscarId.UserId != result)
        {
          return NotFound();
        }
        _context.Tasks.Remove(buscarId);
        await _context.SaveChangesAsync();
        return NoContent();
      }
      return BadRequest();
    }


    ////GET

    //[HttpGet("{id}")]

    //public async Task<ActionResult<IEnumerable<TaskItem>>> getTasks(int id)
    //{
    //  var tarea = await _context.Tasks.FindAsync(id);
    //  if (tarea == null)
    //  {
    //    return NotFound(new { mensaje = "Tarea no encontrada" });
    //  }
    //  return Ok(tarea);
    //}

    ////POST: API TAREA

    //[HttpPost]
    //public async Task<ActionResult<TaskItem>> postTarea(TaskItem tarea)
    //{
    //  _context.Tasks.Add(tarea);
    //  await _context.SaveChangesAsync();

    //  return CreatedAtAction(nameof(getTasks), new { id = tarea.Id }, tarea);

    //}

    ////UPDATE: API TAREA

    //[HttpPut("{id}")]
    //public async Task<ActionResult> putTarea(Guid id, TaskItem tarea)
    //{
    //  if (id != tarea.Id)
    //  {
    //    return BadRequest(new { mensaje = "Los ID's no coinciden" });
    //  }

    //  _context.Entry(tarea).State = EntityState.Modified;

    //  try
    //  {
    //    await _context.SaveChangesAsync();
    //  }
    //  catch (DbUpdateConcurrencyException)
    //  {
    //    if (!tareaExists(id))
    //    {
    //      return NotFound();
    //    }
    //    else
    //    {
    //      throw;
    //    }
    //  }
    //  return NoContent();
    //}

    ////DELETE: API TAREAS

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> deleteTarea(Guid id)
    //{
    //  var tarea = await _context.Tasks.FindAsync(id);

    //  if(tarea == null) {  return NotFound(); }
    //  _context.Tasks.Remove(tarea);
    //  await _context.SaveChangesAsync();
    //  return Ok(new { mensaje = "Eliminado correctamente" });
    //}

    //private bool tareaExists(Guid id)
    //{
    //  return _context.Tasks.Any(t => t.Id == id);
    //}
  }
  }
