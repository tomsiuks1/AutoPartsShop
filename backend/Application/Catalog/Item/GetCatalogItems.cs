using MediatR;
using Models.RequestHelpers;
using Models.Catalog;
using Models.Extentions;
using Persistence;

namespace Application.Catalog.Item
{
    public class GetCatalogItems
    {
        public class Query : IRequest<PagedList<Product>>
        {
            public Query(CatalogParams catalogParams)
            {
                CatalogParams = catalogParams;
            }

            public CatalogParams CatalogParams { get; set; }
        }

        // public class Handler : IRequestHandler<Query, PagedList<Product>>
        // {
        //     private readonly DataContext _context;

        //     public Handler(DataContext context)
        //     {
        //         _context = context;
        //     }

        //     public async Task<PagedList<Product>> Handle(Query request, CancellationToken cancellationToken)
        //     {
        //         var query = _context.Products
        //                     .Sort(request.CatalogParams.OrderBy)
        //                     .Search(request?.CatalogParams.SearchTerm)
        //                     .Filter(request?.CatalogParams.Brands, request.CatalogParams.Types)
        //                     .AsQueryable();

        //         var catalogItems =
        //             await PagedList<Products>.ToPagedList(query, request.CatalogParams.PageNumber, request.CatalogParams.PageSize);

        //         return catalogItems;
        //     }
        // }
    }
}
