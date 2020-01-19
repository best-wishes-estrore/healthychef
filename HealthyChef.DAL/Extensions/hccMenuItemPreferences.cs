using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccMenuItemPreference
    {
        public hccPreference Preference { get { return hccPreference.GetById(this.PreferenceID); } }
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
                    EntityKey key = cont.CreateEntityKey("hccMenuItemPreferences", this);

                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        cont.ApplyCurrentValues(key.EntitySetName, this);
                    }
                    else
                    {
                        cont.hccMenuItemPreferences.AddObject(this);
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
                using (var cont = new healthychefEntitiesAPI())
                {
                    EntityKey key = cont.CreateEntityKey(this.EntityKey.EntitySetName, this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        cont.hccMenuItemPreferences.DeleteObject((hccMenuItemPreference)originalItem);
                        cont.SaveChanges();

                        //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, cont.hccMenuItemPreferences);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccMenuItemPreference> GetBy(int menuItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var prefs = cont.hccMenuItemPreferences
                        .Where(a => a.MenuItemID == menuItemId)
                        .ToList();

                    return prefs;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccMenuItemPreference GetBy(int preferenceId, int menuItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    hccMenuItemPreference pref = cont.hccMenuItemPreferences
                        .Where(a => a.PreferenceID == preferenceId && a.MenuItemID == menuItemId)
                        .SingleOrDefault();

                    return pref;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
