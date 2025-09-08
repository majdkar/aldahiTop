using DocumentFormat.OpenXml.Spreadsheet;
using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Entities.Products;

namespace FirstCall.Application.Specifications.Catalog
{
    public class ProductSearchAdvancedFilterSpecification : HeroSpecification<Product>
    {
        public ProductSearchAdvancedFilterSpecification(int SeasonId,int KindId,int GroupId,int WarehousesId,int ProductCategoryId,int FromQty,int ToQty,string Code, string ProductType)
        {


                Criteria = p =>  !p.IsDeleted &&

                                (SeasonId == 0 ? true : p.SeasonId == SeasonId) &&
                                (KindId == 0 ? true : p.KindId == KindId) &&
                                (GroupId == 0 ? true : p.GroupId == GroupId) &&
                                (ProductCategoryId == 0 ? true : p.ProductCategoryId == ProductCategoryId) &&
                                (WarehousesId == 0 ? true : p.WarehousesId == WarehousesId) &&
                                (Code == null ? true : p.Code == Code) &&
                                (FromQty == 0 ? p.Qty > 0 : p.Qty >= FromQty) &&
                                (ToQty == 0 ? p.Qty > 0 : p.Qty <= ToQty) &&
                                (p.Type == ProductType);
            
        }
    }
}