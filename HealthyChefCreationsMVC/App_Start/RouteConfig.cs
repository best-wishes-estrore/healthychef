using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HealthyChefCreationsMVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
            name: "ProductDescription",
              url: "ProductDescription",
              defaults: new { controller = "Menu", action = "ProductDescription", ItemName = UrlParameter.Optional }
              );

            routes.MapRoute(
              name: "SiteMapXml.aspx",
              url: "SiteMapXml.aspx",
              defaults: new { controller = "Common", action = "SiteMapNodeInXml"}
              );

            routes.MapRoute(
              name: "everydaymealplans.aspx",
              url: "everydaymealplans.aspx",
              defaults: new { controller = "Programs", action = "EverydayMealPlan", NavigationIdsList = UrlParameter.Optional }
              );
            routes.MapRoute(
              name: "weightlossprograms.aspx",
              url: "weight-loss-programs.aspx",
              defaults: new { controller = "Programs", action = "WeightLossProgram", NavigationIdsList = UrlParameter.Optional }
              );
            routes.MapRoute(
              name: "signout.aspx",
              url: "signout.aspx",
              defaults: new { controller = "Home", action = "Signout" }
              );
            routes.MapRoute(
              name: "RegisterModel.aspx",
              url: "RegisterModel.aspx",
              defaults: new { controller = "Home", action = "RegisterModel" }
              );
            routes.MapRoute(
            name: "Sitemap",
            url: "Sitemap",
            defaults: new { controller = "Common", action = "SitemapProvider" }
            );

            // route for Forgot Password
            routes.MapRoute(
                name: "forgotpassword.aspx",
                url: "forgotpassword.aspx",
                defaults: new { controller = "Home", action = "ForgotPassword" }
                );

            // route for contact us  
            routes.MapRoute(
                name: "contact_us.aspx",
                url: "contact_us.aspx",
                defaults: new { controller = "Contactus", action = "Index" }
                );
            // route for login
            routes.MapRoute(
                name: "register",
                url: "register.aspx",
                defaults: new { controller = "Register", action = "Display" }
                );
            // route for login  
            routes.MapRoute(
                name: "DirectHomeDashBoardLoginbyFB.aspx",
                url: "DirectHomeDashBoardLoginbyFB.aspx",
                defaults: new { controller = "Home", action = "DirectHomeDashBoardLoginbyFB", email = UrlParameter.Optional }
                );

            // route for login  
            routes.MapRoute(
                name: "login.aspx",
                url: "login.aspx",
                defaults: new { controller = "Home", action = "Login", ReturnUrl=UrlParameter.Optional }
                );
            // route for login  
            routes.MapRoute(
                name: "Facebooksignup.aspx",
                url: "Facebooksignup.aspx",
                defaults: new { controller = "Home", action = "Login" }
                );

            // route for login  
            routes.MapRoute(
                name: "FacebookLogin.aspx",
                url: "FacebookLogin.aspx",
                defaults: new { controller = "Home", action = "FacebookLogin" }
                );

            // route for gift certifcate for add to cart  
            routes.MapRoute(
                name: "BuyGiftCertificate.aspx",
                url: "BuyGiftCertificate.aspx",
                defaults: new { controller = "Content", action = "BuyGiftCertificate" }
                );

            // route for profile
            routes.MapRoute(
                name: "my-profile.aspx",
                url: "my-profile.aspx/{activeTab}",
                defaults: new { controller = "Account", action = "Index", activeTab= "4" }
                );
                // route for thankyou
            routes.MapRoute(
                name: "ThankYou.aspx",
                url: "ThankYou.aspx",
                defaults: new { controller = "Cart", action = "ThankYou" , purchaseNum =UrlParameter.Optional}
                );
            // route for cart
            routes.MapRoute(
                name: "cart.aspx",
                url: "cart.aspx",
                defaults: new { controller = "Cart", action = "Index" }
                );
            // route for calculate shipping
            routes.MapRoute(
                name: "CalculateShipping",
                url: "CalculateShipping.aspx",
                defaults: new { controller = "Cart", action = "CalculateShipping" }
                );

            // route for meal-programs
            routes.MapRoute(
                name: "meal-programs",
                url: "meal-programs.aspx",
                defaults: new { controller = "Programs", action = "Index" }
                );

            // route for view-menu 
            routes.MapRoute(
                name: "view-menu",
                url: "browse-menu.aspx",
                defaults: new { controller = "Menu", action = "Display", mealtype = UrlParameter.Optional, deliveryDate = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "view-menu-b",
                url: "browse-menu/breakfast.aspx",
                defaults: new { controller = "Menu", action = "Display", mealtype = 0, deliveryDate = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "view-menu-l",
                url: "browse-menu/lunch.aspx",
                defaults: new { controller = "Menu", action = "Display", mealtype = 1, deliveryDate = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "view-menu-c",
                url: "browse-menu/child.aspx",
                defaults: new { controller = "Menu", action = "Display", mealtype = 3, deliveryDate = UrlParameter.Optional }
                );
            routes.MapRoute(
                name: "view-menu-O",
                url: "browse-menu/others.aspx",
                defaults: new { controller = "Menu", action = "Display", mealtype = 5, deliveryDate = UrlParameter.Optional }
                );
            // route for cart
            routes.MapRoute(
                name: "ClearCart.aspx",
                url: "ClearCart.aspx",
                defaults: new { controller = "Cart", action = "ClearCart" }
                );
            // route for cart
            routes.MapRoute(
                name: "cartcheckout.aspx",
                url: "cartcheckout.aspx",
                defaults: new { controller = "Cart", action = "CartCheckout" }
                );
            // route for coupon code in cart page
            routes.MapRoute(
                name: "AddCouponCode.aspx",
                url: "AddCouponCode.aspx",
                defaults: new { controller = "Cart", action = "AddCouponCode" }
                );
            // route for redeem gift in cart page
            routes.MapRoute(
                name: "AddRedeemGift.aspx",
                url: "AddRedeemGift.aspx",
                defaults: new { controller = "Cart", action = "AddRedeemGift" }
                );
            // route for remove item from cart
            routes.MapRoute(
                name: "DeleteCartItem.aspx",
                url: "DeleteCartItem.aspx",
                defaults: new { controller = "Cart", action = "DeleteCartItem" }
                );
            // route for updatebasicinfo
            routes.MapRoute(
                name: "updatebasicinfo",
                url: "updatebasicinfo.aspx",
                defaults: new { controller = "Account", action = "UpdateBasicInfo" }
                );
            // route for UpdateShippingInfo
            routes.MapRoute(
                name: "UpdateShippingInfo",
                url: "UpdateShippingInfo.aspx",
                defaults: new { controller = "Account", action = "UpdateShippingInfo" }
                );
            // route for UpdateBillingInfo  
            routes.MapRoute(
                name: "UpdateBillingInfo",
                url: "UpdateBillingInfo.aspx",
                defaults: new { controller = "Account", action = "UpdateBillingInfo" }
                );
            // route for UpdateCreditCardInfo
            routes.MapRoute(
                name: "UpdateCreditCardInfo",
                url: "UpdateCreditCardInfo.aspx",
                defaults: new { controller = "Account", action = "UpdateCreditCardInfo" }
                );
            // route for UpdateCustomerPassword
            routes.MapRoute(
                name: "UpdateCustomerPassword",
                url: "UpdateCustomerPassword.aspx",
                defaults: new { controller = "Account", action = "UpdateCustomerPassword" }
                );
            // route for UpdateCustomerPrefInfo
            routes.MapRoute(
                name: "UpdateCustomerPrefInfo",
                url: "UpdateCustomerPrefInfo.aspx",
                defaults: new { controller = "Account", action = "UpdateCustomerPrefInfo" }
                );
            // route for UpdateCustomerAllergensInfo
            routes.MapRoute(
                name: "UpdateCustomerAllergensInfo",
                url: "UpdateCustomerAllergensInfo.aspx",
                defaults: new { controller = "Account", action = "UpdateCustomerAllergensInfo" }
                );
            // route for UpdateCustomerAllergensInfo
            routes.MapRoute(
                name: "GetorderdetailsbyCartid",
                url: "Account/GetorderdetailsbyCartid/{Cartid}",
                defaults: new { controller = "Account", action = "GetorderdetailsbyCartid" }
                );
            // route for DeleteRecurringOrder  
            routes.MapRoute(
                name: "DeleteRecurringOrder",
                url: "DeleteRecurringOrder.aspx",
                defaults: new { controller = "Account", action = "DeleteRecurringOrder" }
                );
            // route for DeActivateProfile
            routes.MapRoute(
                name: "DeActivateProfile",
                url: "Account/DeActivateProfile",
                defaults: new { controller = "Account", action = "DeActivateProfile" }
                );
            // route for UpdateSubProfileBasicInfo
            routes.MapRoute(
                name: "UpdateSubProfileBasicInfo",
                url: "Account/UpdateSubProfileBasicInfo",
                defaults: new { controller = "Account", action = "UpdateSubProfileBasicInfo" }
                );
            // route for UpdateSubProfileShippingInfo 
            routes.MapRoute(
                name: "UpdateSubProfileShippingInfo",
                url: "Account/UpdateSubProfileShippingInfo",
                defaults: new { controller = "Account", action = "UpdateSubProfileShippingInfo" }
                );
            // route for UpdateSubProfilePrefInfo
            routes.MapRoute(
                name: "UpdateSubProfilePrefInfo",
                url: "Account/UpdateSubProfilePrefInfo",
                defaults: new { controller = "Account", action = "UpdateSubProfilePrefInfo" }
                );
            // route for UpdateSubProfileAllergensInfo
            routes.MapRoute(
                name: "UpdateSubProfileAllergensInfo",
                url: "Account/UpdateSubProfileAllergensInfo",
                defaults: new { controller = "Account", action = "UpdateSubProfileAllergensInfo" }
                );
            // route for UpdateSubProfilePrefInfo
            routes.MapRoute(
                name: "SelectSubProfile",
                url: "Account/SelectSubProfile",
                defaults: new { controller = "Account", action = "SelectSubProfile" }
                );
            // route for programs    
            routes.MapRoute(
                name: "programdetails",
                url: "programdetails.aspx",
                defaults: new { controller = "Programs", action = "Details", id = UrlParameter.Optional }
                );
            // route for programs checkboxes   
            routes.MapRoute(
                name: "programs",
                url: "Programs/DetailsbyCheckbox/{Id}/{programName}",
                defaults: new { controller = "Programs", action = "DetailsbyCheckbox"}
                );
            // route for view-menu by datechange
            routes.MapRoute(
                name: "MealDetailsByMealId",
                url: "MealDetailsByMealId.aspx",
                defaults: new { controller = "Menu", action = "MealDetailsByMealId", mealtype = UrlParameter.Optional, deliveryDate = UrlParameter.Optional }
                );
            // route for sub-profiles
            routes.MapRoute(
                name: "subprofile",
                url: "subprofile.aspx",
                defaults: new { controller = "Account", action = "SelectSubProfileInfo" } 
                );
            // route for addtocartajax
            routes.MapRoute(
                name: "AddMealToCartAjax",
                url: "AddMealToCartAjax.aspx",
                defaults: new { controller = "Menu", action = "AddMealToCartAjax" }
                );
            // route for ddlchange in programs
            routes.MapRoute(
                name: "DetailsbyCalendarId",
                url: "DetailsbyCalendarId.aspx",
                defaults: new { controller = "Programs", action = "DetailsbyCalendarId", id = UrlParameter.Optional, calendarid = UrlParameter.Optional }
                );
            // route for noofweeks in programs
            routes.MapRoute(
                name: "AddProgramToCart",
                url: "AddProgramToCart.aspx",
                defaults: new { controller = "Programs", action = "AddProgramToCart"}
                );
            // route for add program to cart
            routes.MapRoute(
                name: "GetPlanPriceByNumofweeks",
                url: "GetPlanPriceByNumofweeks.aspx",
                defaults: new { controller = "Programs", action = "GetPlanPriceByNumofweeks", PlanPriceByNumofweeks = UrlParameter.Optional }
                );
            // route for program details
            routes.MapRoute(
                name: "program-details",
                url: "program-details.aspx",
                defaults: new { controller = "Programs", action = "Details" }
                );
            // route for delete item in cart
            routes.MapRoute(
                name: "DeleteAutoRenewItem",
                url: "DeleteAutoRenewItem.aspx",
                defaults: new { controller = "Cart", action = "DeleteAutoRenewItem" }
                ); 
                // route for update cart items
            routes.MapRoute(
                name: "UpdateCartItemQuantity",
                url: "UpdateCartItemQuantity.aspx",
                defaults: new { controller = "Cart", action = "UpdateCartItemQuantity" }
                );
            // route for complete purchase
            routes.MapRoute(
                name: "CompletePurchase",
                url: "CompletePurchase.aspx",
                defaults: new { controller = "Cart", action = "CompletePurchase" }
                );
            // route for paypal  
            routes.MapRoute(
                name: "PaymentWithPaypal",
                url: "PaymentWithPaypal.aspx",
                defaults: new { controller = "Paypal", action = "PaymentWithPaypal" }
                );
            // route for cancel and restart
            routes.MapRoute(
                name: "ConfirmCancel",
                url: "ConfirmCancel.aspx",
                defaults: new { controller = "Cart", action = "ConfirmCancel" }
                );
            //customized route for content page
            routes.MapRoute(
                name: "content",
                url: "Content/Details/{Id}",
                defaults: new { controller = "Content", action = "Details",Id = UrlParameter.Optional }
                );
            ////route for order-now  
            //routes.MapRoute(
            //    name: "order-now.aspx",
            //    url: "order-now.aspx",
            //    defaults: new { controller = "Content", action = "Details", Id = 7 }
            //    );

            //customized route for content page
            routes.MapRoute(
                name: "contentpage",
                url: HealthyChefCreationsMVC.CustomModels.ContentViewModel.routeProvider + "{virtualpath}/{virtualpath2}/{virtualpath3}/{virtualpath4}",
                defaults: new { controller = "Content", action = "DetailsByVirtualPath", virtualpath = UrlParameter.Optional, virtualpath2 = UrlParameter.Optional, virtualpath3 = UrlParameter.Optional, virtualpath4 = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Display", id = UrlParameter.Optional }
            );
        }
    }
}
