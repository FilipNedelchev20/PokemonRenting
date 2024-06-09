using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PokemonRenting.Repositories.Infrastructure;
using System.Security.Claims;

namespace PokemonRenting.Web.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private ICartService _cartService;

        public CartViewComponent(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId != null)
            {
                if (HttpContext.Session.GetInt32("SessionCart") != null)
                {
                    return View(HttpContext.Session.GetInt32("SessionCart"));
                }
                else
                {
                    HttpContext.Session.SetInt32("SessionCart",  _cartService.GetCartItems(userId).GetAwaiter().GetResult().Count());
                    return View(HttpContext.Session.GetInt32("SessionCart"));
                }
            }
            else
            {
                HttpContext.Session.Clear();
                return View(0);
            }
            
        }
    }
}
