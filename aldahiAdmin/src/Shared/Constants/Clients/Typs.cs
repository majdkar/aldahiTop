using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Clients
{
    public static class Types
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Individual", "Individual" },
            {"Member", "Member" },
        };
    }

    public enum TypeEnum
    {
        Individual,
        Member,
    }
}
