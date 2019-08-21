using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL.Classes
{
    public class ActiveCustomerDto
    {
        #region public data members

        public string Email { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? FirstPurchaseDate { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        public decimal LastPurchaseAmount { get; set; }
        public int LastPurchaseId { get; set; }
        public int TotalOrders { get; set; }
        public string MktgOptIn { get; set; }
        public bool IsActive { get; set; }
        public decimal TotalAmount { get; set; }
        public string AccountPreferences { get; set; }
        public string AccountAllergens { get; set; }
        public string ProductType { get; set; }
        public string AccountCustomPreferences { get; set; }
        public int StatusId;
        

        #endregion
    }
}
