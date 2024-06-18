
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PokemonRenting.Models;
using PokemonRentingModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories
{
    public class PokemonContext:IdentityDbContext<ApplicationUser>
    {
        public PokemonContext(DbContextOptions<PokemonContext> options) : base(options)
        {
            
        }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get;  set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<UserDetail> UserDetails { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
    }
}
