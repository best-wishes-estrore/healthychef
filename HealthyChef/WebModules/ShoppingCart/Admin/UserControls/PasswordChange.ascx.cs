using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using HealthyChef.Common;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class ChangePassword : FormControlBase
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);        
        }            

        protected override void LoadForm()
        {            
        }

        protected override void SaveForm()
        {
            MembershipUser user = Helpers.LoggedUser;

            if (user != null) //Customer does not exist create asp.net user.        
            {
                if (Membership.ValidateUser(user.UserName, txtCurrentPassword.Text.Trim()))
                {
                    try
                    {
                        user.ChangePassword(txtCurrentPassword.Text.Trim(), txtNewPassword.Text.Trim());

                        lblFeedback.Text = "Password has been changed.";

                        Email.EmailController ec = new Email.EmailController();
                        ec.SendMail_PasswordChanged(user.Email);

                        OnSaved(new ControlSavedEventArgs(user.ProviderUserKey));
                    }
                    catch (Exception ex)
                    {
                        lblFeedback.Text = ex.Message;
                    }
                }
            }            
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtCurrentPassword.Text = string.Empty;
            txtNewPassword.Text = string.Empty;
            txtNewPasswordConfirm.Text = string.Empty;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }       
    }
}