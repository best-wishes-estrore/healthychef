using HealthyChef.Common;
using HealthyChef.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthyChef
{
    public partial class AdminForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblerror.Text = "";
            lblsuccess.Text = "";
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            lblerror.Text = "";
            lblsuccess.Text = "";
            var users = (from MembershipUser u in Membership.GetAllUsers()
                         where u.Email == txtForgotEmail.Text.Trim()
                         select new { Email = u.Email }).ToList();
            if (users.Count == 0)
            {
                lblerror.Text= "This email is not registered";
               
            }
            else
            {
                string userName = string.Empty;
                MembershipUser user = null;
                bool success = false;
                string newPassword = string.Empty;
                try
                {
                    userName = Membership.GetUserNameByEmail(txtForgotEmail.Text.Trim());
                    MembershipUser forgotpassworduser = Membership.GetUser(userName);
                    string[] roles = Roles.GetRolesForUser(userName);
                    if (roles.Contains("Customer") && roles.Count() == 1)
                    {
                        lblerror.Text = "Access denied";
                    }
                    else
                    { 
                        if (!string.IsNullOrWhiteSpace(userName))
                        {
                            user = Membership.GetUser(userName);
                            if (user != null)
                            {
                                string tempPassword = user.ResetPassword();
                                newPassword = OrderNumberGenerator.GenerateOrderNumber("?#?#?#?#");
                                success = user.ChangePassword(tempPassword, newPassword);
                            }
                            if (success)
                            {
                                //send Email
                                EmailController Ec = new EmailController();
                                Ec.SendMail_PasswordReset(txtForgotEmail.Text.Trim(), newPassword);
                                lblsuccess.Text = "Password Reset Successful - Your new password has been sent to the email address: " + txtForgotEmail.Text.Trim();

                            }
                            else
                            {
                                lblerror.Text = "Password Reset Failed. Email address not recognized. " +
                                "Please re-enter your email address or call customer service at 866-575-2433 for assistance.";

                            }
                        }
                    }
                }
                catch (MembershipPasswordException ex)
                {
                    lblerror.Text= "Cannot reset password. This account is currently locked.";
                }
                catch (Exception exstring)
                {
                    lblerror.Text = exstring.Message;
                }
            }
        }
    }
}