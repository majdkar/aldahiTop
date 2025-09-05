using FirstCall.Shared.Constants.Products;
using System.Linq;

namespace FirstCall.Client.Infrastructure.Routes
{
    public static class ProductsEndpoints
    {
        //products
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy,string ProductType)
        {
            var url = $"api/v1/products/GetAllPaged?ProductType={ProductType}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetAllPagedSearchProduct(int pageNumber, int pageSize, string searchString, string[] orderBy, string productname, decimal fromprice, decimal toprice,string ProductType)
        {
            var url = $"api/v1/products/GetAllPagedSearchProduct?ProductType={ProductType}&productname={productname}&fromprice={fromprice}&toprice={toprice}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetProductById(int id,string ProductType)
        {
            return $"api/v1/Products/{id}/{ProductType}";
        }
        public static string SaveForCompanyProfile = "api/v1/products/AddEditCompanyProduct";
        public static string DeleteProduct = "api/v1/products";

        public static string GetAll(string ProductType)
        { 
          return  $"api/v1/products/{ProductType}";
        }
     
        
        public static string ExportFilteredByCompany(string searchString)
        {
            return $"api/v1/products/export?searchString={searchString}";
        }


   
       

    }
}