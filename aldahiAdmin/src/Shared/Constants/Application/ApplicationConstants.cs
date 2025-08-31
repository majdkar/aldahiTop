using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Shared.Constants.Application
{
    public static class ApplicationConstants
    {
        public static class SignalR
        {
            public const string HubUrl = "/signalRHub";
            public const string SendUpdateDashboard = "UpdateDashboardAsync";
            public const string ReceiveUpdateDashboard = "UpdateDashboard";
            public const string SendRegenerateTokens = "RegenerateTokensAsync";
            public const string ReceiveRegenerateTokens = "RegenerateTokens";
            public const string ReceiveChatNotification = "ReceiveChatNotification";
            public const string SendChatNotification = "ChatNotificationAsync";
            public const string ReceiveMessage = "ReceiveMessage";
            public const string SendMessage = "SendMessageAsync";

            public const string OnConnect = "OnConnectAsync";
            public const string ConnectUser = "ConnectUser";
            public const string OnDisconnect = "OnDisconnectAsync";
            public const string DisconnectUser = "DisconnectUser";
            public const string OnChangeRolePermissions = "OnChangeRolePermissions";
            public const string LogoutUsersByRole = "LogoutUsersByRole";
        }
        public static class Cache
        {
            public const string GetAllSeasonsCacheKey = "all-Seasons";
            public const string GetAllGroupsCacheKey = "all-Groups";
            public const string GetAllWarehousessCacheKey = "all-Warehouses";
            public const string GetAllStocksCacheKey = "all-Stocks";
            public const string GetAllKindsCacheKey = "all-Kinds";
            public const string GetAllProductCategoriesCacheKey = "all-ProductCategories";
            public const string GetAllProductsCacheKey = "all-Products";
            public const string GetAllProductComponentsCacheKey = "all-ProductComponents";
            public const string GetAllCodeCarsCacheKey = "all-CodeCars";
            public const string GetAllProjectsCacheKey = "all-Projects";
            public const string GetAllBrandsCacheKey = "all-brands";
            public const string GetAllProductBrandsCacheKey = "all-productbrands";
            public const string GetAllCarmodelsCacheKey = "all-Carmodels";
            public const string GetAllDocumentTypesCacheKey = "all-document-types";
            public const string GetAllAgeCategorytblsCacheKey = "all-AgeCategorytbls";

            public const string GetAllClientsCacheKey = "all-Clients";
            public const string GetAllPersonsCacheKey = "all-Persons";
            public const string GetAllCompaniesCacheKey = "all-Companies";
            public const string GetAllActivityTypesCacheKey = "all-ActivityTypes";
            public const string GetAllActivitiesCacheKey = "all-Activities";
            public const string GetAllCouponsCacheKey = "all-Coupons";


            /*s0002s*/
            public const string GetAllCountriesCacheKey = "all-countries";
            public const string GetAllPlayersCacheKey = "all-Players";
            public const string GetAllPlayerClassificationsCacheKey = "all-PlayerClassifications";
            public const string GetAllSexsCacheKey = "all-Sexs";
            public const string GetAllAcademyRequestsCacheKey = "all-AcademyRequests";
            public const string GetAllAcademysCacheKey = "all-Academys";
            public const string GetAllClubRequestsCacheKey = "all-ClubRequests";
            public const string GetAllRequestStatussCacheKey = "all-RequestStatuss";
            public const string GetAllPassportsCacheKey = "all-Passports";
            public const string GetAllOwnersCacheKey = "all-Owners";
            public const string GetAllPrincedomsCacheKey = "all-Princedoms";
            public const string GetAllClubTypesCacheKey = "all-ClubTypes";
            public const string GetAllClubsCacheKey = "all-Clubs";
            public const string GetAllChampionshipsClassificationsCacheKey = "all-ChampionshipsClassifications";
            public const string GetAllNationsCacheKey = "all-Nations";
            public const string GetAllBonusPointsCacheKey = "all-BonusPoints";
            public const string GetAllProviderServicesCacheKey = "all-ProviderServices";

            public const string GetAllDeliveryOrdersCacheKey = "all-DeliveryOrders";
            public const string GetAllDeliveryOrderTrackingsCacheKey = "all-DeliveryOrdersTrackings";

            

            public const string GetAllCarCategoriesCacheKey = "all-CarCategories";
            public const string GetAllCarsCacheKey = "all-Cars";
            public const string GetAllCarImagesCacheKey = "all-CarImages";
            public const string GetAllCarPropertiesCacheKey = "all-CarProperties";
            public const string GetAllCarOffersCacheKey = "all-CarOffers";
            public const string GetAllSupplierCompaniesCacheKey = "all-SupplierCompanies";
            public const string GetAllDriversCacheKey = "all-Drivers";
            public const string GetAllManufactureTypeCacheKey = "all-ManufactureType";
            public const string GetAllNotificationsCacheKey = "all-Notifications";
            public const string GetAllNotificationClientsCacheKey = "all-NotificationClients";
            public const string GetAllSuggestionsCacheKey = "all-Suggestions";




            public static string GetAllEntityExtendedAttributesCacheKey(string entityFullName)
            {
                return $"all-{entityFullName}-extended-attributes";
            }

            public static string GetAllEntityExtendedAttributesByEntityIdCacheKey<TEntityId>(string entityFullName, TEntityId entityId)
            {
                return $"all-{entityFullName}-extended-attributes-{entityId}";
            }
        }
        public static class InfoConstants
        {
            public const string Saved = nameof(Saved);
            public const string Updated = nameof(Updated);
            public const string Deleted = nameof(Deleted);
            public const string NoInformations = nameof(NoInformations);
            public const string EmailIsExists = "This email already exists";
        }
        public static class MimeTypes
        {
            public const string OpenXml = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            public const string OpenPdf = "application/pdf";
        }
    }
}
