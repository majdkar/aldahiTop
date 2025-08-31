using System;
using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Specifications.GeneralSettings
{
    public class PrincedomFilterSpecification : HeroSpecification<Princedom>
    {
        public PrincedomFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.ar_title.Contains(searchString) ||p.en_title.Contains(searchString) ||p.Code.Contains(searchString) ||p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
