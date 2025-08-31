using System;
using AutoMapper;
using FirstCall.Application.Features.Princedoms.Commands.AddEdit;
using FirstCall.Application.Features.Princedoms.Queries.GetAll;
using FirstCall.Application.Features.Princedoms.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class PrincedomProfile : Profile
    {
        public PrincedomProfile()
        {
            CreateMap<AddEditPrincedomCommand, Princedom>().ReverseMap();
            CreateMap<GetPrincedomByIdResponse, Princedom>().ReverseMap();
            CreateMap<GetAllPrincedomsResponse, Princedom>().ReverseMap();
        }
    }
}