using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Requests.Deliveries.DeliveryOrders
{
    public class GetAllPagedDeliveryOrdersByDriverIdRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int DriverId { get; set; }

    }
}
