using PokemonRenting.Models;
using PokemonRentingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Infrastructure
{
    public interface ICartService
    {
        void AddToCart(Pokemon pokemon);
        Task AddToCart(Cart cart);
        Task<Cart> GetCartItems(string userId, int pokemonId);
        Task<List<Cart>> GetCartItems(string userId);
        Task ClearCart(string userId);
        //Task RemoveFromCart(int pokemonId, string userId);

        //Task<decimal> GetTotalAmount(string userId);
        //Task<int> GetTotalDuration(string userId);

    }
}
