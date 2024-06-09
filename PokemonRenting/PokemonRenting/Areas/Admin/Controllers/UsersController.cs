using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonRenting.Repositories.Infrastructure;
using PokemonRenting.Repositories.Utility;
using PokemonRenting.Web.Models.ViewModels.ApplicationUserViewModels;
using System.Security.Claims;

namespace PokemonRenting.Web.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize(Roles = FD.Admin_Role)]
        public async Task<IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var users = await _userService.GetApplicationUserAsync(claim.Value);   
            var userViewModel = _mapper.Map<List<UserViewModel>>(users);
            return View(userViewModel);
        }
    }
}
