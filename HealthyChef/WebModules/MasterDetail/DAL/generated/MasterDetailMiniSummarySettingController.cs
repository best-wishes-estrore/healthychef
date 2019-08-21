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
    /// <summary>
    /// Controller class for MasterDetail_MiniSummarySettings
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MasterDetailMiniSummarySettingController
    {
        // Preload our schema..
        MasterDetailMiniSummarySetting thisSchemaLoad = new MasterDetailMiniSummarySetting();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
                if (userName.Length == 0)
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        userName = System.Web.HttpContext.Current.User.Identity.Name;
                    }
                    else
                    {
                        userName = System.Threading.Thread.CurrentPrincipal.Identity.Name;
                    }
                }
                return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public MasterDetailMiniSummarySettingCollection FetchAll()
        {
            MasterDetailMiniSummarySettingCollection coll = new MasterDetailMiniSummarySettingCollection();
            Query qry = new Query(MasterDetailMiniSummarySetting.Schema);
            // Begin Bayshore custom code block (rread 6/26/07)
            // Ignore records marked for deletion when doing a FetchAll
            if (MasterDetailMiniSummarySetting.Schema.GetColumn("IsDeleted") != null)
            {
                qry.WHERE("IsDeleted <> true");
            }
            else if (MasterDetailMiniSummarySetting.Schema.GetColumn("Deleted") != null)
            {
                qry.WHERE("Deleted <> true");
            }
            // End Bayshore custom code block

            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailMiniSummarySettingCollection FetchByID(object ModuleId)
        {
            MasterDetailMiniSummarySettingCollection coll = new MasterDetailMiniSummarySettingCollection().Where("ModuleId", ModuleId).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailMiniSummarySettingCollection FetchByQuery(Query qry)
        {
            MasterDetailMiniSummarySettingCollection coll = new MasterDetailMiniSummarySettingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ModuleId)
        {
            return (MasterDetailMiniSummarySetting.Destroy(ModuleId) == 1);
        }


        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ModuleId)
        {
            return (MasterDetailMiniSummarySetting.Delete(ModuleId) == 1);
        }


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(int ModuleId, int? StartingPageId, int? NumRows, bool? ShowElapsedTime, bool? FeaturedOnly)
        {
            MasterDetailMiniSummarySetting item = new MasterDetailMiniSummarySetting();

            item.ModuleId = ModuleId;

            item.StartingPageId = StartingPageId;

            item.NumRows = NumRows;

            item.ShowElapsedTime = ShowElapsedTime;

            item.FeaturedOnly = FeaturedOnly;

            item.CreatedOn = DateTime.Now;

            item.CreatedBy = UserName;


            item.Save(UserName);
        }
        // Begin Bayshore custom code block (rread 7/16/07)
        // Insert a record and return the Identity.
        /// <summary>
        /// Inserts a record and returns the Identity, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void InsertAndReturnIdentity(int ModuleId, int? StartingPageId, int? NumRows, bool? ShowElapsedTime, bool? FeaturedOnly, out object newId)
        {
            MasterDetailMiniSummarySetting item = new MasterDetailMiniSummarySetting();

            item.ModuleId = ModuleId;

            item.StartingPageId = StartingPageId;

            item.NumRows = NumRows;

            item.ShowElapsedTime = ShowElapsedTime;

            item.FeaturedOnly = FeaturedOnly;

            item.CreatedOn = DateTime.Now;

            item.CreatedBy = UserName;


            item.Save(UserName);

            newId = item.ModuleId;

        }
        // End Bayshore custom code block

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(int ModuleId, int? StartingPageId, int? NumRows, bool? ShowElapsedTime, bool? FeaturedOnly)
        {
            MasterDetailMiniSummarySetting item = new MasterDetailMiniSummarySetting();
            item.MarkOld();
            item.IsLoaded = true;

            item.ModuleId = ModuleId;

            item.StartingPageId = StartingPageId;

            item.NumRows = NumRows;

            item.ShowElapsedTime = ShowElapsedTime;

            item.FeaturedOnly = FeaturedOnly;

            item.ModifiedOn = DateTime.Now;

            item.ModifiedBy = UserName;

            item.Save(UserName);
        }
    }
}
