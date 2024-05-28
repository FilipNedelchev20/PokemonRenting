using PokemonRenting.Web.Utility;

namespace PokemonRenting.Web.Models.ViewModels.Pokemon
{
    public class ListPokemonViewModel
    {
        public IEnumerable<PokemonViewModel> PokemonList { get; set; }
        public PageInfo PageInfo { get; set; }
        public string SearchingText { get; set; }
    }
}
