using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class PasswordReset : FormControlBase
    {
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

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
        }

        protected override void LoadForm()
        {
            
        }

        protected override void SaveForm()
        {
            MembershipUser user = null;

            if (CurrentAspNetId.HasValue)
                user = Membership.GetUser(CurrentAspNetId.Value);

            if (user != null)
            {
                if (user.IsLockedOut)
                {
                    OnCancelled();
                }
                else
                {
                    string newPassword = OrderNumberGenerator.GenerateOrderNumber("?#?#?#?#");
                    string tempPassword = user.ResetPassword();

                    bool success = user.ChangePassword(tempPassword, newPassword);

                    // send email
                    Email.EmailController ec = new Email.EmailController();
                    ec.SendMail_PasswordReset(user.Email, newPassword);

                    OnSaved(new ControlSavedEventArgs(CurrentAspNetId));
                }
            }
        }

        protected override void ClearForm()
        {
            throw new NotImplementedException();
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }
    }
}