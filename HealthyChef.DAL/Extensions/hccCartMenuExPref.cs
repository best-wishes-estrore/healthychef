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
                using (var cont = new healthychefEntitiesAPI())
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
                using (var cont = new healthychefEntitiesAPI())
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

    }
}
