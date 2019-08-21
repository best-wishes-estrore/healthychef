using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Configuration;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public class hccPurchaseNumber
    {
        // deliberately not part of healthychef context as was causing problems with concurrency
        public static int AddNew()
        {
            try
            {
                int retVal = 0;

                using (TransactionScope scope = new TransactionScope())
                {                    
                    using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand("hcc_AddPurchaseNumber", conn))
                        {
                            conn.Open();

                            SqlParameter prm = new SqlParameter("@DateIssued", SqlDbType.DateTime);
                            prm.Value = DateTime.Now;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add(prm);

                            object t = cmd.ExecuteScalar();
                            cmd.Dispose();
                            conn.Close();
                            conn.Dispose();

                            if (t != null)
                                retVal = int.Parse(t.ToString());
                        }                        
                    }
                    scope.Complete();
                }

                if (retVal > 0)
                    return retVal;
                else
                    throw new Exception("A new purchase number could not be created.");
            }
            catch
            {
                throw;
            }
        }
    }
}
