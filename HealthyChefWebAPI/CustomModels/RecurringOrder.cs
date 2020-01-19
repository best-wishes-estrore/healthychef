﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class RecurringOrder
    {
        public int cartid { get; set; }
        public int cartitemid { get; set; }
        public int userprofileid { get; set; }
        public Guid? aspnetuserid { get; set; }
        public String ProfileName { get; set; }
        public string CustomerName { get; set; }
        public string ItemName { get; set; }
        public int purchasenumber { get; set; }
        public string maxdeliverydate {get;set;}
        public string maxcutoffdate { get; set; }
		public int quantity { get; set; }
        public bool Recurringready { get; set; }
        public DateTime MinCutOffDate { get; set; }
        public DateTime? MaxCutOffDate
        {
            get
            {
                DateTime? _maxCutOffDate = null;
                if(!string.IsNullOrEmpty(maxcutoffdate))
                {
                    _maxCutOffDate = Convert.ToDateTime(this.maxcutoffdate);
                }
                return _maxCutOffDate;
            }
        }

    }
}