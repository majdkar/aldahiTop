using AutoMapper;
using FirstCall.Application.Features.Kinds.Commands.AddEdit;
using FirstCall.Application.Features.Kinds.Queries.GetAll;
using FirstCall.Application.Features.Kinds.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class KindProfile : Profile
    {
        public KindProfile()
        {
            CreateMap<AddEditKindCommand, Kind>().ReverseMap();
            CreateMap<GetKindByIdResponse, Kind>().ReverseMap();
            CreateMap<GetAllKindsResponse, Kind>().ReverseMap();
        }
    }
}
