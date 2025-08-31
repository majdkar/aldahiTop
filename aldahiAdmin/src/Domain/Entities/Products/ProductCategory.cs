using FirstCall.Domain.Contracts;

namespace FirstCall.Core.Entities
{
    public class ProductCategory : AuditableEntity<int>
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionAr { get; set; } = default;
        public string DescriptionEn { get; set; } = default;

        public int? ParentCategoryId { get; set; }
        public virtual ProductCategory? ParentCategory { get; set; }
        public int Order { get; set; } = 0;
        public string? ImageDataURL { get; set; }

        public string Type { get; set; } = "B2C"; // check Shared.Constants.Products.ProductTypes (B2B, B2C)
    }
}
