using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PokemonRenting.Models;
using PokemonRenting.Repositories;
using PokemonRenting.Repositories.Implementation;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Web.Models.ViewModels;
using PokemonRenting.Web.Models.ViewModels.Order;
using PokemonRenting.Web.Models.ViewModels.Pokemon;
using PokemonRentingModels;
using Stripe.Climate;
using System.Diagnostics;
using System.Security.Claims;

namespace PokemonRenting.Web.Controllers
{
    public class HomeController : Controller
    {
        private IPokemonRepository _pokemonRepository;
        private IMapper _mapper;
        private IUserService _userService;
        private ICartService _cartService;
        private PokemonContext _context;
        private IOrderDetailsService _orderDetailsService;
        private IOrderHeaderService _orderHeaderService;
        public HomeController(IPokemonRepository pokemonRepository, IMapper mapper, IUserService userService, ICartService cartService, IOrderDetailsService orderDetailsService, IOrderHeaderService orderHeaderService, PokemonContext context)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _userService = userService;
            _cartService = cartService;
            _orderDetailsService = orderDetailsService;
            _orderHeaderService = orderHeaderService;
            this._context = context;
        }



        public async Task<IActionResult> Index()
        {
            var pokemons = await _pokemonRepository.GetPokemons();
            var viewModel = _mapper.Map<List<PokemonViewModel>>(pokemons);
            return View(viewModel);
        }
        public async Task<IActionResult> Details(int id)
        {
            var pokemon = await _pokemonRepository.GetPokemonById(id);
            if (pokemon == null)
            {
                return NotFound();
            }
            var viewModel = _mapper.Map<PokemonDetailsViewModel>(pokemon);
            return View(viewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult Order(int PokemonId, DateTime StartDate, DateTime ReturnDate, decimal DailyRate, string PokemonImage)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var applicationUser = _userService.GetApplicationUser(claims.Value);

            var vm = new SummaryViewModel
            {
                Id = PokemonId,
                StartDate = StartDate,
                ReturnDate = ReturnDate,
                DailyRate = DailyRate,
                TotalAmount = DailyRate * (ReturnDate - StartDate).Days,
                PokemonImage = PokemonImage
            };

            return View(vm);
        }
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Order(SummaryViewModel vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var applicationUser = _userService.GetApplicationUser(claims.Value);

            if (ModelState.IsValid)
            {
                // Create OrderHeader
                var orderHeader = new OrderHeader
                {
                    UserId = applicationUser.Id,
                    OrderDate = DateTime.Now,
                    TotalAmount = vm.TotalAmount
                };

                await _orderHeaderService.CreateOrderHeader(orderHeader);

                // Create OrderDetails
                var orderDetails = new OrderDetail
                {
                    OrderHeaderId = orderHeader.Id,
                    PokemonId = vm.Id,
                    StartDate = vm.StartDate,
                    ReturnDate = vm.ReturnDate,
                    DailyRate = vm.DailyRate,
                    TotalDuration = (vm.ReturnDate - vm.StartDate).Days
                };

                await _orderDetailsService.CreateOrderDetails(orderDetails);

                return RedirectToAction("Index", "Home");
            }

            return View(vm);
        }

        [HttpGet]
        public IActionResult AddToCart(int id)
        {
            List<Cart> cart = GetCartItems();
            var cartItem = cart.Find(item => item.PokemonId == id);

            if (cartItem == null)
            {
                cart.Add(new Cart { PokemonId = id });
            }
            else
            {
                cartItem.Quantity++;
            }

            SaveCartItems(cart);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public IActionResult ClearCart()
        {
            var userIdString = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            Guid userId = Guid.Parse(userIdString);

            var cart = _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.UserId == userId);

            if (cart != null)
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                _context.Carts.Remove(cart);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", "Home");
        }
        private List<Cart> GetCartItems()
        {
            var sessionCart = HttpContext.Session.GetString("Cart");
            return sessionCart == null ? new List<Cart>() : JsonConvert.DeserializeObject<List<Cart>>(sessionCart);
        }

        private void SaveCartItems(List<Cart> cart)
        {
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        }

        // Example action that prepares the CartVM for the Summary view
        public IActionResult Summary()
        {
            var cartVM = new CartVM
            {
                ListOfCart = GetCartItems(),
                OrderHeader = new OrderHeader()
            };

            return View(cartVM);
        }
    }

    

}
