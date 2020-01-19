using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;


namespace HealthyChef.DAL
{
    public partial class hccRecurringOrder
    {

        /// <summary>
        /// Saves a recurring order record used to identify orders that need to be auto generated upon completion. 
        /// </summary>
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    EntityKey key = cont.CreateEntityKey("hccRecurringOrders", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccRecurringOrders", this);
                    }
                    else
                    {
                        cont.hccRecurringOrders.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<hccRecurringOrder> GetExpiringOrders()
        {
            try
            {
                //var results = new List<hcc_GetExpiringOrders_Result>();
                var results = new List<hccRecurringOrder>();

                using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                {
                    using (var cmd = new SqlCommand("hcc_GetExpiringOrders", conn))
                    {

                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();
                        SqlDataReader t = cmd.ExecuteReader();

                        if (t != null && t.HasRows)
                        {
                            while (t.Read())
                            {
                                //var order = new hcc_GetExpiringOrders_Result
                                var order = new hccRecurringOrder
                                    {
                                        CartID = t.GetInt32(0),
                                        CartItemID = t.GetInt32(1),
                                        UserProfileID = t.GetInt32(2),
                                        //MaxDeliveryDate = t.GetDateTime(2)
                                    };
                                results.Add(order);
                            }

                            t.Close();
                        }
                        conn.Close();
                    }
                }

                return results;
            }
            catch
            {
                throw;
            }
        }


        public static List<hccRecurringOrder> GetTestExpiringOrders()
        {
            var results = new List<hccRecurringOrder>();
            using (var cont = new healthychefEntitiesAPI())
            {
                return results = cont.hccRecurringOrders.ToList();
            }
        }


    }
}
