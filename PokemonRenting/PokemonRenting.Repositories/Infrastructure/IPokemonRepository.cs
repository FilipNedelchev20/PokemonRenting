using PokemonRentingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Infrastructure
{
    public interface IPokemonRepository
    {
        Task<IEnumerable<Pokemon>> GetPokemons();
        Task<Pokemon> GetPokemonById(int id);
        Task InsertPokemon(Pokemon pokemon);    
        Task UpdatePokemon(Pokemon pokemon);
        Task DeletePokemon(int id);
        
    }
}
