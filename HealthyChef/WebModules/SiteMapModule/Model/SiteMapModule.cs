using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;

namespace BayshoreSolutions.WebModules.SiteMapModule.Model
{
    public partial class SiteMapModule
    {
        private static readonly string _connectionString = BayshoreSolutions.WebModules.Settings.ConnectionString;

        /// <summary>Saves the entity to the information store.</summary>
        public int Save()
        {
            //pre-save code...

            //call the private Save_ function
            return Save_(this);

            //post-save code...
        }

        /// <summary>Physically deletes the specified entity from the information store.</summary>
        public static void Destroy(
            int moduleId
        )
        {
            //pre-destroy code...

            //call the private Destroy_ function
            Destroy_(
                moduleId
            );

            //post-destroy code...
        }
    }

    public partial class SiteMapModuleCollection
    {
    }
}
