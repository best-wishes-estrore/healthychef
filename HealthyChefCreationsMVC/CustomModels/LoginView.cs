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
    public class LoginViewModel
    {
        [Display(Name ="E-Mail")]
        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        [DataType(DataType.Password)]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        public string Password { get; set; }

       
        public int CustomerType { get; set; }

        public RegistrationModel registrationModel { get; set; }

        public string ReturnUrl { get; set; }

        public bool IsSubscribe { get; set; }

        public LoginViewModel()
        {
            CustomerType = 0;
            IsSubscribe = false;
        }

        public LoginViewModel(string _returnUrl)
        {
            if (_returnUrl == null)
            {
                CustomerType = 1;
                this.ReturnUrl = _returnUrl;
                this.registrationModel = new RegistrationModel();
                IsSubscribe = false;
            }
            else
            if(_returnUrl == "/cart.aspx")
            {
                CustomerType = 1;
                this.ReturnUrl = _returnUrl;
                this.registrationModel = new RegistrationModel();
                IsSubscribe = true;
            }
            else
            {
                CustomerType = 1;
                this.ReturnUrl = _returnUrl;
                this.registrationModel = new RegistrationModel();
            }
        }
        public class RegistrationModel
        {

            [Display(Name = "FirstName")]
            [Required(ErrorMessage = "Please Enter First Name")]
            public string FirstName { get; set; }

            [Display(Name = "LastName")]
            [Required(ErrorMessage = "Please Enter Last Name")]
            public string LastName { get; set; }
            [Display(Name = "E-Mail")]
            [Required(ErrorMessage = "Please Enter Email Address")]
            [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Please Enter Password")]
            [DataType(DataType.Password)]
            [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
            public string Password { get; set; }
            public int CustomerType { get; set; }
            public RegistrationModel()
            {
                CustomerType = 0;
            }
        }
    }

    public class ForgotPasswordViewModel
    {      
        public int Id { get; set; }
        [Display(Name ="Email:")]
        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
    }
    
    //public class ShippinInfo
    //{
    //    public string UserID { get; set; }
    //    public int ShippingAddressID { get; set; }

    //    [Display(Name = "First Name")]
    //    [Required(ErrorMessage = "First Name is required.")]
    //    public string FirstName { get; set; }

    //    [Display(Name = "Last Name")]
    //    [Required(ErrorMessage = "Last Name is required.")]
    //    public string LastName { get; set; }

    //    [Display(Name = "Address 1")]
    //    [Required(ErrorMessage = "Address1 is required.")]
    //    public string Address1 { get; set; }

    //    [Display(Name = "Address 2")]
    //    public string Address2 { get; set; }

    //    [Display(Name = "City")]
    //    [Required(ErrorMessage = "City is required.")]
    //    public string City { get; set; }

    //    [Display(Name = "State")]
    //    [Required(ErrorMessage = "State is required.")]
    //    public string State { get; set; }

    //    [Display(Name = "Phone")]
    //    public string Phone { get; set; }

    //    [Display(Name = "Zip Code")]
    //    [Required(ErrorMessage = "Zip Code is required.")]
    //    public string PostalCode { get; set; }

    //    [Display(Name = "Delivery Type")]
    //    public int DefaultShippingTypeID { get; set; }

    //    [Display(Name = "Is a Business Address?")]
    //    public bool IsBusiness { get; set; }

    //    public ShippinInfo()
    //    {

    //    }
    //    public ShippinInfo(hccUserProfile _profile)
    //    {
    //        if (_profile != null)
    //        {
    //            this.UserID = _profile.MembershipID.ToString();

    //            var CurrentAddress = hccAddress.GetById(_profile.ShippingAddressID ?? 0);
    //            if (CurrentAddress == null)
    //            {
    //                CurrentAddress = new hccAddress { Country = "US", AddressTypeID = 4 };
    //            }
    //            else
    //            {
    //                this.ShippingAddressID = CurrentAddress.AddressID;
    //                this.FirstName = CurrentAddress.FirstName;
    //                this.LastName = CurrentAddress.LastName;
    //                this.Address1 = CurrentAddress.Address1;
    //                this.Address2 = CurrentAddress.Address2;
    //                this.City = CurrentAddress.City;
    //                this.State = CurrentAddress.State;
    //                this.PostalCode = CurrentAddress.PostalCode;
    //                this.Phone = CurrentAddress.Phone;
    //                this.IsBusiness = CurrentAddress.IsBusiness;
    //                this.DefaultShippingTypeID = CurrentAddress.DefaultShippingTypeID;
    //            }
    //        }
    //    }
    //}

    //public class BillingInfo
    //{
    //    public string UserID { get; set; }
    //    public int BillingAddressID { get; set; }

    //    [Display(Name = "First Name")]
    //    [Required(ErrorMessage = "First Name is required.")]
    //    public string FirstName { get; set; }

    //    [Display(Name = "Last Name")]
    //    [Required(ErrorMessage = "Last Name is required.")]
    //    public string LastName { get; set; }

    //    [Display(Name = "Address 1")]
    //    [Required(ErrorMessage = "Address1 is required.")]
    //    public string Address1 { get; set; }

    //    [Display(Name = "Address 2")]
    //    public string Address2 { get; set; }

    //    [Display(Name = "City")]
    //    [Required(ErrorMessage = "City is required.")]
    //    public string City { get; set; }

    //    [Display(Name = "State")]
    //    [Required(ErrorMessage = "State is required.")]
    //    public string State { get; set; }

    //    [Display(Name = "Phone")]
    //    public string Phone { get; set; }

    //    [Display(Name = "Zip Code")]
    //    [Required(ErrorMessage = "Zip Code is required.")]
    //    public string PostalCode { get; set; }

    //    public BillingInfo()
    //    {

    //    }

    //    public BillingInfo(hccUserProfile _profile)
    //    {
    //        if (_profile != null)
    //        {
    //            this.UserID = _profile.MembershipID.ToString();

    //            var CurrentAddress = hccAddress.GetById(_profile.BillingAddressID ?? 0);
    //            if (CurrentAddress == null)
    //            {
    //                CurrentAddress = new hccAddress { Country = "US", AddressTypeID = 2 };
    //            }
    //            else
    //            {
    //                this.BillingAddressID = CurrentAddress.AddressID;
    //                this.FirstName = CurrentAddress.FirstName;
    //                this.LastName = CurrentAddress.LastName;
    //                this.Address1 = CurrentAddress.Address1;
    //                this.Address2 = CurrentAddress.Address2;
    //                this.City = CurrentAddress.City;
    //                this.State = CurrentAddress.State;
    //                this.PostalCode = CurrentAddress.PostalCode;
    //                this.Phone = CurrentAddress.Phone;
    //            }
    //        }
    //    }
    //}

    //public class CardInfo
    //{
    //    public string UserID { get; set; }

    //    [Display(Name = "Update Card Information")]
    //    public bool UpdateCreditCardInfo { get; set; }

    //    [Display(Name = "Name On Card")]
    //    [Required(ErrorMessage = "Name On Card is required.")]
    //    public string NameOnCard { get; set; }

    //    [Display(Name = "Card Number")]
    //    [Required(ErrorMessage = "Card Number is required.")]
    //    public string CardNumber { get; set; }

    //    [Display(Name = "Month")]
    //    [Required(ErrorMessage = "Month is required.")]
    //    public int ExipiresOnMonth { get; set; }

    //    [Display(Name = "Year")]
    //    [Required(ErrorMessage = "Year is required.")]
    //    public int ExipiresOnYear { get; set; }

    //    public string CardIdCode { get; set; }

    //    public CardInfo()
    //    {

    //    }

    //    public CardInfo(hccUserProfile _profile)
    //    {
    //        this.UserID = _profile.MembershipID.ToString();

    //        if (_profile != null)
    //        {
    //            hccUserProfilePaymentProfile paymentProfile = hccUserProfilePaymentProfile.GetBy(_profile.UserProfileID);
    //            if (paymentProfile != null)
    //            {
    //                var CurrentCardInfo = paymentProfile.ToCardInfo();

    //                this.NameOnCard = CurrentCardInfo.NameOnCard;
    //                this.CardNumber = "************" + CurrentCardInfo.CardNumber;
    //                this.ExipiresOnMonth = CurrentCardInfo.ExpMonth;
    //                this.ExipiresOnYear = CurrentCardInfo.ExpYear;
    //            }
    //        }
    //    }

    //}

    //public class CreatePassword
    //{
    //    public string UserID { get; set; }

    //    [Display(Name = "Current Password")]
    //    [Required(ErrorMessage = "Enter Current Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    public string CurrentPassword { get; set; }

    //    [Display(Name = "New Password")]
    //    [Required(ErrorMessage = "Enter New Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    public string NewPassword { get; set; }

    //    [Display(Name = "Confirm Password")]
    //    [Required(ErrorMessage = "Re enter Current Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //    //public CreatePassword()
    //    //{

    //    //}

    //    //public CreatePassword(hccUserProfile _profile)
    //    //{
    //    //    if (_profile != null)
    //    //    {
    //    //        this.UserID = _profile.MembershipID.ToString();
    //    //    }
    //    //}
    //}



    //public class RegisterViewModel
    //{
    //    //Login Information
    //    [Display(Name ="Email")]
    //    [Required(ErrorMessage = "Please Enter Email Address")]
    //    [DataType(DataType.EmailAddress, ErrorMessage = "Please Enter a valid Email Address")]
    //    public string Email { get; set; }
    //    [Display(Name = "Password")]
    //    [Required(ErrorMessage = "Enter Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }
    //    [Display(Name = "Confirm Password")]
    //    [Required(ErrorMessage = "Re enter Current Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }
    //    public string gmail { get; set; }
    //    //Shipping Information.
    //    [Display(Name = "First Name")]
    //    [Required(ErrorMessage = "Please Enter FirstName")]
    //    public string FirstName { get; set; }
    //    [Display(Name = "Last Name")]
    //    [Required(ErrorMessage = "Please Enter LastName")]
    //    public string LastName { get; set; }
    //    [Display(Name = "Address 1")]
    //    [Required(ErrorMessage = "Please Enter Address1")]
    //    public string Address1 { get; set; }
    //    [Display(Name = "Address 2")]
    //    public string Address2 { get; set; }
    //    [Display(Name = "City")]
    //    [Required(ErrorMessage = "Please Enter City")]
    //    public string City { get; set; }
    //    [Display(Name = "State")]
    //    [Required(ErrorMessage = "You must select a State")]
    //    public string States { get; set; }
    //    [Display(Name = "Phone")]
    //    public string Phone { get; set; }
    //    [Display(Name = "Zip code")]
    //    [Required(ErrorMessage = "Please Enter ZipCode")]
    //    [MinLength(5,ErrorMessage = "ZipCode Must be 5 in Length")]
    //    public string PostalCode { get; set; }
    //    //Billing Information
    //    [Display(Name = "First Name")]
    //    [Required(ErrorMessage = "Please Enter FirstName")]
    //    public string BillFirstName { get; set; }
    //    [Display(Name = "Last Name")]
    //    [Required(ErrorMessage = "Please Enter LastName")]
    //    public string BillLastName { get; set; }
    //    [Display(Name = "Address 1")]
    //    [Required(ErrorMessage = "Please Enter Address1")]
    //    public string BillAddress1 { get; set; }
    //    [Display(Name = "Address 2")]
    //    public string BillAddress2 { get; set; }
    //    [Display(Name ="City")]
    //    [Required(ErrorMessage = "Please Enter City")]
    //    public string BillCity { get; set; }
    //    [Display(Name = "State")]
    //    [Required(ErrorMessage = "You must select a State")]
    //    public string BillStates { get; set; }
    //    [Display(Name = "Phone")]
    //    [MinLength(10,ErrorMessage ="Please Enter the 10 numbers only")]
    //    public string BillPhone { get; set; }
    //    [Display(Name = "Zip code")]
    //    [Required(ErrorMessage = "Please Enter ZipCode")]
    //    public string BillPostalCode { get; set; }
    //    //CardType
    //    public int DefaultShippingTypeID { get; set; }
    //    public bool IsBusiness { get; set; }
    //    [Display(Name = "NameonCard")]
    //    [Required(ErrorMessage = "Please Enter NameofCard")]
    //    public string NameonCard { get; set; }
    //    [Required(ErrorMessage = "Please select Month")]
    //    public string ExpireInMonth { get; set; }
    //    [Required(ErrorMessage = "Please select Year")]
    //    public string ExpireInYear { get; set; }
    //    [Display(Name = "Security Code")]
    //    [Required(ErrorMessage = "Please Enter CardId")]
    //    public string SecurityCode { get; set; }
    //    [Display(Name = "Security Code")]
    //    public string DeliveryDate { get; set; }
    //    public bool UpdateCreditCardInfo { get; set; }
    //    [Display(Name = "Delivery type")]
    //    public int Deliverytype { get; set; }
    //    [Display(Name = "Card Number")]
    //    [Required(ErrorMessage = "Please Enter CardNumber")]
    //    public string CardNumber { get; set; }
    //    public string UserID { get; set; }
    //    public int UserIdint { get; set; }
    //    public int ShippingAddressID { get; set; }
    //    public int BillingAddressID { get; set; }
    //    public int BussinessValue { get; set; }
    //    public int BillExpiremonth { get; set; }
    //    public int BillExpireYear { get; set; }
    //    public int CardType { get; set; }
    //}

    //public class CustomerUpdatePassword
    //{
    //    public string UserID { get; set; }

    //    [Display(Name = "Current Password")]
    //    [Required(ErrorMessage = "Enter Current Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    public string CurrentPassword { get; set; }

    //    [Display(Name = "New Password")]
    //    [Required(ErrorMessage = "Enter New Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    public string NewPassword { get; set; }

    //    [Display(Name = "Confirm Password")]
    //    [Required(ErrorMessage = "Re enter Current Password")]
    //    [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
    //    [DataType(DataType.Password)]
    //    [System.ComponentModel.DataAnnotations.Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    //    public string ConfirmPassword { get; set; }

    //    public CustomerUpdatePassword()
    //    {

    //    }

    //    //public CustomerUpdatePassword(hccUserProfile _profile)
    //    //{
    //    //    if (_profile != null)
    //    //    {
    //    //        this.UserID = _profile.MembershipID.ToString();
    //    //    }
    //    //}
    //}

    public class Response
    {
        public string Status { get; set; }
        public int StatusCode { get; set; }
        public object PayLoad { get; set; }
        public string Message { get; set; } 
    }
}


//public class RegisterViewModel
//{
//    public string UserID { get; set; }
//    public int UserId { get; set; }
//    public int ShippingAddressID { get; set; }
//    public int BillingAddressID { get; set; }
//    [Required(ErrorMessage = "Please Enter FirstName")]
//    public string FirstName { get; set; }
//    [Required(ErrorMessage = "Please Enter LastName")]
//    public string LastName { get; set; }
//    [Required(ErrorMessage = "Please Enter Address1")]
//    public string Address1 { get; set; }
//    public string Address2 { get; set; }
//    [Required(ErrorMessage = "Please Enter City")]
//    public string City { get; set; }
//    [Required(ErrorMessage = "Please Enter State")]
//    public string State { get; set; }
//    public string Phone { get; set; }
//    [Required(ErrorMessage = "Please Enter ZipCode")]
//    public string PostalCode { get; set; }
//    public int DefaultShippingTypeID { get; set; }
//    public bool IsBusiness { get; set; }
//    [Required(ErrorMessage = "Please Enter NameofCard")]
//    public string NameonCard { get; set; }
//    [Required(ErrorMessage = "Please Enter CardNumber")]
//    public string CardNumber { get; set; }
//    [Required(ErrorMessage = "Please select Month")]
//    public int ExpireInMonth { get; set; }
//    [Required(ErrorMessage = "Please select Year")]
//    public int ExpireInYear { get; set; }
//    [Required(ErrorMessage = "Please Enter CardId")]
//    public string CardIDCode { get; set; }
//    public string DeliveryDate { get; set; }
//    public bool UpdateCreditCardInfo { get; set; }
//    public int Deliverytype { get; set; }
//    public Enums.CreditCardType CardType
//    {
//        get
//        {
//            return ValidateCardNumber(this.CardNumber);
//        }
//    }
//    private Enums.CreditCardType ValidateCardNumber(string sCardNumber)
//    {
//        string cardNum = sCardNumber.Replace(" ", "");

//        Enums.CreditCardType retVal = Enums.CreditCardType.Unknown;

//        //validate the type of card is accepted
//        if (cardNum.StartsWith("4") == true &&
//            (cardNum.Length == 13
//                || cardNum.Length == 16))
//        {
//            //VISA
//            retVal = Enums.CreditCardType.Visa;
//        }
//        else if ((cardNum.StartsWith("51") == true ||
//                  cardNum.StartsWith("52") == true ||
//                  cardNum.StartsWith("53") == true ||
//                  cardNum.StartsWith("54") == true ||
//                  cardNum.StartsWith("55") == true) &&
//                 cardNum.Length == 16)
//        {
//            //MasterCard
//            retVal = Enums.CreditCardType.MasterCard;
//        }
//        else if ((cardNum.StartsWith("34") == true ||
//                  cardNum.StartsWith("37") == true) &&
//                 cardNum.Length == 15)
//        {
//            //Amex
//            retVal = Enums.CreditCardType.AmericanExpress;
//        }
//        else if (cardNum.StartsWith("6011") == true &&
//                 cardNum.Length == 16)
//        {
//            //Discover
//            retVal = Enums.CreditCardType.Discover;
//        }

//        if (retVal != Enums.CreditCardType.Unknown)
//        {
//            int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
//            int checksum = 0;
//            char[] chars = cardNum.ToCharArray();
//            for (int i = chars.Length - 1; i > -1; i--)
//            {
//                int j = ((int)chars[i]) - 48;
//                checksum += j;
//                if (((i - chars.Length) % 2) == 0)
//                    checksum += DELTAS[j];
//            }

//            if ((checksum % 10) != 0)
//                retVal = Enums.CreditCardType.Unknown;
//        }

//        return retVal;
//    }
//}