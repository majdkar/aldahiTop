using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Specifications
{
    public class BrandFilterSpecification : HeroSpecification<Brand>
    {
        public BrandFilterSpecification(string searchString)
        {

            //IncludeStrings.Add("BrandCategories.CarCategory");

            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.NameEn.Contains(searchString) && !p.IsDeleted;
            }
            else
            {
                Criteria = p => true && !p.IsDeleted;
            }
        }
    }
}
