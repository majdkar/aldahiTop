using System.Collections.Generic;

namespace FirstCall.Shared.Constants.Orders
{
    public static class OrderDeliveryStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Immediately", "Immediately" },
            {"WithinaDay", "Within a Day" },
          

        };
    }

    public enum OrderDeliveryStatusEnum
    {
        Immediately,
        WithinaDay,
  
    }
}
