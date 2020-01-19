using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace HealthyChefWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();
            var cores = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cores);


            #region Login

            config.Routes.MapHttpRoute(
            name: "Login",
            routeTemplate: "api/Login",
            defaults: new { controller = "Account", action = "Login" }
            );

            #endregion

            #region  ListManagement 

            config.Routes.MapHttpRoute(
            name: "GetPlans",
            routeTemplate: "api/GetPlans",
            defaults: new { controller = "ListManagement", action = "GetPlans" }
            );

            config.Routes.MapHttpRoute(
            name: "GetCustomerPrefs",
            routeTemplate: "api/GetCustomerPrefs",
            defaults: new { controller = "ListManagement", action = "GetCustomerPrefs" }
            );

            config.Routes.MapHttpRoute(
            name: "GetMealPrefs",
            routeTemplate: "api/GetMealPrefs",
            defaults: new { controller = "ListManagement", action = "GetMealPrefs" }
            );

            config.Routes.MapHttpRoute(
            name: "GetPrograms",
            routeTemplate: "api/GetPrograms",
            defaults: new { controller = "ListManagement", action = "GetPrograms" }
            );

            config.Routes.MapHttpRoute(
            name: "GetAllAllergens",
            routeTemplate: "api/GetAllAllergens",
            defaults: new { controller = "ListManagement", action = "GetAllAllergens" }
            );

            config.Routes.MapHttpRoute(
            name: "GetAllCoupons",
            routeTemplate: "api/GetAllCoupons",
            defaults: new { controller = "ListManagement", action = "GetAllCoupons" }
            );

            config.Routes.MapHttpRoute(
            name: "GetAllIngredient",
            routeTemplate: "api/GetAllIngredient",
            defaults: new { controller = "ListManagement", action = "GetAllIngredient" }
            );

            config.Routes.MapHttpRoute(
             name: "GetAllMessageboxSizes",
             routeTemplate: "api/GetAllMessageboxSizes",
             defaults: new { controller = "ListManagement", action = "GetAllMessageboxSizes" }
             );

            config.Routes.MapHttpRoute(
            name: "GetAllShippingZone",
            routeTemplate: "api/GetAllShippingZone",
            defaults: new { controller = "ListManagement", action = "GetAllShippingZone" }
            );

            config.Routes.MapHttpRoute(
            name: "GetItems",
            routeTemplate: "api/GetItems",
            defaults: new { controller = "ListManagement", action = "GetItems" }
            );

            #endregion


            config.Routes.MapHttpRoute(
            name: "GetPastCalender",
            routeTemplate: "api/GetPastCalender/{startDate}/{endDate}",
            defaults: new { controller = "ProductionManagement", action = "GetPastCalender", startDate = RouteParameter.Optional, endDate = RouteParameter.Optional }
            );


            config.Routes.MapHttpRoute(
            name: "GetFutureCalender",
            routeTemplate: "api/GetFutureCalender/{startDate}/{endDate}",
            defaults: new { controller = "ProductionManagement", action = "GetFutureCalender", startDate = RouteParameter.Optional, endDate = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
            name: "GetMenus",
            routeTemplate: "api/GetMenus",
            defaults: new { controller = "ProductionManagement", action = "GetMenus" }
            );

            #region IssuedGiftCertificates
            config.Routes.MapHttpRoute(
            name: "GetImported",
            routeTemplate: "api/GetImported",
            defaults: new { controller = "ProductionManagement", action = "GetImported" }
            );

            config.Routes.MapHttpRoute(
            name: "GetRedeemdedCerts",
            routeTemplate: "api/GetRedeemdedCerts/{startDate}/{endDate}",
            defaults: new { controller = "ProductionManagement", action = "GetRedeemdedCerts", startDate = RouteParameter.Optional, endDate = RouteParameter.Optional }
            );

            // config.Routes.MapHttpRoute(
            //  name: "GetIssuedCerts",
            //  routeTemplate: "api/GetIssuedCerts",
            //  defaults: new { controller = "ProductionManagement", action = "GetIssuedCerts" }
            //);

            config.Routes.MapHttpRoute(
             name: "GetIssuedCerts",
             routeTemplate: "api/GetIssuedCerts/{startDate}/{endDate}",
             defaults: new { controller = "ProductionManagement", action = "GetIssuedCerts", startDate = RouteParameter.Optional, endDate = RouteParameter.Optional }
           );

            #endregion 

            #region Cancellationorder
            config.Routes.MapHttpRoute(
              name: "GetCancellation",
              routeTemplate: "api/GetCancellation/{PurchaseNumber}",
              defaults: new { controller = "OrderManagement", action = "GetCancellation" }
            );
            #endregion

            #region RecurringOrder
            config.Routes.MapHttpRoute(
               name: "GetRecurringOrder",
               routeTemplate: "api/GetRecurringOrder",
               defaults: new { controller = "OrderManagement", action = "GetRecurringOrder" }
             );
            #endregion

            #region OrderFullFillMent region
            config.Routes.MapHttpRoute(
                name: "GetOrderFullfillment",
                routeTemplate: "api/GetOrderFullfillment",
                defaults: new { controller = "OrderManagement", action = "GetOrderFullfillment"}
              );
            #endregion

            #region Order Management region
            config.Routes.MapHttpRoute(
                name: "GetAllPurchases",
                routeTemplate: "api/GetAllPurchases",
                defaults: new { controller = "OrderManagement", action = "GetAllPurchases" }
              );

            #endregion

            #region User Accounts region

            config.Routes.MapHttpRoute(
                name: "GetUserAccounts",
                routeTemplate: "api/GetUserAccounts",
                defaults: new { controller = "UserAccounts", action = "GetUserAccounts" }
              );

            config.Routes.MapHttpRoute(
              name: "GetUserAccountsByRole",
              routeTemplate: "api/GetUserAccountsByRole/{roleid}",
              defaults: new { controller = "UserAccounts", action = "GetUserAccountsByRole", roleid = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
             name: "SearchGetUserAccounts",
             routeTemplate: "api/SearchGetUserAccounts",
             defaults: new { controller = "UserAccounts", action = "SearchGetUserAccounts" }
           );
            #endregion

            //Posting
            #region Posting List Data

            config.Routes.MapHttpRoute(
            name: "AddOrUpdateAllergen",
            routeTemplate: "api/AddOrUpdateAllergen",
            defaults: new { controller = "ListManagement", action = "AddOrUpdateAllergen" }
            );

            config.Routes.MapHttpRoute(
            name: "AddOrUpdateCoupon",
            routeTemplate: "api/AddOrUpdateCoupon",
            defaults: new { controller = "ListManagement", action = "AddOrUpdateCoupon" }
            );

            config.Routes.MapHttpRoute(
            name: "AddOrUpdateShippingZone",
            routeTemplate: "api/AddOrUpdateShippingZone",
            defaults: new { controller = "ListManagement", action = "AddOrUpdateShippingZone" }
            );

            config.Routes.MapHttpRoute(
            name: "DeleteShippingZone",
            routeTemplate: "api/DeleteShippingZone/{shippingzoneid}",
            defaults: new { controller = "ListManagement", action = "DeleteShippingZone" }
            );

           config.Routes.MapHttpRoute(
           name: "AddOrUpdateIngredient",
           routeTemplate: "api/AddOrUpdateIngredient",
           defaults: new { controller = "ListManagement", action = "AddOrUpdateIngredient" }
           );

           config.Routes.MapHttpRoute(
           name: "AddOrUpdateItem",
           routeTemplate: "api/AddOrUpdateItem",
           defaults: new { controller = "ListManagement", action = "AddOrUpdateItem" }
           );

           config.Routes.MapHttpRoute(
           name: "AddOrUpdatePlan",
           routeTemplate: "api/AddOrUpdatePlan",
           defaults: new { controller = "ListManagement", action = "AddOrUpdatePlan" }
           );

           config.Routes.MapHttpRoute(
           name: "AddOrUpdatePreference",
           routeTemplate: "api/AddOrUpdatePreference",
           defaults: new { controller = "ListManagement", action = "AddOrUpdatePreference" }
           );
            
           config.Routes.MapHttpRoute(
           name: "AddOrUpdateProgram",
           routeTemplate: "api/AddOrUpdateProgram",
           defaults: new { controller = "ListManagement", action = "AddOrUpdateProgram" }
           );

            #endregion

            #region Production Management Posting 

            config.Routes.MapHttpRoute(
            name: "AddorUpdateProductionCalendar",
            routeTemplate: "api/AddorUpdateProductionCalendar",
            defaults: new { controller = "ProductionManagement", action = "AddorUpdateProductionCalendar" }
            );

            config.Routes.MapHttpRoute(
           name: "UpdateGiftCertificates",
           routeTemplate: "api/UpdateGiftCertificates",
           defaults: new { controller = "ProductionManagement", action = "UpdateGiftCertificates" }
           );
            config.Routes.MapHttpRoute(
           name: "UpdateImportedGiftCertificate",
           routeTemplate: "api/UpdateImportedGiftCertificate",
           defaults: new { controller = "ProductionManagement", action = "UpdateImportedGiftCertificate" }
           );

            config.Routes.MapHttpRoute(
           name: "AddorUpdateMenu",
           routeTemplate: "api/AddorUpdateMenu",
           defaults: new { controller = "ProductionManagement", action = "AddorUpdateMenu" }
           );

            #endregion

            #region OrderManagement Posting

            config.Routes.MapHttpRoute(
            name: "PurchaseDetails",
            routeTemplate: "api/PurchaseDetails/{Cartid}",
            defaults: new { controller = "OrderManagement", action = "PurchaseDetails" }
            );

            config.Routes.MapHttpRoute(
            name: "CancelAutorenew",
            routeTemplate: "api/CancelAutorenew/{cartitemid}/{cartid}",
            defaults: new { controller = "OrderManagement", action = "CancelAutorenew" }
            );

            config.Routes.MapHttpRoute(
            name: "CancelItemDetails",
            routeTemplate: "api/CancelItemDetails/{cartitemid}/{iscancel}",
            defaults: new { controller = "OrderManagement", action = "CancelItemDetails" }
            );

            config.Routes.MapHttpRoute(
            name: "CartItemsDetails",
            routeTemplate: "api/CartItemsDetails/{OrderNumber}",
            defaults: new { controller = "OrderManagement", action = "CartItemsDetails" }
            );

            config.Routes.MapHttpRoute(
             name: "CancelCartItems",
             routeTemplate: "api/CancelCartItems/{PurchaseNumber}/{OrderNumber}/{iscancel}",
             defaults: new { controller = "OrderManagement", action = "CancelCartItems" }
           );

            config.Routes.MapHttpRoute(
              name: "PostponeOrderFullfilment",
              routeTemplate: "api/PostponeOrderFullfilment/{cartItemId}/{delDate}",
              defaults: new { controller = "OrderManagement", action = "PostponeOrderFullfilment" }
            );

            #endregion

            #region User Accounts Posting

            config.Routes.MapHttpRoute(
            name: "UpdateStatusOfCustomer",
            routeTemplate: "api/UpdateStatusOfCustomer",
            defaults: new { controller = "UserAccounts", action = "UpdateStatusOfCustomer" }
            );

            config.Routes.MapHttpRoute(
            name: "UpdateBasicInfo",
            routeTemplate: "api/UpdateBasicInfo",
            defaults: new { controller = "UserAccounts", action = "UpdateBasicInfo" }
            );

            config.Routes.MapHttpRoute(
            name: "UpdateShippingAddress",
            routeTemplate: "api/UpdateShippingAddress",
            defaults: new { controller = "UserAccounts", action = "UpdateShippingAddress" }
            );

            config.Routes.MapHttpRoute(
            name: "UpdateBillingInfo",
            routeTemplate: "api/UpdateBillingInfo",
            defaults: new { controller = "UserAccounts", action = "UpdateBillingInfo" }
            );

            config.Routes.MapHttpRoute(
            name: "AddOrUpdateNotesForUser",
            routeTemplate: "api/AddOrUpdateNotesForUser",
            defaults: new { controller = "UserAccounts", action = "AddOrUpdateNotesForUser" }
            );

            config.Routes.MapHttpRoute(
            name: "AddOrUpdatePreferenceForUser",
            routeTemplate: "api/AddOrUpdatePreferenceForUser",
            defaults: new { controller = "UserAccounts", action = "AddOrUpdatePreferenceForUser" }
            );

            config.Routes.MapHttpRoute(
            name: "UpdateShippingAddressforsubprofile",
            routeTemplate: "api/UpdateShippingAddressforsubprofile",
            defaults: new { controller = "UserAccounts", action = "UpdateShippingAddressforsubprofile" }
            );

            #endregion


            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}


