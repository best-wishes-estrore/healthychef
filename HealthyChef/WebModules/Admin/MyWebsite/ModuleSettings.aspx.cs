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

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class ModuleSettings : System.Web.UI.Page
    {
        protected FormView WebpageFormView;
        protected DropDownList PlaceholderDropDownList;
        protected string urlReferrer = "Default.aspx";
        protected int _instanceId;
        protected int _moduleId;
        protected int _versionId;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Form.DefaultButton = UpdateButton.UniqueID;

            _instanceId = Convert.ToInt32(Request.QueryString["InstanceId"]);
            _moduleId = Convert.ToInt32(Request.QueryString["ModuleId"]);
            _versionId = Convert.ToInt32(Request.QueryString["VersionId"]);

            if (Request.QueryString["InstanceId"] != null)
            {
                urlReferrer += "?InstanceId=" + Request.QueryString["InstanceId"];
            }

            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                WebpageInfo page = Webpage.GetWebpage(_instanceId);
                WebModuleInfo module = WebModule.GetModule(_moduleId);

                Cms.Admin.BindPlaceholdersToList(page, PlaceholderDropDownList, module.Placeholder);
                ModuleNameTextBox.Text = module.Name;
                ModuleTypeName.InnerText = module.WebModuleType.Name;
                ModuleTypeDescription.InnerText = module.WebModuleType.Description;
            }
        }
        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            WebpageInfo p = Webpage.GetWebpage(_instanceId);
            if (null == p) throw new ArgumentException("Invalid navigation id (page not found).");
            WebModule.UpdatePageModuleSettings(ModuleNameTextBox.Text,
                p.Id, _moduleId, PlaceholderDropDownList.SelectedValue);
            Response.Redirect(urlReferrer);
        }
        protected void UpdateCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(urlReferrer);
        }
    }
}