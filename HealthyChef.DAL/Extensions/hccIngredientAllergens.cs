using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccIngredientAllergen
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccIngredientAllergens", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccIngredientAllergens", this);
                    }
                    else
                    {
                        cont.hccIngredientAllergens.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    hccIngredientAllergen t = cont.hccIngredientAllergens
                        .Where(a => a.AllergenID == this.AllergenID && a.IngredientID == this.IngredientID)
                        .SingleOrDefault();

                      if (t != null)
                      {
                          cont.hccIngredientAllergens.DeleteObject(t);
                          cont.SaveChanges();
                      }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<hccIngredientAllergen> GetBy(int ingredientId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccIngredientAllergens
                        .Where(a => a.IngredientID == ingredientId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccAllergen> GetAllergensBy(int ingredientId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccIngredientAllergens
                        .Where(a => a.IngredientID == ingredientId)
                        .Join(cont.hccAllergens, j1 => j1.AllergenID, j2 => j2.AllergenID, (j1, j2) => j2)
                        .OrderBy(a => a.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static void RemoveAllergensBy(int ingredientId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var t = cont.hccIngredientAllergens
                        .Where(a => a.IngredientID == ingredientId).ToList();

                    t.ForEach(a => a.Delete());
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
