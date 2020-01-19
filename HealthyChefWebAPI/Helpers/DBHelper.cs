using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace HealthyChefWebAPI.Helpers
{
    public class DBHelper
    {
        
        public static string SaveErrorData(string pErrorDesc, DateTime pErrorTime)
        {
            try
            {
                Hashtable htData = new Hashtable();
                htData.Add("@ErrorMessage", pErrorDesc);
                htData.Add("@ErrorTime", pErrorTime);
                dynamic intStatus = DBHelper.ExcuteSelectCommand("insert into ErrorLog ([ErrorDesc],[ErrorTime]) values(@ErrorMessage,@ErrorTime)", htData, false);
            }
            catch (Exception)
            {

                return "";
            }

            return "";
        }

        public static HttpClient CreateHttpReq()
        {

            HttpClient client = new HttpClient();
            client.Timeout = new TimeSpan(0, 4, 0);
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["webApiUrl"]);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
        
        public static string ConvertDataToJson(Object objectToConvertToJson)
        {
            string JsonDate = "";
            try
            {
                JsonDate = Newtonsoft.Json.JsonConvert.SerializeObject(objectToConvertToJson);
            }
            catch (Exception E)
            {


            }

            return JsonDate;
        }
        /// <summary>
        /// Get Connection
        /// </summary>
        /// <returns>SqlConnection</returns>     

        public static SqlConnection GetDBConnection()
        {
            String strConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(strConnectionString);
            return connection;
        }
        


        public static object ExecuteScalarObject(string Command__1, Hashtable parms, bool isStoredProcedure = false)
        {
            object intID = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command__2 = new SqlCommand(Command__1, connection);
                if (parms != null && parms.Count > 0)
                {
                    command__2.Parameters.AddRange(GetParameters(parms));
                }
                if (isStoredProcedure == true)
                {
                    command__2.CommandType = CommandType.StoredProcedure;
                }
                connection.Open();
                intID = command__2.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
                String strMsg = ex.Message;
                //ErrorLog.WriteErrorLog(ex.Message);
            }
            return intID;
        }

        public static int ExecuteScalar(string Command__1, Hashtable parms, bool isStoredProcedure = false)
        {
            int intID = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command__2 = new SqlCommand(Command__1, connection);
                if (parms != null && parms.Count > 0)
                {
                    command__2.Parameters.AddRange(GetParameters(parms));
                }
                if (isStoredProcedure == true)
                {
                    command__2.CommandType = CommandType.StoredProcedure;
                }
                connection.Open();
                intID = Convert.ToInt32(command__2.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                String strMsg = ex.Message;
                //ErrorLog.WriteErrorLog(ex.Message);
            }
            return intID;
        }
        /// <summary>
        /// Method For Insert Command
        /// </summary>
        /// <param name="Command">Sql Command</param>
        /// <param name="parms">Parameters</param>
        /// <returns>int</returns>
        public static int ExcuteInsertCommand(string Command, Hashtable parms)
        {
            int id = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }

                connection.Open();
                //connection.OpenWithRetry(GetRetryPolicy());
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
            }
            return id;
        }
        public static bool HasColumn(IDataReader Reader, string ColumnName)
        {
            foreach (DataRow row in Reader.GetSchemaTable().Rows)
            {
                if (row["ColumnName"].ToString() == ColumnName)
                    return true;
            }
            return false;
        }
        public static int ExcuteSelectCommand(string Command, Hashtable parms, bool isStoredProcedure = false)
        {
            int id = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                if (isStoredProcedure == true)
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                connection.Open();
                // connection.OpenWithRetry(GetRetryPolicy());
                id = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
            }
            return id;
        }
        public static string ExcuteSelectCommand(Hashtable parms, string Command, bool isStoredProcedure = false)
        {
            string id = "";
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                if (isStoredProcedure == true)
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                connection.Open();
                //connection.OpenWithRetry(GetRetryPolicy());
                id = Convert.ToString(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        public static int ExecuteReader(string Command, Hashtable parms, bool isStoredProcedure = false)
        {
            int id = 0;
            try
            {
                DataSet ds = new DataSet();
                SqlDataReader dr;
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                SqlDataAdapter ad = new SqlDataAdapter(command);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                if (isStoredProcedure == true)
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                connection.Open();
                //connection.OpenWithRetry(GetRetryPolicy());
                ad.Fill(ds);

                connection.Close();
            }
            catch (Exception ex)
            {
            }
            return id;
        }
        /// <summary>
        /// Method For Insert Command By Stored Procedure
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="parms"></param>
        /// <param name="isStoredProcedure"></param>
        /// <returns>int</returns>
        public static int ExcuteInsertCommand(string Command, Hashtable parms, bool isStoredProcedure)
        {
            int id = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (isStoredProcedure == true)
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                connection.Open();
                //connection.OpenWithRetry(GetRetryPolicy());
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {


                string value = "";
            }
            return id;
        }

        /// <summary>
        /// Method For Update Command
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="parms"></param>
        /// <returns>int</returns>
        public static int ExcuteUpadateCommand(string Command, Hashtable parms)
        {
            int id = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                connection.Open();
                // connection.OpenWithRetry(GetRetryPolicy());
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Method For Update Command By Stored Procedure
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="parms"></param>
        /// <param name="IsStoredProcedure"></param>
        /// <returns>int</returns>
        public static int ExcuteUpadateCommand(string Command, Hashtable parms, bool IsStoredProcedure)
        {
            int id = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (IsStoredProcedure == true)
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                connection.Open();
                //connection.OpenWithRetry(GetRetryPolicy());
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
            }
            return id;
        }

        /// <summary>
        /// Method For Delete Command
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="parms"></param>
        /// <returns>int</returns>
        public static int ExcuteDeleteCommand(string Command, Hashtable parms)
        {
            int id = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                connection.Open();
                //connection.OpenWithRetry(GetRetryPolicy());
                id = command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex)
            {
                string strMsg = ex.Message;
            }
            return id;
        }

        /// <summary>
        /// Method For Select Command That Return Id
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static int ExecuteSelectCommandReturnID(string Command, Hashtable parms)
        {
            int intID = 0;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                connection.Open();
                //connection.OpenWithRetry(GetRetryPolicy());
                intID = Convert.ToInt32(command.ExecuteScalar());
                connection.Close();
            }
            catch (Exception ex)
            {
                String strMsg = ex.Message;
            }
            return intID;
        }

        /// <summary>
        /// Method For Select Command That Return Id
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static object ExecuteSelectCommandReturnObject(string Command, Hashtable parms, bool IsStoredProcedure)
        {
            object objOutput = null;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (IsStoredProcedure == true)
                {
                    command.CommandType = CommandType.StoredProcedure;
                }
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                // connection.OpenWithRetry(GetRetryPolicy());
                connection.Open();
                objOutput = command.ExecuteScalar();

                connection.Close();
            }
            catch (Exception ex)
            {
                String strMsg = ex.Message;
            }
            return objOutput;
        }
        public static object ExecuteSelectCommandReturnObject(string Command, Hashtable parms)
        {
            object objOutput = null;
            try
            {
                SqlConnection connection = GetDBConnection();
                SqlCommand command = new SqlCommand(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    command.Parameters.AddRange(GetParameters(parms));
                }
                connection.Open();
                // connection.OpenWithRetry(GetRetryPolicy());
                objOutput = command.ExecuteScalar();

                connection.Close();
            }
            catch (Exception ex)
            {
                String strMsg = ex.Message;
            }
            return objOutput;
        }

        /// <summary>
        /// Method For Select Command
        /// </summary>
        /// <param name="Command"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public static DataSet ExecuteSelectCommand(string Command, Hashtable parms)
        {
            DataSet dsOutput = new DataSet();
            try
            {
                System.Data.SqlClient.SqlConnection connection = GetDBConnection();
                SqlDataAdapter Sda = new SqlDataAdapter(Command, connection);
                if (parms != null && parms.Count > 0)
                {
                    Sda.SelectCommand.Parameters.AddRange(GetParameters(parms));
                }
                Sda.Fill(dsOutput);
            }
            catch (Exception ex)
            {
                String strMsg = ex.Message;
            }
            return dsOutput;
        }

        /// <summary>
        /// Get Parameters
        /// </summary>
        /// <param name="parms">Parameters List</param>
        /// <returns>SqlParameter[]</returns>
        public static SqlParameter[] GetParameters(Hashtable parms)
        {
            SqlParameter[] sqldbparms = new SqlParameter[parms.Count];
            SqlParameter obj;
            List<String> keys = parms.Keys.OfType<String>().ToList();
            for (int intLoop = 0; intLoop < parms.Count; intLoop++)
            {
                obj = new SqlParameter(keys[intLoop], parms[keys[intLoop]]);
                switch (parms[keys[intLoop]].GetType().Name)
                {
                    case "Int32":
                        obj.SqlDbType = SqlDbType.Int;
                        break;
                    case "NVarChar":
                        obj.SqlDbType = SqlDbType.NVarChar;
                        break;
                    case "Int64":
                        obj.SqlDbType = SqlDbType.Int;
                        break;
                    case "DateTime":
                        obj.SqlDbType = SqlDbType.DateTime;// OleDbType.Date ;           
                        break;
                    case "GUID":
                        obj.SqlDbType = SqlDbType.UniqueIdentifier;// OleDbType.Date ;           
                        break;
                }
                sqldbparms[intLoop] = obj;
            }
            return sqldbparms;
        }


    }
   

    /// <summary>
    /// Helps to wrap database result set row to C# object. It has methods which read value from the data REader object put those values
    /// to C# object with appropriate type.
    /// </summary>
    public class DBUtil
    {
        /// <summary>
        /// The class constructor.
        /// </summary>
        public DBUtil()
        {

        }

        /// <summary>
        /// Gets Int32 value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Integer value or MinValue if it is null in the database.</returns>
        public static int GetIntField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (int)dr[field];
            else
                //return int.MinValue;
                return 0;
        }

        /// <summary>
        /// Gets Int64 value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Long  value or MinValue if it is null in the database.</returns>
        public static long GetLongField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (long)dr[field];
            else
                return 0;
        }

        /// <summary>
        /// Gets string value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>String value or MinValue if it is null in the database.</returns>
        public static string GetCharField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (string)dr[field];
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Gets boolean value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Boolean value or MinValue if it is null in the database.</returns>
        public static bool GetBoolField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)

                if (dr[field].ToString() == "0")
                    return false;
                else if (dr[field].ToString() == "1")
                    return true;
                else
                    return (bool)(dr[field]);

            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets byte value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Byte value or MinValue if it is null in the database.</returns>
        public static byte GetByteField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (byte)dr[field];
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets byte array value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Byte array value or MinValue if it is null in the database.</returns>
        public static byte[] GetBinaryField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (byte[])dr[field];
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets decimal value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Decimal value or MinValue if it is null in the database.</returns>
        public static decimal GetDecimalField(IDataReader dr, string field)
        {
            try
            {


                if (dr[field] != DBNull.Value)
                {
                    string str = dr[field].ToString();
                    return decimal.Parse(str.Replace(",", "."), new NumberFormatInfo());
                }
                else
                {
                    //return decimal.MinValue;
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Gets double value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Double value or MinValue if it is null in the database.</returns>
        public static double GetDoubleField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (double)dr[field];
            else
                return double.MinValue;
        }

        /// <summary>
        /// Gets float value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Float value or MinValue if it is null in the database.</returns>
        public static float GetFloatField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (float)dr[field];
            else
                return float.MinValue;
        }

        /// <summary>
        /// Gets datime value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>DateTime value or MinValue if it is null in the database.</returns>
        public static DateTime GetDateTimeField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (DateTime)dr[field];
            else
            {
                // http://blog.benpowell.co.uk/2011/09/wcf-json-serialization-error-with.html
                DateTime date = DateTime.MinValue;
                date = DateTime.SpecifyKind(date, DateTimeKind.Utc);
                return date;
            }
        }

        public static DateTime? GetNullableDateTimeField(IDataReader dr, string field)
        {
            if (dr[field] != DBNull.Value)
                return (DateTime)dr[field];
            else
            {
                return null;
            }
        }

        public static string GetDateField(IDataReader dr, string field)
        {
            var _dateToReturn = DateTime.MinValue;
            if (dr[field] != DBNull.Value)
                _dateToReturn = (DateTime)dr[field];
            else
            {
                return null;
            }

            return _dateToReturn.ToString("yyyy.MM.dd");
        }

        /// <summary>
        /// Gets Int16 value from database field.
        /// </summary>
        /// <param name="dr">DataReader object which represents current resultset row.</param>
        /// <param name="field">Field name.</param>
        /// <returns>Small integer value or MinValue if it is null in the database.</returns>
        public static Int16 GetSmallIntField(IDataReader dr, string field)
        {
            object o = dr[field];
            return (o != DBNull.Value) ? (Int16)o : Int16.MinValue;
        }

        public static Guid GetGuidField(IDataReader dr, string field)
        {
            var _newGuid = new Guid();
            try
            {
                if (dr[field] != DBNull.Value)
                {
                    _newGuid = new Guid(dr[field].ToString());
                }
                else
                {                    
                    return _newGuid;
                }
            }
            catch
            {
                return _newGuid;
            }
            return _newGuid;
        }

        public static string GetConnectionString()
        {
            string _connString = string.Empty;

            string txtpath = System.Web.Hosting.HostingEnvironment.MapPath("~/App_Data/Connectionstring.txt");
            //string txtpath = Server @"~/Connectionstring.txt";
            try
            {
                if (File.Exists(txtpath))
                {
                    _connString = System.IO.File.ReadAllText(txtpath);
                }
            }
            catch (Exception e)
            {
                return _connString;
            }
            return _connString;
        }
    }
}