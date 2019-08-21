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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthyChefCreationsMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index(int activeTab)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = activeTab;

            return View(accountViewModel);
        }

        #region All Get Results
        [HttpGet]
        public ActionResult UpdateBasicInfo()
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateShippingInfo()
        {
            return RedirectToAction("Index",new { activeTab = 2 });
        }

        [HttpGet]
        public ActionResult UpdateBillingInfo()
        {
            return RedirectToAction("Index", new { activeTab = 3 });
        }

        [HttpGet]
        public ActionResult UpdateCreditCardInfo()
        {
            return RedirectToAction("Index", new { activeTab = 3 });
        }

        [HttpGet]
        public ActionResult UpdateCustomerPassword()
        {
            return RedirectToAction("Index", new { activeTab = 9 });
        }

        [HttpGet]
        public ActionResult UpdateCustomerPrefInfo()
        {
            return RedirectToAction("Index", new { activeTab = 4 });
        }

        [HttpGet]
        public ActionResult UpdateCustomerAllergensInfo()
        {
            return RedirectToAction("Index", new { activeTab = 5 });
        }
        #endregion
        [HttpPost]
        public JsonResult GetorderdetailsbyCartid(int Cartid)
        {
            var cartitemdeatils = "";
            try
            {
                var orderdetails = hccCart.GetById(Cartid);
                 cartitemdeatils = orderdetails.ToHtml();
            }
            catch(Exception ex)
            {
                cartitemdeatils = "";
            }
            return Json(new { Orderdetailsbycartid = cartitemdeatils }, JsonRequestBehavior.AllowGet);
        }

        ////partial views posting

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
        public ActionResult UpdateBasicInfo(CustomerBasicInfo _customerBasicInfo)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 1;

            if(ModelState.IsValid)
            {
                try
                {
                    hccUserProfile _userToUpdate = GetUserProfileByID(_customerBasicInfo.UserID);
                    if (_userToUpdate != null)
                    {
                        _userToUpdate.FirstName = _customerBasicInfo.FirstName;
                        _userToUpdate.LastName = _customerBasicInfo.LastName;
                        _userToUpdate.ProfileName = _customerBasicInfo.ProfileName;
                        _userToUpdate.Save();

                        object providerUserKey = new Guid(_customerBasicInfo.UserID);
                        var _membershipUser = Membership.GetUser(providerUserKey, false);

                        if (_membershipUser.Email != _customerBasicInfo.Email)
                        {
                            _membershipUser.Email = _customerBasicInfo.Email;
                            Membership.UpdateUser(_membershipUser);
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login","Home");
                    }

                }
                catch (Exception ex)
                {

                }

                //ModelState.AddModelError("", "Account information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss"));
                ViewBag.UpdateBasicInfoMessage = "Account information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");
                accountViewModel.CustomerBasicInfoModel = _customerBasicInfo;
                return View("Index", accountViewModel);
                //return RedirectToAction("Index", new { activeTab = 1 });
            }

            accountViewModel.CustomerBasicInfoModel = _customerBasicInfo;
            return View("Index", accountViewModel);
            //return RedirectToAction("Index", new { activeTab = 1 });
        }

        [HttpPost]
        public ActionResult UpdateShippingInfo(CustomerShippinInfo _customerShippingInfo)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 2;

            if (ModelState.IsValid)
            {
                try
                {
                    hccUserProfile _userToUpdate = GetUserProfileByID(_customerShippingInfo.UserID);
                    if (_userToUpdate != null)
                    {
                        var CurrentAddress = hccAddress.GetById(_customerShippingInfo.ShippingAddressID);
                        hccAddress address;

                        if (CurrentAddress == null)
                        {
                            address = new hccAddress { Country = "US", AddressTypeID = 4 };
                        }
                        else
                        {
                            address = CurrentAddress;
                        }
                        int addrId = address.AddressID;
                        address.FirstName = _customerShippingInfo.ShippingFirstName;
                        address.LastName = _customerShippingInfo.LastName;
                        address.Address1 = _customerShippingInfo.Address1;
                        address.Address2 = _customerShippingInfo.Address2;
                        address.City = _customerShippingInfo.City;
                        address.State = _customerShippingInfo.State;
                        address.PostalCode = _customerShippingInfo.PostalCode;
                        address.Phone = _customerShippingInfo.Phone;
                        address.IsBusiness = _customerShippingInfo.IsBusiness;
                        address.DefaultShippingTypeID = _customerShippingInfo.DefaultShippingTypeID;

                        if (_customerShippingInfo.DefaultShippingTypeID == 2)
                        {
                            string ZipCode = _customerShippingInfo.PostalCode;
                            hccShippingZone hccshopin = new hccShippingZone();
                            DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
                            int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());
                            DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                            string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                            if (IsPickup == "True")
                            {
                                address.Save();
                                ViewBag.UpdateShippingInfoMessage = "Shipping information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");
                                accountViewModel.CustomerShippinInfoModel = _customerShippingInfo;
                                return View("Index", accountViewModel);
                                //return RedirectToAction("Index", new { activeTab = 2 });
                            }
                            else
                            {
                                ModelState.AddModelError("PostalCode", "Customer pickup is not available at this Zip Code");
                            }
                        }
                        else
                        {
                            //Save
                            address.Save();
                            ViewBag.UpdateShippingInfoMessage = "Shipping information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");
                            accountViewModel.CustomerShippinInfoModel = _customerShippingInfo;
                            return View("Index", accountViewModel);
                            //return RedirectToAction("Index", new { activeTab = 2 });
                        }

                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
                catch (Exception ex)
                {

                }
            }

            accountViewModel.CustomerShippinInfoModel = _customerShippingInfo;
            return View("Index", accountViewModel);
            //return RedirectToAction("Index", new { activeTab = 2 });
        }

        [HttpPost]
        public ActionResult UpdateBillingInfo(CustomerBillingInfo _customerBillingInfo)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 3;

            if (ModelState.IsValid)
            {
                try
                {
                    hccAddress address;
                    hccUserProfile _userToUpdate = GetUserProfileByID(_customerBillingInfo.UserID);
                    if (_userToUpdate != null)
                    {
                        var CurrentAddress = hccAddress.GetById(_customerBillingInfo.BillingAddressID);
                        //hccAddress address;

                        if (CurrentAddress == null)
                        {
                            address = new hccAddress { Country = "US", AddressTypeID = 2};
                        }
                        else
                        {
                            address = CurrentAddress;
                        }
                        int addrId = address.AddressID;
                        address.FirstName = _customerBillingInfo.BillingFirstName;
                        address.LastName = _customerBillingInfo.BillingLastName;
                        address.Address1 = _customerBillingInfo.BillingAddress1;
                        address.Address2 = _customerBillingInfo.BillingAddress2;
                        address.City = _customerBillingInfo.BillingCity;
                        address.State = _customerBillingInfo.BillingState;
                        address.PostalCode = _customerBillingInfo.BillingPostalCode;
                        address.Phone = _customerBillingInfo.BillingPhone;

                        address.Save();
                        ViewBag.UpdateBillingInfoMessage = "Billing address saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");
                        accountViewModel.CustomerBillingInfoModel = _customerBillingInfo;
                        return View("Index", accountViewModel);
                        //return RedirectToAction("Index", new { activeTab = 3 });
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
                catch (Exception ex)
                {

                }
            }
            //ViewBag.UpdateBillingInfoMessage = "Billing address saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");
            accountViewModel.CustomerBillingInfoModel = _customerBillingInfo;
            return View("Index", accountViewModel);
            //return RedirectToAction("Index", new { activeTab = 3 });
        }

        [HttpPost]
        public ActionResult UpdateCreditCardInfo(CustomerCreditCardInfo _customerCreditCardInfo)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 3;
            hccUserProfile _userToUpdate = GetUserProfileByID(_customerCreditCardInfo.UserID);
            if (_customerCreditCardInfo.UpdateCreditCardInfo)
            {
                if (ModelState.IsValid)
                {
                    CardInfo CurrentCardInfo = new CardInfo();

                    CurrentCardInfo.NameOnCard = _customerCreditCardInfo.NameOnCard;
                    CurrentCardInfo.CardNumber = _customerCreditCardInfo.CardNumber;
                    //CurrentCardInfo.CardType = _customerBillingInfo.CardType;
                    CurrentCardInfo.ExpMonth = _customerCreditCardInfo.ExipiresOnMonth;
                    CurrentCardInfo.ExpYear = _customerCreditCardInfo.ExipiresOnYear;
                    CurrentCardInfo.SecurityCode = _customerCreditCardInfo.CardIdCode;

                    DateTime ExpireYearMnth = new DateTime(_customerCreditCardInfo.ExipiresOnYear, _customerCreditCardInfo.ExipiresOnMonth, 1).AddMonths(1);
                    bool yearandmonth = ExpireYearMnth.CompareTo(DateTime.Now) > 0;
                    if (!yearandmonth)
                    {
                        ModelState.AddModelError("ExipiresOnYear", "Select a current or future month.");
                        return View("Index", accountViewModel);
                    }

                    CurrentCardInfo.CardType = ValidateCardNumber(_customerCreditCardInfo.CardNumber);
                    if (CurrentCardInfo.CardType == Enums.CreditCardType.Unknown)
                    {
                        ModelState.AddModelError("CardNumber", "Enter a valid card number.");
                        //ViewBag.UpdateCreditCardInfoErrors = "Enter a valid card number.";
                        _customerCreditCardInfo.UpdateCreditCardInfo = true;
                        accountViewModel.CustomerCreditCardInfoModel = _customerCreditCardInfo;
                        return View("Index", accountViewModel);
                    }

                    if (_userToUpdate != null)
                    {

                        Address billAddr = null;
                        var _customerBillingInfo = hccAddress.GetById(_userToUpdate.BillingAddressID ?? 0);

                        if (_customerBillingInfo != null)
                        {
                            billAddr = hccAddress.GetById(_customerBillingInfo.AddressID).ToAuthNetAddress();
                        }
                        if (CurrentCardInfo.HasValues && billAddr != null)
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
                                //if (cust == null)
                                //    cust = cim.GetCustomerByEmail(userProfile.ASPUser.Email);

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

                                            ViewBag.UpdateCreditCardInfoMessage = "Payment Profile is saved";
                                            accountViewModel.CustomerCreditCardInfoModel = _customerCreditCardInfo;
                                            CustomerCreditCardInfo c = new CustomerCreditCardInfo(_userToUpdate);
                                            accountViewModel.CustomerCreditCardInfoModel.CardNumber = c.CardNumber;
                                            return View("Index", accountViewModel);
                                            //return RedirectToAction("Index", new { activeTab = 3 });
                                        }
                                        else
                                        {
                                            ViewBag.UpdateCreditCardInfoErrors = "Payment Profile has been created, but validation failed.";
                                        }
                                    }
                                    else
                                    {
                                        ViewBag.UpdateCreditCardInfoErrors = "Authorize.Net response is empty.";
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(autnetResult))
                                        ModelState.AddModelError("", autnetResult);
                                    ViewBag.CardErrorMessage = "The Entered Information of card Details is not a valid.";
                                }
                            }
                            catch (Exception ex)
                            {
                                ViewBag.UpdateCreditCardInfoErrors = "Error in updating credit card details " + ex.Message;
                            }
                        }
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
            }

            _customerCreditCardInfo.UpdateCreditCardInfo = true;
            accountViewModel.CustomerCreditCardInfoModel = _customerCreditCardInfo;

            return View("Index", accountViewModel);
            //return RedirectToAction("Index", new { activeTab = 3 });
        }


        [HttpPost]
        public ActionResult UpdateCustomerPassword(CustomerUpdatePassword customerUpdatePassword)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 8;
            bool passwordUpdated = false;

            MembershipUser user = Helpers.LoggedUser;

            if (user != null) //Customer does not exist create asp.net user.        
            {
                if (Membership.ValidateUser(user.UserName, customerUpdatePassword.CurrentPassword))
                {
                    try
                    {
                        if(customerUpdatePassword.CurrentPassword == customerUpdatePassword.NewPassword)
                        {
                            TempData["customerupdatePasswod"] = "Please Enter the different Password.";
                            return View("Index",accountViewModel);
                        }
                        else
                        {
                            passwordUpdated = user.ChangePassword(customerUpdatePassword.CurrentPassword, customerUpdatePassword.NewPassword);

                            HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                            ec.SendMail_PasswordChanged(user.Email);

                            ViewBag.UpdatePasswordMessage = "Password has been changed.";
                            return View("Index", accountViewModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (passwordUpdated)
                        {
                            ViewBag.UpdatePasswordMessage = "Password has been changed.";
                            return View("Index", accountViewModel);
                        }
                    }
                }
                else
                {
                    TempData["CurrentPasswordMessage"] = "Entered Current Password is wrong!!!";
                    return RedirectToAction("Index", accountViewModel);
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            accountViewModel.CustomerUpdatePasswordModel = customerUpdatePassword;
            return View("Index", accountViewModel);
            //return RedirectToAction("Index", new { activeTab = 9 });
        }

        [HttpPost]
        public ActionResult NewPassword(CustomerUpdatePassword customerUpdatePassword)
        {
            MembershipUser user = Helpers.LoggedUser;
            if (Membership.ValidateUser(user.UserName, customerUpdatePassword.CurrentPassword))
            {
                try
                {
                    user.ChangePassword(customerUpdatePassword.NewPassword, customerUpdatePassword.ConfirmPassword);
                    HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                    ec.SendMail_PasswordChanged(user.Email);
                }
                catch (Exception ex)
                {

                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult UpdateCustomerPrefInfo(CustomerPrefUpdate customerPrefUpdate)
        {
            MembershipUser user = Helpers.LoggedUser;
            if (user != null)
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(Convert.ToString(user.ProviderUserKey));
                if (_userToUpdate != null)
                {
                    List<hccUserProfilePreference> existingUserPrefs = hccUserProfilePreference.GetBy(_userToUpdate.UserProfileID, true);
                    existingUserPrefs.ForEach(delegate (hccUserProfilePreference userPref) { userPref.Delete(); });

                    if (customerPrefUpdate.PreferencesSelected != null)
                    {
                        foreach (int prefId in customerPrefUpdate.PreferencesSelected)
                        {
                            hccUserProfilePreference newPref = new hccUserProfilePreference
                            {
                                PreferenceID = prefId,
                                UserProfileID = _userToUpdate.UserProfileID,
                                IsActive = true
                            };
                            newPref.Save();
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.UpdatePrefInfoMessage = "Preferences Updated.";
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 4;
            return View("Index", accountViewModel);
            //return RedirectToAction("Index", new { activeTab = 4 });
        }

        [HttpPost]
        public ActionResult UpdateCustomerAllergensInfo(CustomerAllergensUpdate customerAllergensUpdate)
        {
            MembershipUser user = Helpers.LoggedUser;
            if (user != null)
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(Convert.ToString(user.ProviderUserKey));
                if (_userToUpdate != null)
                {
                    List<hccUserProfileAllergen> existingAllergen = hccUserProfileAllergen.GetBy(_userToUpdate.UserProfileID, true);
                    existingAllergen.ForEach(a => a.Delete());

                    if (customerAllergensUpdate.AllergensSelected != null)
                    {
                        foreach (int allerId in customerAllergensUpdate.AllergensSelected)
                        {
                            hccUserProfileAllergen all = new hccUserProfileAllergen
                            {
                                AllergenID = allerId,
                                UserProfileID = _userToUpdate.UserProfileID,
                                IsActive = true
                            };
                            all.Save();
                        }
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }

            ViewBag.UpdateAllergensInfoInfoMessage = "Allergens Updated.";
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 5;
            return View("Index", accountViewModel);
            //return RedirectToAction("Index", new { activeTab = 5 });
        }

        [HttpGet]
        public ActionResult DeleteRecurringOrder(int cartId, int cartItemId)
        {
            try
            {
                using (var hcE = new healthychefEntities())
                {
                    var rOrder = hcE.hccRecurringOrders.FirstOrDefault(i => i.CartID == cartId && i.CartItemID == cartItemId);
                    hcE.hccRecurringOrders.DeleteObject(rOrder);
                    var cartitem = hccCartItem.GetById(cartItemId);
                    if(cartitem!=null)
                    {
                        cartitem.Plan_IsAutoRenew = false;
                        cartitem.Save();
                    }

                    hcE.SaveChanges();
                }
            }
            catch (Exception E)
            {
                return RedirectToAction("Index", new { activeTab = 9 });
            }
            return RedirectToAction("Index", new { activeTab = 9 });
        }
        public ActionResult SelectSubProfile(int profileId)
        {
            AccountViewModel accountViewModel = new AccountViewModel();
            accountViewModel.ActiveTab = 6;

            MembershipUser user = Helpers.LoggedUser;
            var _profile = new hccUserProfile();

            try
            {
                if (user != null)
                {
                    var UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                    _profile = hccUserProfile.GetParentProfileBy(UserProfileId);
                }

                accountViewModel.CustomerSubProfilesinfoModel = new CustomerSubProfilesinfo(_profile, profileId);
            }
            catch (Exception E)
            {
                return View("Index", accountViewModel);
            }
            return View("Index", accountViewModel);
        }

        [HttpPost]
        public JsonResult SelectSubProfileInfo(int profileId)
        {
            //AccountViewModel accountViewModel = new AccountViewModel();
            //accountViewModel.ActiveTab = 6;
            var CustomerSubProfileBasicInfoModel = new CustomerSubProfileBasicInfo();
            var CustomerSubProfileShippinInfoModel = new CustomerSubProfileShippinInfo();
            var CustomerSubProfilePrefUpdateModel = new CustomerSubProfilePrefUpdate();
            var CustomerSubProfileAllergensUpdateModel = new CustomerSubProfileAllergensUpdate();
            MembershipUser user = Helpers.LoggedUser;
            var _profile = new hccUserProfile();

            try
            {
                //if (user != null)
                //{
                //    var UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                //    _profile = hccUserProfile.GetParentProfileBy(UserProfileId);
                //}

                //accountViewModel.CustomerSubProfilesinfoModel = new CustomerSubProfilesinfo(_profile, profileId);
                var CurrentProfileId = profileId;
                var _currentProfile = hccUserProfile.GetById(profileId);
                 CustomerSubProfileBasicInfoModel = new CustomerSubProfileBasicInfo(_currentProfile);
                 CustomerSubProfileShippinInfoModel = new CustomerSubProfileShippinInfo(_currentProfile);
                 CustomerSubProfilePrefUpdateModel = new CustomerSubProfilePrefUpdate(_currentProfile);
                 CustomerSubProfileAllergensUpdateModel = new CustomerSubProfileAllergensUpdate(_currentProfile);
            }
            catch (Exception E)
            {
                return Json(E.ToString(), JsonRequestBehavior.AllowGet);
            }
            return Json(new { SubProfileBasicinfo = RenderRazorViewToString("~/Views/Account/_SubProfileBasicInfo.cshtml",CustomerSubProfileBasicInfoModel), SubProfilShippinginfo = RenderRazorViewToString("~/Views/Account/_SubProfileShippingInfo.cshtml", CustomerSubProfileShippinInfoModel), SubProfilPreferenceinfo = RenderRazorViewToString("~/Views/Account/_SubProfilePreferencesInfo.cshtml", CustomerSubProfilePrefUpdateModel), SubProfilAllergensinfo = RenderRazorViewToString("~/Views/Account/_SubProfileAllergensInfo.cshtml", CustomerSubProfileAllergensUpdateModel) }, JsonRequestBehavior.AllowGet);
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        [HttpPost]
        public JsonResult DeActivateProfile(int profileId)
        {
            bool _profileDeactivated = false;
            string _message = "";

            try
            {
                hccUserProfile delSub = hccUserProfile.GetById(profileId);

                if (delSub != null)
                {
                    delSub.Activation(!delSub.IsActive);
                    _profileDeactivated = true;
                }
                else
                {
                    _message = "Sub profile not found with this sub-profile id";
                }
            }
            catch (Exception E)
            {
                _message = "Error in deactivating sub-profile " + E.Message;
            }

            return Json(new { Success = _profileDeactivated, Message = _message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSubProfileBasicInfo(CustomerSubProfileBasicInfo _customerSubProfileBasicInfo)
        {
            int CurrentProfileId = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    hccUserProfile CurrentUserProfile = hccUserProfile.GetById(_customerSubProfileBasicInfo.ProfileId);
                    hccUserProfile parentProfile = GetUserProfileByID(_customerSubProfileBasicInfo.UserID);

                    MembershipUser user = Helpers.LoggedUser;
                    if (user != null)
                    {
                        var UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                        parentProfile = hccUserProfile.GetParentProfileBy(UserProfileId);
                    }

                    if (parentProfile != null)
                    {
                        if (IsProfileNameExists(_customerSubProfileBasicInfo.ProfileName, parentProfile.MembershipID, _customerSubProfileBasicInfo.ProfileId))
                        {
                            ModelState.AddModelError("ProfileName", "The Sub-Profile Name entered already exists for this account.");
                            AccountViewModel accountViewModel = new AccountViewModel();
                            accountViewModel.ActiveTab = 6;
                            accountViewModel.CustomerSubProfilesinfoModel = new CustomerSubProfilesinfo(parentProfile, _customerSubProfileBasicInfo.ProfileId);
                            accountViewModel.CustomerSubProfilesinfoModel.CustomerSubProfileBasicInfoModel = _customerSubProfileBasicInfo;
                            return View("Index", accountViewModel);
                        }
                    }

                    if (CurrentUserProfile == null && parentProfile != null)
                    {
                        hccUserProfile newProfile = new hccUserProfile
                        {
                            ParentProfileID = parentProfile.UserProfileID,
                            CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                            CreatedDate = DateTime.Now,
                            IsActive = _customerSubProfileBasicInfo.IsActive,
                            FirstName = _customerSubProfileBasicInfo.FirstName,
                            LastName = _customerSubProfileBasicInfo.LastName,
                            ProfileName = _customerSubProfileBasicInfo.ProfileName,
                            MembershipID = parentProfile.MembershipID
                        };

                        newProfile.Save();
                        CurrentUserProfile = newProfile;
                        CurrentProfileId = CurrentUserProfile.UserProfileID;
                        TempData["BasicProfileData1"] = "Please save shipping information too";
                    }

                    if (CurrentUserProfile != null)
                    {
                        CurrentUserProfile.IsActive = _customerSubProfileBasicInfo.IsActive;

                        if (_customerSubProfileBasicInfo.ProfileName != CurrentUserProfile.ProfileName)
                            CurrentUserProfile.ProfileName = _customerSubProfileBasicInfo.ProfileName;

                        if (_customerSubProfileBasicInfo.FirstName != CurrentUserProfile.FirstName)
                            CurrentUserProfile.FirstName = _customerSubProfileBasicInfo.FirstName;

                        if (_customerSubProfileBasicInfo.LastName != CurrentUserProfile.LastName)
                            CurrentUserProfile.LastName = _customerSubProfileBasicInfo.LastName;


                        CurrentUserProfile.Save();
                        CurrentProfileId = CurrentUserProfile.UserProfileID;
                    }

                }
                catch (Exception ex)
                {
                    return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
                }
                TempData["BasicProfileData"] = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
            }
            TempData["BasicProfileData"] = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
            return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
        }

        [HttpPost]
        public ActionResult UpdateSubProfileShippingInfo(CustomerSubProfileShippinInfo _customerSubProfileShippinInfo)
        {
            int CurrentProfileId = 0;

            if (_customerSubProfileShippinInfo.SameAsMainAccount)
            {
                hccUserProfile CurrentUserProfile = hccUserProfile.GetById(_customerSubProfileShippinInfo.ProfileId);
                hccUserProfile parentProfile = GetUserProfileByID(_customerSubProfileShippinInfo.UserID);

                MembershipUser user = Helpers.LoggedUser;
                if (user != null)
                {
                    var UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                    parentProfile = hccUserProfile.GetParentProfileBy(UserProfileId);
                    if (CurrentUserProfile == null && parentProfile != null)
                    {
                        if (CurrentUserProfile != null && parentProfile != null)
                        {
                            CurrentUserProfile = new hccUserProfile
                            {
                                ParentProfileID = parentProfile.UserProfileID,
                                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                MembershipID = parentProfile.MembershipID
                            };

                            CurrentUserProfile.Save();
                        }
                        else
                        {
                            TempData["BasicProfileData2"] = "Please Save Basic Info first to save shipping details";
                            ModelState.AddModelError("", "Please Save Basic Info first to to save shipping details");
                            //TempData["BasicProfileData"].Color = Red;
                            return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
                        }
                    }
                }
                var CurrentAddress = hccAddress.GetById(parentProfile.ShippingAddressID ?? 0);
                if (CurrentAddress != null)
                {
                    var currentshippingaddress =new hccAddress();
                    currentshippingaddress.AddressTypeID = CurrentAddress.AddressTypeID;
                    currentshippingaddress.Address1 = CurrentAddress.Address1;
                    currentshippingaddress.Address2 = CurrentAddress.Address2;
                    currentshippingaddress.City = CurrentAddress.City;
                    currentshippingaddress.State = CurrentAddress.State;
                    currentshippingaddress.PostalCode = CurrentAddress.PostalCode;
                    currentshippingaddress.FirstName = CurrentAddress.FirstName;
                    currentshippingaddress.LastName = CurrentAddress.LastName;
                    currentshippingaddress.Country = CurrentAddress.Country;
                    currentshippingaddress.Phone = CurrentAddress.Phone;
                    currentshippingaddress.IsBusiness = CurrentAddress.IsBusiness;
                    currentshippingaddress.DefaultShippingTypeID = CurrentAddress.DefaultShippingTypeID;
                    currentshippingaddress.ProfileName = CurrentAddress.ProfileName;
                    if (_customerSubProfileShippinInfo.DefaultShippingTypeID == 2)
                    {
                        string ZipCode = _customerSubProfileShippinInfo.PostalCode;
                        hccShippingZone hccshopin = new hccShippingZone();
                        DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
                        int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());
                        DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                        string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                        if (IsPickup == "True")
                        {
                            currentshippingaddress.Save();
                            TempData["BasicProfileData"] = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                            return RedirectToAction("SelectSubProfile", new { profileId = 0 });
                        }
                        else
                        {
                            TempData["BasicProfileData"] = "Sub-Profile Saved but Customer pickup is not available at this Zip Code- " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                            ModelState.AddModelError("PostalCode", "Customer pickup is not available at this Zip Code");
                        }
                    }
                    else
                    {
                        currentshippingaddress.Save();
                        if (CurrentUserProfile != null)
                        {
                            CurrentUserProfile.ShippingAddressID = currentshippingaddress.AddressID;
                            CurrentUserProfile.Save();
                        }
                        TempData["BasicProfileData"] = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                        return RedirectToAction("SelectSubProfile", new { profileId = 0 });
                    }
                    currentshippingaddress.Save();
                    CurrentUserProfile.ShippingAddressID = currentshippingaddress.AddressID;
                }
                CurrentUserProfile.Save();

            }
            else if (ModelState.IsValid)
            {
                try
                {
                    hccUserProfile CurrentUserProfile = hccUserProfile.GetById(_customerSubProfileShippinInfo.ProfileId);
                    hccUserProfile parentProfile = GetUserProfileByID(_customerSubProfileShippinInfo.UserID);

                    MembershipUser user = Helpers.LoggedUser;
                    if (user != null)
                    {
                        var UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                        parentProfile = hccUserProfile.GetParentProfileBy(UserProfileId);
                        if (CurrentUserProfile == null && parentProfile != null)
                        {
                            CurrentUserProfile = new hccUserProfile
                            {
                                ParentProfileID = parentProfile.UserProfileID,
                                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                IsActive = true,
                                MembershipID = parentProfile.MembershipID
                            };

                            CurrentUserProfile.Save();
                        }
                    }

                    if (CurrentUserProfile != null && parentProfile != null)
                    {
                        CurrentProfileId = CurrentUserProfile.UserProfileID;


                        if (_customerSubProfileShippinInfo.SameAsMainAccount)
                        {
                            CurrentUserProfile.ShippingAddressID = parentProfile.ShippingAddressID;
                            CurrentUserProfile.Save();
                        }
                        else
                        {
                            var CurrentAddress = hccAddress.GetById(CurrentUserProfile.ShippingAddressID ?? 0);
                            hccAddress address;

                            if (CurrentAddress == null)
                            {
                                address = new hccAddress { Country = "US", AddressTypeID = 4 };
                            }
                            else
                            {
                                address = CurrentAddress;
                            }
                            int addrId = address.AddressID;
                            address.FirstName = _customerSubProfileShippinInfo.FirstName;
                            address.LastName = _customerSubProfileShippinInfo.LastName;
                            address.Address1 = _customerSubProfileShippinInfo.Address1;
                            address.Address2 = _customerSubProfileShippinInfo.Address2;
                            address.City = _customerSubProfileShippinInfo.City;
                            address.State = _customerSubProfileShippinInfo.State;
                            address.PostalCode = _customerSubProfileShippinInfo.PostalCode;
                            address.Phone = _customerSubProfileShippinInfo.Phone;
                            address.IsBusiness = _customerSubProfileShippinInfo.IsBusiness;
                            address.DefaultShippingTypeID = _customerSubProfileShippinInfo.DefaultShippingTypeID;

                            if (_customerSubProfileShippinInfo.DefaultShippingTypeID == 2)
                            {
                                string ZipCode = _customerSubProfileShippinInfo.PostalCode;
                                hccShippingZone hccshopin = new hccShippingZone();
                                DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
                                int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());
                                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                                string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                                if (IsPickup == "True")
                                {
                                    address.Save();
                                    TempData["BasicProfileData"] = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                                    return RedirectToAction("SelectSubProfile", new { profileId = 0 });
                                }
                                else
                                {
                                    TempData["BasicProfileData"] = "Sub-Profile Saved but Customer pickup is not available at this Zip Code- " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                                    ModelState.AddModelError("PostalCode", "Customer pickup is not available at this Zip Code");
                                }
                            }
                            else
                            {
                                address.Save();
                                if(CurrentUserProfile!=null)
                                {
                                    CurrentUserProfile.ShippingAddressID = address.AddressID;
                                    CurrentUserProfile.Save();
                                }
                                TempData["BasicProfileData"] = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
                                return RedirectToAction("SelectSubProfile", new { profileId = 0 });
                            }
                        }

                    }
                    else
                    {
                        TempData["BasicProfileData"] = "Please Save Basic Info first to to save shipping details";
                        ModelState.AddModelError("", "Please Save Basic Info first to to save shipping details");
                        return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
                    }
                }
                catch (Exception ex)
                {

                }
            }
            //TempData["BasicProfileData"] = "Sub-Profile Saved - " + DateTime.Now.ToString("MM/dd/yyy hh:mm:ss");
            return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
        }


        [HttpPost]
        public ActionResult UpdateSubProfilePrefInfo(CustomerSubProfilePrefUpdate _customerSubProfilePrefUpdate)
        {
            int CurrentProfileId = 0;
            MembershipUser user = Helpers.LoggedUser;
            if (user != null)
            {
                hccUserProfile CurrentUserProfile = hccUserProfile.GetById(_customerSubProfilePrefUpdate.ProfileId);
                hccUserProfile parentProfile = GetUserProfileByID(_customerSubProfilePrefUpdate.UserID);

                var UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                parentProfile = hccUserProfile.GetParentProfileBy(UserProfileId);
                if (CurrentUserProfile == null && parentProfile != null)
                {
                    CurrentUserProfile = new hccUserProfile
                    {
                        ParentProfileID = parentProfile.UserProfileID,
                        CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        MembershipID = parentProfile.MembershipID
                    };

                    CurrentUserProfile.Save();
                }

                if (CurrentUserProfile != null && parentProfile != null)
                {
                    CurrentProfileId = CurrentUserProfile.UserProfileID;
                    List<hccUserProfilePreference> existingUserPrefs = hccUserProfilePreference.GetBy(_customerSubProfilePrefUpdate.ProfileId, true);
                    existingUserPrefs.ForEach(delegate (hccUserProfilePreference userPref) { userPref.Delete(); });

                    if (_customerSubProfilePrefUpdate.PreferencesSelected != null)
                    {
                        foreach (int prefId in _customerSubProfilePrefUpdate.PreferencesSelected)
                        {
                            hccUserProfilePreference newPref = new hccUserProfilePreference
                            {
                                PreferenceID = prefId,
                                UserProfileID = CurrentUserProfile.UserProfileID,
                                IsActive = true
                            };
                            newPref.Save();
                        }
                    }
                }
                else
                {
                    return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
                }
            }
            else
            {
                return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
            }

            return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
        }

        [HttpPost]
        public ActionResult UpdateSubProfileAllergensInfo(CustomerSubProfileAllergensUpdate _customerSubProfileAllergensUpdate)
        {
            int CurrentProfileId = 0;
            MembershipUser user = Helpers.LoggedUser;
            if (user != null)
            {
                hccUserProfile CurrentUserProfile = hccUserProfile.GetById(_customerSubProfileAllergensUpdate.ProfileId);
                hccUserProfile parentProfile = GetUserProfileByID(_customerSubProfileAllergensUpdate.UserID);

                var UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                parentProfile = hccUserProfile.GetParentProfileBy(UserProfileId);
                if (CurrentUserProfile == null && parentProfile != null)
                {
                    CurrentUserProfile = new hccUserProfile
                    {
                        ParentProfileID = parentProfile.UserProfileID,
                        CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        MembershipID = parentProfile.MembershipID
                    };

                    CurrentUserProfile.Save();
                }

                if (CurrentUserProfile != null && parentProfile != null)
                {
                    CurrentProfileId = CurrentUserProfile.UserProfileID;
                    List<hccUserProfileAllergen> existingAllergen = hccUserProfileAllergen.GetBy(_customerSubProfileAllergensUpdate.ProfileId, true);
                    existingAllergen.ForEach(a => a.Delete());

                    if (_customerSubProfileAllergensUpdate.AllergensSelected != null)
                    {
                        foreach (int allerId in _customerSubProfileAllergensUpdate.AllergensSelected)
                        {
                            hccUserProfileAllergen all = new hccUserProfileAllergen
                            {
                                AllergenID = allerId,
                                UserProfileID = CurrentUserProfile.UserProfileID,
                                IsActive = true
                            };
                            all.Save();
                        }
                    }
                }
                else
                {
                    return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
                }
            }
            else
            {
                return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
            }

            return RedirectToAction("SelectSubProfile", new { profileId = CurrentProfileId });
        }

        private bool IsProfileNameExists(string _profileName,Guid? CurrentParentAspNetId,int CurrentProfileId)
        {
            if (CurrentParentAspNetId.HasValue)
            {
                var profs = hccUserProfile.GetBy(CurrentParentAspNetId.Value, true);

                if (profs != null)
                {
                    foreach (var p in profs)
                    {
                        if(p.ProfileName == _profileName && p.UserProfileID != CurrentProfileId)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
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