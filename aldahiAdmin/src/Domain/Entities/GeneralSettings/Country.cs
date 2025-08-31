using FirstCall.Domain.Contracts;

namespace FirstCall.Domain.Entities.GeneralSettings
{
    public class Country : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string Code { get; set; }
        public string PhoneCode { get; set; }
    }
}
