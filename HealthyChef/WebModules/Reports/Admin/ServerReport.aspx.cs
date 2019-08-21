using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class ServerReport : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Bind_Report();
        }

        void Bind_Report()
        {
            if(!Page.IsPostBack)
            {
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Remote;
                Microsoft.Reporting.WebForms.ServerReport serverReport = ReportViewer1.ServerReport;

                serverReport.ReportServerUrl = new Uri("");
                
                serverReport.ReportPath = "~/WebModules/Reports/SalesReport3.rdlc";

                //serverReport.
            }
        }
    }
}