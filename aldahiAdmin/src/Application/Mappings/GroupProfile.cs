using AutoMapper;
using FirstCall.Application.Features.Groups.Commands.AddEdit;
using FirstCall.Application.Features.Groups.Queries.GetAll;
using FirstCall.Application.Features.Groups.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class GroupProfile : Profile
    {
        public GroupProfile()
        {
            CreateMap<AddEditGroupCommand, Group>().ReverseMap();
            CreateMap<GetGroupByIdResponse, Group>().ReverseMap();
            CreateMap<GetAllGroupsResponse, Group>().ReverseMap();
        }
    }
}
