using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Linq;
using System.Data.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

using BayshoreSolutions.WebModules;
using HealthyChef.Common;
using HealthyChef.DAL;
using System.Collections.Generic;

namespace BayshoreSolutions.WebModules.Security.Login
{
    public partial class Login : BayshoreSolutions.WebModules.WebModuleBase
    {
        public enum LoginView
        {
            Login = 0,
            Registration = 1,
            Password = 2,
            LoggedIn = 3
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated)
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('im sree');", true);
                Response.Redirect("~/my-profile.aspx", true);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdoCustomerType.SelectedIndex == 0)
                {
                    //Check to see that the e-mail address hasn't already been taken
                    var users = (from MembershipUser u in Membership.GetAllUsers()
                                 where u.Email == txtEmail.Text.Trim()
                                 select new { Email = u.Email }).ToList();

                    if (users.Count == 0)
                    {
                        usrctrlRegister.Email = txtEmail.Text.Trim();
                        multiViews.ActiveViewIndex = (int)LoginView.Registration;
                        Page.Title = "Account Sign Up";
                    }
                    else
                    {
                        litMessage.Text = "The e-mail address you entered is already in use.";
                    }
                }
                else
                {
                    string userName = Membership.GetUserNameByEmail(txtEmail.Text.Trim());

                    if (userName == null)
                        userName = txtEmail.Text.Trim();

                    if (Membership.ValidateUser(userName, txtPassword.Text.Trim()))
                    {
                        MembershipUser user = Membership.GetUser(userName);
                    string []roles=    Roles.GetRolesForUser(userName);
                        if (user != null)
                        {
                            // ensure user has Customer record in db
                            hccCart unloggedCart = hccCart.GetCurrentCart();
                            hccCart loggedCart = hccCart.GetCurrentCart(user);

                            if (unloggedCart != null)
                            {
                                hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                                if (parentProfile != null) // no profile for user OR is Admin and admin's dont have profiles
                                {
                                    List<hccCartItem> unloggedcartItems = hccCartItem.GetBy(unloggedCart.CartID);
                                    List<hccCartItem> loggedcartItems = hccCartItem.GetBy(loggedCart.CartID);

                                    unloggedcartItems.ToList().ForEach(delegate(hccCartItem item)
                                    {

                                        hccCartItem modelItem = loggedcartItems.FirstOrDefault(a => a.UserProfileID == parentProfile.UserProfileID
                                                && a.DeliveryDate == item.DeliveryDate);

                                        if (modelItem != null)
                                            item.OrderNumber = modelItem.OrderNumber;
                                        else
                                            item.GetOrderNumber(loggedCart);

                                        item.UserProfileID = parentProfile.UserProfileID;

                                        if (item.ItemType == Enums.CartItemType.GiftCard)
                                        {
                                            if (item.Gift_IssuedTo == null || item.Gift_IssuedTo == Guid.Empty)
                                            {
                                                item.Gift_IssuedTo = parentProfile.MembershipID;
                                                item.Gift_IssuedDate = DateTime.Now;
                                            }
                                        }


                                        item.CartID = loggedCart.CartID;
                                        item.Save();
                                    });

                                    unloggedCart.StatusID = (int)Enums.CartStatus.Cancelled;
                                    unloggedCart.Save();
                                }
                            }
                        }
                        if (Request.QueryString["fc"] != null)
                        {
                            FormsAuthentication.SetAuthCookie(userName, true);
                            Response.Redirect("~/cart.aspx?confirm=1", true);
                        }
                        else
                        {
                            // Was user redirected from meal programs due to recurring selection
                            if (Request.QueryString["rp"] != null)
                            {
                                FormsAuthentication.SetAuthCookie(userName, true);
                                Response.Redirect("~/details/" + Request.QueryString["rp"] + "?rc=true");                               
                            }
                            else
                                FormsAuthentication.RedirectFromLoginPage(userName, true);
                        }
                    }
                    else
                    {
                        MembershipUser user = Membership.GetUser(userName);

                        if (user == null || !Membership.ValidateUser(userName, txtPassword.Text.Trim()))
                        {
                            litMessage.Text = "Login Attempt Failed.  Email/password combination not recognized.  Please re-enter your email address and account password.  If you have forgotten your password, please click the link below or call customer service at 866-575-2433 for assistance.";
                        }
                        else if (!user.IsApproved)
                        {
                            litMessage.Text = "That account has been deactivated. Please contact customer service at 866-575-2433 for assistance.";
                        }
                        else if (user.IsLockedOut)
                        { //password lock-out
                            litMessage.Text = "That account is locked out. Please contact customer service at 866-575-2433 for assistance.";
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void linkForgot_Click(object sender, EventArgs e)
        {
            multiViews.ActiveViewIndex = (int)LoginView.Password;
        }
    }
}
