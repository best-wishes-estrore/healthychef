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
using System.Collections.Generic;

using BayshoreSolutions.WebModules;

using System.Linq;

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class PagesSharingModule : System.Web.UI.Page
    {
        int? _moduleId = null;
        int? _instanceId = null;
        WebpageInfo _page = null;
        WebModuleInfo _module = null;

        protected string Referrer
        {
            get { return (string)ViewState["referrer"]; }
            set { ViewState["referrer"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //check user permissions.
            if (_instanceId.HasValue && !BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId.Value, Page.User))
                throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

            int n;
            string strModuleId = Request.QueryString["moduleId"];
            if (int.TryParse(strModuleId, out n))
            {
                _moduleId = n;
                _module = WebModule.GetModule(_moduleId.Value);
            }

            string strInstanceId = Request.QueryString["InstanceId"];
            if (int.TryParse(strInstanceId, out n))
            {
                _instanceId = n;
                _page = Webpage.GetWebpage(_instanceId.Value);
            }

            if (!IsPostBack)
            {
                lvPages.DataSource = null;
                if (_moduleId.HasValue)
                {
                    var pages = (from w in Webpage.GetWebpagesSharingModule(_moduleId.Value)
                                 orderby w.Path
                                 select new { w.InstanceId, w.Path }).Distinct();
                    lvPages.DataSource = pages;
                }
                lvPages.DataBind();

                if (Request.UrlReferrer != null)
                {
                    Referrer = Request.UrlReferrer.PathAndQuery;
                }

                if (_module != null)
                {
                    literalModuleName.Text = _module.Name;
                    literalModuleType.Text = _module.ModuleTypeName;
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs args)
        {
            string strUrl = Referrer;
            if (!string.IsNullOrEmpty(strUrl))
            {
                try
                {
                    Response.Redirect(strUrl);
                }
                catch (System.Threading.ThreadAbortException)
                {
                }
            }
        }
    }
}