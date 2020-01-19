using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Allergens
    {
       public int AllergenID { get; set; }
       public string Name { get; set; }
       public string Description { get; set; }
       public bool IsActive { get; set; }
    }


    public class AllergensItem : Helpers.PostHttpResponse
    {
        public int AllergenID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public AllergensItem()
        {

        }

        public AllergensItem(Allergens _allergenToValidate)
        {
            if (string.IsNullOrEmpty(_allergenToValidate.Name))
            {
                this.AddValidationError("Allergen Name is Required");
                this.isValid = false;
            }
            if (string.IsNullOrEmpty(_allergenToValidate.Description))
            {
                this.AddValidationError("Allergen Description Name is Required");
                this.isValid = false;
            }
        }
    }

}