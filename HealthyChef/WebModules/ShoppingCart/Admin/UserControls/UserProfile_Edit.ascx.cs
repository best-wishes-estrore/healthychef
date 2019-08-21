using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using System.Web.Security;
using HealthyChef.AuthNet;
using System.Configuration.Provider;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class UserProfile_Edit : FormControlBase
    {   //this.PrimaryKeyIndex as hccUserProfileId 
        public event PasswordResetEventHandler PasswordReset;
        public event PasswordResetEventHandler PasswordCancel;
        protected void OnPasswordReset(object sender, PasswordResetEventArgs e)
        {
            if (PasswordReset != null)
                PasswordReset(this, e);
        }
        protected void OnPasswordCancel(object sender, PasswordResetEventArgs e)
        {
            if (PasswordCancel != null)
                PasswordCancel(this, e);
        }

        protected hccUserProfile CurrentUserProfile { get; set; }

        public Guid? CurrentAspNetId
        {
            get
            {
                if (ViewState["CurrentAspNetId"] == null)
                    return null;
                else
                    return Guid.Parse(ViewState["CurrentAspNetId"].ToString());
            }
            set
            {
                ViewState["CurrentAspNetId"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            cblRoles.SelectedIndexChanged += new EventHandler(cblRoles_SelectedIndexChanged);
            PasswordReset1.ControlSaved += PasswordReset1_ControlSaved;
            PasswordReset1.ControlCancelled += PasswordReset1_ControlCancelled;

            AddressEdit_Shipping1.ControlSaved += UserProfileInfo_ControlSaved;
            BillingInfoEdit1.ControlSaved += new ControlSavedEventHandler(BillingInfoEdit1_ControlSaved);

            SubProfileEdit1.ControlCancelled += new ControlCancelledEventHandler(SubProfileEdit1_ControlCancelled);
            SubProfileEdit1.ControlSaved += new ControlSavedEventHandler(SubProfileEdit1_ControlSaved);

            gvwSubProfiles.RowCreated += new GridViewRowEventHandler(gvwSubProfiles_RowCreated);
            gvwSubProfiles.RowDeleting += new GridViewDeleteEventHandler(gvwSubProfiles_RowDeleting);
            gvwSubProfiles.SelectedIndexChanged += new EventHandler(gvwSubProfiles_SelectedIndexChanged);

            //ProfileCartEdit1.ControlSaved += ProfileCartEdit1_ControlSaved;
            ProfileCartEdit1.ControlCancelled += ProfileCartEdit1_ControlCancelled;

            ProfileNotesEdit_Billing.ControlCancelled += ProfileNotesEdit_ControlCancelled;
            ProfileNotesEdit_General.ControlCancelled += ProfileNotesEdit_ControlCancelled;
            ProfileNotesEdit_Shipping.ControlCancelled += ProfileNotesEdit_ControlCancelled;

            btnSave0.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel0.Click += new EventHandler(base.CancelButtonClick);
            cstValProfile0.ServerValidate += new ServerValidateEventHandler(cstValProfile0_ServerValidate);
            cstValCardInfo0.ServerValidate += new ServerValidateEventHandler(cstValCardInfo0_ServerValidate);
            cstProfileName.ServerValidate += cstProfileName_ServerValidate;

            ProfileCartEdit1.CartSaved += ProfileCartEdit1_CartSaved;
            ProfileLedger1.ControlSaved += ProfileLedger1_ControlSaved;
        }

        protected override void LoadForm()
        {
            try
            {
                BindcblRoles();
                //BindrblDeliveryTypes();
                BindddlCoupons();

                //form fields
                if (CurrentAspNetId != null)
                {
                    divPassword.Visible = true;

                    MembershipUser user = Membership.GetUser(CurrentAspNetId);

                    chkIsLockedOut.Checked = user.IsLockedOut;
                    chkIsActive.Checked = user.IsApproved;

                    if ((Guid)user.ProviderUserKey == (Guid)Helpers.LoggedUser.ProviderUserKey)
                        chkIsLockedOut.Enabled = false;

                    txtEmail.Text = user.Email;

                    string[] userRoles = Roles.GetRolesForUser(user.UserName);

                    foreach (string role in userRoles)
                    {
                        ListItem roleItem = cblRoles.Items.FindByValue(role);

                        if (roleItem != null)
                            roleItem.Selected = true;
                    }

                    if (userRoles.ToList().Count(a => a == "Customer") > 0)
                        DisplayProfileTabs(true);
                    else
                        DisplayProfileTabs(false);

                    PasswordReset1.CurrentAspNetId = CurrentAspNetId;

                    CurrentUserProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                    if (CurrentUserProfile == null)
                    {
                        hccUserProfile newProfile = new hccUserProfile
                        {
                            MembershipID = (Guid)user.ProviderUserKey,
                            CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                            CreatedDate = DateTime.Now,
                            ProfileName = string.Empty,
                            AccountBalance = 0.00m,
                            IsActive = true
                        };

                        newProfile.Save();
                        CurrentUserID.Value = newProfile.MembershipID.ToString();
                        CurrentUserProfile = newProfile;
                        this.PrimaryKeyIndex = newProfile.UserProfileID;
                        
                    }
                    

                    if (CurrentUserProfile != null)
                    {
                        txtProfileName.Text = CurrentUserProfile.ProfileName;
                        txtFirstName.Text = CurrentUserProfile.FirstName;
                        txtLastName.Text = CurrentUserProfile.LastName;

                        // Canyon Ranch
                        if (CurrentUserProfile.CanyonRanchCustomer != null)
                            cbCanyonRanchCustomer.Checked = CurrentUserProfile.CanyonRanchCustomer.Value;

                        if (CurrentUserProfile.DefaultCouponId.HasValue)
                            ddlCoupons.SelectedIndex = ddlCoupons.Items.IndexOf(
                                ddlCoupons.Items.FindByValue(CurrentUserProfile.DefaultCouponId.ToString()));

                        //billing info                        
                        BillingInfoEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                        BillingInfoEdit1.Bind();

                        //shipping address
                        if (CurrentUserProfile.ShippingAddressID.HasValue)
                        {
                            AddressEdit_Shipping1.PrimaryKeyIndex = CurrentUserProfile.ShippingAddressID.Value;
                            AddressEdit_Shipping1.Bind();
                        }

                        //preferences
                        ProfilePrefsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                        ProfilePrefsEdit1.Bind();

                        //allergens
                        ProfileAllgsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                        ProfileAllgsEdit1.Bind();

                        //subprofiles
                        BindgvwSubProfiles();
                        SubProfileEdit1.CurrentParentAspNetId = CurrentAspNetId;
                        SubProfileEdit1.CurrentParentProfileId = CurrentUserProfile.UserProfileID;

                        //order history
                        BindHistory(); //0);

                        // recurring
                        BindRecurring();

                        //ledger
                        BindLedger();

                        //notes
                        ProfileNotesEdit_Billing.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                        ProfileNotesEdit_Billing.Bind();
                        ProfileNotesEdit_General.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                        ProfileNotesEdit_General.Bind();
                        ProfileNotesEdit_Shipping.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                        ProfileNotesEdit_Shipping.Bind();

                        //current cart
                        ProfileCartEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                        ProfileCartEdit1.Bind();
                        
                    }
                }
                else
                {
                    cblRoles.Items.FindByText("Customer").Selected = true;
                    DisplayProfileTabs(true);

                    liBilling.Visible = false;
                    liShipping.Visible = false;
                    liPrefs.Visible = false;
                    liAllergens.Visible = false;
                    liSubProfiles.Visible = false;
                    liNotes.Visible = false;
                    liTransactions.Visible = false;
                    liPurchases.Visible = false;
                    liCart.Visible = false;

                    tabs2.Visible = false;
                    tabs3.Visible = false;
                    tabs4.Visible = false;
                    tabs5.Visible = false;
                    tabs6.Visible = false;
                    tabs8.Visible = false;
                    tabs10.Visible = false;
                    tabs7.Visible = false;
                    tabs9.Visible = false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SaveForm()
        {
            try
            {
                MembershipUser user = null;

                if (CurrentAspNetId != null)
                    user = Membership.GetUser(CurrentAspNetId);
                else
                {
                    try
                    {
                        user = Membership.GetUser(Membership.GetUserNameByEmail(txtEmail.Text.Trim()));
                    }
                    catch (Exception) { }
                }

                if (user == null) // create new profile
                {
                    // create membership user

                    //formulate username
                    string email = txtEmail.Text.Trim();
                    string userName = email.Split('@')[0] + DateTime.Now.ToString("yyyyMMddHHmmtt");
                    string password = OrderNumberGenerator.GenerateOrderNumber("?#?#?#?#");

                    MembershipCreateStatus createResult;
                    MembershipUser newUser = Membership.CreateUser(userName, password, email, null, null, true, out createResult);

                    List<ListItem> selRoles = cblRoles.Items.OfType<ListItem>().Where(a => a.Selected).ToList();
                    selRoles.ForEach(delegate(ListItem item) { Roles.AddUserToRole(userName, item.Value); });

                    if (createResult == MembershipCreateStatus.Success)
                    {
                        CurrentAspNetId = (Guid)newUser.ProviderUserKey;

                        Email.EmailController ec = new Email.EmailController();
                        ec.SendMail_NewUserConfirmation(email, password);

                        if (selRoles.Where(a => a.Text.Contains("Customer")).Count() > 0)
                        {
                            hccUserProfile newProfile = new hccUserProfile
                            {
                                MembershipID = (Guid)newUser.ProviderUserKey,
                                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                ProfileName = txtProfileName.Text.Trim(),
                                FirstName = txtFirstName.Text.Trim(),
                                LastName = txtLastName.Text.Trim(),
                                IsActive = true
                            };

                            if (ddlCoupons.SelectedIndex > 0)
                                newProfile.DefaultCouponId = int.Parse(ddlCoupons.SelectedValue);
                            else
                                newProfile.DefaultCouponId = null;

                            newProfile.Save();
                            CurrentUserID.Value = newProfile.MembershipID.ToString();
                            this.PrimaryKeyIndex = newProfile.UserProfileID;
                            Response.Redirect("~/WebModules/ShoppingCart/Admin/AccountManager.aspx?UserID=" + newProfile.MembershipID.ToString(), false);
                        }

                        liBilling.Visible = true;
                        liShipping.Visible = true;
                        liPrefs.Visible = true;
                        liAllergens.Visible = true;
                        liSubProfiles.Visible = true;
                        liNotes.Visible = true;
                        liTransactions.Visible = true;
                        liPurchases.Visible = true;
                        liCart.Visible = true;

                        tabs2.Visible = true;
                        tabs3.Visible = true;
                        tabs4.Visible = true;
                        tabs5.Visible = true;
                        tabs6.Visible = true;
                        tabs8.Visible = true;
                        tabs10.Visible = true;
                        tabs7.Visible = true;
                        tabs9.Visible = true;

                        LoadForm();

                        OnSaved(new ControlSavedEventArgs(newUser.ProviderUserKey));
                    }
                    else
                    {
                        cstValProfile0.Enabled = true;
                        cstValProfile0.ErrorMessage = Helpers.CreateUserStatusMessage(createResult);
                        cstValProfile0.Validate();
                        Page.Validate();
                    }
                }
                else // edit existing profile
                {
                    if (user.Email != txtEmail.Text.Trim()) // update userprofile and aspmembership user
                    {
                        user.Email = txtEmail.Text.Trim();
                        Membership.UpdateUser(user);
                    }

                    if (chkIsLockedOut.Checked)
                        Helpers.LockUser(user);
                    else
                    {
                        if (user.IsLockedOut)
                            user.UnlockUser();

                        if (!user.IsApproved)
                        {
                            user.IsApproved = true;
                            Membership.UpdateUser(user);
                        }
                    }

                    
                    List<ListItem> selRoles = cblRoles.Items.OfType<ListItem>().Where(a => a.Selected).ToList();
                    if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators"))
                    {
                        Roles.GetAllRoles().ToList().ForEach(delegate(string roleName)
                        {
                            if (Roles.IsUserInRole(user.UserName, roleName))
                                Roles.RemoveUserFromRole(user.UserName, roleName);
                        });

                        selRoles.ForEach(delegate(ListItem item)
                        {
                            Roles.AddUserToRole(user.UserName, item.Value);
                        });
                    }

                    hccUserProfile editProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                    if (editProfile == null && selRoles.Where(a => a.Text.Contains("Customer")).Count() > 0)
                    {
                        editProfile = new hccUserProfile
                        {
                            MembershipID = (Guid)user.ProviderUserKey,
                            CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                            CreatedDate = DateTime.Now,
                            ProfileName = txtProfileName.Text.Trim(),
                            FirstName = txtFirstName.Text.Trim(),
                            LastName = txtLastName.Text.Trim(),
                            CanyonRanchCustomer = cbCanyonRanchCustomer.Checked
                        };
                       
                        editProfile.Save();
                        this.PrimaryKeyIndex = editProfile.UserProfileID;
                       
                        OnSaved(new ControlSavedEventArgs(editProfile.UserProfileID));
                    }

                    if (editProfile != null)
                    {
                        editProfile.ProfileName = txtProfileName.Text.Trim();
                        editProfile.FirstName = txtFirstName.Text.Trim();
                        editProfile.LastName = txtLastName.Text.Trim();

                        editProfile.CanyonRanchCustomer = cbCanyonRanchCustomer.Checked;

                        if (ddlCoupons.SelectedIndex > 0)
                            editProfile.DefaultCouponId = int.Parse(ddlCoupons.SelectedValue);
                        else
                            editProfile.DefaultCouponId = null;

                        editProfile.Save();

                        if (!ProfilePrefsEdit1.ShowSave)
                            ProfilePrefsEdit1.Save();

                        if (!ProfileAllgsEdit1.ShowSave)
                            ProfileAllgsEdit1.Save();

                        ProfileCartEdit1.Bind();

                        OnSaved(new ControlSavedEventArgs(editProfile.UserProfileID));
                    }
                    OnSaved(new ControlSavedEventArgs(editProfile.UserProfileID));
                }
            }
            catch (ProviderException pex)
            {
                lblFeedback.Text = pex.Message;
            }
            catch
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            CurrentAspNetId = null;
            DisplayProfileTabs(false);

            chkIsLockedOut.Checked = false;
            chkIsLockedOut.Enabled = true;

            cbCanyonRanchCustomer.Checked = false;

            txtEmail.Text = string.Empty;
            txtProfileName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            ddlCoupons.ClearSelection();
            //ddlDeliveryTypes.ClearSelection();

            BillingInfoEdit1.Clear();

            AddressEdit_Shipping1.Clear();
            AddressEdit_Shipping1.EnableFields = false;

            ////preferences
            ProfilePrefsEdit1.Clear();

            ////allergens
            ProfileAllgsEdit1.Clear();

            //subprofiles
            SubProfileEdit1.Clear();
            gvwSubProfiles.DataSource = null;
            gvwSubProfiles.DataBind();


        }

        void PasswordReset1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            if ((Guid)e.PrimaryKeyIndex != Guid.Empty)
                OnPasswordReset(this, new PasswordResetEventArgs(true));
            else
                OnPasswordReset(this, new PasswordResetEventArgs(false));
        }

        void PasswordReset1_ControlCancelled(object sender)
        {
            OnPasswordCancel(this, new PasswordResetEventArgs(false));
        }

        void BillingInfoEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetParentProfileBy(CurrentAspNetId.Value);

            if (CurrentUserProfile != null)
            {
                CurrentUserProfile.BillingAddressID = BillingInfoEdit1.CurrentBillingAddressID;
                CurrentUserProfile.Save();
            }

            // some profile value changed, make sure order numbers for current cart are still inline with one another
            SessionManager.CurrentUserProfileInfoChanged = true;

            ProfileCartEdit1.Bind();
        }

        protected void UserProfileInfo_ControlSaved(object sender, EventArgs e) // Shipping address
        {
            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetParentProfileBy(CurrentAspNetId.Value);

            if (CurrentUserProfile != null)
            {
                CurrentUserProfile.ShippingAddressID = AddressEdit_Shipping1.PrimaryKeyIndex;
                CurrentUserProfile.Save();
            }

            // some profile value changed, make sure order numbers for current cart are still inline with one another
            SessionManager.CurrentUserProfileInfoChanged = true;
        }

        void BindcblRoles()
        {
            if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators"))
                cblRoles.Enabled = true;

            cblRoles.Items.Clear();
            cblRoles.ClearSelection();

            cblRoles.DataSource = Roles.GetAllRoles();
            cblRoles.DataBind();

            ListItem adminItem = cblRoles.Items.FindByValue("Administrators");

            if (adminItem != null && ((Guid)Helpers.LoggedUser.ProviderUserKey == CurrentAspNetId))
            {
                adminItem.Text += " - Cannot remove your own administrative role.";
                adminItem.Enabled = false;
            }

        }

        void cblRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<ListItem> selItems = cblRoles.Items.OfType<ListItem>().Where(a => a.Selected).ToList();
            bool containsConsumer = false;

            selItems.ForEach(delegate(ListItem item)
            {
                if (item.Text == "Customer")
                {
                    containsConsumer = true;
                }
            });

            DisplayProfileTabs(containsConsumer);
        }

        void BindddlCoupons()
        {
            if (ddlCoupons.Items.Count == 0)
            {
                ddlCoupons.DataSource = hccCoupon.GetActive().Where(c=>c.UsageTypeID == 1) ;
                ddlCoupons.DataValueField = "CouponID";
                ddlCoupons.DataTextField = "Title";
                ddlCoupons.DataBind();

                ddlCoupons.Items.Insert(0, new ListItem("Select a default coupon...", "-1"));
            }
        }

         void BindgvwSubProfiles()
        {
            try
            {
                List<hccUserProfile> subProfiles = new List<hccUserProfile>();

                if (CurrentAspNetId.HasValue)
                    CurrentUserProfile = hccUserProfile.GetParentProfileBy(CurrentAspNetId.Value);

                if (CurrentUserProfile != null)
                    subProfiles = CurrentUserProfile.GetSubProfiles();

                gvwSubProfiles.DataSource = subProfiles;
                gvwSubProfiles.DataBind();
            }
            catch
            {
                throw;
            }
        }

        void BindHistory()
        {
            PurchaseHistory1.CurrentAspNetId = CurrentAspNetId.Value;
            //PurchaseHistory1.PrimaryKeyIndex = selectCartId;
            PurchaseHistory1.Bind();
        }

        void BindRecurring()
        {
            RecurringOrders1.CurrentAspNetId = CurrentAspNetId.Value;
            RecurringOrders1.Bind();

        }
        void BindLedger()
        {
            ProfileLedger1.CurrentAspNetUserId = CurrentAspNetId.Value;
            ProfileLedger1.Bind();
        }

        void gvwSubProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindgvwSubProfiles();
            SubProfileEdit1.CurrentParentAspNetId = CurrentAspNetId;
            SubProfileEdit1.CurrentParentProfileId = CurrentUserProfile.UserProfileID;
           
            try
            {
                int subProfileId = int.Parse(gvwSubProfiles.DataKeys[gvwSubProfiles.SelectedIndex].Value.ToString());
                
                SubProfileEdit1.PrimaryKeyIndex = subProfileId;
                SubProfileEdit1.Bind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void gvwSubProfiles_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int subProfileId = int.Parse(gvwSubProfiles.DataKeys[e.RowIndex].Value.ToString());

                hccUserProfile delSub = hccUserProfile.GetById(subProfileId);

                if (delSub != null)
                {
                    delSub.Activation(!delSub.IsActive);
                }

                SubProfileEdit1.Clear();

                BindgvwSubProfiles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void gvwSubProfiles_RowCreated(object sender, GridViewRowEventArgs e)
        {
            try
            {
                var profile = new hccUserProfile();
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lkbDelete = e.Row.Cells[2].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Deactivate");

                    if (lkbDelete != null)
                        lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to deactivate/reactivate this sub-profile?');");
                    
                    if(((HealthyChef.DAL.hccUserProfile)e.Row.DataItem)==null)
                    {
                        BindgvwSubProfiles();
                        profile = (hccUserProfile)e.Row.DataItem;
                    }
                    else
                    {
                         profile = hccUserProfile.GetById(((HealthyChef.DAL.hccUserProfile)e.Row.DataItem).UserProfileID);
                    }
                    
                    //hccUserProfile profile = (hccUserProfile)e.Row.DataItem;

                    if (profile != null && !profile.IsActive)
                    {
                        lkbDelete.Text = "Reactivate";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void SubProfileEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            SessionManager.CurrentUserProfileInfoChanged = true;
            BindgvwSubProfiles();
        }

        protected void SubProfileEdit1_ControlCancelled(object sender)
        {
            gvwSubProfiles.SelectedIndex = -1;
            SubProfileEdit1.Clear();
        }

        protected void cstProfileName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (CurrentAspNetId.HasValue)
            {
                List<hccUserProfile> profs = hccUserProfile.GetBy(CurrentAspNetId.Value);

                profs.ForEach(delegate(hccUserProfile prof)
                {

                    if (prof.ProfileName == txtProfileName.Text.Trim()
                        && prof.MembershipID != CurrentAspNetId)
                    {
                        args.IsValid = false;
                    }
                });
            }
        }

        void ProfileCartEdit1_ControlCancelled(object sender)
        {
            ProfileCartEdit1.Clear();
        }

        protected void ProfileCartEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            BindLedger();
            //int tryInt = 0;
            //if (e.PrimaryKeyIndex != null && int.TryParse(e.PrimaryKeyIndex.ToString(), out tryInt))
            BindHistory(); //tryInt);
        }

        protected void cstValProfile0_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = false;
        }

        void cstValCardInfo0_ServerValidate(object source, ServerValidateEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(cstValCardInfo0.ErrorMessage))
                e.IsValid = false;

            if (!string.IsNullOrWhiteSpace(e.Value))
                cstValCardInfo0.ErrorMessage = e.Value;
        }

        void DisplayProfileTabs(bool isVisible)
        {
            hdnShowTabs.Value = isVisible.ToString().ToLower();

            liBasic.Visible = isVisible;
            liBilling.Visible = isVisible;
            BillingInfoEdit1.Visible = isVisible;
            liShipping.Visible = isVisible;
            AddressEdit_Shipping1.EnableFields = isVisible;
            liPrefs.Visible = isVisible;
            liAllergens.Visible = isVisible;
            liSubProfiles.Visible = isVisible;
            liTransactions.Visible = isVisible;
            liNotes.Visible = isVisible;

            rfvProfileName.Enabled = isVisible;
            rfvFirstName.Enabled = isVisible;
            rfvLastName.Enabled = isVisible;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        protected void ProfileCartEdit1_CartSaved(object sender, CartEventArgs e)
        {
            BindHistory(); //e.CartId);
            BindLedger();

            hdnProfileLastTab.Value = "Purchase";
            lblPurchHistoryFeedback.Text = "Purchase Completed: " + DateTime.Now.ToString();
        }

        void ProfileNotesEdit_ControlCancelled(object sender)
        {
            ((UserProfileNote_Edit)sender).Clear();
        }

        void ProfileLedger1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            if (CurrentUserProfile != null)
            {
                ProfileCartEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                ProfileCartEdit1.Bind();
            }
        }

        protected void btnSaveActivestatus_Click(object sender, EventArgs e)
        {
            MembershipUser user = null;

            if (CurrentAspNetId != null)
                user = Membership.GetUser(CurrentAspNetId);
            else
            {
                try
                {
                    user = Membership.GetUser(Membership.GetUserNameByEmail(txtEmail.Text.Trim()));
                }
                catch (Exception) { }
            }

            if (user != null) // create new profile
            {
                if (chkIsLockedOut.Checked)
                    Helpers.LockUser(user);
                else
                {
                    if (user.IsLockedOut)
                        user.UnlockUser();

                    if (!user.IsApproved)
                    {
                        user.IsApproved = true;
                        chkIsActive.Checked = true;
                    }
                }

                user.IsApproved = chkIsActive.Checked;
                Membership.UpdateUser(user);

                lblSaveActiveStatusFeedback.Text = "User activation status updated. " + DateTime.Now.ToString();
            }
            else
                lblSaveActiveStatusFeedback.Text = "User not found. " + DateTime.Now.ToString();
        }
    }
}