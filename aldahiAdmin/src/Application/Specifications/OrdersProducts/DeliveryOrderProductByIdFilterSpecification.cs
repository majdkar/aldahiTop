using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Specifications.OrderProducts
{
    public class DeliveryOrderProductByIdFilterSpecification : HeroSpecification<DeliveryOrderProduct>
    {
        public DeliveryOrderProductByIdFilterSpecification(int orderProductId)
        {
            Includes.Add(p => p.DeliveryOrder);
            Includes.Add(p => p.Product);

            Criteria = p => p.Id == orderProductId && !p.IsDeleted;
        }
    }
}
