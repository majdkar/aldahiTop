using AutoMapper;
using FirstCall.Application.Features.Countries.Commands.AddEdit;
using FirstCall.Application.Features.Countries.Queries.GetAll;
using FirstCall.Application.Features.Countries.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class CountryProfile : Profile
    {
        public CountryProfile()
        {
            CreateMap<AddEditCountryCommand, Country>().ReverseMap();
            CreateMap<GetCountryByIdResponse, Country>().ReverseMap();
            CreateMap<GetAllCountriesResponse, Country>().ReverseMap();
        }
    }
}
