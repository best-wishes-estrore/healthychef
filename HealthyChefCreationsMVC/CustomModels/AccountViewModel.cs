using HealthyChef.Common;
using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class AccountViewModel
    {
        public int ActiveTab { get; set; }
        public Guid UserProfileId { get; set; }
        public string UserEmail { get; set; }
        public hccUserProfile profile { get; set; }


        public CustomerBasicInfo CustomerBasicInfoModel { get; set; }
        public CustomerShippinInfo CustomerShippinInfoModel { get; set; }
        public CustomerBillingInfo CustomerBillingInfoModel { get; set; }
        public CustomerCreditCardInfo CustomerCreditCardInfoModel { get; set; }
        public CustomerUpdatePassword CustomerUpdatePasswordModel { get; set; }
        public CustomerPrefUpdate CustomerPrefUpdateModel { get; set; }
        public CustomerAllergensUpdate CustomerAllergensUpdateModel { get; set; }
        public CustomerOrderInfo CustomerOrderInfoModel { get; set; }
        public CustomerRecurringOrderInfo CustomerRecurringOrderInfoModel { get; set; }
        public CustomerSubProfilesinfo CustomerSubProfilesinfoModel { get; set; }


        public AccountViewModel()
        {
            this.ActiveTab = 1;
            this.UserProfileId = Guid.Empty;
            this.profile = new hccUserProfile();
            this.UserEmail = string.Empty;

            MembershipUser user = Helpers.LoggedUser;
            if (user != null)
            {
                this.UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                this.profile = hccUserProfile.GetParentProfileBy(this.UserProfileId);
                this.UserEmail = user.Email;
            }
            else
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["Email"] as string))
                {
                    user = Membership.GetUser(Membership.GetUserNameByEmail(HttpContext.Current.Session["Email"].ToString()));
                    this.UserProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                    this.profile = hccUserProfile.GetParentProfileBy(this.UserProfileId);
                    this.UserEmail = user.Email;
                }
            }

            //binding sub models

            //CustomerBasicInfoModel
            this.CustomerBasicInfoModel = new CustomerBasicInfo(this.profile, this.UserEmail);
            this.CustomerShippinInfoModel = new CustomerShippinInfo(this.profile);

            this.CustomerBillingInfoModel = new CustomerBillingInfo(this.profile);
            this.CustomerCreditCardInfoModel = new CustomerCreditCardInfo(this.profile);
            this.CustomerUpdatePasswordModel = new CustomerUpdatePassword(this.profile);
            this.CustomerPrefUpdateModel = new CustomerPrefUpdate(this.profile);
            this.CustomerAllergensUpdateModel = new CustomerAllergensUpdate(this.profile);
            this.CustomerOrderInfoModel = new CustomerOrderInfo(this.profile);
            this.CustomerRecurringOrderInfoModel = new CustomerRecurringOrderInfo(this.profile);
            this.CustomerSubProfilesinfoModel = new CustomerSubProfilesinfo(this.profile);
        }
    }

    public class CustomerBasicInfo
    {
        public string UserID { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Profile Name")]
        [Required(ErrorMessage = "Profile Name is required.")]
        public string ProfileName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        public decimal AccountBalance { get; set; }

        public CustomerBasicInfo()
        {

        }
        public CustomerBasicInfo(hccUserProfile _profile, string _email)
        {
            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();
                this.Email = _email;
                this.ProfileName = _profile.ProfileName;
                this.FirstName = _profile.FirstName;
                this.LastName = _profile.LastName;
                this.AccountBalance = _profile.AccountBalance;
            }
            else
            {
                this.UserID = string.Empty;
                this.Email = string.Empty;
                this.ProfileName = string.Empty;
                this.FirstName = string.Empty;
                this.LastName = string.Empty;
                this.AccountBalance = decimal.Zero;
            }
        }
    }

    public class CustomerShippinInfo
    {
        public string UserID { get; set; }
        public int ShippingAddressID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string ShippingFirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Display(Name = "Address 1")]
        [Required(ErrorMessage = "Address1 is required.")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Display(Name = "Phone")]
        [MinLength(10, ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        [MaxLength(20, ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        //[Required(ErrorMessage = "You must provide a phone number")]
        public string Phone { get; set; }

        [Display(Name = "Zip Code")]
        [MaxLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [MinLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [Required(ErrorMessage = "Zip Code is required.")]
        public string PostalCode { get; set; }

        [Display(Name = "Delivery Type")]
        public int DefaultShippingTypeID { get; set; }

        [Display(Name = "Is a Business Address?")]
        public bool IsBusiness { get; set; }

        public string NotesTitle { set; get; }
        public string DisplayNote { set; get; }

        public CustomerShippinInfo()
        {

        }

        public CustomerShippinInfo(hccUserProfile _profile)
        {
            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();

                var CurrentAddress = hccAddress.GetById(_profile.ShippingAddressID ?? 0);
                if (CurrentAddress == null)
                {
                    CurrentAddress = new hccAddress { Country = "US", AddressTypeID = 4 };
                }
                else
                {
                    this.ShippingAddressID = CurrentAddress.AddressID;
                    this.ShippingFirstName = CurrentAddress.FirstName;
                    this.LastName = CurrentAddress.LastName;
                    this.Address1 = CurrentAddress.Address1;
                    this.Address2 = CurrentAddress.Address2;
                    this.City = CurrentAddress.City;
                    this.State = CurrentAddress.State;
                    this.PostalCode = CurrentAddress.PostalCode;
                    this.Phone = CurrentAddress.Phone;
                    this.IsBusiness = CurrentAddress.IsBusiness;
                    this.DefaultShippingTypeID = CurrentAddress.DefaultShippingTypeID;
                }


                //Note: to dispaly Notes created at Admin Panel
                int CurrentUserProfileId = _profile.UserProfileID;

                HealthyChef.Common.Enums.UserProfileNoteTypes CurrentNoteType = HealthyChef.Common.Enums.UserProfileNoteTypes.ShippingNote;

                List<hccUserProfileNote> notes = new List<hccUserProfileNote>();
                notes = hccUserProfileNote.GetBy(Convert.ToInt32(CurrentUserProfileId), CurrentNoteType, true);

                if (notes.Count > 0)
                {
                    //pnlNotesDisplay.Visible = true;
                    NotesTitle = HealthyChef.Common.Enums.GetEnumDescription(CurrentNoteType) + "(s)";
                    DisplayNote = notes.Select(a => a.Note).DefaultIfEmpty(string.Empty).Aggregate((a, b) => a + "; " + b);
                }


            }
        }
    }

    public class CustomerBillingInfo
    {
        public string UserID { get; set; }
        public int BillingAddressID { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string BillingFirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string BillingLastName { get; set; }

        [Display(Name = "Address 1")]
        [Required(ErrorMessage = "Address1 is required.")]
        public string BillingAddress1 { get; set; }

        [Display(Name = "Address 2")]
        public string BillingAddress2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        public string BillingCity { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required.")]
        public string BillingState { get; set; }

        [Display(Name = "Phone")]
        [MinLength(10, ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        [MaxLength(20, ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        //[Required(ErrorMessage = "You must provide a phone number")]
        public string BillingPhone { get; set; }

        [Display(Name = "Zip Code")]
        [MaxLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [MinLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [Required(ErrorMessage = "Zip Code is required.")]
        public string BillingPostalCode { get; set; }

        public CustomerBillingInfo()
        {

        }

        public CustomerBillingInfo(hccUserProfile _profile)
        {
            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();

                var CurrentAddress = hccAddress.GetById(_profile.BillingAddressID ?? 0);
                if (CurrentAddress == null)
                {
                    CurrentAddress = new hccAddress { Country = "US", AddressTypeID = 2 };
                }
                else
                {
                    this.BillingAddressID = CurrentAddress.AddressID;
                    this.BillingFirstName = CurrentAddress.FirstName;
                    this.BillingLastName = CurrentAddress.LastName;
                    this.BillingAddress1 = CurrentAddress.Address1;
                    this.BillingAddress2 = CurrentAddress.Address2;
                    this.BillingCity = CurrentAddress.City;
                    this.BillingState = CurrentAddress.State;
                    this.BillingPostalCode = CurrentAddress.PostalCode;
                    this.BillingPhone = CurrentAddress.Phone;
                }
            }
        }
    }

    public class CustomerCreditCardInfo
    {
        public string UserID { get; set; }

        [Display(Name = "Update Card Information")]
        public bool UpdateCreditCardInfo { get; set; }

        [Display(Name = "Name On Card")]
        [Required(ErrorMessage = "Name On Card is required.")]
        public string NameOnCard { get; set; }

        [Display(Name = "Card Number")]
        [Required(ErrorMessage = "Card Number is required.")]
        [DataType(DataType.CreditCard, ErrorMessage = "Enter a valid card number.")]
        public string CardNumber { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is required.")]
        public int ExipiresOnMonth { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year is required.")]
        public int ExipiresOnYear { get; set; }

        [Display(Name = "CVV Code")]
        [MaxLength(4, ErrorMessage = "CardIdCode should be 4 digits in length.")]
        [MinLength(3, ErrorMessage = "CardIdCode should be 3 digits in length.")]
        [Required(ErrorMessage = "Card Id code is required.")]
        public string CardIdCode { get; set; }

        public string NotesTitle { set; get; }
        public string DisplayNote { set; get; }

        public CustomerCreditCardInfo()
        {

        }

        public CustomerCreditCardInfo(hccUserProfile _profile)
        {

            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();
                hccUserProfilePaymentProfile paymentProfile = hccUserProfilePaymentProfile.GetBy(_profile.UserProfileID);
                if (paymentProfile != null)
                {
                    var CurrentCardInfo = paymentProfile.ToCardInfo();

                    this.NameOnCard = CurrentCardInfo.NameOnCard;
                    this.CardNumber = "************" + CurrentCardInfo.CardNumber;
                    this.ExipiresOnMonth = CurrentCardInfo.ExpMonth;
                    this.ExipiresOnYear = CurrentCardInfo.ExpYear;
                }

                //Note: to dispaly Notes created at Admin Panel
                int CurrentUserProfileId = _profile.UserProfileID;

                HealthyChef.Common.Enums.UserProfileNoteTypes CurrentNoteType = HealthyChef.Common.Enums.UserProfileNoteTypes.BillingNote;

                List<hccUserProfileNote> notes = new List<hccUserProfileNote>();
                notes = hccUserProfileNote.GetBy(Convert.ToInt32(CurrentUserProfileId), CurrentNoteType, true);

                if (notes.Count > 0)
                {
                    //pnlNotesDisplay.Visible = true;
                    NotesTitle = HealthyChef.Common.Enums.GetEnumDescription(CurrentNoteType) + "(s)";
                    DisplayNote = notes.Select(a => a.Note).DefaultIfEmpty(string.Empty).Aggregate((a, b) => a + "; " + b);
                }
                this.UpdateCreditCardInfo = true;
            }
        }
    }

    public class CustomerUpdatePassword
    {
        public string UserID { get; set; }

        [Display(Name = "Current Password")]
        [Required(ErrorMessage = "Enter Current Password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [Display(Name = "New Password")]
        [Required(ErrorMessage = "Enter New Password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "Confirm New Password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public CustomerUpdatePassword()
        {

        }

        public CustomerUpdatePassword(hccUserProfile _profile)
        {
            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();
            }
        }
    }

    public class CustomerPrefUpdate
    {
        public string UserID { get; set; }

        public List<CustomerPreferenceItem> AllPreferences { get; set; }

        public int[] PreferencesSelected { get; set; }
        public bool DisplaytoUser { set; get; }
        public string NotesTitle { set; get; }
        public string DisplayNote { set; get; }

        public CustomerPrefUpdate()
        {

        }

        public CustomerPrefUpdate(hccUserProfile _profile)
        {
            this.AllPreferences = new List<CustomerPreferenceItem>();

            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();

                var _allPrefs = hccPreference.GetBy(Enums.PreferenceType.Customer, false);
                List<hccUserProfilePreference> userPrefs = hccUserProfilePreference.GetBy(_profile.UserProfileID, true);

                foreach (var p in _allPrefs)
                {
                    var _prefItem = new CustomerPreferenceItem();

                    _prefItem.PreferenceID = p.PreferenceID;
                    _prefItem.Name = p.Name;

                    if (userPrefs.Where(a => a.PreferenceID == p.PreferenceID).Count() > 0)
                    {
                        _prefItem.IsSelected = true;
                    }

                    this.AllPreferences.Add(_prefItem);
                }

                //Note: to dispaly Notes created at Admin Panel
                int CurrentUserProfileId = _profile.UserProfileID;

                HealthyChef.Common.Enums.UserProfileNoteTypes CurrentNoteType = HealthyChef.Common.Enums.UserProfileNoteTypes.PreferenceNote;

                List<hccUserProfileNote> notes = new List<hccUserProfileNote>();
                notes = hccUserProfileNote.GetBy(Convert.ToInt32(CurrentUserProfileId), CurrentNoteType, true);

                if (notes.Count > 0)
                {
                    //pnlNotesDisplay.Visible = true;
                    if(notes.ToList().Where(x=>x.DisplayToUser==true).ToList().Count > 0)
                    {
                        DisplaytoUser = true;
                    }
                    NotesTitle = HealthyChef.Common.Enums.GetEnumDescription(CurrentNoteType) + "(s)";
                    DisplayNote = notes.Select(a => a.Note).DefaultIfEmpty(string.Empty).Aggregate((a, b) => a + "; " + b);
                }

            }
        }
    }

    public class CustomerPreferenceItem
    {
        public int PreferenceID { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }

        public string NotesTitle { set; get; }
        public string DisplayNote { set; get; }


    }

    public class CustomerAllergensUpdate
    {
        public string UserID { get; set; }

        public List<CustomerAllergenItem> AllAllergens { get; set; }

        public int[] AllergensSelected { get; set; }

        public CustomerAllergensUpdate()
        {

        }

        public CustomerAllergensUpdate(hccUserProfile _profile)
        {
            this.AllAllergens = new List<CustomerAllergenItem>();

            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();

                var _allAllergens = hccAllergen.GetActive();
                List<hccUserProfileAllergen> userAllgs = hccUserProfileAllergen.GetBy(_profile.UserProfileID, true);

                foreach (var _a in _allAllergens)
                {
                    var _allerItem = new CustomerAllergenItem();

                    _allerItem.AllergenID = _a.AllergenID;
                    _allerItem.Name = _a.Name;

                    if (userAllgs.Where(a => a.AllergenID == _a.AllergenID).Count() > 0)
                    {
                        _allerItem.IsSelected = true;
                    }

                    this.AllAllergens.Add(_allerItem);
                }
            }
        }
    }

    public class CustomerAllergenItem
    {
        public int AllergenID { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

    public class CustomerOrderInfo
    {
        public List<hccCart> carts { get; set; }

        public List<OrdersCartItem> CartItems { get; set; }

        public CustomerOrderInfo()
        {

        }

        public CustomerOrderInfo(hccUserProfile _profile)
        {
            this.carts = new List<hccCart>();
            this.CartItems = new List<OrdersCartItem>();

            MembershipUser user = Helpers.LoggedUser;
            if (user != null)
            {
                var _userProfileId = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators"))
                    carts = hccCart.GetBy(_userProfileId);
                else
                    carts = hccCart.GetCompleted(_userProfileId);
            }

            foreach (var c in this.carts)
            {
                var _cartItem = new OrdersCartItem();

                _cartItem.PurchaseNumber = c.PurchaseNumber;
                _cartItem.CartId = c.CartID;
                _cartItem.PurchaseDate = c.PurchaseDate;
                _cartItem.TotalAmount = c.TotalAmount;
               // _cartItem.CartHTML = c.ToHtml();
                _cartItem.StatusText = Enums.GetEnumDescription(c.Status);

                this.CartItems.Add(_cartItem);
            }
        }
    }

    public class OrdersCartItem
    {
        public int PurchaseNumber { get; set; }
        public string StatusText { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string CartHTML { get; set; }
        public int CartId { get; set; }
    }

    public class CustomerRecurringOrderInfo
    {
        public List<hccRecurringOrder> RecurringOrders { get; set; }

        public List<CustomerRecurringOrderItem> CustomerRecurringOrders { get; set; }

        public CustomerRecurringOrderInfo()
        {

        }

        public CustomerRecurringOrderInfo(hccUserProfile _profile)
        {
            this.RecurringOrders = new List<hccRecurringOrder>();
            this.CustomerRecurringOrders = new List<CustomerRecurringOrderItem>();

            if (_profile != null)
            {
                using (var hcE = new healthychefEntities())
                {
                    if (Helpers.LoggedUser != null)
                    {

                        if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators") || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "EmployeeManager") || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "EmployeeService"))
                        {
                            this.RecurringOrders = hcE.hccRecurringOrders.Where(u => u.AspNetUserID == _profile.MembershipID).ToList();
                        }
                        else // TODO: this may not be required, if the viewstate always holds a valid AspNetId, then use code above.
                        {
                            MembershipUser user = Helpers.LoggedUser;
                            this.RecurringOrders = hcE.hccRecurringOrders.Where(u => u.AspNetUserID == (Guid)user.ProviderUserKey).ToList();
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(HttpContext.Current.Session["Email"] as string))
                        {
                            var user = Membership.GetUser(Membership.GetUserNameByEmail(HttpContext.Current.Session["Email"].ToString()));
                            if (Roles.IsUserInRole(user.UserName, "Administrators") || Roles.IsUserInRole(user.UserName, "EmployeeManager") || Roles.IsUserInRole(user.UserName, "EmployeeService"))
                            {
                                this.RecurringOrders = hcE.hccRecurringOrders.Where(u => u.AspNetUserID == _profile.MembershipID).ToList();
                            }
                            else // TODO: this may not be required, if the viewstate always holds a valid AspNetId, then use code above.
                            {
                                this.RecurringOrders = hcE.hccRecurringOrders.Where(u => u.AspNetUserID == (Guid)user.ProviderUserKey).ToList();
                            }
                        }
                    }
                }
            }

            foreach (var _r in this.RecurringOrders)
            {
                var _cro = new CustomerRecurringOrderItem();
                _cro.CartId = _r.CartID;
                _cro.CartItemId = _r.CartItemID;

                var cartItem = hccCartItem.GetById(_cro.CartItemId);

                if (cartItem != null)
                {
                    _cro.ItemDetails = cartItem.ItemName + "<br />(Quantity: " + cartItem.Quantity + ")";
                }
                else
                {
                    _cro.ItemDetails = "Item not available";
                }

                using (var hcE = new healthychefEntities())
                {
                    try
                    {
                        var recurringItem = hcE.hcc_RecurringOrderStartDate(_r.CartID, _r.CartItemID).SingleOrDefault();

                        _cro.RecurringDate = ((DateTime)recurringItem.MaxDeliveryDate).ToShortDateString();
                        _cro.AllowCancel = (DateTime)recurringItem.MaxDeliveryDate >= DateTime.Now.AddDays(15);
                    }
                    catch (Exception)
                    {
                        _cro.RecurringDate = "Date not available";
                    }
                }

                this.CustomerRecurringOrders.Add(_cro);
            }
        }
    }

    public class CustomerRecurringOrderItem
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public string ItemDetails { get; set; }
        public string RecurringDate { get; set; }
        public bool AllowCancel { get; set; }

    }

    public class CustomerSubProfilesinfo
    {
        public string UserId { get; set; }
        public int CurrentProfileId { get; set; }
        public List<hccUserProfile> subProfiles { get; set; }
        public CustomerSubProfileBasicInfo CustomerSubProfileBasicInfoModel { get; set; }
        public CustomerSubProfileShippinInfo CustomerSubProfileShippinInfoModel { get; set; }
        public CustomerSubProfilePrefUpdate CustomerSubProfilePrefUpdateModel { get; set; }
        public CustomerSubProfileAllergensUpdate CustomerSubProfileAllergensUpdateModel { get; set; }

        public CustomerSubProfilesinfo()
        {

        }

        public CustomerSubProfilesinfo(hccUserProfile _profile)
        {
            this.subProfiles = new List<hccUserProfile>();

            var CurrentUserProfile = new hccUserProfile();
            MembershipUser user = Helpers.LoggedUser;
            if (_profile != null)
            {
                CurrentUserProfile = hccUserProfile.GetParentProfileBy(_profile.MembershipID);
                this.UserId = _profile.MembershipID.ToString();
            }

            if (CurrentUserProfile != null)
                subProfiles = CurrentUserProfile.GetSubProfiles();

            var _currentProfile = hccUserProfile.GetById(this.CurrentProfileId);
            this.CustomerSubProfileBasicInfoModel = new CustomerSubProfileBasicInfo(_currentProfile);
            this.CustomerSubProfileShippinInfoModel = new CustomerSubProfileShippinInfo(_currentProfile);
            this.CustomerSubProfilePrefUpdateModel = new CustomerSubProfilePrefUpdate(_currentProfile);
            this.CustomerSubProfileAllergensUpdateModel = new CustomerSubProfileAllergensUpdate(_currentProfile);


        }

        public CustomerSubProfilesinfo(hccUserProfile _profile, int currentProfileId)
        {
            this.subProfiles = new List<hccUserProfile>();

            var CurrentUserProfile = new hccUserProfile();
            MembershipUser user = Helpers.LoggedUser;
            if (_profile != null)
                CurrentUserProfile = hccUserProfile.GetParentProfileBy(_profile.MembershipID);

            if (CurrentUserProfile != null)
                subProfiles = CurrentUserProfile.GetSubProfiles();

            this.CurrentProfileId = currentProfileId;
            var _currentProfile = hccUserProfile.GetById(this.CurrentProfileId);
            this.CustomerSubProfileBasicInfoModel = new CustomerSubProfileBasicInfo(_currentProfile);
            this.CustomerSubProfileShippinInfoModel = new CustomerSubProfileShippinInfo(_currentProfile);
            this.CustomerSubProfilePrefUpdateModel = new CustomerSubProfilePrefUpdate(_currentProfile);
            this.CustomerSubProfileAllergensUpdateModel = new CustomerSubProfileAllergensUpdate(_currentProfile);
        }
    }

    public class CustomerSubProfileBasicInfo
    {
        public string UserID { get; set; }
        public int ProfileId { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }

        [Display(Name = "Profile Name")]
        [Required(ErrorMessage = "Profile Name is required.")]
        public string ProfileName { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        public CustomerSubProfileBasicInfo()
        {

        }
        public CustomerSubProfileBasicInfo(hccUserProfile _profile)
        {
            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();
                this.ProfileId = _profile.UserProfileID;
                this.ProfileName = _profile.ProfileName;
                this.FirstName = _profile.FirstName;
                this.LastName = _profile.LastName;
                this.IsActive = _profile.IsActive;
            }
            else
            {
                this.UserID = string.Empty;
                this.ProfileName = string.Empty;
                this.FirstName = string.Empty;
                this.LastName = string.Empty;
                this.IsActive = true;
            }
        }
    }

    public class CustomerSubProfileShippinInfo
    {
        public string UserID { get; set; }
        public int ProfileId { get; set; }
        public int ShippingAddressID { get; set; }

        [Display(Name = "Same as Main Account Shipping Address")]
        public bool SameAsMainAccount { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        public string LastName { get; set; }

        [Display(Name = "Address 1")]
        [Required(ErrorMessage = "Address1 is required.")]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        public string City { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required.")]
        public string State { get; set; }

        [Display(Name = "Phone")]
        public string Phone { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "Zip Code is required.")]
        public string PostalCode { get; set; }

        [Display(Name = "Delivery Type")]
        public int DefaultShippingTypeID { get; set; }

        [Display(Name = "Is a Business Address?")]
        public bool IsBusiness { get; set; }

        public string NotesTitle { set; get; }
        public string DisplayNote { set; get; }

        public CustomerSubProfileShippinInfo()
        {

        }

        public CustomerSubProfileShippinInfo(hccUserProfile _profile)
        {
            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();
                this.ProfileId = _profile.UserProfileID;

                var CurrentAddress = hccAddress.GetById(_profile.ShippingAddressID ?? 0);
                if (CurrentAddress == null)
                {
                    CurrentAddress = new hccAddress { Country = "US", AddressTypeID = 4 };
                }
                else
                {
                    this.ShippingAddressID = CurrentAddress.AddressID;
                    this.FirstName = CurrentAddress.FirstName;
                    this.LastName = CurrentAddress.LastName;
                    this.Address1 = CurrentAddress.Address1;
                    this.Address2 = CurrentAddress.Address2;
                    this.City = CurrentAddress.City;
                    this.State = CurrentAddress.State;
                    this.PostalCode = CurrentAddress.PostalCode;
                    this.Phone = CurrentAddress.Phone;
                    this.IsBusiness = CurrentAddress.IsBusiness;
                    this.DefaultShippingTypeID = CurrentAddress.DefaultShippingTypeID;
                }

                //Note: to dispaly Notes created at Admin Panel
                int CurrentUserProfileId = _profile.UserProfileID;
                HealthyChef.Common.Enums.UserProfileNoteTypes CurrentNoteType = HealthyChef.Common.Enums.UserProfileNoteTypes.ShippingNote;

                List<hccUserProfileNote> notes = new List<hccUserProfileNote>();
                notes = hccUserProfileNote.GetBy(Convert.ToInt32(CurrentUserProfileId), CurrentNoteType, true);

                if (notes.Count > 0)
                {
                    //pnlNotesDisplay.Visible = true;
                    NotesTitle = HealthyChef.Common.Enums.GetEnumDescription(CurrentNoteType) + "(s)";
                    DisplayNote = notes.Select(a => a.Note).DefaultIfEmpty(string.Empty).Aggregate((a, b) => a + "; " + b);
                }

            }
        }
    }

    public class CustomerSubProfilePrefUpdate
    {

        public string UserID { get; set; }
        public int ProfileId { get; set; }
        public bool DisplaytoUser { set; get; }
        public string NotesTitle { set; get; }
        public string DisplayNote { set; get; }

        public List<CustomerPreferenceItem> AllPreferences { get; set; }

        public int[] PreferencesSelected { get; set; }

        public CustomerSubProfilePrefUpdate()
        {

        }

        public CustomerSubProfilePrefUpdate(hccUserProfile _profile)
        {
            this.AllPreferences = new List<CustomerPreferenceItem>();

            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();
                this.ProfileId = _profile.UserProfileID;

                var _allPrefs = hccPreference.GetBy(Enums.PreferenceType.Customer, false);
                List<hccUserProfilePreference> userPrefs = hccUserProfilePreference.GetBy(_profile.UserProfileID, true);

                foreach (var p in _allPrefs)
                {
                    var _prefItem = new CustomerPreferenceItem();

                    _prefItem.PreferenceID = p.PreferenceID;
                    _prefItem.Name = p.Name;

                    if (userPrefs.Where(a => a.PreferenceID == p.PreferenceID).Count() > 0)
                    {
                        _prefItem.IsSelected = true;
                    }

                    this.AllPreferences.Add(_prefItem);
                }


                //Note: to dispaly Notes created at Admin Panel
                int CurrentUserProfileId = _profile.UserProfileID;

                HealthyChef.Common.Enums.UserProfileNoteTypes CurrentNoteType = HealthyChef.Common.Enums.UserProfileNoteTypes.PreferenceNote;

                List<hccUserProfileNote> notes = new List<hccUserProfileNote>();
                notes = hccUserProfileNote.GetBy(Convert.ToInt32(CurrentUserProfileId), CurrentNoteType, true);

                if (notes.Count > 0)
                {
                    if (notes.ToList().Where(x => x.DisplayToUser == true).ToList().Count > 0)
                    {
                        DisplaytoUser = true;
                    }
                    //pnlNotesDisplay.Visible = true;
                    NotesTitle = HealthyChef.Common.Enums.GetEnumDescription(CurrentNoteType) + "(s)";
                    DisplayNote = notes.Select(a => a.Note).DefaultIfEmpty(string.Empty).Aggregate((a, b) => a + "; " + b);
                }


            }
            else
            {
                var _allPrefs = hccPreference.GetBy(Enums.PreferenceType.Customer, false);
                foreach (var p in _allPrefs)
                {
                    var _prefItem = new CustomerPreferenceItem();

                    _prefItem.PreferenceID = p.PreferenceID;
                    _prefItem.Name = p.Name;
                    this.AllPreferences.Add(_prefItem);
                }
            }
        }
    }

    public class CustomerSubProfileAllergensUpdate
    {
        public string UserID { get; set; }
        public int ProfileId { get; set; }

        public List<CustomerAllergenItem> AllAllergens { get; set; }

        public int[] AllergensSelected { get; set; }

        public CustomerSubProfileAllergensUpdate()
        {

        }

        public CustomerSubProfileAllergensUpdate(hccUserProfile _profile)
        {
            this.AllAllergens = new List<CustomerAllergenItem>();

            if (_profile != null)
            {
                this.UserID = _profile.MembershipID.ToString();
                this.ProfileId = _profile.UserProfileID;

                var _allAllergens = hccAllergen.GetActive();
                List<hccUserProfileAllergen> userAllgs = hccUserProfileAllergen.GetBy(_profile.UserProfileID, true);

                foreach (var _a in _allAllergens)
                {
                    var _allerItem = new CustomerAllergenItem();

                    _allerItem.AllergenID = _a.AllergenID;
                    _allerItem.Name = _a.Name;

                    if (userAllgs.Where(a => a.AllergenID == _a.AllergenID).Count() > 0)
                    {
                        _allerItem.IsSelected = true;
                    }

                    this.AllAllergens.Add(_allerItem);
                }
            }
            else
            {
                var _allAllergens = hccAllergen.GetActive();
                foreach (var _a in _allAllergens)
                {
                    var _allerItem = new CustomerAllergenItem();

                    _allerItem.AllergenID = _a.AllergenID;
                    _allerItem.Name = _a.Name;

                    this.AllAllergens.Add(_allerItem);
                }
            }
        }
    }
}