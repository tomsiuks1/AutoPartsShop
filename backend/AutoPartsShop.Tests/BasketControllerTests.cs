using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.Controllers;
using Models.Baskets;
using Models.DTOs;
using Persistence;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Models.Catalog;
using System.Security.Claims;

namespace AutoPartsShop.Tests
{
    public class BasketControllerTests
    {
        private readonly BasketController _controller;
        private readonly DataContext _context;

        public BasketControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DataContext(options);

            SeedDatabase();

            _controller = new BasketController(_context);

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "test_buyer")
            }));

            var controllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.ControllerContext = controllerContext;
        }

        private void SeedDatabase()
        {
            var products = new List<Product>
            {
                new Product { Id = Guid.NewGuid(), Name = "Laptop", Type = "Electronics", Brand = "Lenovo" },
                new Product { Id = Guid.NewGuid(), Name = "Harry Potter", Type = "Books", Brand = "knygos.lt" }
            };
            _context.Products.AddRange(products);

            var baskets = new List<Basket>
            {
                new Basket { Id = Guid.NewGuid(), BuyerId = "test_buyer", Items = new List<BasketItem> { new BasketItem { ProductId = products[0].Id, Quantity = 1 } } }
            };
            _context.Baskets.AddRange(baskets);

            _context.SaveChanges();
        }

        [Fact]
        public async Task GetBasketTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            await _controller.AddItemToBasket(productId, 1);
            var result = await _controller.GetBasket();

            var actionResult = Assert.IsType<ActionResult<BasketDto>>(result);
            var returnValue = Assert.IsType<BasketDto>(actionResult.Value);
            Assert.Equal("test_buyer", returnValue.BuyerId);
        }

        [Fact]
        public async Task RemoveBasketItemTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var result = await _controller.RemoveBasketItem(productId);

            Assert.IsType<OkResult>(result);
            var basket = await _context.Baskets.Include(b => b.Items).FirstOrDefaultAsync(b => b.BuyerId == "test_buyer");
            Assert.Empty(basket.Items);
        }

        [Fact]
        public async Task AddItemToBasketTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var result = await _controller.AddItemToBasket(productId, 1);

            var actionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<BasketDto>(actionResult.Value);
            Assert.Equal(2, returnValue.Items[0].Quantity);
        }

        [Fact]
        public async Task UpdateBasketTest()
        {
            var productId = await _context.Products.Select(p => p.Id).FirstAsync();
            var result = await _controller.UpdateBasket(productId, 2);

            var actionResult = Assert.IsType<CreatedAtRouteResult>(result);
            var returnValue = Assert.IsType<BasketDto>(actionResult.Value);
            Assert.Equal(3, returnValue.Items[0].Quantity);
        }
    }
}