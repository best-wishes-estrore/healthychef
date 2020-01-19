using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class ShippingZone
    {
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public string Description { get; set; }
        public string Multiplier { get; set; }
        public decimal MinFee { get; set; }
        public decimal MaxFee { get; set; }
        public bool IsDefaultShippingZone { get; set; }
        public bool IsPickupShippingZone { get; set; }
        public string TypeName { get; set; }

        public int OrderMinimum { get; set; }
    }


    public class ShippingZoneItem : Helpers.PostHttpResponse
    {
        public int ZoneID { get; set; }
        public string ZoneName { get; set; }
        public string Multiplier { get; set; }
        public decimal MinFee { get; set; }
        public decimal MaxFee { get; set; }
        public bool IsDefaultShippingZone { get; set; }
        public bool IsPickupShippingZone { get; set; }
        public string TypeName { get; set; }
    }
}