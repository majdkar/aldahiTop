

using FirstCall.Core.Entities;

namespace FirstCall.Application.Features.ProductCategories.Queries.GetAllPaged
{
    public class GetAllPagedProductCategoriesResponse
    {
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; }
        public string DescriptionEn { get; set; }

        public int? ParentCategoryId { get; set; }
        public int Order { get; set; }
        public string? ImageDataURL { get; set; }
        public int SonsCount { get; set; }
        public string Type { get; set; }

        public ProductCategory ParentCategory { get; set; }

    }
}
