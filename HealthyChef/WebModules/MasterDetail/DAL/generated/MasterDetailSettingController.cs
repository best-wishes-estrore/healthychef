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
    /// Controller class for MasterDetail_Settings
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MasterDetailSettingController
    {
        // Preload our schema..
        MasterDetailSetting thisSchemaLoad = new MasterDetailSetting();
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
        public MasterDetailSettingCollection FetchAll()
        {
            MasterDetailSettingCollection coll = new MasterDetailSettingCollection();
            Query qry = new Query(MasterDetailSetting.Schema);
            // Begin Bayshore custom code block (rread 6/26/07)
            // Ignore records marked for deletion when doing a FetchAll
            if (MasterDetailSetting.Schema.GetColumn("IsDeleted") != null)
            {
                qry.WHERE("IsDeleted <> true");
            }
            else if (MasterDetailSetting.Schema.GetColumn("Deleted") != null)
            {
                qry.WHERE("Deleted <> true");
            }
            // End Bayshore custom code block

            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailSettingCollection FetchByID(object ModuleId)
        {
            MasterDetailSettingCollection coll = new MasterDetailSettingCollection().Where("ModuleId", ModuleId).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailSettingCollection FetchByQuery(Query qry)
        {
            MasterDetailSettingCollection coll = new MasterDetailSettingCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ModuleId)
        {
            return (MasterDetailSetting.Destroy(ModuleId) == 1);
        }


        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ModuleId)
        {
            return (MasterDetailSetting.Delete(ModuleId) == 1);
        }


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(int ModuleId, bool IsPostDateVisible, int ItemsPerPage, bool RequireAuthentication, bool AllowComments, bool ShowTagFilter, bool ShowImageIfBlank, string Template, bool PageVisibleInNavigation)
        {
            MasterDetailSetting item = new MasterDetailSetting();

            item.ModuleId = ModuleId;

            item.IsPostDateVisible = IsPostDateVisible;

            item.ItemsPerPage = ItemsPerPage;

            item.RequireAuthentication = RequireAuthentication;

            item.AllowComments = AllowComments;

            item.ShowTagFilter = ShowTagFilter;

            item.ShowImageIfBlank = ShowImageIfBlank;

            item.Template = Template;

            item.PageVisibleInNavigation = PageVisibleInNavigation;


            item.Save(UserName);
        }
        // Begin Bayshore custom code block (rread 7/16/07)
        // Insert a record and return the Identity.
        /// <summary>
        /// Inserts a record and returns the Identity, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
        public void InsertAndReturnIdentity(int ModuleId, bool IsPostDateVisible, int ItemsPerPage, bool RequireAuthentication, bool AllowComments, bool ShowTagFilter, bool ShowImageIfBlank, string Template, bool PageVisibleInNavigation, out object newId)
        {
            MasterDetailSetting item = new MasterDetailSetting();

            item.ModuleId = ModuleId;

            item.IsPostDateVisible = IsPostDateVisible;

            item.ItemsPerPage = ItemsPerPage;

            item.RequireAuthentication = RequireAuthentication;

            item.AllowComments = AllowComments;

            item.ShowTagFilter = ShowTagFilter;

            item.ShowImageIfBlank = ShowImageIfBlank;

            item.Template = Template;

            item.PageVisibleInNavigation = PageVisibleInNavigation;


            item.Save(UserName);

            newId = item.ModuleId;

        }
        // End Bayshore custom code block

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(int ModuleId, bool IsPostDateVisible, int ItemsPerPage, bool RequireAuthentication, bool AllowComments, bool ShowTagFilter, bool ShowImageIfBlank, string Template, bool PageVisibleInNavigation)
        {
            MasterDetailSetting item = new MasterDetailSetting();
            item.MarkOld();
            item.IsLoaded = true;

            item.ModuleId = ModuleId;

            item.IsPostDateVisible = IsPostDateVisible;

            item.ItemsPerPage = ItemsPerPage;

            item.RequireAuthentication = RequireAuthentication;

            item.AllowComments = AllowComments;

            item.ShowTagFilter = ShowTagFilter;

            item.ShowImageIfBlank = ShowImageIfBlank;

            item.Template = Template;

            item.PageVisibleInNavigation = PageVisibleInNavigation;

            item.Save(UserName);
        }
    }
}
