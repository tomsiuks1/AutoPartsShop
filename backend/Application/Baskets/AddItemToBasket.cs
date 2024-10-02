using MediatR;
using Persistence;
using Models.Baskets;
using Application.Services;

namespace Application.Baskets
{
    public class AddItemToBasket
    {
        public class Command : IRequest<Basket>
        {
            public Guid CatalogItemId { get; set; }
            public int Quantity { get; set; }

            public Command(Guid catalogItemId, int quantity)
            {
                CatalogItemId = catalogItemId;
                Quantity = quantity;
            }
        }

        public class Handler : IRequestHandler<Command, Basket>
        {
            private readonly DataContext _context;
            private readonly BasketService _basketService;

            public Handler(DataContext context)
            {
                _context = context;
            }

            public async Task<Basket> Handle(Command request, CancellationToken cancellationToken)
            {
                /*                var basket = await _basketService.RetrieveBasket(_basketService.GetBuyerId());*/
                Basket basket = null;

                if (basket == null)
                {
                    basket = _basketService.CreateBasket();
                }

                var product = await _context.CatalogItems.FindAsync(request.CatalogItemId);

                if (product == null)
                {
                    return null;
                }

                basket.AddItem(product, request.Quantity);

                if (await _context.SaveChangesAsync() > 0)
                {
                    return basket;
                }

                return null;
            }
        }
    }
}
