using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Orders
{
    public class ShippingOrderStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Ordered", "Ordered" },
            {"Processing", "Processing" },
            {"Shipped", "Shipped" },
            {"Deliverd", "Deliverd" }
        };
    }

    public enum ShippingOrderStatusEnum
    {
        Ordered,
        Processing,
        Shipped,
        Deliverd,
    }
}
