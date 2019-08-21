using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public class ShippingLabelDetails
    {
        public int PurchaseNum { get; set; }
        public string OrderNumber { get; set; }
        public string Company {get;set;}
        public string Contact{get;set;}
        public string Address1 {get;set;}
        public string Address2 {get;set;}
        public string City {get;set;}
        public string StateProvince {get;set;}
        public string Country {get;set;}
        public string Zip {get;set;}
        public string Phone {get;set;}
        public string Email {get;set;}
        public int UserProfileId { get; set; }
        

    }
}
