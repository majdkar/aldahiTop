
using System.Linq;


namespace FirstCall.Client.Infrastructure.Routes
{
    public class ProductCategoriesEndpoints
    {
        //public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        //{
        //    var url = $"api/v1/productCategories?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
        //    if (orderBy?.Any() == true)
        //    {
        //        foreach (var orderByPart in orderBy)
        //        {
        //            url += $"{orderByPart},";
        //        }
        //        url = url[..^1]; // loose training ,
        //    }
        //    return url;
        //}

        public static string GetAllPagedByType(string type, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/productCategories/GetAllPagedByType?type={type}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }
        public static string GetAllPagedSons(string type,int categoryId, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/productCategories/GetAllSons?type={type}&categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            if (orderBy?.Any() == true)
            {
                foreach (var orderByPart in orderBy)
                {
                    url += $"{orderByPart},";
                }
                url = url[..^1]; // loose training ,
            }
            return url;
        }
        public static string GetCount = "api/v1/productCategories/count";

        public static string GetProductCategoryImage(int productCategoryId)
        {
            return $"api/v1/productCategories/image/{productCategoryId}";
        }

        public static string GetAllByType(string type)
        {
            return $"api/v1/productCategories/GetAllByType?type={type}";
        }

        
          public static string GetAllByParentCategory(int categoryId)
        {
            return $"api/v1/productCategories/GetAllByParentCategory/{categoryId}";
        }

        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string GetAll = "api/v1/productCategories/GetAll";
        public static string Save = "api/v1/productCategories";
        public static string Delete = "api/v1/productCategories";
        public static string Export = "api/v1/productCategories/export";
        public static string ChangePassword = "api/identity/account/changepassword";
        public static string UpdateProfile = "api/identity/account/updateprofile";
    }
}
