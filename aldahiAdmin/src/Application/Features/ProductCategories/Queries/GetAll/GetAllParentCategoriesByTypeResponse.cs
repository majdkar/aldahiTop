
using FirstCall.Core.Entities;

namespace FirstCall.Application.Features.ProductCategories.Queries.GetAll
{
    public class GetAllParentCategoriesByTypeResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? ImageDataURL { get; set; }
        public string Type { get; set; }
        
    }
}
