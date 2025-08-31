using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.GeneralSettings;
using FirstCall.Domain.Entities.Products;
using System.Collections.Generic;
using FirstCall.Domain.Entities;

namespace FirstCall.Application.Features.Products.Queries.GetAll
{
    public class GetAllProductsResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? Code { get; set; }
    }
}
