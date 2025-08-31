using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Routes
{
    public static class CountriesEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/Countries/export";

        public static string GetAll = "api/v1/Countries";
        public static string Delete = "api/v1/Countries";
        public static string Save = "api/v1/Countries";
        public static string GetCount = "api/v1/Countries/count";
    }
}
