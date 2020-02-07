using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccDeliverySetting
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccDeliverySettings", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccDeliverySettings", this);
                    }
                    else
                    {
                        cont.hccDeliverySettings.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public static List<hccDeliverySetting> GetAll()
        {
            using (var cont = new healthychefEntities())
            {
                return cont.hccDeliverySettings.ToList();
            }
        }

        public static hccDeliverySetting GetBy(Enums.MealTypes mealType)
        {
            using (var cont = new healthychefEntities())
            {
                return cont.hccDeliverySettings
                    .Where(a => a.MealTypeID == (int)mealType)
                    .SingleOrDefault();
            }
        }
    }
}
