using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
   public partial class hccCartMenuExPref
    {
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartMenuExPrefs", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartMenuExPrefs", this);
                    }
                    else
                    {
                        cont.hccCartMenuExPrefs.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static hccCartMenuExPref GetById(int cartItemId,int daynumber)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartMenuExPrefs
                        .FirstOrDefault(i => i.CartItemID == cartItemId && i.DayNumber==daynumber);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<hccCartMenuExPref> GetByCartItemId(int cartItemId, int daynumber)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartMenuExPrefs
                       .Where(i => i.CartItemID == cartItemId && i.DayNumber == daynumber).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static List<hccCartMenuExPref> GetByCartItem(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartMenuExPrefs
                       .Where(i => i.CartItemID == cartItemId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartMenuExPrefs", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        hccCartMenuExPref item = cont.hccCartMenuExPrefs
                            .Where(a => a.CartMenuExPrefID == this.CartMenuExPrefID).SingleOrDefault();

                        if (item != null)
                        {
                            cont.hccCartMenuExPrefs.DeleteObject(item);
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
    }
}
