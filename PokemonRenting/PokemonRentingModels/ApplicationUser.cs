using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using static PokemonRenting.Models.Constants.EntityConstants;
using Microsoft.EntityFrameworkCore;

namespace PokemonRenting.Models
{
    public class ApplicationUser : IdentityUser
    {
        [StringLength(FullNameMaxLenght)]
        public string? FullName { get; set; }
        [StringLength(AddressMaxLenght)]
        public string? Address { get; set; }
        public virtual ICollection<Rental> Bookings { get; set; }
    }
}
