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
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;

namespace HealthyChef.WebModules.Reports.Admin
{
    public partial class MealLabels : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

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

                //List<MOTCartItem> motItems = Search_MealOrderTicketForMOT2(searchStart, searchEnd)
                //     .Where(a => a.MealType == ((Enums.MealTypes)Enum.Parse(typeof(Enums.MealTypes), ddlMealTypes.SelectedValue)))
                //     .OrderBy(a => a.CartItem.ItemType).ThenBy(a => a.ItemName).ThenBy(a => a.CustomerName).ToList();

                Tuple<List<MOTCartItem>, List<MOTCartItem>> sortItems = MOTCartItem.SortOddEven(motItems);

                ReportViewer1.LocalReport.DataSources.Clear();

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/WebModules/Reports/MealLabels1.rdlc");
                ReportDataSource rdsO = new ReportDataSource("MOT7Odds", sortItems.Item1);
                ReportViewer1.LocalReport.DataSources.Add(rdsO);
                ReportDataSource rdsE = new ReportDataSource("MOT7Evens", sortItems.Item2);
                ReportViewer1.LocalReport.DataSources.Add(rdsE);
                this.ReportViewer1.LocalReport.Refresh();
            }
        }

        public static List<MOTCartItem> Search_MealOrderTicketForMOT2(DateTime? startDate, DateTime? endDate)
        {

            List<MOTCartItem> motItems = new List<MOTCartItem>();
            //List<hccMenuItem> _allMenuItems = hccMenuItem.GetAll();  
            

            try
            {
                //SqlParameter[] dateParams = new SqlParameter[] {new SqlParameter("@STARTDATE", startDate), new SqlParameter("@ENDDATE", endDate) };
                //SqlConnection _conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString);

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GETDATAFORMEALLABELREPORT", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@STARTDATE", startDate);
                        cmd.Parameters.AddWithValue("@ENDDATE", endDate);

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            motItems.Add(PrepareMOTCartItem(t));
                        }

                        conn.Close();
                    }
                }

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GETDATAFORMEALLABELREPORTBYPROGRAM", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@STARTDATE", startDate);
                        cmd.Parameters.AddWithValue("@ENDDATE", endDate);

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            motItems.Add(PrepareMOTCartItem(t));
                        }

                        conn.Close();
                    }
                }

                return motItems.OrderBy(a => a.DeliveryDate)
                    .ThenBy(a => a.ItemName).ThenBy(a => a.OrderNumber.Contains("ALC")).ThenBy(a => a.CustomerName).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MOTCartItem PrepareMOTCartItem(IDataReader t)
        {
            MOTCartItem _MOTCartItem = new MOTCartItem();

            try
            {
                _MOTCartItem.CartItemId = Convert.ToInt32(t["CartItemID"]);
                _MOTCartItem.DeliveryDate = Convert.ToDateTime(t["DeliveryDate"]);
                _MOTCartItem.OrderNumber = string.Format("{0}-ALC", t["OrderNumber"]);
                _MOTCartItem.Servings = Convert.ToString(t["Quantity"]);
                _MOTCartItem.Quantity = Convert.ToInt32(t["Quantity"]);

                if (t["MEAL_MEALSIZEID"] != DBNull.Value)
                {
                    int _mealSizeID = Convert.ToInt32(t["MEAL_MEALSIZEID"]);
                    Enums.CartItemSize size = (Enums.CartItemSize)_mealSizeID;

                    if (size == Enums.CartItemSize.NoSize)
                        size = Enums.CartItemSize.RegularSize;

                    _MOTCartItem.PortionSize = Enums.GetEnumDescription(size);
                }

                _MOTCartItem.CustomerName = Convert.ToString(t["CUSTOMERNAME"]);
                _MOTCartItem.ProfileName = Convert.ToString(t["ProfileName"]);

                _MOTCartItem.ItemName = Convert.ToString(t["NAME"]);

                if (t["MealTypeID"] != DBNull.Value)
                {
                    int _mealTypeID = Convert.ToInt32(t["MealTypeID"]);
                    _MOTCartItem.MealType = (Enums.MealTypes)_mealTypeID;
                }

                if (t["DefaultShippingTypeID"] != DBNull.Value)
                {
                    int _delMethodID = Convert.ToInt32(t["DefaultShippingTypeID"]);
                    _MOTCartItem.DeliveryMethod = ((Enums.DeliveryTypes)_delMethodID).ToString();
                }

                //preferences
                _MOTCartItem.Preferences = hccCartItemMealPreference.GetPrefsBy(_MOTCartItem.CartItemId)
                        .Select(a => a.Name).DefaultIfEmpty("None").Aggregate((c, d) => c + ", " + d);

                var CartItemID = Convert.ToInt32(t["ItemTypeID"]);
                _MOTCartItem.CartItem = new hccCartItem() { CartItemID = _MOTCartItem.CartItemId, ItemTypeID = CartItemID, Meal_MealSizeID = Convert.ToInt32(t["MEAL_MEALSIZEID"]) };

                //sides
                _MOTCartItem.Sides = "None";
                if (hccMenuItem.EntreeMealTypes.Contains(_MOTCartItem.MealType) && _MOTCartItem.CartItem.MealSideMenuItems.Count > 0)
                {
                    _MOTCartItem.Sides = _MOTCartItem.CartItem.GetMealSideMenuItemsAsSectionString(", ");
                }
                
            }
            catch (Exception E)
            {

            }

            return _MOTCartItem;
        }
        
    }
}