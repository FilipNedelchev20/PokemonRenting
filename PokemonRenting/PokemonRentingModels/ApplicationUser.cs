﻿using System;
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
        
        public string Username { get; set; }
        public string Address { get; set; }
        public bool IsAdmin { get; set; }
    }
}
