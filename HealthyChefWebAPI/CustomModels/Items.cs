using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Items
    {
        public int MENUITEMID { get; set; }
        public string ITEMNAME { get; set; }
        public string MEALTYPE { get; set; }
        public decimal COSTCHILD { get; set; }
        public decimal COSTREGULAR { get; set; }
        public decimal COSTLARGE { get; set; }
        public decimal COSTSMALL { get; set; }
        public bool ISTAXELIGIBLE { get; set; }
        public bool ISRETIRED { get; set; }
        public string ALLERGENS { get; set; }


    }


    public class ItemPost
    {
        public int MenuItemId { get; set; }
        public string ItemName { get; set; }
        public string Description { get; set; }
        public int MealTypeId { get; set; }
        public string CostChild { get; set; }
        public string CostRegular { get; set; }
        public string CostLarge { get; set; }
        public string CostSmall { get; set; }
        public bool IsTaxEligible { get; set; }
        public bool IsRetired { get; set; }

        public string Caleries { get; set; }
        public string DietaryFiber { get; set; }
        public string Protein { get; set; }
        public string TotalCarbohydrates { get; set; }
        public string TotalFat { get; set; }

        public List<int> selectedIngredients { get; set; }
        public List<int> selectedPrefs { get; set; }
        public List<int> usedInMenus { get; set; }

        public bool UseCostChild { get; set; }
        public bool UseCostSmall { get; set; }
        public bool UseCostRegular { get; set; }
        public bool UseCostLarge { get; set; }

        public bool CanyonRanchRecipe { get; set; }
        public bool CanyonRanchApproved { get; set; }
        public bool VegetarianOptionAvailable { get; set; }
        public bool VeganOptionAvailable { get; set; }
        public bool GlutenFreeOptionAvailable { get; set; }
    }

}