using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using BayshoreSolutions.WebModules.Cms.Security.Model;

using HealthyChef.Common;
using HealthyChef.DAL;
using System.Security.Policy;

namespace BayshoreSolutions.WebModules.Cms.Security.UserRegistration
{
    public partial class Display :WebModuleBase
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
        protected hccCart CurrentCart { get; set; }
        protected bool DisableRedirect { get; set; }
        protected hccUserProfile CurrentUserProfile { get; set; }
        public String Email { get { return txtEmail.Text; } set { txtEmail.Text = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            try
            {
                Page.Validate("NewUserGroup");

                if (Page.IsValid)
                {
                    //Fill cart from anonymous user                
                    MembershipUser user = Membership.GetUser();

                    if (user != null)
                        CurrentCart = hccCart.GetCurrentCart(user);
                    else
                        CurrentCart = hccCart.GetCurrentCart();

                    UserRegistration_Module userRegistrationModule = null;
                    WebpageInfo confirmPage = null;

                    if (null != this.WebModuleInfo)
                        UserRegistration_Module.Get(this.WebModuleInfo.Id);

                    //formulate username
                    if (CurrentAspNetId != null)
                        user = Membership.GetUser(CurrentAspNetId);

                    string email = txtEmail.Text.Trim();
                    string userName = email.Split('@')[0] + DateTime.Now.ToString("yyyyMMddHHmmtt");
                    string password = txtPassword.Text; //OrderNumberGenerator.GenerateOrderNumber("?#?#?#?#");

                    MembershipCreateStatus createResult;
                    MembershipUser newUser = Membership.CreateUser(userName, password, email, null, null, true, out createResult);

                    if (createResult == MembershipCreateStatus.Success)
                    {
                        //Assign Customer role to newUser
                        Roles.AddUserToRole(newUser.UserName, "Customer");

                        //log in user.
                        FormsAuthentication.SetAuthCookie(newUser.UserName, false);

                        //Create a Healthy Chef profile for this new user
                        hccUserProfile newProfile = new hccUserProfile
                        {
                            MembershipID = (Guid)newUser.ProviderUserKey,
                            CreatedBy = (Membership.GetUser() == null ? Guid.Empty : (Guid)Membership.GetUser().ProviderUserKey),
                            CreatedDate = DateTime.Now,
                            AccountBalance = 0.00m,
                            IsActive = true
                        };

                        //save Shipping Address
                        AddressEdit_Shipping1.Save();
                        newProfile.ShippingAddressID = AddressEdit_Shipping1.PrimaryKeyIndex;
                        newProfile.FirstName = AddressEdit_Shipping1.CurrentAddress.FirstName.Trim();
                        newProfile.LastName = AddressEdit_Shipping1.CurrentAddress.LastName.Trim();
                        newProfile.ProfileName = AddressEdit_Shipping1.CurrentAddress.FirstName.Trim();

                        //save Billing Address
                        AddressEdit_Billing1.Save();
                        newProfile.BillingAddressID = AddressEdit_Billing1.PrimaryKeyIndex;

                        //Save all hccProfile information
                        newProfile.Save();

                        //Credit Card   
                        try
                        {
                            CreditCard1.CurrentUserProfileID = newProfile.UserProfileID;
                            CreditCard1.Save();
                        }
                        catch { }

                        //Update previously anonymously-created hccCart
                        CurrentCart.AspNetUserID = newProfile.MembershipID;
                        CurrentCart.Save();

                        List<hccCartItem> cartItems = hccCartItem.GetBy(CurrentCart.CartID);
                        cartItems.ForEach(delegate(hccCartItem ci) { ci.UserProfileID = newProfile.UserProfileID; ci.Save(); });

                        //Send E-mail notification to account user
                        try
                        {
                            HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                            ec.SendMail_NewUserConfirmation(email, password);
                        }
                        catch { }

                        if (null != userRegistrationModule)
                        {
                            if (!string.IsNullOrEmpty(userRegistrationModule.NotifyEmailAddress))
                            {
                                SecurityEmail.Send(userRegistrationModule.NotifyEmailAddress,
                                    "New user registration",
                                    "A new user is waiting for approval. To manage users, click this link:\n"
                                        + Request.Url.Scheme
                                        + "://"
                                        + Request.Url.Authority
                                        + "/WebModules/Security/Manage/UserList.aspx"
                                );
                            }

                            if (!DisableRedirect)
                            {
                                confirmPage = Webpage.GetWebpage(userRegistrationModule.ConfirmationPageNavigationId);
                                if (null != confirmPage)
                                    Response.Redirect(confirmPage.Path);
                                else
                                {
                                    if (Request.QueryString["fc"] != null)
                                    {
                                        HttpContext.Current.Response.Redirect("~/cart.aspx?confirm=1", false); 
                                    }
                                    else
                                    {
                                        if (newUser != null)
                                            HttpContext.Current.Response.Redirect(FormsAuthentication.GetRedirectUrl(newUser.UserName, false));
                                        else
                                            HttpContext.Current.Response.Redirect("~/", false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (Request.QueryString["fc"] != null)
                            {
                                HttpContext.Current.Response.Redirect("~/cart.aspx?confirm=1", false);
                            }
                            else
                            {
                                if (newUser != null)
                                    HttpContext.Current.Response.Redirect(FormsAuthentication.GetRedirectUrl(newUser.UserName, false));
                                else
                                    HttpContext.Current.Response.Redirect("~/", false);
                            }
                        }

                    }
                    else Msg.ShowError(UserRegistration_Module.GetHumanStatusMessage(createResult));
                }
            }
            catch (Exception )
            {
                throw;
            }
        }

        protected void chxSameBillingAddress_CheckedChanged(object sender, EventArgs e)
        {

            if (chxSameBillingAddress.Checked)
            {
                AddressEdit_Billing1.CurrentAddress = AddressEdit_Shipping1.GetCloningAddress();
                AddressEdit_Billing1.Bind();
            }
            else
            {
                AddressEdit_Billing1.Clear();
            }
        }

    }
}
