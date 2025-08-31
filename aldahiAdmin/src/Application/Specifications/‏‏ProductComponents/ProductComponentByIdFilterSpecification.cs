using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Specifications.Products
{
    public class ProductComponentByIdFilterSpecification : HeroSpecification<ProductCom>
    {
        public ProductComponentByIdFilterSpecification(int id)
        {

            Includes.Add(p => p.Product);

            Criteria = x => x.Id == id;
        }
    }
}
