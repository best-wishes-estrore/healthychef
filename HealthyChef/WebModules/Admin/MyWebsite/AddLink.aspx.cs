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

    public partial class AddLink : System.Web.UI.Page
    {
        int _parentNavigationId = -1;

        protected void Page_Load(object sender, EventArgs e)
        {
            Form.DefaultButton = uxCreateButton.UniqueID;

            if (string.IsNullOrEmpty(Request.QueryString["ParentInstanceID"])) throw new ArgumentNullException("ParentInstanceID");
            _parentNavigationId = int.Parse(Request.QueryString["ParentInstanceID"]);

            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_parentNavigationId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                uxView.SetActiveView(uxExternalLinkView);
                uxIsAlias.Checked = false;

                Page.SetFocus(uxTitle.ClientID);
            }
        }
        private Webpage.WebpageCreateStatus createInternalAlias()
        {
            Webpage.WebpageCreateStatus status;
            int pageNavId = PagePicker1.SelectedNavigationId;
            WebpageInfo p = Webpage.GetWebpage(pageNavId);
            if (null == p) throw new ArgumentException("Invalid navigation id (page not found).");

            //an "alias" adds a record to WebModules_Navigation, but not WebModules_Pages.
            WebpageInfo alias = Webpage.CreateAlias(p.Id, this._parentNavigationId, 0, uxShowInNavigation.Checked, uxTitle.Text, uxRedirect.Checked, out status);

            if (status == Webpage.WebpageCreateStatus.Success)
                Response.Redirect("Default.aspx?instanceId=" + alias.InstanceId);

            return status;
        }
        private Webpage.WebpageCreateStatus createAbsoluteLink()
        {
            Webpage.WebpageCreateStatus status;

            //a link is really a Webpage with an external link; thus,
            //it adds a record to both WebModules_Navigation and WebModules_Pages.
            WebpageInfo link = Webpage.CreateWebpage(uxTitle.Text,
                this._parentNavigationId,
                uxAbsoluteUrl.Text,
                uxShowInNavigation.Checked,
                out status);

            if (status == Webpage.WebpageCreateStatus.Success)
                Response.Redirect("Default.aspx?instanceId=" + link.InstanceId);

            return status;
        }
        protected void uxCreateButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            Webpage.WebpageCreateStatus status = uxIsAlias.Checked
                ? createInternalAlias()
                : createAbsoluteLink();

            switch (status)
            {
                case Webpage.WebpageCreateStatus.DuplicateName:
                    Msg.ShowError(string.Format("Failed to save link. Path name '{0}' already exists.", uxTitle.Text));
                    break;
                case Webpage.WebpageCreateStatus.IllegalName:
                    Msg.ShowError(string.Format("Failed to save link. Path name '{0}' is not allowed.", uxTitle.Text));
                    break;
                case Webpage.WebpageCreateStatus.Success:
                    break;
                case Webpage.WebpageCreateStatus.None:
                default:
                    Msg.ShowError("Error.");
                    break;
            }
        }
        protected void uxCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?InstanceID=" + this._parentNavigationId);
        }

        protected void uxIsAlias_CheckedChanged(object sender, EventArgs e)
        {
            if (uxIsAlias.Checked)
                uxView.SetActiveView(uxInternalAliasView);
            else
                uxView.SetActiveView(uxExternalLinkView);
        }
    }
}