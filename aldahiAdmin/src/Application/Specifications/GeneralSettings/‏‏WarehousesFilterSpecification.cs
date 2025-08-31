using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Specifications.GeneralSettings
{
    public class WarehousesFilterSpecification : HeroSpecification<Warehouses>
    {
        public WarehousesFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.NameAr.Contains(searchString) || p.NameEn.Contains(searchString)) && !p.IsDeleted;
            }
            else
            {
                Criteria = p => !p.IsDeleted;
            }
        }
    }
}
