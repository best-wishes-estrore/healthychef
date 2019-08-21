using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Data;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthyChef.Common
{
    public static class Helpers
    {
        public static MembershipUser LoggedUser
        {
            get
            {
                return Membership.GetUser();
            }
        }

        public static List<US_State> US_States
        {
            get
            {
                List<US_State> states = new List<US_State>();

                states.Add(new US_State { Name = "Alabama", Abbr = "AL" });
                states.Add(new US_State { Name = "Alaska", Abbr = "AK" });
                states.Add(new US_State { Name = "Arizona", Abbr = "AZ" });
                states.Add(new US_State { Name = "Arkansas", Abbr = "AR" });
                states.Add(new US_State { Name = "California", Abbr = "CA" });
                states.Add(new US_State { Name = "Colorado", Abbr = "CO" });
                states.Add(new US_State { Name = "Connecticut", Abbr = "CT" });
                states.Add(new US_State { Name = "District of Columbia", Abbr = "DC" });
                states.Add(new US_State { Name = "Delaware", Abbr = "DE" });
                states.Add(new US_State { Name = "Florida", Abbr = "FL" });
                states.Add(new US_State { Name = "Georgia", Abbr = "GA" });
                states.Add(new US_State { Name = "Hawaii", Abbr = "HI" });
                states.Add(new US_State { Name = "Idaho", Abbr = "ID" });
                states.Add(new US_State { Name = "Illinois", Abbr = "IL" });
                states.Add(new US_State { Name = "Indiana", Abbr = "IN" });
                states.Add(new US_State { Name = "Iowa", Abbr = "IA" });
                states.Add(new US_State { Name = "Kansas", Abbr = "KS" });
                states.Add(new US_State { Name = "Kentucky", Abbr = "KY" });
                states.Add(new US_State { Name = "Louisiana", Abbr = "LA" });
                states.Add(new US_State { Name = "Maine", Abbr = "ME" });
                states.Add(new US_State { Name = "Maryland", Abbr = "MD" });
                states.Add(new US_State { Name = "Massachusetts", Abbr = "MA" });
                states.Add(new US_State { Name = "Michigan", Abbr = "MI" });
                states.Add(new US_State { Name = "Minnesota", Abbr = "MN" });
                states.Add(new US_State { Name = "Mississippi", Abbr = "MS" });
                states.Add(new US_State { Name = "Missouri", Abbr = "MO" });
                states.Add(new US_State { Name = "Montana", Abbr = "MT" });
                states.Add(new US_State { Name = "Nebraska", Abbr = "NE" });
                states.Add(new US_State { Name = "Nevada", Abbr = "NV" });
                states.Add(new US_State { Name = "New Hampshire", Abbr = "NH" });
                states.Add(new US_State { Name = "New Jersey", Abbr = "NJ" });
                states.Add(new US_State { Name = "New Mexico", Abbr = "NM" });
                states.Add(new US_State { Name = "New York", Abbr = "NY" });
                states.Add(new US_State { Name = "North Carolina", Abbr = "NC" });
                states.Add(new US_State { Name = "North Dakota", Abbr = "ND" });
                states.Add(new US_State { Name = "Ohio", Abbr = "OH" });
                states.Add(new US_State { Name = "Oklahoma", Abbr = "OK" });
                states.Add(new US_State { Name = "Oregon", Abbr = "OR" });
                states.Add(new US_State { Name = "Pennsylvania", Abbr = "PA" });
                states.Add(new US_State { Name = "Rhode Island", Abbr = "RI" });
                states.Add(new US_State { Name = "South Carolina", Abbr = "SC" });
                states.Add(new US_State { Name = "South Dakota", Abbr = "SD" });
                states.Add(new US_State { Name = "Tennessee", Abbr = "TN" });
                states.Add(new US_State { Name = "Texas", Abbr = "TX" });
                states.Add(new US_State { Name = "Utah", Abbr = "UT" });
                states.Add(new US_State { Name = "Vermont", Abbr = "VT" });
                states.Add(new US_State { Name = "Virginia", Abbr = "VA" });
                states.Add(new US_State { Name = "Washington", Abbr = "WA" });
                states.Add(new US_State { Name = "West Virginia", Abbr = "WV" });
                states.Add(new US_State { Name = "Wisconsin", Abbr = "WI" });
                states.Add(new US_State { Name = "Wyoming", Abbr = "WY" });

                return states;
            }
        }

        public static string CreateUserStatusMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.InvalidPassword:
                    return "Password does not meet strength requirements, or does not meet the following criteria: "
                        + "<br/>minimum length: "
                        + Membership.MinRequiredPasswordLength
                        + "<br/>minimum non-alphanumeric characters: "
                        + Membership.MinRequiredNonAlphanumericCharacters;
                case MembershipCreateStatus.DuplicateEmail:
                    return " Email address is already in use.";
                case MembershipCreateStatus.DuplicateUserName:
                    return " Username is already in use.";
                default:
                    return status.ToString();
            }
        }

        public static bool LockUser(MembershipUser user)
        {
            try
            {
                for (int i = 0; i < Membership.MaxInvalidPasswordAttempts; i++)
                    Membership.ValidateUser(user.UserName, "thanksMSformakingususehacks");

                return user.IsLockedOut;
            }
            catch
            {
                throw;
            }

        }

        public static string GetSiteRoot()
        {
            string Port = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = System.Web.HttpContext.Current.Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            string appPath = System.Web.HttpContext.Current.Request.ApplicationPath;
            if (appPath == "/")
                appPath = "";

            string sOut = Protocol + System.Web.HttpContext.Current.Request.ServerVariables["SERVER_NAME"] + Port + appPath;
            return sOut;
        }

        public static string GetFileText(string virtualPath)
        {
            //Read from file
            StreamReader sr = null;
            try
            {
                sr = new StreamReader(System.Web.HttpContext.Current.Server.MapPath(virtualPath));
            }
            catch
            {
                sr = new StreamReader(virtualPath);

            }
            string strOut = sr.ReadToEnd();
            sr.Close();
            return strOut;
        }

        public class US_State
        {
            public string Name { get; set; }
            public string Abbr { get; set; }
        }

        /// <summary>
        /// Gets the HttpContext.Current.User.Identity.Name OR HttpContext.Current.Request.UserHostAddress(IP) depending on whether the current user is authenticated.
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            string sUserName = string.Empty;

            if (HttpContext.Current.User.Identity.IsAuthenticated)
                sUserName = HttpContext.Current.User.Identity.Name;
            else
                sUserName = HttpContext.Current.Request.AnonymousID;

            return sUserName.Trim();
        }

        public static List<IValidator> GetValidatorsByGroup(ValidatorCollection valColl, string validationGroup)
        {
            List<IValidator> groupValidators = new List<IValidator>();

            valColl.Cast<IValidator>().ToList().ForEach(delegate(IValidator iVal)
            {
                Type valType = iVal.GetType();

                switch (valType.Name.ToString())
                {
                    case "CustomValidator":
                        CustomValidator val1 = (CustomValidator)iVal;
                        if (val1.ValidationGroup == validationGroup)
                            groupValidators.Add(val1);
                        break;
                    case "RequiredFieldValidator":
                        RequiredFieldValidator val2 = (RequiredFieldValidator)iVal;
                        if (val2.ValidationGroup == validationGroup)
                            groupValidators.Add(val2);
                        break;
                    case "CompareValidator":
                        CompareValidator val3 = (CompareValidator)iVal;
                       if (val3.ValidationGroup == validationGroup)
                           groupValidators.Add(val3);
                        break;
                    case "RegularExpressionValidator":
                        RegularExpressionValidator val4 = (RegularExpressionValidator)iVal;
                       if (val4.ValidationGroup == validationGroup)
                           groupValidators.Add(val4);
                        break;
                    case "RangeValidator":
                        RangeValidator val5 = (RangeValidator)iVal;
                        if (val5.ValidationGroup == validationGroup)
                            groupValidators.Add(val5);
                        break;
                    default:
                        throw new Exception(valType.ToString() + ": Add this type to the switch -> Helpers.GetValidatorsByGroup().");
                }
            });

            return groupValidators;
        }

        public static decimal TruncateDecimal(decimal value, int precision)
        {
            try
            {
                decimal step = (decimal)Math.Pow(10, precision);
                int tmp = (int)Math.Truncate(step * value);
                return tmp / step;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
