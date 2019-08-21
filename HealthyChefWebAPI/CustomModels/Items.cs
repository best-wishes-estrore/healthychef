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
        public decimal CostChild { get; set; }
        public decimal CostRegular { get; set; }
        public decimal CostLarge { get; set; }
        public decimal CostSmall { get; set; }
        public bool IsTaxEligible { get; set; }
        public bool IsRetired { get; set; }

        public int Caleries { get; set; }
        public int DietaryFiber { get; set; }
        public int Protein { get; set; }
        public int TotalCarbohydrates { get; set; }
        public int TotalFat { get; set; }

        public List<int> selectedIngredients { get; set; }
        public List<int> selectedPrefs { get; set; }
        public List<int> usedInMenus { get; set; }
    }

}