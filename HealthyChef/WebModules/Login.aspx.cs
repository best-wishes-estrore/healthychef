using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BayshoreSolutions.WebModules.Security;

namespace BayshoreSolutions.WebModules
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //
            //if this is a fresh install, create a new user.
            //
            int nrUsers;
            Membership.GetAllUsers(0, 1, out nrUsers);
            if (nrUsers == 0)
            {
                LoginMultiView.SetActiveView(CreateUserView);
                //Membership.CreateUser("admin", "admin", ");
            }

            //Page.Form.DefaultFocus = Login1.FindControl("UserName").UniqueID;
            //Page.SetFocus(Login1);
            Login1.Focus();

            //ID="LoginButton" if button is a Button.
            //ID="LoginLinkButton" if button is a link LinkButton.
            Page.Form.DefaultButton = Login1.FindControl("LoginButton").UniqueID;
        }
        protected void CreateUserWizard_CreatedUser(object sender, EventArgs e)
        {
            if (!Roles.RoleExists(Role.Administrators))
                Roles.CreateRole(Role.Administrators);

            Roles.AddUserToRole(CreateUserWizard.UserName, Role.Administrators);
        }
        protected void Login1_LoggedIn(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
            {
                Login1.DestinationPageUrl = "~/WebModules/Default.aspx";
            }
        }
        protected void CreateUserWizard_ContinueButtonClick(object sender, EventArgs e)
        {
            Response.Redirect("~/WebModules/Default.aspx");
        }
        protected void CompleteWizardStep1_Activate(object sender, EventArgs e)
        { //skip the "complete" step and go directly to the admin.
            Response.Redirect("~/WebModules/Default.aspx");
        }
    }
}
