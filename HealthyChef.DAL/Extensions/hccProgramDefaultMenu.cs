using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccProgramDefaultMenu
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccProgramDefaultMenus", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccProgramDefaultMenus", this);
                    }
                    else
                    {
                        cont.hccProgramDefaultMenus.AddObject(this);
                    }

                    cont.SaveChanges();
                    //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, this);
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccProgramDefaultMenus", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.hccProgramDefaultMenus.DeleteObject((hccProgramDefaultMenu)oldObj);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public static List<hccProgramDefaultMenu> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenus
                        .OrderBy(a => a.CalendarID)
                        .ThenBy(a => a.ProgramID)
                        .ThenBy(a => a.DayNumber)
                        .ThenBy(a => a.MealTypeID)
                        .ThenBy(a => a.Ordinal)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProgramDefaultMenu GetById(int defaultMenuID)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenus
                        .Where(a => a.DefaultMenuID == defaultMenuID)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProgramDefaultMenu> GetBy(int calendarId, int programId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenus
                        .Where(a => a.ProgramID == programId && a.CalendarID == calendarId)
                        .OrderBy(a => a.DayNumber).ThenBy(a => a.MealTypeID)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProgramDefaultMenu> GetBy(int calendarId, int programId, int numDays)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenus
                        .Where(a => a.ProgramID == programId && a.CalendarID == calendarId && a.DayNumber <= numDays)
                        .OrderBy(a => a.DayNumber).ThenBy(a => a.MealTypeID)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProgramDefaultMenu GetBy(int calendarId, int programId, int day, int mealTypeId, int ordinal)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramDefaultMenus
                        .Where(a => a.ProgramID == programId
                            && a.CalendarID == calendarId
                            && a.DayNumber == day
                            && a.MealTypeID == mealTypeId
                            && a.Ordinal == ordinal)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteDuplicates()
        {
            try
            {

          
            List<hccProgramDefaultMenu> defMenus = GetAll();
            hccProgramDefaultMenu lastDefMenu = null;
            List<int> deleteIDs = new List<int>();

            foreach (hccProgramDefaultMenu curDefMenu in defMenus)
            {
                if (!curDefMenu.IsDuplicateOf(lastDefMenu))
                    lastDefMenu = curDefMenu;
                else
                {
                    if (lastDefMenu.MenuItemID == 0 && curDefMenu.MenuItemID > 0)
                    {
                        deleteIDs.Add(lastDefMenu.DefaultMenuID);
                        lastDefMenu = curDefMenu;
                    }
                    else
                        deleteIDs.Add(curDefMenu.DefaultMenuID);
                }
            }

            foreach (int id in deleteIDs)
            {
                hccProgramDefaultMenu defMenu = hccProgramDefaultMenu.GetById(id);
                defMenu.Delete();
            }
            }
            catch (Exception)
            {

                throw;
            }
        }

        bool IsDuplicateOf(hccProgramDefaultMenu otherDefMenu)
        {
            if (otherDefMenu == null)
                return false;
            else
            {
                if (this.CalendarID == otherDefMenu.CalendarID
                    && this.ProgramID == otherDefMenu.ProgramID
                    && this.DayNumber == otherDefMenu.DayNumber
                    && this.MealTypeID == otherDefMenu.MealTypeID
                    && this.Ordinal == otherDefMenu.Ordinal)
                    return true;
                else
                    return false;
            }
        }
    }
}
