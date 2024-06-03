using AutoMapper;
using PokemonRenting.Web.Models.ViewModels.Pokemon;
using PokemonRenting.Web.Utility;
using PokemonRentingModels;

namespace PokemonRenting.Web.Mapper
{
    public class PokemonProfile:Profile
    {
        private readonly IWebHostEnvironment _WebHostEnvironment;
        public PokemonProfile(IWebHostEnvironment webHostEnvironment)
        {

            _WebHostEnvironment = webHostEnvironment;
            CreateMap<Pokemon, PokemonViewModel>();
            CreateMap<CreatePokemonViewModel, Pokemon>()
                .ForMember(destination => destination.PokemonImage, 
                opt=> opt.MapFrom(source=> new ImageUpload(_WebHostEnvironment).SaveImageFile(source.PokemonImageUrl)));
        }

       
    }
}
