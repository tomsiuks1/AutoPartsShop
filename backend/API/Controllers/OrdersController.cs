using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.Extentions;
using Models.Orders;
using Persistence;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IEmailService _emailService;

        public OrdersController(DataContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            return Ok(await _context.Orders.Include(o => o.OrderItems).ProjectOrderToOrderDto()
                    .Where(x => x.BuyerId == User.Identity.Name)
                    .ToListAsync());
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderId)
        {
            return Ok(await _context.Orders.ProjectOrderToOrderDto()
                .Where(x => x.BuyerId == User.Identity.Name && x.Id == orderId).FirstOrDefaultAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            var basket = await _context.Baskets.RetrieveBasketWithItems(User.Identity.Name).FirstOrDefaultAsync();

            if (basket == null)
            {
                return BadRequest(new ProblemDetails { Title = "Error occurred creating order" });
            }

            var items = new List<OrderItem>();

            foreach (var item in basket.Items)
            {
                var catalogItem = await _context.Products.FindAsync(item.ProductId);
                var itemOrdered = new CatalogItemOrdered
                {
                    ProductId = catalogItem.Id,
                    Name = catalogItem.Name,
                    PictureUrl = catalogItem.PictureUrl
                };
                var orderItem = new OrderItem
                {
                    ItemOrdered = itemOrdered,
                    Price = catalogItem.Price,
                    Quantity = item.Quantity
                };
                items.Add(orderItem);
                catalogItem.QuantityInStock -= item.Quantity;
            }

            var subtotal = items.Sum(item => item.Price * item.Quantity);
            var deliveryFee = subtotal > 10000 ? 0 : 500;

            var order = new Order
            {
                OrderItems = items,
                BuyerId = User.Identity.Name,
                ShippingAddress = orderDto.ShippingAddress,
                Subtotal = subtotal,
                DeliveryFee = deliveryFee,
                PaymentIntentId = basket.PaymentIntentId
            };

            _context.Orders.Add(order);
            _context.Baskets.Remove(basket);

            if (orderDto.SaveAddress)
            {
                var user = await _context.Users.
                    Include(a => a.Address)
                    .FirstOrDefaultAsync(x => x.UserName == User.Identity.Name);

                var address = new UserAddress
                {
                    FullName = orderDto.ShippingAddress.FullName,
                    Address1 = orderDto.ShippingAddress.Address1,
                    Address2 = orderDto.ShippingAddress.Address2,
                    City = orderDto.ShippingAddress.City,
                    State = orderDto.ShippingAddress.State,
                    Zip = orderDto.ShippingAddress.Zip,
                    Country = orderDto.ShippingAddress.Country
                };
                user.Address = address;
            }

            if (await _context.SaveChangesAsync() == 0)
            {
                return BadRequest(new ProblemDetails { Title = "Error occurred creating order" });
            }

            var emailBody = $"Order confirmation\n";
            emailBody += $"Shipping address: {orderDto.ShippingAddress}\n";
            emailBody += $"Delivery fee: {order.DeliveryFee}\n";

            await _emailService.SendEmail(User.Identity.Name, emailBody);

            return CreatedAtRoute("GetOrder", new { id = order.Id }, order);
        }
    }
}