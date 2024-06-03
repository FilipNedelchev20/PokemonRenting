using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;

namespace PokemonRenting.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Rental> Bookings { get; set; }
    }
}
