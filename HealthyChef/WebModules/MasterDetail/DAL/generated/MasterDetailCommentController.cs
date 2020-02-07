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
    /// Controller class for MasterDetail_Comments
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class MasterDetailCommentController
    {
        // Preload our schema..
        MasterDetailComment thisSchemaLoad = new MasterDetailComment();
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
        public MasterDetailCommentCollection FetchAll()
        {
            MasterDetailCommentCollection coll = new MasterDetailCommentCollection();
            Query qry = new Query(MasterDetailComment.Schema);
            // Begin Bayshore custom code block (rread 6/26/07)
            // Ignore records marked for deletion when doing a FetchAll
            if (MasterDetailComment.Schema.GetColumn("IsDeleted") != null)
            {
                qry.WHERE("IsDeleted <> true");
            }
            else if (MasterDetailComment.Schema.GetColumn("Deleted") != null)
            {
                qry.WHERE("Deleted <> true");
            }
            // End Bayshore custom code block

            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailCommentCollection FetchByID(object Id)
        {
            MasterDetailCommentCollection coll = new MasterDetailCommentCollection().Where("Id", Id).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public MasterDetailCommentCollection FetchByQuery(Query qry)
        {
            MasterDetailCommentCollection coll = new MasterDetailCommentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (MasterDetailComment.Destroy(Id) == 1);
        }


        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (MasterDetailComment.Delete(Id) == 1);
        }


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(int ModuleId, string Username, string Email, string IPAddress, string CommentText)
        {
            MasterDetailComment item = new MasterDetailComment();

            item.ModuleId = ModuleId;

            item.Username = Username;

            item.Email = Email;

            item.IPAddress = IPAddress;

            item.CommentText = CommentText;

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
        public void InsertAndReturnIdentity(int ModuleId, string Username, string Email, string IPAddress, string CommentText, out object newId)
        {
            MasterDetailComment item = new MasterDetailComment();

            item.ModuleId = ModuleId;

            item.Username = Username;

            item.Email = Email;

            item.IPAddress = IPAddress;

            item.CommentText = CommentText;

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
        public void Update(long Id, int ModuleId, string Username, string Email, string IPAddress, string CommentText)
        {
            MasterDetailComment item = new MasterDetailComment();
            item.MarkOld();
            item.IsLoaded = true;

            item.Id = Id;

            item.ModuleId = ModuleId;

            item.Username = Username;

            item.Email = Email;

            item.IPAddress = IPAddress;

            item.CommentText = CommentText;

            item.Save(UserName);
        }
    }
}
