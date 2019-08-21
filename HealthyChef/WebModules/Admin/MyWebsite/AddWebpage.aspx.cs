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
using System.Text;

using BayshoreSolutions.WebModules;

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class AddWebpage : System.Web.UI.Page
    {
        int _parentNavId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            const string PARENT_PARAM_NAME = "ParentInstanceID";
            _parentNavId = int.Parse(Request.QueryString[PARENT_PARAM_NAME]);

            if (!IsPostBack)
            {
                WebpageInfo parentPage = Webpage.GetWebpage(_parentNavId);
                if (null == parentPage) throw new ArgumentException(string.Format("Invalid {0}.", PARENT_PARAM_NAME));

                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_parentNavId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                if (_parentNavId == Webpage.RootNavigationId) //system root
                { //root page of a new website, located under the system root.
                    HeaderCtl.InnerHtml = "New Website";
                    DisplayCheckBox.Visible = false;
                }

                //
                //bind templates list
                //
                TemplateDropDownList.DataSource = BayshoreSolutions.WebModules.WebDirectories.GetTemplates();
                TemplateDropDownList.DataBind();
                string defaultTemplate = parentPage.Website.RootWebpage.TemplateGroup + " - Subpage";
                //if(TemplateDropDownList.Items.FindByValue(
                //default to the "subpage" template for the convenience of the user.
                TemplateDropDownList.SelectedValue = defaultTemplate;

                //
                //bind themes list
                //
                ThemeDropDownList.DataSource = BayshoreSolutions.WebModules.WebDirectories.GetThemes();
                ThemeDropDownList.DataBind();

                Page.SetFocus(TitleTextBox.ClientID);
            }
        }

        protected void CreateButton_Click(object sender, EventArgs e)
        {
            string TemplateGroup = TemplateDropDownList.SelectedValue.Replace(" - ", "-").Split('-')[0];
            string Template = TemplateDropDownList.SelectedValue.Replace(" - ", "-").Split('-')[1];
            string Theme = null;

            if (ThemeDropDownList.SelectedValue != string.Empty)
            {
                Theme = ThemeDropDownList.SelectedValue;
            }

            string title = TitleTextBox.Text;
            string keywords = KeywordsTextBox.Text;
            string description = DescriptionTextBox.Text;

            bool DisplayInNavigation = DisplayCheckBox.Checked;

            Webpage.WebpageCreateStatus status;
            WebpageInfo newPage = Webpage.CreateWebpage(TemplateGroup, Template, Theme, title, keywords, description, description, _parentNavId, DisplayInNavigation,
                out status);

            switch (status)
            {
                case Webpage.WebpageCreateStatus.DuplicateName:
                    Msg.ShowError(string.Format("Failed to save page. Path name '{0}' already exists.", TitleTextBox.Text));
                    break;
                case Webpage.WebpageCreateStatus.IllegalName:
                    Msg.ShowError(string.Format("Failed to save page. Path name '{0}' is not allowed.", TitleTextBox.Text));
                    break;
                case Webpage.WebpageCreateStatus.Success:
                    CreateWebsiteIfRootPage(newPage);
                    Response.Redirect(string.Format("AddModule.aspx?ParentInstanceId={0}&PageId={1}&InstanceId={2}",
                        _parentNavId,
                        newPage.Id,
                        newPage.InstanceId));
                    break;
                case Webpage.WebpageCreateStatus.None:
                default:
                    Msg.ShowError("Error.");
                    break;
            }
        }

        private void CreateWebsiteIfRootPage(WebpageInfo page)
        {
            bool isRootPage = (page.ParentInstanceId == Webpage.RootNavigationId);
            if (isRootPage)
            { //create a 1-to-1 associated website for the new root webpage.
                Website website = new Website();
                website.DomainExpression = string.Empty;
                website.RootNavigationId = page.InstanceId;
                website.Save();

                WebsiteResource websiteRes = new WebsiteResource();
                websiteRes.SiteId = website.SiteId;
                websiteRes.Culture = CultureCode.Current;
                websiteRes.Name = page.Title;
                websiteRes.Description = string.Empty;
                websiteRes.Save();
            }
        }

        protected void InsertCancelButton_Click(object sender, EventArgs e)
        {
            BayshoreSolutions.WebModules.Cms.Admin.RedirectToMainMenu(_parentNavId);
        }
    }
}