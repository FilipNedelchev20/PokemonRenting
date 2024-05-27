using Microsoft.AspNetCore.Mvc;

namespace PokemonRenting.Web.Controllers
{
    public class PokemonsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }

}
