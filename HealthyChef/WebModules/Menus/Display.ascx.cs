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

namespace HealthyChef.WebModules.Menus
{
    public partial class Display : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            testMessage.Text = string.Format("ModuleId={0}</br>Module Name={1}", this.ModuleId, (this.WebModuleInfo != null) ? this.WebModuleInfo.Name : "not set");
        }
    }
}