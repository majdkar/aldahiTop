using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Requests.Deliveries.DeliveryOrders
{
    public class GetAllPagedDeliveryOrdersRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public string Status { get; set; }

    }
}
