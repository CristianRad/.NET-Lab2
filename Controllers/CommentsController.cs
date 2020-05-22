using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.Models;
using AutoMapper;
using Lab2.ViewModels;

namespace Lab2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly TaskContext _context;
        private readonly IMapper _mapper;

        public CommentsController(TaskContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Comments
        /// <summary>
        /// Gets a list of all comments.
        /// </summary>
        /// <returns>A list of Comment objects.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetComments()
        {
            var comments = await _context.Comments.ToListAsync();
            var commentsDto = _mapper.Map<IEnumerable<CommentDto>>(comments);
            return Ok(commentsDto);
        }

        // GET: api/Comments/5
        /// <summary>
        /// Retrieves a comment by a given id.
        /// </summary>
        /// <param name="id">The id of the comment to retrieve.</param>
        /// <returns>The comment with the given id.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(long id)
        {
            var comment = await _context.Comments.FindAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            var commentDto = _mapper.Map<CommentDto>(comment);
            return Ok(commentDto);
        }

        // PUT: api/Comments/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        /// <summary>
        /// Updates a comment.
        /// </summary>
        /// <param name="id">The id of the comment to update.</param>
        /// <param name="comment">The updated comment.</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComment(long id, Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest();
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // DELETE: api/Comments/5
        /// <summary>
        /// Removes a comment.
        /// </summary>
        /// <param name="id">The id of the comment to remove.</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<CommentDto>> DeleteComment(long id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            var deletedComment = _mapper.Map<CommentDto>(comment);
            return deletedComment;
        }

        private bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
