using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class ThankYouPageModel
    {
        public int PurchaseNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal ShippingAmount { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        public string Country { get; set; }

        public int Isnewcustomer { get; set; }

        public string GoogleTrackScript { get; set; }

        public string UserName { get; set; }
    }
}