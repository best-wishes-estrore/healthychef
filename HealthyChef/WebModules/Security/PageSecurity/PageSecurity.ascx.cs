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

using cms = BayshoreSolutions.WebModules.Cms;
using BayshoreSolutions.WebModules.Security;

namespace BayshoreSolutions.WebModules.Cms.Security.PageSecurity
{
    public partial class PageSecurity : System.Web.UI.UserControl
    {
        int _instanceId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["InstanceId"] != null)
                _instanceId = int.Parse(Request.QueryString["InstanceId"]);
            else Response.Redirect("Default.aspx");

            if (!IsPostBack)
            {
                //check user permissions.
                if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(_instanceId, Page.User))
                    throw new System.Security.SecurityException(BayshoreSolutions.WebModules.Security.Permission.MSG_SECURITY_FAILURE);

                MainMenuLink.HRef = cms.Admin.GetMainMenuUrl(_instanceId);

                load_();
            }
        }
        void load_()
        {
            load_(Webpage.GetWebpage(_instanceId));
        }
        void load_(WebpageInfo p)
        {
            //
            //the business rules of inheritance are implemented by NavigationRole.IsUserAuthorized()
            //using NavigationRole.GetAcl(); the rules are followed in the same way by this 
            //control, PageSecurity.ascx, and therefore there is a precarious coupling between
            //the two. In the future we will encapsulate the ACL/IsPublic business rules.
            //

            bool isPublic;

            NavigationRoleCollection acl = NavigationRole.GetAcl(p, true, out isPublic);

            AclList.DataSource = acl;
            AclList.DataBind();

            IsPublic.Checked = isPublic;

            if (!isPublic && p.IsPublic)
            {
                IsPublic.Enabled = false;
                IsPublic.Text = "Public (inherited)";
            }
            else
            {
                IsPublic.Text = "Public";
            }

            //
            //set help text
            //
            if (isPublic) //overrides role assignments
                HelpText.Text = "<strong>All users,</strong> including anonymous/public users, can access the page.";
            else if (acl.Count > 0)
                HelpText.Text = "<strong>Access is restricted.</strong> Only the roles listed below have access to the page.";
            else
                HelpText.Text = "<strong>All logged-in users</strong> can access the page. Anonymous/public users cannot access the page.";
        }
        protected void CancelButton_Click(object sender, EventArgs e)
        {
            Response.Redirect("Default.aspx?instanceId=" + _instanceId);
        }

        protected void AclList_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int navId = int.Parse(AclList.DataKeys[e.RowIndex].Values[0].ToString());
            string roleName = AclList.DataKeys[e.RowIndex].Values[1].ToString();

            WebpageInfo p = Webpage.GetWebpage(navId);
            p.RemoveRoleFromAcl(roleName);

            load_();
        }

        protected void AclList_DataBound(object sender, EventArgs e)
        {
            //AclListHelpText.Visible = (AclList.Rows.Count > 0);
        }

        protected void IsPublic_CheckedChanged(object sender, EventArgs e)
        {
            WebpageInfo p = Webpage.GetWebpage(_instanceId);
            p.IsPublic = IsPublic.Checked;
            Webpage.UpdateWebpage(p);

            //TODO: use Webpage.ForEachDescendant() to propogate changes down the tree.

            load_(p);
        }

        protected void RolesDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            WebpageInfo p = Webpage.GetWebpage(_instanceId);
            p.AddRoleToAcl(RolesDropDown.Text);

            RolesDropDown.ClearSelection();
            load_();
        }
    }
}