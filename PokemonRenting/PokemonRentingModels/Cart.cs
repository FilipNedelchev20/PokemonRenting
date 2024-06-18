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
        public int Quantity { get; set; } = 1;
        public decimal TotalAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int TotalDuration { get; set; }
      
        public Guid UserId { get; set; }
      
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
