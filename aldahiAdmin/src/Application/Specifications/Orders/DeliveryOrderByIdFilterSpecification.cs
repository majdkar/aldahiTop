using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Specifications.Orders
{
    public class DeliveryOrderByIdFilterSpecification : HeroSpecification<DeliveryOrder>
    {
        public DeliveryOrderByIdFilterSpecification(int orderId)
        {
            Includes.Add(p => p.Client);
            IncludeStrings.Add("Client.Person");

            Criteria = p => p.Id == orderId && !p.IsDeleted ;
        }
    }
}
