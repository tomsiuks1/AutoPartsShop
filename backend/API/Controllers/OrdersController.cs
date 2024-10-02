using API.Service;
using Application.Orders;
using Microsoft.AspNetCore.Mvc;
using Models.DTOs;
using Models.Orders;
using Stripe;

namespace API.Controllers
{
    public class OrdersController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<List<OrderDto>>> GetOrders()
        {
            return await Mediator.Send(new GetOrders.Query(User.Identity.Name));
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(Guid orderId)
        {
            return await Mediator.Send(new GetOrder.Query(User.Identity.Name, orderId));
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder(CreateOrderDto orderDto)
        {
            var order = await Mediator.Send(new CreateOrder.Command(orderDto, User.Identity.Name));

            if(order is null)
            {
                return BadRequest(new ProblemDetails { Title = "Error occurred creating order" });
            }

            var emailBody = $"Order confirmation\n";
            emailBody += $"Shipping address: {orderDto.ShippingAddress}\n";
            emailBody += $"Delivery fee: {order.DeliveryFee}\n";

            await EmailService.SendEmail(User.Identity.Name, emailBody);

            return CreatedAtRoute("GetOrder", new { id = order.Id }, order.Id);
        }
    }
}