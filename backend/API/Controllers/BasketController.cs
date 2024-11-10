using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Baskets;
using Models.DTOs;
using Models.Extentions;
using Persistence;

namespace API.Controllers
{
    public class BasketController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public BasketController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet(Name = "GetBasket")]
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket(GetBuyerId());

            if (basket == null) return NotFound();

            return basket.MapBasketToDto();
        }

        [HttpDelete("{productId}")]
        public async Task<ActionResult> RemoveBasketItem(Guid productId, int quantity = 1)
        {
            var basket = await RetrieveBasket(GetBuyerId());

            if (basket == null) return NotFound();

            basket.RemoveItem(productId, quantity);

            var result = await _dataContext.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest(new ProblemDetails { Title = "Problem removing item from the basket" });
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> UpdateBasket(Guid productId, int quantity = 1)
        {
            var basket = await RetrieveBasket(GetBuyerId());

            if (basket == null) basket = CreateBasket();

            var product = await _dataContext.Products.FindAsync(productId);

            if (product == null) return BadRequest(new ProblemDetails { Title = "Product not found" });

            basket.AddItem(product, quantity);

            var result = await _dataContext.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
        }


        [HttpPost]
        public async Task<ActionResult> AddItemToBasket(Guid productId, int quantity = 1)
        {
            var basket = await RetrieveBasket(GetBuyerId());

            if (basket == null) basket = CreateBasket();

            var product = await _dataContext.Products.FindAsync(productId);

            if (product == null) return BadRequest(new ProblemDetails { Title = "Product not found" });

            basket.AddItem(product, quantity);

            var result = await _dataContext.SaveChangesAsync() > 0;

            if (result) return CreatedAtRoute("GetBasket", basket.MapBasketToDto());

            return BadRequest(new ProblemDetails { Title = "Problem saving item to basket" });
        }

        private async Task<Basket> RetrieveBasket(string buyerId)
        {
            if (string.IsNullOrEmpty(buyerId))
            {
                Response.Cookies.Delete("buyerId");
                return null;
            }

            return await _dataContext.Baskets
                .Include(i => i.Items)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(basket => basket.BuyerId == buyerId);
        }

        private string GetBuyerId()
        {
            return User.Identity?.Name ?? Request.Cookies["buyerId"];
        }

        private Basket CreateBasket()
        {
            var buyerId = User.Identity?.Name;
            if (string.IsNullOrEmpty(buyerId))
            {
                buyerId = Guid.NewGuid().ToString();
                var cookieOptions = new CookieOptions { IsEssential = true, Expires = DateTime.Now.AddDays(30) };
                Response.Cookies.Append("buyerId", buyerId, cookieOptions);
            }

            var basket = new Basket { BuyerId = buyerId };
            _dataContext.Baskets.Add(basket);
            return basket;
        }
    }
}
