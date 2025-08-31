using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Clients
{
    public static class ClientTypes
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Person", "Person" },
            {"Company", "Company" }           
        };
    }
    public enum ClientTypesEnum
    {
        Person,
        Company,
        Driver
    }
}
