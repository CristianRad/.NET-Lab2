using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.Models;
using Task = Lab2.Models.Task;
using System;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;

        public TasksController(TaskContext context)
        {
            _context = context;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Task>>> GetTasks(DateTime? from = null, DateTime? to = null)
        {
            IQueryable<Task> tasks = _context.Tasks;
            if (from != null)
            {
                tasks = tasks.Where(t => t.Deadline >= from);
            }
            if (to != null)
            {
                tasks = tasks.Where(t => t.Deadline <= to);
            }

            return await tasks.ToListAsync();
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Task>> GetTask(long id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            return task;
        }

        // PUT: api/Tasks/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTask(long id, Task task)
        {
            if (id != task.Id)
            {
                return BadRequest();
            }

            if (task.State == State.CLOSED)
            {
                task.ClosedAt = DateTime.Now;
            }

  
            var existingTask = _context.Tasks.AsNoTracking()
                                    .FirstOrDefault(t => t.Id == id);
            if (existingTask.State == State.CLOSED && task.State != State.CLOSED)
            {
                task.ClosedAt = null;
            }

            _context.Entry(task).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExists(id))
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

        // POST: api/Tasks
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Task>> PostTask(Task task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTask), new { id = task.Id }, task);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Task>> DeleteTask(long id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return task;
        }

        private bool TaskExists(long id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
