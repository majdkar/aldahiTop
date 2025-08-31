using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Clients
{
    public static class ClientStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Pending", "Pending" },
            {"Accepted", "Accepted" },
            {"Refused", "Refused" }
        };
    }
    public enum ClientStatusEnum
    {
        Pending,
        Accepted,
        Refused
    }
}
