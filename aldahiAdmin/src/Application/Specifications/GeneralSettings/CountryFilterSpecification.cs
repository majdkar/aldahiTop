using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Specifications.GeneralSettings
{
    public class CountryFilterSpecification : HeroSpecification<Country>
    {
        public CountryFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.NameAr.Contains(searchString) || p.NameEn.Contains(searchString) || p.Code.Contains(searchString)) && !p.IsDeleted;
            }
            else
            {
                Criteria = p => !p.IsDeleted;
            }
        }
    }
}
