using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Configuration;

namespace HealthyChef.DAL.Extensions
{
    public class hccShippingZone
    {
        #region Manage Shipping Zones

        public static int AddUpdateShippingZone(int ZoneID, string ZoneName, string TypeName, string Multiplier, string MinFee, string MaxFee, bool DefaultshipZone, bool IsPickupShippingZone)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_UpsertShippingZone", conn))
                        {
                            var minFee = MinFee == "" ? "0.00" : MinFee;
                            var maxFee = MaxFee == "" ? "0.00" : MaxFee;
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                            cmd.Parameters.AddWithValue("@ZoneName", ZoneName);
                            cmd.Parameters.AddWithValue("@TypeName", TypeName);
                            cmd.Parameters.AddWithValue("@Multiplier", Multiplier);
                            cmd.Parameters.AddWithValue("@MinFee", Convert.ToDecimal(minFee));
                            cmd.Parameters.AddWithValue("@MaxFee", Convert.ToDecimal(maxFee));
                            cmd.Parameters.AddWithValue("@IsDefaultShippingZone", DefaultshipZone);
                            cmd.Parameters.AddWithValue("@IsPickupShippingZone", IsPickupShippingZone);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                    }
                    if (retVal > 0)
                        scope.Complete();
                }

                //if (retVal > 0)
                return retVal;
                //else
                // throw new Exception("A new Shipping Zone could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }

