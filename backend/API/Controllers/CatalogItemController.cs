using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Persistence;

namespace API.Controllers
{
    public class CatalogItemController : BaseApiController
    {
        private readonly DataContext _context;

        public CatalogItemController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogItem>>> GetCatalogItems()
        {
            return await _context.CatalogItems.Include(c => c.Comments).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItem>> GetCatalogItem(Guid id)
        {
            var catalogItem = await _context.CatalogItems
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (catalogItem == null)
            {
                return NotFound();
            }

            return catalogItem;
        }

        [HttpPost]
        public async Task<ActionResult<CatalogItem>> CreateCatalogItem(CatalogItem catalogItem, Guid categoryId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == categoryId);

            if (category == null) {
                return BadRequest("Category not found!");
            }

            catalogItem.Category = category;
            _context.CatalogItems.Add(catalogItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatalogItem", new { id = catalogItem.Id }, catalogItem);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCatalogItem(Guid id, CatalogItem catalogItem)
        {
            if (id != catalogItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(catalogItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.CatalogItems.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteCatalogItem(Guid id)
        {
            var catalogItem = await _context.CatalogItems.FindAsync(id);
            if (catalogItem == null)
            {
                return NotFound();
            }

            _context.CatalogItems.Remove(catalogItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{catalogItemId}/Comment")]
        public async Task<ActionResult<Comment>> AddCommentToCatalogItem(Guid catalogItemId, Comment comment)
        {
            var catalogItem = await _context.CatalogItems.FindAsync(catalogItemId);
            if (catalogItem == null)
            {
                return NotFound();
            }

            comment.CatalogItemId = catalogItemId;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatalogItem", new { id = catalogItemId }, comment);
        }
    }
}