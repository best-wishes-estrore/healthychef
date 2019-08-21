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

using model = BayshoreSolutions.WebModules.SiteMapModule.Model;

namespace BayshoreSolutions.WebModules.SiteMapModule
{
    public partial class Display : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                model.SiteMapModule smModule = model.SiteMapModule.Get(ModuleId);
                if (smModule != null)
                {
                    this.siteMapDataSource.SiteMapProvider = smModule.SiteMapProviderName;
                }
            }
        }
    }
}