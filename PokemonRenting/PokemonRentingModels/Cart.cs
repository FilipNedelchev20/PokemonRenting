using PokemonRentingModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey(nameof(PokemonId))]
        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }
        
        public decimal TotalAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int TotalDuration { get; set; }
        public ApplicationUser User { get; set; }   
    }
}
