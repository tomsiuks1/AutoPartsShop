using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Extentions;
using Models.Orders;
using Persistence;

namespace Application.Orders
{
    public class GetOrder
    {
        public class Query : IRequest<OrderDto>
        {
            public string BuyerId { get; set; }
            public Guid Id { get; set; }

            public Query(string buyerId, Guid orderId)
            {
                BuyerId = buyerId;
                Id = orderId;
            }
        }

        public class Handler : IRequestHandler<Query, OrderDto>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<OrderDto> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Orders
                    .ProjectOrderToOrderDto()
                    .Where(x => x.BuyerId == request.BuyerId && x.Id == request.Id)
                    .FirstOrDefaultAsync();
            }
        }
    }
}
