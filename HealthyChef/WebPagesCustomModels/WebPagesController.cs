using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using HealthyChef.Common;
using System.ComponentModel;
using BayshoreSolutions.WebModules;
using System.Configuration;

namespace HealthyChef.WebPagesCustomModels
{
    public class WebPagesController
    {

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static List<WebpageInfo> GetWebpages(int parentNavigationId)
        {
            List<int> parentNavigationIdslist = new List<int>();
            List<WebpageInfo> webpageInfoList1 = new List<WebpageInfo>();
            var _siteMapObj = new BayshoreSolutions.WebModules.SiteMapProvider();
            //string parentNavigationIds = "2,3,4,5,6,8,10,11,19,22,36,39,45,47,56,57,67,2081,2092";
            //string[] parentNavigationIdsstringarray;
            //if (parentNavigationId == 2)
            //{
            //    parentNavigationIdsstringarray = parentNavigationIds.Split(',');
            //}
            //else
            //{
            //    parentNavigationIdsstringarray = new string[] { parentNavigationId.ToString() };
            //}
            //foreach (var NavigationId in parentNavigationIdsstringarray)
            //{
            //    parentNavigationIdslist.Add(Convert.ToInt16(NavigationId));
            //}

            //get parentNavigationIds from DB
            if (parentNavigationId == 2)
            {

                string connStr = ConfigurationManager.ConnectionStrings["WebModules"].ConnectionString;
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    using (SqlCommand cmd = new SqlCommand("select DISTINCT(ParentNavigationId) from [dbo].[WebModules_Navigation] where ParentNavigationId is not null and ParentNavigationId <> 1", conn))
                    {
                        cmd.CommandTimeout = 180;
                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            parentNavigationIdslist.Add(Convert.ToInt32(t[0]));
                        }

                        conn.Close();
                    }
                }
            }
            else
            {
                parentNavigationIdslist.Add(parentNavigationId);
            }

            foreach (int NavigationId in parentNavigationIdslist)
            {
                foreach (WebpageInfo webpage in Webpage.GetWebpages(NavigationId))
                {
                    if (!webpageInfoList1.Contains(webpage))
                        webpageInfoList1.Add(webpage);
                }
            }

            return webpageInfoList1;
        }
    }
}