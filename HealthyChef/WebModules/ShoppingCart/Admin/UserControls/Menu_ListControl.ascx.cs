using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class Menu_ListControl : System.Web.UI.UserControl
    {
        private DateTime _SelectedCalendarDate;

        public DateTime SelectedCalendarDate
        {
            get { return _SelectedCalendarDate; }
            set
            {
                if (!_SelectedCalendarDate.Date.Equals(value.Date))
                {
                    _SelectedCalendarDate = value;
                    BindMenus(value);
                }
            }
        }

        public string SelectedValue
        {
            get
            {
                return radiobuttonlist1.SelectedValue;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BindMenus(DateTime date)
        {
            //BindMenus(radiobuttonlist1, hccProductionCalendar.GetBy(date.Date));
        }

        protected void BindMenus(ListControl control, List<hccMenu> menus)
        {
            if (menus.Count == 0)
            {
                norecordsplaceholder.Visible = true;
                radiobuttonlist1.Visible = false;

                return;
            }

            norecordsplaceholder.Visible = false;
            radiobuttonlist1.Visible = true;

            control.Items.Clear();
            control.DataTextField = "Name";
            control.DataValueField= "MenuID";

            control.DataSource = menus;
            control.DataBind();
        }
    }
}