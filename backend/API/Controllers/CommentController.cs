using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Models.DTOs;
using Persistence;
using System.Security.Claims;

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
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == User.Identity.Name);
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == productId);

            if (product == null)
            {
                return BadRequest("Product does not exist");
            }

            var newComment = new Comment
            {
                ProductId = productId,
                Content = comment.Content,
                Product = product,
                UserId = user.Id,
            };

            _context.Comments.Add(newComment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { productId, id = newComment.Id }, newComment);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid productId, Guid id, UpdateCommentDto commentDto)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return BadRequest("Comment does not exist");
            }

            comment.Content = commentDto.Content;
            await _context.SaveChangesAsync();

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