using Microsoft.EntityFrameworkCore;
using Models.Baskets;
using Models.DTOs;

namespace Models.Extentions
{
    public static class BasketExtensions
    {
        public static BasketDto MapBasketToDto(this Basket basket)
        {
            return new BasketDto
            {
                Id = basket.Id,
                BuyerId = basket.BuyerId,
                PaymentIntentId = basket.PaymentIntentId,
                ClientSecret = basket.ClientSecret,
                Items = basket.Items.Select(item => new BasketItemDto
                {
                    CatalogItemId = item.CatalogItemId,
                    Name = item.CatalogItem.Name,
                    Price = item.CatalogItem.Price,
                    PictureUrl = item.CatalogItem.PictureUrl,
                    Type = item.CatalogItem.Type,
                    Brand = item.CatalogItem.Brand,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public static IQueryable<Basket> RetrieveBasketWithItems(this IQueryable<Basket> query, string buyerId)
        {
            return query
                .Include(i => i.Items)
                .ThenInclude(p => p.CatalogItem)
                .Where(b => b.BuyerId == buyerId);
        }
    }
}
