    using PokemonRenting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Infrastructure
{
    public interface IOrderDetailsService
    {
        Task CreateOrderDetails(OrderDetail orderDetail);
        IEnumerable<OrderDetail> GetOrderDetail(int orderHeaderId);
    }
}
