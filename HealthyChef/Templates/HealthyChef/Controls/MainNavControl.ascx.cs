using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using Bss = BayshoreSolutions.Common;
using BayshoreSolutions.WebModules;

namespace HealthyChef.Templates.HealthyChef.Controls
{
    public partial class MainNavControl : System.Web.UI.UserControl
    {
      protected override void OnPreRender(EventArgs e)
      {
        base.OnPreRender(e);

        List<SiteMapNode> mySiteMap = new List<SiteMapNode>();
        int myCounter = 0;

        if (TopNavDataSource.Provider.RootNode.HasChildNodes)
        {
          foreach (SiteMapNode myNode in  TopNavDataSource.Provider.RootNode.ChildNodes)
          {
            if (myCounter < 4)
            {
              mySiteMap.Add(myNode);
              myCounter++;
            }
          }

          mySiteMap.Reverse();

          rptMainNav.DataSource = mySiteMap;
          rptMainNav.DataBind();
        }
        else
        {
          rptMainNav.DataSource = TopNavDataSource;
          rptMainNav.DataBind();
        }
                
      }
      
      protected void rptMainNavItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                if (e.Item.ItemIndex >= 4)
                    e.Item.Visible = false;
        }
    }
}
