using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Specifications.Orders
{
    public class DeliveryOrderFilterSpecification : HeroSpecification<DeliveryOrder>
    {
        public DeliveryOrderFilterSpecification(string searchString)
        {
            Includes.Add(p => p.Client);
            IncludeStrings.Add("Client.Person");
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.OrderNumber.Contains(searchString))
                                && !p.IsDeleted ;
            }
            else
            {
                Criteria = p => !p.IsDeleted;
            }
        }
    }
}
