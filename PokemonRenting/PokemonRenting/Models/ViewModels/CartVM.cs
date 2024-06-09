using PokemonRenting.Models;

namespace PokemonRenting.Web.Models.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Cart> ListOfCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
