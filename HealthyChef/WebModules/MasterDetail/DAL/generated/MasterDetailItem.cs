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
    /// Strongly-typed collection for the MasterDetailItem class.
    /// </summary>
    [Serializable]
    public partial class MasterDetailItemCollection : ActiveList<MasterDetailItem, MasterDetailItemCollection>
    {
        public MasterDetailItemCollection()
        {
            base.ProviderName = "WebModules";

        }

        /// <summary>
        /// Filters an existing collection based on the set criteria. This is an in-memory filter
        /// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MasterDetailItemCollection</returns>
        public MasterDetailItemCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MasterDetailItem o = this[i];
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
    /// This is an ActiveRecord class which wraps the MasterDetail_Item table.
    /// </summary>
    [Serializable]
    public partial class MasterDetailItem : ActiveRecord<MasterDetailItem>, IActiveRecord
    {
        #region .ctors and Default Settings

        public MasterDetailItem()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        private void InitSetDefaults() { SetDefaults(); }

        public MasterDetailItem(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public MasterDetailItem(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public MasterDetailItem(string columnName, object columnValue)
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
                TableSchema.Table schema = new TableSchema.Table("MasterDetail_Item", TableType.Table, DataService.GetInstance("WebModules"));
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

                TableSchema.TableColumn colvarCulture = new TableSchema.TableColumn(schema);
                colvarCulture.ColumnName = "Culture";
                colvarCulture.DataType = DbType.AnsiString;
                colvarCulture.MaxLength = 10;
                colvarCulture.AutoIncrement = false;
                colvarCulture.IsNullable = false;
                colvarCulture.IsPrimaryKey = false;
                colvarCulture.IsForeignKey = false;
                colvarCulture.IsReadOnly = false;

                colvarCulture.DefaultSetting = @"('en-US')";
                colvarCulture.ForeignKeyTableName = "";
                schema.Columns.Add(colvarCulture);

                TableSchema.TableColumn colvarStatusId = new TableSchema.TableColumn(schema);
                colvarStatusId.ColumnName = "StatusId";
                colvarStatusId.DataType = DbType.Int32;
                colvarStatusId.MaxLength = 0;
                colvarStatusId.AutoIncrement = false;
                colvarStatusId.IsNullable = false;
                colvarStatusId.IsPrimaryKey = false;
                colvarStatusId.IsForeignKey = false;
                colvarStatusId.IsReadOnly = false;

                colvarStatusId.DefaultSetting = @"((0))";
                colvarStatusId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarStatusId);

                TableSchema.TableColumn colvarShortDescription = new TableSchema.TableColumn(schema);
                colvarShortDescription.ColumnName = "ShortDescription";
                colvarShortDescription.DataType = DbType.String;
                colvarShortDescription.MaxLength = 256;
                colvarShortDescription.AutoIncrement = false;
                colvarShortDescription.IsNullable = true;
                colvarShortDescription.IsPrimaryKey = false;
                colvarShortDescription.IsForeignKey = false;
                colvarShortDescription.IsReadOnly = false;
                colvarShortDescription.DefaultSetting = @"";
                colvarShortDescription.ForeignKeyTableName = "";
                schema.Columns.Add(colvarShortDescription);

                TableSchema.TableColumn colvarLongDescription = new TableSchema.TableColumn(schema);
                colvarLongDescription.ColumnName = "LongDescription";
                colvarLongDescription.DataType = DbType.String;
                colvarLongDescription.MaxLength = 1073741823;
                colvarLongDescription.AutoIncrement = false;
                colvarLongDescription.IsNullable = true;
                colvarLongDescription.IsPrimaryKey = false;
                colvarLongDescription.IsForeignKey = false;
                colvarLongDescription.IsReadOnly = false;
                colvarLongDescription.DefaultSetting = @"";
                colvarLongDescription.ForeignKeyTableName = "";
                schema.Columns.Add(colvarLongDescription);

                TableSchema.TableColumn colvarImagePath = new TableSchema.TableColumn(schema);
                colvarImagePath.ColumnName = "ImagePath";
                colvarImagePath.DataType = DbType.String;
                colvarImagePath.MaxLength = 512;
                colvarImagePath.AutoIncrement = false;
                colvarImagePath.IsNullable = true;
                colvarImagePath.IsPrimaryKey = false;
                colvarImagePath.IsForeignKey = false;
                colvarImagePath.IsReadOnly = false;
                colvarImagePath.DefaultSetting = @"";
                colvarImagePath.ForeignKeyTableName = "";
                schema.Columns.Add(colvarImagePath);

                TableSchema.TableColumn colvarIsFeatured = new TableSchema.TableColumn(schema);
                colvarIsFeatured.ColumnName = "IsFeatured";
                colvarIsFeatured.DataType = DbType.Boolean;
                colvarIsFeatured.MaxLength = 0;
                colvarIsFeatured.AutoIncrement = false;
                colvarIsFeatured.IsNullable = false;
                colvarIsFeatured.IsPrimaryKey = false;
                colvarIsFeatured.IsForeignKey = false;
                colvarIsFeatured.IsReadOnly = false;

                colvarIsFeatured.DefaultSetting = @"((0))";
                colvarIsFeatured.ForeignKeyTableName = "";
                schema.Columns.Add(colvarIsFeatured);

                TableSchema.TableColumn colvarTags = new TableSchema.TableColumn(schema);
                colvarTags.ColumnName = "Tags";
                colvarTags.DataType = DbType.String;
                colvarTags.MaxLength = 256;
                colvarTags.AutoIncrement = false;
                colvarTags.IsNullable = true;
                colvarTags.IsPrimaryKey = false;
                colvarTags.IsForeignKey = false;
                colvarTags.IsReadOnly = false;
                colvarTags.DefaultSetting = @"";
                colvarTags.ForeignKeyTableName = "";
                schema.Columns.Add(colvarTags);

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
                DataService.Providers["WebModules"].AddSchema("MasterDetail_Item", schema);
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

        [XmlAttribute("ModuleId")]
        [Bindable(true)]
        public int ModuleId
        {
            get { return GetColumnValue<int>(Columns.ModuleId); }
            set { SetColumnValue(Columns.ModuleId, value); }
        }

        [XmlAttribute("Culture")]
        [Bindable(true)]
        public string Culture
        {
            get { return GetColumnValue<string>(Columns.Culture); }
            set { SetColumnValue(Columns.Culture, value); }
        }

        [XmlAttribute("StatusId")]
        [Bindable(true)]
        public int StatusId
        {
            get { return GetColumnValue<int>(Columns.StatusId); }
            set { SetColumnValue(Columns.StatusId, value); }
        }

        [XmlAttribute("ShortDescription")]
        [Bindable(true)]
        public string ShortDescription
        {
            get { return GetColumnValue<string>(Columns.ShortDescription); }
            set { SetColumnValue(Columns.ShortDescription, value); }
        }

        [XmlAttribute("LongDescription")]
        [Bindable(true)]
        public string LongDescription
        {
            get { return GetColumnValue<string>(Columns.LongDescription); }
            set { SetColumnValue(Columns.LongDescription, value); }
        }

        [XmlAttribute("ImagePath")]
        [Bindable(true)]
        public string ImagePath
        {
            get { return GetColumnValue<string>(Columns.ImagePath); }
            set { SetColumnValue(Columns.ImagePath, value); }
        }

        [XmlAttribute("IsFeatured")]
        [Bindable(true)]
        public bool IsFeatured
        {
            get { return GetColumnValue<bool>(Columns.IsFeatured); }
            set { SetColumnValue(Columns.IsFeatured, value); }
        }

        [XmlAttribute("Tags")]
        [Bindable(true)]
        public string Tags
        {
            get { return GetColumnValue<string>(Columns.Tags); }
            set { SetColumnValue(Columns.Tags, value); }
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




        #region ForeignKey Properties

        #endregion



        //no ManyToMany tables defined (0)



        #region ObjectDataSource support


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void Insert(int varModuleId, string varCulture, int varStatusId, string varShortDescription, string varLongDescription, string varImagePath, bool varIsFeatured, string varTags, DateTime? varCreatedOn, string varCreatedBy)
        {
            MasterDetailItem item = new MasterDetailItem();

            item.ModuleId = varModuleId;

            item.Culture = varCulture;

            item.StatusId = varStatusId;

            item.ShortDescription = varShortDescription;

            item.LongDescription = varLongDescription;

            item.ImagePath = varImagePath;

            item.IsFeatured = varIsFeatured;

            item.Tags = varTags;

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
        public static MasterDetailItem InsertAndReturnObject(int varModuleId, string varCulture, int varStatusId, string varShortDescription, string varLongDescription, string varImagePath, bool varIsFeatured, string varTags, DateTime? varCreatedOn, string varCreatedBy)
        {
            MasterDetailItem item = new MasterDetailItem();

            item.ModuleId = varModuleId;

            item.Culture = varCulture;

            item.StatusId = varStatusId;

            item.ShortDescription = varShortDescription;

            item.LongDescription = varLongDescription;

            item.ImagePath = varImagePath;

            item.IsFeatured = varIsFeatured;

            item.Tags = varTags;

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
        public static void Update(int varId, int varModuleId, string varCulture, int varStatusId, string varShortDescription, string varLongDescription, string varImagePath, bool varIsFeatured, string varTags, DateTime? varCreatedOn, string varCreatedBy)
        {
            MasterDetailItem item = new MasterDetailItem();

            item.Id = varId;

            item.ModuleId = varModuleId;

            item.Culture = varCulture;

            item.StatusId = varStatusId;

            item.ShortDescription = varShortDescription;

            item.LongDescription = varLongDescription;

            item.ImagePath = varImagePath;

            item.IsFeatured = varIsFeatured;

            item.Tags = varTags;

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



        public static TableSchema.TableColumn CultureColumn
        {
            get { return Schema.Columns[2]; }
        }



        public static TableSchema.TableColumn StatusIdColumn
        {
            get { return Schema.Columns[3]; }
        }



        public static TableSchema.TableColumn ShortDescriptionColumn
        {
            get { return Schema.Columns[4]; }
        }



        public static TableSchema.TableColumn LongDescriptionColumn
        {
            get { return Schema.Columns[5]; }
        }



        public static TableSchema.TableColumn ImagePathColumn
        {
            get { return Schema.Columns[6]; }
        }



        public static TableSchema.TableColumn IsFeaturedColumn
        {
            get { return Schema.Columns[7]; }
        }



        public static TableSchema.TableColumn TagsColumn
        {
            get { return Schema.Columns[8]; }
        }



        public static TableSchema.TableColumn CreatedOnColumn
        {
            get { return Schema.Columns[9]; }
        }



        public static TableSchema.TableColumn CreatedByColumn
        {
            get { return Schema.Columns[10]; }
        }



        #endregion
        #region Columns Struct
        public struct Columns
        {
            public static string Id = @"Id";
            public static string ModuleId = @"ModuleId";
            public static string Culture = @"Culture";
            public static string StatusId = @"StatusId";
            public static string ShortDescription = @"ShortDescription";
            public static string LongDescription = @"LongDescription";
            public static string ImagePath = @"ImagePath";
            public static string IsFeatured = @"IsFeatured";
            public static string Tags = @"Tags";
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
