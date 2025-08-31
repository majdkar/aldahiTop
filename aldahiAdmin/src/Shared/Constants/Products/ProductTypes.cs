using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Products
{
    public static class ProductTypes
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"B2B", "B2B" },
            {"B2C", "B2C" }
        };
    }
    public enum ProductTypesEnum
    {
        B2B,
        B2C
    }
}