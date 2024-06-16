using Microsoft.EntityFrameworkCore;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRentingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Implementation
{
    public class PokemonRepository : IPokemonRepository
    {
        private readonly PokemonContext _context;
        public PokemonRepository(PokemonContext context)
        {
            _context = context;
        }
        public async Task DeletePokemon(int id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon != null)
            {
                _context.Pokemons.Remove(pokemon);
                _context.SaveChanges();
                
            }
            
        }

        public async Task<Pokemon> GetPokemonById(int id )
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                throw new Exception($"Pokemon with ID {id} not found");
            }
            
            return pokemon;
        }

        public async Task<IEnumerable<Pokemon>> GetPokemons()
        {
            var pokemons = await _context.Pokemons.ToListAsync();
            if (pokemons.Count == 0)
            {
                throw new Exception($"Pokemon Table is Empty");
            }
            return pokemons;
        }

        public async Task InsertPokemon(Pokemon pokemon)
        {
            await _context.Pokemons.AddAsync(pokemon);
            await _context.SaveChangesAsync();
           
        }

        public async Task UpdatePokemon(Pokemon pokemon)
        {
           var pokemonFromDB = await _context.Pokemons.FindAsync(pokemon.Id);
            if (pokemonFromDB != null)
            {
                pokemonFromDB.PokemonName = pokemon.PokemonName;
                pokemonFromDB.PokemonType = pokemon.PokemonType;
                pokemonFromDB.PokemonColor = pokemon.PokemonColor;
                pokemonFromDB.PokemonNumber = pokemon.PokemonNumber;
                pokemonFromDB.DailyRate = pokemon.DailyRate;
                if (pokemon.PokemonImage != null)
                {
                    pokemonFromDB.PokemonImage = pokemon.PokemonImage;
                }
                pokemonFromDB.Generation = pokemon.Generation;
                pokemonFromDB.PokemonPrice = pokemon.PokemonPrice;
                pokemonFromDB.UpdatedAt = DateTime.UtcNow;
                pokemonFromDB.PokemonDescription = pokemon.PokemonDescription;        
                _context.SaveChanges();
             }
        }
    }
}
