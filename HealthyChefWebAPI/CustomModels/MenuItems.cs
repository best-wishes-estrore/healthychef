using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class MenuItems
    {
      public int MenuItemID { get; set; }
      public string Name { get; set; }
      public int MealTypeID { get; set; }
      public string Description { get; set; }
      public bool IsRetired { get; set; }
      public bool IsTaxEligible { get; set; }
      public bool UseCostSmall { get; set; }
      public decimal CostSmall { get; set; }
      public bool UseCostRegular { get; set; }
      public decimal CostRegular { get; set; }
      public bool UseCostLarge { get; set; }
      public decimal CostLarge { get; set; }
      public bool UseCostChild { get; set; }
      public decimal CostChild { get; set; }
      public bool CanyonRanchRecipe { get; set; }
        public bool CanyonRanchApproved { get; set; }
        public bool VegetarianOptionAvailable { get; set; }
        public bool VeganOptionAvailable { get; set; }
        public bool GlutenFreeOptionAvailable { get; set; }
    }
}