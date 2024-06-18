using PokemonRenting.Models;

namespace PokemonRenting.Web.Models.ViewModels
{
    public class CartVM
    {
        public CartVM()
        {
            OrderHeader = new OrderHeader();
        }
        public List<Cart> ListOfCart { get; set; }
        public OrderHeader OrderHeader { get; set; }
    }
}
