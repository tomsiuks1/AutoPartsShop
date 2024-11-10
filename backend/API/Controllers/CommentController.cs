using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Models.DTOs;
using Persistence;

namespace API.Controllers
{
    [Route("api/product/{productId}/comments")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly DataContext _context;

        public CommentController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetComentsByCatalogItemIdDto>>> GetComments(Guid productId)
        {
            var comments = await _context.Comments
                .Where(c => c.ProductId == productId)
                .Select(c => new GetComentsByCatalogItemIdDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    DisplayName = _context.Users.FirstOrDefault(u => u.Id == c.UserId).DisplayName
                })
                .ToListAsync();

            return comments;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Comment>> GetComment(Guid productId, Guid id)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id && c.ProductId == productId);

            if (comment == null)
            {
                return NotFound();
            }

            return comment;
        }

        [HttpPost]
        public async Task<ActionResult<Comment>> CreateComment(Guid productId, CreateCommentDto comment)
        {
            var newComment = new Comment
            {
                ProductId = productId,
                Content = comment.Content,
                UserId = comment.UserId,
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { productId, id = newComment.Id }, comment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid productId, Guid id, UpdateCommentDto comment)
        {
            if (id != comment.Id || productId != comment.ProductId)
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
                if (!_context.Comments.Any(e => e.Id == id && e.ProductId == productId))
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}