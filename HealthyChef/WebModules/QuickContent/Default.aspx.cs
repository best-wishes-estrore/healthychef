using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SubSonic;

/***************
 * This page displays a list of all Quick Content areas,
 * and provides links for editing as well.
 ***************/
namespace BayshoreSolutions.WebModules.QuickContent
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            //prevent loading multiple css style sheet
            HtmlControl css = null;
            css = Page.Header.FindControl("PopupEditorCSS") as HtmlControl;

            if (css == null)
            {
                //load the style sheet
                HtmlLink cssLink = new HtmlLink();
                cssLink.ID = "PopupEditorCSS";
                cssLink.Href = ResolveUrl("~/WebModules/QuickContent/public/css/subModal.css");
                cssLink.Attributes["rel"] = "stylesheet";
                cssLink.Attributes["type"] = "text/css";

                // Add the HtmlLink to the Head section of the page.
                Page.Header.Controls.Add(cssLink);

            }

            // Load javascript
            this.Page.ClientScript.RegisterClientScriptInclude("submodal", ResolveUrl("~/WebModules/QuickContent/public/js/subModal.js"));

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Get list of active quick content areas
            Query q = QuickContentContent.CreateQuery();
            q.SelectList = QuickContentContent.Columns.ContentName;
            q.AddWhere(QuickContentContent.Columns.StatusId, 2);
            q.AddWhere(QuickContentContent.Columns.Culture, Thread.CurrentThread.CurrentUICulture.Name.ToLower());
            q.ORDER_BY(QuickContentContent.Columns.ContentName);

            GridView1.DataSource = q.ExecuteReader();
            GridView1.DataBind();

        }
        public string GetBaseUrl()
        {
            string Port = Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            // *** Figure out the base Url which points at the application's root
            string strBaseUrl = Protocol + Request.ServerVariables["SERVER_NAME"] + Port + Request.ApplicationPath;
            if (!strBaseUrl.EndsWith("/"))
            {
                strBaseUrl += "/";
            }

            return strBaseUrl;
        }

    }
}
