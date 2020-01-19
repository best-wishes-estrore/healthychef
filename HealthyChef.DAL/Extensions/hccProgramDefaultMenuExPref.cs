using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccProgramDefaultMenuExPref
    {
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccProgramDefaultMenuExPrefs", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccProgramDefaultMenuExPrefs", this);
                    }
                    else
                    {
                        cont.hccProgramDefaultMenuExPrefs.AddObject(this);
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccProgramDefaultMenuExPrefs", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        List<hccProgramDefaultMenuExPref> items = cont.hccProgramDefaultMenuExPrefs
                            .Where(a => a.DefaultMenuId == this.DefaultMenuId).ToList();

                        if (items.Count > 0)
                        {
                            foreach (var item in items)
                            {
                                cont.hccProgramDefaultMenuExPrefs.DeleteObject(item);
                                cont.SaveChanges();
                            }
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccProgramDefaultMenuExPref GetBy(int DefaultMenuId, int preferenceId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenuExPrefs
                        .Where(a => a.DefaultMenuId == DefaultMenuId
                            && a.PreferenceId == preferenceId)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }


        public static List<hccProgramDefaultMenuExPref> GetBy(int defaultMenuId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenuExPrefs
                        .Where(a => a.DefaultMenuId == defaultMenuId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProgramDefaultMenu GetDefaultMenuItemIdByMenuItemId(int MenuitemID)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenus
                        .Where(a => a.MenuItemID == MenuitemID)
                        .OrderBy(a => a.DefaultMenuID)
                        .FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
