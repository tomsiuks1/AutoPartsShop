using MediatR;
using Models.RequestHelpers;
using Models.Catalog;
using Models.Extentions;
using Persistence;

namespace Application.Catalog.Item
{
    public class GetCatalogItems
    {
        public class Query : IRequest<PagedList<CatalogItem>>
        {
            public Query(CatalogParams catalogParams)
            {
                CatalogParams = catalogParams;
            }

            public CatalogParams CatalogParams { get; set; }
        }

        public class Handler : IRequestHandler<Query, PagedList<CatalogItem>>
        {
            private readonly DataContext _context;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<PagedList<CatalogItem>> Handle(Query request, CancellationToken cancellationToken)
            {
                var query = _context.CatalogItems
                            .Sort(request.CatalogParams.OrderBy)
                            .Search(request?.CatalogParams.SearchTerm)
                            .Filter(request?.CatalogParams.Brands, request.CatalogParams.Types)
                            .AsQueryable();

                var catalogItems =
                    await PagedList<CatalogItem>.ToPagedList(query, request.CatalogParams.PageNumber, request.CatalogParams.PageSize);

                return catalogItems;
            }
        }
    }
}
