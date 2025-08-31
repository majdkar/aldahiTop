using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Requests.Deliveries.DeliveryOrderProducts
{
    public class GetAllDeliveryOrderMixProductsRequest : PagedRequest
    {
        public string SearchString { get; set; }
        public int Mix { get; set; }
    }
}
