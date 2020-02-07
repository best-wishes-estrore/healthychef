using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Templates.WebModules.Controls
{
    public partial class Breadcrumbs : System.Web.UI.UserControl
    {
        public int NavigationId
        {
            get { return (int)(ViewState["NavigationId"] ?? -1); }
            set { ViewState["NavigationId"] = value; }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (this.NavigationId <= 0)
                { //try query string
                    string qnavid = Request.QueryString["instanceId"] ?? Request.QueryString["parentInstanceId"];
                    if (!String.IsNullOrEmpty(qnavid))
                        this.NavigationId = int.Parse(qnavid);
                }

                this.Load_(this.NavigationId);
            }
        }
        public void Load_(int navigationId)
        {
            if (this.NavigationId <= 0)
            {
                BreadcrumbsList.Visible = false;
            }
            else
            {
                BreadcrumbsList.Visible = true;
                System.Collections.Generic.List<WebpageInfo> ancestors = new System.Collections.Generic.List<WebpageInfo>();
                WebpageInfo p = Webpage.GetWebpage(navigationId);
                while (p != null)
                {
                    ancestors.Add(p);
                    p = p.Parent;
                }
                ancestors.Reverse();
                BreadcrumbsList.DataSource = ancestors;
                BreadcrumbsList.DataBind();
            }
        }
    }
}