using FirstCall.Core.Entities;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.GeneralSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Domain.Entities.Products
{
    public class ProductCom : AuditableEntity<int>
    {
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public string? DescriptionAboutEn { get; set; }
        public string ProductComponentImageUrl { get; set; }
        public string ProductComponentImageUrl1 { get; set; }
        public string ProductComponentImageUrl2 { get; set; }
        public string ProductComponentImageUrl3 { get; set; }
        public string ProductComponentImageUrl4 { get; set; }
        public string ProductComponentImageUrl5 { get; set; }

        public int Order { get; set; } = 0;

    }
}
