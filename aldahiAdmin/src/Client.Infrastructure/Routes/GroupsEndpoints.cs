using System.Linq;

namespace FirstCall.Client.Infrastructure.Routes
{
    public class GroupsEndpoints
    {
        public static string GetAllPaged(int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"api/v1/Groups/GetAllPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
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
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/Groups/export";

        public static string GetAll = "api/v1/Groups";
        public static string Delete = "api/v1/Groups";
        public static string Save = "api/v1/Groups";
        public static string GetCount = "api/v1/Groups/count";
    }
}
