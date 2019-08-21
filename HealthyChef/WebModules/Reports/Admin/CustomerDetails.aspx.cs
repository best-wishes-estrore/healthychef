using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using HealthyChef.DAL.Datasets;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class CustomerDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReportViewer1.ZoomMode = ZoomMode.FullPage;
                ReportViewer1.ProcessingMode = ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/CustomerDetails.rdlc");
                DataSet1 dscustomers = Getdata();
                ReportDataSource dataSource = new ReportDataSource("DataSet1", dscustomers.Tables[0]);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dataSource);



                //this.ReportViewer1.LocalReport.Refresh();

            }
        }
        private DataSet1 Getdata()
        {
           
            String conString = ConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            SqlCommand cmd = new SqlCommand("hcc_GetCustomerReport");
            using (SqlConnection con = new SqlConnection(conString))
            {

                using (SqlDataAdapter da = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    cmd.CommandType = CommandType.StoredProcedure;
                    da.SelectCommand = cmd;
                    using (DataSet1 dscustomers = new DataSet1())
                    {
                        da.Fill(dscustomers, "DataTable1");
                        return dscustomers;
                    }
                }

            }

        }
    }
}