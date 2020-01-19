using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccPreference : EntityObject
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccPreferences", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccPreferences", this);
                    }
                    else
                    {
                        cont.hccPreferences.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
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

        public static List<hccPreference> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPreferences.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccPreference> GetBy(HealthyChef.Common.Enums.PreferenceType prefType, bool? isRetired)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var p = cont.hccPreferences
                        .Where(a => a.PreferenceType == (int)prefType)
                        .OrderBy(b => b.Name)
                        .ToList();

                    if (isRetired.HasValue)
                        p = p.Where(a => a.IsRetired == isRetired.Value).ToList();

                    return p;
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccPreference GetBy(int prefType, string preferenceName)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var p = cont.hccPreferences
                        .Where(a => a.PreferenceType == prefType
                            && a.Name.Trim().ToLower() == preferenceName.Trim().ToLower())
                        .SingleOrDefault();

                    return p;
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccPreference GetById(int preferenceId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPreferences
                        .SingleOrDefault(a => a.PreferenceID == preferenceId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccPreference> GetActive()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPreferences
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

        public static List<hccPreference> GetActive(int prefTypeId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPreferences
                        .Where(a => !a.IsRetired && a.PreferenceType == prefTypeId)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccPreference> GetRetired()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPreferences
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

        public static List<hccPreference> GetRetired(int prefTypeId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPreferences
                        .Where(a => a.IsRetired && a.PreferenceType == prefTypeId)
                        .OrderBy(b => b.Name)
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
