using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

using BayshoreSolutions.WebModules;
using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.Cms.FileManager.Model
{
    public partial class FileList_Module
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

        public string GetFullVirtualRootPath()
        {
            return Bss.Web.Url.Combine(Settings.FileStorageRootPath, this.RootPath);
        }
    }

    public partial class FileList_ModuleCollection
    {
    }
}
