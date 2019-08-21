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

using WM = BayshoreSolutions.WebModules;
using CMS = BayshoreSolutions.WebModules.Cms;

namespace HealthyChef.WebModules.ShoppingCart
{
    //
    //BayshoreSolutions.WebModules.Cms.ModuleAdminPage provides the following 
    //properties (by reading the querystring):
    //      Module
    //      ModuleId
    //      PageNavigationId
    //
    //if you are using legacy WebModules, just drop the latest
    //WebModules/Admin/ModuleAdminPage.cs file into your project.
    //
    public partial class Edit : BayshoreSolutions.WebModules.Cms.ModuleAdminPage
    {
        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        override protected void EnsureModule()
        {
            //implement custom module check/creation code here, if any.
        }

        private void LoadModule()
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadModule();
            }
        }
    }
}
