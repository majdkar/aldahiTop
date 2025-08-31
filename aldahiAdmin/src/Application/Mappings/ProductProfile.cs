using AutoMapper;
using FirstCall.Application.Features.ProductComponents.Commands.AddEdit;
using FirstCall.Application.Features.Products.Commands.AddEdit;
using FirstCall.Application.Features.Products.Queries.GetAll;
using FirstCall.Application.Features.Products.Queries.GetById;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddEditCompanyProductCommand, Product>().ReverseMap();
            CreateMap<GetProductByIdResponse, Product>().ReverseMap();
            CreateMap<GetAllProductsResponse, Product>().ReverseMap();   
            
            CreateMap<AddEditCompanyProductComponentCommand, ProductCom>().ReverseMap();
            CreateMap<GetProductComponentByIdResponse, ProductCom>().ReverseMap();
            CreateMap<GetAllProductComponentsResponse, ProductCom>().ReverseMap(); 
        }
    }
}