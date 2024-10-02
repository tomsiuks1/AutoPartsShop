using Microsoft.EntityFrameworkCore;
using Models.Baskets;
using Models;
using Persistence;

namespace Application.Services
{
    public class BasketService
    {
/*        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DataContext _context;

        public BasketService(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }*/

/*        public string GetBuyerId()
        {
            var user = _httpContextAccessor.HttpContext.User;
            return user.Identity?.Name ?? _httpContextAccessor.HttpContext.Request.Cookies["buyerId"];
        }*/

        public void SetBuyerIdCookie(string buyerId)
        {
/*            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.Now.AddDays(30)
            };
            _httpContextAccessor.HttpContext.Response.Cookies.Append("buyerId", buyerId, cookieOptions);*/
        }

        public Basket CreateBasket()
        {
            /*            var buyerId = User.Identity?.Name;
                        if (string.IsNullOrEmpty(buyerId))
                        {
                            buyerId = Guid.NewGuid().ToString();
                            var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
                            Response.Cookies.Append("buyerId", buyerId, cookieOptions);
                        }

                        var basket = new Basket { BuyerId = buyerId };
                        _context.Baskets.Add(basket);
                        return basket;*/
            return null;
        }

        public async Task<Basket> RetrieveBasket(string buyerId)
        {
            /*            if (string.IsNullOrEmpty(buyerId))
                        {
                            Response.Cookies.Delete("buyerId");
                            return null;
                        }

                        return await _context.Baskets
                            .Include(i => i.Items)
                            .ThenInclude(p => p.CatalogItem)
                            .FirstOrDefaultAsync(basket => basket.BuyerId == buyerId);*/
            return null;
        }

        public void DeleteBuyerIdCookie()
        {
/*            _httpContextAccessor.HttpContext.Response.Cookies.Delete("buyerId");*/
        }
    }
}
