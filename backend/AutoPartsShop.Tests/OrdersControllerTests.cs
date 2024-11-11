using API.Controllers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using Models;
using Models.DTOs;
using Models.Orders;
using Persistence;
using Microsoft.AspNetCore.Http;
using Models.Baskets;
using Models.Enums;
using Order = Models.Orders.Order;
using Product = Models.Catalog.Product;

namespace API.Tests
{
    public class OrdersControllerTests
    {
        private readonly DataContext _context;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new DataContext(options);
            _emailServiceMock = new Mock<IEmailService>();
            _controller = new OrdersController(_context, _emailServiceMock.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext
                    {
                        User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.Name, "tomban@ktu.lt")
                        }, "mock"))
                    }
                }
            };

            var testUser = new User
            {
                Id = Guid.NewGuid(),
                UserName = "tomban@ktu.lt",
                Email = "tomban@ktu.lt",
                DisplayName = "Test User"
            };

            _context.Users.Add(testUser);

            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), BuyerId = "tomban@ktu.lt", OrderDate = DateTime.UtcNow, Subtotal = 100, DeliveryFee = 10, OrderStatus = OrderStatus.Pending },
                new Order { Id = Guid.NewGuid(), BuyerId = "tomban@ktu.lt", OrderDate = DateTime.UtcNow, Subtotal = 200, DeliveryFee = 20, OrderStatus = OrderStatus.Pending }
            };

            _context.Orders.AddRange(orders);
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetOrdersTest()
        {
            var orders = new List<Order>
            {
                new Order { Id = Guid.NewGuid(), BuyerId = "tomban@ktu.lt" }
            };

            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();

            var result = await _controller.GetOrders();

            var actionResult = Assert.IsType<ActionResult<List<OrderDto>>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<List<OrderDto>>(okResult.Value);
            Assert.Equal(9, returnValue.Count);
        }

        [Fact]
        public async Task GetOrderTest()
        {
            var orderId = Guid.NewGuid();
            var orders = new List<Order>
            {
                new Order { Id = orderId, BuyerId = "tomban@ktu.lt" }
            };

            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync();

            var result = await _controller.GetOrder(orderId);

            var actionResult = Assert.IsType<ActionResult<OrderDto>>(result);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            var returnValue = Assert.IsType<OrderDto>(okResult.Value);
            Assert.Equal(orderId, returnValue.Id);
        }

        [Fact]
        public async Task CreateOrderTest()
        {
            var orderDto = new CreateOrderDto
            {
                ShippingAddress = new ShippingAddress { Address1 = "123 Test St" },
                SaveAddress = true
            };

            var basket = new Basket
            {
                BuyerId = "tomban@ktu.lt",
                Items = new List<BasketItem>
                {
                    new BasketItem { ProductId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            var product = new Product
            {
                Id = basket.Items.First().ProductId,
                Price = 100,
                QuantityInStock = 10
            };

            _context.Baskets.Add(basket);
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            _emailServiceMock.Setup(e => e.SendEmail(It.IsAny<string>(), It.IsAny<string>())).Returns(Task.CompletedTask);

            var result = await _controller.CreateOrder(orderDto);

            var actionResult = Assert.IsType<ActionResult<Order>>(result);
            var createdAtRouteResult = Assert.IsType<CreatedAtRouteResult>(actionResult.Result);
            var returnValue = Assert.IsType<Guid>(createdAtRouteResult.Value);
            var createdOrder = await _context.Orders.FindAsync(returnValue);
            Assert.NotNull(createdOrder);
            Assert.Equal("tomban@ktu.lt", createdOrder.BuyerId);
        }
    }
}