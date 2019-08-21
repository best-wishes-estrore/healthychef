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
    #region Tables Struct
    public partial struct Tables
    {

        public static string MasterDetailComment = @"MasterDetail_Comments";

        public static string MasterDetailFlagComment = @"MasterDetail_FlagComments";

        public static string MasterDetailItem = @"MasterDetail_Item";

        public static string MasterDetailMiniSummarySetting = @"MasterDetail_MiniSummarySettings";

        public static string MasterDetailSetting = @"MasterDetail_Settings";

    }
    #endregion
    #region Schemas
    public partial class Schemas
    {

        public static TableSchema.Table MasterDetailComment
        {
            get { return DataService.GetSchema("MasterDetail_Comments", "WebModules"); }
        }

        public static TableSchema.Table MasterDetailFlagComment
        {
            get { return DataService.GetSchema("MasterDetail_FlagComments", "WebModules"); }
        }

        public static TableSchema.Table MasterDetailItem
        {
            get { return DataService.GetSchema("MasterDetail_Item", "WebModules"); }
        }

        public static TableSchema.Table MasterDetailMiniSummarySetting
        {
            get { return DataService.GetSchema("MasterDetail_MiniSummarySettings", "WebModules"); }
        }

        public static TableSchema.Table MasterDetailSetting
        {
            get { return DataService.GetSchema("MasterDetail_Settings", "WebModules"); }
        }


    }
    #endregion
    #region View Struct
    public partial struct Views
    {

    }
    #endregion

    #region Query Factories
    public static partial class DB
    {
        public static DataProvider _provider = DataService.Providers["WebModules"];
        static ISubSonicRepository _repository;
        public static ISubSonicRepository Repository
        {
            get
            {
                if (_repository == null)
                    return new SubSonicRepository(_provider);
                return _repository;
            }
            set { _repository = value; }
        }

        public static Select SelectAllColumnsFrom<T>() where T : RecordBase<T>, new()
        {
            return Repository.SelectAllColumnsFrom<T>();

        }
        public static Select Select()
        {
            return Repository.Select();
        }

        public static Select Select(params string[] columns)
        {
            return Repository.Select(columns);
        }

        public static Select Select(params Aggregate[] aggregates)
        {
            return Repository.Select(aggregates);
        }

        public static Update Update<T>() where T : RecordBase<T>, new()
        {
            return Repository.Update<T>();
        }


        public static Insert Insert()
        {
            return Repository.Insert();
        }

        public static Delete Delete()
        {

            return Repository.Delete();
        }

        public static InlineQuery Query()
        {

            return Repository.Query();
        }


    }
    #endregion

}
// Commented out this declaration within all modules because only one can exist globally. (rread 10/27/09)
//#region Databases
//public partial struct Databases 
//{
//	
//	public static string WebModules = @"WebModules";
//  
//}
//#endregion