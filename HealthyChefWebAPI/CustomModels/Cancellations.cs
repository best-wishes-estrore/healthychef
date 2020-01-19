using HealthyChefWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Cancellations
    {
        public string ProfileName { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string DeliveryDate { get; set; }
        public bool IsCancelled { get; set; }
        public string OrderNumber { get; set; }
        public int ItemCount { get; set; }
        public bool Status{ get; set; }
        public int CartID { get; set; }
        public int StatusID { get; set; }
       
    }
}