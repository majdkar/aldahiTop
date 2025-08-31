using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.GeneralSettings;
using System.Collections.Generic;

namespace FirstCall.Application.Features.Brands.Queries.GetById
{
    public class GetBrandByIdResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Tax { get; set; }
        public string NameEn { get; set; }
   
        public string ImageDataURL { get; set; }


    }
}