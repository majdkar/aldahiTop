using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Core.Entities
{
    public class Currency
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Country Country { get; set; }
        public int CountryId { get; set; }

        public string Symbol { get; set; }

        public decimal USDollarRatio { get; set; }

        public bool IsActive { get; set; }
    }
}
