using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Coupons
{
    public static class CouponsStatus
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"All", "All" },
            {"Used", "Used" },
             {"NotUsed", "NotUsed" }
        };
    }
    public enum CouponStatusEnum
    {
        All,
        Used,
        NotUsed
    }
}
