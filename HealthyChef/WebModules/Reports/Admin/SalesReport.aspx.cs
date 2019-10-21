using System;
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
            if (!IsPostBack)
            {
                hccProductionCalendar nextProdCal = hccProductionCalendar.GetNext4Calendars().FirstOrDefault();

                txtStartDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
                txtEndDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
                BindReport();
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

                var carts = hccCart.GetSalesReportCarts(searchStart.Value, searchEnd.Value);
                 
                try
                {
                    ReportViewer1.LocalReport.DataSources.Clear();                   
                    
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/SalesReport1.rdlc");
                    ReportViewer1.LocalReport.SubreportProcessing +=
                        new SubreportProcessingEventHandler(LocalReport_SubreportProcessing);
                    
                    var resourceStream = new StreamReader(Server.MapPath("~/WebModules/Reports/SalesReport2.rdlc"));

                   // Assembly.GetCallingAssembly()
                     //   .GetManifestResourceStream("HealthyChef.WebModules.Reports.SalesReport2.rdlc");

                    ReportViewer1.LocalReport.LoadSubreportDefinition("SalesReport2", resourceStream);

                    ReportDataSource rds1 = new ReportDataSource("SalesReport1", carts);
                    ReportViewer1.LocalReport.DataSources.Add(rds1);

                    this.ReportViewer1.LocalReport.Refresh();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            try
            {
                int cartId = int.Parse(e.Parameters[0].Values[0]);

                var items = hccCartItem.GetTaxable(cartId);

                e.DataSources.Add(new ReportDataSource("SalesReport2", items));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}