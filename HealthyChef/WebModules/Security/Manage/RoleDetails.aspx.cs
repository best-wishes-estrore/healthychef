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

namespace BayshoreSolutions.WebModules.Security.Manage
{
    public partial class RoleDetails : System.Web.UI.Page
    {
        string _roleName = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            _roleName = Request.QueryString["role"];
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete_")
            {
                string selectedUser = e.CommandArgument.ToString();
                //string selectedRole = e.Keys[1].ToString();
                string currentUser = Page.User.Identity.Name;
                string adminRole = BayshoreSolutions.WebModules.Security.Role.Administrators;

                if (selectedUser == currentUser
                    && _roleName == adminRole)
                {
                    ErrorMessage.Text = string.Format(
                        "You cannot remove yourself from the '{0}' role.",
                        adminRole);
                }
                else //delete the user
                {
                    System.Web.Security.Roles.RemoveUserFromRole(selectedUser, _roleName);
                }

                GridView1.DataBind();
            }
        }
    }
}
