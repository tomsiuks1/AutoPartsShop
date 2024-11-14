using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Models.DTOs;
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
            return await _context.Categories.Include(c => c.Products).ThenInclude(ci => ci.Comments).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(Guid id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpGet("{categoryId}/products")]
        public async Task<ActionResult<IEnumerable<Product>>> GetCatalogItemsByCategory(Guid categoryId)
        {
            var catalogItems = await _context.Products
                .Where(ci => ci.CategoryId == categoryId)
                .ToListAsync();

            return catalogItems;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetCategory", new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(Guid id, UpdateCategoryDto updateCategoryDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(category == null)
            {
                return BadRequest("Category not found!");
            }

            category.Name = updateCategoryDto.Name;

            await _context.SaveChangesAsync();

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

        [HttpPost("{categoryId}/product/{productId}")]
        public async Task<ActionResult<Product>> AddProductToCategory(Guid categoryId, Guid productId)
        {
            var category = await _context.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            if (product.CategoryId != categoryId)
            {
                product.CategoryId = categoryId;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("GetCategory", new { id = categoryId }, category);
        }
    }
}