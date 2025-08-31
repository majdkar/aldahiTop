using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Specifications.Catalog
{
    public class ProductByProductCategoryIdFilterSpecification : HeroSpecification<Product>
    {
        public ProductByProductCategoryIdFilterSpecification(string searchString,int ProductCategoryId,int subProductCategoryId)
        {


            if (!string.IsNullOrEmpty(searchString))
            {
                if (ProductCategoryId > 0) {
                    Criteria = p => !p.IsDeleted && p.ProductCategoryId == ProductCategoryId &&
                                    (p.NameAr.Contains(searchString) ||
                                    p.NameEn.Contains(searchString) ||
                                    p.Code.Contains(searchString) ||
                                    p.Price.ToString().Contains(searchString));
                } 
                if(subProductCategoryId > 0)
                {
                    Criteria = p => !p.IsDeleted && p.ProductCategoryId == subProductCategoryId &&
                                   (p.NameAr.Contains(searchString) ||
                                   p.NameEn.Contains(searchString) ||
                                   p.Code.Contains(searchString) ||
                                   p.Price.ToString().Contains(searchString));
                }
            
            }
            else
            {
                if (ProductCategoryId > 0)
                {
                    Criteria = p => !p.IsDeleted && p.ProductCategoryId == ProductCategoryId;
                }
                if (subProductCategoryId > 0)
                {
                    Criteria = p => !p.IsDeleted && p.ProductCategoryId == subProductCategoryId;

                }
            }
        }
    }
}