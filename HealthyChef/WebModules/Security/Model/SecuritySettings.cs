using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Cms.Security.Model
{
    public class SecuritySettings
    {
        public const string ModuleType_Security = "Security";
        public const string ModuleType_PasswordRecovery = "Security/PasswordRecovery";
        public const string ModuleType_UserRegistration = "Security/UserRegistration";

        public static string EmailFromAddress
        {
            get
            {
                return Settings.GetSettingValue(ModuleType_Security,
                    "EmailFromAddress",
                    string.Format("no-reply@{0}", HttpContext.Current.Request.Url.Host));
            }
            set { Settings.Save(ModuleType_Security, "EmailFromAddress", value); }
        }
        public static string EmailBodyHeader
        {
            get
            {
                return Settings.GetSettingValue(ModuleType_Security,
                    "EmailBodyHeader",
                    string.Empty);
            }
            set { Settings.Save(ModuleType_Security, "EmailBodyHeader", value); }
        }
        public static string EmailBodyFooter
        {
            get
            {
                return Settings.GetSettingValue(ModuleType_Security,
                    "EmailBodyFooter",
                    string.Format(@"
Thank you!

---

This is an automated email from the " + Website.Current.Resource.Name + @" website. If you believe you have received this email in error, please contact the site administrator.
",
                        Website.Current.Resource.Name));
            }
            set { Settings.Save(ModuleType_Security, "EmailBodyFooter", value); }
        }

        public static string UserRegistrationEmailSubject
        {
            get
            {
                return Settings.GetSettingValue(ModuleType_UserRegistration,
                    "UserRegistrationEmailSubject",
                    "New user registration");
            }
            set { Settings.Save(ModuleType_UserRegistration, "UserRegistrationEmailSubject", value); }
        }
        public static string UserRegistrationEmailBody
        {
            get
            {
                return Settings.GetSettingValue(ModuleType_UserRegistration,
                    "UserRegistrationEmailBody",
                    string.Format("Your login and password for {0} are as follows:",
                        Website.Current.Resource.Name));
            }
            set { Settings.Save(ModuleType_UserRegistration, "UserRegistrationEmailBody", value); }
        }

        public static string PasswordRecoveryEmailSubject
        {
            get
            {
                return Settings.GetSettingValue(ModuleType_PasswordRecovery,
                    "PasswordRecoveryEmailSubject",
                    "Login information");
            }
            set { Settings.Save(ModuleType_PasswordRecovery, "PasswordRecoveryEmailSubject", value); }
        }
        public static string PasswordRecoveryEmailBody
        {
            get
            {
                return Settings.GetSettingValue(ModuleType_PasswordRecovery,
                    "PasswordRecoveryEmailBody",
                    string.Format("Your login and password for {0} are as follows:",
                        Website.Current.Resource.Name));
            }
            set { Settings.Save(ModuleType_PasswordRecovery, "PasswordRecoveryEmailBody", value); }
        }
    }
}
