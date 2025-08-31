using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Specifications.OrderProducts
{
    public class DeliveryOrderProductFilterSpecification : HeroSpecification<DeliveryOrderProduct>
    {
        public DeliveryOrderProductFilterSpecification(string searchString)
        {
            Includes.Add(p => p.DeliveryOrder);
            Includes.Add(p => p.Product);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.DeliveryOrder.OrderNumber.Contains(searchString) ||
                p.Product.NameAr.Contains(searchString)||
                p.Product.NameEn.Contains(searchString))
                                && !p.IsDeleted;
            }
            else
            {
                Criteria = p => !p.IsDeleted; ;
            }
        }
    }
}
