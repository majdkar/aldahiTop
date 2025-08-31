using System.Collections.Generic;

namespace FirstCall.Shared.Constants.Orders
{
    public static class OrderStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Accepted", "Accepted" },
            {"Completed", "Completed" },
            {"Canceled", "Canceled" },
            {"Closed", "Closed" },
            {"Pending", "Pending" },

        };
    }

    public enum OrderStatusEnum
    {
        Accepted,
        Completed,
        Canceled,
        Closed,
        Pending,
    }
}
