using AutoMapper;
using PokemonRenting.Web.Models.ViewModels.Pokemon;
using PokemonRentingModels;

namespace PokemonRenting.Web.Mapper
{
    public class PokemonProfile:Profile
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public PokemonProfile(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;



            CreateMap<Pokemon, PokemonViewModel>();
            CreateMap<CreatePokemonViewModel, Pokemon>()
                .ForMember(destination => destination.PokemonImage, 
                opt=> opt.MapFrom(source=> SaveImageFile(source.PokemonImageUrl)));
        }

        public PokemonProfile()
        {
            
        }

        private string SaveImageFile(IFormFile pokemonImageUrl)
        {
            if (pokemonImageUrl == null || pokemonImageUrl.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension();
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, fileName);
            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                File.CopyTo(fileStream);
            }
        }
    }
}
