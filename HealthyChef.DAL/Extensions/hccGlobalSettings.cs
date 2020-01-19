using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccGlobalSetting
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccGlobalSettings", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccGlobalSettings", this);
                    }
                    else
                    {
                        cont.hccGlobalSettings.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public static hccGlobalSetting GetSettings()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var gs = cont.hccGlobalSettings.FirstOrDefault();

                    if (gs == null)
                    {
                        gs = new hccGlobalSetting();
                        gs.Save();
                    }

                    return gs;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
