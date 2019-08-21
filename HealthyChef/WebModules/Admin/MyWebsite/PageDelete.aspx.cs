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

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class PageDelete : System.Web.UI.Page
    {
        string _urlReferrer = "Default.aspx?InstanceId=";
        int _instanceId = -1;
        WebpageInfo _page = null;

        public String PageType
        {
            get { return (String)(ViewState["PageType"] ?? "page"); }
            set { ViewState["PageType"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            _instanceId = int.Parse(Request.QueryString["InstanceId"]);
            _page = Webpage.GetWebpage(_instanceId);

            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                this.PageType = _page.IsAlias || !String.IsNullOrEmpty(_page.ExternalUrl) ? "link" : "page";
                PageName.Text = _page.Text;
            }
        }
        protected void DeleteCancelButton_Click(object sender, EventArgs e)
        {
            _urlReferrer += _instanceId;
            Response.Redirect(_urlReferrer);
        }
        protected void EditDeleteButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            //prevent malicious deletes.
            if (_page.InstanceId == Webpage.RootNavigationId)
                throw new InvalidOperationException("The portal root may not be deleted.");
            if (_page.ParentInstanceId == Webpage.RootNavigationId)
                throw new InvalidOperationException("Root pages (website roots) may not be deleted.");

            if (!_page.IsAlias)
            {
                if (_page.Children.Count > 0)
                    throw new InvalidOperationException("Pages with children may not be deleted.");

                System.Collections.Generic.List<WebModuleInfo> modules = WebModule.GetModules(_instanceId);
                if (modules.Count > 0)
                    throw new InvalidOperationException("Pages with modules may not be deleted.");
            }

            _urlReferrer += _page.ParentInstanceId.Value;
            Webpage.DeleteWebpage(_instanceId);
            Response.Redirect(_urlReferrer);
        }
    }
}