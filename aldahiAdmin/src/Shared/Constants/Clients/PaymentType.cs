using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Clients
{
    public static class PaymentTypes
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"Cash", "نقدي" },
            {"Check", "شيك" },
            {"Bank transformation", "تحويل بنكي" },
        };
    }

    public enum PaymentTypeEnum
    {
        Cash,
        Check,
        transformation,
    }
}
