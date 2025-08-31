using FirstCall.Application.Specifications.Base;
using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Specifications.Catalog
{
    public class ProductCategoryFilterSpecification : HeroSpecification<ProductCategory>
    {
        public ProductCategoryFilterSpecification(string searchString)
        {
            
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p =>  (p.NameAr.Contains(searchString) || p.NameEn.Contains(searchString) || p.DescriptionAr.Contains(searchString) || p.DescriptionEn.Contains(searchString) || p.ParentCategory.NameEn.Contains(searchString) || p.ParentCategory.NameAr.Contains(searchString)) && !p.IsDeleted;
            }
            else
            {
                Criteria = p => !p.IsDeleted;
            }
        }
    }
}
