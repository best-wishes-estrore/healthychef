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
using BayshoreSolutions.WebModules.Security;

using BayshoreSolutions.WebModules.Cms.Security.Model;

namespace BayshoreSolutions.WebModules.Admin.Security
{
    public partial class UserAdd : System.Web.UI.Page
    {
        //void ShowError(string msg)
        //{
        //    uxMsg.Visible = true;
        //    uxMsg.Text = msg;
        //    uxMsg.ForeColor = System.Drawing.Color.Red;
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            //uxMsg.Visible = false;
            //Page.Form.DefaultButton = InsertButton.UniqueID;
        }
        //protected void InsertButton_Click(object sender, EventArgs e)
        //{
        //    //MembershipCreateStatus createResult;
        //    //MembershipUser currentUser = Membership.CreateUser(UserNameTextBox.Text,
        //    //    PasswordTextBox.Text,
        //    //    emailTextBox.Text,
        //    //    null,
        //    //    null,
        //    //    true,
        //    //    out createResult);

        //    //if (createResult == MembershipCreateStatus.Success)
        //    //{
        //    //    WebModulesProfile profile = WebModulesProfile.Create(currentUser.UserName, true);

        //    //    profile.FirstName = FirstName.Text;
        //    //    profile.LastName = LastName.Text;
        //    //    profile.StartPageId = (PagePicker.SelectedNavigationId == -1)
        //    //        ? Webpage.RootNavigationId
        //    //        : PagePicker.SelectedNavigationId;
        //    //    profile.Save();

        //    //    Response.Redirect("UserList.aspx");
        //    //}
        //    //else ShowError("Failed to create user.<br/>"
        //    //    + UserRegistration_Module.GetHumanStatusMessage(createResult));
        //}
        //protected void InsertCancelButton_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("UserList.aspx");
        //}
    }
}
