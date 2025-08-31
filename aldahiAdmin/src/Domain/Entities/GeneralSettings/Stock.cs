using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Domain.Entities.GeneralSettings
{
    public class Stock : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public int Quantity { get; set; }


        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }


        [ForeignKey("Warehouses")]
        public int WarehousesId { get; set; }
        public virtual Warehouses Warehouses { get; set; }


        
    }
}
