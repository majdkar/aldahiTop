using AutoMapper;
using FirstCall.Application.Features.Nations.Commands.AddEdit;
using FirstCall.Application.Features.Nations.Queries.GetAll;
using FirstCall.Application.Features.Nations.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class NationProfile : Profile
    {
        public NationProfile()
        {
            CreateMap<AddEditNationCommand, Nation>().ReverseMap();
            CreateMap<GetNationByIdResponse, Nation>().ReverseMap();
            CreateMap<GetAllNationsResponse, Nation>().ReverseMap();
        }
    }
}