using System;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class BasicInfo_Edit : FormControlBase
    {   // Note: this.PrimaryKeyIndex as hccUserProfileId
        protected hccUserProfile CurrentUserProfile
        {
            get;
            set;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += base.SubmitButtonClick;
        }

        protected override void LoadForm()
        {
            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            MembershipUser user = Membership.GetUser(CurrentUserProfile.MembershipID);

            if (user != null)
                if ((Roles.IsUserInRole(user.UserName, "Customer")) && (CurrentUserProfile != null))
                {
                    lblAccountBalance.Text = CurrentUserProfile.AccountBalance.ToString("c");
                    txtFirstName.Text = CurrentUserProfile.FirstName;
                    txtLastName.Text = CurrentUserProfile.LastName;
                    txtProfileName.Text = CurrentUserProfile.ProfileName;
                    cbMarketingOptIn.Checked = CurrentUserProfile.CanyonRanchCustomer ?? false;
                    txtEmail.Text = user.Email;
                }
        }

        protected override void SaveForm()
        {
            if (CurrentUserProfile == null)
                CurrentUserProfile = hccUserProfile.GetById(this.PrimaryKeyIndex);

            if (CurrentUserProfile != null) //hccProfile exists
            {
                CurrentUserProfile.ProfileName = txtProfileName.Text.Trim();
                CurrentUserProfile.FirstName = txtFirstName.Text.Trim();
                CurrentUserProfile.LastName = txtLastName.Text.Trim();
                CurrentUserProfile.CanyonRanchCustomer = cbMarketingOptIn.Checked;
                CurrentUserProfile.Save();
                lblFeedback0.Text = "Account information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");
            }

            MembershipUser user = Membership.GetUser(CurrentUserProfile.MembershipID);

            if (user != null)
            {
                if (user.Email != txtEmail.Text.Trim()) // update userprofile and aspmembership user
                {
                    user.Email = txtEmail.Text.Trim();
                    try
                    {
                        Membership.UpdateUser(user);
                        lblFeedback0.ForeColor = System.Drawing.Color.Green;
                        lblFeedback0.Text = "Account information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");
                    }
                    catch (ProviderException pex)
                    {
                        lblFeedback0.ForeColor = System.Drawing.Color.Red;
                        lblFeedback0.Text = pex.Message;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        protected override void ClearForm()
        {
            txtProfileName.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }


    }
}