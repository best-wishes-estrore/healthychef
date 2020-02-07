using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using Microsoft.Reporting.WebForms;


namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class WeeklyMenuReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hccProductionCalendar nextProdCal = hccProductionCalendar.GetNext4Calendars().FirstOrDefault();

                txtStartDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
                txtEndDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
                ButtonRefresh_Click(this, new EventArgs());
            }
        }

        protected void ButtonRefresh_Click(object sender, EventArgs e)
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

                List<hcc_WeeklyMenuReport_Result> list = hccMenuItem.GetWeeklyMenuReport(searchStart.Value, searchEnd.Value);

                try
                {
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/WeeklyMenu1.rdlc");

                    ReportDataSource rds1 = new ReportDataSource("WeeklyMenu1", list);
                    ReportViewer1.LocalReport.DataSources.Add(rds1);

                    this.ReportViewer1.LocalReport.Refresh();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }      
        
    }
}