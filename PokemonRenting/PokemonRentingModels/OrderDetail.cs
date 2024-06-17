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
        [Key]
        public int Id { get; set; }

        public int OrderHeaderId { get; set; }
        public int PokemonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal DailyRate { get; set; }
        public int TotalDuration { get; set; }

        // Navigation properties
        public OrderHeader OrderHeader { get; set; }
        public Pokemon Pokemon { get; set; }
    }
}
