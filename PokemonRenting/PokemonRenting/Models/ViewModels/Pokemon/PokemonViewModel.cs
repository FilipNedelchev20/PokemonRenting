using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PokemonRenting.Web.Models.ViewModels.Pokemon
{
    public class PokemonViewModel
    {
        public int Id { get; set; }
       
        public string PokemonName { get; set; }
       
        public string PokemonType { get; set; }
        public string PokemonNumber { get; set; }
        public string PokemonImage { get; set; }
        public string Generation { get; set; }
        public string PokemonColor { get; set; }
        public decimal PokemonPrice { get; set; }
        public decimal DailyRate { get; set; }
        public bool IsAvailable { get; set; } 
        public bool IsDeleted { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public string PokemonDescription { get; set; }
    }
}
