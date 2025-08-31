namespace FirstCall.Client.Infrastructure.Routes
{
    public static class NationsEndpoints
    {
        public static string ExportFiltered(string searchString)
        {
            return $"{Export}?searchString={searchString}";
        }

        public static string Export = "api/v1/nations/export";

        public static string GetAll = "api/v1/nations";
        public static string Delete = "api/v1/nations";
        public static string Save = "api/v1/nations";
        public static string GetCount = "api/v1/nations/count";
    }
}