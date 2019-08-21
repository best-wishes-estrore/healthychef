using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using System.Text;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class AccountManager : System.Web.UI.Page
    {
        List<MembershipUser> CurrentUsers { get; set; }

        protected bool IsAdmin { get; set; }
        protected bool IsEmployeeManager { get; set; }
        protected bool IsEmployeeProduction { get; set; }
        protected bool IsEmployeeService { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSearch.Click += btnSearch_Click;
            btnClear.Click += btnClear_Click;
            btnAddNewUser.Click += new EventHandler(btnAddNewUser_Click);

            gvwAccounts.RowDataBound += gvwAccounts_RowDataBound;
            gvwAccounts.SelectedIndexChanged += new EventHandler(gvwAccounts_SelectedIndexChanged);
            gvwAccounts.PageIndexChanging += gvwAccounts_PageIndexChanging;

            UserProfileEdit1.ControlCancelled += new ControlCancelledEventHandler(UserProfileEdit1_ControlCancelled);
            UserProfileEdit1.ControlSaved += new ControlSavedEventHandler(UserProfileEdit1_ControlSaved);
            UserProfileEdit1.PasswordReset += new PasswordResetEventHandler(UserProfileEdit1_PasswordReset);
            UserProfileEdit1.PasswordCancel += UserProfileEdit1_PasswordCancel;
        }

        void UserProfileEdit1_PasswordCancel(object sender, PasswordResetEventArgs e)
        {
            lblFeedback.Text = "Password Reset Failed - User may be locked out. - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
        }

        void UserProfileEdit1_PasswordReset(object sender, PasswordResetEventArgs e)
        {
            if (e.IsSuccessful)
                lblFeedback.Text = "Password Reset Success - An email has been sent to the user - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
            else
                lblFeedback.Text = "Password Reset Failed - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentLoggedUserID.Value = Membership.GetUser().ProviderUserKey.ToString();
            string UserID = string.Empty;
            if (Request.QueryString.AllKeys.Contains("UserID"))
            {
                UserID = Request.QueryString["UserID"].ToString();
            }

            if(!string.IsNullOrEmpty(UserID) && UserProfileEdit1.PrimaryKeyIndex == 0)
            {
                CurrentUserID.Value = UserID;
                divSearchPanel.Visible = false;
                btnAddNewUser.Visible = false;
                gvwAccounts.Visible = false;
                pnlGrids.Visible = false;
                divEdit.Visible = true;

                Guid aspNetId = Guid.Parse(UserID);
                hccUserProfile prof = hccUserProfile.GetParentProfileBy(aspNetId);

                if (prof != null)
                    UserProfileEdit1.PrimaryKeyIndex = prof.UserProfileID;

                UserProfileEdit1.CurrentAspNetId = aspNetId;
                UserProfileEdit1.Bind();
                UserProfileEdit1.Visible = true;
            }

            else if (!IsPostBack)
            {
                //BindddlDeliveryDates();
                //BindgvwAccounts();
            }

            MembershipUser user = HealthyChef.Common.Helpers.LoggedUser;
            IsAdmin = Roles.IsUserInRole(user.UserName, "Administrators");
            IsEmployeeManager = Roles.IsUserInRole(user.UserName, "EmployeeManager");
            IsEmployeeProduction = Roles.IsUserInRole(user.UserName, "EmployeeProduction");
            IsEmployeeService = Roles.IsUserInRole(user.UserName, "EmployeeService");

            divany.Visible = IsAdmin;
            divAdministrator.Visible = IsAdmin;
            divCustomer.Visible = IsEmployeeManager|| IsAdmin|| IsEmployeeService;
            divEmployeeManager.Visible = IsAdmin;
            divEmployeeProduction.Visible = IsEmployeeManager || IsAdmin;
            divEmployeeService.Visible = IsEmployeeManager || IsAdmin;

        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            Page.Validate("UserSearchGroup");

            if (Page.IsValid)
            {
                string lastName = null;
                if (!string.IsNullOrWhiteSpace(txtSearchLastName.Text.Trim())) { lastName = txtSearchLastName.Text.Trim(); }

                string email = null;
                if (!string.IsNullOrWhiteSpace(txtSearchEmail.Text.Trim())) { email = txtSearchEmail.Text.Trim(); }

                string phone = null;
                if (!string.IsNullOrWhiteSpace(txtSearchPhone.Text.Trim())) { phone = txtSearchPhone.Text.Trim(); }

                int? purchNum = null;
                if (!string.IsNullOrWhiteSpace(txtSearchPurchNum.Text.Trim())) { purchNum = int.Parse(txtSearchPurchNum.Text.Trim()); }

                DateTime? delivDate = null;
                if (ddlDeliveryDates.SelectedIndex != 0) { delivDate = DateTime.Parse(ddlDeliveryDates.SelectedItem.Text.Trim()); }

                string roles = null;
                foreach (ListItem item in cblRoles.Items)
                {
                    if (item.Selected && item.Text != "Any")
                        roles += item.Text.Trim() + ",";
                }

                if (!string.IsNullOrWhiteSpace(roles))
                {
                    roles = roles.TrimEnd(',');
                }

                CurrentUsers = hccUserProfile.Search(lastName, email, phone, purchNum, delivDate, roles);
                BindgvwAccounts();
            }
        }

        void btnClear_Click(object sender, EventArgs e)
        {
            txtSearchLastName.Text = string.Empty;
            txtSearchEmail.Text = string.Empty;
            txtSearchPhone.Text = string.Empty;
            txtSearchPurchNum.Text = string.Empty;
            ddlDeliveryDates.ClearSelection();
            BindgvwAccounts();
        }

        protected void btnAddNewUser_Click(object sender, EventArgs e)
        {
            divSearchPanel.Visible = false;
            btnAddNewUser.Visible = false;
            gvwAccounts.Visible = false;
            divEdit.Visible = true;              
            
            pnlGrids.Visible = false;            

            UserProfileEdit1.Clear();
            UserProfileEdit1.Bind();
            UserProfileEdit1.Visible = true;
        }

        void BindddlDeliveryDates()
        {
            if (ddlDeliveryDates.Items.Count == 0)
            {
                List<hccProductionCalendar> cals = hccProductionCalendar.GetAll();
                ddlDeliveryDates.DataSource = cals;
                ddlDeliveryDates.DataTextField = "DeliveryDate";
                ddlDeliveryDates.DataTextFormatString = "{0:MM/dd/yyyy}";
                ddlDeliveryDates.DataValueField = "CalendarID";
                ddlDeliveryDates.DataBind();

                ddlDeliveryDates.Items.Insert(0, new ListItem("Select a delivery date...", "-1"));
            }

            if (cblRoles.Items.Count == 0)
            {
                cblRoles.DataSource = Roles.GetAllRoles();
                cblRoles.DataBind();

                if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators"))
                {
                    cblRoles.Items.Insert(0, new ListItem("Any"));
                }
                else if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "EmployeeManager"))
                {
                    cblRoles.Items.Remove(cblRoles.Items.FindByText("Administrators"));
                    cblRoles.Items.Remove(cblRoles.Items.FindByText("EmployeeManager"));
                }
                else if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "EmployeeService"))
                {
                    cblRoles.Items.Remove(cblRoles.Items.FindByText("Administrators"));
                    cblRoles.Items.Remove(cblRoles.Items.FindByText("EmployeeManager"));
                    cblRoles.Items.Remove(cblRoles.Items.FindByText("EmployeeService"));
                    cblRoles.Items.Remove(cblRoles.Items.FindByText("EmployeeProduction"));
                }
                else if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "EmployeeProduction"))
                {
                    Response.Redirect("~/Admin", true);
                }

                cblRoles.Items.FindByValue("Customer").Selected = true;

                //var _rolesButtons = cblRoles.Items;
                //foreach (ListItem li in cblRoles.Items)
                //{
                //    if (li.Text == "Any") { li.Value = ""; }
                //    li.Attributes.Add("ng-model", "RoleFilter");
                //}
            }
        }

        void BindgvwAccounts()
        {
            //if (CurrentUsers == null)
            //    CurrentUsers = Membership.GetAllUsers().OfType<MembershipUser>().OrderBy(a => a.Email).ToList();

            //gvwAccounts.DataSource = CurrentUsers;
            //gvwAccounts.DataBind();

            //lblAccountCount.Text = "Count: " + ((gvwAccounts.PageIndex * gvwAccounts.PageSize) + 1).ToString()
            //    + " - " + ((gvwAccounts.PageIndex * gvwAccounts.PageSize) + gvwAccounts.PageSize).ToString()
            //    + " of " + CurrentUsers.Count;
        }

        //protected void gvwAccounts_RowCreated(object sender, GridViewRowEventArgs e)
        //{
        //    //if (e.Row.RowType == DataControlRowType.DataRow)
        //    //{
        //    //    LinkButton lkbDelete = e.Row.Cells[4].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Deactivate");

        //    //    if (lkbDelete != null)
        //    //        lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to deactivate/reactivate this account?');");
        //    //}
        //}

        protected void gvwAccounts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    RoledUser user = new RoledUser((MembershipUser)e.Row.DataItem);

                    if (user != null)
                    {
                        Label lblFullName = (Label)e.Row.FindControl("lblFullName");
                        Label lblUserRole = (Label)e.Row.FindControl("lblUserRole");

                        lblFullName.Text = user.FullName;
                        lblUserRole.Text = user.UserRoles;
                        //LinkButton lkbDelete = e.Row.Cells[0].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Deactivate");

                        //if ((Guid)user.ProviderUserKey == (Guid)Helpers.LoggedUser.ProviderUserKey)
                        //{
                        //    lkbDelete.Visible = false;
                        //}
                        //else
                        //{
                        //    if (!user.IsApproved)
                        //    {
                        //        lkbDelete.Text = "Reactivate";
                        //    }
                        //}

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void gvwAccounts_SelectedIndexChanged(object sender, EventArgs e)
        {
            divSearchPanel.Visible = false;
            btnAddNewUser.Visible = false;
            gvwAccounts.Visible = false;
            divEdit.Visible = true;

            Guid aspNetId = Guid.Parse(gvwAccounts.SelectedDataKey.Value.ToString());
            hccUserProfile prof = hccUserProfile.GetParentProfileBy(aspNetId);

            if (prof != null)
                UserProfileEdit1.PrimaryKeyIndex = prof.UserProfileID;

            UserProfileEdit1.CurrentAspNetId = aspNetId;
            UserProfileEdit1.Bind();
            UserProfileEdit1.Visible = true;
        }

        void gvwAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
             Page.Validate("UserSearchGroup");

             if (Page.IsValid)
             {
                 gvwAccounts.PageIndex = e.NewPageIndex;

                 string lastName = null;
                 if (!string.IsNullOrWhiteSpace(txtSearchLastName.Text.Trim())) { lastName = txtSearchLastName.Text.Trim(); }

                 string email = null;
                 if (!string.IsNullOrWhiteSpace(txtSearchEmail.Text.Trim())) { email = txtSearchEmail.Text.Trim(); }

                 string phone = null;
                 if (!string.IsNullOrWhiteSpace(txtSearchPhone.Text.Trim())) { phone = txtSearchPhone.Text.Trim(); }

                 int? purchNum = null;
                 if (!string.IsNullOrWhiteSpace(txtSearchPurchNum.Text.Trim())) { purchNum = int.Parse(txtSearchPurchNum.Text.Trim()); }

                 DateTime? delivDate = null;
                 if (ddlDeliveryDates.SelectedIndex != 0) { delivDate = DateTime.Parse(ddlDeliveryDates.SelectedItem.Text.Trim()); }

                 string roles = null;
                 foreach (ListItem item in cblRoles.Items)
                 {
                     if (item.Selected && item.Text != "Any")
                         roles += item.Text.Trim() + ",";
                 }

                 if (!string.IsNullOrWhiteSpace(roles))
                 {
                     roles = roles.TrimEnd(',');
                 }


                 CurrentUsers = hccUserProfile.Search(lastName, email, phone, purchNum, delivDate, roles);

                 BindgvwAccounts();
             }
        }

        protected void UserProfileEdit1_ControlCancelled(object sender)
        {
            BindgvwAccounts();
            UserProfileEdit1.Clear();

            divSearchPanel.Visible = true;
            btnAddNewUser.Visible = true;
            UserProfileEdit1.Visible = false;

            btnAddNewUser.Visible = true;
            gvwAccounts.Visible = true;
            divEdit.Visible = false;
        }

        protected void UserProfileEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            lblFeedback.Text = "Account Information Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
            //BindgvwAccounts();
            //UserProfileEdit1.Clear();

            //btnAddNewUser.Visible = true;
            //gvwAccounts.Visible = true;
            //divEdit.Visible = false;      
        }
    }

    public class RoledUser
    {
        public RoledUser(MembershipUser user)
        {
            StringBuilder roles = new StringBuilder();
            Roles.GetRolesForUser(user.UserName).ToList().ForEach(a => roles.Append(a));

            ProviderUserKey = user.ProviderUserKey;
            Email = user.Email;
            IsApproved = user.IsApproved;
            IsLockedOut = user.IsLockedOut;
            IsOnline = user.IsOnline;
            UserRoles = roles.ToString();

            if (UserRoles.Contains("Customer"))
            {
                hccUserProfile profile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                //Create basic profile is user does not have one.
                if (profile == null)
                {
                    profile = new hccUserProfile
                    {
                        MembershipID = (Guid)user.ProviderUserKey,
                        CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        AccountBalance = 0.00m,
                        ProfileName = "Main"
                    };
                    profile.Save();
                }
                else
                {
                    this.FullName = profile.FullName;
                }
            }
        }

        public RoledUser(hccUserProfile profile)
        {
            StringBuilder roles = new StringBuilder();
            Roles.GetRolesForUser(profile.ASPUser.UserName).ToList().ForEach(a => roles.Append(a));

            //ParentUser = profile.ASPUser;
            ProviderUserKey = profile.ASPUser.ProviderUserKey;
            Email = profile.ASPUser.Email;
            IsApproved = profile.ASPUser.IsApproved;
            IsLockedOut = profile.ASPUser.IsLockedOut;
            IsOnline = profile.ASPUser.IsOnline;
            UserRoles = roles.ToString();
        }

        public object ProviderUserKey { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public bool IsApproved { get; set; }
        public bool IsLockedOut { get; set; }
        public bool IsOnline { get; set; }
        public string UserRoles { get; set; }

        //public static List<RoledUser> GetFromRange(List<MembershipUser> users)
        //{
        //    List<RoledUser> roledUsers = new List<RoledUser>();

        //    foreach (MembershipUser user in users)
        //    {
        //        roledUsers.Add(new RoledUser(user));
        //    }

        //    return roledUsers;
        //}

        //public static List<RoledUser> GetFromRange(MembershipUserCollection users)
        //{
        //    List<RoledUser> roledUsers = new List<RoledUser>();

        //    foreach (MembershipUser user in users)
        //    {
        //        roledUsers.Add(new RoledUser(user));
        //    }

        //    return roledUsers;
        //}
        //public static List<RoledUser> GetFromRange(List<hccUserProfile> profiles)
        //{
        //    List<RoledUser> roledUsers = new List<RoledUser>();

        //    foreach (hccUserProfile profile in profiles)
        //    {
        //        roledUsers.Add(new RoledUser(profile));
        //    }

        //    return roledUsers;
        //}
    }
}