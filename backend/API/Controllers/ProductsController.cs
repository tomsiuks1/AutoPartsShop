using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Models.DTOs;
using Models.RequestHelpers;
using Models.Extentions;
using Persistence;
using API.Extentions;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly DataContext _context;

        public ProductsController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<Product>>> GetCatalogItems([FromQuery] CatalogParams request)
        {
            var query = _context.Products.Include(c => c.Comments).Sort(request.OrderBy).Search(request?.SearchTerm).Filter(request?.Brands, request.Types).AsQueryable();
            var catalogItems = await PagedList<Product>.ToPagedList(query, request.PageNumber, request.PageSize);
            Response.AddPaginationHeader(catalogItems.MetaData);

            return catalogItems;
        }

        [HttpGet("catalogItem/{id}")]
        public async Task<ActionResult<Product>> GetCatalogItem(Guid id)
        {
            var catalogItem = await _context.Products
                .Include(c => c.Comments)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (catalogItem == null)
            {
                return NotFound();
            }

            return catalogItem;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Product>> CreateCatalogItem(CatalogItemCreateDto catalogItemDto)
        {
            var category = await _context.Categories.FindAsync(catalogItemDto.CategoryId);
            if (category == null)
            {
                return BadRequest("Category not found!");
            }

            var catalogItem = new Product
            {
                Name = catalogItemDto.Name,
                CategoryId = catalogItemDto.CategoryId
            };

            _context.Products.Add(catalogItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatalogItem", new { id = catalogItem.Id }, catalogItem);
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid productId, CatalogItemUpdateDto catalogItemDto)
        {
            var catalogItem = await _context.Products.FindAsync(productId);
            if (catalogItem == null)
            {
                return NotFound();
            }

            catalogItem.Name = catalogItemDto.Name;
            catalogItem.CategoryId = catalogItemDto.CategoryId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == productId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(catalogItem);
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCatalogItem(Guid id)
        {
            var catalogItem = await _context.Products.FindAsync(id);
            if (catalogItem == null)
            {
                return NotFound();
            }

            _context.Products.Remove(catalogItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{productId}/comment")]
        public async Task<ActionResult<Comment>> AddCommentToCatalogItem(Guid productId, Comment comment)
        {
            var catalogItem = await _context.Products.FindAsync(productId);
            if (catalogItem == null)
            {
                return NotFound();
            }

            comment.ProductId = productId;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatalogItem", new { id = productId }, comment);
        }

        [HttpGet("filters")]
        public async Task<ActionResult<ProductFilter>> GetFilters()
        {
            var brands = await _context.Products.Select(p => p.Brand).Distinct().ToListAsync();
            var types = await _context.Products.Select(p => p.Type).Distinct().ToListAsync();
            return new ProductFilter
            {
                Brands = brands,
                Types = types
            };
        }
    }
}