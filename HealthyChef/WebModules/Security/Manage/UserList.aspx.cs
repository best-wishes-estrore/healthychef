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

using BayshoreSolutions.WebModules.Security;

namespace BayshoreSolutions.WebModules.Admin.Security
{
    public partial class UserList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void AddButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserAdd.aspx");
        }

        protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            object commmandArg = e.CommandArgument;

            if (e.CommandName == "Delete_")
            {
                string selectedUser = commmandArg.ToString();
                string currentUser = Page.User.Identity.Name;

                if (selectedUser == currentUser)
                {
                    ErrorMessage.Text = "You cannot delete yourself.";
                }
                else
                {
                    Membership.DeleteUser(selectedUser, true);

                    GridView1.DataBind(); //reload
                }
            }
        }

        //
        //Database-side GridView-paging requires an ObjectDataSource;
        //ObjectDataSource-paging requires the following method signatures.
        //see: http://weblogs.asp.net/alessandro/archive/2007/10/09/custom-serverside-paging-in-gridview-vs-datagrid.aspx
        //
        //required for paging
        int _totalUsersCount = 0;
        //required for paging
        public int GetAllUsersCount()
        {
            return _totalUsersCount;
        }
        //required for paging
        public MembershipUserCollection GetAllUsers(int startRowIndex, int maximumRows)
        {
            if (startRowIndex > 0)
                startRowIndex = startRowIndex / maximumRows;

            //startRowIndex += 1;
            return Membership.GetAllUsers(startRowIndex,
                maximumRows,
                out _totalUsersCount);
        }
    }
}
