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

namespace BayshoreSolutions.WebModules.Admin.Security
{
    public partial class RoleList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void FormView1_ItemInserted(object sender, FormViewInsertedEventArgs e)
        {
            Response.Redirect(Request.FilePath);
        }
        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete_")
            {
                string role = e.CommandArgument.ToString();
                int nrUsersInRole = Roles.GetUsersInRole(role).Length;
                //Allow delete only if there are no users in the role.
                if (nrUsersInRole > 0)
                {
                    throw new InvalidOperationException(string.Format("Cannot delete a non-empty role '{0}'. All users must be removed from the role.", role));
                }
                else //the role is empty, ok to delete.
                {
                    Roles.DeleteRole(role);
                    Response.Redirect(Request.FilePath);
                }
            }
        }
    }
}