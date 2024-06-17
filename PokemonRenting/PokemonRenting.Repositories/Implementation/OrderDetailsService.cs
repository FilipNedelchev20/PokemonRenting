using Microsoft.EntityFrameworkCore;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Implementation
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private PokemonContext _context;

        public OrderDetailsService(PokemonContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderDetail> GetOrderDetail(int orderHeaderId)
        {
            return _context.OrderDetails.Where(x => x.OrderHeaderId == orderHeaderId)
                .Include(y=> y.Pokemon).ToList();
        }

        public async Task CreateOrderDetails(OrderDetail orderDetail)
        {
           await _context.OrderDetails.AddAsync(orderDetail);
            await _context.SaveChangesAsync();
        }
    }
}
