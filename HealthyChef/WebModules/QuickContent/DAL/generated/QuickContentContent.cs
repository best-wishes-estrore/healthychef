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
    /// Strongly-typed collection for the QuickContentContent class.
    /// </summary>
    [Serializable]
    public partial class QuickContentContentCollection : ActiveList<QuickContentContent, QuickContentContentCollection>
    {
        public QuickContentContentCollection()
        {
            base.ProviderName = "WebModules";

        }

        /// <summary>
        /// Filters an existing collection based on the set criteria. This is an in-memory filter
        /// Thanks to developingchris for this!
        /// </summary>
        /// <returns>QuickContentContentCollection</returns>
        public QuickContentContentCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                QuickContentContent o = this[i];
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
    /// This is an ActiveRecord class which wraps the QuickContent_Content table.
    /// </summary>
    [Serializable]
    public partial class QuickContentContent : ActiveRecord<QuickContentContent>, IActiveRecord
    {
        #region .ctors and Default Settings

        public QuickContentContent()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        private void InitSetDefaults() { SetDefaults(); }

        public QuickContentContent(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public QuickContentContent(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public QuickContentContent(string columnName, object columnValue)
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
                TableSchema.Table schema = new TableSchema.Table("QuickContent_Content", TableType.Table, DataService.GetInstance("WebModules"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns

                TableSchema.TableColumn colvarId = new TableSchema.TableColumn(schema);
                colvarId.ColumnName = "Id";
                colvarId.DataType = DbType.Int32;
                colvarId.MaxLength = 0;
                colvarId.AutoIncrement = true;
                colvarId.IsNullable = false;
                colvarId.IsPrimaryKey = true;
                colvarId.IsForeignKey = false;
                colvarId.IsReadOnly = false;
                colvarId.DefaultSetting = @"";
                colvarId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarId);

                TableSchema.TableColumn colvarContentName = new TableSchema.TableColumn(schema);
                colvarContentName.ColumnName = "ContentName";
                colvarContentName.DataType = DbType.String;
                colvarContentName.MaxLength = 100;
                colvarContentName.AutoIncrement = false;
                colvarContentName.IsNullable = false;
                colvarContentName.IsPrimaryKey = false;
                colvarContentName.IsForeignKey = false;
                colvarContentName.IsReadOnly = false;
                colvarContentName.DefaultSetting = @"";
                colvarContentName.ForeignKeyTableName = "";
                schema.Columns.Add(colvarContentName);

                TableSchema.TableColumn colvarStatusId = new TableSchema.TableColumn(schema);
                colvarStatusId.ColumnName = "StatusId";
                colvarStatusId.DataType = DbType.Int32;
                colvarStatusId.MaxLength = 0;
                colvarStatusId.AutoIncrement = false;
                colvarStatusId.IsNullable = false;
                colvarStatusId.IsPrimaryKey = false;
                colvarStatusId.IsForeignKey = false;
                colvarStatusId.IsReadOnly = false;

                colvarStatusId.DefaultSetting = @"((1))";
                colvarStatusId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarStatusId);

                TableSchema.TableColumn colvarBody = new TableSchema.TableColumn(schema);
                colvarBody.ColumnName = "Body";
                colvarBody.DataType = DbType.String;
                colvarBody.MaxLength = 1073741823;
                colvarBody.AutoIncrement = false;
                colvarBody.IsNullable = true;
                colvarBody.IsPrimaryKey = false;
                colvarBody.IsForeignKey = false;
                colvarBody.IsReadOnly = false;
                colvarBody.DefaultSetting = @"";
                colvarBody.ForeignKeyTableName = "";
                schema.Columns.Add(colvarBody);

                TableSchema.TableColumn colvarCulture = new TableSchema.TableColumn(schema);
                colvarCulture.ColumnName = "Culture";
                colvarCulture.DataType = DbType.AnsiString;
                colvarCulture.MaxLength = 10;
                colvarCulture.AutoIncrement = false;
                colvarCulture.IsNullable = false;
                colvarCulture.IsPrimaryKey = false;
                colvarCulture.IsForeignKey = false;
                colvarCulture.IsReadOnly = false;

                colvarCulture.DefaultSetting = @"(N'en-US')";
                colvarCulture.ForeignKeyTableName = "";
                schema.Columns.Add(colvarCulture);

                TableSchema.TableColumn colvarCreatedOn = new TableSchema.TableColumn(schema);
                colvarCreatedOn.ColumnName = "CreatedOn";
                colvarCreatedOn.DataType = DbType.DateTime;
                colvarCreatedOn.MaxLength = 0;
                colvarCreatedOn.AutoIncrement = false;
                colvarCreatedOn.IsNullable = true;
                colvarCreatedOn.IsPrimaryKey = false;
                colvarCreatedOn.IsForeignKey = false;
                colvarCreatedOn.IsReadOnly = false;

                colvarCreatedOn.DefaultSetting = @"(getdate())";
                colvarCreatedOn.ForeignKeyTableName = "";
                schema.Columns.Add(colvarCreatedOn);

                TableSchema.TableColumn colvarCreatedBy = new TableSchema.TableColumn(schema);
                colvarCreatedBy.ColumnName = "CreatedBy";
                colvarCreatedBy.DataType = DbType.String;
                colvarCreatedBy.MaxLength = 50;
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
                DataService.Providers["WebModules"].AddSchema("QuickContent_Content", schema);
            }
        }
        #endregion

        #region Props

        [XmlAttribute("Id")]
        [Bindable(true)]
        public int Id
        {
            get { return GetColumnValue<int>(Columns.Id); }
            set { SetColumnValue(Columns.Id, value); }
        }

        [XmlAttribute("ContentName")]
        [Bindable(true)]
        public string ContentName
        {
            get { return GetColumnValue<string>(Columns.ContentName); }
            set { SetColumnValue(Columns.ContentName, value); }
        }

        [XmlAttribute("StatusId")]
        [Bindable(true)]
        public int StatusId
        {
            get { return GetColumnValue<int>(Columns.StatusId); }
            set { SetColumnValue(Columns.StatusId, value); }
        }

        [XmlAttribute("Body")]
        [Bindable(true)]
        public string Body
        {
            get { return GetColumnValue<string>(Columns.Body); }
            set { SetColumnValue(Columns.Body, value); }
        }

        [XmlAttribute("Culture")]
        [Bindable(true)]
        public string Culture
        {
            get { return GetColumnValue<string>(Columns.Culture); }
            set { SetColumnValue(Columns.Culture, value); }
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




        //no foreign key tables defined (0)



        //no ManyToMany tables defined (0)



        #region ObjectDataSource support


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void Insert(string varContentName, int varStatusId, string varBody, string varCulture, DateTime? varCreatedOn, string varCreatedBy)
        {
            QuickContentContent item = new QuickContentContent();

            item.ContentName = varContentName;

            item.StatusId = varStatusId;

            item.Body = varBody;

            item.Culture = varCulture;

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
        public static QuickContentContent InsertAndReturnObject(string varContentName, int varStatusId, string varBody, string varCulture, DateTime? varCreatedOn, string varCreatedBy)
        {
            QuickContentContent item = new QuickContentContent();

            item.ContentName = varContentName;

            item.StatusId = varStatusId;

            item.Body = varBody;

            item.Culture = varCulture;

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
        public static void Update(int varId, string varContentName, int varStatusId, string varBody, string varCulture, DateTime? varCreatedOn, string varCreatedBy)
        {
            QuickContentContent item = new QuickContentContent();

            item.Id = varId;

            item.ContentName = varContentName;

            item.StatusId = varStatusId;

            item.Body = varBody;

            item.Culture = varCulture;

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



        public static TableSchema.TableColumn ContentNameColumn
        {
            get { return Schema.Columns[1]; }
        }



        public static TableSchema.TableColumn StatusIdColumn
        {
            get { return Schema.Columns[2]; }
        }



        public static TableSchema.TableColumn BodyColumn
        {
            get { return Schema.Columns[3]; }
        }



        public static TableSchema.TableColumn CultureColumn
        {
            get { return Schema.Columns[4]; }
        }



        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[5]; }
        }



        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[6]; }
        }



        #endregion
        #region Columns Struct
        public struct Columns
        {
            public static string Id = @"Id";
            public static string ContentName = @"ContentName";
            public static string StatusId = @"StatusId";
            public static string Body = @"Body";
            public static string Culture = @"Culture";
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
