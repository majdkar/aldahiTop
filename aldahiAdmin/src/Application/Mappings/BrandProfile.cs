using AutoMapper;
using FirstCall.Application.Features.Brands.Commands.AddEdit;
using FirstCall.Application.Features.Brands.Queries.GetAll;
using FirstCall.Application.Features.Brands.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Mappings
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<AddEditBrandCommand, Brand>().ReverseMap();
            CreateMap<GetBrandByIdResponse, Brand>().ReverseMap();
            CreateMap<GetAllBrandsResponse, Brand>().ReverseMap();
        }
    }
}