using HealthyChef.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Purchase
    {
        public int CartID { get; set; }
        public Guid? AspNetUserID { get; set; }
        public int PurchaseNumber { get; set; }
        public int StatusID { get; set; }
        public decimal SubTotalAmount { get; set; }
        public decimal SubTotalDiscount { get; set; }
        public decimal ShippingAmount { get; set; }
        public decimal ShippingDiscount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal DiscretionaryTaxAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TaxDiscount { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TotalDiscount { get; set; }
        public int CouponID { get; set; }
        public DateTime PurchaseDate { get; set; }
        public Guid? PurchaseBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string AuthNetResponse { get; set; }
        public decimal AnonGiftRedeemCredit { get; set; }
        public bool IsTestOrder { get; set; }
        public decimal CreditAppliedToBalance { get; set; }
        public decimal PaymentDue { get; set; }
        public int PaymentProfileID { get; set; }
        public string RedeemedGiftCertCode { get; set; }
        public string AnonymousID { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public decimal TaxRate { get; set; }
    }



    public class PurchaseDetails
    {
        public int PurchaseNumber { get; set; }
        public int CartID { get; set; }
        public Guid? AspNetUserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PurchaseDate { get; set; }
        public int StatusID { get; set; }
        public decimal TotalAmount { get; set; }
        public string DeliveryDate { get; set; }


        public string CustomerName
        {
            get
            {
                return getCustomerName();
            }
        }

        public string Status
        {
            get
            {
                return getStatus();
            }
        }

        private string getCustomerName()
        {
            if (string.IsNullOrEmpty(this.FirstName))
                return this.LastName;
            else if (string.IsNullOrEmpty(this.LastName))
                return this.FirstName;
            else
                return this.FirstName + "," + this.LastName;

        }


        private string getStatus()
        {
           return Enums.GetEnumDescription(((Enums.CartStatus) this.StatusID));
        }
    }
}