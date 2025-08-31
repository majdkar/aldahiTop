using FirstCall.Application.Specifications.Base;
using FirstCall.Core.Entities;


namespace FirstCall.Application.Specifications.Products
{
    public class ProductCategoryByTypeFilterSpecification : HeroSpecification<ProductCategory>
    {
        public ProductCategoryByTypeFilterSpecification(string type, string searchString)
        {

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.NameAr.Contains(searchString) || p.NameEn.Contains(searchString) || p.DescriptionAr.Contains(searchString) || p.DescriptionEn.Contains(searchString) || p.ParentCategory.NameEn.Contains(searchString) || p.ParentCategory.NameAr.Contains(searchString)) && p.Type == type && !p.IsDeleted;
            }
            else
            {
                Criteria = p =>  p.Type == type && !p.IsDeleted;
            }
        }
    }
}
