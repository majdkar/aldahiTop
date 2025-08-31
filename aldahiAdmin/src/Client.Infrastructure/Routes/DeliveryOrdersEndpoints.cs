using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Client.Infrastructure.Routes
{
    public static class DeliveryOrdersEndpoints
    {

        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/deliveryOrders?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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


        public static string GetAllByStatus(int pageNumber, int pageSize, string searchString, string[] orderBy,string status)
        {
            var url = $"api/v1/deliveryOrders/GetByStatus?status={status}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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



        public static string GetAllPagedByClient(int pageNumber, int pageSize, string searchString, string[] orderBy,int clientId)
        {
            var url = $"api/v1/deliveryOrders/GetPagedByClient?clientId={clientId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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



        public static string GetAllPagedByProductId(int pageNumber, int pageSize, string searchString, string[] orderBy, int productId)
        {
            var url = $"api/v1/deliveryOrders/GetPagedByProductId?productId={productId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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

 
        public static string ExportFiltered(string searchString)
        {
            //00
            return $"{Export}?searchString={searchString}";
        }

        public static string GetById(int orderId)
        {
            return $"api/v1/deliveryOrders/{orderId}";  
        }

        public static string Export = "api/v1/deliveryOrders/export";

        public static string GetAll = "api/v1/deliveryOrders";
        public static string Delete = "api/v1/deliveryOrders";
        public static string Save = "api/v1/deliveryOrders";

        public static string Accept = "api/v1/deliveryOrders/accept";
        public static string Refuse = "api/v1/deliveryOrders/refuse";

        public static string AccceptOrderRequest = "api/v1/deliveryOrders/AccceptOrderRequest";

        public static string OrderCloseRequest = "api/v1/deliveryOrders/OrderCloseRequest";

    }
}
