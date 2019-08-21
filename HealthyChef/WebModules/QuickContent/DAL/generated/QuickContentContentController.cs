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
    /// <summary>
    /// Controller class for QuickContent_Content
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class QuickContentContentController
    {
        // Preload our schema..
        QuickContentContent thisSchemaLoad = new QuickContentContent();
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
        public QuickContentContentCollection FetchAll()
        {
            QuickContentContentCollection coll = new QuickContentContentCollection();
            Query qry = new Query(QuickContentContent.Schema);
            // Begin Bayshore custom code block (rread 6/26/07)
            // Ignore records marked for deletion when doing a FetchAll
            if (QuickContentContent.Schema.GetColumn("IsDeleted") != null)
            {
                qry.WHERE("IsDeleted <> true");
            }
            else if (QuickContentContent.Schema.GetColumn("Deleted") != null)
            {
                qry.WHERE("Deleted <> true");
            }
            // End Bayshore custom code block

            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickContentContentCollection FetchByID(object Id)
        {
            QuickContentContentCollection coll = new QuickContentContentCollection().Where("Id", Id).Load();
            return coll;
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public QuickContentContentCollection FetchByQuery(Query qry)
        {
            QuickContentContentCollection coll = new QuickContentContentCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (QuickContentContent.Destroy(Id) == 1);
        }


        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (QuickContentContent.Delete(Id) == 1);
        }


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
        public void Insert(string ContentName, int StatusId, string Body, string Culture)
        {
            QuickContentContent item = new QuickContentContent();

            item.ContentName = ContentName;

            item.StatusId = StatusId;

            item.Body = Body;

            item.Culture = Culture;

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
        public void InsertAndReturnIdentity(string ContentName, int StatusId, string Body, string Culture, out object newId)
        {
            QuickContentContent item = new QuickContentContent();

            item.ContentName = ContentName;

            item.StatusId = StatusId;

            item.Body = Body;

            item.Culture = Culture;

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
        public void Update(int Id, string ContentName, int StatusId, string Body, string Culture)
        {
            QuickContentContent item = new QuickContentContent();
            item.MarkOld();
            item.IsLoaded = true;

            item.Id = Id;

            item.ContentName = ContentName;

            item.StatusId = StatusId;

            item.Body = Body;

            item.Culture = Culture;

            item.Save(UserName);
        }
    }
}
