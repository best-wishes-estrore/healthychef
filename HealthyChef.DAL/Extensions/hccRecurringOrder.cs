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
                using (var cont = new healthychefEntities())
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

                using (var conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
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
            using (var cont = new healthychefEntities())
            {
                return results = cont.hccRecurringOrders.ToList();
            }
        }
        public static hccRecurringOrder GetByCartItemId(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccRecurringOrders
                        .SingleOrDefault(i => i.CartItemID == cartItemId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccRecurringOrders", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        hccRecurringOrder item = cont.hccRecurringOrders
                            .Where(a => a.CartItemID == this.CartItemID).SingleOrDefault();

                        if (item != null)
                        {
                            cont.hccRecurringOrders.DeleteObject(item);
                            cont.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void DeleteByCartId(int CartId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    hccRecurringOrder item = cont.hccRecurringOrders
                            .Where(a => a.CartID == CartId).SingleOrDefault();
                    if (item != null)
                    {
                        cont.hccRecurringOrders.DeleteObject(item);
                        cont.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
