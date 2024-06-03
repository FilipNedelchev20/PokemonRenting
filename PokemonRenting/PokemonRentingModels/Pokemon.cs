using PokemonRenting.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonRentingModels
{
    public class Pokemon
    {
        public Pokemon()
        {
            CreatedAt = DateTime.UtcNow;
            IsAvailable = true;
            IsDeleted = false;
            UpdatedAt = DateTime.UtcNow;
        }
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string PokemonName { get; set; }
        [Required]
        public string PokemonType { get; set; }
        public string PokemonNumber { get; set; }
        public string PokemonColor { get; set; }
        public string PokemonImage { get; set; }
        public string Generation { get; set; }
        [Required]
        [Column(TypeName = "money")]
        public decimal PokemonPrice { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PokemonDescription { get; set;}
        public virtual  ICollection<Rental> Bookings { get; set; }
    }
}
