using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCartDefaultMenuException
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartDefaultMenuExceptions", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartDefaultMenuExceptions", this);
                    }
                    else
                    {
                        cont.hccCartDefaultMenuExceptions.AddObject(this);
                    }

                    cont.SaveChanges();
                    //cont.Refresh(RefreshMode.StoreWins, this);
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
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartDefaultMenuExceptions", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                         hccCartDefaultMenuException item = cont.hccCartDefaultMenuExceptions
                            .Where(a => a.DefaultMenuExceptID == this.DefaultMenuExceptID)
                            .SingleOrDefault();

                         if (item != null)
                         {
                             cont.hccCartDefaultMenuExceptions.DeleteObject(item);
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

        public static hccCartDefaultMenuException GetById(int defaultMenuExId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartDefaultMenuExceptions
                        .Where(a => a.DefaultMenuExceptID == defaultMenuExId)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCartDefaultMenuException> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartDefaultMenuExceptions.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCartDefaultMenuException> GetBy(int cartCalendarId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartDefaultMenuExceptions
                        .Where(a => a.CartCalendarID == cartCalendarId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCartDefaultMenuException GetBy(int defaultMenuID, int cartCalendarId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartDefaultMenuExceptions
                        .Where(a => a.DefaultMenuID == defaultMenuID && a.CartCalendarID == cartCalendarId)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<hccAllergen> GetMenuItemAllergens()
        {
            try
            {


                List<hccAllergen> algns = new List<hccAllergen>();
                hccMenuItem item = hccMenuItem.GetById(this.MenuItemID);

                if (item != null)
                    algns.AddRange(item.GetAllergens());

                return algns;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
