using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using System.Data;
using Microsoft.Reporting.WebForms;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class ProductFeed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<hccProductionCalendar> nextProdCal = hccProductionCalendar.GetNext4Last2Calendars();
                txtStartDate.Text = nextProdCal[2].DeliveryDate.ToShortDateString();
                txtEndDate.Text = nextProdCal[2].DeliveryDate.ToShortDateString();
            }

        }
        protected void ButtonRefresh_Click(object sender, EventArgs e)
        {
            BindReport();
        }

        void BindReport()
        {
            Page.Validate("ProductFeedGroup");

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
                List<HealthyChef.DAL.ProductFeed> Finalitems = ReportSprocs.GetProductFeed(searchStart,searchEnd);


                ProductFeed_ReportViewer.LocalReport.DataSources.Clear();

                ProductFeed_ReportViewer.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/ProductFeed.rdlc");
                ReportDataSource rdsO = new ReportDataSource("ProductFeed", Finalitems);
                ProductFeed_ReportViewer.LocalReport.DataSources.Add(rdsO);
                this.ProductFeed_ReportViewer.LocalReport.Refresh();
            }
        }
    }
}