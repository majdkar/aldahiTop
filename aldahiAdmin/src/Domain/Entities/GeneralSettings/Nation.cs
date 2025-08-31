using FirstCall.Domain.Contracts;

namespace FirstCall.Domain.Entities.GeneralSettings
{
    public class Nation : AuditableEntity<int>
    {
		public string Name { get; set; }
        public string Description { get; set; }
		
		public string ArabicName { get; set; }
		public string Code { get; set; }
		public string PhoneCode { get; set; }
        public int action { get; set; }
        public float Shipping_Price { get; set; }
        public string country_code { get; set; }
        //public decimal Tax { get; set; }
    }
}