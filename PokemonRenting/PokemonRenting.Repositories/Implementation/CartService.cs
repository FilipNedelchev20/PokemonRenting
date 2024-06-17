using Microsoft.EntityFrameworkCore;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRentingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Implementation
{
    public class CartService : ICartService
    {
        private readonly PokemonContext _context;
        private List<Pokemon> _cartItems;


        public CartService(PokemonContext context, List<Pokemon> cartItems)
        {
            _context = context;
            _cartItems = cartItems;
        }
        public void AddToCart(Pokemon pokemon)
        {
            _cartItems.Add(pokemon);
        }
      

        public async Task AddToCart(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task ClearCart(string userId)
        {
            var cartItems = await _context.Carts.Where(x=> x.User.Id == userId).ToListAsync();
            _context.Carts.RemoveRange(cartItems);
            await _context.SaveChangesAsync();
        }

        public async Task<Cart> GetCartItems(string userId, int pokemonId)
        {
           var cart = await _context.Carts.Where(c=> c.User.Id == userId && c.PokemonId == pokemonId).FirstOrDefaultAsync();
            return cart;
        }

        public async Task<List<Cart>> GetCartItems(string userId)
        {
            var cartItems = await _context.Carts
                                          .Include(c => c.Pokemon) // Ensure Pokemon is included
                                          .Where(x => x.User.Id == userId)
                                          .ToListAsync();
            return cartItems;
        }

        public async Task RemoveFromCart(int cartItemId)
        {
            var cartItem = await _context.Carts.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.Carts.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

    }
}
