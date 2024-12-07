using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Models.DTOs;
using Models.RequestHelpers;
using Models.Extentions;
using Persistence;
using API.Extentions;
using Microsoft.AspNetCore.Authorization;
using Stripe;

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
        public async Task<ActionResult<PagedList<Models.Catalog.Product>>> GetCatalogItems([FromQuery] CatalogParams request)
        {
            var query = _context.Products.Include(c => c.Comments).Sort(request.OrderBy).Search(request?.SearchTerm).Filter(request?.Brands, request.Types).AsQueryable();
            var catalogItems = await PagedList<Models.Catalog.Product>.ToPagedList(query, request.PageNumber, request.PageSize);
            Response.AddPaginationHeader(catalogItems.MetaData);

            return catalogItems;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Models.Catalog.Product>> GetCatalogItem(Guid id)
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
        public async Task<ActionResult<Models.Catalog.Product>> CreateCatalogItem(CatalogItemCreateDto catalogItemDto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == catalogItemDto.Type);
            if (category == null)
            {
                return BadRequest("Category not found!");
            }

            var product = new Models.Catalog.Product
            {
                Name = catalogItemDto.Name,
                Brand = catalogItemDto.Brand,
                Type = category.Name,
                CategoryId = category.Id,
                Price = catalogItemDto.Price,
                Description = catalogItemDto.Description,
                QuantityInStock = catalogItemDto.QuantityInStock,
                PictureUrl = catalogItemDto.PictureUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCatalogItem", new { id = product.Id }, product);
        }

        [HttpPut("{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(Guid productId, CatalogItemUpdateDto productDto)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return BadRequest("Product not found!");
            }

            var category = await _context.Categories.FirstOrDefaultAsync(x => x.Name == productDto.Type);

            if (category == null)
            {
                return BadRequest("Category not found!");
            }

            product.Name = productDto.Name;
            product.CategoryId = productDto.CategoryId;
            product.Brand = productDto.Brand;
            product.Type = productDto.Type;
            product.Name = category.Name;
            product.CategoryId = category.Id;
            product.Price = productDto.Price;
            product.Description = productDto.Description;
            product.QuantityInStock = productDto.QuantityInStock;
            product.PictureUrl = productDto.PictureUrl;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Products.Any(e => e.Id == productId))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(product);
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

            return Ok();
        }

        [HttpPost("{productId}/comment")]
        public async Task<ActionResult<Comment>> AddCommentToCatalogItem(Guid productId, Comment comment)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                return BadRequest();
            }

            comment.ProductId = productId;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return Ok(comment);
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