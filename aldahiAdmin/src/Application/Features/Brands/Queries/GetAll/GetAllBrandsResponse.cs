using FirstCall.Core.Entities;
using FirstCall.Domain.Entities.GeneralSettings;
using System.Collections.Generic;

namespace FirstCall.Application.Features.Brands.Queries.GetAll
{
    public class GetAllBrandsResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public decimal Tax { get; set; }
        public string ImageDataURL { get; set; }

    }
}