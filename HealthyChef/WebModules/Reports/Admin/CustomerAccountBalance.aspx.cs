using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using Microsoft.Reporting.WebForms;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class CustomerAccountBalance : System.Web.UI.Page
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
                txtStartDate.Text = DateTime.Now.ToShortDateString();

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

                DateTime? searchStart = null;

                if (tryStart)
                    searchStart = startDate;
                DateTime datetime = searchStart.Value;
                var t = hccLedger.GetBalanceReportItems(datetime);

                ReportViewer1.LocalReport.DataSources.Clear();

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/AcctBalance1.rdlc");
                ReportDataSource rdsO = new ReportDataSource("AcctBalance1", t);
                ReportViewer1.LocalReport.DataSources.Add(rdsO);

                this.ReportViewer1.LocalReport.Refresh();
            }
        }
    }
}