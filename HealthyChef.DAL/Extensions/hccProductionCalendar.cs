using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccProductionCalendar
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public int Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccProductionCalendars", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccProductionCalendars", this);
                    }
                    else
                    {
                        cont.hccProductionCalendars.AddObject(this);
                    }

                    cont.SaveChanges();
                    return this.CalendarID;
                }
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is SqlException)
                {
                    try
                    {
                        if (ex.InnerException.Message.Contains("Cannot insert duplicate key row"))
                            return GetBy(this.DeliveryDate).CalendarID;
                        else
                            throw;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
                else
                    throw ex;
            }
        }

        public static List<hccProductionCalendar> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProductionCalendars
                        .OrderByDescending(a => a.DeliveryDate)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProductionCalendar GetBy(DateTime deliveryDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProductionCalendars
                        .Where(a => a.DeliveryDate == deliveryDate)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProductionCalendar> GetBy(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProductionCalendars
                        .Where(a => a.DeliveryDate > startDate && a.DeliveryDate < endDate)
                        .OrderBy(a => a.DeliveryDate)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProductionCalendar GetBy(string name)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProductionCalendars
                        .Where(a => a.Name == name)
                        .OrderByDescending(a => a.DeliveryDate)
                        .ThenByDescending(a => a.OrderCutOffDate)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProductionCalendar GetNextCalendar()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProductionCalendars
                        .Where(a => a.OrderCutOffDate >= DateTime.Today)
                        .OrderBy(a => a.DeliveryDate)
                        .FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProductionCalendar> GetNext4Calendars()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProductionCalendars
                        .Where(a => a.OrderCutOffDate >= DateTime.Today)
                        .OrderBy(a => a.DeliveryDate)
                        .Take(4)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProductionCalendar> GetNext4Last2Calendars()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var t = cont.hccProductionCalendars
                        .Where(a => a.OrderCutOffDate >= DateTime.Today)
                        .OrderBy(a => a.DeliveryDate)
                        .Take(4)
                        .ToList();

                    var u = cont.hccProductionCalendars
                       .Where(a => a.OrderCutOffDate < DateTime.Today)
                       .OrderByDescending(a => a.DeliveryDate)
                       .Take(2)
                       .ToList();

                    return t.Union(u).OrderBy(a => a.DeliveryDate).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProductionCalendar GetById(int calendarId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProductionCalendars
                .SingleOrDefault(a => a.CalendarID == calendarId);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public hccMenu GetMenu()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccMenus
                        .Where(a => a.MenuID == this.MenuID)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
