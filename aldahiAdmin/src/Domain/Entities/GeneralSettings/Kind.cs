using FirstCall.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Domain.Entities.GeneralSettings
{
    public class Kind : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }
}
