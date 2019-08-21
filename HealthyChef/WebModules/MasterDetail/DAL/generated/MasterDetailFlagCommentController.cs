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
    /// Controller class for MasterDetail_FlagComments
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MasterDetailFlagCommentController
    {
        // Preload our schema..
        MasterDetailFlagComment thisSchemaLoad = new MasterDetailFlagComment();
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
        public MasterDetailFlagCommentCollection FetchAll()
        {
            MasterDetailFlagCommentCollection coll = new MasterDetailFlagCommentCollection();
            Query qry = new Query(MasterDetailFlagComment.Schema);
            // Begin Bayshore custom code block (rread 6/26/07)
            // Ignore records marked for deletion when doing a FetchAll
            if (MasterDetailFlagComment.Schema.GetColumn("IsDeleted") != null)
            {
                qry.WHERE("IsDeleted <> true");
            }
            else if (MasterDetailFlagComment.Schema.GetColumn("Deleted") != null)
            {
                qry.WHERE("Deleted <> true");
            }
            // End Bayshore custom code block

            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailFlagCommentCollection FetchByID(object Id)
        {
            MasterDetailFlagCommentCollection coll = new MasterDetailFlagCommentCollection().Where("Id", Id).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailFlagCommentCollection FetchByQuery(Query qry)
        {
            MasterDetailFlagCommentCollection coll = new MasterDetailFlagCommentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MasterDetailFlagComment.Destroy(Id) == 1);
        }


        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MasterDetailFlagComment.Delete(Id) == 1);
        }


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(long CommentId, string IPAddress, string Reason)
        {
            MasterDetailFlagComment item = new MasterDetailFlagComment();

            item.CommentId = CommentId;

            item.IPAddress = IPAddress;

            item.Reason = Reason;

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
        public void InsertAndReturnIdentity(long CommentId, string IPAddress, string Reason, out object newId)
        {
            MasterDetailFlagComment item = new MasterDetailFlagComment();

            item.CommentId = CommentId;

            item.IPAddress = IPAddress;

            item.Reason = Reason;

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
        public void Update(long Id, long CommentId, string IPAddress, string Reason)
        {
            MasterDetailFlagComment item = new MasterDetailFlagComment();
            item.MarkOld();
            item.IsLoaded = true;

            item.Id = Id;

            item.CommentId = CommentId;

            item.IPAddress = IPAddress;

            item.Reason = Reason;

            item.Save(UserName);
        }
    }
}
