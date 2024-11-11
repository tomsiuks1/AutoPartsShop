using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers;
using Models.Catalog;
using Models.DTOs;
using Models.RequestHelpers;
using Persistence;
using Microsoft.AspNetCore.Http;

namespace AutoPartsShop.Tests
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly DataContext _context;

        public ProductsControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            SeedDatabase();

            _controller = new ProductsController(_context);

            var httpContext = new DefaultHttpContext();
            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;
        }

        private void SeedDatabase()
        {
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Electronics" },
                new Category { Id = Guid.NewGuid(), Name = "Books" }
            };
            _context.Categories.AddRange(categories);

            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Laptop", CategoryId = categories[0].Id, Type = categories[0].Name, Brand = "Lenovo" },
                new Product { Id = Guid.NewGuid(), Name = "Harry Potter", CategoryId = categories[1].Id, Type = categories[1].Name, Brand = "knygos.lt" }
            };
            _context.Products.AddRange(products);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCatalogItemsTest()
        {
            var request = new CatalogParams { PageNumber = 1, PageSize = 10 };
            var result = await _controller.GetCatalogItems(request);

            var actionResult = Assert.IsType<ActionResult<PagedList<Product>>>(result);
            var returnValue = Assert.IsType<PagedList<Product>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCatalogItemTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var result = await _controller.GetCatalogItem(productId);

            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var returnValue = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal(productId, returnValue.Id);
        }

        [Fact]
        public async Task CreateCatalogItemTest()
        {
            var category = await _context.Categories.FirstAsync();
            var catalogItemDto = new CatalogItemCreateDto { Name = "Smartphone", CategoryId = category.Id };

            var result = await _controller.CreateCatalogItem(catalogItemDto);

            var actionResult = Assert.IsType<ActionResult<Product>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Product>(createdAtActionResult.Value);
            Assert.Equal("Smartphone", returnValue.Name);
        }

        [Fact]
        public async Task UpdateProductTest()
        {
            var product = await _context.Products.FirstAsync();
            var catalogItemDto = new CatalogItemUpdateDto { Name = "Updated Laptop", CategoryId = product.CategoryId };

            var result = await _controller.UpdateProduct(product.Id, catalogItemDto);

            var actionResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<Product>(actionResult.Value);
            Assert.Equal("Updated Laptop", returnValue.Name);
        }

        [Fact]
        public async Task DeleteCatalogItemTest()
        {
            var product = await _context.Products.FirstAsync();

            var result = await _controller.DeleteCatalogItem(product.Id);

            Assert.IsType<NoContentResult>(result);
            Assert.Null(await _context.Products.FindAsync(product.Id));
        }

        [Fact]
        public async Task AddCommentToCatalogItemTest()
        {
            var product = await _context.Products.FirstAsync();
            var comment = new Comment { Content = "Great product!" };

            var result = await _controller.AddCommentToCatalogItem(product.Id, comment);

            var actionResult = Assert.IsType<ActionResult<Comment>>(result);
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            var returnValue = Assert.IsType<Comment>(createdAtActionResult.Value);
            Assert.Equal("Great product!", returnValue.Content);
        }

        [Fact]
        public async Task GetFiltersTest()
        {
            var result = await _controller.GetFilters();

            var actionResult = Assert.IsType<ActionResult<ProductFilter>>(result);
            var returnValue = Assert.IsType<ProductFilter>(actionResult.Value);
            Assert.Contains("Lenovo", returnValue.Brands);
            Assert.Contains("Books", returnValue.Types);
        }
    }
}