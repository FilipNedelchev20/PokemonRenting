namespace PokemonRenting.Web.Models.ViewModels.Pokemon
{
    public class CreatePokemonViewModel
    {
        public string PokemonName { get; set; }

        public string PokemonType { get; set; }
        public string PokemonNumber { get; set; }
        public string PokemonImage { get; set; }
        public string Generation { get; set; }
        public string PokemonColor { get; set; }
        public IFormFile PokemonImageUrl { get; set; }
        public decimal PokemonPrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PokemonDescription { get; set; }
    }
}
