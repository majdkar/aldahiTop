using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Domain.Entities.Orders
{
    public class DeliveryOrderProduct : AuditableEntity<int>
    {

        public string OrderNumber { get; set; }


        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }


        [JsonIgnore]
        [ForeignKey("DeliveryOrder")]
        public int DeliveryOrderId { get; set; }
        public virtual DeliveryOrder DeliveryOrder { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

    }
}
