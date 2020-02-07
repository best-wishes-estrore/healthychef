using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthyChef
{
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = Membership.GetUserNameByEmail(txtEmail.Text.Trim());

                if (userName == null)
                    userName = txtEmail.Text.Trim();

                if (Membership.ValidateUser(userName, txtPassword.Text.Trim()))
                {
                    MembershipUser user = Membership.GetUser(userName);
                    string[] roles = Roles.GetRolesForUser(userName);
                    if (user != null)
                    {
                        if (roles.Contains("Administrators"))
                        {
                            FormsAuthentication.SetAuthCookie(userName, true);   
                            Response.Redirect("~/WebModules/Admin/MyWebsite/Default.aspx",false); 
                        }
                        else if (roles.Contains("EmployeeManager") || roles.Contains("EmployeeProduction") || roles.Contains("EmployeeService"))
                        {
                            FormsAuthentication.SetAuthCookie(userName, true);
                            Response.Redirect("~/WebModules/ShoppingCart/", false);
                        }
                        else
                        {
                            litMessage.Text = "This user does not have admin permissions.";
                        }
                    }
                    else
                    {
                        litMessage.Text = "Login Attempt Failed.  Email/password combination not recognized.  Please re-enter your email address and account password.  If you have forgotten your password, please click the link below or call customer service at 866-575-2433 for assistance.";
                    }
                }
                else
                {
                    MembershipUser user = Membership.GetUser(userName);

                    if (user == null || !Membership.ValidateUser(userName, txtPassword.Text.Trim()))
                    {
                        litMessage.Text = "Login Attempt Failed.  Email/password combination not recognized.  Please re-enter your email address and account password.  If you have forgotten your password, please click the link below or call customer service at 866-575-2433 for assistance.";
                    }
                    else if (!user.IsApproved)
                    {
                        litMessage.Text = "That account has been deactivated. Please contact customer service at 866-575-2433 for assistance.";
                    }
                    else if (user.IsLockedOut)
                    { //password lock-out
                        litMessage.Text = "That account is locked out. Please contact customer service at 866-575-2433 for assistance.";
                    }
                }
            }
            catch (Exception E)
            {
                throw;
            }
        }
    }
}