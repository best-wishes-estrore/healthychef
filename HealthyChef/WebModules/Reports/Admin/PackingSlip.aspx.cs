using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Hosting;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using Microsoft.Reporting.WebForms;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class PackingSlip : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ReportViewer1.ReportRefresh += ReportViewer1_ReportRefresh;
            ReportViewer1.ReportError += ReportViewer1_ReportError;
        }

        protected void ReportViewer1_ReportError(object sender, ReportErrorEventArgs e)
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
                hccProductionCalendar nextProdCal = hccProductionCalendar.GetNext4Last2Calendars()[1];
                txtStartDate.Text = nextProdCal == null ? DateTime.Now.ToShortDateString() : nextProdCal.DeliveryDate.ToShortDateString();
            }
        }

        void BindReport()
        {
            string empty = string.Empty;
            this.Page.Validate("MealOrderGroup");
            if (!this.Page.IsValid)
                return;
            DateTime result;
            bool flag = DateTime.TryParse(this.txtStartDate.Text.Trim(), out result);
            DateTime? nullable = new DateTime?();
            if (flag)
                nullable = new DateTime?(result);
            List<HealthyChef.DAL.PackingSlip> packingSlips = HealthyChef.DAL.PackingSlip.GeneratePackingSlips(nullable.Value);
            try
            {
                this.ReportViewer1.LocalReport.DataSources.Clear();
                string applicationPhysicalPath = HostingEnvironment.ApplicationPhysicalPath;
                this.ReportViewer1.LocalReport.ReportPath = this.Server.MapPath("~/WebModules/Reports/PackSlip1.rdlc");
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), Environment.NewLine);
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "resourceStream-Start1" + Environment.NewLine);
                this.ReportViewer1.LocalReport.SubreportProcessing += new SubreportProcessingEventHandler(this.LocalReport_SubreportProcessing);
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "resourceStream-Start2" + Environment.NewLine);
                Stream manifestResourceStream = Assembly.GetCallingAssembly().GetManifestResourceStream("HealthyChef.WebModules.Reports.ItemsByOrderNum.rdlc");
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "resourceStream-Start3" + Environment.NewLine);
                this.ReportViewer1.LocalReport.LoadSubreportDefinition("SubItemsByOrderNum", manifestResourceStream);
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "resourceStream-Start4" + Environment.NewLine);
                string[] manifestResourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "resourceStream-Start5" + Environment.NewLine);
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), Environment.NewLine + "Logged at " + DateTime.Now.ToString("yyyyMMddHHmmss") + Environment.NewLine);
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "Records Retrived : " + (object)packingSlips.Count + Environment.NewLine);
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "Path taken " + this.ReportViewer1.LocalReport.ReportPath + Environment.NewLine);
                string contents = "";
                foreach (string str in manifestResourceNames)
                    contents = contents + str + Environment.NewLine;
                if (manifestResourceNames.Length == 0)
                    contents = "No Resources found";
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), contents);
                this.ReportViewer1.LocalReport.DataSources.Add(new ReportDataSource("PackSlip1", packingSlips));
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "resourceStream-Start6" + Environment.NewLine);
                string str1 = applicationPhysicalPath + "/WebModules/Reports/PackSlip1.rdlc";
                this.ReportViewer1.LocalReport.ReportPath = str1;
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "Path Used" + str1 + Environment.NewLine);
                this.ReportViewer1.LocalReport.Refresh();
            }
            catch (Exception ex)
            {
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "Message" + ex.Message.ToString());
                File.AppendAllText(this.Server.MapPath("~/ErrLog.txt"), "InnerException" + ex.InnerException.ToString());
            }
        }
        void LocalReport_SubreportProcessing(object sender, SubreportProcessingEventArgs e)
        {
            string ordNum = e.Parameters[0].Values[0];
            DateTime deliveryDate = DateTime.Parse(e.Parameters[1].Values[0]);

            List<ChefProdItem> psItems = hccMenuItem.GetPackSlipItems(ordNum, deliveryDate);

            e.DataSources.Add(new ReportDataSource("PackSlipItems1", psItems));
        }
    }
}