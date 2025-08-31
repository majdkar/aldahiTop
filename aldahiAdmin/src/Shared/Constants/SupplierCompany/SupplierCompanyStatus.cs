using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.SupplierCompany
{
    public class SupplierCompanyStatus
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

    public enum SupplierCompanyStatusEnum
    {
        Accepted,
        Completed,
        Canceled,
        Closed,
        Pending,
    }

}
