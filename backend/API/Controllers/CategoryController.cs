using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Persistence;

namespace API.Controllers
{
    public class CategoryController : BaseApiController
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.Include(c => c.CatalogItems).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _context.Categories
                .Include(c => c.CatalogItems)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, Category category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Categories.Any(e => e.Id == id))
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
        public async Task<IActionResult> DeleteCategory(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // [HttpPost("{categoryId}/CatalogItem/{catalogItemId}")]
        // public async Task<IActionResult> AddCatalogItemToCategory(Guid categoryId, Guid catalogItemId)
        // {
        //     var category = await _context.Categories.FindAsync(categoryId);
        //     var catalogItem = await _context.CatalogItems.FindAsync(catalogItemId);

        //     if (category == null || catalogItem == null)
        //     {
        //         return NotFound();
        //     }

        //     var catalogItemCategory = new CatalogItemCategory
        //     {
        //         CategoryId = categoryId,
        //         CatalogItemId = catalogItemId
        //     };

        //     _context.CatalogItemCategories.Add(catalogItemCategory);
        //     await _context.SaveChangesAsync();

        //     return NoContent();
        // }
    }
}