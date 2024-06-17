using PokemonRenting.Models;
using System.ComponentModel.DataAnnotations;

namespace PokemonRenting.Web.Models.ViewModels.Pokemon
{
    public class SummaryViewModel
    {
        public int Id { get; set; }

      

        public string PokemonType { get; set; }
        public string PokemonNumber { get; set; }
        public string PokemonImage { get; set; }

        public decimal TotalAmount { get; set; }
        public decimal DailyRate { get; set; }
  
        public DateTime StartDate { get; set; }
    

        public DateTime ReturnDate { get; set; }
        public int TotalDuration { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
