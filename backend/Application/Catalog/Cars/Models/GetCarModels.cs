using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Persistence;

namespace Application.Catalog.Cars.Models
{
    public class GetCarModels
    {
        public class Query : IRequest<List<CarModel>>
        {

        }

        public class Handler : IRequestHandler<Query, List<CarModel>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<List<CarModel>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Models.ToListAsync();
            }
        }
    }
}
