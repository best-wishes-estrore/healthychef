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

        public MealPreferencesItem()
        {

        }

        public MealPreferencesItem(MealPreferences _mealPreferencesToValidate)
        {
            if (string.IsNullOrEmpty(_mealPreferencesToValidate.Name))
            {
                this.AddValidationError("Preference Name is Required");
                this.isValid = false;
            }
            if (string.IsNullOrEmpty(_mealPreferencesToValidate.Description))
            {
                this.AddValidationError("Preference Description is Required");
                this.isValid = false;
            }
            if (_mealPreferencesToValidate.PrefType == -1)
            {
                this.AddValidationError("Preference Type is Required");
                this.isValid = false;
            }
        }
    }
}