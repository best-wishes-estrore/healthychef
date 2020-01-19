using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Configuration;

namespace HealthyChef.DAL
{
    public class ReportSprocs
    {
        public static List<MealItemReportItem> GetMenuItemsByDateRange(DateTime startDate, DateTime endDate, Boolean includeAlaCarte = true)
        {
            try
            {
                List<MealItemReportItem> retVals = new List<MealItemReportItem>();

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_MenuItemsByDateRange", conn))
                        {
                            conn.Open();

                            SqlParameter prm1 = new SqlParameter("@start", SqlDbType.DateTime);
                            prm1.Value = startDate;
                            SqlParameter prm2 = new SqlParameter("@end", SqlDbType.DateTime);
                            prm2.Value = endDate;
                            SqlParameter prm3 = new SqlParameter("@includeAlaCarte", SqlDbType.Bit);
                            prm3.Value = includeAlaCarte;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(prm1);
                            cmd.Parameters.Add(prm2);
                            cmd.Parameters.Add(prm3);

                            SqlDataReader t = cmd.ExecuteReader();

                            if (t != null)
                            {
                                // get from reader
                                retVals = MealItemReportItem.GetFromReader(t);
                            }
                            else
                                retVals = null;

                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    scope.Complete();
                }

                if (retVals != null)
                    return retVals;
                else
                    throw new Exception("GetMenuItemsByDateRange could not be returned.");
            }
            catch
            {
                throw;
            }
        }

        public static List<MealItemReportItem> GetMenuItemsByOrderNumber(string orderNumber, DateTime deliveryDate)
        {
            try
            {
                List<MealItemReportItem> retVals = null;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_MenuItemsByOrderNumber", conn))
                        {
                            conn.Open();

                            SqlParameter prm1 = new SqlParameter("@orderNumber", SqlDbType.VarChar);
                            prm1.Value = orderNumber;
                            SqlParameter prm2 = new SqlParameter("@deliveryDate", SqlDbType.DateTime);
                            prm2.Value = deliveryDate;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(prm1);
                            cmd.Parameters.Add(prm2);

                            SqlDataReader t = cmd.ExecuteReader();

                            if (t != null)
                            {   // get from reader
                                retVals = MealItemReportItem.GetFromReader(t);
                            }
                            else
                                retVals = null;

                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    scope.Complete();
                }

                if (retVals != null)
                    return retVals;
                else
                    throw new Exception("GetMenuItemsByDateRange could not be returned.");
            }
            catch
            {
                throw;
            }
        }
    }

    public class MealItemReportItem
    {
        public string OrderNumber { get; set; }
        public string ItemName { get; set; }
        public string MealTypeName { get; set; }
        public string MealSizeName { get; set; }
        public int Quantity { get; set; }
        public int DayNum { get; set; }
        public string Prefs { get; set; }
        public string ParentType { get; set; }
        public int ParentTypeId { get; set; }
        public int ParentId { get; set; }
        public int MealTypeId { get; set; }
        public int MenuItemId { get; set; }
        public int MealSizeId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public int RowId { get; set; }
        public int CartItemId { get; set; }
        public int UserProfileId { get; set; }
        public string ProfileName { get; set; }
        public string OwnerName { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }

        public static List<MealItemReportItem> GetFromReader(SqlDataReader reader)
        {
            try
            {
                List<MealItemReportItem> retVals = new List<MealItemReportItem>();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MealItemReportItem retVal = new MealItemReportItem();

                        retVal.OrderNumber = reader["OrderNumber"].ToString();
                        retVal.ItemName = reader["ItemName"].ToString();
                        retVal.MealTypeName = reader["MealTypeName"].ToString();
                        retVal.MealSizeName = reader["MealSizeName"].ToString();
                        retVal.Quantity = int.Parse(reader["Quantity"].ToString());
                        retVal.DayNum = int.Parse(reader["DayNum"].ToString());
                        retVal.Prefs = reader["Prefs"].ToString();
                        retVal.ParentType = reader["ParentType"].ToString();
                        retVal.ParentTypeId = int.Parse(reader["ParentTypeId"].ToString());
                        retVal.ParentId = int.Parse(reader["ParentId"].ToString());
                        retVal.MealTypeId = int.Parse(reader["MealTypeId"].ToString());
                        retVal.MenuItemId = int.Parse(reader["MenuItemId"].ToString());
                        retVal.MealSizeId = int.Parse(reader["MealSizeId"].ToString());
                        retVal.DeliveryDate = DateTime.Parse(reader["DeliveryDate"].ToString());
                        retVal.CartItemId = int.Parse(reader["CartItemId"].ToString());
                        retVal.UserProfileId = int.Parse(reader["UserProfileId"].ToString());
                        retVal.PlanId = int.Parse(reader["PlanId"].ToString());
                        retVal.PlanName = reader["PlanName"].ToString();
                        retVal.RowId = int.Parse(reader["RowId"].ToString());

                        retVals.Add(retVal);
                    }

                    reader.Close();
                }

                return retVals;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
