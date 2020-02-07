using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using BayshoreSolutions.WebModules.Admin.MyWebsite;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class MiniSummaryDisplay : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/MasterDetail/public/css/MasterDetail.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            this.Page.Header.Controls.AddAt(1, cssLink);

            // Add javscript reference to page
            this.Page.ClientScript.RegisterClientScriptInclude("masterdetail.js", ResolveUrl("~/WebModules/MasterDetail/public/js/masterdetail.js"));

            if (!IsPostBack)
            {
                //int instanceId;
                //int.TryParse(Request.QueryString["instanceId"], out instanceId);
                WebpageInfo startingPage;
                MasterDetailMiniSummarySetting settings = MasterDetailMiniSummarySetting.FetchByID(this.ModuleId);
                if ((settings != null) && (settings.StartingPageId != null))
                {
                    startingPage = Webpage.GetWebpage((int)settings.StartingPageId);
                }
                else
                {
                    WebpageInfo currentPage = Webpage.GetWebpage(Request.Url);
                    startingPage = currentPage.Website.RootWebpage;
                }

                List<WebpageInfo> webpages = Webpage.GetDescendants(startingPage);
                List<WebpageInfo> pagesWithThisModule = GetPagesWithModuleType("Master Detail Item", webpages);

                StringBuilder pageIdList = new StringBuilder();
                foreach (WebpageInfo page in pagesWithThisModule)
                {
                    if (page.PostDate > DateTime.Now) continue;
                    if ((page.RemoveDate != null) && (page.RemoveDate < DateTime.Now)) continue;

                    if (pageIdList.Length == 0)
                    {
                        pageIdList.Append(page.Id);
                    }
                    else
                    {
                        pageIdList.Append("," + page.Id);
                    }
                    // Maximum length of pageIdList for stored proc is 8000
                    if (pageIdList.Length > 7990) break;
                }

                if (pageIdList.Length > 0)
                {
                    if (settings != null)
                    {
                        MiniSummaryRepeater.DataSource =
                            SPs.MasterDetailGetRecentItems(pageIdList.ToString(), settings.FeaturedOnly, settings.NumRows).GetDataSet();
                    }
                    else
                    {
                        MiniSummaryRepeater.DataSource =
                            SPs.MasterDetailGetRecentItems(pageIdList.ToString(), false, 10).GetDataSet();
                    }
                    MiniSummaryRepeater.DataBind();
                }
            }
        }

        protected void MiniSummaryRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {

                Label lblCategory = e.Item.FindControl("lblCategory") as Label;
                if (lblCategory != null)
                {
                    int pageId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "PageId"));
                    WebpageInfo parentPageInfo = Webpage.GetWebpageByPageId(pageId);
                    if (parentPageInfo != null)
                    {
                        lblCategory.Text = parentPageInfo.Parent.Text + ":&nbsp;&nbsp;";
                    }

                    HyperLink hypTitle = e.Item.FindControl("hypTitle") as HyperLink;
                    if (hypTitle != null)
                    {
                        hypTitle.NavigateUrl = DataBinder.Eval(e.Item.DataItem, "Path").ToString();

                        string scriptStr = "autoEllipseText('" + hypTitle.ClientID + "', \""
                                    + (DataBinder.Eval(e.Item.DataItem, "NavigationText")).ToString().Replace("\"", "&quot;") + "\", '"
                                    + lblCategory.ClientID + "');";
                        Page.ClientScript.RegisterStartupScript(GetType(), "truncate" + e.Item.ItemIndex, scriptStr, true);
                    }

                }

                // TODO: Cache this info
                int moduleId = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "ModuleId"));
                MasterDetailItem item = MasterDetailItem.GetSafeResource(moduleId);
                MasterDetailSetting setting = item.GetContentList();
                MasterDetailMiniSummarySetting miniSummarySetting = MasterDetailMiniSummarySetting.FetchByID(this.ModuleId);

                Label lblCommentCount = e.Item.FindControl("lblCommentCount") as Label;
                if (lblCommentCount != null)
                {
                    if ((setting != null) && (setting.AllowComments))
                    {
                        int numComments =
                            MasterDetailComment.Query().WHERE(MasterDetailComment.Columns.ModuleId, moduleId).GetCount(
                                MasterDetailComment.Columns.ModuleId);
                        if (numComments > 999) numComments = 999; // max display size is 3 digits.

                        lblCommentCount.Text = numComments.ToString();
                        lblCommentCount.ToolTip = numComments + " Reader Comments";
                    }
                    else
                    {
                        lblCommentCount.Visible = false;
                    }
                }

                Label lblTimestamp = e.Item.FindControl("lblTimestamp") as Label;
                if (lblTimestamp != null)
                {
                    if ((miniSummarySetting != null) && ((bool)miniSummarySetting.ShowElapsedTime))
                    {
                        DateTime postDate = Convert.ToDateTime(DataBinder.Eval(e.Item.DataItem, "PostDate"));
                        TimeSpan ts = DateTime.Now.Subtract(postDate);
                        if (ts.Days > 0)
                        {
                            lblTimestamp.Text = ts.Days + "d ago";
                        }
                        else if (ts.Hours > 0)
                        {
                            lblTimestamp.Text = ts.Hours + "h ago";
                        }
                        else if (ts.Minutes > 0)
                        {
                            lblTimestamp.Text = ts.Minutes + "m ago";
                        }
                        else
                        {
                            lblTimestamp.Text = "< 1m ago";
                        }
                    }
                    else
                    {
                        lblTimestamp.Visible = false;
                    }
                }

            }
        }

        protected List<WebpageInfo> GetPagesWithModuleType(string moduleName, List<WebpageInfo> pages)
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
                                return module.WebModuleType.Name.Equals(moduleName);
                        });
                });

            //sort by path
            pagesWithModuleType.Sort(delegate(WebpageInfo p1, WebpageInfo p2)
            {
                return p1.Path.CompareTo(p2.Path);
            });

            return pagesWithModuleType;
        }

    }
}