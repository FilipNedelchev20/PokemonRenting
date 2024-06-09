using System.ComponentModel.DataAnnotations;

namespace PokemonRenting.Web.Models.ViewModels.Pokemon
{
    public class PokemonDetailsViewModel
    {
        public int Id { get; set; }

        public string PokemonName { get; set; }

        public string PokemonType { get; set; }
        public string PokemonNumber { get; set; }
        public string PokemonImage { get; set; }
        public string Generation { get; set; }
        public string PokemonColor { get; set; }
        public string PokemonDescription { get; set; }
        public decimal DailyRate { get; set; }
        public decimal PokemonPrice { get; set; }
        [Display(Name ="Start Date")]
        [Required(ErrorMessage ="Please select a Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Return Date")]
        [Required(ErrorMessage = "Please select a End Date")]
        [DataType(DataType.Date)]

        public DateTime? ReturnDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
