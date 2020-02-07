using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using Microsoft.Reporting.WebForms;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class SalesReport : System.Web.UI.Page
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

        void ReportViewer1_ReportRefresh(object sender, System.ComponentModel.CancelEventArgs e)
        {
            BindReport();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
                return;
            hccProductionCalendar productionCalendar = ((IEnumerable<hccProductionCalendar>)hccProductionCalendar.GetNext4Calendars()).FirstOrDefault<hccProductionCalendar>();
            TextBox txtStartDate = this.txtStartDate;
            DateTime dateTime;
            string shortDateString1;
            if (productionCalendar != null)
            {
                shortDateString1 = productionCalendar.DeliveryDate.ToShortDateString();
            }
            else
            {
                dateTime = DateTime.Now;
                shortDateString1 = dateTime.ToShortDateString();
            }
            txtStartDate.Text = shortDateString1;
            TextBox txtEndDate = this.txtEndDate;
            string shortDateString2;
            if (productionCalendar != null)
            {
                dateTime = productionCalendar.DeliveryDate;
                shortDateString2 = dateTime.ToShortDateString();
            }
            else
            {
                dateTime = DateTime.Now;
                shortDateString2 = dateTime.ToShortDateString();
            }
            txtEndDate.Text = shortDateString2;
            this.BindReport();
        }

        private void BindReport()
        {
            this.Page.Validate("MealOrderGroup");
            if (!this.Page.IsValid)
                return;
            DateTime result1;
            bool flag1 = DateTime.TryParse(this.txtStartDate.Text.Trim(), out result1);
            DateTime result2;
            bool flag2 = DateTime.TryParse(this.txtEndDate.Text.Trim(), out result2);
            DateTime? nullable1 = new DateTime?();
            DateTime? nullable2 = new DateTime?();
            if (flag1)
                nullable1 = new DateTime?(result1);
            if (flag2)
                nullable2 = new DateTime?(result2.AddDays(1.0));
            List<hcc_SalesReportCarts_Result> salesReportCarts = hccCart.GetSalesReportCarts(nullable1.Value, nullable2.Value);
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.ReportPath = this.Server.MapPath("~/WebModules/Reports/SalesReport1.rdlc");
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                //this.ReportViewer1.LocalReport.LoadSubreportDefinition("SalesReport2", Assembly.GetCallingAssembly().GetManifestResourceStream("HealthyChef.WebModules.Reports.SalesReport2.rdlc"));
                this.ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("SalesReport1", (IEnumerable)salesReportCarts));
                this.ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                List<hcc_SalesReportCartItems_Result> taxable = hccCartItem.GetTaxable(int.Parse(e.Parameters[0].Values[0]));
                e.DataSources.Add(new ReportDataSource("SalesReport2", (IEnumerable)taxable));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}