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
    }
}