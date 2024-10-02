using MediatR;
using Microsoft.EntityFrameworkCore;
using Models.Catalog;
using Models.RequestHelpers;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Catalog.Item
{
    public class GetCatalogFilters
    {
        public class Query : IRequest<CatalogItemFilter>
        {
        }

        public class Handler : IRequestHandler<Query, CatalogItemFilter>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<CatalogItemFilter> Handle(Query request, CancellationToken cancellationToken)
            {
                var brands = await _context.CatalogItems.Select(p => p.Brand).Distinct().ToListAsync();
                var types = await _context.CatalogItems.Select(p => p.Type).Distinct().ToListAsync();

                return new CatalogItemFilter
                {
                    Brands = brands,
                    Types = types
                };
            }
        }
    }
}
