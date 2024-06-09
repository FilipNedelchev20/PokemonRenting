using PokemonRenting.Models.Enums;
using PokemonRentingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Models
{
    public class Rental
    {
        [Key]
        public int Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        [Required]
        [Column(TypeName="money")]
        public double TotalPrice { get; set; }
        public bool IsPaid { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public RentalStatus RentalStatus { get; set; }
        [ForeignKey(nameof(PokemonId))]
        public int PokemonId { get; set; }
        public virtual Pokemon Pokemon { get; set; }
        [ForeignKey(nameof(ApplicationUserId))]
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
