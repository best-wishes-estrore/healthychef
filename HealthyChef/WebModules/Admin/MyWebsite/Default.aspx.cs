using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BayshoreSolutions.WebModules;
using System.Collections.ObjectModel;

using BayshoreSolutions.WebModules.Security;
using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class _Default : System.Web.UI.Page
    {
        protected int _instanceId;
        protected bool _isSystemRoot = false;

        #region PageSettings stuff is deprecated, please don't follow this pattern.

        protected struct PageSetting
        {
            public string Url;
            public string Type;
            public string Text;
            public bool Enabled;
            public string ToolTip;

            public PageSetting(string url, string text, string type)
            {
                Url = url;
                Type = type;
                Text = text;
                Enabled = true;
                ToolTip = null;
            }

            public PageSetting(string url, string text, string type, bool enabled, string toolTip)
            {
                Url = url;
                Type = type;
                Text = text;
                Enabled = enabled;
                ToolTip = toolTip;
            }
        }

        List<PageSetting> GetPageSettings(WebpageInfo page)
        {
            List<PageSetting> pageSettings = new List<PageSetting>();

            // check IsAlias before checking ExternalUrl (otherwise an alias 
            //_to_ an external link will look like an external link).
            if (page.IsAlias)
            {
                //TODO: add alias settings
                //if (AllowEditPage()) pageSettings.Add(new PageSetting("AliasSettings", "Alias Settings", "Settings"));

                if (!_isSystemRoot && Permission.AllowDeletePage())
                    pageSettings.Add(new PageSetting("PageDelete", "Delete Alias", "Delete"));
            }
            else if (!string.IsNullOrEmpty(page.ExternalUrl))
            { //the navigation instance is an external link.
                if (Permission.AllowManagePage()) pageSettings.Add(new PageSetting("LinkSettings", "Link Settings", "Settings"));
                if (!_isSystemRoot && Permission.AllowDeletePage())
                {
                    bool bEnableDelete = false;
                    if (page.Children.Count == 0)
                    {
                        bEnableDelete = true;
                    }

                    string strDisabledToolTip = "This link cannot be deleted because it contains subpages.  Please first delete all subpages before attempting to delete this link.";
                    pageSettings.Add(new PageSetting("PageDelete", "Delete Link", "Delete", bEnableDelete, strDisabledToolTip));
                }
            }
            else
            {
                if (_isSystemRoot)
                { //the page is the system root.
                    //if (Permission.AllowManageSystem()) pageSettings.Add(new PageSetting("WebsiteEdit", "Manage Websites", "Settings"));
                    //pageSettings.Add(new PageSetting("PagesModulesList", "Manage Modules", "Settings"));
                }
                else
                {
                    if (Permission.AllowManagePage()) pageSettings.Add(new PageSetting("PageSettings", "Page Settings", "Settings"));

                    if (page.ParentInstanceId.HasValue && page.ParentInstanceId.Value == Webpage.RootNavigationId)
                    { //the page is a website root page.
                        pageSettings.Add(new PageSetting("PagesModulesList", "Website Modules", "Settings"));
                        if (Permission.AllowManageSystem()) pageSettings.Add(new PageSetting("WebsiteEdit", "Website Settings", "Settings"));
                    }
                    else
                    { //the page is not a website root page.
                        if (Permission.AllowManagePage()) pageSettings.Add(new PageSetting("PageSecurity", "Page Security", "Security"));

                        bool bEnableDelete = false;

                        if (page.Children.Count == 0)
                        {
                            List<WebModuleInfo> modules = WebModule.GetModules(_instanceId);
                            if (modules.Count == 0)
                            {
                                bEnableDelete = true;
                            }
                        }

                        string strDisabledToolTip = "This page cannot be deleted because it contains modules and/or subpages.  Please first delete all modules and subpages before attempting to delete this page.";
                        if (Permission.AllowDeletePage()) pageSettings.Add(new PageSetting("PageDelete", "Delete Page", "Delete", bEnableDelete, strDisabledToolTip));
                    }
                }
            }

            return pageSettings;
        }

        #endregion

        private void HideSortButtons()
        {
            SubpagesGridView.Columns[2].Visible = false;
            SubpagesGridView.Columns[3].Visible = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // get the current page navigation id
            if (!int.TryParse(Request.QueryString["InstanceId"], out _instanceId))
            {
                _instanceId = WebModulesProfile.Current.StartPageId;

                if (_instanceId <= 0)
                    _instanceId = Webpage.RootNavigationId;
            }

            _isSystemRoot = (_instanceId == Webpage.RootNavigationId);

            //check user permissions.
            if (!NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

            SubpagesDataSource.SelectParameters["parentNavigationId"].DefaultValue = _instanceId.ToString();

            if (!IsPostBack)
            {
                // get the current page instance
                WebpageInfo page = Webpage.GetWebpage(_instanceId);

                if (null == page) throw new ArgumentException(string.Format("Page with instance id '{0}' does not exist.", _instanceId));

                // set page details
                WebpageNameLink.Text = page.Text;
                WebpageUrlLabel.Text = Bss.Web.Html.InsertSoftBreaks(page.Path.TrimStart('~'), 90);
                TitleLabel.Text = page.Title;
                Keywords_p.Visible = (page.MetaKeywords != null && page.MetaKeywords.Trim().Length > 0);
                KeywordsLabel.Text = page.MetaKeywords;
                Description_p.Visible = (page.MetaDescription != null && page.MetaDescription.Trim().Length > 0);
                DescriptionLabel.Text = page.MetaDescription;

                AddPageLink.Visible = Permission.AllowManagePage();
                AddPageLink.NavigateUrl = "AddWebpage.aspx?parentInstanceId=" + _instanceId;
                uxAddLink.Visible = AddPageLink.Visible;
                uxAddLink.NavigateUrl = "AddLink.aspx?parentInstanceId=" + _instanceId;
                AddModuleLink.NavigateUrl = string.Format(
                    "AddModule.aspx?PageId={0}&InstanceId={1}", page.Id, _instanceId);

                //
                // set the UI based on the type of page.
                //
                if (_isSystemRoot)
                { //this is the system root--not an actual page.
                    AddPageLink.Text = "New Website";
                    WebpageNameLink.NavigateUrl = null;
                    WebpageNameLink.CssClass += " disabled";
                    uxAddLink.Visible = false; //it wouldn't make sense to use a link as a website root.
                    PageDetails_div.Visible = false;
                    Modules_div.Visible = false;
                    WebpageUrlLabel.Visible = false;
                    AddModuleLink.Visible = false;
                    HideSortButtons();
                }
                else if (null == page.Website)
                { //the root page is not associated with a website, so any link would be bogus.
                    WebpageNameLink.NavigateUrl = null;
                    WebpageNameLink.CssClass += " disabled";
                    WebpageNameLink.ToolTip = "This page is not associated with a website.";
                }
                else if (page.Website.SiteId != Website.Current.SiteId)
                { //the page's website is different than the current website, therefore we must use a kludge.
                    //DO NOT USE THE 'webmodules_websiteId' QUERY-STRING ANYWHERE ELSE--
                    //WE DO NOT WANT ANOTHER QUERY-STRING DEPENDENCY IN THE CMS.
                    WebpageNameLink.NavigateUrl += page.Path + "?webmodules_websiteId=" + page.Website.SiteId; // "/?NavigationId=" + page.InstanceId;
                }
                else
                { //joy! we are administering the current website.
                    WebpageNameLink.NavigateUrl = page.Path;
                }

                if (!Permission.AllowManagePage())
                {
                    HideSortButtons();
                }

                // check IsAlias before checking ExternalUrl (otherwise an alias 
                //_to_ an external link will look like an external link).
                if (page.IsAlias)
                {
                    AddModuleLink.Visible = false;
                }
                else if (!string.IsNullOrEmpty(page.ExternalUrl))
                { //the navigation instance is an external link.
                    AddModuleLink.Visible = false;
                }

                //bind the settings list.
                PageSettingsList1.DataSource = GetPageSettings(page);
                PageSettingsList1.DataBind();
                PageSettings_div.Visible = (PageSettingsList1.Items.Count > 0);
            }
        }

        protected void SubpagesGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string commandName = e.CommandName;

            if (commandName == "SortUp" || commandName == "SortDown")
            {
                int instanceId = int.Parse(e.CommandArgument.ToString());

                if (commandName == "SortUp")
                    Webpage.SortWebpageUp(instanceId);

                if (commandName == "SortDown")
                    Webpage.SortWebpageDown(instanceId);

                ((GridView)sender).DataBind();
            }
        }

        protected void SubpagesGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                WebpageInfo page = e.Row.DataItem as WebpageInfo;
                //disable the page if the user does not have permissions to it.
                e.Row.Enabled = NavigationRole.IsUserAuthorized(page.InstanceId, Page.User);

                string iconPath = "~/WebModules/Admin/Images/Icons/Small/";
                Image iconImage = (Image)e.Row.Cells[0].FindControl("IconImage");
                if (e.Row.Enabled)
                {
                    if (page.IsAlias || !string.IsNullOrEmpty(page.ExternalUrl))
                        iconImage.ImageUrl = iconPath + "WebpageAlias.png";
                    else
                        iconImage.ImageUrl = iconPath + "Webpage.gif";
                }
                else
                {
                    iconImage.ImageUrl = iconPath + "Security.gif";
                }
            }
        }
    }
}