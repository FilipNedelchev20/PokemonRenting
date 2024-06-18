using Microsoft.AspNetCore.Mvc;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Web.Models.ViewModels;
using PokemonRenting.Web.Utility;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;



namespace PokemonRenting.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartsController : Controller   
    {
        private ICartService _cartService;
        private IUserService _userService;
        private IOrderHeaderService _orderHeaderService;
        private IOrderDetailsService _orderDetailsService;
        public CartsController(ICartService cartService, IUserService userService, IOrderHeaderService orderHeaderService, IOrderDetailsService orderDetailsService)
        {
            _cartService = cartService;
            _userService = userService;
            _orderHeaderService = orderHeaderService;
            _orderDetailsService = orderDetailsService;
        }

        
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCart(cartItemId);
            return RedirectToAction("Index");
        }

       
    }
    
}
