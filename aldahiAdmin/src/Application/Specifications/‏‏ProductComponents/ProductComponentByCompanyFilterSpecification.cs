using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Specifications.Catalog
{
    public class ProductComponentByCompanyFilterSpecification : HeroSpecification<ProductCom>
    {
        public ProductComponentByCompanyFilterSpecification(string searchString)
        {

            Includes.Add(p => p.Product);

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  !p.IsDeleted &&
                                (p.NameAr.Contains(searchString) ||
                                p.NameEn.Contains(searchString) ||
                                p.DescriptionAboutEn.Contains(searchString) ||
                                p.DescriptionAboutEn.Contains(searchString));
            }
            else
            {
                Criteria = p =>  !p.IsDeleted;
            }
        }
    }
}