using FirstCall.Application.Specifications.Base;
using FirstCall.Core.Entities;


namespace FirstCall.Application.Specifications.GeneralSettings
{
    public class ProductCategorySonsFilterSpecification : HeroSpecification<ProductCategory>
    {
        public ProductCategorySonsFilterSpecification(string type,string searchString, int categoryId = 0)
        {
            Includes.Add(p => p.ParentCategory);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => (p.NameAr.Contains(searchString)
                || p.NameEn.Contains(searchString)
                || p.DescriptionAr.Contains(searchString)
                || p.DescriptionEn.Contains(searchString)
                || p.ParentCategory.NameEn.Contains(searchString)
                || p.ParentCategory.NameAr.Contains(searchString)) && !p.IsDeleted && p.Type == type &&
                ((categoryId == 0 ? p.ParentCategoryId == null : p.ParentCategoryId == categoryId));
                //|| ((categoryId == 0 ? (p.ParentCategoryId > 0 || p.ParentCategoryId == null) : p.ParentCategory.ParentCategoryId == categoryId))
                //|| ((categoryId == 0 ? (p.ParentCategoryId > 0 || p.ParentCategoryId == null) : p.ParentCategory.ParentCategory.ParentCategoryId == categoryId)));
              
            }
            else
            {
                Criteria = p => !p.IsDeleted && p.Type == type &&
                ((categoryId == 0 ? p.ParentCategoryId == null : p.ParentCategoryId == categoryId));
                //|| ((categoryId == 0 ? (p.ParentCategoryId > 0 || p.ParentCategoryId == null) : p.ParentCategory.ParentCategoryId == categoryId))
                //|| ((categoryId == 0 ? (p.ParentCategoryId > 0 || p.ParentCategoryId == null) : p.ParentCategory.ParentCategory.ParentCategoryId == categoryId)));

            }
        }
    }
}
