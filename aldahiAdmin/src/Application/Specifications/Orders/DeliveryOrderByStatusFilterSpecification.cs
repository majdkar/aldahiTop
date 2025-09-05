using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Specifications.Orders
{
    public class DeliveryOrderByStatusFilterSpecification : HeroSpecification<DeliveryOrder>
    {
        public DeliveryOrderByStatusFilterSpecification(string status,string searchString ,string type)
        {
            Includes.Add(p => p.Client);
            IncludeStrings.Add("Client.Person");
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.OrderNumber.Contains(searchString)) 
                               
                                && p.Status == status
                                 && !p.IsDeleted && p.Type == type;
            }
            else
            {
                Criteria = p => p.Status == status && p.Type == type &&!p.IsDeleted;//&& p.IsConfirm == true;
            }
        }
    }
}
