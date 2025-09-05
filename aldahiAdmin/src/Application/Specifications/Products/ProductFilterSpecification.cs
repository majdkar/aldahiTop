using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;
using FirstCall.Shared.Constants.Products;

namespace FirstCall.Application.Specifications.Products
{
    public class ProductFilterSpecification : HeroSpecification<Product>
    {
        public ProductFilterSpecification(string ProductType)
        {
            Criteria = p => !p.IsDeleted && p.Type == ProductType;

        }
    }
}
