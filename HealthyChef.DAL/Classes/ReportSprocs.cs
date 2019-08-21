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
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
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
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
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
        public static List<ProductFeed> GetProductFeed(DateTime? startdate, DateTime? enddate)
        {
            try
            {
                List<ProductFeed> returnValues = null;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_ProductFeed", conn))
                        {
                            conn.Open();

                            SqlParameter prm1 = new SqlParameter("@start", SqlDbType.DateTime);
                            prm1.Value = startdate;
                            SqlParameter prm2 = new SqlParameter("@end", SqlDbType.DateTime);
                            prm2.Value = enddate;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(prm1);
                            cmd.Parameters.Add(prm2);

                            SqlDataReader t = cmd.ExecuteReader();

                            if (t != null)
                            {   
                                returnValues = ProductFeed.GetFromReader(t);
                            }
                            else
                                returnValues = null;
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                        }
                    }
                    scope.Complete();
                }
                if (returnValues != null)
                    return returnValues;
                else
                    throw new Exception("GetMenuItemsByDateRange could not be returned.");
            }
            catch(Exception ex)
            {
                throw ex;
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
        public bool IsFamilyStyle { get; set; }
        public int CartId { get; set; }

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

                        retVal.CartId = int.Parse(reader["CartId"].ToString());
                        if(reader["IsFamilyStyle"].ToString()!="")
                        {
                            retVal.IsFamilyStyle = Convert.ToBoolean(reader["IsFamilyStyle"].ToString());
                        }
                        else
                        {
                            retVal.IsFamilyStyle = false;
                        }
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
    public class ProductFeed
    {
        public int TypeId { get; set; }
        public string MenuItemId { get; set; }
        public int MenuId { get; set; }
        public int MealTypeId { get; set; }
        public string ItemTitle { get; set; }
        public string ItemDescription { get; set; }
        public string ItemLink { get; set; }
        public string ItemCondition { get; set; }
        public decimal Price { get; set; }
        public string ItemPrice { get; set; }
        public string ItemAvailability { get; set; }
        public string ItemImageLink { get; set; }
        public string ItemMPN { get; set; }
        public string Brand { get; set; }
        public string GoogleProductCategory { get; set; }

        public static List<ProductFeed> GetFromReader(SqlDataReader reader)
        {
            try
            {
                var Host = "https://www.healthychefcreations.com";
                //var Host = "dev.healthychefcreations.com";
                var HostAdmin = "https://admin.healthychefcreations.com";
                //var HostAdmin = "devadmin.healthychefcreations.com";
                List<ProductFeed> retVals = new List<ProductFeed>();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ProductFeed retVal = new ProductFeed();
                        retVal.TypeId = int.Parse(reader["TypeId"].ToString());
                        if (int.Parse(reader["TypeId"].ToString()) == 1)
                        {
                            retVal.MenuItemId = reader["ItemId"].ToString();
                        }
                        else
                        {
                            retVal.MenuItemId = "PR-" + reader["ItemId"].ToString();
                        }
                        if (int.Parse(reader["TypeId"].ToString()) != 1)
                        {
                            retVal.MenuId = int.Parse(reader["MenuId"].ToString());
                        }
                        if (int.Parse(reader["TypeId"].ToString()) != 2)
                        {
                            retVal.MealTypeId = int.Parse(reader["MealTypeId"].ToString());
                        }
                        retVal.ItemTitle = reader["ItemName"].ToString();
                        retVal.ItemDescription = reader["ItemDescription"].ToString();
                        retVal.ItemLink = Host+"/browse-menu.aspx";
                        retVal.ItemCondition = "new";
                        retVal.Price =Math.Round(decimal.Parse(reader["ItemPrice"].ToString()),2);
                        retVal.ItemPrice = "$" + retVal.Price;
                        if (Boolean.Parse(reader["ItemAvailability"].ToString()))
                        {
                            retVal.ItemAvailability = "In Stock";
                        }
                        else
                        {
                            retVal.ItemAvailability = "Out Of Stock";
                        }
                        if(reader["ItemImagePath"].ToString()=="" || reader["ItemImagePath"].ToString()==null && int.Parse(reader["TypeId"].ToString()) == 1)
                        {
                            retVal.ItemImageLink = Host+"/userfiles/images/genericMenu.jpg";
                        }
                        else if (reader["ItemImagePath"].ToString() == "" || reader["ItemImagePath"].ToString() == null && int.Parse(reader["TypeId"].ToString()) == 2)
                        {
                            retVal.ItemImageLink = "N/A";
                        }
                        else
                        {
                            retVal.ItemImageLink = HostAdmin + reader["ItemImagePath"].ToString();
                        }
                        retVal.ItemMPN = retVal.MenuItemId;
                        retVal.Brand = "Healthy Chef Creations";
                        retVal.GoogleProductCategory = "Food, Beverages & Tobacco > Food Items > Prepared Foods > Prepared Meals & Entrees";
                        if (retVals.Count(x => x.MenuItemId == retVal.MenuItemId && x.TypeId==retVal.TypeId) == 0)
                        {
                            retVals.Add(retVal);
                        }
                    }

                    reader.Close();
                }

                foreach(var item in retVals.Where(x=>x.TypeId==2)) //updating URL's for programs
                {
                    switch(item.MenuId)
                    {
                        case 33 :
                            item.ItemLink = Host+"/weight-loss-programs/healthy-weight-loss.aspx";
                            break;
                        case 34:
                            item.ItemLink = Host+"/meal-programs/healthy-mommy.aspx";
                            break;
                        case 49:
                            item.ItemLink = Host+"/meal-programs/healthy-living.aspx";
                            break;
                        case 59:
                            item.ItemLink = Host+"/weight-loss-programs/low-carb.aspx";
                            break;
                        case 61:
                            item.ItemLink = Host+"/weight-loss-programs/hcg-diet.aspx";
                            break;
                        case 50:
                            item.ItemLink = Host+"/Programs/DetailsbyCheckbox/50/HealthyLivingDinner";
                            break;
                        case 51:
                            item.ItemLink = Host+"/Programs/DetailsbyCheckbox/51/HealthyLivingDinner";
                            break;
                        case 53:
                            item.ItemLink = Host+"/Programs/DetailsbyCheckbox/53/HealthyLivingDinner";
                            break;
                        case 54:
                            item.ItemLink = Host+"/Programs/DetailsbyCheckbox/54/HealthyLivingBreakfastLunch";
                            break;
                        case 63:
                            item.ItemLink = Host+"/Programs/DetailsbyCheckbox/63/HealthyLivingBreakfast";
                            break;
                        case 64:
                            item.ItemLink = Host+"/Programs/DetailsbyCheckbox/64/HealthyLivingLunch";
                            break;
                    }
                }

                foreach(var item in retVals.Where(x=>x.TypeId==1))
                {
                    item.ItemLink = Host+"/ProductDescription?ItemName=" + item.ItemTitle;
                   //item.ItemLink = "http://localhost:49883//ProductDescription?ItemName=" + item.ItemTitle;
                }
                return retVals;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
