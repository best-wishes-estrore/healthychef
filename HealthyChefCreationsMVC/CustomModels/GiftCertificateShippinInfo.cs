using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class GiftCertificateShippinInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public string ReceiptEmail { set; get; }
        public string Amount { set; get; }
        public string ReceiptMessage { set; get; }
    }
}