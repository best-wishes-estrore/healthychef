using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class ContactUsViewModel
    {
       
        [Required(ErrorMessage = "Please Enter First Name")]
        [Display(Name= "First Name")]  
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "City")]
        public string City { get; set; }

       
        [Display(Name = "State")]
        public string State { get; set; }

        [MinLength(5, ErrorMessage = "Zip Code should be 5 digits in length")]
        [MaxLength(5, ErrorMessage = "Zip Code should be 5 digits in length")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [MinLength(10,ErrorMessage = "Phone Number Should not be greater than 20 or less than 10")]
        [MaxLength(20, ErrorMessage = "Phone Number Should not be greater than 20 or less than 10")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Please Enter Email")]
        [EmailAddress(ErrorMessage = "Email Address Not Valid")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = @"Question / Comments")]
        public string QuestionsComments { get; set; }
    }
}