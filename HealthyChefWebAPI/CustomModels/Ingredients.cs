using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Ingredients
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRetired { get; set; }
        public List<int> AllergensIds { get; set; }
        public string Allergens
        {
            get
            {
                return _getAllergensstring();
            }
        }

        string _getAllergensstring()
        {
            string allergens = string.Empty;
            try
            {
                hccIngredient ing = new hccIngredient()
                {
                    IngredientID = this.IngredientID
                };


                ing.GetAllergens().ForEach(a => allergens += a.Name + ", ");

                return allergens.Trim().TrimEnd(',');
            }
            catch
            {
                return allergens;
            }
        }
    }


    public class IngredientItem : Helpers.PostHttpResponse
    {
        public int IngredientID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsRetired { get; set; }

        public IngredientItem()
        {

        }

        public IngredientItem(Ingredients _ingredientToValidate)
        {
            if (string.IsNullOrEmpty(_ingredientToValidate.Name))
            {
                this.AddValidationError("Ingredient Name is Required");
                this.isValid = false;
            }
            if (string.IsNullOrEmpty(_ingredientToValidate.Description))
            {
                this.AddValidationError("Ingredient Description is Required");
                this.isValid = false;
            }
        }
    }
}