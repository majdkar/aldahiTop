using FirstCall.Application.Requests;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Features.OrderProducts.Queries.GetAllByOrder
{
    public class GetAllDeliveryOrderProductsResponse : PagedRequest
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int DeliveryOrderId { get; set; }
        public string DeliveryOrder { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
      


    }
}
