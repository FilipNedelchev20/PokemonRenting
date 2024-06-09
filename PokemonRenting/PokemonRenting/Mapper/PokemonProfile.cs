using AutoMapper;
using PokemonRenting.Models;
using PokemonRenting.Web.Models.ViewModels.ApplicationUserViewModels;
using PokemonRenting.Web.Models.ViewModels.Pokemon;
using PokemonRenting.Web.Utility;
using PokemonRentingModels;
using Stripe.Climate;

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
                opt=> opt.MapFrom(source=> new ImageUpload(_webHostEnvironment).SaveImageFile(source.PokemonImageUrl)));

            CreateMap<Pokemon, EditPokemonViewModel>()
                .ForMember(destination=> destination.PokemonImageUrl, opt=> opt.Ignore());
            CreateMap<SummaryViewModel, Order>();
          
            CreateMap<Pokemon, PokemonDetailsViewModel>()
               .ForMember(destination => destination.StartDate, opt => opt.Ignore())
               .ForMember(destination => destination.ReturnDate, opt => opt.Ignore());

            CreateMap<EditPokemonViewModel, Pokemon>()
                .ForMember(destination => destination.PokemonImage,
                opt => opt.MapFrom(source => new ImageUpload(_webHostEnvironment).SaveImageFile(source.PokemonImageUrl)));
            CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(destination => destination.UserId, opt=>opt.MapFrom(source => source.Id));
        }

       
    }
}
