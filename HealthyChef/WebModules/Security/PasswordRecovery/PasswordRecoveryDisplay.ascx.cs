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
using HealthyChef.Common;
using HealthyChef.Email;

namespace BayshoreSolutions.WebModules.Security.PasswordRecovery
{
    public partial class PasswordRecoveryDisplay : BayshoreSolutions.WebModules.WebModuleBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("PasswordRecovery1");

                if (Page.IsValid)
                {
                    ResetPassword(txtEmail.Text.Trim());
                }
            }
            catch (MembershipPasswordException)
            {
                lblFeedback.Text = "Cannot reset password. This account is currently locked.";
            }
            catch (Exception)
            {
                throw;
            }
        }

        void ResetPassword(string emailAddress)
        {
            string userName = string.Empty;
            MembershipUser user = null;
            bool success = false;
            string newPassword = string.Empty;

            userName = Membership.GetUserNameByEmail(emailAddress);            

            if (!string.IsNullOrWhiteSpace(userName))
            {
                user = Membership.GetUser(userName);

                if (user != null)
                {
                    string tempPassword = user.ResetPassword();
                    newPassword = OrderNumberGenerator.GenerateOrderNumber("?#?#?#?#");
                    success = user.ChangePassword(tempPassword, newPassword);
                }
            }

            if (success)
            {   // send email
                EmailController ec = new EmailController();
                ec.SendMail_PasswordReset(emailAddress, newPassword);

                lblFeedback.Text = "<span style='color:green;'>Password Reset Successful - Your new password has been sent to the email address: " + emailAddress +
                  "</span><br/><br/><a href='/login.aspx'>Click Here</a> to return to account log in screen.</span>";

                divForm.Visible = false;
            }
            else
                lblFeedback.Text = "<span style='color:red;'>Password Reset Failed. Email address not recognized. " +
                    "Please re-enter your email address or call customer service at 866-575-2433 for assistance.</span>";
        }
    }
}
