using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
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
    public class OrderDetail
    {
        public int Id { get; set; }
        [ForeignKey(nameof(OrderHeaderId))]
        [Required]
        public int OrderHeaderId { get; set; }
        [ValidateNever]
        public OrderHeader OrderHeader { get; set; }
        [ForeignKey(nameof(PokemonId))]
        [Required]
        public int PokemonId { get; set; }
        [ValidateNever]
        public Pokemon Pokemon { get; set; }
      
        public decimal RentalTotal { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
