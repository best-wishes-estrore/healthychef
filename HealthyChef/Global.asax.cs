using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Diagnostics;
using System.Web.Configuration;
using BayshoreSolutions.WebModules;
using System.Web.Routing;
using System.Net;

namespace HealthyChef
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;


            RouteTable.Routes.MapPageRoute("details", "details/{programname}", "~/Details.aspx", false);
            //RouteTable.Routes.MapPageRoute("meal-programs", "meal-programs/{programname}", "~/Details.aspx", false);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

            if (Request.UrlReferrer == null)
                return;

            if (Request.UrlReferrer.DnsSafeHost != Request.Url.DnsSafeHost)
            {
                Session.Add("session_referringUrl", Request.UrlReferrer.AbsoluteUri);

                string strRequestedUrl = string.Format("http{0}://{1}{2}",
                        Request.Url.Port == 443 ? "s" : string.Empty,
                        Request.Url.Authority,
                        Request.RawUrl);

                Session.Add("session_landingUrl", strRequestedUrl);

                Session.Add("session_referrer", Request.UrlReferrer.Host);

                if (!string.IsNullOrEmpty(Request.UrlReferrer.Query))
                {
                    Session.Add("session_keywords", GetQuery(Request.UrlReferrer.Query));
                }
            }
        }

        private string GetQuery(string u)
        {
            // 1
            // Try to match start of query with "&q=". These matches are ideal.
            int start = u.IndexOf("&q=", StringComparison.Ordinal);
            int length = 3;
            // 2
            // Try to match part with q=. This may be prefixed by another letter.
            if (start == -1)
            {
                start = u.IndexOf("q=", StringComparison.Ordinal);
                length = 2;
            }
            // 3
            // Try to match start of query with "p=".
            if (start == -1)
            {
                start = u.IndexOf("p=", StringComparison.Ordinal);
                length = 2;
            }
            // 4
            // Return if not possible
            if (start == -1)
            {
                return string.Empty;
            }
            // 5
            // Advance N characters
            start += length;
            // 6
            // Find first & after that
            int end = u.IndexOf('&', start);
            // 7
            // Use end index if no & was found
            if (end == -1)
            {
                end = u.Length;
            }
            // 8
            // Get substring between two parameters
            string sub = u.Substring(start, end - start);
            // 9
            // Get the decoded URL
            string result = HttpUtility.UrlDecode(sub);
            // 10
            // Get the HTML representation
            result = HttpUtility.HtmlEncode(result);
            // 11
            // Prepend sitesearch label to output
            if (u.IndexOf("sitesearch", StringComparison.Ordinal) != -1)
            {
                result = "sitesearch: " + result;
            }
            return result;
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.AppRelativeCurrentExecutionFilePath == "~/")
                Response.Redirect("~/AdminLogin.aspx");
            if (bool.Parse(WebConfigurationManager.AppSettings["IsDownTime"].ToString()) && !HttpContext.Current.Request.Url.OriginalString.Contains("DownTime.aspx"))
            {
                Response.Redirect("~/DownTime.aspx", true);
            }
        }

        public void AnonymousIdentification_Creating(Object sender, AnonymousIdentificationEventArgs e)
        {
            // Change the anonymous id
            e.AnonymousID = "hcc.com_Anon_User_" + DateTime.Now.Ticks;            
        }
    }

}