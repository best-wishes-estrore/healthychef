using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCartItemMealPreference
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartItemMealPreferences", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartItemMealPreferences", this);
                    }
                    else
                    {
                        cont.hccCartItemMealPreferences.AddObject(this);
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
                    var d = cont.hccCartItemMealPreferences
                                .SingleOrDefault(a => a.CartItemID == this.CartItemID && a.PreferenceID == this.PreferenceID);

                    if (d != null)
                    {
                        cont.hccCartItemMealPreferences.DeleteObject(d);
                        cont.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccCartItemMealPreference> GetBy(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemMealPreferences
                        .Where(a => a.CartItemID == cartItemId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccPreference> GetPrefsBy(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemMealPreferences
                        .Where(a => a.CartItemID == cartItemId)
                        .Join(cont.hccPreferences, j1 => j1.PreferenceID, j2 => j2.PreferenceID, (j1, j2) => j2)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCartItemMealPreference GetBy(int cartItemId, int preferenceId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemMealPreferences
                        .Where(a => a.CartItemID == cartItemId
                            && a.PreferenceID == preferenceId)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
