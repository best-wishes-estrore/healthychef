using AuthorizeNet;
using AuthorizeNet.APICore;
using HealthyChef.AuthNet;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Extensions;
using HealthyChefCreationsMVC.CustomModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace HealthyChefCreationsMVC.Controllers
{
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Display()
        {
            var _loginRegistrationModel = new LoginRegistrationViewModel();
            return View(_loginRegistrationModel);
        }

        hccUserProfile GetUserProfileByID(string _userID)
        {
            hccUserProfile profile = new hccUserProfile();

            try
            {
                if (!string.IsNullOrEmpty(_userID))
                {
                    Guid aspNetId = Guid.Parse(_userID);
                    profile = hccUserProfile.GetParentProfileBy(aspNetId);
                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }
            return profile;
        }

        [HttpPost]
        public ActionResult Display(LoginRegistrationViewModel loginRegistrationViewModel)
        {
            var users = (from MembershipUser u in Membership.GetAllUsers()
                             where u.Email == loginRegistrationViewModel.Email
                             select new { Email = u.Email }).ToList();
               if (users.Count > 0)
               {
                   ModelState.AddModelError("Email", "The e-mail address you entered is already in use.");
                   return View(loginRegistrationViewModel);
               }
            if (ModelState.IsValid)
                {
                //shipping
                hccAddress shippingaddress = new hccAddress { Country = "US", AddressTypeID = 4 };
                var CurrentShippingAddress = hccAddress.GetById(loginRegistrationViewModel.ShippingAddressID);

                if (CurrentShippingAddress == null)
                {
                    shippingaddress = new hccAddress { Country = "US", AddressTypeID = 4 };
                }
                else
                {
                    shippingaddress = CurrentShippingAddress;
                }

                int addrId = shippingaddress.AddressID;
                shippingaddress.FirstName = loginRegistrationViewModel.ShippingFirstName;
                shippingaddress.LastName = loginRegistrationViewModel.ShippingLastName;
                shippingaddress.Address1 = loginRegistrationViewModel.ShippingAddress1;
                shippingaddress.Address2 = loginRegistrationViewModel.ShippingAddress2;
                shippingaddress.City = loginRegistrationViewModel.ShippingCity;
                shippingaddress.State = loginRegistrationViewModel.ShippingState;
                shippingaddress.PostalCode = loginRegistrationViewModel.ShippingPostalCode;
                shippingaddress.Phone = loginRegistrationViewModel.ShippingPhone;
                shippingaddress.IsBusiness = loginRegistrationViewModel.IsBusiness;
                shippingaddress.DefaultShippingTypeID = loginRegistrationViewModel.DefaultShippingTypeID;
               
                if (loginRegistrationViewModel.DefaultShippingTypeID == 2)
                {
                    string ZipCode = loginRegistrationViewModel.ShippingPostalCode;
                    hccShippingZone hccshopin = new hccShippingZone();
                    DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
                    int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());
                    DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                    if (IsPickup == "True")
                    {

                    }
                    else
                    {
                        ModelState.AddModelError("ShippingPostalCode", "Customer pickup is not available at this Zip Code");
                        return View(loginRegistrationViewModel);
                    }
                }
                
                //Card Information.
                CardInfo CurrentCardInfo = new CardInfo();

                CurrentCardInfo.NameOnCard = loginRegistrationViewModel.NameOnCard;
                CurrentCardInfo.CardNumber = loginRegistrationViewModel.CardNumber;
                CurrentCardInfo.ExpMonth = loginRegistrationViewModel.ExipiresOnMonth;
                CurrentCardInfo.ExpYear = loginRegistrationViewModel.ExipiresOnYear;
                CurrentCardInfo.SecurityCode = loginRegistrationViewModel.CardIdCode;

                CurrentCardInfo.CardType = ValidateCardNumber(loginRegistrationViewModel.CardNumber);

                if (CurrentCardInfo.CardType == Enums.CreditCardType.Unknown)
                {
                    ModelState.AddModelError("CardNumber", "Enter a valid card number.");
                    return View(loginRegistrationViewModel);
                }
                
                   DateTime dtSelected = new DateTime(loginRegistrationViewModel.ExipiresOnYear, loginRegistrationViewModel.ExipiresOnMonth, 1).AddMonths(1);
                   bool yearandmonth = dtSelected.CompareTo(DateTime.Now) > 0;
                   if (!yearandmonth)
                   {
                       ModelState.AddModelError("ExipiresOnYear", "Select a current or future month.");
                       return View(loginRegistrationViewModel);
                   }

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

                hccUserProfile newprofile =hccUserProfile.GetById(loginRegistrationViewModel.UserProfileId);
                if (newprofile == null)
                {
                    newprofile = new hccUserProfile();
                    string Email = loginRegistrationViewModel.Email;
                    string UserName = Email.Split('@')[0] + DateTime.Now.ToString("yyyyMMddHHmmtt");
                    string password = loginRegistrationViewModel.RegiPassword;

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

                        loginRegistrationViewModel.UserProfileId = newprofile.UserProfileID;
                    }
                }
                //shipping

                    shippingaddress.Save();
                    loginRegistrationViewModel.ShippingAddressID = shippingaddress.AddressID;
                    newprofile.FirstName = shippingaddress.FirstName;
                    newprofile.LastName = shippingaddress.LastName;
                    newprofile.ProfileName = shippingaddress.FirstName;
                    newprofile.ShippingAddressID = shippingaddress.AddressID;
                    //billing Address
                   
                    hccAddress billingaddress = new hccAddress { Country = "US", AddressTypeID = 2 };
                    var CurrentBillingAddress = hccAddress.GetById(loginRegistrationViewModel.BillingAddressID);

                    if (CurrentBillingAddress == null)
                    {
                        billingaddress = new hccAddress { Country = "US", AddressTypeID = 2 };
                    }
                    else
                    {
                        billingaddress = CurrentBillingAddress;
                    }
                    billingaddress.FirstName = loginRegistrationViewModel.BillingFirstName;
                    billingaddress.LastName = loginRegistrationViewModel.BillingLastName;
                    billingaddress.Address1 = loginRegistrationViewModel.BillingAddress1;
                    billingaddress.Address2 = loginRegistrationViewModel.BillingAddress2;
                    billingaddress.City = loginRegistrationViewModel.BillingCity;
                    billingaddress.State = loginRegistrationViewModel.BillingState;
                    billingaddress.PostalCode = loginRegistrationViewModel.BillingPostalCode;
                    billingaddress.Phone = loginRegistrationViewModel.BillingPhone;
                    billingaddress.Save();
                    loginRegistrationViewModel.BillingAddressID = billingaddress.AddressID;
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
                        if (CurrentCardInfo !=null && billAddr != null)
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
                    //var confirmEmail = "below link to complete your registration: <a href=\"{1}\"title =\"User Email Confirm\">{1}</a>," + loginRegistrationViewModel.Email; Url.Action("ConfirmEmail", "Account", new { Token = (Guid)(Membership.GetUser(user.UserName, false).ProviderUserKey), Email = user.Email }, Request.Url.Scheme);
                    HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                    ec.SendMail_NewUserConfirmation(loginRegistrationViewModel.Email, loginRegistrationViewModel.RegiPassword);
                }
                    catch(Exception E)
                    {
                    //return RedirectToAction("Display", "Home");
                    return RedirectToAction("Index", new RouteValueDictionary( new { controller ="Account", action = "Index"}));
                    }
                //return RedirectToAction("Display", "Home");
                return RedirectToAction("Index", new RouteValueDictionary(new { controller = "Account", action = "Index" }));
            }
            return View(loginRegistrationViewModel);
        }
        
        private Enums.CreditCardType ValidateCardNumber(string sCardNumber)
        {
            string cardNum = sCardNumber.Replace(" ", "");

            Enums.CreditCardType retVal = Enums.CreditCardType.Unknown;

            //validate the type of card is accepted
            if (cardNum.StartsWith("4") == true &&
                (cardNum.Length == 13
                    || cardNum.Length == 16))
            {
                //VISA
                retVal = Enums.CreditCardType.Visa;
            }
            else if ((cardNum.StartsWith("51") == true ||
                      cardNum.StartsWith("52") == true ||
                      cardNum.StartsWith("53") == true ||
                      cardNum.StartsWith("54") == true ||
                      cardNum.StartsWith("55") == true) &&
                     cardNum.Length == 16)
            {
                //MasterCard
                retVal = Enums.CreditCardType.MasterCard;
            }
            else if ((cardNum.StartsWith("34") == true ||
                      cardNum.StartsWith("37") == true) &&
                     cardNum.Length == 15)
            {
                //Amex
                retVal = Enums.CreditCardType.AmericanExpress;
            }
            //else if ((cardNum.StartsWith("300") == true ||
            //          cardNum.StartsWith("301") == true ||
            //          cardNum.StartsWith("302") == true ||
            //          cardNum.StartsWith("304") == true ||
            //          cardNum.StartsWith("305") == true ||
            //          cardNum.StartsWith("36") == true ||
            //          cardNum.StartsWith("38") == true) &&
            //         cardNum.Length == 14)
            //{
            //    //Diners Club/Carte Blanche
            //    retVal = Enums.CreditCardType.DinersClub;
            //}
            else if (cardNum.StartsWith("6011") == true &&
                     cardNum.Length == 16)
            {
                //Discover
                retVal = Enums.CreditCardType.Discover;
            }

            if (retVal != Enums.CreditCardType.Unknown)
            {
                int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
                int checksum = 0;
                char[] chars = cardNum.ToCharArray();
                for (int i = chars.Length - 1; i > -1; i--)
                {
                    int j = ((int)chars[i]) - 48;
                    checksum += j;
                    if (((i - chars.Length) % 2) == 0)
                        checksum += DELTAS[j];
                }

                if ((checksum % 10) != 0)
                    retVal = Enums.CreditCardType.Unknown;
            }

            return retVal;
        }

    }
}