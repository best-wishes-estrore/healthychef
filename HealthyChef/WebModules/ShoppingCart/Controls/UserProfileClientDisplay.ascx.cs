using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart
{
    public partial class UserProfileClientDisplay : FormControlBase
    {   // Note: this.PrimaryKeyIndex as hccUserProfileId
        protected hccUserProfile CurrentUserProfile
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SubProfileEdit1.ControlCancelled += new ControlCancelledEventHandler(SubProfileEdit1_ControlCancelled);
            SubProfileEdit1.ControlSaved += new ControlSavedEventHandler(SubProfileEdit1_ControlSaved);

            gvwSubProfiles.RowCreated += new GridViewRowEventHandler(gvwSubProfiles_RowCreated);
            gvwSubProfiles.RowDeleting += new GridViewDeleteEventHandler(gvwSubProfiles_RowDeleting);
            gvwSubProfiles.SelectedIndexChanged += new EventHandler(gvwSubProfiles_SelectedIndexChanged);

            AddressEdit_Shipping1.ControlSaved += UserProfileInfo_ControlSaved;
            BillingInfoEdit1.ControlSaved += UserProfileInfo_ControlSaved;
            SubProfileEdit1.ControlSaved += SubProfileEdit1_ControlSaved;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }

        protected override void LoadForm()
        {
            try
            {
                MembershipUser user = Helpers.LoggedUser;

                //form fields
                if (user != null)
                {
                    CurrentUserProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                    if (Roles.IsUserInRole("Customer"))
                    {
                        if (CurrentUserProfile == null)
                        {
                            CurrentUserProfile = new hccUserProfile
                            {
                                AccountBalance = 0.0m,
                                CreatedBy = (Guid)user.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                MembershipID = (Guid)user.ProviderUserKey,
                                ModifiedBy = (Guid)user.ProviderUserKey,
                                ModifiedDate = DateTime.Now
                            };

                            CurrentUserProfile.Save();
                        }

                        if (CurrentUserProfile != null)
                        {
                            this.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;

                            //Basic Info
                            BasicEdit1.PrimaryKeyIndex = this.PrimaryKeyIndex;
                            BasicEdit1.Bind();

                            //shipping address    
                            if (CurrentUserProfile.ShippingAddressID.HasValue)
                            {
                                AddressEdit_Shipping1.PrimaryKeyIndex = CurrentUserProfile.ShippingAddressID.Value;
                                AddressEdit_Shipping1.Bind();
                            }

                            ProfileNotesEdit_Shipping.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                            ProfileNotesEdit_Shipping.Bind();

                            //billing address
                            BillingInfoEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                            BillingInfoEdit1.CurrentBillingAddressID = CurrentUserProfile.BillingAddressID;
                            BillingInfoEdit1.Bind();

                            ProfileNotesEdit_Billing.CurrentUserProfileId = CurrentUserProfile.UserProfileID;
                            ProfileNotesEdit_Billing.Bind();

                            ProfilePrefsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                            ProfilePrefsEdit1.Bind();

                            ProfileAllgsEdit1.PrimaryKeyIndex = CurrentUserProfile.UserProfileID;
                            ProfileAllgsEdit1.Bind();

                            //subprofiles
                            BindgvwSubProfiles();
                            SubProfileEdit1.CurrentParentAspNetId = (Guid)user.ProviderUserKey;
                            SubProfileEdit1.CurrentParentProfileId = CurrentUserProfile.UserProfileID;

                            //order history
                            PurchaseHistory1.CurrentAspNetId = (Guid)user.ProviderUserKey;
                            PurchaseHistory1.Bind();

                            //Recurring Orders
                            UserProfileRecurringOrders.CurrentAspNetId = (Guid)user.ProviderUserKey;
                            UserProfileRecurringOrders.Bind();
                            
                        }
                    }
                    else
                    {
                        //This Account exists but doesn't have the Customer Role    
                        //pnl_js_noncustomer.Visible = true;
                        li_link_01.Visible = false;
                        panel1.Visible = false;
                        li_link_02.Visible = false;
                        panel2.Visible = false;
                        li_link_03.Visible = false;
                        panel3.Visible = false;
                        li_link_04.Visible = false;
                        panel4.Visible = false;
                        li_link_05.Visible = false;
                        panel5.Visible = false;
                        li_link_06.Visible = false;
                        panel6.Visible = false;
                        li_link_07.Visible = false;
                        panel7.Visible = false;
                        li_link_09.Visible = false;
                        panel9.Visible = false;

                        if (CurrentUserProfile != null)
                        {
                            CurrentUserProfile.Activation(false);
                        }
                    }
                }
                else
                {
                    FormsAuthentication.RedirectToLoginPage();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SaveForm()
        {
            // no save here, individual saves in member controls
        }

        protected override void ClearForm()
        {
            BillingInfoEdit1.Clear();

            AddressEdit_Shipping1.Clear();
            AddressEdit_Shipping1.EnableFields = false;

            //subprofiles
            SubProfileEdit1.Clear();
            gvwSubProfiles.DataSource = null;
            gvwSubProfiles.DataBind();
        }

        protected void UserProfileInfo_ControlSaved(object sender, EventArgs e)
        {
            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            if (CurrentUserProfile != null)
            {
                CurrentUserProfile.ShippingAddressID = AddressEdit_Shipping1.PrimaryKeyIndex;
                CurrentUserProfile.Save();
            }

            // some profile value changed, make sure order numbers for current cart are still inline with one another
            SessionManager.CurrentUserProfileInfoChanged = true;
        }

        void BindgvwSubProfiles()
        {
            try
            {
                List<hccUserProfile> subProfiles = new List<hccUserProfile>();

                CurrentUserProfile = hccUserProfile.GetParentProfileBy((Guid)Helpers.LoggedUser.ProviderUserKey);

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

        void BindgvwOrders()
        {
            try
            {
                PurchaseHistory1.CurrentAspNetId = CurrentUserProfile.MembershipID;
                PurchaseHistory1.Bind();

                //List<hcc> orders = hcc_Order.GetBy(this.PrimaryKeyIndex);

                //gvwOrders.DataSource = orders;
                //gvwOrders.DataBind();
            }
            catch
            {
                throw;
            }
        }

        void gvwSubProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    LinkButton lkbDelete = e.Row.Cells[2].Controls.OfType<LinkButton>().SingleOrDefault(a => a.Text == "Deactivate");

                    if (lkbDelete != null)
                        lkbDelete.Attributes.Add("onclick", "return confirm('Are you sure that you want to deactivate/reactivate this sub-profile?');");

                    hccUserProfile profile = (hccUserProfile)e.Row.DataItem;

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

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }
    }
}