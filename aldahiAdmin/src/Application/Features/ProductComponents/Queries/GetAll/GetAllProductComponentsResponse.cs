using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Domain.Entities.Products;
using System.Collections.Generic;
using FirstCall.Domain.Entities;

namespace FirstCall.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductComponentsResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }


        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public string? DescriptionAboutEn { get; set; }


        public string ProductComponentImageUrl { get; set; }

        public string ProductComponentImageUrl1 { get; set; }
        public string ProductComponentImageUrl2 { get; set; }
        public string ProductComponentImageUrl3 { get; set; }
        public string ProductComponentImageUrl4 { get; set; }
        public string ProductComponentImageUrl5 { get; set; }
    }
}
