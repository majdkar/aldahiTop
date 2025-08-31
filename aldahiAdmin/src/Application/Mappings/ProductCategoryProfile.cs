using AutoMapper;
using FirstCall.Application.Features.ProductCategories.Commands.AddEdit;
using FirstCall.Application.Features.ProductCategories.Queries.GetAll;
using FirstCall.Core.Entities;


namespace FirstCall.Application.Mappings
{
    public class ProductCategoryProfile:Profile
    {
        public ProductCategoryProfile()
        {
            CreateMap<AddEditProductCategoryCommand, ProductCategory>().ReverseMap();
            CreateMap<GetAllProductCategoriesResponse, ProductCategory>().ReverseMap();
            CreateMap<GetAllParentCategoriesByTypeResponse, ProductCategory>().ReverseMap();
            
        }
    }
}
