using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using wm = BayshoreSolutions.WebModules;
using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.Admin.WebsiteSettings
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<wm.WebApplicationType> appTypes =
                    wm.WebApplicationType.GetApplications().FindAll(
                        delegate(wm.WebApplicationType a) { return a.HasSettings; });
                ApplicationsList.DataSource = appTypes;
                ApplicationsList.DataBind();
            }
        }

        protected void ClearCacheButton_Click(object sender, EventArgs e)
        {
            BayshoreSolutions.WebModules.Cache.Clear();
            BayshoreSolutions.WebModules.WebApplicationType.LoadApplications();
        }

        protected void ApplicationsList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                wm.WebApplicationType appType = (wm.WebApplicationType)e.Item.DataItem;
                Repeater modulesList = (Repeater)e.Item.FindControl("ModulesList");
                List<wm.WebModuleType> modules = wm.WebModuleType.GetModuleTypes(appType.Name);

                if (null == modulesList) throw new MissingMemberException("ModulesList control not found.");

                if (modules != null && modules.Count > 0)
                {
                    modulesList.DataSource = modules;
                    modulesList.DataBind();
                }
                else
                    modulesList.Visible = false;
            }
        }

        protected void ToggleAdminButtons_Click(object sender, EventArgs e)
        {
            WebModulesProfile.Current.EnableCmsButtons = !WebModulesProfile.Current.EnableCmsButtons;
            WebModulesProfile.Current.Save();

            Msg.ShowSuccess(string.Format("Front-end CMS buttons have been {0}.",
                WebModulesProfile.Current.EnableCmsButtons ? "enabled" : "disabled"));
        }
    }
}