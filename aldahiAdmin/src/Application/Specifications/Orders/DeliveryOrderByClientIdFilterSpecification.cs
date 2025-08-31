using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Specifications.Orders
{
    public class DeliveryOrderByClientIdFilterSpecification : HeroSpecification<DeliveryOrder>
    {
        public DeliveryOrderByClientIdFilterSpecification(string searchstring, int ClientId)
        {
            Includes.Add(p => p.Client);

            Criteria = p => p.ClientId == ClientId && !p.IsDeleted;// && p.IsConfirm == true ;
        }
    }
}
