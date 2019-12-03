using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HealthyChef.DAL;
using HealthyChefCreationsMVC.CustomModels;
using HealthyChef.Common;
using HealthyChef.Email;
using HealthyChef.DAL.Extensions;
using System.Data;
using HealthyChefCreationsMVC.Repository;
using AuthorizeNet;
using HealthyChef.AuthNet;
using AuthorizeNet.APICore;
using HealthyChef.Common.Events;
using BayshoreSolutions.WebModules;
using System.Security.Claims;
using Facebook;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Microsoft.Owin.Host.SystemWeb;
using System.Net;
using static HealthyChefCreationsMVC.CustomModels.LoginViewModel;

namespace HealthyChefCreationsMVC.Controllers
{
    public class HomeController : Controller
    {
        protected hccCart CurrentCart;

        public enum LoginView
        {
            Login = 0,
            Registration = 1,
            Password = 2,
            LoggedIn = 3
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            string ItemName = System.Web.HttpContext.Current.Request.UrlReferrer.ToString();
            if (ItemName.Contains("ProductDescription?ItemName="))
            {
                ReturnUrl= System.Web.HttpContext.Current.Request.UrlReferrer.ToString().Split('/')[3];
            }
            ViewBag.ErrorMessage = string.Empty;
            LoginViewModel loginModel = new LoginViewModel(ReturnUrl);
            return View(loginModel);
        }

