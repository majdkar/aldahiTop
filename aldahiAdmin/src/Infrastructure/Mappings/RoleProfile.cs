using AutoMapper;
using FirstCall.Infrastructure.Models.Identity;
using FirstCall.Application.Responses.Identity;

namespace FirstCall.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, BlazorHeroRole>().ReverseMap();
        }
    }
}