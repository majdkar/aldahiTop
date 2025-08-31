using AutoMapper;
using FirstCall.Application.Features.Clients.Persons.Commands.AddEdit;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;

using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Infrastructure.Models.Identity;
using FirstCall.Shared.Constants.Role;

namespace FirstCall.Application.Mappings
{
    public class PersonProfile : Profile
    {
        public PersonProfile()
        {
            CreateMap<AddEditPersonCommand, Person>().ReverseMap();


            CreateMap<AddEditPersonCommand, BlazorHeroUser>()
                                 .ForMember(c => c.ClientType, opt => opt.MapFrom(src => RoleConstants.BasicRole));

            CreateMap<GetAllPersonsResponse, Person>().ReverseMap();
            CreateMap<GetAllPersonsResponse, Person>().ReverseMap();
        }
    }
}
