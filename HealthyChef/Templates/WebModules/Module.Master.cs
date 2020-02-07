using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using CMS = BayshoreSolutions.WebModules.Cms;

namespace BayshoreSolutions.WebModules.Templates.WebModules
{
    public partial class Module : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CMS.ModuleAdminPage moduleAdminPage =
                    this.Page as BayshoreSolutions.WebModules.Cms.ModuleAdminPage;
                if (null != moduleAdminPage)
                {
                    ModuleNameCtl.Text = moduleAdminPage.Module.Name;
                    ModuleTypeNameCtl.Text = moduleAdminPage.Module.WebModuleType.Name;
                    Page.Title = moduleAdminPage.Module.WebModuleType.Name + " Module";
                    MainMenuLink.HRef = CMS.Admin.GetMainMenuUrl(moduleAdminPage.PageNavigationId);
                }
            }
        }
    }
}
