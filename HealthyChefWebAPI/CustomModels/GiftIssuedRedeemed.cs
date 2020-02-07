using HealthyChefWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class GiftIssued
    {
        public int CartItemId { get; set; }
        public string RedeemCode { get; set; }
        public decimal Amount { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public string IssuedDate { get; set; }
        public bool SendToRecipient { get; set; }

        public string IssuedTo
        {
            get
            {
                return getCustomerName();
            }
        }
        private string getCustomerName()
        {
            if (string.IsNullOrEmpty(this.FirstName))
                return this.Lastname;
            else if (string.IsNullOrEmpty(this.Lastname))
                return this.FirstName;
            else
                return this.Lastname + "," + this.FirstName;

        }
    }

    public class GiftRedeemed
    {
        public int CartItemId { get; set; }
        public string RedeemCode { get; set; }
        public decimal Amount { get; set; }
        public string IssuedTo { get; set; }
        public string IssuedDate { get; set; }
        public string RedeemedBy { get; set; }
        public string RedeemedDate { get; set; }
    }
    //Post
    public class GiftCertificates
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public decimal Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Phone { get; set; }
        public string ReceipientEmail { get; set; }
        public string ReceipientMessage { get; set; }
        public bool SendtoReceipient { get; set; }
        public int AddressID { get; set; }
        public int AddressTypeID { get; set; }
    }

    public class GiftCertificatesResult : PostHttpResponse
    {
        public int CartId { get; set; }
        public int CartItemId { get; set; }
        public decimal Amount { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Phone { get; set; }
        public string ReceipientEmail { get; set; }
        public string ReceipientMessage { get; set; }
        public bool SendtoReceipient { get; set; }
        public int AddressTypeID { get; set; }
    }

   

}