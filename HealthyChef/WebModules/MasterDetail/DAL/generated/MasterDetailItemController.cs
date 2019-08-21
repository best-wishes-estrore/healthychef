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
    /// Controller class for MasterDetail_Item
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MasterDetailItemController
    {
        // Preload our schema..
        MasterDetailItem thisSchemaLoad = new MasterDetailItem();
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
        public MasterDetailItemCollection FetchAll()
        {
            MasterDetailItemCollection coll = new MasterDetailItemCollection();
            Query qry = new Query(MasterDetailItem.Schema);
            // Begin Bayshore custom code block (rread 6/26/07)
            // Ignore records marked for deletion when doing a FetchAll
            if (MasterDetailItem.Schema.GetColumn("IsDeleted") != null)
            {
                qry.WHERE("IsDeleted <> true");
            }
            else if (MasterDetailItem.Schema.GetColumn("Deleted") != null)
            {
                qry.WHERE("Deleted <> true");
            }
            // End Bayshore custom code block

            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailItemCollection FetchByID(object Id)
        {
            MasterDetailItemCollection coll = new MasterDetailItemCollection().Where("Id", Id).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailItemCollection FetchByQuery(Query qry)
        {
            MasterDetailItemCollection coll = new MasterDetailItemCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MasterDetailItem.Destroy(Id) == 1);
        }


        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MasterDetailItem.Delete(Id) == 1);
        }


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(int ModuleId, string Culture, int StatusId, string ShortDescription, string LongDescription, string ImagePath, bool IsFeatured, string Tags)
        {
            MasterDetailItem item = new MasterDetailItem();

            item.ModuleId = ModuleId;

            item.Culture = Culture;

            item.StatusId = StatusId;

            item.ShortDescription = ShortDescription;

            item.LongDescription = LongDescription;

            item.ImagePath = ImagePath;

            item.IsFeatured = IsFeatured;

            item.Tags = Tags;

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
        public void InsertAndReturnIdentity(int ModuleId, string Culture, int StatusId, string ShortDescription, string LongDescription, string ImagePath, bool IsFeatured, string Tags, out object newId)
        {
            MasterDetailItem item = new MasterDetailItem();

            item.ModuleId = ModuleId;

            item.Culture = Culture;

            item.StatusId = StatusId;

            item.ShortDescription = ShortDescription;

            item.LongDescription = LongDescription;

            item.ImagePath = ImagePath;

            item.IsFeatured = IsFeatured;

            item.Tags = Tags;

            item.CreatedOn = DateTime.Now;

            item.CreatedBy = UserName;


            item.Save(UserName);

            newId = item.Id;

        }
        // End Bayshore custom code block

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
        public void Update(int Id, int ModuleId, string Culture, int StatusId, string ShortDescription, string LongDescription, string ImagePath, bool IsFeatured, string Tags)
        {
            MasterDetailItem item = new MasterDetailItem();
            item.MarkOld();
            item.IsLoaded = true;

            item.Id = Id;

            item.ModuleId = ModuleId;

            item.Culture = Culture;

            item.StatusId = StatusId;

            item.ShortDescription = ShortDescription;

            item.LongDescription = LongDescription;

            item.ImagePath = ImagePath;

            item.IsFeatured = IsFeatured;

            item.Tags = Tags;

            item.Save(UserName);
        }
    }
}
