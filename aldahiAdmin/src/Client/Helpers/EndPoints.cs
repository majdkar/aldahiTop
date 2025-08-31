using System.Linq;

namespace FirstCall.Client.Helpers
{
    public static class EndPoints
    {
        public static string GetAllPaged(string mainRoute, int pageNumber, int pageSize, string searchString, string[] orderBy)
        {
            var url = $"{mainRoute}?pageNumber={pageNumber}&pageSize={pageSize}";
            if (searchString != "" && orderBy != null)
            {
                url = $"{mainRoute}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            }
            else if (searchString == "" && orderBy != null)
            {
                url = $"{mainRoute}?pageNumber={pageNumber}&pageSize={pageSize}&orderBy=";
            }
            else if (searchString != "" && orderBy == null)
            {
                url = $"{mainRoute}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
                return url;
            }
            else
            {
                url = $"{mainRoute}?pageNumber={pageNumber}&pageSize={pageSize}";
            }
            if(orderBy!=null)
            {
                //var url = $"{mainRoute}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
                if (orderBy?.Any() == true)
                {
                    foreach (var orderByPart in orderBy)
                    {
                        url += $"{orderByPart},";
                    }
                    url = url[..^1];
                }


            }
            return url;

        }
        public static string GetAllPagedByCategoryID(string mainRoute, int pageNumber, int pageSize, string searchString, string[] orderBy, int categoryId)
        {
            var url = $"{mainRoute}?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}";
            if (searchString != "" && orderBy != null)
            {
                url = $"{mainRoute}?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
            }
            else if (searchString == "" && orderBy != null)
            {
                url = $"{mainRoute}?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}&orderBy=";
            }
            else if (searchString != "" && orderBy == null)
            {
                url = $"{mainRoute}?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}";
                return url;
            }
            else
            {
                url = $"{mainRoute}?categoryId={categoryId}&pageNumber={pageNumber}&pageSize={pageSize}";
            }
            if (orderBy != null)
            {
                //var url = $"{mainRoute}?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchString}&orderBy=";
                if (orderBy?.Any() == true)
                {
                    foreach (var orderByPart in orderBy)
                    {
                        url += $"{orderByPart},";
                    }
                    url = url[..^1];
                }


            }
            return url;

        }
        public static string GetAll(string mainRoute, string searchString, string[] orderBy) //without paging
        {
            var url = $"{mainRoute}";
            if(searchString!="" && orderBy != null)
            {
                url = $"{mainRoute}?searchString={searchString}&orderBy=";
            }
            else if (searchString == "" && orderBy != null)
            {
                url = $"{mainRoute}?orderBy=";
            }
            else if (searchString != "" && orderBy == null)
            {
                url = $"{mainRoute}?searchString={searchString}";
                return url;
            }
            else
            {
                url = $"{mainRoute}";
            }

            if(orderBy != null)
            {
                if (orderBy?.Any() == true)
                {
                    foreach (var orderByPart in orderBy)
                    {
                        url += $"{orderByPart},";
                    }
                    url = url[..^1];
                }
            }

            return url;
        }


        public static string BlockCategories = "api/blockcategories";
        public static string BlockCategoriesSelect = "api/blockcategories/all";
        public static string BlockCategoriesTranslation = "api/BlockCategoryTranslation";
        public static string Blocks = "api/blocks";
        public static string BlocksTranslation = "api/BlockTranslation";
        public static string BlocksPhoto = "api/BlockPhoto";
        public static string BlocksAttachement = "api/BlockAttachement";
        public static string BlocksVideo = "api/BlockVideo";


        public static string ProductCategories = "api/productcategories";
        public static string ProductCategoriesSelect = "api/productcategories/all";
        public static string ProductCategoriesTranslation = "api/ProductCategoryTranslation";
        public static string Products = "api/products";
        public static string ProductInstantPricing = "api/ProductInstantPricing";

        public static string ProductsTranslation = "api/ProductTranslation";
        public static string ProductsPhoto = "api/ProductPhoto";
        public static string ProductVideo = "api/ProductVideo";
        public static string ProductOption = "api/ProductOption";
        public static string ProductOffer = "api/ProductOffer";
        public static string ProductRating = "api/ProductRating";
        public static string ProductAverageRating = "api/ProductRating/AverageRating";
        public static string AllOffers = "api/ProductOffer/all";
        public static string ProductsAttachement = "api/ProductAttachement";
        public static string ProductSubCategories = "api/productcategories/subcategories";
        public static string ProductsNewCollectionBuCategory = "api/products/newcollection";
        public static string ProductsNewCollection = "api/products/newcollection/all";
        public static string ProductCategoriesMain = "api/productcategories/main";
        public static string ProductsByCategoryAndSubCategories = "api/products/allproducts";
        public static string ProductsBySupplierCompanyId = "api/products/allproductsBySupplierCompanyId";


        public static string MenuCategories = "api/menucategories";
        public static string MenuCategoriesSelect = "api/menucategories/all";
        public static string MenuCategoriesTranslation = "api/MenuCategoryTranslation";
        
        public static string Menus = "api/v1/menus";
        public static string MenusMaster = "api/v1/menus/GetMenuMaster";
        public static string MenuSub = "api/v1/menus/GetMenuSub";
        public static string MenusNoCategory = "api/v1/menus/NoCategory";
        public static string MenuSelect = "api/v1/menus/all";
        public static string MenusTranslation = "api/v1/MenuTranslation";
        public static string GetAllMasterOrSubmenu = "api/v1/GetAllMasterOrSubmenu";
        public static string GetAllWithChildern = "api/v1/GetAllWithChildern";

        public static string Pages = "api/pages";
        public static string PagesTransaltion = "api/PageTranslation";
        public static string PagesPhoto = "api/PagePhoto";
        public static string PagesAttachement = "api/PageAttachement";

        public static string EventCategories = "api/eventcategories";
        public static string EventCategoriesSelect = "api/eventcategories/all";
        public static string EventCategoriesTranslation = "api/eventCategoryTranslation";
        public static string Events = "api/events";
        public static string EventsTranslation = "api/EventTranslation";
        public static string EventsPhoto = "api/EventPhoto";
        public static string EventsAttachement = "api/EventAttachement";

        public static string ArticleCategories = "api/ArticleCategories";
        public static string ArticleCategoriesSelect = "api/articlecategories/all";
        public static string Articles = "api/Articles";
        public static string ArticlesSelect = "api/Articles/all";

        public static string FileUpload = "api/fileupload";

        public static string ResetPassword = "api/resetpassword";

        public static string Statistics = "api/statistics";

        public static string Languages = "api/languages";
        public static string Countries = "api/Countries";
        public static string Currencies = "api/Currencies";
        public static string Units = "api/Units";
    }
}
