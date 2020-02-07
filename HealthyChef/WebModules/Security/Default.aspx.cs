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
using BayshoreSolutions.WebModules.Cms.Security.Model;

namespace BayshoreSolutions.WebModules.Security
{
    public partial class Default : System.Web.UI.Page
    {
        void Init_()
        {
            //
            //general email settings for security-related notifications.
            //
            EmailFromAddress.Text = SecuritySettings.EmailFromAddress;
            EmailBodyHeader.Text = SecuritySettings.EmailBodyHeader;
            EmailBodyFooter.Text = SecuritySettings.EmailBodyFooter;

            //
            //initial user registration settings
            //
            UserRegEmailSubject.Text = SecuritySettings.UserRegistrationEmailSubject;
            UserRegEmailBody.Text = SecuritySettings.UserRegistrationEmailBody;

            //
            //password recovery settings
            //
            PasswordRecoveryEmailSubject.Text = SecuritySettings.PasswordRecoveryEmailSubject;
            PasswordRecoveryEmailBody.Text = SecuritySettings.PasswordRecoveryEmailBody;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init_();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            //
            //general email settings for security-related notifications.
            //
            SecuritySettings.EmailFromAddress = EmailFromAddress.Text;
            SecuritySettings.EmailBodyHeader = EmailBodyHeader.Text;
            SecuritySettings.EmailBodyFooter = EmailBodyFooter.Text;

            //
            //initial user registration settings
            //
            SecuritySettings.UserRegistrationEmailSubject = UserRegEmailSubject.Text;
            SecuritySettings.UserRegistrationEmailBody = UserRegEmailBody.Text;

            //
            //password recovery settings
            //
            SecuritySettings.PasswordRecoveryEmailSubject = PasswordRecoveryEmailSubject.Text;
            SecuritySettings.PasswordRecoveryEmailBody = PasswordRecoveryEmailBody.Text;

            //redirect
            Response.Redirect("~/WebModules/Admin/WebsiteSettings/Default.aspx");
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/WebModules/Admin/WebsiteSettings/Default.aspx");
        }
    }
}
