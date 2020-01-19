using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccAllergen
    {
       //static healthychefEntities cont
       // {
       //     get { return healthychefEntities.Default; }
       // }

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccAllergens", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccAllergens", this);
                    }
                    else
                    {
                        cont.hccAllergens.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public void Retire(bool isRetired)
        {
            try
            {
                this.IsActive = !isRetired;
                this.Save();
            }
            catch
            {
                throw;
            }
        }

        public static hccAllergen GetById(int id)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccAllergens.FirstOrDefault(a => a.AllergenID == id);
                }
            }
            catch
            {
                throw;
            }
        }
        
        public static hccAllergen GetBy(string allergenName)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var p = cont.hccAllergens
                        .Where(a => a.Name.Trim().ToLower() == allergenName.Trim().ToLower())
                        .SingleOrDefault();

                    return p;
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccAllergen> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccAllergens.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccAllergen> GetActive()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccAllergens
                        .Where(a => a.IsActive)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccAllergen> GetInactive()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccAllergens.Where(a => !a.IsActive).ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
