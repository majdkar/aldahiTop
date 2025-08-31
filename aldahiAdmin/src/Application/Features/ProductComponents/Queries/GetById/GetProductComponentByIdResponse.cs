using FirstCall.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Domain.Entities;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Features.Products.Queries.GetById
{
    public class GetProductComponentByIdResponse
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
        public int Order { get; set; }

    }
}
