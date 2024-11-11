using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Controllers;
using Models.Catalog;
using Persistence;
using System;

namespace AutoPartsShop.Tests
{
    public class CategoryControllerTests
    {   private readonly CategoryController _controller;
        private readonly DataContext _context;

        public CategoryControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            SeedDatabase();

            _controller = new CategoryController(_context);
        }

        private void SeedDatabase()
        {
            var categories = new List<Category>
            {
                new Category { Id = Guid.NewGuid(), Name = "Electronics" },
                new Category { Id = Guid.NewGuid(), Name = "Books" }
            };
            _context.Categories.AddRange(categories);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetCategoriesTest()
        {
            var result = await _controller.GetCategories();

            var actionResult = Assert.IsType<ActionResult<IEnumerable<Category>>>(result);
            var returnValue = Assert.IsType<List<Category>>(actionResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCategoryTest()
        {
            var categoryId = await _context.Categories.Select(c => c.Id).FirstAsync();
            var result = await _controller.GetCategory(categoryId);

            var actionResult = Assert.IsType<ActionResult<Category>>(result);
            var returnValue = Assert.IsType<Category>(actionResult.Value);
            Assert.Equal(categoryId, returnValue.Id);
        }
    }
}