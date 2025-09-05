using FirstCall.Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using FirstCall.Domain.Entities;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Application.Features.Products.Queries.GetAllPaged
{
    public class GetAllPagedProductsResponse
    {

        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? Code { get; set; }
        public int? WarehousesId { get; set; }
        public virtual Warehouses Warehouses { get; set; }
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
        public string Type { get; set; }

        public int SeasonId { get; set; }
        public virtual Season Season { get; set; }

        public int ProductCategoryId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }


        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        public int KindId { get; set; }
        public virtual Kind Kind { get; set; }
        public string KindNameAr  { get; set; }
        public string KindNameEn  { get; set; }
    }
}