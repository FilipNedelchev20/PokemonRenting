using PokemonRenting.Models;

namespace PokemonRenting.Web.Models.ViewModels.Order
{
    public class OrderViewModel
    {
        public OrderHeader OrderHeader { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
      
    }
}