        public DataTable BindGrid()
        {

            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetShippingZoneList"))
                {

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];

                        }
                    }
                }
            }

        }

        public static int Delete(int customerId)
        {
            try
            {
                int retVal = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_DeleteShippingZone", con))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ZoneID", customerId);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            con.Close();
                            con.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                        if (retVal > 0)
                            scope.Complete();
                    }
                }
                if (retVal > 0)
                    return retVal;
                else
                    throw new Exception("Shipping Zone could not be deleted.");
            }
            catch
            {
                return 1;
            }
        }


        public string GetDefaultshippingZone()
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("select ZoneID from [dbo].[hccShippingZone] where IsDefaultShippingZone=1"))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    string zoneid = "";
                    while (dr.Read())
                    {
                        zoneid = dr["ZoneID"].ToString();
                    }
                    return zoneid;

                }
            }

        }

        #endregion Manage Shipping Zones

        #region Manage Box Size

        public static int AddUpdateBox(int BoxID, string BoxName, string DIM_W, string DIM_L, string DIM_H, int MaxNoMeals)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_UpsertBoxSize", conn))
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BoxID", BoxID);
                            cmd.Parameters.AddWithValue("@BoxName", BoxName);
                            cmd.Parameters.AddWithValue("@DIM_W", Convert.ToDecimal(DIM_W));
                            cmd.Parameters.AddWithValue("@DIM_L", Convert.ToDecimal(DIM_L));
                            cmd.Parameters.AddWithValue("@DIM_H", Convert.ToDecimal(DIM_H));
                            cmd.Parameters.AddWithValue("@MaxNoMeals", MaxNoMeals);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                    }
                    if (retVal > 0)
                        scope.Complete();
                }

                //if (retVal > 0)
                return retVal;
                //else
                //throw new Exception("A new Box could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }

        public DataTable BindGridBoxSizes()
        {

            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetBoxSizeList"))
                {

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];
                        }
                    }
                }
            }
        }

        public static int DeleteBoxSize(int BoxId)
        {
            try
            {
                int retVal = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_DeleteBoxSize", con))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BoxID", BoxId);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            con.Close();
                            con.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                        if (retVal > 0)
                            scope.Complete();
                    }
                }
                if (retVal > 0)
                    return retVal;
                else
                    throw new Exception("Box could not be deleted.");
            }
            catch
            {
                return 1;
            }
        }

        #endregion Manage Box Size

        #region Manage Zip Codes
        public DataTable BindGridZipCodesNew()
        {

            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                //using (SqlCommand cmd = new SqlCommand("hcc_GetZipCodesListNew"))
                using (SqlCommand cmd = new SqlCommand("hcc_GetZipCodesList_new"))
                {

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];
                        }
                    }
                }
            }

        }
        public DataTable BindGridZipCodes()
        {

            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetZipCodesList"))//Changes required //Sp_helptext
                {

                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];
                        }
                    }
                }
            }

        }
        public List<hccZipCodes> BindGridDownloadZipCodes()
        {
            List<hccZipCodes> list_zipcodes = new List<hccZipCodes>();
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetZipCodesList"))////Changes required //Sp_helptext
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        hccZipCodes zipcodes = new Extensions.hccZipCodes();
                        zipcodes.ZipZoneID = dr.GetInt32(0);
                        zipcodes.ZipCode = dr.GetString(1);
                        zipcodes.ZipZoneID = dr.GetInt32(2);
                        list_zipcodes.Add(zipcodes);
                    }
                }

            }
            return list_zipcodes;
        }

        public static int AddUpdateZipCodelatest(int ZipCodeID, string ZipCode, string ZoneID, string ShippingClass)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_UpsertZipCodeLatest", conn))
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ZipZoneID", ZipCodeID);
                            cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                            cmd.Parameters.AddWithValue("@ShippingClass", ShippingClass);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }
                    }
                    if (retVal > 0)
                        scope.Complete();
                }
                //if (retVal > 0)
                return retVal;
                //else
                //throw new Exception("A new Zip Code could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }
        public static int AddUpdateZipCodeNew(int ZipCodeID, string ZipcodeFrom, string ZipcodeTo, string ZoneID, int ShipmenttypeId)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_UpsertZipCodeNew", conn)) //Changes required //Sp_helptext
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ZipZoneID", ZipCodeID);
                            cmd.Parameters.AddWithValue("@ZipCodeFrom", ZipcodeFrom);
                            cmd.Parameters.AddWithValue("@ZipCodeTo", ZipcodeTo);
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                            cmd.Parameters.AddWithValue("@ShipmentTypeId", ShipmenttypeId);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }
                    }
                    if (retVal > 0)
                        scope.Complete();
                }
                //if (retVal > 0)
                return retVal;
                //else
                //throw new Exception("A new Zip Code could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }
        public static int AddUpdateZipCode(int ZipCodeID, string Zipcode, string ZoneID)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_UpsertZipCode", conn))//Changes required //Sp_helptext
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ZipZoneID", ZipCodeID);
                            cmd.Parameters.AddWithValue("@ZipCode", Zipcode);
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                    }
                    if (retVal > 0)
                        scope.Complete();
                }
                //if (retVal > 0)
                return retVal;
                //else
                //throw new Exception("A new Zip Code could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }

        public static int DeleteZipCode(int ZipCodeID)
        {
            try
            {
                int retVal = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_DeleteZipCode", con))//Changes required //Sp_helptext
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ZipZoneID", ZipCodeID);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            con.Close();
                            con.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                        if (retVal > 0)
                            scope.Complete();
                    }
                }
                if (retVal > 0)
                    return retVal;
                else
                    throw new Exception("Zip Code could not be deleted.");
            }
            catch
            {
                return 1;
            }
        }

        public static int UploadZipCodesCSV(DataTable dt)
        {
            try
            {
                string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        //Set the database table name
                        sqlBulkCopy.DestinationTableName = "dbo.hccZipCodes";
                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }

        }

        public static int IsLookUpNew(string zipCode)
        {
            int userCount = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("hcc_GetShippingZoneByZipCodeNew", conn))
                    {

                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ZipCode", zipCode);
                            userCount = (int)cmd.ExecuteScalar();
                        }
                    }
                }
                return userCount;
            }
            catch
            {
                throw new Exception("Zip Code is not in Range.") { Source = "CUSTOM" };
            }

        }
        public static int IsLookUp(string zipCode)
        {
            int userCount = 0;
            try
            {
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Select Count(*) from [dbo].[hccZipCodes] where ZipCode=@ZipCode", conn))
                    {
                        conn.Open();
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZipCode", zipCode);
                        userCount = (int)cmd.ExecuteScalar();

                    }

                }

                return userCount;
            }
            catch
            {
                throw;
            }

        }

        #endregion Manage Zip Codes

        #region Shipping Zones

        public int AddUpdateBoxtoZoneFee(int ID, int BoxID, int ZoneID, decimal Cost, decimal Price, string Notes)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_UpsertBoxtoShippingZoneFee", conn))
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);
                            cmd.Parameters.AddWithValue("@BoxID", BoxID);
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                            cmd.Parameters.AddWithValue("@Cost", Cost);
                            cmd.Parameters.AddWithValue("@Price", Price);
                            cmd.Parameters.AddWithValue("@Notes", Notes);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                    }
                    if (retVal > 0)
                        scope.Complete();
                }

                if (retVal > 0)
                    return retVal;
                else
                    throw new Exception("A new Zip Code could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }

        public int DeleteBoxToZoneFee(int ID)
        {
            try
            {
                int retVal = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_DeleteBoxtoShippingZoneFee", con))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", ID);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            con.Close();
                            con.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                        if (retVal > 0)
                            scope.Complete();
                    }
                }
                if (retVal > 0)
                    return retVal;
                else
                    throw new Exception("Box to Zone Fee could not be deleted.");
            }
            catch
            {
                return 1;
            }
        }

        public DataTable BindGridShippingZoneByZoneID(int ZoneID)
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetShippingZoneDelails", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZoneId", ZoneID);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];
                        }
                    }
                }
            }
        }
        public int GetNumWeeks(int CartitemId)
        {
            try
            {
                int userCount = 1;
                string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("hcc_GetNoofWeeks", con);
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CartItemID", CartitemId);
                    object t = cmd.ExecuteScalar();
                    userCount = (int)cmd.ExecuteScalar();
                    //userCount = (int)cmd.ExecuteNonQuery();
                }
                return userCount;
            }
            catch //(Exception ex)
            {
                return 1;
                //throw ex;
            }
        }

        public int UpdateZoneName(string ZipCode, string ZoneID)
        {
            try
            {
                int userCount = 0;
                string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("update hccZipCodestemp set ZoneID=@ZoneID where ZipCode=@ZipCode", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
                    cmd.Parameters.AddWithValue("@ZoneID", ZoneID);
                    userCount = (int)cmd.ExecuteNonQuery();
                }
                return userCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataSet BindZoneByZipCode(string ZipCode) //Bind Zone by Zip Code into Dropdown
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetShippingZoneByZipCode", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
        }
        public string GetZipCodeByZoneName(string ZipCode) //Bind Zone by Zip Code into text box (Manoj)
        {
            string id;
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetZipcodebyZoneName", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
                    SqlParameter returnParameter = cmd.Parameters.Add("ZipCode", SqlDbType.NVarChar);
                    returnParameter.Direction = ParameterDirection.ReturnValue;
                    cmd.ExecuteNonQuery();

                    id = Convert.ToString (returnParameter.Value);
                }
            }
            return id;
        }
        //hcc_GetShippingZone

        public DataSet BindZoneGetShippingZone()
        {

            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetShippingZone", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@ZipCode", zipcode);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
        }
        public DataSet BindZoneByZipCodeNew(string ZipCode) //Bind Zone by Zip Code into Dropdown
        {

            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetShippingZoneByZipCodeNew", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.Fill(ds);
                            return ds;
                        }
                    }
                }
            }
        }

        public DataTable BindBoxToShippingzonePickupFee(string PickupFee, int ZoneID)
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("insert into hccBoxtoShippingZoneFee(PickupFee)values(@PickupFee)", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter("select * from hccBoxtoShippingZoneFee", con))
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZoneId", ZoneID);
                        cmd.Parameters.AddWithValue("@PickupFee", PickupFee);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];
                        }
                    }
                }
            }
        }
        public bool UpdatePickupfeecost(int Boxid, decimal price, string Pickfee, int ZoneId)
        {
            try
            {
                string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    //UPDATE Customers SET City = 'Hamburg' WHERE CustomerID = 1;
                    using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[hccBoxtoShippingZoneFee] SET Price=@Price,PickupFee=@PickupFee where ZoneID=@ZoneID and BoxID=@BoxID", con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter("select * from hccBoxtoShippingZoneFee", con))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PickupFee", Pickfee);
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneId);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.Parameters.AddWithValue("@BoxID", Boxid);
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            using (DataSet dt = new DataSet())
                            {
                                sda.Fill(dt);
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public bool UpdatePickupfee(string Pickfee, int ZoneId)
        {
            try
            {
                string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    //UPDATE Customers SET City = 'Hamburg' WHERE CustomerID = 1;
                    using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[hccBoxtoShippingZoneFee] SET PickupFee=@PickupFee where ZoneID=@ZoneID", con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter("select * from hccBoxtoShippingZoneFee", con))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@PickupFee", Pickfee);
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneId);
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            using (DataSet dt = new DataSet())
                            {
                                sda.Fill(dt);
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateIsPickupfee(int IsPickUpShippingZone,int ZoneId)
        {
            try
            {
                string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    //UPDATE Customers SET City = 'Hamburg' WHERE CustomerID = 1;
                    using (SqlCommand cmd = new SqlCommand("UPDATE [dbo].[hccShippingZone] SET IsPickUpShippingZone=@IsPickUpShippingZone where ZoneID=@ZoneID", con))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter("select * from hccShippingZone", con))
                        {
                            //cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IsPickUpShippingZone", IsPickUpShippingZone);
                            cmd.Parameters.AddWithValue("@ZoneID", ZoneId);
                            cmd.Connection = con;
                            sda.SelectCommand = cmd;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            using (DataSet dt = new DataSet())
                            {
                                sda.Fill(dt);
                                return true;
                            }
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public DataTable BindBoxToShippingZoneFee(int ZoneID)
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetBoxShippingZoneFee", con))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ZoneId", ZoneID);
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];
                        }
                    }
                }
            }
        }

        public int UpdateCartItemName(int cartItemId, string CartItemName)
        {
            try
            {
                int userCount = 0;
                string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand cmd = new SqlCommand("update hccCartItems set ItemName=@ItemName where CartItemID=@CartItemID", con);
                    con.Open();
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CartItemID", cartItemId);
                    cmd.Parameters.AddWithValue("@ItemName", CartItemName);
                    userCount = (int)cmd.ExecuteNonQuery();
                }
                return userCount;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Shipping Zones

        #region Bind Dropdown List
        //Shipping Zone DropDown List
        public DataSet ShippingZoneDDL()
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("Select ZoneID,ZoneName FROM hccShippingZone", con);
                //SqlCommand cmd = new SqlCommand("select Id, TypeName from hccShipmentType", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        //ShippingZoneID Dropdown list for Shippingzone
        public DataSet ShippingZoneID()
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("select Id, TypeName from hccShipmentType", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        //Shipping Zip Code DropDown List
        public DataSet ZipCodeDDL()
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("Select ZipZoneID,ZipCode FROM hccZipCodes", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }
        //using (SqlCommand cmd = new SqlCommand("hcc_GetShippingZoneByZipCodeNew", con))
        //{
        //    using (SqlDataAdapter sda = new SqlDataAdapter())
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
        //        cmd.Connection = con;
        //        sda.SelectCommand = cmd;
        //        using (DataSet ds = new DataSet())
        //        {
        //            sda.Fill(ds);
        //            return ds;
        //        }
        //    }
        //}

        public DataSet ZipCodeDDLNew()
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("Select ZipZoneID,ZipCodeFrom,ZipCodeTo FROM hccZipCodes_temp", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                return ds;
            }
        }

        OleDbConnection Econ;
        SqlConnection con;
        string constr, Query, sqlconn;
        private void ExcelConn(string FilePath)
        {
            constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", FilePath);
            Econ = new OleDbConnection(constr);

        }
        private void connection()
        {
            sqlconn = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            con = new SqlConnection(sqlconn);
        }

        public void InsertExcelRecords(string FilePath)
        {
            ExcelConn(FilePath);

            Query = string.Format("Select ZipCode,ZoneID FROM [{0}]", "Sheet1$");

            OleDbCommand Ecom = new OleDbCommand(Query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(Query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable Exceldt = ds.Tables[0];
            connection();
            //creating object of SqlBulkCopy    
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            //assigning Destination table name    
            objbulk.DestinationTableName = "hccZipCodes";
            //Mapping Table column    
            objbulk.ColumnMappings.Add("ZipCode", "ZipCode");
            objbulk.ColumnMappings.Add("ZoneID", "ZoneID");
            //inserting Datatable Records to DataBase    
            con.Open();
            objbulk.WriteToServer(Exceldt);
            con.Close();
        }

        public void InsertExcelDataTemp(DataTable table)
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                //creating object of SqlBulkCopy    
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //assigning Destination table name    
                objbulk.DestinationTableName = "hccZipCodes_temp";
                //Mapping Table column   

                //  objbulk.ColumnMappings.Add("ZipZoneID", "ZipZoneID");
                objbulk.ColumnMappings.Add("ZipCodeFrom", "ZipCodeFrom");
                objbulk.ColumnMappings.Add("ZipCodeTo", "ZipCodeTo");
                objbulk.ColumnMappings.Add("ZoneID", "ZoneID");
                objbulk.ColumnMappings.Add("ShipmentTypeId", "ShipmentTypeId");

                //inserting Datatable Records to DataBase    
                con.Open();
                objbulk.WriteToServer(table);
                con.Close();
            }
        }

        public void InsertExcelDataNew(DataTable table)
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;

            DataTable dt = new DataTable();
            dt.Columns.Add(
                new DataColumn()
                {
                    DataType = System.Type.GetType("System.Int32"),
                    ColumnName = "ZoneID"
                }
                );
            dt.Columns.Add(
new DataColumn()
{
    DataType = System.Type.GetType("System.Int32"),//Single Decimal
    ColumnName = "Cost"
}
);
            dt.Columns.Add(
new DataColumn()
{
    DataType = System.Type.GetType("System.Int32"),
    ColumnName = "BoxID"
}
);
            var distinctValues = table.AsEnumerable()
                        .Select(row => new
                        {
                            ZoneID = row.Field<string>("ZoneID"),
                            Cost = row.Field<string>("Small"),
                            BoxID = row.Field<string>("SmallID"),
                        }).Distinct();

            foreach (var element in distinctValues)
            {
                if (element.ZoneID != "" && element.Cost != "" && element.BoxID != "")
                {
                    var row = dt.NewRow();
                    row["ZoneID"] = element.ZoneID;
                    row["Cost"] = element.Cost;
                    row["BoxID"] = element.BoxID;

                    dt.Rows.Add(row);
                }
            }
            distinctValues = table.AsEnumerable()
                        .Select(row => new
                        {
                            ZoneID = row.Field<string>("ZoneID"),
                            Cost = row.Field<string>("Medium"),
                            BoxID = row.Field<string>("MediumID"),
                        }).Distinct();

            foreach (var element in distinctValues)
            {
                if (element.ZoneID != "" && element.Cost != "" && element.BoxID != "")
                {
                    var row = dt.NewRow();
                    row["ZoneID"] = element.ZoneID;
                    row["Cost"] = element.Cost;
                    row["BoxID"] = element.BoxID;

                    dt.Rows.Add(row); 
                }
            }

            distinctValues = table.AsEnumerable()
                       .Select(row => new
                       {
                           ZoneID = row.Field<string>("ZoneID"),
                           Cost = row.Field<string>("Large"),
                           BoxID = row.Field<string>("LargeID"),
                       }).Distinct();

            foreach (var element in distinctValues)
            {
                if (element.ZoneID != "" && element.Cost != "" && element.BoxID != "")
                {
                    var row = dt.NewRow();
                    row["ZoneID"] = element.ZoneID;
                    row["Cost"] = element.Cost;
                    row["BoxID"] = element.BoxID;

                    dt.Rows.Add(row);
                }
            }

            using (SqlConnection con = new SqlConnection(constr))
            {
                //creating object of SqlBulkCopy    
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                SqlBulkCopy objbulkBox = new SqlBulkCopy(con);

                //assigning Destination table name
                objbulk.DestinationTableName = "hccZipCodestemp";
                objbulkBox.DestinationTableName = "hccBoxtoShippingZoneFee";

                //Mapping Table column   
                //  objbulk.ColumnMappings.Add("ZipZoneID", "ZipZoneID");
                objbulk.ColumnMappings.Add("ZipCode", "ZipCode");
                objbulk.ColumnMappings.Add("ZoneID", "ZoneID");
                objbulk.ColumnMappings.Add("ShippingClass", "ShippingClass");

                objbulkBox.ColumnMappings.Add("ZoneID", "ZoneID");
                objbulkBox.ColumnMappings.Add("Cost", "Cost");
                objbulkBox.ColumnMappings.Add("BoxID", "BoxID");

                //inserting Datatable Records to DataBase    
                con.Open();
                objbulk.WriteToServer(table);
                objbulkBox.WriteToServer(dt);
                con.Close();
            }
        }

        public void InsertExcelData(DataTable table)
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                //creating object of SqlBulkCopy    
                SqlBulkCopy objbulk = new SqlBulkCopy(con);
                //assigning Destination table name    
                objbulk.DestinationTableName = "hccZipCodes";
                //Mapping Table column    
                objbulk.ColumnMappings.Add("ZipZoneID", "ZipZoneID");
                objbulk.ColumnMappings.Add("ZipCode", "ZipCode");
                objbulk.ColumnMappings.Add("ZoneID", "ZoneID");
                //inserting Datatable Records to DataBase    
                con.Open();
                objbulk.WriteToServer(table);
                con.Close();
            }
        }
        #endregion
        #region Shipping Class

        public static int AddUpdateShippingClass(int txtID, string ShippingClassName)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_UpsertShipmentType", conn))
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@Id", txtID);
                            cmd.Parameters.AddWithValue("@TypeName", ShippingClassName);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                    }
                    if (retVal > 0)
                        scope.Complete();
                }

                //if (retVal > 0)
                return retVal;
                //else
                //throw new Exception("A new Box could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }

        public int GetZoneIDByZoneName(string ZoneName)
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("SELECT ZoneID FROM hccShippingZone WHERE ZoneName = @ZoneName", conn))
                        {
                            conn.Open();
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@ZoneName", ZoneName);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                    }
                    if (retVal > 0)
                        scope.Complete();
                }

                //if (retVal > 0)
                return retVal;
                //else
                //throw new Exception("A new Box could not be created.") { Source = "CUSTOM" };
            }
            catch
            {
                throw;
            }
        }

        public DataTable BindGridShippingClass()
        {
            string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("select * from hccShipmentType"))
                {
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        //cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet dt = new DataSet())
                        {
                            sda.Fill(dt);
                            return dt.Tables[0];
                        }
                    }
                }
            }
        }

        public static int DeleteShippingClass(int Id)
        {
            try
            {
                int retVal = 0;
                using (TransactionScope scope = new TransactionScope())
                {
                    string constr = WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_DeleteShipmentType", con))
                        {
                            cmd.Connection = con;
                            con.Open();
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ID", Id);
                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            con.Close();
                            con.Dispose();
                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }

                        if (retVal > 0)
                            scope.Complete();
                    }
                }
                if (retVal > 0)
                    return retVal;
                else
                    throw new Exception("Box could not be deleted.");
            }
            catch
            {
                return 1;
            }
        }
        #endregion
    }

    public class hccZipCodes
    {
        public int ZipZoneID { get; set; }
        public string ZipCode { get; set; }
        public int ZoneID { get; set; }
    }
}
