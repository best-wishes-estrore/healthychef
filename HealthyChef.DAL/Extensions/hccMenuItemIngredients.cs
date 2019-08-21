using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccMenuItemIngredient
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccMenuItemIngredients", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccMenuItemIngredients", this);
                    }
                    else
                    {
                        cont.hccMenuItemIngredients.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    EntityKey key = cont.CreateEntityKey(this.EntityKey.EntitySetName, this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        cont.hccMenuItemIngredients.DeleteObject((hccMenuItemIngredient)originalItem);
                        cont.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccMenuItemIngredient GetBy(int ingredientId, int menuItemId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    hccMenuItemIngredient ingrd = cont.hccMenuItemIngredients
                        .Where(a => a.IngredientID == ingredientId && a.MenuItemID == menuItemId)
                        .SingleOrDefault();

                    return ingrd;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    }
}
