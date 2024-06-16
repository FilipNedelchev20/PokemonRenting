using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Web.Models.ViewModels.Pokemon;
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

        public HomeController(IPokemonRepository pokemonRepository, IMapper mapper, IUserService userService, ICartService cartService)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
            _userService = userService;
            _cartService = cartService;
        }



        public async Task<IActionResult> Index()
        {
            var pokemons = _pokemonRepository.GetPokemons().GetAwaiter().GetResult()
                .ToList().Where(x => !x.IsDeleted && x.IsAvailable);
            var viewModel = _mapper.Map<List<PokemonViewModel>>(pokemons);
            return View();
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
        //[HttpPost]
        //[Authorize(Roles = "Customer")]
        //public async Task<IActionResult> Order(SummaryViewModel vm)
        //{
        //}
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


    }
}
