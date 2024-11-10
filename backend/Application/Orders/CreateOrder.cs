using MediatR;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.DTOs;
using Models.Extentions;
using Models.Orders;
using Persistence;

namespace Application.Orders
{
    public class CreateOrder
    {
        public class Command : IRequest<Order>
        {
            public CreateOrderDto OrderDto { get; set; }
            public string BuyerId { get; set; }

            public Command(CreateOrderDto orderDto, string buyerId)
            {
                OrderDto = orderDto;
                BuyerId = buyerId;
            }
        }

        public class Handler : IRequestHandler<Command, Order>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Order> Handle(Command request, CancellationToken cancellationToken)
            {
                var basket = await _context.Baskets
                    .RetrieveBasketWithItems(request.BuyerId)
                    .FirstOrDefaultAsync();

                if (basket == null) return null;

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
                    BuyerId = request.BuyerId,
                    ShippingAddress = request.OrderDto.ShippingAddress,
                    Subtotal = subtotal,
                    DeliveryFee = deliveryFee,
                    PaymentIntentId = basket.PaymentIntentId
                };

                _context.Orders.Add(order);
                _context.Baskets.Remove(basket);

                if (request.OrderDto.SaveAddress)
                {
                    var user = await _context.Users.
                        Include(a => a.Address)
                        .FirstOrDefaultAsync(x => x.UserName == request.BuyerId);

                    var address = new UserAddress
                    {
                        FullName = request.OrderDto.ShippingAddress.FullName,
                        Address1 = request.OrderDto.ShippingAddress.Address1,
                        Address2 = request.OrderDto.ShippingAddress.Address2,
                        City = request.OrderDto.ShippingAddress.City,
                        State = request.OrderDto.ShippingAddress.State,
                        Zip = request.OrderDto.ShippingAddress.Zip,
                        Country = request.OrderDto.ShippingAddress.Country
                    };
                    user.Address = address;
                }

                if (await _context.SaveChangesAsync() > 0)
                {
                    return order;
                }

                return null;
            }
        }
    }
}
