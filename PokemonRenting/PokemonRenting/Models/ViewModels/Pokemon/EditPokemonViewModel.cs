namespace PokemonRenting.Web.Models.ViewModels.Pokemon
{
    public class EditPokemonViewModel
    {
        public int Id { get; set; }

        public string PokemonName { get; set; }

        public string PokemonType { get; set; }
        public string PokemonNumber { get; set; }
        public IFormFile PokemonImageUrl { get; set; }
        public string Generation { get; set; }
        public string PokemonColor { get; set; }
        public decimal PokemonPrice { get; set; }
        public decimal DailyRate { get; set; }
        public bool IsAvailable { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string PokemonDescription { get; set; }
    }
}
