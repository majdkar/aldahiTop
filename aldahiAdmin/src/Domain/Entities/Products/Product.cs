using FirstCall.Core.Entities;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.GeneralSettings;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace FirstCall.Domain.Entities.Products
{
    public class Product : AuditableEntity<int>
    {
    
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? Code { get; set; }
        public string? PackageNumber { get; set; }
        public string? Colors { get; set; }
        public string? Sizes { get; set; }
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public int Order { get; set; } = 0;
        public string ProductImageUrl { get; set; }
        public string ProductImageUrl2 { get; set; }
        public string ProductImageUrl3 { get; set; }
        public string ProductImageUrl4 { get; set; }


        [ForeignKey("Season")]
        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }


        [ForeignKey("ProductCategory")]
        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }




        [ForeignKey("Kind")]
        public int KindId { get; set; }
        public virtual Kind Kind { get; set; }

        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }


        [ForeignKey("Warehouses")]
        public int? WarehousesId { get; set; }
        public virtual Warehouses Warehouses { get; set; }
      
    }
}
