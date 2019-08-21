using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Api;

namespace HealthyChefCreationsMVC.CustomModels
{
    public static class PaypalConfiguration
    {
        //Variables for storing the clientID and clientSecret key
        public readonly static string ClientId;
        public readonly static string ClientSecret;

        //Constructor
        static PaypalConfiguration()
        {
            var config = ConfigManager.Instance.GetProperties();
            //ClientId = System.Configuration.ConfigurationManager.AppSettings["clientId"]; ;//config["clientId"];
            //ClientSecret = System.Configuration.ConfigurationManager.AppSettings["clientSecret"]; ;// config["clientSecret"];
        }

        // getting properties from the web.config
        public static Dictionary<string, string> GetConfig()
        {
            return PayPal.Api.ConfigManager.Instance.GetProperties();
        }

        private static string GetAccessToken()
        {
            string accessToken = string.Empty;
            try
            {
                //ExceptionLogging.ProcessingFalied(ClientId);
                //ExceptionLogging.ProcessingFalied(ClientSecret);
                //ExceptionLogging.ProcessingFalied(GetConfig().ToString());
                // getting accesstocken from paypal                
                var config = ConfigManager.Instance.GetProperties();
                accessToken = new OAuthTokenCredential(config).GetAccessToken();
                //ExceptionLogging.ProcessingFalied("step for getting accesstocken from paypal "+ accessToken);
            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);
                throw ex;
            }

            return accessToken;
        }

        public static APIContext GetAPIContext()
        {
            //ExceptionLogging.ProcessingFalied("GetAPIContext Step1");
            APIContext apiContext = new APIContext(GetAccessToken());
            try
            {
                // return apicontext object by invoking it with the accesstoken

                apiContext.Config = GetConfig();

            }
            catch (Exception ex)
            {
               // ExceptionLogging.SendErrorToText(ex);
                throw ex;
            }
            return apiContext;
        }
    }
}