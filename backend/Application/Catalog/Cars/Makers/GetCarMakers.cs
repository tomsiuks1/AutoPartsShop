using Models.Catalog;
using MediatR;
using Persistence;
using Microsoft.EntityFrameworkCore;

namespace Application.Catalog.Cars.Makers
{
    public class GetCarMakers
    {
        public class Query : IRequest<List<CarMaker>>
        {

        }

        public class Handler : IRequestHandler<Query, List<CarMaker>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<CarMaker>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Makers.ToListAsync();
            }
        }
    }
}
