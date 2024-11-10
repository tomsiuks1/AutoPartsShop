using Models.DTOs;
using API.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Security.Claims;
using Models.Baskets;
using Persistence;
using Models.Extentions;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenService _tokenService;
        private readonly DataContext _dataContext;

        public AccountController(UserManager<User> userManager, TokenService tokenService, DataContext dataContext)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _dataContext = dataContext;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized();

            var isAuthenticated = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            var userBasket = await RetrieveBasket(loginDto.Email);
            var anonBasket = await RetrieveBasket(Request.Cookies["buyerId"]);

            if (anonBasket != null)
            {
                if (userBasket != null) _dataContext.Baskets.Remove(userBasket);
                anonBasket.BuyerId = user.UserName;
                Response.Cookies.Delete("buyerId");
                await _dataContext.SaveChangesAsync();
            }

            if (isAuthenticated)
            {
                return await CreateUserObject(user, anonBasket != null ? anonBasket.MapBasketToDto() : userBasket?.MapBasketToDto());
            }

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if(await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Email))
            {
                return BadRequest("Email is already taken");
            }

            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email,

            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if(result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Member");
                return await CreateUserObject(user);
            }

            return BadRequest(result.Errors);
        }

        [Authorize]
        [HttpGet("currentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailAsync(User.FindFirstValue(ClaimTypes.Email));
            var userBasket = await RetrieveBasket(User.Identity.Name);

            return await CreateUserObject(user, userBasket?.MapBasketToDto());
        }

        private async Task<UserDto> CreateUserObject(User user, BasketDto basket = null)
        {
            return new UserDto
            {
                DisplayName = user.UserName,
                Image = null,
                Token = await _tokenService.CreateToken(user),
                UserName = user.UserName,
                Basket = basket
            };
        }

        [Authorize]
        [HttpGet("savedAddress")]
        public async Task<ActionResult<UserAddress>> GetSavedAddress()
        {
            return await _userManager.Users
                .Where(x => x.UserName == User.Identity.Name)
                .Select(user => user.Address)
                .FirstOrDefaultAsync();
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
    }
}
