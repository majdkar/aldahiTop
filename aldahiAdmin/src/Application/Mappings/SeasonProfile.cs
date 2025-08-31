using AutoMapper;
using FirstCall.Application.Features.Seasons.Commands.AddEdit;
using FirstCall.Application.Features.Seasons.Queries.GetAll;
using FirstCall.Application.Features.Seasons.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class SeasonProfile : Profile
    {
        public SeasonProfile()
        {
            CreateMap<AddEditSeasonCommand, Season>().ReverseMap();
            CreateMap<GetSeasonByIdResponse, Season>().ReverseMap();
            CreateMap<GetAllSeasonsResponse, Season>().ReverseMap();
        }
    }
}
