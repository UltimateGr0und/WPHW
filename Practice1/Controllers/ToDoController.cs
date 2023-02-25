using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practice1.Models;
using System.Reflection.Metadata.Ecma335;

namespace Practice1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDosController : ControllerBase
    {
        private ToDoContext db = new ToDoContext();
        private readonly ILogger<ToDosController> _logger;

        public ToDosController(ILogger<ToDosController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetToDos")]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetToDos()
        {
            return db.toDos;
        }

        [HttpGet("{id}", Name = "GetToDo")]
        public async Task<ActionResult<ToDoItem>> GetToDo(int id)
        {
            ToDoItem item = await db.toDos.FindAsync(id);
            if (item == null) { return NotFound(); }
            return Ok(item);
        }
        [HttpPost(Name = "PostToDo")]
        public async Task<ActionResult<ToDoItem>> PostToDo(ToDoItem toDo)
        {
            ToDoItem item = new ToDoItem { Deadline = toDo.Deadline, IsFinished=toDo.IsFinished, Name = toDo.Name, Priority = toDo.Priority };
            db.toDos.Add(item);
            db.SaveChanges();
            return CreatedAtAction("GetToDo", new { id = toDo.Id }, toDo);
        }
        [HttpPut("{id}",Name = "PutToDo")]
        public async Task<IActionResult> PutWeatherForecast(int id, ToDoItem toDo)
        {
            if (id != toDo.Id)
            {
                return BadRequest();
            }

            db.Entry(toDo).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoExists(id))
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
        [HttpDelete("{id}",Name = "DeleteToDo")]
        public async Task<ActionResult<ToDoItem>> DeleteToDo(int id)
        {
            var weatherForecast = await db.toDos.FindAsync(id);
            if (weatherForecast == null)
            {
                return NotFound();
            }

            db.toDos.Remove(weatherForecast);
            await db.SaveChangesAsync();

            return weatherForecast;
        }
        private bool ToDoExists(int id)
        {
            return db.toDos.Any(e => e.Id == id);
        }
    }
}