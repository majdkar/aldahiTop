using System.Linq;

namespace FirstCall.Client.Infrastructure.Routes
{
    public static class ProductComponentsEndpoints
    {
        //ProductComponents
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/ProductComponents/GetAllPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
        
        public static string GetAllPagedByProductId(int pageNumber, int pageSize, string searchString, string[] orderBy,int ProductId)
        {
            var url = $"api/v1/ProductComponents/GetAllPagedByProductId?ProductId={ProductId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetAllPagedSearchProductComponent(int pageNumber, int pageSize, string searchString, string[] orderBy, string ProductComponentname, decimal fromprice, decimal toprice)
        {
            var url = $"api/v1/ProductComponents/GetAllPagedSearchProductComponent?ProductComponentname={ProductComponentname}&fromprice={fromprice}&toprice={toprice}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetProductComponentById(int id)
        {
            return $"api/v1/ProductComponents/{id}";
        }
        public static string SaveForCompanyProfile = "api/v1/ProductComponents/AddEditCompanyProductComponent";
        public static string DeleteProductComponent = "api/v1/ProductComponents";
        public static string GetAll = "api/v1/ProductComponents";
        public static string ExportFilteredByCompany(string searchString)
        {
            return $"api/v1/ProductComponents/export?searchString={searchString}";
        }


   
       

    }
}