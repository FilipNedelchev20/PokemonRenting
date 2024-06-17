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

        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = await _cartService.GetCartItems(claim.Value);
            var vm = new CartVM()
            {
                ListOfCart = cartList,
                OrderHeader = new OrderHeader()
            };
            foreach (var item in cartList)
            {
                vm.OrderHeader.TotalAmount += item.TotalAmount;
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartService.RemoveFromCart(cartItemId);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = await _cartService.GetCartItems(claim.Value);
            var vm = new CartVM()
            {
                ListOfCart = cartList,
                OrderHeader = new OrderHeader()
            };
            var user =  _userService.GetApplicationUser(claim.Value);
            
            vm.OrderHeader.User = user;
            foreach (var item in cartList)
            {
                vm.OrderHeader.TotalAmount += item.TotalAmount;
            }
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Summary(CartVM vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var cartList = await _cartService.GetCartItems(claims.Value);

            vm.ListOfCart = cartList;
         
            vm.OrderHeader.OrderDate = DateTime.Now;
            vm.OrderHeader.UserId = claims.Value;
            foreach (var item in cartList)
            {
                vm.OrderHeader.TotalAmount += (item.TotalAmount);
            }
            _orderHeaderService.CreateOrderHeader(vm.OrderHeader);
            foreach (var item in vm.ListOfCart)
            {
                var orderDetail = new OrderDetail
                {
                    OrderHeaderId = vm.OrderHeader.Id,
                    PokemonId = item.PokemonId,
                    StartDate = item.StartDate,
                    ReturnDate = item.ReturnDate,
                    
                };
                _orderDetailsService.CreateOrderDetails(orderDetail);
            }
            await _cartService.ClearCart(claims.Value);
            var domain = "http://localhost:7256/";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/carts/OrderSuccess?id={vm.OrderHeader.Id}",
                CancelUrl = domain + $"customer/carts/Index",
            };
            foreach (var item in vm.ListOfCart)
            {
                var lineItemsOptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.TotalAmount * 100),
                        Currency = "BGN",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Pokemon.PokemonName,
                        },
                    },
                    Quantity = vm.ListOfCart.Count(),
                };
                options.LineItems.Add(lineItemsOptions);
            }
            var service = new SessionService();
            
            Session session = service.Create(options);
            
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        //public IActionResult OrderSuccess(int id)
        //{
        //    var orderHeader = _orderHeaderService.GetOrderHeader(id);
        //    var service = new SessionService();
        //    Session session = service.Get(orderHeader.SessionId);
        //    if (session.PaymentStatus == "Paid")
        //    {
        //        _orderHeaderService.UpdateStatus(orderHeader.Id, session.Id, session.PaymentIntentId);
        //        _orderHeaderService.UpdateOrderStatus(orderHeader.Id, GlobalConfiguration.StatusApproved, GlobalConfiguration.StatusApproved);
        //    }
        //    return View(id); 
        //}
    }
    
}
