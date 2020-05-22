using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.Models;
using Task = Lab2.Models.Task;
using System;
using AutoMapper;
using Lab2.ViewModels;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly TaskContext _context;
        private readonly IMapper _mapper;

        public TasksController(TaskContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Tasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetTasks(DateTime? from = null, DateTime? to = null)
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

            var tasksToReturn = await tasks.ToListAsync();
            var tasksDto = _mapper.Map<IEnumerable<TaskDto>>(tasksToReturn);
            return Ok(tasksDto);
        }

        // GET: api/Tasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDto>> GetTask(long id)
        {
            var task = await _context.Tasks.FindAsync(id);

            if (task == null)
            {
                return NotFound();
            }

            var taskDto = _mapper.Map<TaskDto>(task);
            return taskDto;
        }

        [HttpGet("{taskId}/comments")]
        public async Task<ActionResult<TaskDto>> GetTaskWithComments(long taskId)
        {
            var comments = await _context.Comments
                                    .Where(c => c.Task.Id == taskId)
                                    .ToListAsync();

            var taskComments = _mapper.Map<IEnumerable<CommentDto>>(comments);

            return Ok(taskComments);
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

        [HttpPost("{taskId}/comments")]
        public async Task<ActionResult<CommentDto>> PostComment(long taskId, CommentCreationDto comment)
        {
            var task = await _context.Tasks
                                        .Include(t => t.Comments)
                                        .FirstOrDefaultAsync(t => t.Id == taskId);

            var newComment = _mapper.Map<Comment>(comment);
            task.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            var commentToReturn = _mapper.Map<CommentDto>(newComment);

            return Ok(commentToReturn);
        }

        // DELETE: api/Tasks/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TaskDto>> DeleteTask(long id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            var deletedTask = _mapper.Map<TaskDto>(task);
            return deletedTask;
        }

        private bool TaskExists(long id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
