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
using BayshoreSolutions.WebModules.Cms.Security.Model;

namespace BayshoreSolutions.WebModules.Cms.Security.UserRegistration
{
    public partial class Edit : System.Web.UI.Page
    {
        /// <summary>
        /// The module instance id assigned by the CMS.
        /// </summary>
        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }
        /// <summary>
        /// The instance id (navigation id) of the page that contains the module.
        /// </summary>
        public int PageNavigationId
        {
            get { return (int)(ViewState["PageNavigationId"] ?? -1); }
            set { ViewState["PageNavigationId"] = value; }
        }
        private void InitModule()
        {
            int moduleId = 0;
            int.TryParse(Request.QueryString["ModuleId"], out moduleId);
            this.ModuleId = moduleId;

            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            WebpageInfo page = null;
            if (this.ModuleId <= 0
                || null == module
                || null == (page = module.Webpage))
            {
                Redirect(Webpage.RootNavigationId);
            }
            this.PageNavigationId = page.InstanceId;

            //check user permissions.
            if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(this.PageNavigationId, Page.User))
                throw new System.Security.SecurityException("The current user does not have permission to access this resource.");

            HeaderCtl.Text = module.WebModuleType.Name;

            EnsureModule();
        }
        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is a new module), then a 
        /// new module object is created using the module id assigned by the 
        /// CMS.
        /// </summary>
        private void EnsureModule()
        {
            UserRegistration_Module userRegModule = UserRegistration_Module.Get(this.ModuleId);
            if (null == userRegModule)
            {
                userRegModule = new UserRegistration_Module();
                userRegModule.ModuleId = this.ModuleId;
                userRegModule.Save();
            }
        }
        private void LoadModule()
        {
            UserRegistration_Module userRegModule = UserRegistration_Module.Get(this.ModuleId);
            ConfirmationPage.SelectedNavigationId = userRegModule.ConfirmationPageNavigationId;
            NotifyEmail.Text = userRegModule.NotifyEmailAddress;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitModule();
                LoadModule();
            }
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Redirect(this.PageNavigationId);
        }
        private void Redirect(int navId)
        {
            Response.Redirect("~/WebModules/Admin/MyWebsite/Default.aspx?instanceId=" + navId);
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            UserRegistration_Module userRegModule = UserRegistration_Module.Get(this.ModuleId);
            userRegModule.ConfirmationPageNavigationId = ConfirmationPage.SelectedNavigationId;
            userRegModule.NotifyEmailAddress = NotifyEmail.Text.Trim();
            userRegModule.Save();
            Redirect(this.PageNavigationId);
        }
    }
}
