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

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class PagesModulesList : System.Web.UI.Page
    {
        List<WebpageInfo> _webpages = null;
        int _instanceId;

        public int WebsiteId
        {
            get { return (int)(ViewState["WebsiteId"] ?? -1); }
            set { ViewState["WebsiteId"] = value; }
        }

        private void LoadPagesModules(int websiteId)
        {
            //get all webpages in the selected website.
            Website website = Website.Get(websiteId);
            WebpageInfo websiteRootPage = website.RootWebpage;
            _webpages = Webpage.GetDescendants(websiteRootPage);
            _webpages.Insert(0, websiteRootPage);

            //List<WebModuleType> moduleTypes = WebModuleType.GetModuleTypes();

            List<WebApplicationType> webappTypes = WebApplicationType.GetApplications();
            //sort alphabetically
            webappTypes.Sort(delegate(WebApplicationType app1, WebApplicationType app2)
                { return app1.Name.CompareTo(app2.Name); });

            //get applications that have >=1 addable module.
            WebAppsList.DataSource = webappTypes.FindAll(delegate(WebApplicationType app)
                {
                    foreach (WebModuleType m in app.Modules.Values)
                        if (m.CanAddModule) return true;
                    return false;
                });
            WebAppsList.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int.TryParse(Request.QueryString["instanceId"], out _instanceId);
            WebpageInfo websiteRootPage = Webpage.GetWebpage(_instanceId);

            if (!websiteRootPage.ParentInstanceId.HasValue
                || websiteRootPage.ParentInstanceId.Value != Webpage.RootNavigationId)
                throw new InvalidOperationException();

            if (!IsPostBack)
            {
                this.WebsiteId = websiteRootPage.Website.SiteId;
                LoadPagesModules(this.WebsiteId);

                WebsiteNameCtl.Text = websiteRootPage.Website.Resource.Name;
            }
        }

        protected void WebAppsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                WebApplicationType webappType = (WebApplicationType)e.Item.DataItem;
                Repeater modulesList = (Repeater)e.Item.FindControl("ModulesList");
                modulesList.ItemDataBound += new RepeaterItemEventHandler(modulesList_ItemDataBound);
                modulesList.DataSource = WebApplicationType.Items[webappType.Name].Modules.Values;
                modulesList.DataBind();
            }
        }

        public static List<WebpageInfo> GetPagesWithModuleType(
            WebModuleType moduleType,
            List<WebpageInfo> pages)
        {
            List<WebpageInfo> pagesWithModuleType = pages.FindAll(
                delegate(WebpageInfo p)
                {
                    return p.Modules.Exists(
                        delegate(WebModuleInfo module)
                        {
                            if (//module.WebModuleType might be null if the cache is out of sync with the database.
                                null == module.WebModuleType
                                || module.IsAlias //exclude aliases.
                                )
                                return false;
                            else
                                return module.WebModuleType.Equals(moduleType);
                        });
                });

            //sort by path
            pagesWithModuleType.Sort(delegate(WebpageInfo p1, WebpageInfo p2)
            {
                return p1.Path.CompareTo(p2.Path);
            });

            return pagesWithModuleType;
        }

        void modulesList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                WebModuleType moduleType = (WebModuleType)e.Item.DataItem;
                Repeater pagesList = (Repeater)e.Item.FindControl("PagesList");
                HtmlGenericControl notShownMsgCtl = (HtmlGenericControl)e.Item.FindControl("NotShownMsgCtl");
                HtmlGenericControl noPagesMsgCtl = (HtmlGenericControl)e.Item.FindControl("NoPagesMsgCtl");
                int pagesCount = -1;

                //maximum number of module instances to show for each module.
                //this is a compromise to deal with e.g. the ContentModule, which
                //could have hundreds of instances.
                int maxModuleInstances = int.Parse(MaxModuleInstancesSelect.Text);

                List<WebpageInfo> pagesWithThisModule = GetPagesWithModuleType(moduleType, _webpages);

                pagesCount = pagesWithThisModule.Count;

                //sort by path
                pagesWithThisModule.Sort(delegate(WebpageInfo p1, WebpageInfo p2)
                    { return p1.Path.CompareTo(p2.Path); });

                pagesList.DataSource = (pagesCount > maxModuleInstances)
                    ? pagesWithThisModule.GetRange(0, maxModuleInstances)
                    : pagesWithThisModule;
                pagesList.DataBind();

                if (notShownMsgCtl.Visible = (pagesCount > maxModuleInstances))
                {
                    notShownMsgCtl.InnerHtml = string.Format("<strong>{0}</strong> results not shown.",
                        pagesCount - maxModuleInstances);
                }

                noPagesMsgCtl.Visible = (pagesCount <= 0);

                //modulesList.ItemDataBound += new RepeaterItemEventHandler(modulesList_ItemDataBound);
            }
        }

        protected void MaxModuleInstancesSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadPagesModules(this.WebsiteId);
        }

        //protected void WebsitePicker1_OnWebsiteSelected(object sender, System.EventArgs e)
        //{
        //    LoadPagesModules(WebsitePicker1.SelectedWebsiteId);
        //}
    }
}
