using System;
using System.Threading;
using System.Web;
using System.Web.UI.HtmlControls;

/***************************
 * This implementation of the ContentArea control uses a javascript/iframe implementation
 * to popup the content editor dialog.  
 * 
 * The popup window functionality was originally attempted using the infamous AJAX
 * ModalPopupExtender.  There were two issues that prevented the use of this control.
 * 
 * First, because the FCKEditor accepts HTML input, it was required to set the 
 * ValidateRequest Page attribute to false.  This would have to be added to every page
 * that used the ContentArea control and would potentially open up pages to scripting
 * attacks.
 * 
 * Secondly, the version and culture dropdownlists required postbacks to reload the 
 * selected content.  This proved to be problematic with the ModalPopupExtender
 * and was abandoned.
 * (rread 11/5/09)
 ***************************/

namespace BayshoreSolutions.WebModules.QuickContent
{
    public partial class ContentArea : System.Web.UI.UserControl
    {
        const string ADMINISTRATOR = "Administrators";
        const string CONTENT_EDITOR_ROLE = "Content Authors";
        public bool UserCanEdit()
        {
            bool canEdit = (HttpContext.Current.User.IsInRole(ADMINISTRATOR) || HttpContext.Current.User.IsInRole(CONTENT_EDITOR_ROLE));
            return canEdit;
        }

        private string _contentName;
        public string ContentName
        {
            get { return _contentName; }
            set { _contentName = value; }
        }

        public string Title = "";
        protected string ContentText = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadActiveContent();
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            if (UserCanEdit())
            {
                placeholderEdit.Visible = true;
                placeholderView.Visible = false;
                LoadCss();

                this.Page.ClientScript.RegisterClientScriptInclude("submodal", ResolveUrl("~/WebModules/QuickContent/public/js/subModal.js"));
                this.Page.ClientScript.RegisterClientScriptInclude("quick-content", ResolveUrl("~/WebModules/QuickContent/public/js/quickContent.js"));
            }
            else
            {
                placeholderEdit.Visible = false;
                placeholderView.Visible = true;
            }
        }

        // Load CSS and prevent duplicates
        internal void LoadCss()
        {
            //prevent loading multiple css style sheet
            HtmlControl css = null;
            css = Page.Header.FindControl("PopupEditorCSS") as HtmlControl;

            if (css == null)
            {
                //load the style sheet
                HtmlLink cssLinkSubModal = new HtmlLink();
                cssLinkSubModal.ID = "PopupEditorCSS";
                cssLinkSubModal.Href = "~/WebModules/QuickContent/public/css/subModal.css";
                cssLinkSubModal.Attributes["rel"] = "stylesheet";
                cssLinkSubModal.Attributes["type"] = "text/css";

                // Add the HtmlLink to the Head section of the page.
                Page.Header.Controls.Add(cssLinkSubModal);

                //load the style sheet
                HtmlLink cssLink = new HtmlLink();
                cssLink.ID = "QuickContentCSS";
                cssLink.Href = "~/WebModules/QuickContent/public/css/quickContent.css";
                cssLink.Attributes["rel"] = "stylesheet";
                cssLink.Attributes["type"] = "text/css";

                // Add the HtmlLink to the Head section of the page.
                Page.Header.Controls.Add(cssLink);

            }
        }

        protected string GetCMSUrl()
        {
            return GetBaseUrl() + "WebModules/QuickContent/PopupEditor.aspx?id=" + ContentName;
        }

        public string GetBaseUrl()
        {
            string Port = Request.ServerVariables["SERVER_PORT"];
            if (Port == null || Port == "80" || Port == "443")
                Port = "";
            else
                Port = ":" + Port;

            string Protocol = Request.ServerVariables["SERVER_PORT_SECURE"];
            if (Protocol == null || Protocol == "0")
                Protocol = "http://";
            else
                Protocol = "https://";

            // *** Figure out the base Url which points at the application's root
            string strBaseUrl = Protocol + Request.ServerVariables["SERVER_NAME"] + Port + Request.ApplicationPath;
            if (!strBaseUrl.EndsWith("/"))
            {
                strBaseUrl += "/";
            }
            return strBaseUrl;
        }

        private void LoadActiveContent()
        {
            if (!string.IsNullOrEmpty(ContentName))
            {
                QuickContentContent text = GetActiveContent();
                if (text.IsLoaded)
                {
                    ContentText = text.Body;
                }
            }
            else
            {
                ContentText = "Missing ContentName attribute in the ContentArea user control";
            }

            if (UserCanEdit())
            {
                if (ContentText.Trim().Length == 0)
                {
                    literalContentEdit.Text = "Double-click to add text...";
                }
                else
                {
                    literalContentEdit.Text = ContentText;
                }
            }
            else
            {
                literalContentView.Text = ContentText;
            }

        }
        public QuickContentContent GetActiveContent()
        {
            if (!string.IsNullOrEmpty(ContentName))
            {
                QuickContentContentCollection coll = new QuickContentContentCollection()
                    .Where(QuickContentContent.Columns.ContentName, ContentName)
                    .Where(QuickContentContent.Columns.Culture, Thread.CurrentThread.CurrentUICulture.Name.ToLower())
                    .Where(QuickContentContent.Columns.StatusId, 2)
                    .Load();

                if (coll.Count > 0)
                {
                    return coll[0];
                }
            }
            return new QuickContentContent();
        }
    }
}
