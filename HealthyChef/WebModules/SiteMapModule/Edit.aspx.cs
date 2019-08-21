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
    public partial class edit : System.Web.UI.Page
    {
        private int _moduleId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["ModuleId"], out _moduleId))
                LeavePage();

            if (!IsPostBack)
            {
                foreach (System.Web.SiteMapProvider provider in System.Web.SiteMap.Providers)
                {
                    this.siteMapProviderName.Items.Add(provider.Name);
                }
                model.SiteMapModule smModule = model.SiteMapModule.Get(_moduleId);
                if (smModule != null)
                {
                    this.siteMapProviderName.Text = smModule.SiteMapProviderName;
                }
            }
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            LeavePage();
        }
        private void LeavePage()
        {
            int instanceId;
            WebModuleInfo module = WebModule.GetModule(_moduleId);
            if (_moduleId == 0 || null == module || null == module.Webpage)
            {
                instanceId = Webpage.RootNavigationId;
            }
            else
            {
                instanceId = module.Webpage.InstanceId;
            }
            Response.Redirect("~/WebModules/Admin/MyWebsite/Default.aspx?InstanceId=" + instanceId);
        }
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            model.SiteMapModule smModule = new model.SiteMapModule();
            //because the PK is manual, this will create-new or update-existing as appropriate.
            smModule.ModuleId = _moduleId;
            smModule.SiteMapProviderName = siteMapProviderName.Text;
            smModule.Save();
            LeavePage();
        }
    }
}
