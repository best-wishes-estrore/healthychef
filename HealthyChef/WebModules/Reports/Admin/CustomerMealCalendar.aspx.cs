using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class CustomerMealCalendar : System.Web.UI.Page
    {
        CustomerMenu cM = new CustomerMenu();
        List<CustomerMenu.Day> days = new List<CustomerMenu.Day>();
        List<MOTCartItem> lstMCI = new List<MOTCartItem>();
        List<hccProgramDefaultMenu> defaultMenuSelections = new List<hccProgramDefaultMenu>();

        protected void Page_Init(object sender, EventArgs e)
        {
            lvCustomerMealReport.ItemDataBound += lvCustomerMealReport_ItemDataBound;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack && !string.IsNullOrEmpty(ddlCustomers.SelectedValue)) { }
        }

        protected void lvCustomerMealReport_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem) return;

            Label DayNumberLabel = (Label)e.Item.FindControl("DayNumberLabel");
            Label BreakfastLabel = (Label)e.Item.FindControl("BreakfastLabel");
            Label SnackLabel = (Label)e.Item.FindControl("SnackLabel");
            Label LunchLabel = (Label)e.Item.FindControl("LunchLabel");
            Label SnackLabel1 = (Label)e.Item.FindControl("SnackLabel1");
            Label DinnerLabel = (Label)e.Item.FindControl("DinnerLabel");
            Label DessertLabel = (Label)e.Item.FindControl("DessertLabel");

            try
            {
                DayNumberLabel.Text = ((CustomerMenu.Day)e.Item.DataItem).DayTitle;
            }
            catch { }

            foreach (CustomerMenu.MealTypes mt in ((CustomerMenu.Day)e.Item.DataItem).MealTypes)
            {
                if (mt.MealType.Contains("breakfast"))
                    BreakfastLabel.Text = mt.MealInfo;
                else if (mt.MealType.Contains("snack"))
                    SnackLabel.Text = mt.MealInfo;
                else if (mt.MealType.Contains("lunch"))
                    LunchLabel.Text = mt.MealInfo;
                else if (mt.MealType.Contains("snack"))
                    SnackLabel1.Text = mt.MealInfo;
                else if (mt.MealType.Contains("dinner"))
                    DinnerLabel.Text = mt.MealInfo;
                else if (mt.MealType.Contains("dessert"))
                    DessertLabel.Text = mt.MealInfo;
            }
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            hccProductionCalendar cal = hccProductionCalendar.GetBy(DateTime.Parse(txtStartDate.Text));

            if (cal != null)
                cM.CurrentCalendarId = cal.CalendarID;

            cM.CurrentCartItem = hccCartItem.GetById(int.Parse(ddlCustomers.SelectedItem.Value));

            if (cM.CurrentCartItem.Plan_PlanID != null)
            {
                hccProgramPlan plan = hccProgramPlan.GetById(cM.CurrentCartItem.Plan_PlanID.Value);
                hccProgram program = hccProgram.GetById(plan.ProgramID);

                if (plan != null && program != null)
                {
                    // load user profile data
                    hccUserProfile profile = cM.CurrentCartItem.UserProfile;
                    hccUserProfile parent = hccUserProfile.GetParentProfileBy(profile.MembershipID);

                    lblOrderData.Text = string.Format("Order #{0}: {1}/{2}",
                        cM.CurrentCartItem.OrderNumber, program.Name, plan.Name);
                    lblCustData.Text = string.Format("For: {0}, {1} ({2})  Delivery Date: {3}",
                        parent.LastName, parent.FirstName, profile.ProfileName, txtStartDate.Text); //cM.CurrentCartItem.DeliveryDate.ToShortDateString()

                    defaultMenuSelections = hccProgramDefaultMenu.GetBy(cM.CurrentCalendarId, program.ProgramID);

                    days = cM.BindWeeklyGlance(defaultMenuSelections, plan.NumDaysPerWeek, int.Parse(ddlCustomers.SelectedValue));
                    lvCustomerMealReport.DataSource = days;
                    lvCustomerMealReport.DataBind();
                }
            }

        }

        protected void btnLoadCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                ddlCustomers.Items.Clear();

                lstMCI = hccCartItem.Search_MealOrderTicketForWaaG(DateTime.Parse(txtStartDate.Text), (DateTime.Parse(txtStartDate.Text)));
                List<CustomerCalendarMenu> lstCMC = new List<CustomerCalendarMenu>();

                var cccValue = new CustomerCalendarMenu();

                var currentCustCalendarValues =
                    (from c in lstMCI
                     where c.CartItem.Plan_PlanID != null
                     orderby c.CustomerName
                     select c.CustomerName + "|" + c.CartItemId + "|" + c.OrderNumber + "|" + c.CartItem.Plan_PlanID).Distinct();

                foreach (string c in currentCustCalendarValues)
                {
                    hccProgramPlan plan = hccProgramPlan.GetById(int.Parse(c.Split('|')[3]));
                    hccProgram prog = hccProgram.GetById(plan.ProgramID);

                    ListItem li = new ListItem(c.Split('|')[0] + " order # " + c.Split('|')[2] + " " + prog.Name, c.Split('|')[1]);
                    ddlCustomers.Items.Add(li);
                }
            }
            catch { }
        }
    }
    public class CustomerCalendarMenu
    {
        public string customerName { get; set; }
        public int cartItemId { get; set; }
    }
}