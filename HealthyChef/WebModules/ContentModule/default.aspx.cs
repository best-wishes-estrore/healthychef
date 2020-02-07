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

namespace BayshoreSolutions.WebModules.ContentModule
{
    public partial class _default : System.Web.UI.Page
    {
        public const string CONTENT_MODULETYPE = "ContentModule";
        public const string SETTING_REVIEWEREMAIL = "PendingContentEmail";

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.DefaultButton = SaveSettings.UniqueID;

            if (!IsPostBack)
            {
                Settings oldEmail = Settings.Get(CONTENT_MODULETYPE, SETTING_REVIEWEREMAIL);
                if (oldEmail != null)
                    emailAddress.Text = oldEmail.Value;
            }
        }

        protected void SaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Save(CONTENT_MODULETYPE, SETTING_REVIEWEREMAIL, emailAddress.Text);

            Response.Redirect("~/webmodules/Admin/WebsiteSettings/Default.aspx");
        }
    }
}
