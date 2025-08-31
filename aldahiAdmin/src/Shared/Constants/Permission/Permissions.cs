using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FirstCall.Shared.Constants.Permission
{
    public static class Permissions
    {
        public static class WebSiteManagement
        {
            public const string View = "Permissions.WebSiteManagement.View";
            public const string Create = "Permissions.WebSiteManagement.Create";
            public const string Edit = "Permissions.WebSiteManagement.Edit";
            public const string Delete = "Permissions.WebSiteManagement.Delete";
            public const string Export = "Permissions.WebSiteManagement.Export";
            public const string Search = "Permissions.WebSiteManagement.Search";
        }
        public static class Seasons
        {
            public const string View = "Permissions.Seasons.View";
            public const string Create = "Permissions.Seasons.Create";
            public const string Edit = "Permissions.Seasons.Edit";
            public const string Delete = "Permissions.Seasons.Delete";
            public const string Export = "Permissions.Seasons.Export";
            public const string Search = "Permissions.Seasons.Search";
        }  
        
        public static class Kinds
        {
            public const string View = "Permissions.Kinds.View";
            public const string Create = "Permissions.Kinds.Create";
            public const string Edit = "Permissions.Kinds.Edit";
            public const string Delete = "Permissions.Kinds.Delete";
            public const string Export = "Permissions.Kinds.Export";
            public const string Search = "Permissions.Kinds.Search";
        }          
        public static class Groups
        {
            public const string View = "Permissions.Groups.View";
            public const string Create = "Permissions.Groups.Create";
            public const string Edit = "Permissions.Groups.Edit";
            public const string Delete = "Permissions.Groups.Delete";
            public const string Export = "Permissions.Groups.Export";
            public const string Search = "Permissions.Groups.Search";
        }     public static class Stocks
        {
            public const string View = "Permissions.Stocks.View";
            public const string Create = "Permissions.Stocks.Create";
            public const string Edit = "Permissions.Stocks.Edit";
            public const string Delete = "Permissions.Stocks.Delete";
            public const string Export = "Permissions.Stocks.Export";
            public const string Search = "Permissions.Stocks.Search";
        }        public static class Warehouses
        {
            public const string View = "Permissions.Warehouses.View";
            public const string Create = "Permissions.Warehouses.Create";
            public const string Edit = "Permissions.Warehouses.Edit";
            public const string Delete = "Permissions.Warehouses.Delete";
            public const string Export = "Permissions.Warehouses.Export";
            public const string Search = "Permissions.Warehouses.Search";
        }    
        
        public static class Products
        {
            public const string View = "Permissions.Products.View";
            public const string Create = "Permissions.Products.Create";
            public const string Edit = "Permissions.Products.Edit";
            public const string Delete = "Permissions.Products.Delete";
            public const string Export = "Permissions.Products.Export";
            public const string Search = "Permissions.Products.Search";
        }     public static class ProductComponents
        {
            public const string View = "Permissions.ProductComponents.View";
            public const string Create = "Permissions.ProductComponents.Create";
            public const string Edit = "Permissions.ProductComponents.Edit";
            public const string Delete = "Permissions.ProductComponents.Delete";
            public const string Export = "Permissions.ProductComponents.Export";
            public const string Search = "Permissions.ProductComponents.Search";
        }
        public static class Blocks
        {
            public const string View = "Permissions.Blocks.View";
            public const string Create = "Permissions.Blocks.Create";
            public const string Edit = "Permissions.Blocks.Edit";
            public const string Delete = "Permissions.Blocks.Delete";
            public const string Export = "Permissions.Blocks.Export";
            public const string Search = "Permissions.Blocks.Search";
        }
        //public static class Projects
        //{
        //    public const string View = "Permissions.Projects.View";
        //    public const string Create = "Permissions.Projects.Create";
        //    public const string Edit = "Permissions.Projects.Edit";
        //    public const string Delete = "Permissions.Projects.Delete";
        //    public const string Export = "Permissions.Projects.Export";
        //    public const string Search = "Permissions.Projects.Search";
        //}
        public static class BlockCategories
        {
            public const string View = "Permissions.BlockCategories.View";
            public const string Create = "Permissions.BlockCategories.Create";
            public const string Edit = "Permissions.BlockCategories.Edit";
            public const string Delete = "Permissions.BlockCategories.Delete";
            public const string Export = "Permissions.BlockCategories.Export";
            public const string Search = "Permissions.BlockCategories.Search";
        }

        public static class Events
        {
            public const string View = "Permissions.Events.View";
            public const string Create = "Permissions.Events.Create";
            public const string Edit = "Permissions.Events.Edit";
            public const string Delete = "Permissions.Events.Delete";
            public const string Export = "Permissions.Events.Export";
            public const string Search = "Permissions.Events.Search";
        }
        public static class EventCategories
        {
            public const string View = "Permissions.EventCategories.View";
            public const string Create = "Permissions.EventCategories.Create";
            public const string Edit = "Permissions.EventCategories.Edit";
            public const string Delete = "Permissions.EventCategories.Delete";
            public const string Export = "Permissions.EventCategories.Export";
            public const string Search = "Permissions.EventCategories.Search";
        }

        public static class Menues
        {
            public const string View = "Permissions.Menues.View";
            public const string Create = "Permissions.Menues.Create";
            public const string Edit = "Permissions.Menues.Edit";
            public const string Delete = "Permissions.Menues.Delete";
            public const string Export = "Permissions.Menues.Export";
            public const string Search = "Permissions.Menues.Search";
        }
        public static class MenuCategories
        {
            public const string View = "Permissions.MenuCategories.View";
            public const string Create = "Permissions.MenuCategories.Create";
            public const string Edit = "Permissions.MenuCategories.Edit";
            public const string Delete = "Permissions.MenuCategories.Delete";
            public const string Export = "Permissions.MenuCategories.Export";
            public const string Search = "Permissions.MenuCategories.Search";
        }
        public static class Pages
        {
            public const string View = "Permissions.Pages.View";
            public const string Create = "Permissions.Pages.Create";
            public const string Edit = "Permissions.Pages.Edit";
            public const string Delete = "Permissions.Pages.Delete";
            public const string Export = "Permissions.Pages.Export";
            public const string Search = "Permissions.Pages.Search";
        }
        //public static class Products
        //{
        //    public const string View = "Permissions.Products.View";
        //    public const string Create = "Permissions.Products.Create";
        //    public const string Edit = "Permissions.Products.Edit";
        //    public const string Delete = "Permissions.Products.Delete";
        //    public const string Export = "Permissions.Products.Export";
        //    public const string Search = "Permissions.Products.Search";
        //}

        //public static class ProductCategories
        //{
        //    public const string View = "Permissions.ProductCategories.View";
        //    public const string Create = "Permissions.ProductCategories.Create";
        //    public const string Edit = "Permissions.ProductCategories.Edit";
        //    public const string Delete = "Permissions.ProductCategories.Delete";
        //    public const string Export = "Permissions.ProductCategories.Export";
        //    public const string Search = "Permissions.ProductCategories.Search";
        //}

        /*s0003s*/


        //public static class Sexs
        //{
        //    public const string View = "Permissions.Sexs.View";
        //    public const string Create = "Permissions.Sexs.Create";
        //    public const string Edit = "Permissions.Sexs.Edit";
        //    public const string Delete = "Permissions.Sexs.Delete";
        //    public const string Export = "Permissions.Sexs.Export";
        //    public const string Search = "Permissions.Sexs.Search";
        //}

        public static class Countries
        {
            public const string View = "Permissions.Countries.View";
            public const string Create = "Permissions.Countries.Create";
            public const string Edit = "Permissions.Countries.Edit";
            public const string Delete = "Permissions.Countries.Delete";
            public const string Export = "Permissions.Countries.Export";
            public const string Search = "Permissions.Countries.Search";
        }
        //public static class RequestStatuss
        //{
        //    public const string View = "Permissions.RequestStatuss.View";
        //    public const string Create = "Permissions.RequestStatuss.Create";
        //    public const string Edit = "Permissions.RequestStatuss.Edit";
        //    public const string Delete = "Permissions.RequestStatuss.Delete";
        //    public const string Export = "Permissions.RequestStatuss.Export";
        //    public const string Search = "Permissions.RequestStatuss.Search";
        //}
        //public static class Passports
        //{
        //    public const string View = "Permissions.Passports.View";
        //    public const string Create = "Permissions.Passports.Create";
        //    public const string Edit = "Permissions.Passports.Edit";
        //    public const string Delete = "Permissions.Passports.Delete";
        //    public const string Export = "Permissions.Passports.Export";
        //    public const string Search = "Permissions.Passports.Search";
        //}
        //public static class Owners
        //{
        //    public const string View = "Permissions.Owners.View";
        //    public const string Create = "Permissions.Owners.Create";
        //    public const string Edit = "Permissions.Owners.Edit";
        //    public const string Delete = "Permissions.Owners.Delete";
        //    public const string Export = "Permissions.Owners.Export";
        //    public const string Search = "Permissions.Owners.Search";
        //}
        public static class Princedoms
        {
            public const string View = "Permissions.Princedoms.View";
            public const string Create = "Permissions.Princedoms.Create";
            public const string Edit = "Permissions.Princedoms.Edit";
            public const string Delete = "Permissions.Princedoms.Delete";
            public const string Export = "Permissions.Princedoms.Export";
            public const string Search = "Permissions.Princedoms.Search";
        }
 
     

        //public static class ProviderServices
        //{
        //    public const string View = "Permissions.ProviderServices.View";
        //    public const string Create = "Permissions.ProviderServices.Create";
        //    public const string Edit = "Permissions.ProviderServices.Edit";
        //    public const string Delete = "Permissions.ProviderServices.Delete";
        //    public const string Export = "Permissions.ProviderServices.Export";
        //    public const string Search = "Permissions.ProviderServices.Search";
        //}
        public static class Nations
        {
            public const string View = "Permissions.Nations.View";
            public const string Create = "Permissions.Nations.Create";
            public const string Edit = "Permissions.Nations.Edit";
            public const string Delete = "Permissions.Nations.Delete";
            public const string Export = "Permissions.Nations.Export";
            public const string Search = "Permissions.Nations.Search";
        }
        //public static class BonusPoints
        //{
        //    public const string View = "Permissions.BonusPoints.View";
        //    public const string Create = "Permissions.BonusPoints.Create";
        //    public const string Edit = "Permissions.BonusPoints.Edit";
        //    public const string Delete = "Permissions.BonusPoints.Delete";
        //    public const string Export = "Permissions.BonusPoints.Export";
        //    public const string Search = "Permissions.BonusPoints.Search";
        //}
        //public static class Suggestions
        //{
        //    public const string View = "Permissions.Suggestions.View";
        //    public const string Create = "Permissions.Suggestions.Create";
        //    public const string Edit = "Permissions.Suggestions.Edit";
        //    public const string Delete = "Permissions.Suggestions.Delete";
        //    public const string Export = "Permissions.Suggestions.Export";
        //    public const string Search = "Permissions.Suggestions.Search";
        //}

        //public static class Cars
        //{
        //    public const string View = "Permissions.Cars.View";
        //    public const string Create = "Permissions.Cars.Create";
        //    public const string Edit = "Permissions.Cars.Edit";
        //    public const string Delete = "Permissions.Cars.Delete";
        //    public const string Export = "Permissions.Cars.Export";
        //    public const string Search = "Permissions.Cars.Search";
        //}

        //public static class Coupons
        //{
        //    public const string View = "Permissions.Coupons.View";
        //    public const string Create = "Permissions.Coupons.Create";
        //    public const string Edit = "Permissions.Coupons.Edit";
        //    public const string Delete = "Permissions.Coupons.Delete";
        //    public const string Export = "Permissions.Coupons.Export";
        //    public const string Search = "Permissions.Coupons.Search";
        //}



        //public static class AgeCategorytbls
        //{
        //    public const string View = "Permissions.AgeCategorytbls.View";
        //    public const string Create = "Permissions.AgeCategorytbls.Create";
        //    public const string Edit = "Permissions.AgeCategorytbls.Edit";
        //    public const string Delete = "Permissions.AgeCategorytbls.Delete";
        //    public const string Export = "Permissions.AgeCategorytbls.Export";
        //    public const string Search = "Permissions.AgeCategorytbls.Search";
        //}

        public static class Documents
        {
            public const string View = "Permissions.Documents.View";
            public const string Create = "Permissions.Documents.Create";
            public const string Edit = "Permissions.Documents.Edit";
            public const string Delete = "Permissions.Documents.Delete";
            public const string Search = "Permissions.Documents.Search";
        }

        public static class DocumentTypes
        {
            public const string View = "Permissions.DocumentTypes.View";
            public const string Create = "Permissions.DocumentTypes.Create";
            public const string Edit = "Permissions.DocumentTypes.Edit";
            public const string Delete = "Permissions.DocumentTypes.Delete";
            public const string Export = "Permissions.DocumentTypes.Export";
            public const string Search = "Permissions.DocumentTypes.Search";
        }

        public static class DocumentExtendedAttributes
        {
            public const string View = "Permissions.DocumentExtendedAttributes.View";
            public const string Create = "Permissions.DocumentExtendedAttributes.Create";
            public const string Edit = "Permissions.DocumentExtendedAttributes.Edit";
            public const string Delete = "Permissions.DocumentExtendedAttributes.Delete";
            public const string Export = "Permissions.DocumentExtendedAttributes.Export";
            public const string Search = "Permissions.DocumentExtendedAttributes.Search";
        }

        public static class Users
        {
            public const string View = "Permissions.Users.View";
            public const string Create = "Permissions.Users.Create";
            public const string Edit = "Permissions.Users.Edit";
            public const string Delete = "Permissions.Users.Delete";
            public const string Export = "Permissions.Users.Export";
            public const string Search = "Permissions.Users.Search";
        }

        public static class Roles
        {
            public const string View = "Permissions.Roles.View";
            public const string Create = "Permissions.Roles.Create";
            public const string Edit = "Permissions.Roles.Edit";
            public const string Delete = "Permissions.Roles.Delete";
            public const string Search = "Permissions.Roles.Search";
        }

        public static class RoleClaims
        {
            public const string View = "Permissions.RoleClaims.View";
            public const string Create = "Permissions.RoleClaims.Create";
            public const string Edit = "Permissions.RoleClaims.Edit";
            public const string Delete = "Permissions.RoleClaims.Delete";
            public const string Search = "Permissions.RoleClaims.Search";
        }

        public static class Persons
        {
            public const string View = "Permissions.Persons.View";
            public const string Create = "Permissions.Persons.Create";
            public const string Edit = "Permissions.Persons.Edit";
            public const string Delete = "Permissions.Persons.Delete";
            public const string Export = "Permissions.Persons.Export";
            public const string Search = "Permissions.Persons.Search";
        }
        //public static class Companies
        //{
        //    public const string View = "Permissions.Companies.View";
        //    public const string Create = "Permissions.Companies.Create";
        //    public const string Edit = "Permissions.Companies.Edit";
        //    public const string Delete = "Permissions.Companies.Delete";
        //    public const string Export = "Permissions.Companies.Export";
        //    public const string Search = "Permissions.Companies.Search";
        //}
        //public static class CompanyProfile
        ////{
        ////    public const string View = "Permissions.CompanyProfile.View";
        ////    public const string Create = "Permissions.CompanyProfile.Create";
        ////    public const string Edit = "Permissions.CompanyProfile.Edit";
        //    public const string Delete = "Permissions.CompanyProfile.Delete";
        //    public const string Export = "Permissions.CompanyProfile.Export";
        //    public const string Search = "Permissions.CompanyProfile.Search";
        //}
        public static class DeliveryOrders
        {
            public const string View = "Permissions.DeliveryOrders.View";
            public const string Create = "Permissions.DeliveryOrders.Create";
            public const string Edit = "Permissions.DeliveryOrders.Edit";
            public const string Delete = "Permissions.DeliveryOrders.Delete";
            public const string Export = "Permissions.DeliveryOrders.Export";
            public const string Search = "Permissions.DeliveryOrders.Search";
        }
        //public static class DeliveryOrderProducts
        //{
        //    public const string View = "Permissions.DeliveryOrderProducts.View";
        //    public const string Create = "Permissions.DeliveryOrderProducts.Create";
        //    public const string Edit = "Permissions.DeliveryOrderProducts.Edit";
        //    public const string Delete = "Permissions.DeliveryOrderProducts.Delete";
        //    public const string Export = "Permissions.DeliveryOrderProducts.Export";
        //    public const string Search = "Permissions.DeliveryOrderProducts.Search";
        //}

        //public static class DeliveryOrderTracking
        //{
        //    public const string View = "Permissions.DeliveryOrderTracking.View";
        //    public const string Create = "Permissions.DeliveryOrderTracking.Create";
        //    public const string Edit = "Permissions.DeliveryOrderTracking.Edit";
        //    public const string Delete = "Permissions.DeliveryOrderTracking.Delete";
        //    public const string Export = "Permissions.DeliveryOrderTracking.Export";
        //    public const string Search = "Permissions.DeliveryOrderTracking.Search";
        //}

        
        //public static class SupplierCompanies
        //{
        //    public const string View = "Permissions.SupplierCompanies.View";
        //    public const string Create = "Permissions.SupplierCompanies.Create";
        //    public const string Edit = "Permissions.SupplierCompanies.Edit";
        //    public const string Delete = "Permissions.SupplierCompanies.Delete";
        //    public const string Export = "Permissions.SupplierCompanies.Export";
        //    public const string Search = "Permissions.SupplierCompanies.Search";
        //}
        //public static class Drivers
        //{
        //    public const string View = "Permissions.Drivers.View";
        //    public const string Create = "Permissions.Drivers.Create";
        //    public const string Edit = "Permissions.Drivers.Edit";
        //    public const string Delete = "Permissions.Drivers.Delete";
        //    public const string Export = "Permissions.Drivers.Export";
        //    public const string Search = "Permissions.Drivers.Search";
        //}
        //public static class Notifications
        //{
        //    public const string View = "Permissions.Notifications.View";
        //    public const string Create = "Permissions.Notifications.Create";
        //    public const string Edit = "Permissions.Notifications.Edit";
        //    public const string Delete = "Permissions.Notifications.Delete";
        //    public const string Export = "Permissions.Notifications.Export";
        //    public const string Search = "Permissions.Notifications.Search";
        //}
        //public static class NotificationClients
        //{
        //    public const string View = "Permissions.NotificationClients.View";
        //    public const string Create = "Permissions.NotificationClients.Create";
        //    public const string Edit = "Permissions.NotificationClients.Edit";
        //    public const string Delete = "Permissions.NotificationClients.Delete";
        //    public const string Export = "Permissions.NotificationClients.Export";
        //    public const string Search = "Permissions.NotificationClients.Search";
        //}
        public static class Communication
        {
            public const string Chat = "Permissions.Communication.Chat";
        }

        public static class Preferences
        {
            public const string ChangeLanguage = "Permissions.Preferences.ChangeLanguage";

            //TODO - add permissions
        }

        public static class Dashboards
        {
            public const string View = "Permissions.Dashboards.View";
        }

        public static class Hangfire
        {
            public const string View = "Permissions.Hangfire.View";
        }

        public static class AuditTrails
        {
            public const string View = "Permissions.AuditTrails.View";
            public const string Export = "Permissions.AuditTrails.Export";
            public const string Search = "Permissions.AuditTrails.Search";
        }
        //public static class ActivityTypes
        //{
        //    public const string View = "Permissions.ActivityTypes.View";
        //    public const string Create = "Permissions.ActivityTypes.Create";
        //    public const string Edit = "Permissions.ActivityTypes.Edit";
        //    public const string Delete = "Permissions.ActivityTypes.Delete";
        //    public const string Export = "Permissions.ActivityTypes.Export";
        //    public const string Search = "Permissions.ActivityTypes.Search";
        //}

        public static class Brands
        {
            public const string View = "Permissions.Brands.View";
            public const string Create = "Permissions.Brands.Create";
            public const string Edit = "Permissions.Brands.Edit";
            public const string Delete = "Permissions.Brands.Delete";
            public const string Export = "Permissions.Brands.Export";
            public const string Search = "Permissions.Brands.Search";
        }
        //public static class ProductBrands
        //{
        //    public const string View = "Permissions.ProductBrands.View";
        //    public const string Create = "Permissions.ProductBrands.Create";
        //    public const string Edit = "Permissions.ProductBrands.Edit";
        //    public const string Delete = "Permissions.ProductBrands.Delete";
        //    public const string Export = "Permissions.ProductBrands.Export";
        //    public const string Search = "Permissions.ProductBrands.Search";
        //}
        //public static class CarCategories
        //{
        //    public const string View = "Permissions.CarCategories.View";
        //    public const string Create = "Permissions.CarCategories.Create";
        //    public const string Edit = "Permissions.CarCategories.Edit";
        //    public const string Delete = "Permissions.CarCategories.Delete";
        //    public const string Export = "Permissions.CarCategories.Export";
        //    public const string Search = "Permissions.CarCategories.Search";
        //}
        //public static class Carmodels
        //{
        //    public const string View = "Permissions.Carmodels.View";
        //    public const string Create = "Permissions.Carmodels.Create";
        //    public const string Edit = "Permissions.Carmodels.Edit";
        //    public const string Delete = "Permissions.Carmodels.Delete";
        //    public const string Export = "Permissions.Carmodels.Export";
        //    public const string Search = "Permissions.Carmodels.Search";
        //}
        public static class CodeCars
        {
            public const string View = "Permissions.CodeCars.View";
            public const string Create = "Permissions.CodeCars.Create";
            public const string Edit = "Permissions.CodeCars.Edit";
            public const string Delete = "Permissions.CodeCars.Delete";
            public const string Export = "Permissions.CodeCars.Export";
            public const string Search = "Permissions.CodeCars.Search";
        }

        //public static class ManufactureType
        //{
        //    public const string View = "Permissions.ManufactureType.View";
        //    public const string Create = "Permissions.ManufactureType.Create";
        //    public const string Edit = "Permissions.ManufactureType.Edit";
        //    public const string Delete = "Permissions.ManufactureType.Delete";
        //    public const string Export = "Permissions.ManufactureType.Export";
        //    public const string Search = "Permissions.ManufactureType.Search";
        //}
        /// <summary>
        /// Returns a list of Permissions.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetRegisteredPermissions()
        {
            var permssions = new List<string>();
            foreach (var prop in typeof(Permissions).GetNestedTypes().SelectMany(c => c.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)))
            {
                var propertyValue = prop.GetValue(null);
                if (propertyValue is not null)
                    permssions.Add(propertyValue.ToString());
            }
            return permssions;
        }
    }
}