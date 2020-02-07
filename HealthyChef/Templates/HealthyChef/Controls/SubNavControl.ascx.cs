using System;
using System.Collections.Generic;
using System.Web;
using BayshoreSolutions.WebModules;

namespace HealthyChef.Templates.HealthyChef.Controls
{
    public partial class SubNavControl : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                WebpageBase page = this.Page as WebpageBase;
                if (null != page && null != page.WebpageInfo)
                {
                    if (SiteMap.CurrentNode != null
                        //removed (current page may be invisible, but menu should still show children). jkeyes 20080204.
                        //&& !page.WebpageInfo.Visible
                        )
                    { // get the visible child pages
                        List<WebpageInfo> visibleChildren = page.WebpageInfo.Children.FindAll(
                            delegate(WebpageInfo p) { return p.Visible; });
                        if (visibleChildren.Count == 0)
                        {
                            if (page.WebpageInfo.ParentInstanceId
                                != Website.Current.RootNavigationId)
                            { //show siblings instead of nothing
                                SubNavDataSource.StartingNodeOffset = -1;
                            }
                        }
                    }
                }
            }
        }
    }
}