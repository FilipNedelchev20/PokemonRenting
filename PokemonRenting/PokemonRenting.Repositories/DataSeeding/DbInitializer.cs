using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PokemonRenting.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.DataSeeding
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private PokemonContext _pokeContext;

        public DbInitializer(UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            PokemonContext pokeContext)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _pokeContext = pokeContext;
        }

        public void Initialize()
        {
            try
            {
                if (_pokeContext.Database.GetPendingMigrations().Count()> 0)
                {
                    _pokeContext.Database.Migrate();
                }
            }
            catch (Exception)
            {
                throw;
            }
            if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole("Admin"))
                    .GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole("Customer"))
                    .GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                 
                }, "Admin@123").GetAwaiter().GetResult();
                ApplicationUser user = _pokeContext.ApplicationUsers.FirstOrDefault(x=> x.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user,"Admin").GetAwaiter().GetResult() ;
            }
            return;
        }
    }
}
