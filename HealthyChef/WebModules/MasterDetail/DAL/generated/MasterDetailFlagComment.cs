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
    /// Strongly-typed collection for the MasterDetailFlagComment class.
    /// </summary>
    [Serializable]
    public partial class MasterDetailFlagCommentCollection : ActiveList<MasterDetailFlagComment, MasterDetailFlagCommentCollection>
    {
        public MasterDetailFlagCommentCollection()
        {
            base.ProviderName = "WebModules";

        }

        /// <summary>
        /// Filters an existing collection based on the set criteria. This is an in-memory filter
        /// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MasterDetailFlagCommentCollection</returns>
        public MasterDetailFlagCommentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MasterDetailFlagComment o = this[i];
                foreach (SubSonic.Where w in this.wheres)
                {
                    bool remove = false;
                    System.Reflection.PropertyInfo pi = o.GetType().GetProperty(w.ColumnName);
                    if (pi.CanRead)
                    {
                        object val = pi.GetValue(o, null);
                        switch (w.Comparison)
                        {
                            case SubSonic.Comparison.Equals:
                                if (!val.Equals(w.ParameterValue))
                                {
                                    remove = true;
                                }
                                break;
                        }
                    }
                    if (remove)
                    {
                        this.Remove(o);
                        break;
                    }
                }
            }
            return this;
        }


    }
    /// <summary>
    /// This is an ActiveRecord class which wraps the MasterDetail_FlagComments table.
    /// </summary>
    [Serializable]
    public partial class MasterDetailFlagComment : ActiveRecord<MasterDetailFlagComment>, IActiveRecord
    {
        #region .ctors and Default Settings

        public MasterDetailFlagComment()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        private void InitSetDefaults() { SetDefaults(); }

        public MasterDetailFlagComment(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public MasterDetailFlagComment(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public MasterDetailFlagComment(string columnName, object columnValue)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByParam(columnName, columnValue);
        }

        protected static void SetSQLProps() { GetTableSchema(); }

        #endregion

        #region Schema and Query Accessor
        public static Query CreateQuery() { return new Query(Schema); }
        public static TableSchema.Table Schema
        {
            get
            {
                if (BaseSchema == null)
                    SetSQLProps();
                return BaseSchema;
            }
        }

        private static void GetTableSchema()
        {
            if (!IsSchemaInitialized)
            {
                //Schema declaration
                TableSchema.Table schema = new TableSchema.Table("MasterDetail_FlagComments", TableType.Table, DataService.GetInstance("WebModules"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns

                TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
                colvarId.ColumnName = "Id";
                colvarId.DataType = DbType.Int64;
                colvarId.MaxLength = 0;
                colvarId.AutoIncrement = true;
                colvarId.IsNullable = false;
                colvarId.IsPrimaryKey = true;
                colvarId.IsForeignKey = false;
                colvarId.IsReadOnly = false;
                colvarId.DefaultSetting = @"";
                colvarId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarId);

                TableSchema.TableColumn colvarCommentId = new TableSchema.TableColumn(schema);
                colvarCommentId.ColumnName = "CommentId";
                colvarCommentId.DataType = DbType.Int64;
                colvarCommentId.MaxLength = 0;
                colvarCommentId.AutoIncrement = false;
                colvarCommentId.IsNullable = false;
                colvarCommentId.IsPrimaryKey = false;
                colvarCommentId.IsForeignKey = true;
                colvarCommentId.IsReadOnly = false;
                colvarCommentId.DefaultSetting = @"";

                colvarCommentId.ForeignKeyTableName = "MasterDetail_Comments";
                schema.Columns.Add(colvarCommentId);

                TableSchema.TableColumn colvarIPAddress = new TableSchema.TableColumn(schema);
                colvarIPAddress.ColumnName = "IPAddress";
                colvarIPAddress.DataType = DbType.AnsiString;
                colvarIPAddress.MaxLength = 20;
                colvarIPAddress.AutoIncrement = false;
                colvarIPAddress.IsNullable = true;
                colvarIPAddress.IsPrimaryKey = false;
                colvarIPAddress.IsForeignKey = false;
                colvarIPAddress.IsReadOnly = false;
                colvarIPAddress.DefaultSetting = @"";
                colvarIPAddress.ForeignKeyTableName = "";
                schema.Columns.Add(colvarIPAddress);

                TableSchema.TableColumn colvarReason = new TableSchema.TableColumn(schema);
                colvarReason.ColumnName = "Reason";
                colvarReason.DataType = DbType.String;
                colvarReason.MaxLength = 256;
                colvarReason.AutoIncrement = false;
                colvarReason.IsNullable = true;
                colvarReason.IsPrimaryKey = false;
                colvarReason.IsForeignKey = false;
                colvarReason.IsReadOnly = false;
                colvarReason.DefaultSetting = @"";
                colvarReason.ForeignKeyTableName = "";
                schema.Columns.Add(colvarReason);

                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = false;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;

                colvarCreatedOn.DefaultSetting = @"(getdate())";
                colvarCreatedOn.ForeignKeyTableName = "";
                schema.Columns.Add(colvarCreatedOn);

                TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
                colvarCreatedBy.ColumnName = "CreatedBy";
                colvarCreatedBy.DataType = DbType.String;
                colvarCreatedBy.MaxLength = 256;
                colvarCreatedBy.AutoIncrement = false;
                colvarCreatedBy.IsNullable = true;
                colvarCreatedBy.IsPrimaryKey = false;
                colvarCreatedBy.IsForeignKey = false;
                colvarCreatedBy.IsReadOnly = false;
                colvarCreatedBy.DefaultSetting = @"";
                colvarCreatedBy.ForeignKeyTableName = "";
                schema.Columns.Add(colvarCreatedBy);

                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["WebModules"].AddSchema("MasterDetail_FlagComments", schema);
            }
        }
        #endregion

        #region Props

        [XmlAttribute("Id")]
        [Bindable(true)]
        public long Id
        {
            get { return GetColumnValue<long>(Columns.Id); }
            set { SetColumnValue(Columns.Id, value); }
        }

        [XmlAttribute("CommentId")]
        [Bindable(true)]
        public long CommentId
        {
            get { return GetColumnValue<long>(Columns.CommentId); }
            set { SetColumnValue(Columns.CommentId, value); }
        }

        [XmlAttribute("IPAddress")]
        [Bindable(true)]
        public string IPAddress
        {
            get { return GetColumnValue<string>(Columns.IPAddress); }
            set { SetColumnValue(Columns.IPAddress, value); }
        }

        [XmlAttribute("Reason")]
        [Bindable(true)]
        public string Reason
        {
            get { return GetColumnValue<string>(Columns.Reason); }
            set { SetColumnValue(Columns.Reason, value); }
        }

        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime CreatedOn
        {
            get { return GetColumnValue<DateTime>(Columns.CreatedOn); }
            set { SetColumnValue(Columns.CreatedOn, value); }
        }

        [XmlAttribute("CreatedBy")]
        [Bindable(true)]
        public string CreatedBy
        {
            get { return GetColumnValue<string>(Columns.CreatedBy); }
            set { SetColumnValue(Columns.CreatedBy, value); }
        }

        #endregion




        #region ForeignKey Properties

        /// <summary>
        /// Returns a MasterDetailComment ActiveRecord object related to this MasterDetailFlagComment
        /// 
        /// </summary>
        public BayshoreSolutions.WebModules.MasterDetail.MasterDetailComment MasterDetailComment
        {
            get { return BayshoreSolutions.WebModules.MasterDetail.MasterDetailComment.FetchByID(this.CommentId); }
            set { SetColumnValue("CommentId", value.Id); }
        }


        #endregion



        //no ManyToMany tables defined (0)



        #region ObjectDataSource support


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void Insert(long varCommentId, string varIPAddress, string varReason, DateTime varCreatedOn, string varCreatedBy)
        {
            MasterDetailFlagComment item = new MasterDetailFlagComment();

            item.CommentId = varCommentId;

            item.IPAddress = varIPAddress;

            item.Reason = varReason;

            item.CreatedOn = varCreatedOn;

            item.CreatedBy = varCreatedBy;


            if (System.Web.HttpContext.Current != null)
                item.Save(System.Web.HttpContext.Current.User.Identity.Name);
            else
                item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }
        // Begin Bayshore custom code block (rread 7/6/07)
        // Custom Insert method to return the object so we can retrieve the Identity after inserting.
        /// <summary>
        /// Inserts a record, CANNOT be used with the Object Data Source
        /// </summary>
        public static MasterDetailFlagComment InsertAndReturnObject(long varCommentId, string varIPAddress, string varReason, DateTime varCreatedOn, string varCreatedBy)
        {
            MasterDetailFlagComment item = new MasterDetailFlagComment();

            item.CommentId = varCommentId;

            item.IPAddress = varIPAddress;

            item.Reason = varReason;

            item.CreatedOn = varCreatedOn;

            item.CreatedBy = varCreatedBy;


            if (System.Web.HttpContext.Current != null)
                item.Save(System.Web.HttpContext.Current.User.Identity.Name);
            else
                item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);

            return item;
        }

        // Override the ActiveRecord Save() method to insert the username, if it exists.
        public void Save()
        {
            if (System.Web.HttpContext.Current != null)
                Save(System.Web.HttpContext.Current.User.Identity.Name);
            else
                Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }

        // End Bayshore custom code block

        /// <summary>
        /// Updates a record, can be used with the Object Data Source
        /// </summary>
        public static void Update(long varId, long varCommentId, string varIPAddress, string varReason, DateTime varCreatedOn, string varCreatedBy)
        {
            MasterDetailFlagComment item = new MasterDetailFlagComment();

            item.Id = varId;

            item.CommentId = varCommentId;

            item.IPAddress = varIPAddress;

            item.Reason = varReason;

            item.CreatedOn = varCreatedOn;

            item.CreatedBy = varCreatedBy;

            item.IsNew = false;
            if (System.Web.HttpContext.Current != null)
                item.Save(System.Web.HttpContext.Current.User.Identity.Name);
            else
                item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }
        #endregion



        #region Typed Columns


        public static TableSchema.TableColumn IdColumn
        {
            get { return Schema.Columns[0]; }
        }



        public static TableSchema.TableColumn CommentIdColumn
        {
            get { return Schema.Columns[1]; }
        }



        public static TableSchema.TableColumn IPAddressColumn
        {
            get { return Schema.Columns[2]; }
        }



        public static TableSchema.TableColumn ReasonColumn
        {
            get { return Schema.Columns[3]; }
        }



        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[4]; }
        }



        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[5]; }
        }



        #endregion
        #region Columns Struct
        public struct Columns
        {
            public static string Id = @"Id";
            public static string CommentId = @"CommentId";
            public static string IPAddress = @"IPAddress";
            public static string Reason = @"Reason";
            public static string CreatedOn = @"CreatedOn";
            public static string CreatedBy = @"CreatedBy";

        }
        #endregion

        #region Update PK Collections

        #endregion

        #region Deep Save

        #endregion
    }
}
