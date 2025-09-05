using FirstCall.Domain.Entities.Clients;
using FirstCall.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Domain.Entities.Orders;

namespace FirstCall.Application.Features.Orders.Queries.GetAll
{
    public class GetAllDeliveryOrdersResponse
    {

        public int Id { get; set; }


        public virtual Client Client { get; set; }


        public string OrderNumber { get; set; }

        public decimal TotalPrice { get; set; }

        public int ClientId { get; set; }

        public string ClientName{ get; set; }


        public string Type { get; set; }


        public string ImageBillLadingUrl { get; set; }





        public DateTime? OrderDate { get; set; }
        public string Status { get; set; }



        public virtual List<DeliveryOrderProduct> Products { get; set; }

    }
}
