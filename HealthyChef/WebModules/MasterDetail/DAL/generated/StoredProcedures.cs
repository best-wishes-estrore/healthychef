using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Xml;
using System.Xml.Serialization;
using SubSonic;
using SubSonic.Utilities;
namespace BayshoreSolutions.WebModules.MasterDetail
{
    public partial class SPs
    {

        /// <summary>
        /// Creates an object wrapper for the MasterDetail_GetCommentFlags Procedure
        /// </summary>
        public static StoredProcedure MasterDetailGetCommentFlags(string IdList, int? MinFlagCount, int? SortByNumFlags)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("MasterDetail_GetCommentFlags", DataService.GetInstance("WebModules"), "dbo");

            sp.Command.AddParameter("@IdList", IdList, DbType.AnsiString, null, null);

            sp.Command.AddParameter("@MinFlagCount", MinFlagCount, DbType.Int32, 0, 10);

            sp.Command.AddParameter("@SortByNumFlags", SortByNumFlags, DbType.Int32, 0, 10);

            return sp;
        }

        /// <summary>
        /// Creates an object wrapper for the MasterDetail_GetRecentItems Procedure
        /// </summary>
        public static StoredProcedure MasterDetailGetRecentItems(string pageIdList, bool? featuredOnly, int? maxRecCount)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("MasterDetail_GetRecentItems", DataService.GetInstance("WebModules"), "dbo");

            sp.Command.AddParameter("@pageIdList", pageIdList, DbType.AnsiString, null, null);

            sp.Command.AddParameter("@featuredOnly", featuredOnly, DbType.Boolean, null, null);

            sp.Command.AddParameter("@maxRecCount", maxRecCount, DbType.Int32, 0, 10);

            return sp;
        }

    }

}
