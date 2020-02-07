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
    /// Strongly-typed collection for the MasterDetailComment class.
    /// </summary>
    [Serializable]
    public partial class MasterDetailCommentCollection : ActiveList<MasterDetailComment, MasterDetailCommentCollection>
    {
        public MasterDetailCommentCollection()
        {
            base.ProviderName = "WebModules";

        }

        /// <summary>
        /// Filters an existing collection based on the set criteria. This is an in-memory filter
        /// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MasterDetailCommentCollection</returns>
        public MasterDetailCommentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MasterDetailComment o = this[i];
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
    /// This is an ActiveRecord class which wraps the MasterDetail_Comments table.
    /// </summary>
    [Serializable]
    public partial class MasterDetailComment : ActiveRecord<MasterDetailComment>, IActiveRecord
    {
        #region .ctors and Default Settings

        public MasterDetailComment()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        private void InitSetDefaults() { SetDefaults(); }

        public MasterDetailComment(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public MasterDetailComment(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public MasterDetailComment(string columnName, object columnValue)
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
                TableSchema.Table schema = new TableSchema.Table("MasterDetail_Comments", TableType.Table, DataService.GetInstance("WebModules"));
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

                TableSchema.TableColumn colvarModuleId = new TableSchema.TableColumn(schema);
                colvarModuleId.ColumnName = "ModuleId";
                colvarModuleId.DataType = DbType.Int32;
                colvarModuleId.MaxLength = 0;
                colvarModuleId.AutoIncrement = false;
                colvarModuleId.IsNullable = false;
                colvarModuleId.IsPrimaryKey = false;
                colvarModuleId.IsForeignKey = true;
                colvarModuleId.IsReadOnly = false;
                colvarModuleId.DefaultSetting = @"";

                colvarModuleId.ForeignKeyTableName = "WebModules_Modules";
                schema.Columns.Add(colvarModuleId);

                TableSchema.TableColumn colvarUsername = new TableSchema.TableColumn(schema);
                colvarUsername.ColumnName = "Username";
                colvarUsername.DataType = DbType.AnsiString;
                colvarUsername.MaxLength = 128;
                colvarUsername.AutoIncrement = false;
                colvarUsername.IsNullable = true;
                colvarUsername.IsPrimaryKey = false;
                colvarUsername.IsForeignKey = false;
                colvarUsername.IsReadOnly = false;
                colvarUsername.DefaultSetting = @"";
                colvarUsername.ForeignKeyTableName = "";
                schema.Columns.Add(colvarUsername);

                TableSchema.TableColumn colvarEmail = new TableSchema.TableColumn(schema);
                colvarEmail.ColumnName = "Email";
                colvarEmail.DataType = DbType.AnsiString;
                colvarEmail.MaxLength = 128;
                colvarEmail.AutoIncrement = false;
                colvarEmail.IsNullable = true;
                colvarEmail.IsPrimaryKey = false;
                colvarEmail.IsForeignKey = false;
                colvarEmail.IsReadOnly = false;
                colvarEmail.DefaultSetting = @"";
                colvarEmail.ForeignKeyTableName = "";
                schema.Columns.Add(colvarEmail);

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

                TableSchema.TableColumn colvarCommentText = new TableSchema.TableColumn(schema);
                colvarCommentText.ColumnName = "CommentText";
                colvarCommentText.DataType = DbType.AnsiString;
                colvarCommentText.MaxLength = 2147483647;
                colvarCommentText.AutoIncrement = false;
                colvarCommentText.IsNullable = true;
                colvarCommentText.IsPrimaryKey = false;
                colvarCommentText.IsForeignKey = false;
                colvarCommentText.IsReadOnly = false;
                colvarCommentText.DefaultSetting = @"";
                colvarCommentText.ForeignKeyTableName = "";
                schema.Columns.Add(colvarCommentText);

                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = true;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;
                colvarCreatedOn.DefaultSetting = @"";
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
                DataService.Providers["WebModules"].AddSchema("MasterDetail_Comments", schema);
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

        [XmlAttribute("ModuleId")]
        [Bindable(true)]
        public int ModuleId
        {
            get { return GetColumnValue<int>(Columns.ModuleId); }
            set { SetColumnValue(Columns.ModuleId, value); }
        }

        [XmlAttribute("Username")]
        [Bindable(true)]
        public string Username
        {
            get { return GetColumnValue<string>(Columns.Username); }
            set { SetColumnValue(Columns.Username, value); }
        }

        [XmlAttribute("Email")]
        [Bindable(true)]
        public string Email
        {
            get { return GetColumnValue<string>(Columns.Email); }
            set { SetColumnValue(Columns.Email, value); }
        }

        [XmlAttribute("IPAddress")]
        [Bindable(true)]
        public string IPAddress
        {
            get { return GetColumnValue<string>(Columns.IPAddress); }
            set { SetColumnValue(Columns.IPAddress, value); }
        }

        [XmlAttribute("CommentText")]
        [Bindable(true)]
        public string CommentText
        {
            get { return GetColumnValue<string>(Columns.CommentText); }
            set { SetColumnValue(Columns.CommentText, value); }
        }

        [XmlAttribute("CreatedOn")]
        [Bindable(true)]
        public DateTime? CreatedOn
        {
            get { return GetColumnValue<DateTime?>(Columns.CreatedOn); }
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


        #region PrimaryKey Methods

        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);

            SetPKValues();
        }


        public BayshoreSolutions.WebModules.MasterDetail.MasterDetailFlagCommentCollection MasterDetailFlagComments
        {
            get { return new BayshoreSolutions.WebModules.MasterDetail.MasterDetailFlagCommentCollection().Where(MasterDetailFlagComment.Columns.CommentId, Id).Load(); }
        }
        #endregion



        #region ForeignKey Properties

        #endregion



        //no ManyToMany tables defined (0)



        #region ObjectDataSource support


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void Insert(int varModuleId, string varUsername, string varEmail, string varIPAddress, string varCommentText, DateTime? varCreatedOn, string varCreatedBy)
        {
            MasterDetailComment item = new MasterDetailComment();

            item.ModuleId = varModuleId;

            item.Username = varUsername;

            item.Email = varEmail;

            item.IPAddress = varIPAddress;

            item.CommentText = varCommentText;

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
        public static MasterDetailComment InsertAndReturnObject(int varModuleId, string varUsername, string varEmail, string varIPAddress, string varCommentText, DateTime? varCreatedOn, string varCreatedBy)
        {
            MasterDetailComment item = new MasterDetailComment();

            item.ModuleId = varModuleId;

            item.Username = varUsername;

            item.Email = varEmail;

            item.IPAddress = varIPAddress;

            item.CommentText = varCommentText;

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
        public static void Update(long varId, int varModuleId, string varUsername, string varEmail, string varIPAddress, string varCommentText, DateTime? varCreatedOn, string varCreatedBy)
        {
            MasterDetailComment item = new MasterDetailComment();

            item.Id = varId;

            item.ModuleId = varModuleId;

            item.Username = varUsername;

            item.Email = varEmail;

            item.IPAddress = varIPAddress;

            item.CommentText = varCommentText;

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



        public static TableSchema.TableColumn ModuleIdColumn
        {
            get { return Schema.Columns[1]; }
        }



        public static TableSchema.TableColumn UsernameColumn
        {
            get { return Schema.Columns[2]; }
        }



        public static TableSchema.TableColumn EmailColumn
        {
            get { return Schema.Columns[3]; }
        }



        public static TableSchema.TableColumn IPAddressColumn
        {
            get { return Schema.Columns[4]; }
        }



        public static TableSchema.TableColumn CommentTextColumn
        {
            get { return Schema.Columns[5]; }
        }



        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[6]; }
        }



        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[7]; }
        }



        #endregion
        #region Columns Struct
        public struct Columns
        {
            public static string Id = @"Id";
            public static string ModuleId = @"ModuleId";
            public static string Username = @"Username";
            public static string Email = @"Email";
            public static string IPAddress = @"IPAddress";
            public static string CommentText = @"CommentText";
            public static string CreatedOn = @"CreatedOn";
            public static string CreatedBy = @"CreatedBy";

        }
        #endregion

        #region Update PK Collections

        public void SetPKValues()
        {
        }
        #endregion

        #region Deep Save

        public void DeepSave()
        {
            Save();

        }
        #endregion
    }
}
