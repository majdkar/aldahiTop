using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Specifications.GeneralSettings
{
    public class NationFilterSpecification : HeroSpecification<Nation>
    {
        public NationFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.ArabicName.Contains(searchString) ||p.Code.Contains(searchString) ||p.PhoneCode.Contains(searchString) ||p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
