using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class CustomerPreferences
    {
        public int PreferenceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRetired { get; set; }
    }
}