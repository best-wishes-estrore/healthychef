using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.ObjectModel;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Threading;
using System.Globalization;
using System.Net.Mail;

using BayshoreSolutions.WebModules.Security;

namespace BayshoreSolutions.WebModules.ContentModule
{
    public partial class Edit : System.Web.UI.Page
    {
        protected int _moduleId = -1;
        private int _pageInstanceId;
        private string _culture = null;

        public static string FormatDate(DateTime d)
        {
            if (CultureCode.Current == "en-US")
                return d.ToString("MM/dd/yyyy hh:mm:ss tt");
            else
                return d.ToString();
        }

        public static bool AllowApproveContent()
        {
            return HttpContext.Current.User.IsInRole(Role.Administrators);
        }

        private void SendProposedContentNotificationEmail()
        {
            bool isMultiCulture = (CultureCode.Get().Count > 1);
            CultureInfo currentCulture = CultureInfo.CreateSpecificCulture(_culture);
            Settings reviewerEmail = Settings.Get(_default.CONTENT_MODULETYPE, _default.SETTING_REVIEWEREMAIL);
            if (reviewerEmail != null)
            {
                MailMessage email = new MailMessage();
                email.To.Add(reviewerEmail.Value);
                email.Subject = string.Format("[{0}] New content pending approval", Website.Current.Resource.Name);
                email.Body = string.Format(@"New content has been submitted for approval on the {0} website.

To view the pending content, click this link:
{1}
{2}

",
                    Website.Current.Resource.Name,
                    Request.Url,
                    (isMultiCulture ? null : string.Format("then select the '{0}' culture.", currentCulture.NativeName))
                    );
                new SmtpClient().Send(email);
            }
        }

        private void LoadContent(Content content)
        {
            RestoreArchived.Visible = false;

            ContentTextEditor.Text = content.Text;
            ContentModifiedDate.Visible = true;
            ContentModifiedDate.Text = FormatDate(content.Modified);
            Propose.Visible = (content.StatusId == 1 || content.StatusId == 2);
            SaveAndPublish.Visible = Propose.Visible && AllowApproveContent();
        }

        private void InitContent(string listItemText, Content content, bool isBeingEdited)
        {
            ListItem li = new ListItem(listItemText,
                content.ContentVersionId.ToString());

            if (isBeingEdited)
            {
                li.Selected = true;
                LoadContent(content);
            }

            ContentVersions.Items.Insert(0, li);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!int.TryParse(Request.QueryString["ModuleId"], out _moduleId))
                throw new ArgumentException("Invalid module id.");
            if (!int.TryParse(Request.QueryString["InstanceId"], out _pageInstanceId))
                throw new ArgumentException("Invalid module id.");

            _culture = CultureCode.Current;

            if (!Page.IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_pageInstanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                Content activeContent = Content.GetActiveContent(_moduleId, _culture);
                Content pendingContent = Content.GetPendingContent(_moduleId, _culture);
                Collection<Content> history = Content.GetArchivedContent(_moduleId, _culture);

                if (null == activeContent && null == pendingContent)
                {
                    ModifiedDatePanel.Visible = false;
                    HistoryPanel.Visible = false;
                }

                if (null != activeContent)
                    InitContent("Current (Published)", activeContent, (null == pendingContent));

                if (null != pendingContent)
                    InitContent("Pending (Not Published)", pendingContent, true);

                //init archives
                RestoreArchived.Visible = false;
                foreach (Content c in history)
                {
                    ContentVersions.Items.Add(
                        new ListItem("Archived - " + FormatDate(c.Modified),
                            c.ContentVersionId.ToString()));
                }
            }
        }
        private void SaveNewContent(int statusId)
        {
            Content newContent = new Content();
            newContent.ModuleId = _moduleId;
            newContent.Culture = _culture;
            newContent.Text = ContentTextEditor.Text;
            newContent.Modified = DateTime.Now;
            newContent.StatusId = statusId;
            newContent.CreateContent();
        }
        private void ArchiveExistingContent()
        {
            Content content = Content.GetActiveContent(_moduleId, _culture);
            if (content != null)
            {
                content.StatusId = 3;//3=archived
                content.UpdateContent();
            }
        }
        protected void SaveAndPublish_Click(object sender, EventArgs e)
        {
            Content content = null;
            int contentId;
            if (int.TryParse(ContentVersions.SelectedValue, out contentId))
                content = Content.GetContentByContentVersionId(contentId);

            ArchiveExistingContent();

            if (null == content)
            { //create and publish new content.
                SaveNewContent(2);//2=active
            }
            else
            {
                if (content.StatusId == 1)
                { //publish the existing pending content.
                    content.Text = ContentTextEditor.Text;
                    content.Modified = DateTime.Now;
                    content.StatusId = 2;
                    content.UpdateContent();
                }
                else if (content.StatusId == 2)
                    SaveNewContent(2);//2=active
                else //failsafe
                    SaveNewContent(2);//2=active
            }

            leavePage();
        }
        protected void Propose_Click(object sender, EventArgs e)
        {
            Content pendingContent = Content.GetPendingContent(_moduleId, _culture);
            if (null == pendingContent)
            {
                SaveNewContent(1);//1=pending
            }
            else
            {
                pendingContent.Text = ContentTextEditor.Text;
                pendingContent.Modified = DateTime.Now;
                pendingContent.UpdateContent();
            }

            SendProposedContentNotificationEmail();

            leavePage();
        }
        protected void Restore_Click(object sender, EventArgs e)
        {
            ArchiveExistingContent();

            //publish the selected archived content
            Content newContent = Content.GetContentByContentVersionId(
                int.Parse(ContentVersions.SelectedValue));
            newContent.Modified = DateTime.Now;
            newContent.StatusId = 2;//2=active

            newContent.CreateContent();

            leavePage();
        }
        protected void Cancel_Click(object sender, EventArgs e)
        {
            leavePage();
        }
        protected void ContentVersions_Change(object sender, EventArgs e)
        {
            Content content = Content.GetContentByContentVersionId(
                int.Parse(ContentVersions.SelectedValue));

            if (content.StatusId == 3) //archived content
            {
                ContentTextEditor.Text = content.Text;
                ContentModifiedDate.Text = content.Modified.ToString();

                SaveAndPublish.Visible = false;
                Propose.Visible = false;
                RestoreArchived.Visible = AllowApproveContent();
            }
            else
                LoadContent(content);
        }
        private void leavePage()
        {
            Cms.Admin.RedirectToMainMenu(_pageInstanceId);
        }

        protected void PreviewPendingButton_Click(object sender, EventArgs e)
        {
            WebpageInfo page = Webpage.GetWebpage(_pageInstanceId);

            Content previewContent = new Content();
            previewContent.ModuleId = _moduleId;
            previewContent.Culture = _culture;
            previewContent.Text = ContentTextEditor.Text;
            previewContent.Modified = DateTime.Now;
            previewContent.StatusId = 2;

            if (null != Session)
                Session["BayshoreSolutions.WebModules.ContentModule.PreviewContent"] = previewContent;

            Page.ClientScript.RegisterStartupScript(this.GetType(),
                "",
                string.Format("window.open('{0}');", ResolveUrl(page.Path)),
                true);
        }
    }
}
