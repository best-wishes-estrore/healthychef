using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Web.Security;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCartItemCalendar
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartItemCalendars", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartItemCalendars", this);
                    }
                    else
                    {
                        cont.hccCartItemCalendars.AddObject(this);
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
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartItemCalendars", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        hccCartItemCalendar item = cont.hccCartItemCalendars
                            .Where(a => a.CartCalendarID == this.CartCalendarID).SingleOrDefault();

                        if (item != null)
                        {
                            cont.hccCartItemCalendars.DeleteObject(item);
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

        public static hccCartItemCalendar GetById(int cartCalendarId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemCalendars
                        .SingleOrDefault(i => i.CartCalendarID == cartCalendarId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCartItemCalendar GetBy(int cartItemId, int calendarId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemCalendars
                        .SingleOrDefault(i => i.CartItemID == cartItemId
                            && i.CalendarID == calendarId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCartItemCalendar GetBy(int cartItemId, DateTime deliveryDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemCalendars
                        .Join(cont.hccProductionCalendars, a => a.CalendarID, b => b.CalendarID, (a, b) => new { a, b })
                        .Where(i => i.a.CartItemID == cartItemId && i.b.DeliveryDate == deliveryDate)
                        .Select(a => a.a)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCartItemCalendar> GetByCartItemID(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemCalendars
                        .Join(cont.hccProductionCalendars, a => a.CalendarID, b => b.CalendarID, (a, b) => new { CartCal = a, ProdCal = b })
                        .Where(a => a.CartCal.CartItemID == cartItemId)
                        .OrderBy(a => a.ProdCal.DeliveryDate)
                        .Select(a => a.CartCal)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCartItemCalendar> GetByCalendarID(int calendarId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemCalendars
                        .Join(cont.hccProductionCalendars, a => a.CalendarID, b => b.CalendarID, (a, b) => new { CartCal = a, ProdCal = b })
                        .Where(i => i.CartCal.CalendarID == calendarId)
                        .OrderBy(o => o.ProdCal.DeliveryDate)
                        .Select(a => a.CartCal)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCartItemCalendar> GetBy(DateTime deliveryDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItemCalendars
                        .Join(cont.hccProductionCalendars, a => a.CalendarID, b => b.CalendarID, (a, b) => new { CartCal = a, ProdCal = b })
                        .Where(i => i.ProdCal.DeliveryDate == deliveryDate)
                        .Select(a => a.CartCal).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public hccCartItemCalendar GetNextCartCalendar(DayOfWeek requiredDayOfWeek)
        {
            try
            {
                hccProductionCalendar prodCal = hccProductionCalendar.GetById(this.CalendarID);
                DateTime nextDeliveryDate = prodCal.DeliveryDate.AddDays(7);
                hccProductionCalendar nextProdCal = hccProductionCalendar.GetBy(nextDeliveryDate);
                int calendarId = 0;

                if (nextProdCal == null)
                {
                    nextProdCal = new DAL.hccProductionCalendar
                    {
                        DeliveryDate = nextDeliveryDate,
                        Name = "Delivery Date " + nextDeliveryDate.ToShortDateString(),
                        OrderCutOffDate = nextDeliveryDate.AddDays(-8)
                    };
                    calendarId = nextProdCal.Save();
                }
                else
                    calendarId = nextProdCal.CalendarID;

                var nextCartCalendar = hccCartItemCalendar.GetBy(this.CartItemID, calendarId);

                if (nextCartCalendar == null)
                {
                    nextCartCalendar = new hccCartItemCalendar { CalendarID = calendarId, CartItemID = this.CartItemID };
                    nextCartCalendar.Save();
                    return nextCartCalendar;
                }
                else
                {
                    throw new Exception("hccCartItemCalendar - nextCartCalendar was expected to be null");
                }

            }
            catch
            {
                throw;
            }
        }

    }
}
