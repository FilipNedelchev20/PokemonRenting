using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Web.Models.ViewModels.ApplicationUserViewModels;
using PokemonRenting.Web.Models.ViewModels.Order;
using PokemonRenting.Web.Utility;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace PokemonRenting.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrdersController : Controller
    {
        private IOrderHeaderService _orderHeaderService;
        private IOrderDetailsService _orderDetailsService;
        private IWebHostEnvironment _webHostEnvironment;
        public OrdersController(IOrderHeaderService orderHeaderService, IOrderDetailsService orderDetailsService, IWebHostEnvironment webHostEnvironment)
        {
            _orderHeaderService = orderHeaderService;
            _orderDetailsService = orderDetailsService;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize]
        public IActionResult Index(string orderStatus)
        {
            IEnumerable<OrderHeader> orderHeader;
            if (User.IsInRole("Admin"))
            {
                orderHeader = _orderHeaderService.GetAllOrders();
            }
            else
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                var userId = claim.Value;
                orderHeader = _orderHeaderService.GetAllOrdersByUserId(userId);
            }
            switch (orderStatus)
            {
                case "Pending":
                        orderHeader.Where(o => o.PaymentStatus == GlobalConfiguration.StatusPending);
                    break;
                case "Approved":
                    orderHeader=orderHeader.Where(o=> o.PaymentStatus == GlobalConfiguration.StatusApproved);
                    break;
                case "InProcess":
                    orderHeader = orderHeader.Where(o=> o.OrderStatus == GlobalConfiguration.StatusInProcess);
                    break;
                case "Shipped":
                    orderHeader = orderHeader.Where(o=>o.OrderStatus == GlobalConfiguration.StatusShipped);
                    break;
                default:
                    break;
            }
            return View(orderHeader);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var order = new OrderViewModel
            {
                OrderHeader = _orderHeaderService.GetOrderHeader(id),
                OrderDetails = _orderDetailsService.GetOrderDetail(id)
            };
          return View(order);

        }
        [HttpPost]
        public IActionResult PayNow(OrderViewModel order)
        {
            var orderHeader = _orderHeaderService.GetOrderHeader(order.OrderHeader.Id);
            var orderDetails = _orderDetailsService.GetOrderDetail(order.OrderHeader.Id);
            var domain = "http://localhost:7256";
            var options = new SessionCreateOptions
            {
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"customer/carts/OrderSuccess?id={order.OrderHeader.Id}",
                CancelUrl = domain + $"customer/carts/Index",
            };
            foreach (var item in orderDetails)
            {
                var lineItemsOptions = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.RentalTotal * 100),
                        Currency = "BGN",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Pokemon.PokemonName,
                        },
                    },
                    Quantity = orderDetails.Count(),
                };
                options.LineItems.Add(lineItemsOptions);
            }
            var service = new SessionService();
            Session session = service.Create(options);
            _orderHeaderService.UpdateStatus(order.OrderHeader.Id,session.Id, session.PaymentIntentId);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
        [HttpPost]
        public IActionResult CancelOrder(OrderViewModel order)
        {
            var orderHeader = _orderHeaderService.GetOrderHeader(order.OrderHeader.Id);
           // var orderDetails = _orderDetailsService.GetOrderDetail(order.OrderHeader.Id);
            if (orderHeader.PaymentStatus == GlobalConfiguration.StatusApproved)
            {
                var refund = new RefundCreateOptions
                {

                };
                var service = new RefundService();
                Refund reFund = service.Create(refund);
                _orderHeaderService.UpdateOrderStatus(orderHeader.Id,GlobalConfiguration.StatusCancelled, GlobalConfiguration.StatusApproved);
            }
            else
            {

            }
            TempData["success"] = "Order Cancelled";
            return RedirectToAction("Details", "Orders", new {id= order.OrderHeader.Id});
        }
        [HttpGet]
        public IActionResult UpdateUserDetail(string userId)
        {
            UserDetailViewModel vm = new UserDetailViewModel();
            vm.UserId = userId;
            return View(vm);
        }
        //[HttpPost]
        //public IActionResult UpdateUserDetail(UserDetailViewModel vm)
        //{
        //    if (vm.TrainerLevel!= null || vm.PhotoProfId!= null)
        //    {
        //        string 
        //    }
        //}

    }
}
