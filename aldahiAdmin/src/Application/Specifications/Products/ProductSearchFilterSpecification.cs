using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Specifications.Catalog
{
    public class ProductSearchFilterSpecification : HeroSpecification<Product>
    {
        public ProductSearchFilterSpecification(string searchString, string productname, decimal fromprice, decimal toprice)
        {


            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  !p.IsDeleted &&
                                 (productname == null ? p.NameEn.Length > 0 : p.NameEn.Contains(productname)) &&
                                (fromprice == 0 ? p.Price > 0 : p.Price >= fromprice) &&
                                (toprice == 0 ? p.Price > 0 : p.Price <= toprice) &&
                                (p.NameAr.Contains(searchString) &&
                                p.NameEn.Contains(searchString) ||
                                p.Code.Contains(searchString));
            }
            else
            {
                Criteria = p =>  !p.IsDeleted;
            }
        }
    }
}