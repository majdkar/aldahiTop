using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Specifications.Products
{
    public class ProductComponentFilterSpecification : HeroSpecification<ProductCom>
    {
        public ProductComponentFilterSpecification()
        {
            Includes.Add(p => p.Product);
        }
    }
}
