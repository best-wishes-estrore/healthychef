using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccUserProfileAllergen
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccUserProfileAllergens", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccUserProfileAllergens", this);
                    }
                    else
                    {
                        cont.hccUserProfileAllergens.AddObject(this);
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
                    hccUserProfileAllergen t = cont.hccUserProfileAllergens
                        .Where(a => a.AllergenID == this.AllergenID && a.UserProfileID == this.UserProfileID)
                        .SingleOrDefault();

                    if (t != null)
                    {
                        cont.hccUserProfileAllergens.DeleteObject(t);
                        cont.SaveChanges();
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<hccUserProfileAllergen> GetBy(int userProfileId, bool isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfileAllergens
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

        public static List<hccAllergen> GetAllergensBy(int userProfileId, bool isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfileAllergens
                        .Where(a => a.IsActive == isActive && a.UserProfileID == userProfileId)
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
    }
}
