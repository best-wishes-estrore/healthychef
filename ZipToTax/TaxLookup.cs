using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZipToTaxService
{
    public class TaxLookup
    {
        public string Zip_Code { get; set; }
        public string Sales_Tax_Rate { get; set; }
        public string Post_Office_City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Shipping_Taxable { get; set; }


        public static TaxLookup RequestTax(string zipCode)
        {
            ZipToTaxConfig z2tConfig = new ZipToTaxConfig();

            using (SqlConnection cn = new SqlConnection(z2tConfig.ConnString))
            {
                //string zipCode = "90210"; //sample zip code must be between 90001 and 90999


                string cmdText = "exec z2t_lookup '" + zipCode + "', '" + z2tConfig.Settings.LoginUserName +
                    "', '" + z2tConfig.Settings.LoginUserPassword + "'";

                SqlCommand cmd = new SqlCommand(cmdText, cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                rdr.Read();

                TaxLookup response = new TaxLookup
                {
                    County = rdr["County"].ToString(),
                    Post_Office_City = rdr["Post_Office_City"].ToString(),
                    Sales_Tax_Rate = rdr["Sales_Tax_Rate"].ToString(),
                    Shipping_Taxable = rdr["Shipping_Taxable"].ToString(),
                    State = rdr["State"].ToString(),
                    Zip_Code = rdr["Zip_Code"].ToString()
                };

                rdr.Close();

                return response;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Zip Code: " + this.County + "<br />");
            sb.Append("Sales Tax Rate: " + this.Sales_Tax_Rate + "<br />");
            sb.Append("Post Office City: " + this.Post_Office_City  + "<br />");
            sb.Append("County: " + this.County  + "<br />");
            sb.Append("State: " + this.State  + "<br />");
            sb.Append("Shipping Taxable: " + this.Shipping_Taxable + "<br />");
            return sb.ToString();
        }

    }
}
