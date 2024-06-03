using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Web.Models.ViewModels.Pokemon;
using PokemonRentingModels;

namespace PokemonRenting.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private IMapper _mapper;
        public PokemonController(IPokemonRepository pokemonRepository)
        {
            _pokemonRepository = pokemonRepository;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 10, string SearchingText = null)
        {
            IEnumerable<PokemonViewModel> viewModelList;
            var pokemons = _pokemonRepository.GetPokemons().GetAwaiter()
                .GetResult().Skip(pageNumber * pageSize - pageSize).Take(pageSize);
            viewModelList = _mapper.Map<IEnumerable<PokemonViewModel>>(pokemons);
            if (!string.IsNullOrEmpty(SearchingText))
            {
                viewModelList = viewModelList.Where(x => x.PokemonNumber.Equals(SearchingText));
            }
            var pokemonViewModel = new ListPokemonViewModel
            {
                PokemonList = viewModelList,
                PageInfo = new Utility.PageInfo
                {
                    ItemsPerPage = pageSize,
                    CurrentPage = pageNumber,
                    TotalItems = _pokemonRepository.GetPokemons().GetAwaiter().GetResult().Count()
                },

            }
                ;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreatePokemonViewModel viewModel)
        {
            var model = _mapper.Map<Pokemon>(viewModel);
            await _pokemonRepository.InsertPokemon(model);
            return RedirectToAction(nameof(Index));

        }
    }

}
