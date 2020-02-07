using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class SearchParams
    {
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public int? purchaseNumber { get; set; }
        public DateTime? deliveryDate { get; set; }
        public string roles { get; set; }

        public SearchParams()
        {
            this.lastName = this.email = this.phone = null;
            this.purchaseNumber = null;
            this.deliveryDate = null;
            this.roles = "Customer";
        }
    }
}