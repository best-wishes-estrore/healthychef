using System;
using System.Collections.Generic;
using System.Web;

namespace BayshoreSolutions.WebModules.Cms
{
    abstract public class ModuleAdminPage : System.Web.UI.Page
    {
        WebModuleInfo _webmodule = null;
        public WebModuleInfo Module
        {
            get { return _webmodule; }
        }

        /// <summary>
        /// Module id assigned by the CMS.
        /// </summary>
        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }

        /// <summary>
        /// Page instance id (navigation id) that contains the module.
        /// </summary>
        public int PageNavigationId
        {
            get { return (int)(ViewState["PageNavigationId"] ?? -1); }
            set { ViewState["PageNavigationId"] = value; }
        }

        private void InitModule()
        {
            int moduleId = 0;
            int.TryParse(Request["ModuleId"], out moduleId);
            this.ModuleId = moduleId;

            _webmodule = WebModule.GetModule(moduleId);

            WebpageInfo page = null;
            if (this.ModuleId <= 0
                || null == _webmodule
                || null == (page = _webmodule.Webpage))
            {
                Admin.RedirectToMainMenu();
            }
            this.PageNavigationId = page.InstanceId;

            //check user permissions.
            if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(this.PageNavigationId, Page.User))
                throw new System.Security.SecurityException("The current user does not have permission to access this resource.");
        }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
                _webmodule = WebModule.GetModule(this.ModuleId);
            else
            {
                InitModule();
                EnsureModule();
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        abstract protected void EnsureModule();
    }
}
