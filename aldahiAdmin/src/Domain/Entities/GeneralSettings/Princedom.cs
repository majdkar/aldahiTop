using System;
using FirstCall.Domain.Contracts;

namespace FirstCall.Domain.Entities.GeneralSettings
{
    public class Princedom : AuditableEntity<int>
    {
		public string Name { get; set; }
        public string Description { get; set; }
		
		public string ar_title { get; set; }
		public string en_title { get; set; }
		public string Code { get; set; }

        //public decimal Tax { get; set; }
    }
}