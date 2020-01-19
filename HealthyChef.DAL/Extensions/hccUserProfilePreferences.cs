using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccUserProfilePreference
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccUserProfilePreferences", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccUserProfilePreferences", this);
                    }
                    else
                    {
                        cont.hccUserProfilePreferences.AddObject(this);
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
                    hccUserProfilePreference t = cont.hccUserProfilePreferences
                        .Where(a => a.PreferenceID == this.PreferenceID && a.UserProfileID == this.UserProfileID)
                        .SingleOrDefault();

                    if (t != null)
                    {
                        cont.hccUserProfilePreferences.DeleteObject(t);
                        cont.SaveChanges();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<hccUserProfilePreference> GetBy(int userProfileId, bool isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfilePreferences
                        .Where(a => a.IsActive == isActive)
                        .Where(a => a.UserProfileID == userProfileId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccPreference> GetPrefsBy(int userProfileId, bool isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfilePreferences
                        .Where(a => a.IsActive == isActive && a.UserProfileID == userProfileId)
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
