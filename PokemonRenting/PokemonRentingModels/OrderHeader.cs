using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Models
{
    public class OrderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime DateOfOrder { get; set; }
        public DateTime DateOfRent { get; set; }
        [Column(TypeName ="money")]
        public decimal OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public DateTime DateOfPayment { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string FullName { get; set; }
    }
}
