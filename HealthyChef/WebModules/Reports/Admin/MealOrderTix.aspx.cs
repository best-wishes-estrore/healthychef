using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using Microsoft.Reporting.WebForms;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class MealOrderTix : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ReportViewer1.ReportRefresh += ReportViewer1_ReportRefresh;
            ReportViewer1.ReportError += ReportViewer1_ReportError;
        }

        void ReportViewer1_ReportError(object sender, ReportErrorEventArgs e)
        {
            lblFeedback.Text = e.Exception.Message + e.Exception.StackTrace;

            if (e.Exception.InnerException != null)
            {
                lblFeedback.Text += e.Exception.InnerException.Message + e.Exception.InnerException.StackTrace;

                if (e.Exception.InnerException.InnerException != null)
                {
                    lblFeedback.Text += e.Exception.InnerException.InnerException.Message + e.Exception.InnerException.InnerException.StackTrace;
                }
            }
        }

        protected void ReportViewer1_ReportRefresh(object sender, EventArgs e)
        {
            BindReport();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hccProductionCalendar nextProdCal = hccProductionCalendar.GetNext4Calendars().FirstOrDefault();

                txtStartDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
                txtEndDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
                BindddlMealTypes();
            }
        }

        void BindddlMealTypes()
        {
            if (ddlMealTypes.Items.Count == 0)
            {
                ddlMealTypes.DataSource = Enums.GetEnumAsTupleList(typeof(Enums.MealTypes));
                ddlMealTypes.DataTextField = "Item1";
                ddlMealTypes.DataValueField = "Item2";
                ddlMealTypes.DataBind();

                if (ddlMealTypes.Items.FindByText("Unknown") != null)
                    ddlMealTypes.Items.Remove(ddlMealTypes.Items.FindByText("Unknown"));

                for (var i = ddlMealTypes.Items.Count - 1; i >= 0; i--)
                {
                    if (!hccMenuItem.SideMealTypes.Contains((Enums.MealTypes)int.Parse(ddlMealTypes.Items[i].Value)))
                        continue;
                    ddlMealTypes.Items.RemoveAt(i);
                }
            }
        }

        void BindReport()
        {
            Page.Validate("MealOrderGroup");

            if (Page.IsValid)
            {
                DateTime startDate;
                bool tryStart = DateTime.TryParse(txtStartDate.Text.Trim(), out startDate);
                DateTime endDate;
                bool tryEnd = DateTime.TryParse(txtEndDate.Text.Trim(), out endDate);

                DateTime? searchStart = null;
                DateTime? searchEnd = null;

                if (tryStart)
                    searchStart = startDate;

                if (tryEnd)
                    searchEnd = endDate.AddDays(1);

                List<MOTCartItem> motItems = hccCartItem.Search_MealOrderTicketForMOT(searchStart, searchEnd)
                    .Where(a => a.MealType == ((Enums.MealTypes)Enum.Parse(typeof(Enums.MealTypes), ddlMealTypes.SelectedValue)))
                    .OrderBy(a => a.CartItem.ItemType).ThenBy(a => a.ItemName).ThenBy(a => a.CustomerName).ToList();

                Tuple<List<MOTCartItem>, List<MOTCartItem>> sortItems = MOTCartItem.SortOddEven(motItems);

                ReportViewer1.LocalReport.DataSources.Clear();

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/MealOrderTickets1.rdlc");
                ReportDataSource rdsO = new ReportDataSource("MOT7Odds", sortItems.Item1);
                ReportViewer1.LocalReport.DataSources.Add(rdsO);
                ReportDataSource rdsE = new ReportDataSource("MOT7Evens", sortItems.Item2);
                ReportViewer1.LocalReport.DataSources.Add(rdsE);
                this.ReportViewer1.LocalReport.Refresh();
            }
        }
    }
}