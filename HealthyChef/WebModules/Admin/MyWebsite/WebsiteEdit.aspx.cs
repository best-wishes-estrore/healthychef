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

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class WebsiteEdit : System.Web.UI.Page
    {
        string _culture = System.Threading.Thread.CurrentThread.CurrentCulture.ToString();
        protected int _rootPageNavigationId;
        /*public int SiteId
        {
            get { return (int)(ViewState["SiteId"] ?? 0); }
            set { ViewState["SiteId"] = value; }
        }*/
        private void showMsg(string msg)
        {
            Msg.Visible = true;
            Msg.InnerHtml = msg;
        }
        Website getWebsiteByRootPageNavigationId()
        {
            //get the website associated with the root webpage.
            //we are assuming a 1-to-1 relationship in the database.
            return Website.Find(null, null, _rootPageNavigationId, null, null).First;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Msg.Visible = false;

            //string q_siteId = Request.QueryString["SiteId"];
            //if (null == q_siteId) LoadList();
            //this.SiteId = int.Parse(q_siteId);

            if (!int.TryParse(Request.QueryString["instanceId"], out _rootPageNavigationId))
                throw new ArgumentException("Invalid instance/navigation id.");

            WebpageInfo page = Webpage.GetWebpage(_rootPageNavigationId);
            if (page.ParentInstanceId != Webpage.RootNavigationId)
                throw new ArgumentException("Invalid instance/navigation id: only a top-level page may be associated with a website.");

            if (!this.Page.IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.Permission.AllowManageSystem())
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                EditItem();
            }
        }
        void EditItem()//Website website)
        {
            Website website = getWebsiteByRootPageNavigationId();

            /*
            //only immediate children of the system root may be website root pages.
            WebpageInfo portalRootPage = Webpage.GetWebpage(Webpage.RootNavigationId);
            RootNavId.DataSource = portalRootPage.Children;
            RootNavId.DataValueField = "InstanceId";
            RootNavId.DataTextField = "Title";
            RootNavId.DataBind();
            RootNavId.Items.Insert(0, new ListItem(String.Empty, "0"));
            */

            if (null != website)//edit existing item.
            {
                WebsiteResource websiteRes = WebsiteResource.Get(_culture, website.SiteId);
                //if (null == website) throw new InvalidOperationException("Website does not exist (it may have been deleted).");
                LoadInputFieldsFromEntity(website);
                //if WebsiteResource is empty, try to get the default.
                if (null == websiteRes)
                {
                    websiteRes = WebsiteResource.GetDefault(website.SiteId);

                    if (null == websiteRes) //if WebsiteResource is still empty, create a stub.
                    {
                        //throw new InvalidOperationException("Website resource does not exist (it may have been deleted).");
                        websiteRes = new WebsiteResource();
                        websiteRes.SiteId = website.SiteId;
                        websiteRes.Culture = _culture;
                        websiteRes.Name = website.SiteId.ToString();
                        websiteRes.Description = string.Empty;
                        websiteRes.Save();
                    }
                    else
                    { //alert the user that the default was loaded.
                        showMsg(string.Format("Data for the current culture ({1}) was not found; the values for the first available culture ({2}) were loaded instead. Saving this form will create culture-specific values for the current culture.", websiteRes.Name, _culture, websiteRes.Culture));
                    }
                }
                //else
                //{
                //    showMsg(string.Format("Editing the <strong>{0}</strong> website.", websiteRes.Name));
                //}

                WebsiteNameCtl.Text = websiteRes.Name;

                LoadInputFieldsFromWebsiteResourceEntity(websiteRes);
            }
            else
            {
                throw new ArgumentException("The specified navigation id is not associated with a website.");
            }
            uxMultiView.SetActiveView(uxView_Edit);
        }
        public Website GetWebsiteEntityFromInputFields()
        {
            Website website = getWebsiteByRootPageNavigationId();// (this.SiteId > 0) ? Website.Get(this.SiteId) : new Website();
            if (null == website) throw new InvalidOperationException("The specified Website does not exist (it may have been deleted).");
            website.DomainExpression = DomainExpression.Text.Trim();
            //website.RootNavigationId = _rootPageNavigationId;
            website.IsActive = IsActive.Checked;
            website.IsDefault = IsDefault.Checked;
            return website;
        }
        public WebsiteResource GetWebsiteResourceEntityFromInputFields(int siteId)
        {
            WebsiteResource websiteRes = WebsiteResource.Get(_culture, siteId);
            if (null == websiteRes) //create new website resource to support existing website
                websiteRes = new WebsiteResource();

            websiteRes.SiteId = siteId;
            websiteRes.Culture = _culture;
            websiteRes.Name = Name.Text.Trim();
            websiteRes.Description = Description.Text.Trim();

            return websiteRes;
        }
        public void LoadInputFieldsFromEntity(Website website)
        {
            DomainExpression.Text = website.DomainExpression;
            //RootNavId.Text = website.RootNavigationId.ToString();
            IsActive.Checked = website.IsActive;
            IsDefault.Checked = website.IsDefault;
        }
        public void LoadInputFieldsFromWebsiteResourceEntity(WebsiteResource websiteRes)
        {
            Name.Text = websiteRes.Name;
            Description.Text = websiteRes.Description;
        }
        protected void uxItemSaveButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            Website website = GetWebsiteEntityFromInputFields();
            website.Save();

            WebsiteResource websiteRes = GetWebsiteResourceEntityFromInputFields(website.SiteId);
            websiteRes.Save();

            //UITool.Clear(uxView_Edit);
            //LoadList();
            //uxMsg.ShowSuccess(String.Format("Saved <strong>{0}</strong>.", ship.Name));
            BayshoreSolutions.WebModules.Cms.Admin.RedirectToMainMenu(_rootPageNavigationId);
        }
        protected void uxItemCancelButton_Click(object sender, EventArgs e)
        {
            //UITool.Clear(uxView_Edit);
            //LoadList();
            //uxMsg.Show("The changes were discarded.");
            BayshoreSolutions.WebModules.Cms.Admin.RedirectToMainMenu(_rootPageNavigationId);
        }

        /*
        void LoadList()
        {
            uxMultiView.SetActiveView(uxView_List);
            WebsiteResourceCollection websiteResC = WebsiteResource.GetByCultureOrDefaultCulture(_culture);
            //sort by website id; as a rule the smallest id is considered the default website.
            websiteResC.Sort(delegate(WebsiteResource w1, WebsiteResource w2) { return w1.SiteId.CompareTo(w2.SiteId); });
            uxList.DataSource = websiteResC;
            uxList.DataBind();
        }
        protected void uxList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            uxList.PageIndex = e.NewPageIndex;
        }
        protected void uxList_PageIndexChanged(object sender, EventArgs e)
        {
            LoadList();
        }
        protected void uxList_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            object commmandArg = e.CommandArgument;

            if (e.CommandName == "Delete_")
            {
                int siteId = int.Parse(commmandArg.ToString());

                if (siteId == Website.Default.SiteId) throw new ArgumentException("Cannot delete the special 'Default' website.");

                Website.Destroy(siteId);
            }

            LoadList();
        }
        protected void uxList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{ 
            //}
        }
        */
    }
}
