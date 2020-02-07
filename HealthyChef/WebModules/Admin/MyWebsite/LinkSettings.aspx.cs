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

namespace BayshoreSolutions.WebModules.Admin.MyWebsite
{
    public partial class LinkSettings : System.Web.UI.Page
    {
        string _urlReferrer = "Default.aspx";
        private int _instanceId = 0;
        WebpageInfo _link = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.DefaultButton = UpdateButton.UniqueID;

            if (Request.QueryString["InstanceId"] != null)
            {
                _instanceId = int.Parse(Request.QueryString["InstanceId"]);
                _urlReferrer += "?InstanceId=" + _instanceId;
                _link = Webpage.GetWebpage(_instanceId);
            }
            else
            {
                Response.Redirect(_urlReferrer);
            }

            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                if (_link == null) Response.Redirect(_urlReferrer);

                TextText.Text = _link.Text;
                PathNameEditCtl.PathName = _link.PathName;
                TargetUrlText.Text = _link.ExternalUrl;
                DisplayLinkInNavCheckBox.Checked = _link.Visible;
            }
        }

        protected void UpdateCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect(_urlReferrer);
        }

        protected void UpdateButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            //prevent malicious edits
            if (_instanceId == Webpage.RootNavigationId)
                throw new InvalidOperationException("The portal root may not be edited through this form.");
            if (_link.ParentInstanceId == Webpage.RootNavigationId)
                throw new InvalidOperationException("Root pages (website roots) may not be edited through this form.");

            _link.InstanceId = _instanceId;
            _link.Text = TextText.Text;
            //'Title' has no meaning for an external link, so just make it the same as the navigation text.
            _link.Title = _link.Text;
            _link.PathName = PathNameEditCtl.PathName;//sanitized internally by WebModules.
            _link.ExternalUrl = TargetUrlText.Text;
            _link.Visible = DisplayLinkInNavCheckBox.Checked;

            if (!_link.Visible)
            { //reset sort order so that invisible links may sort alphabetically.
                _link.SortOrder = 1;
            }

            Webpage.WebpageCreateStatus status = Webpage.UpdateWebpage(_link);
            switch (status)
            {
                case Webpage.WebpageCreateStatus.DuplicateName:
                    Msg.ShowError(string.Format("Failed to save link settings. Path name '{0}' already exists.", PathNameEditCtl.PathName));
                    break;
                case Webpage.WebpageCreateStatus.IllegalName:
                    Msg.ShowError(string.Format("Failed to save link settings. Path name '{0}' is not allowed.", PathNameEditCtl.PathName));
                    break;
                case Webpage.WebpageCreateStatus.Success:
                    Response.Redirect(_urlReferrer);
                    break;
                case Webpage.WebpageCreateStatus.None:
                default:
                    Msg.ShowError("Error.");
                    break;
            }
        }
    }
}