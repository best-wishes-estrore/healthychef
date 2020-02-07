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

using Bss = BayshoreSolutions.Common;
using BayshoreSolutions.WebModules;
using BayshoreSolutions.WebModules.Cms.FileManager.Model;

namespace BayshoreSolutions.WebModules.Cms.FileManager.FileList
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

            HeaderCtl.InnerHtml = module.WebModuleType.Name;

            EnsureModule();
        }
        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        private void EnsureModule()
        {
            FileList_Module module = FileList_Module.Get(this.ModuleId);
            if (null == module)
            { //create default
                module = new FileList_Module();
                module.ModuleId = this.ModuleId;
                module.ShowFolderList = true;
                module.RootPath = string.Empty;
                module.Save();
            }
        }
        private void LoadModule()
        {
            FileList_Module module = FileList_Module.Get(this.ModuleId);
            RootPath.Text = module.RootPath;
            ShowFolderList.Checked = module.ShowFolderList;
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
        private bool Validate(FileList_Module module)
        {
            if (!Page.IsValid)
                return false;

            string virtualPath = Bss.Web.Url.Combine(Settings.FileStorageRootPath, module.RootPath);
            string physicalPath = Server.MapPath(virtualPath);

            if (!System.IO.Directory.Exists(physicalPath))
            {
                Msg.ShowError("Invalid folder path.");
                return false;
            }

            return true;
        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            FileList_Module module = FileList_Module.Get(this.ModuleId);
            module.ShowFolderList = ShowFolderList.Checked;
            module.RootPath = RootPath.Text;

            if (!Validate(module))
                return;

            //
            //save
            //
            module.Save();

            Redirect(this.PageNavigationId);
        }
    }
}
