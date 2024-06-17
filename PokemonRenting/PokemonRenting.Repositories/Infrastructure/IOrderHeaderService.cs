using PokemonRenting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Infrastructure
{
    public interface IOrderHeaderService
    {
        Task CreateOrderHeader(OrderHeader orderHeader);
        OrderHeader GetOrderHeader(int id);
        IEnumerable<OrderHeader> GetAllOrders();
        IEnumerable<OrderHeader> GetAllOrdersByUserId(string userId);
      
    }
}
