using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;

namespace HealthyChef.DAL
{
    public partial class healthychefEntitiesAPI
    {
        //private const string ENTITIES_DATA_CONTEXT = "healthychefEntities";

        //private static healthychefEntities _DefaultInstance = null;
        //private static object _DefaultInstanceLock = new object();

        //internal static healthychefEntities Default
        //{
        //    get
        //    {
        //        try
        //        {
        //            lock (_DefaultInstanceLock)
        //            {
        //                if (_DefaultInstance == null)
        //                {
        //                    _DefaultInstance = (healthychefEntities)System.Web.HttpContext.Current.Items[ENTITIES_DATA_CONTEXT];

        //                    if (_DefaultInstance == null)
        //                    {
        //                        string connectionString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["healthychefEntities"].ConnectionString;
        //                        _DefaultInstance = new healthychefEntities(connectionString);
        //                        System.Web.HttpContext.Current.Items.Add(ENTITIES_DATA_CONTEXT, _DefaultInstance);
        //                    }
        //                }
        //            }

        //            return _DefaultInstance;
        //        }
        //        catch
        //        {
        //            throw;
        //        }
        //    }
        //}        
    }
}
