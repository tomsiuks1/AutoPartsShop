using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Extentions;
using Persistence;

namespace Application.Orders
{
    public class GetOrders
    {
        public class Query : IRequest<List<OrderDto>>
        {
            public string BuyerId { get; set; }

            public Query(string buyerId)
            {
                BuyerId = buyerId;
            }
        }

        public class Handler : IRequestHandler<Query, List<OrderDto>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<OrderDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Orders.Include(o => o.OrderItems)
                    .ProjectOrderToOrderDto()
                    .Where(x => x.BuyerId == request.BuyerId)
                    .ToListAsync();
            }
        }
    }
}
