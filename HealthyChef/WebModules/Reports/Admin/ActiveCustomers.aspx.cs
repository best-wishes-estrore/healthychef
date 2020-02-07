using HealthyChef.DAL;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Objects;
using HealthyChef.DAL.Classes;
using HealthyChef.Common;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Text;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class ActiveCustomers : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            rvwCustomers.ReportRefresh += RefreshReport;
            rvwCustomers.ReportError += ShowReportErrors;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                hccProductionCalendar cal = hccProductionCalendar.GetNext4Calendars().FirstOrDefault();

                txtStartDate.Text = cal == null ? DateTime.Now.ToShortDateString() : cal.DeliveryDate.ToShortDateString();
                txtEndDate.Text = cal == null ? DateTime.Now.ToShortDateString() : cal.DeliveryDate.ToShortDateString();

                BindProductTypes();
            }
        }

        private void BindProductTypes()
        {
            if (ddlProductTypes.Items.Count < 1)
            {
                using (var context = new healthychefEntities())
                {
                    Dictionary<string, int> dataSource = new Dictionary<string, int>();

                    var programs = context.hccPrograms.ToList();
                    foreach (var program in programs)
                    {
                        dataSource.Add(program.Name, program.ProgramID);
                    }

                    dataSource.Add("A La Carte", -2);
                    dataSource.Add("Gift Certificate", -3);
                    dataSource.Add("A La Carte - Family", -4);

                    ddlProductTypes.DataSource = dataSource;
                    ddlProductTypes.DataTextField = "Key";
                    ddlProductTypes.DataValueField = "Value";

                    ddlProductTypes.DataBind();

                    ddlProductTypes.Items.Insert(0, new ListItem("<< All Product Types >>", "-1"));
                }
            }
        }

        private void BindReport()
        {
            Page.Validate("CustomerGroup");

            if (Page.IsValid)
            {
                DateTime start;

                bool startDateIsValid = DateTime.TryParse(txtStartDate.Text.Trim(), out start);

                DateTime end;

                bool endDateIsValid = DateTime.TryParse(txtEndDate.Text.Trim(), out end);

                int programId = int.Parse(ddlProductTypes.SelectedValue);

                rvwCustomers.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/ActiveCustomers.rdlc");
                var ds = new ReportDataSource("ActiveCustomers", GetQueryResults(start, end, programId));

                rvwCustomers.LocalReport.DataSources.Clear();

                rvwCustomers.LocalReport.DataSources.Add(ds);

                rvwCustomers.LocalReport.Refresh();
            }
        }

        protected void RefreshReport(object sender, EventArgs e)
        {
            BindReport();
        }

        protected void ShowReportErrors(object sender, ReportErrorEventArgs e)
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

        private List<ActiveCustomerDto> GetQueryResults(DateTime startDate, DateTime endDate, int programId)
        {
            var activeCustomers = ActiveCustomersQueryFactory.CreateQuery(startDate, endDate, programId);
            //According to Dan- June-29-2019 - version 2
            
            //foreach (var customer in activeCustomers)
            //{
            //    // For getting total amount and product type
            //    hccCart hcart = hccCart.GetBy(customer.LastPurchaseId);
            //    Guid? userId = hcart.AspNetUserID;
            //    List<hccCart> carts = new List<hccCart>();
                
               // carts = hccCart.GetBy(userId.Value);
                //foreach (var cartid in carts)//.Where(x => x.StatusID == 20)) //x.PurchaseDate >= start && x.PurchaseDate <= end && 
                //{
                //    hccCart cart = hccCart.GetById(cartid.CartID);
                //    List<hccCartItem> hccCartItems = hccCartItem.GetBy(cartid.CartID);
                //    customer.TotalAmount += cartid.TotalAmount;                    
                //}
                //customer.TotalAmount = Math.Round(customer.TotalAmount, 2);
                // For getting user preferences

              //  hccUserProfile UserProfile = hccUserProfile.GetByAspId(userId.Value);
                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                //{
                //    using (SqlCommand cmd = new SqlCommand("GETUSERPREFERENCES", conn))
                //    {
                //        conn.Open();
                //        SqlParameter prm1 = new SqlParameter("@UserProfileId", SqlDbType.Int);
                //        prm1.Value = UserProfile.UserProfileID;
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(prm1);
                //        SqlDataReader reader = cmd.ExecuteReader();
                //        if (reader != null)
                //        {
                //            StringBuilder PreferenceN = new StringBuilder();
                //            while (reader.Read())
                //            {
                //                var name = reader["PreferenceName"].ToString();
                //                PreferenceN.Append(name);
                //                    PreferenceN.Append(",");
                //            }
                //            customer.AccountPreferences = PreferenceN.ToString();
                //        }
                //        else
                //          customer.AccountPreferences = null;
                //        cmd.Dispose();
                //        conn.Close();
                //        conn.Dispose();
                //    }
                //}
                // For getting user allergens
                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                //{
                //    using (SqlCommand cmd = new SqlCommand("GETUSERALLERGENS", conn))
                //    {
                //        conn.Open();
                //        SqlParameter prm1 = new SqlParameter("@UserProfileId", SqlDbType.Int);
                //        prm1.Value = UserProfile.UserProfileID;
                //        cmd.CommandType = CommandType.StoredProcedure;
                //        cmd.Parameters.Add(prm1);
                //        SqlDataReader reader = cmd.ExecuteReader();
                //        if (reader != null)
                //        {
                //            StringBuilder AllergenN = new StringBuilder();
                //            while (reader.Read())
                //            {
                //                var name = reader["AllergenName"].ToString();
                //                AllergenN.Append(name);
                //                    AllergenN.Append(",");
                //            }
                //            customer.AccountAllergens = AllergenN.ToString();
                //        }
                //        else
                //            customer.AccountAllergens = null;
                //        cmd.Dispose();
                //        conn.Close();
                //        conn.Dispose();
                //    }
                //}
               
                
            //    //For getting product type
            //    List<hccCartItem> cartItems = hccCartItem.GeCartItemsByPurchaseNumber(customer.LastPurchaseId);
            //    StringBuilder sb = new StringBuilder();
            //    List<string> itemTypes = new List<string>();
            //    if (cartItems.Count() > 0)
            //    {
            //        var lastItem = cartItems.Last();
            //    }
            //    foreach (var items in cartItems.Where(x=>x.ItemPrice>0))
            //    {
            //        switch(items.ItemTypeID)
            //        {
            //            case 1:
            //                if (items.Plan_IsAutoRenew == true)
            //                {
            //                    itemTypes.Add("A La Carte - Family");
            //                }
            //                else
            //                {
            //                    itemTypes.Add("A La Carte");
            //                }
            //                break;
            //            case 2:
            //                hccProgramPlan plan = hccProgramPlan.GetById(items.Plan_PlanID.Value);
            //                itemTypes.Add(plan.Name);
            //                break;
            //            case 3:
            //                itemTypes.Add("Gift Certificate");
            //                break;
            //        }
            //    }
            //    foreach(var itemName in itemTypes.Distinct())
            //    {
            //        sb.Append(itemName + ",");
            //    }
            //    if (sb.ToString() != "")
            //    {
            //        customer.ProductType = sb.ToString().Remove(sb.ToString().Length - 1);// To remove "," at the end
            //    }
            //    //For getting custom preferences
            //    StringBuilder customPreference = new StringBuilder();
            //    List<string> customPreferences = new List<string>();
            //    foreach (var items in cartItems.Where(x => x.ItemPrice > 0 && x.ItemTypeID==1))
            //    {
            //        int count = items.ItemName.Split('-').Count();
            //        if (count == 5 )
            //        {
            //            customPreferences.Add(items.ItemName.Split('-')[3]);
            //        }
            //    }
            //    foreach(var name in customPreferences.Distinct())
            //    {
            //        customPreference.Append(name + ",");
            //    }
            //    if (customPreference.ToString() != "")
            //    {
            //        customer.AccountCustomPreferences = customPreference.ToString().Remove(customPreference.ToString().Length - 1);// To remove "," at the end
            //    }
            //}
            return activeCustomers;
        }
    }
}