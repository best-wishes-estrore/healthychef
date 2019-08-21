using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BayshoreSolutions.WebModules.Security;
using securityModule = BayshoreSolutions.WebModules.Cms.Security;

namespace BayshoreSolutions.WebModules.Admin.Security
{
    public partial class UserEdit : System.Web.UI.Page
    {
        protected MembershipUser _user;

        protected string formatDate(DateTime date)
        {
            return (date > new DateTime(1900, 1, 1))
                ? string.Format("{0} {1}", date.ToShortDateString(), date.ToShortTimeString())
                : "Never";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["UserName"] != null)
            {
                _user = Membership.GetUser(Request.QueryString["UserName"]);
                if (_user == null)
                    Response.Redirect("UserList.aspx");
            }
            else
                _user = Membership.GetUser();

            if (!Page.IsPostBack)
            {
                DataBind();

                WebModulesProfile profile = WebModulesProfile.GetProfile(_user.UserName);
                if (profile != null)
                {
                    FirstNameCtl.Text = profile.FirstName;
                    LastNameCtl.Text = profile.LastName;
                    PagePicker.SelectedNavigationId = profile.StartPageId;
                }

                //
                //bind the roles list
                //
                RolesList.DataSource = Roles.GetAllRoles();
                RolesList.DataBind();
                foreach (string role in Roles.GetRolesForUser(_user.UserName))
                {
                    ListItem item = RolesList.Items.FindByValue(role);
                    if (item != null)
                    {
                        item.Selected = true;
                        if (Page.User.Identity.Name == _user.UserName
                            && role == Role.Administrators)
                        { //cannot add/remove oneself to/from the 'Administrators' role.
                            item.Enabled = false;
                        }
                    }
                }
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            //indicates whether the user is being approved.
            bool isInitialApproval = false;

            //if start page id isn't valid, 'WebModules_Pages_CheckPermission' sproc will fail. 
            //if page id is null, user has access to all pages.
            int startPageId = (PagePicker.SelectedNavigationId == -1)
                ? Webpage.RootNavigationId
                : PagePicker.SelectedNavigationId;

            List<string> grantedRoles = new List<string>();
            string[] roles = Roles.GetRolesForUser(_user.UserName);

            foreach (ListItem li in RolesList.Items)
                if (li.Selected)
                    grantedRoles.Add(li.Value);

            if (roles.Length > 0)
                Roles.RemoveUserFromRoles(_user.UserName, roles);

            if (grantedRoles.Count > 0)
                Roles.AddUserToRoles(_user.UserName, grantedRoles.ToArray());

            _user.Email = EmailCtl.Text;
            _user.Comment = CommentsCtl.Text;

            isInitialApproval =
                //was the user not approved yet?
                !_user.IsApproved
                //is the user being approved with this update?
                && IsApprovedCheckBox.Checked;

            _user.IsApproved = IsApprovedCheckBox.Checked;

            Membership.UpdateUser(_user);

            WebModulesProfile profile = WebModulesProfile.GetProfile(_user.UserName);
            if (null == profile) profile = WebModulesProfile.Create(_user.UserName, true);
            if (profile != null)
            {
                profile.FirstName = FirstNameCtl.Text;
                profile.LastName = LastNameCtl.Text;
                profile.StartPageId = startPageId;
                profile.Save();
            }

            if (isInitialApproval)
            { //notify the user that s/he has been approved. 
                if (!string.IsNullOrEmpty(_user.Email))
                    //this also resets the user's password.
                    securityModule.Model.SecurityEmail.NotifyUserOfApproval(_user);
            }

            Response.Redirect("UserList.aspx");
        }
        protected void EditDeleteButton_Click(object sender, EventArgs e)
        {
            string currentUser = Page.User.Identity.Name;

            if (_user.UserName == currentUser)
            {
                Msg.ShowError("You cannot delete yourself.");
                return;
            }

            Membership.DeleteUser(_user.UserName, true);

            Response.Redirect("UserList.aspx");
        }
        protected void SaveCancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserList.aspx");
        }
        protected void Unlock_Click(object sender, EventArgs e)
        {
            _user.UnlockUser();
            Response.Redirect("UserEdit.aspx?UserName=" + _user.UserName);
        }
        protected void ResetPassword_Click(object sender, EventArgs e)
        {
            NewPassword.InnerHtml = "New Password: " + _user.ResetPassword();
            LastPasswordChangeDateLabel.DataBind();
        }
    }
}