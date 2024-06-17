using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Implementation;
using PokemonRenting.Repositories.Infrastructure;
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
        private IOrderDetailsService _orderDetailsService;
        private IOrderHeaderService _orderHeaderService;
        public HomeController(IPokemonRepository pokemonRepository, IMapper mapper, IUserService userService, ICartService cartService, IOrderDetailsService orderDetailsService, IOrderHeaderService orderHeaderService)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _userService = userService;
            _cartService = cartService;
            _orderDetailsService = orderDetailsService;
            _orderHeaderService = orderHeaderService;
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
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Details(PokemonDetailsViewModel vm)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var applicationUser = _userService.GetApplicationUser(claims.Value);
            var cart = _cartService.GetCartItems(claims.Value, vm.Id);
            if (cart == null)
            {
                if (ModelState.IsValid)
                {
                    Cart cartObj = new Cart();
                    TimeSpan duration = (TimeSpan)(vm.ReturnDate - vm.StartDate);
                    cartObj.TotalAmount = vm.DailyRate * duration.Days;
                    cartObj.ReturnDate = vm.ReturnDate;
                    cartObj.StartDate = vm.StartDate;
                    cartObj.TotalAmount = vm.TotalAmount;
                    cartObj.TotalDuration = duration.Days;
                    cartObj.Pokemon.PokemonImage = vm.PokemonImage;
                    cartObj.Pokemon.Id = vm.Id;
                    cartObj.User = applicationUser;
                    await _cartService.AddToCart(cartObj);
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Message = "Pokemon already added to cart";
            }

            return View(vm);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var pokemon = await _pokemonRepository.GetPokemonById(id);
            if (pokemon == null)
            {
                return NotFound();
            }

            _cartService.AddToCart(pokemon);
            return RedirectToAction("Index");
        }
    }
}
