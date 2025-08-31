using System;
namespace FirstCall.Client.Infrastructure.Routes
{
    public static class PrincedomsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/princedoms/export";

        public static string GetAll = "api/v1/princedoms";
        public static string GetById = "api/v1/princedoms";
        public static string Delete = "api/v1/princedoms";
        public static string Save = "api/v1/princedoms";
        public static string GetCount = "api/v1/princedoms/count";
    }
}