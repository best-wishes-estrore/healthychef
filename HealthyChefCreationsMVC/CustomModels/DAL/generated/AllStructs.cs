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
namespace BayshoreSolutions.WebModules.QuickContent
{
    #region Tables Struct
    public partial struct Tables
    {

        public static string QuickContentContent = @"QuickContent_Content";

    }
    #endregion
    #region Schemas
    public partial class Schemas
    {

        public static TableSchema.Table QuickContentContent
        {
            get { return DataService.GetSchema("QuickContent_Content", "WebModules"); }
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