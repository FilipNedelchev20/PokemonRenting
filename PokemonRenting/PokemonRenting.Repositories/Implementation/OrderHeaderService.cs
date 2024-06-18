using Microsoft.EntityFrameworkCore;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PokemonRenting.Repositories.Implementation
{
    public class OrderHeaderService : IOrderHeaderService
    {
        private PokemonContext _context;

        public OrderHeaderService(PokemonContext context)
        {
            _context = context;
        }

        public IEnumerable<OrderHeader> GetAllOrders()
        {
            return _context.OrderHeaders
                .Include(order => order.User) // Ensure this is a navigation property
                .Include(order => order.OrderDetails) // Ensure this is a navigation property
                .ThenInclude(detail => detail.Pokemon) // Ensure this is a navigation property within OrderDetails
                .ToList();
        }

        public IEnumerable<OrderHeader> GetAllOrdersByUserId(string userId)
        {
            var orders = _context.OrderHeaders.Where(x=> x.UserId == userId).Include(x => x.User).ToList();
            return orders;
        }

        public OrderHeader GetOrderHeader(int id)
        {
            return _context.OrderHeaders.Include(x=> x.User).FirstOrDefault(x=>x.Id==id);
        }

        public async Task CreateOrderHeader(OrderHeader orderHeader)
        {
          await _context.OrderHeaders.AddAsync(orderHeader);
            await _context.SaveChangesAsync();
        }

       
    }
}
