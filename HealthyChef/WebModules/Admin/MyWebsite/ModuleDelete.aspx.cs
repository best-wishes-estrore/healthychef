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
    public partial class ModuleDelete : System.Web.UI.Page
    {
        int _instanceId;
        string _urlReferrer;
        int _moduleId;
        WebModuleInfo _module = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            _instanceId = int.Parse(Request.QueryString["InstanceId"]);
            _urlReferrer = "Default.aspx?InstanceId=" + _instanceId;
            _moduleId = int.Parse(Request.QueryString["ModuleId"]);
            _module = WebModule.GetModule(_moduleId);

            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                ModuleName.Text = _module.Name;

                if (_module.ShareCount > 0)
                {
                    messageBox.ShowError("Unable to delete this module because it is shared on other pages.");
                    EditDeleteButton.Enabled = false;
                }
            }
        }
        protected void DeleteCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(_urlReferrer);
        }
        protected void EditDeleteButton_Click(object sender, EventArgs e)
        {
            WebModule.DeleteModule(_moduleId);

            Response.Redirect(_urlReferrer);
        }
    }
}