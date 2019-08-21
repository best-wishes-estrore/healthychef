using System;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Collections;

using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class SummaryDisplay : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected MasterDetailSetting _itemList = null;

        private ArrayList _tagList;
        public ArrayList TagList
        {
            get { return _tagList; }
            set { _tagList = value; }
        }

        public int PageNumber
        {
            get
            {
                if (ViewState["PageNumber"] != null)
                {
                    return Convert.ToInt32(ViewState["PageNumber"]);
                }
                return 0;
            }
            set
            {
                ViewState["PageNumber"] = value;
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PagerRepeater.ItemCommand += PagerRepeater_ItemCommand;
        }

        void InitRss()
        {
            WebpageInfo page = this.WebModuleInfo.Webpage;

            //'categoryName' querystring is required by the RSS library--even though we don't use it--so we just pass a nonsense value.
            RssHyperLink1.NavigateUrl =
                 "~/WebModules/MasterDetail/public/rss/RssFeed.ashx?moduleId=" + this.ModuleId;

            //add a <link> element to <head> to notify browsers of RSS feed.
            HtmlLink rssLink = new HtmlLink();
            rssLink.Attributes["rel"] = "alternate";
            rssLink.Attributes["type"] = "application/rss+xml";
            rssLink.Attributes["title"] = page.Text;
            rssLink.Href = RssHyperLink1.NavigateUrl;
            this.Page.Header.Controls.AddAt(1, rssLink);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            InitRss();
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/MasterDetail/public/css/MasterDetail.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            this.Page.Header.Controls.AddAt(1, cssLink);

            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            _itemList = MasterDetailSetting.FetchByID(this.ModuleId);

            string tagFilter = ddlFilter.SelectedValue;
            if (!string.IsNullOrEmpty(tagFilter) && tagFilter == "All")
            {
                tagFilter = null;
            }
            PagedDataSource pgitems = new PagedDataSource();
            pgitems.AllowPaging = true;
            pgitems.DataSource = MasterDetailList.GetMasterDetailChildrenFiltered(this.ModuleId, true, true, tagFilter, true);
            //			pgitems.DataSource = SPs.MasterDetailGetRecentItems(this.Page.ID.ToString(), 10).GetDataSet();
            pgitems.PageSize = _itemList.ItemsPerPage;
            pgitems.CurrentPageIndex = PageNumber;

            if (pgitems.PageCount > 1)
            {
                PagerRepeater.Visible = true;
                ArrayList pages = new ArrayList();
                for (int i = 0; i < pgitems.PageCount; i++)
                    pages.Add((i + 1).ToString());
                PagerRepeater.DataSource = pages;
                PagerRepeater.DataBind();
            }
            else
            {
                PagerRepeater.Visible = false;
            }

            // Clear taglist
            TagList = new ArrayList();

            SummaryListRepeater.DataSource = pgitems;
            SummaryListRepeater.DataBind();

            if (_itemList.ShowTagFilter && (ddlFilter.Items.Count == 0))
            {
                lblFilter.Visible = true;
                ddlFilter.Visible = true;

                TagList.Sort();
                TagList.Insert(0, "All");
                ddlFilter.DataSource = TagList;
                ddlFilter.DataBind();
            }
        }

        protected void SummaryListRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                WebModuleInfo module = (WebModuleInfo)e.Item.DataItem;
                WebpageInfo page = module.Webpage;
                MasterDetailItem item = MasterDetailItem.GetSafeResource(module.Id);

                if (item != null)
                {
                    if (item.IsFeatured)
                    {
                        HtmlControl divSummary = (HtmlControl)e.Item.FindControl("divSummary");
                        if (divSummary != null) divSummary.Attributes["class"] += " MasterDetail_ListItemFeatured";

                        HtmlControl trFeaturedHeader = (HtmlControl)e.Item.FindControl("trFeaturedHeader");
                        if (trFeaturedHeader != null) trFeaturedHeader.Visible = item.IsFeatured;
                    }

                    Image imgMain = (Image)e.Item.FindControl("imgMain");
                    HtmlTableCell tdImageCol = (HtmlTableCell)e.Item.FindControl("tdImageCol");
                    imgMain.Visible = !string.IsNullOrEmpty(item.ImagePath);
                    if (imgMain.Visible)
                    {
                        imgMain.ImageUrl = ResolveUrl(string.Format("~/Image.ashx?File={0}&Size=100", item.ImagePath));
                    }
                    else
                    {
                        if (!_itemList.ShowImageIfBlank)
                        {
                            tdImageCol.Visible = false;
                        }
                    }

                    if (_itemList.IsPostDateVisible)
                    {
                        Label postDateCtl = (Label)e.Item.FindControl("lblPostDate");
                        if (page.PostDate.HasValue)
                            postDateCtl.Text = page.PostDate.Value.ToLongDateString();
                    }

                    HyperLink hypTitle = (HyperLink)e.Item.FindControl("hypTitle");
                    hypTitle.NavigateUrl = ResolveUrl(page.Path);
                    hypTitle.Text = MasterDetailItem.Chop(page.Text, 150, true);

                    HyperLink hypReadMore = (HyperLink)e.Item.FindControl("hypReadMore");
                    hypReadMore.NavigateUrl = ResolveUrl(page.Path);

                    Literal litSummary = (Literal)e.Item.FindControl("litSummary");
                    litSummary.Text = item.GetSummary(page);

                    if (item.Tags != null)
                    {
                        // Build unique list of tags for all articles in the summary display
                        string[] tagArray = item.Tags.Split(',');
                        foreach (var s in tagArray)
                        {
                            if (!TagList.Contains(s))
                            {
                                TagList.Add(s);
                            }
                        }
                    }
                }
                else
                    e.Item.Visible = false;
            }
        }

        void PagerRepeater_ItemCommand(object source,
                  RepeaterCommandEventArgs e)
        {
            PageNumber = Convert.ToInt32(e.CommandArgument) - 1;
            LoadData();
        }

        protected void PagerRepeater_ItemDataBound(object source, RepeaterItemEventArgs args)
        {
            switch (args.Item.ItemType)
            {
                case ListItemType.Item:
                case ListItemType.AlternatingItem:
                    {
                        int nPageNumber = int.Parse(args.Item.DataItem.ToString()) - 1;

                        System.Web.UI.Control divActivePageNumber = args.Item.FindControl("divActivePageNumber");
                        System.Web.UI.Control divPageNumber = args.Item.FindControl("divPageNumber");

                        if (nPageNumber == PageNumber)
                        {
                            divActivePageNumber.Visible = true;
                            divPageNumber.Visible = false;
                        }
                        else
                        {
                            divActivePageNumber.Visible = false;
                            divPageNumber.Visible = true;
                        }
                    }
                    break;
            }
        }

        protected void Filter_Changed(object sender, EventArgs e)
        {
            PageNumber = 0;
            LoadData();
        }

    }
}