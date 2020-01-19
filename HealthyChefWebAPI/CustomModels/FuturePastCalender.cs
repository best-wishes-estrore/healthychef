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
        public DateTime DeliveryDateObj { get; set; }
    }
    public class PastCalender
    {
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public string DeliveryDate { get; set; }
        public string OrderCutOffDate { get; set; }
        public string Menu { get; set; }
        public DateTime DeliveryDateObj { get; set; }
    }
    public class ProductionCalendar
    {
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderCutOffDate { get; set; }
        public int MenuId { get; set; }
        public string Description { get; set; }
        public string DeliveryDatestring { get; set; }
        public string OrderCutOffDatestring { get; set; }
    }

    public class ProductionCalenderResult : PostHttpResponse
    {
        public int CalendarId { get; set; }
        public string Name { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime OrderCutOffDate { get; set; }
        public int MenuId { get; set; }
        public string Description { get; set; }

        public ProductionCalenderResult()
        {

        }

        public ProductionCalenderResult(ProductionCalendar calendar)
        {


            if (string.IsNullOrEmpty(calendar.Name))
            {
                this.AddValidationError("Calendar Name is Required");
                this.isValid = false;
            }

            if (calendar.MenuId == -1)
            {
                this.AddValidationError("A menu is Required");
                this.isValid = false;
            }
            if (string.IsNullOrEmpty(calendar.DeliveryDatestring))
            {
                this.AddValidationError("Select a Delivery Date");
                this.isValid = false;
            }
            else
            {
                //var _d = new DateTime().ToLongDateString();
                var activetime = DateTime.MinValue;
                if (Convert.ToDateTime(calendar.DeliveryDatestring) == activetime)
                //if (DateTime.TryParse(calendar.DeliveryDatestring, out _d))                    
                {

                    this.AddValidationError("Select a Delivery Date");
                    this.isValid = false;
                }

            }
            if (string.IsNullOrEmpty(calendar.OrderCutOffDatestring))
            {
                this.AddValidationError("Select a Cut-Off Date");
                this.isValid = false;
            }
            else
            {
                //var _d = new DateTime();
                var activetime = DateTime.MinValue;
                if (Convert.ToDateTime(calendar.OrderCutOffDatestring) == activetime)
                //if (DateTime.TryParse(calendar.OrderCutOffDatestring, out _d))
                {
                    this.AddValidationError("Select a Cut-Off Date");
                    this.isValid = false;
                }

            }
            //if (string.IsNullOrEmpty(calendar.Description))
            //{
            //    this.AddValidationError("calendar Description is Required");
            //    this.isValid = false;
            //}
        }
    }
}