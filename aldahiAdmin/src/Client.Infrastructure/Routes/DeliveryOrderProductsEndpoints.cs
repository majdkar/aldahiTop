using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Routes
{
    public static class DeliveryOrderProductsEndpoints
    {

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy, int deliveryorder)
        {
            var url = $"api/v1/deliveryorderproducts?deliveryorder={deliveryorder}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

        public static string GetDeliveryOrderProductById(int productId)
        {
            return $"api/v1/deliveryOrderproducts/{productId}";
        }
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }
        public static string GetAll = "api/v1/deliveryorderproducts";
        public static string Save = "api/v1/deliveryorderproducts";
        public static string Delete = "api/v1/deliveryorderproducts";
        public static string Export = "api/v1/deliveryorderproducts/export";


        public static string GetCount = "api/v1/deliveryorderproducts/count";

       
    }
}
