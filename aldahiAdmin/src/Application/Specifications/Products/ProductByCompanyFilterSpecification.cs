using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Specifications.Catalog
{
    public class ProductByCompanyFilterSpecification : HeroSpecification<Product>
    {
        public ProductByCompanyFilterSpecification(string searchString ,string productType)
        {
            Includes.Add(p => p.ProductCategory);
            Includes.Add(p => p.Season);
            Includes.Add(p => p.Kind);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  !p.IsDeleted && p.Type == productType &&
                                (p.NameAr.Contains(searchString) ||
                                p.NameEn.Contains(searchString) ||
                                p.Code.Contains(searchString)||
                                p.Price.ToString().Contains(searchString));
            }
            else
            {
                Criteria = p =>  !p.IsDeleted && p.Type == productType;
            }
        }
    }
}