using HealthyChefWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class FutureCalender
    {
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public string DeliveryDate { get; set; }
        public string OrderCutOffDate { get; set; }
        public string Menu { get; set; }
    }
    public class PastCalender
    {
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public string DeliveryDate { get; set; }
        public string OrderCutOffDate { get; set; }
        public string Menu { get; set; }
    }
    public class ProductionCalendar
    {
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderCutOffDate { get; set; }
        public int MenuId { get; set; }
        public string Description { get; set; }
    }

    public class ProductionCalenderResult: PostHttpResponse
    {
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderCutOffDate { get; set; }
        public int MenuId { get; set; }
        public string Description { get; set; }
    }
}