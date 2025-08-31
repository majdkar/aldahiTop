using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Features.OrderProducts.Queries.GetById
{
    public class GetDeliveryOrderProductByIdResponse
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int DeliveryOrderId { get; set; }
        public string DeliveryOrder { get; set; }
        public int ProductId { get; set; }
    }
}
