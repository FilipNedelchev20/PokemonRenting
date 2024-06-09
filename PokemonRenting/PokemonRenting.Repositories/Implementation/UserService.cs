using Microsoft.EntityFrameworkCore;
using PokemonRenting.Models;
using PokemonRenting.Repositories.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokemonRenting.Repositories.Implementation
{
    public class UserService :IUserService
    {
        private PokemonContext _context;

        public UserService(PokemonContext context)
        {
            _context = context;
        }

        public ApplicationUser GetApplicationUser(string userId)
        {
           var applicationUser = _context.ApplicationUsers.Where(x=>x.Id == userId).FirstOrDefault();
            return applicationUser;
        }

        public async Task<IEnumerable<ApplicationUser>> GetApplicationUserAsync(string adminId)
        {
            var users = await _context.ApplicationUsers.Where(x=> x.Id != adminId).ToListAsync();
            return users;
        }

        public async Task AddUserDetail(string userid, UserDetail userDetail)
        {
            //var user = _context.ApplicationUsers.Where(x=> x.Id == userid).FirstOrDefault();
           await _context.UserDetails.AddAsync(userDetail);
            await _context.SaveChangesAsync();
        }
    }
}
