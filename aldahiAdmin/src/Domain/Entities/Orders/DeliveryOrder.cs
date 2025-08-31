using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Domain.Entities.Orders
{
    public class DeliveryOrder : AuditableEntity<int>
    {
        public string OrderNumber { get; set; }

        public decimal TotalPrice { get; set; }

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        public DateTime? OrderDate { get; set; }

        public string Status { get; set; }

        public virtual List<DeliveryOrderProduct> Products { get; set; }

        public string ImageBillLadingUrl { get; set; }
    }
}
