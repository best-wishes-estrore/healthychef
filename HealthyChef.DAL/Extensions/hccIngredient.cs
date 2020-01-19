using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccIngredient
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccIngredients", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccIngredients", this);
                    }
                    else
                    {
                        cont.hccIngredients.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public void Retire(bool isRetired)
        {
            try
            {
                this.IsRetired = isRetired;
                this.Save();
            }
            catch
            {
                throw;
            }
        }

        public static List<hccIngredient> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccIngredients
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccIngredient GetBy(string ingredientName)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var p = cont.hccIngredients
                        .Where(a => a.Name.Trim().ToLower() == ingredientName.Trim().ToLower())
                        .SingleOrDefault();

                    return p;
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccIngredient> GetActive()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccIngredients
                        .Where(a => !a.IsRetired)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccIngredient> GetRetired()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccIngredients
                        .Where(a => a.IsRetired)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccIngredient GetById(int IngredientId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccIngredients
                        .SingleOrDefault(a => a.IngredientID == IngredientId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccIngredient> GetBy(int allergenID)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccIngredientAllergens
                        .Where(c => c.AllergenID == allergenID)
                        .Join(cont.hccIngredients, a => a.IngredientID, b => b.IngredientID, (a, b) => b)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<hccAllergen> GetAllergens()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return hccIngredientAllergen.GetAllergensBy(this.IngredientID);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
