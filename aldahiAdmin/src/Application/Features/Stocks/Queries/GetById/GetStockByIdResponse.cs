using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Domain.Entities.Products;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Application.Features.Stocks.Queries.GetById
{
    public class GetStockByIdResponse
    {
        public int Id { get; set; }
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