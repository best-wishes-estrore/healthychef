using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class CalendarMenuItemSelectionControl : System.Web.UI.UserControl
    {
        public event DateChangedEventHandler DateChanged;

        protected void OnDateChanged(DateChangedEventArgs args)
        {
            if (DateChanged != null)
            {
                DateChanged(this, args);
            }
        }

        protected string PagePostBackScript { get; private set; }

        protected override void OnInit(EventArgs e)
        {
            PagePostBackScript = Page.ClientScript.GetPostBackEventReference(SelectedCalendarDate, "date_changed", false);
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            DateTime selectedDate;

            if (Page.IsPostBack
                && !string.IsNullOrWhiteSpace(Request["__EVENTTARGET"])
                && !string.IsNullOrWhiteSpace(Request["__EVENTARGUMENT"])
                && Request["__EVENTARGUMENT"] == "date_changed"
                && !string.IsNullOrWhiteSpace(SelectedCalendarDate.Value)
                && DateTime.TryParse(SelectedCalendarDate.Value, out selectedDate)
            )
            {
                OnDateChanged(new DateChangedEventArgs() { PreviousDate = null, NewDate = selectedDate });
            }
        }
    }
}