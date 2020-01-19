using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCartDefaultMenuExPref
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartDefaultMenuExPrefs", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartDefaultMenuExPrefs", this);
                    }
                    else
                    {
                        cont.hccCartDefaultMenuExPrefs.AddObject(this);
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartDefaultMenuExPrefs", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        hccCartDefaultMenuExPref item = cont.hccCartDefaultMenuExPrefs
                            .Where(a => a.MenuExPrefID == this.MenuExPrefID).SingleOrDefault();

                        if (item != null)
                        {
                            cont.hccCartDefaultMenuExPrefs.DeleteObject(item);
                            cont.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccCartDefaultMenuExPref GetBy(int defaultMenuExceptID, int preferenceId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartDefaultMenuExPrefs
                        .Where(a => a.DefaultMenuExceptID == defaultMenuExceptID
                            && a.PreferenceID == preferenceId)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCartDefaultMenuExPref> GetBy(int defaultMenuExceptId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartDefaultMenuExPrefs
                        .Where(a => a.DefaultMenuExceptID == defaultMenuExceptId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccPreference> GetPrefsBy(int defaultMenuExceptID)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartDefaultMenuExPrefs
                        .Where(a => a.DefaultMenuExceptID == defaultMenuExceptID)
                        .Join(cont.hccPreferences, j1 => j1.PreferenceID, j2 => j2.PreferenceID, (j1, j2) => j2)
                        .OrderBy(a => a.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
