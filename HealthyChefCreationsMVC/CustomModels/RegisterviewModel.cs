using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class RegisterviewModel
    {
        public int ActiveTab { get; set; }
        public string UserProfileId { get; set; }
        public ShippinInfo ShippinInfoModel { get; set; }
        public BillingInfo BillingInfoModel { get; set; }
        public CardInformation CreditCardInfoModel { get; set; }
        public CreatePassword PasswordModel { get; set; }

        public RegisterviewModel()
        {
            this.ActiveTab = 1;
            this.ShippinInfoModel = new ShippinInfo() {UserID = this.UserProfileId };
            this.BillingInfoModel = new BillingInfo() { UserID = this.UserProfileId };
            this.CreditCardInfoModel = new CardInformation() { UserID = this.UserProfileId };
            this.PasswordModel = new CreatePassword() { UserID = this.UserProfileId };
        }

        public RegisterviewModel(int _userProfileId)
        {
            this.UserProfileId = _userProfileId.ToString();
            this.ShippinInfoModel = new ShippinInfo() { UserID = this.UserProfileId };
            this.BillingInfoModel = new BillingInfo() { UserID = this.UserProfileId };
            this.CreditCardInfoModel = new CardInformation() { UserID = this.UserProfileId };
            this.PasswordModel = new CreatePassword() { UserID = this.UserProfileId };
        }
    }


    public class ShippinInfo
    {
        public string UserID { get; set; }
        public int ShippingAddressID { get; set; }

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

        public ShippinInfo()
        {

        }
        public ShippinInfo(hccUserProfile _profile)
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
            }
        }
    }

    public class BillingInfo
    {
        public string UserID { get; set; }
        public int BillingAddressID { get; set; }

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

        public BillingInfo()
        {

        }

        public BillingInfo(hccUserProfile _profile)
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
                    this.FirstName = CurrentAddress.FirstName;
                    this.LastName = CurrentAddress.LastName;
                    this.Address1 = CurrentAddress.Address1;
                    this.Address2 = CurrentAddress.Address2;
                    this.City = CurrentAddress.City;
                    this.State = CurrentAddress.State;
                    this.PostalCode = CurrentAddress.PostalCode;
                    this.Phone = CurrentAddress.Phone;
                }
            }
        }
    }

    public class CardInformation
    {
        public string UserID { get; set; }

        [Display(Name = "Update Card Information")]
        public bool UpdateCreditCardInfo { get; set; }

        [Display(Name = "Name On Card")]
        [Required(ErrorMessage = "Name On Card is required.")]
        public string NameOnCard { get; set; }

        [Display(Name = "Card Number")]
        [Required(ErrorMessage = "Card Number is required.")]
        public string CardNumber { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is required.")]
        public int ExipiresOnMonth { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year is required.")]
        public int ExipiresOnYear { get; set; }

        public string CardIdCode { get; set; }

        public CardInformation()
        {

        }

        public CardInformation(hccUserProfile _profile)
        {
            this.UserID = _profile.MembershipID.ToString();

            if (_profile != null)
            {
                hccUserProfilePaymentProfile paymentProfile = hccUserProfilePaymentProfile.GetBy(_profile.UserProfileID);
                if (paymentProfile != null)
                {
                    var CurrentCardInfo = paymentProfile.ToCardInfo();

                    this.NameOnCard = CurrentCardInfo.NameOnCard;
                    this.CardNumber = "************" + CurrentCardInfo.CardNumber;
                    this.ExipiresOnMonth = CurrentCardInfo.ExpMonth;
                    this.ExipiresOnYear = CurrentCardInfo.ExpYear;
                }
            }
        }

    }

    public class CreatePassword
    {
        public string UserID { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Enter Email")]
        [DataType(DataType.Password)]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter Password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Re enter Current Password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}