        [HttpPost]
        public ActionResult RegisterModel(RegistrationModel registrationModel)
        {
            try
            {
                if (registrationModel.CustomerType == 0)
                {
                    //Check to see that the e-mail address hasn't already been taken
                    var users = (from MembershipUser u in Membership.GetAllUsers()
                                 where u.Email == registrationModel.Email.Trim()
                                 select new { Email = u.Email }).ToList();
                    if (users.Count == 0)
                    {
                        var _loginRegistrationModel = new LoginRegistrationViewModel(registrationModel.Email, registrationModel.FirstName, registrationModel.LastName,true);
                        return View("~/Views/Register/Display.cshtml", _loginRegistrationModel);
                    }
                    else
                    {
                        //ModelState.AddModelError("Email", "The e-mail address you entered is already in use.");
                        ViewBag.EmailChecked = "The e-mail address you entered is already in use.";
                        LoginViewModel loginView = new LoginViewModel();
                        loginView.Email = registrationModel.Email;
                        loginView.registrationModel = new RegistrationModel();
                        return View("~/Views/Home/Login.cshtml", loginView); ;
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Login");
        }
        [HttpPost]
        public ActionResult Login(LoginViewModel loginModel)
        {
            try
            {
                loginModel.registrationModel = new RegistrationModel();
                if (loginModel.CustomerType == 1)
                {
                    string userName = Membership.GetUserNameByEmail(loginModel.Email.Trim());
                    if (userName == null)
                        userName = loginModel.Email.Trim();
                    if (Membership.ValidateUser(userName, loginModel.Password.Trim()))
                    {
                        MembershipUser user = Membership.GetUser(userName);
                        string[] roles = Roles.GetRolesForUser(userName);
                        if (roles.Contains("Customer"))
                        {
                            if (user != null)
                            {
                                hccCart unloggedCart = hccCart.GetCurrentCart();
                                hccCart loggedCart = hccCart.GetCurrentCart(user);
                                hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);
                                if (unloggedCart != null)
                                {
                                     parentProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                                    if (parentProfile != null) // no profile for user OR is Admin and admin's dont have profiles
                                    {
                                        List<hccCartItem> unloggedcartItems = hccCartItem.GetBy(unloggedCart.CartID);
                                        List<hccCartItem> loggedcartItems = hccCartItem.GetBy(loggedCart.CartID);

                                        unloggedcartItems.ToList().ForEach(delegate (hccCartItem item)
                                        {

                                            hccCartItem modelItem = loggedcartItems.FirstOrDefault(a => a.UserProfileID == parentProfile.UserProfileID
                                                    && a.DeliveryDate == item.DeliveryDate && a.ItemName == item.ItemName);

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
                                            if (modelItem != null)
                                            {
                                                if (item.ItemName == modelItem.ItemName)
                                                {
                                                    modelItem.Quantity = modelItem.Quantity + 1;
                                                    modelItem.Save();
                                                }
                                                else
                                                {
                                                    item.CartID = loggedCart.CartID;
                                                    item.Save();
                                                }
                                            }
                                            else
                                            {
                                                item.CartID = loggedCart.CartID;
                                                item.CreatedBy = loggedCart.AspNetUserID;
                                                item.Save();
                                            }
                                        });

                                        unloggedCart.StatusID = (int)Enums.CartStatus.Cancelled;
                                        unloggedCart.CouponID = parentProfile.DefaultCouponId;
                                        unloggedCart.Save();
                                        loggedCart.CouponID = parentProfile.DefaultCouponId;
                                        loggedCart.Save();

                                        if (Session["autorenew"] != null)
                                        {
                                            var unloggedrecurringorders = ((List<hccRecurringOrder>)Session["autorenew"]);
                                            if (unloggedrecurringorders.Count() > 0)
                                            {
                                                Session["autorenew"] = null;
                                                foreach (var recurringorder in unloggedrecurringorders)
                                                {
                                                    recurringorder.AspNetUserID = loggedCart.AspNetUserID;
                                                    recurringorder.UserProfileID = parentProfile.UserProfileID;
                                                    Session["autorenew"] = recurringorder;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    loggedCart.CouponID = parentProfile.DefaultCouponId;
                                    loggedCart.Save();
                                }
                            }
                            if (Request.QueryString["fc"] != null)
                            {
                                FormsAuthentication.SetAuthCookie(userName, true);
                                if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
                                {
                                    return Redirect(loginModel.ReturnUrl);
                                }
                                else
                                {
                                    return RedirectToAction("Display");
                                }
                            }
                            else
                            {
                                // Was user redirected from meal programs due to recurring selection
                                if (Request.QueryString["rp"] != null)
                                {
                                    
                                    FormsAuthentication.SetAuthCookie(userName, true);
                                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
                                    {
                                        if (loginModel.ReturnUrl == "/Cart/CartCheckout")
                                        {
                                            return Redirect("/cart.aspx");
                                        }
                                        else
                                        {
                                            return Redirect(loginModel.ReturnUrl);
                                        }
                                    }
                                    else
                                    {
                                        return RedirectToAction("Display");
                                    }

                                }
                                else
                                {
                                    FormsAuthentication.RedirectFromLoginPage(userName, true);
                                    if (!string.IsNullOrEmpty(loginModel.ReturnUrl))
                                    {
                                        if (loginModel.ReturnUrl == "/Cart/CartCheckout")
                                        {
                                            return Redirect("/cart.aspx");
                                        }
                                        else
                                        {
                                            return Redirect(loginModel.ReturnUrl);
                                        }
                                    }
                                    else
                                    {
                                        FormsAuthentication.RedirectFromLoginPage(userName, true);
                                        return RedirectToAction("Display");
                                    }

                                }

                            }
                        }
                        else
                        {
                                ViewBag.ErrorMessage = "Login Attempt Failed.  Email/password combination not recognized in customers data.";
                                return View(loginModel);
                        }
                    }
                    else
                    {
                        MembershipUser user = Membership.GetUser(userName);
                        if (user == null)
                        {
                            ViewBag.ErrorMessage = "Login Attempt Failed.  Email/password combination not recognized.  Please re-enter your email address and account password.  If you have forgotten your password, please click the link above or call customer service at 866-575-2433 for assistance.";
                            return View(loginModel);
                        }
                        else if (!user.IsApproved)
                        {
                            ViewBag.ErrorMessage = "That account has been deactivated. Please contact customer service at 866-575-2433 for assistance.";
                            return View(loginModel);
                        }
                        else if (user.IsLockedOut)
                        {
                            ViewBag.ErrorMessage = "That account is locked out. Please contact customer service at 866-575-2433 for assistance.";
                            return View(loginModel);
                        }
                        else if (!Membership.ValidateUser(userName, loginModel.Password.Trim()))
                        {
                            ViewBag.ErrorMessage = "Login Attempt Failed.  Email/password combination not recognized.  Please re-enter your email address and account password.  If you have forgotten your password, please click the link above or call customer service at 866-575-2433 for assistance.";
                            return View(loginModel);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }

            return View(loginModel);
        }

        #region FB Login
        public ActionResult FacebookLogin(FacebookLoginModel model)
        {
            LoginRegistrationViewModel FacebookloginRegistrationViewModel = new LoginRegistrationViewModel();
            try
            {
                var fb = new FacebookClient();
                fb.AccessToken = model.accessToken;
                System.Web.HttpContext.Current.Session["_accessToken"] = model.accessToken;
                dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,age_range,id");
                string Email = me.id + "@facebook.com";
                var users = (from MembershipUser u in Membership.GetAllUsers()
                             where u.Email == Email
                             select new { Email = u.Email }).ToList();
                if (users.Count > 0)
                {
                    LoginViewModel loginModel = new LoginViewModel();
                    loginModel.Email = Email;
                    loginModel.Password = Email;
                    return Json(loginModel.Email, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        Email = me.id + "@facebook.com";
                        //shipping
                        hccAddress shippingaddress = new hccAddress { Country = "US", AddressTypeID = 4 };
                        int addrId = shippingaddress.AddressID;
                        shippingaddress.FirstName = me.first_name;
                        shippingaddress.LastName = me.last_name;
                        shippingaddress.Address1 ="";
                        shippingaddress.Address2 ="";
                        shippingaddress.City = "";
                        shippingaddress.State = "";
                        shippingaddress.PostalCode = "";
                        shippingaddress.Phone = "";
                        shippingaddress.IsBusiness =false;
                        shippingaddress.DefaultShippingTypeID =1;
                        

                        //Card Information.
                        CardInfo CurrentCardInfo = new CardInfo();

                        CurrentCardInfo.NameOnCard ="";
                        CurrentCardInfo.CardNumber = "";
                        CurrentCardInfo.ExpMonth =0;
                        CurrentCardInfo.ExpYear =0;
                        CurrentCardInfo.SecurityCode ="";

                        CurrentCardInfo.CardType =(int)Enums.CreditCardType.Unknown;
                        
                        var CurrentCart = new hccCart();
                        MembershipUser user = Membership.GetUser();
                        if (user != null)
                        {
                            CurrentCart = hccCart.GetCurrentCart(user);
                        }
                        else
                        {
                            CurrentCart = hccCart.GetCurrentCart();
                        }
                        hccUserProfile newprofile = hccUserProfile.GetById(0);
                        if (newprofile == null)
                        {
                            newprofile = new hccUserProfile();
                            string UserName = Email.Split('@')[0] + DateTime.Now.ToString("yyyyMMddHHmmtt");
                            string password = Email;

                            MembershipCreateStatus CreateResult;
                            MembershipUser newUser = Membership.CreateUser(UserName, password, Email, null, null, true, out CreateResult);

                            if (CreateResult == MembershipCreateStatus.Success)
                            {
                                //Assign Customer role to newUser
                                Roles.AddUserToRole(newUser.UserName, "Customer");
                                //MailHelper.SendConfirmationEmail(model.UserName);

                                //log in user.
                                FormsAuthentication.SetAuthCookie(newUser.UserName, false);

                                //Create a Healthy Chef profile for this new user

                                newprofile.MembershipID = (Guid)newUser.ProviderUserKey;
                                newprofile.CreatedBy = (Membership.GetUser() == null ? Guid.Empty : (Guid)Membership.GetUser().ProviderUserKey);
                                newprofile.CreatedDate = DateTime.Now;
                                newprofile.AccountBalance = 0.00m;
                                newprofile.IsActive = true;
                                newprofile.AccountBalance = 0;
                                newprofile.ModifiedDate = (DateTime?)null;
                                newprofile.UseParentShipping =false;

                                FacebookloginRegistrationViewModel.UserProfileId = newprofile.UserProfileID;
                            }
                        }
                        //shipping

                        shippingaddress.Save();
                        FacebookloginRegistrationViewModel.ShippingAddressID = shippingaddress.AddressID;
                        newprofile.ProfileName = shippingaddress.FirstName + " " + shippingaddress.LastName;
                        newprofile.FirstName = shippingaddress.FirstName;
                        newprofile.LastName = shippingaddress.LastName;
                        newprofile.ProfileName = shippingaddress.ProfileName;
                        newprofile.ShippingAddressID = shippingaddress.AddressID;
                        //billing Address

                        hccAddress billingaddress = new hccAddress { Country = "US", AddressTypeID = 2 };
                        billingaddress.FirstName = me.first_name;
                        billingaddress.LastName = me.last_name;
                        billingaddress.Address1 ="";
                        billingaddress.Address2 = "";
                        billingaddress.City = "";
                        billingaddress.State ="";
                        billingaddress.PostalCode = "";
                        billingaddress.Phone = "";
                        billingaddress.Save();
                        FacebookloginRegistrationViewModel.BillingAddressID = billingaddress.AddressID;
                        newprofile.BillingAddressID = billingaddress.AddressID;

                        //credit card Details.
                        hccUserProfile _userToUpdate = newprofile;
                        if (_userToUpdate != null)
                        {
                            Address billAddr = null;
                            var _customerBillingInfo = hccAddress.GetById(billingaddress.AddressID);

                            if (_customerBillingInfo != null)
                            {
                                billAddr = hccAddress.GetById(_customerBillingInfo.AddressID).ToAuthNetAddress();
                            }
                            if (CurrentCardInfo != null && billAddr != null)
                            {
                                try
                                {
                                    //send card to Auth.net for Auth.net profile
                                    CustomerInformationManager cim = new CustomerInformationManager();
                                    Customer cust = null;
                                    string autnetResult = string.Empty;

                                    if (!string.IsNullOrWhiteSpace(_userToUpdate.AuthNetProfileID))
                                        cust = cim.GetCustomer(_userToUpdate.AuthNetProfileID);

                                    //Will Martinez - Commented out on 7/30/2013.
                                    //This code scans all existing Profiles generated to check for duplicated email addresses, however the site registration prevents that
                                    //commented out since this process had a significant performance effect on the site.

                                    if (cust == null)
                                        cust = cim.CreateCustomer(_userToUpdate.ASPUser.Email, _userToUpdate.ASPUser.UserName);

                                    // had to add it back in, unable to create records with duplicate email addresses caused by IT desynching data.
                                    // this should only be called infrequently since we try to create the account first.
                                    if (cust.ProfileID == null)
                                        cust = cim.GetCustomerByEmail(_userToUpdate.ASPUser.Email, out autnetResult);
                                    if (cust != null)
                                    {
                                        if (_userToUpdate.AuthNetProfileID != cust.ProfileID)
                                        {
                                            _userToUpdate.AuthNetProfileID = cust.ProfileID;
                                            _userToUpdate.Save();
                                        }

                                        List<PaymentProfile> payProfiles = cust.PaymentProfiles.ToList();

                                        if (payProfiles.Count > 0)
                                            payProfiles.ForEach(a => cim.DeletePaymentProfile(_userToUpdate.AuthNetProfileID, a.ProfileID));

                                        // create new payment profile
                                        autnetResult = cim.AddCreditCard(cust, CurrentCardInfo.CardNumber, CurrentCardInfo.ExpMonth,
                                             CurrentCardInfo.ExpYear, CurrentCardInfo.SecurityCode, billAddr);

                                        if (!string.IsNullOrWhiteSpace(autnetResult))
                                        {
                                            // Validate card profile
                                            validateCustomerPaymentProfileResponse valProfile = cim.ValidateProfile(_userToUpdate.AuthNetProfileID,
                                                autnetResult, AuthorizeNet.ValidationMode.TestMode);

                                            if (valProfile.messages.resultCode == messageTypeEnum.Ok)
                                            {
                                                hccUserProfilePaymentProfile activePaymentProfile = null;
                                                activePaymentProfile = _userToUpdate.ActivePaymentProfile;

                                                if (_userToUpdate.ActivePaymentProfile == null)
                                                    activePaymentProfile = new hccUserProfilePaymentProfile();

                                                activePaymentProfile.CardTypeID = (int)CurrentCardInfo.CardType;
                                                activePaymentProfile.CCLast4 = CurrentCardInfo.CardNumber.Substring(CurrentCardInfo.CardNumber.Length - 4, 4);
                                                activePaymentProfile.ExpMon = CurrentCardInfo.ExpMonth;
                                                activePaymentProfile.ExpYear = CurrentCardInfo.ExpYear;
                                                activePaymentProfile.NameOnCard = CurrentCardInfo.NameOnCard;
                                                activePaymentProfile.UserProfileID = _userToUpdate.UserProfileID;
                                                activePaymentProfile.IsActive = true;

                                                activePaymentProfile.AuthNetPaymentProfileID = autnetResult;
                                                activePaymentProfile.Save();
                                            }
                                            else
                                            {
                                                TempData["CreditCardError"] = "Payment Profile has been created, but validation failed.";
                                            }
                                        }
                                        else
                                        {
                                            TempData["CreditCardError"] = "Registered Successfully but Authorize.Net response is empty.";
                                        }
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrEmpty(autnetResult))
                                            TempData["CreditCardError"] = "Registered Successfully but" + autnetResult;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    TempData["CreditCardError"] = "Registered Successfully but Error in updating credit card details " + ex.Message;
                                }
                            }
                            else
                            {
                                TempData["CreditCardError"] = "Registered Successfully but Error in updating credit card details";
                            }
                        }
                        else
                        {
                            TempData["CreditCardError"] = "Registered Successfully but Error in updating credit card details";
                        }

                        //Cart
                        newprofile.Save();
                        CurrentCart.AspNetUserID = newprofile.MembershipID;
                        CurrentCart.Save();

                        List<hccCartItem> cartItems = hccCartItem.GetBy(CurrentCart.CartID);
                        cartItems.ForEach(delegate (hccCartItem ci) { ci.UserProfileID = newprofile.UserProfileID; ci.Save(); });
                        //sending Email to user
                        try
                        {
                            //var confirmEmail = "below link to complete your registration: <a href=\"{1}\"title =\"User Email Confirm\">{1}</a>," + Email; Url.Action("ConfirmEmail", "Account", new { Token = (Guid)(Membership.GetUser(user.UserName, false).ProviderUserKey), Email = user.Email }, Request.Url.Scheme);
                            HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                            ec.SendMail_NewUserConfirmation(Email, "");
                        }
                        catch (Exception E)
                        {
                            //return RedirectToAction("Display", "Home");
                            return Json(Email, JsonRequestBehavior.AllowGet);
                        }
                        //return RedirectToAction("Display", "Home");
                        return Json(Email, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
            return Json("",JsonRequestBehavior.AllowGet);
        }

        public ActionResult DirectHomeDashBoardLoginbyFB(string email)
        {
            FormsAuthentication.SetAuthCookie(email, false);
            Session["Email"] = email;
            return RedirectToAction("Index", "Account", new { activeTab = 4 });
        }

        #endregion


        //[OutputCache(Duration = 120)]
        public ActionResult Display()
        {
            HomeViewModel homeViewModel = new HomeViewModel();
            return View(homeViewModel);
        }
        //ForgotPassword
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            ForgotPasswordViewModel forgot = new ForgotPasswordViewModel();
            return View(forgot);
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel ResetPassword)
        {
            //Declaring the varable to use further.
            var users = (from MembershipUser u in Membership.GetAllUsers()
                         where u.Email == ResetPassword.Email
                         select new { Email = u.Email }).ToList();
            if (users.Count == 0)
            {
                ModelState.AddModelError("Email", "This email is not registered");
                return View(ResetPassword);
            }
            else
            {
                string userName = string.Empty;
                MembershipUser user = null;
                bool success = false;
                string newPassword = string.Empty;
                try
                {
                    userName = Membership.GetUserNameByEmail(ResetPassword.Email);
                    if (!string.IsNullOrWhiteSpace(userName))
                    {
                        user = Membership.GetUser(userName);
                        if (user != null)
                        {
                            string tempPassword = user.ResetPassword();
                            newPassword = OrderNumberGenerator.GenerateOrderNumber("?#?#?#?#");
                            success = user.ChangePassword(tempPassword, newPassword);
                        }
                        if (success)
                        {
                            //send Email
                            EmailController Ec = new EmailController();
                            Ec.SendMail_PasswordReset(ResetPassword.Email, newPassword);
                            ViewBag.Message = "Password Reset Successful - Your new password has been sent to the email address: " + ResetPassword.Email;
                            return View(ResetPassword);
                        }
                        else
                        {
                            ModelState.AddModelError("Email", "Password Reset Failed. Email address not recognized. " +
                            "Please re-enter your email address or call customer service at 866-575-2433 for assistance.");
                            return View(ResetPassword);
                        }
                    }
                }
                catch (MembershipPasswordException ex)
                {
                    ModelState.AddModelError("Email", "Cannot reset password. This account is currently locked.");
                    return View(ResetPassword);
                }
                catch (Exception exstring)
                {
                    ModelState.AddModelError("", exstring);
                    ViewBag.Message = exstring.Message;
                    return View(ResetPassword);
                }
            }
            return View(ResetPassword);
        }

        //SignOut
        public ActionResult Signout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            Roles.DeleteCookie();

            // clear authentication cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            //Clear FB Login Details
            if (!String.IsNullOrEmpty(System.Web.HttpContext.Current.Session["_accessToken"] as string))
            {
                var fb = new FacebookClient();

                var logoutUrl = fb.GetLogoutUrl(new
                {
                    next = "https://www.facebook.com/connect/login_success.html",
                    access_token = System.Web.HttpContext.Current.Session["_accessToken"].ToString()
                });
               
                using (WebClient client = new WebClient())
                {
                  var  s = client.DownloadString(logoutUrl.AbsoluteUri);
                }
             }


            //Clear Cookies
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("Display");
        }
    }
}

