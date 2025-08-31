using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Clients
{
    public static class LegalForms
    {
        public static Dictionary<string, string> Values = new Dictionary<string, string>
        {
            {"contributor", "contributor" },
            {"limited", "limited" },
            {"holding", "holding" }
        };
    }

    public enum LegalFormsEnum
    {
        contributor,   // Contributor مساهمة
        limited,   // Limited Liability ذات مسؤولية محدودة
        holding   // Holding قابضة
    }
}
