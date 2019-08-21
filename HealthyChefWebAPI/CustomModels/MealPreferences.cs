using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class MealPreferences
    {
        public int PreferenceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRetired { get; set; }
        public int PrefType { get; set; }
    }



    public class MealPreferencesItem : Helpers.PostHttpResponse
    {
        public int PreferenceID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRetired { get; set; }
        public int PrefType { get; set; }
    }
}