using HealthyChef.Common;
using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class LoginRegistrationViewModel
    {
        public LoginRegistrationViewModel()
        {

        }

        public LoginRegistrationViewModel(string _email,string firstname,string lastname,bool SameasShippingAddress)
        {
            this.UserEmail = _email;
            this.Email = _email;
            this.BillingFirstName = firstname;
            this.BillingLastName = lastname;
            this.ShippingFirstName = firstname;
            this.ShippingLastName = lastname;
            this.SameasShipping = SameasShippingAddress;
        }

        public LoginRegistrationViewModel(bool isActive)
        {

            MembershipUser user = Helpers.LoggedUser;
            if (user != null)
            {
                var userProviderKey = Guid.Parse(Convert.ToString(user.ProviderUserKey));
                this.profile = hccUserProfile.GetParentProfileBy(userProviderKey);
                this.UserEmail = user.Email;
                this.Email = user.Email;
            }
            else
            {
                
            }
        }

        public int UserProfileId { get; set; }
        public string UserEmail { get; set; }
        public hccUserProfile profile { get; set; }
        public string UserID { get; set; }


        //shipping info properties
        public int ShippingAddressID { get; set; }

        
        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression("^[a-zA-Z\\s]*$", ErrorMessage = "Only Alphabets allowed.")]
        public string ShippingFirstName { get; set; }

        [Display(Name = "Last Name")]
        [RegularExpression("^[a-zA-Z\\s]*$", ErrorMessage = "Only Alphabets allowed.")]
        [Required(ErrorMessage = "Last Name is required.")]

        public string ShippingLastName { get; set; }

        [Display(Name = "Address 1")]
        [Required(ErrorMessage = "Address1 is required.")]
        public string ShippingAddress1 { get; set; }

        [Display(Name = "Address 2")]
        public string ShippingAddress2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Only Alphabets allowed.")]
        public string ShippingCity { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required.")]
        public string ShippingState { get; set; }

        //[Required(ErrorMessage = "phone number required")]
        [Display(Name = "Phone")]
        //[MinLength(10,ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        //[MaxLength(20, ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        public string ShippingPhone { get; set; }

        [Display(Name = "Zip Code")]
        [MaxLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [MinLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [Required(ErrorMessage = "Zip Code is required.")]
        public string ShippingPostalCode { get; set; }

        [Display(Name = "Delivery Type")]
        public int DefaultShippingTypeID { get; set; }

        [Display(Name = "Is a Business Address?")]
        public bool IsBusiness { get; set; }

        //billing address info
        public int BillingAddressID { get; set; }

        [Display(Name = "This is same as my shipping address")]
        public bool SameasShipping { get; set; }

        [Display(Name = "First Name")]
        [Required(ErrorMessage = "First Name is required.")]
        [RegularExpression("^[a-zA-Z\\s]*$", ErrorMessage = "Only Alphabets allowed.")]
        public string BillingFirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "Last Name is required.")]
        [RegularExpression("^[a-zA-Z\\s]*$", ErrorMessage = "Only Alphabets allowed.")]
        public string BillingLastName { get; set; }

        [Display(Name = "Address 1")]
        [Required(ErrorMessage = "Address1 is required.")]
        public string BillingAddress1 { get; set; }

        [Display(Name = "Address 2")]
        public string BillingAddress2 { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "City is required.")]
        [RegularExpression("[A-Za-z ]*", ErrorMessage = "Only Alphabets allowed.")]
        public string BillingCity { get; set; }

        [Display(Name = "State")]
        [Required(ErrorMessage = "State is required.")]
        public string BillingState { get; set; }

        //[Required(ErrorMessage = "phone number required")]
        [Display(Name = "Phone")]
        //[MinLength(10, ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        //[MaxLength(20, ErrorMessage = "Phone Number Should not be grater than 20 or less than 10.")]
        public string BillingPhone { get; set; }

        [Display(Name = "Zip Code")]
        [MaxLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [MinLength(5, ErrorMessage = "Zip Code should be 5 digits in length.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Enter Valid Postal Code.")]
        [Required(ErrorMessage = "Zip Code is required.")]
        public string BillingPostalCode { get; set; }

        //card info properties
        [Display(Name = "Update Card Information")]
        public bool UpdateCreditCardInfo { get; set; }

        [Display(Name = "Name On Card")]
        [Required(ErrorMessage = "Name On Card is required.")]
        [RegularExpression("^[a-zA-Z\\s]*$", ErrorMessage = "Only Alphabets allowed.")]
        public string NameOnCard { get; set; }

        [Display(Name = "Card Number")]
        [Required(ErrorMessage = "Card Number is required.")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Only Numbers allowed.")]
        [StringLength(30, ErrorMessage = "cannot exceed 15 characters")]
        public string CardNumber { get; set; }

        [Display(Name = "Month")]
        [Required(ErrorMessage = "Month is required.")]
        public int ExipiresOnMonth { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year is required.")]
        public int ExipiresOnYear { get; set; }

        [Display(Name = "Card Id Code")]
        [Required(ErrorMessage ="Please Enter Code")]
        [MaxLength(4, ErrorMessage = "CardIdCode should be 4 digits in length.")]
        [MinLength(3, ErrorMessage = "CardIdCode should be 3 digits in length.")]
        [RegularExpression("^[0-9]*$", ErrorMessage ="the cvv code is Invalid")]
        public string CardIdCode { get; set; }

        //login details

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Please Enter Email Address")]
        [EmailAddress(ErrorMessage = "Please Enter a Valid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Enter the Password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        [DataType(DataType.Password)]
        public string RegiPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Re enter the Password")]
        [MinLength(5, ErrorMessage = "Password must be at least 5 characters in length.")]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("RegiPassword", ErrorMessage = "The password and confirmation password should be same")]
        public string ConfirmPassword { get; set; }
    }
}