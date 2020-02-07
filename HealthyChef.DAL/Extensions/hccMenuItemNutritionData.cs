using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccMenuItemNutritionData
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
                    EntityKey key = cont.CreateEntityKey("hccMenuItemNutritionDatas", this);

                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        cont.ApplyCurrentValues(key.EntitySetName, this);
                    }
                    else
                    {
                        cont.hccMenuItemNutritionDatas.AddObject(this);
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
                        cont.hccMenuItemNutritionDatas.DeleteObject((hccMenuItemNutritionData)originalItem);
                        cont.SaveChanges();

                       // cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, cont.hccMenuItemNutritionDatas);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccMenuItemNutritionData GetBy(int menuItemId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    var prefs = cont.hccMenuItemNutritionDatas
                        .Where(a => a.MenuItemID == menuItemId)
                        .SingleOrDefault();

                    return prefs;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       
    }
}
