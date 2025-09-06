using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Specifications.Orders
{
    public class DeliveryOrderByClientIdFilterSpecification : HeroSpecification<DeliveryOrder>
    {
        public DeliveryOrderByClientIdFilterSpecification(string searchstring, int ClientId,string Type,string status)
        {
            Includes.Add(p => p.Client);

            Criteria = p => p.ClientId == ClientId && p.Type == Type&& p.Status == status && !p.IsDeleted;// && p.IsConfirm == true ;
        }
    }
}
