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
    /// Strongly-typed collection for the MasterDetailMiniSummarySetting class.
    /// </summary>
    [Serializable]
    public partial class MasterDetailMiniSummarySettingCollection : ActiveList<MasterDetailMiniSummarySetting, MasterDetailMiniSummarySettingCollection>
    {
        public MasterDetailMiniSummarySettingCollection()
        {
            base.ProviderName = "WebModules";

        }

        /// <summary>
        /// Filters an existing collection based on the set criteria. This is an in-memory filter
        /// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MasterDetailMiniSummarySettingCollection</returns>
        public MasterDetailMiniSummarySettingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MasterDetailMiniSummarySetting o = this[i];
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
    /// This is an ActiveRecord class which wraps the MasterDetail_MiniSummarySettings table.
    /// </summary>
    [Serializable]
    public partial class MasterDetailMiniSummarySetting : ActiveRecord<MasterDetailMiniSummarySetting>, IActiveRecord
    {
        #region .ctors and Default Settings

        public MasterDetailMiniSummarySetting()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        private void InitSetDefaults() { SetDefaults(); }

        public MasterDetailMiniSummarySetting(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public MasterDetailMiniSummarySetting(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public MasterDetailMiniSummarySetting(string columnName, object columnValue)
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
                TableSchema.Table schema = new TableSchema.Table("MasterDetail_MiniSummarySettings", TableType.Table, DataService.GetInstance("WebModules"));
                schema.Columns = new TableSchema.TableColumnCollection();
                schema.SchemaName = @"dbo";
                //columns

                TableSchema.TableColumn colvarModuleId = new TableSchema.TableColumn(schema);
                colvarModuleId.ColumnName = "ModuleId";
                colvarModuleId.DataType = DbType.Int32;
                colvarModuleId.MaxLength = 0;
                colvarModuleId.AutoIncrement = false;
                colvarModuleId.IsNullable = false;
                colvarModuleId.IsPrimaryKey = true;
                colvarModuleId.IsForeignKey = true;
                colvarModuleId.IsReadOnly = false;
                colvarModuleId.DefaultSetting = @"";

                colvarModuleId.ForeignKeyTableName = "WebModules_Modules";
                schema.Columns.Add(colvarModuleId);

                TableSchema.TableColumn colvarStartingPageId = new TableSchema.TableColumn(schema);
                colvarStartingPageId.ColumnName = "StartingPageId";
                colvarStartingPageId.DataType = DbType.Int32;
                colvarStartingPageId.MaxLength = 0;
                colvarStartingPageId.AutoIncrement = false;
                colvarStartingPageId.IsNullable = true;
                colvarStartingPageId.IsPrimaryKey = false;
                colvarStartingPageId.IsForeignKey = false;
                colvarStartingPageId.IsReadOnly = false;
                colvarStartingPageId.DefaultSetting = @"";
                colvarStartingPageId.ForeignKeyTableName = "";
                schema.Columns.Add(colvarStartingPageId);

                TableSchema.TableColumn colvarNumRows = new TableSchema.TableColumn(schema);
                colvarNumRows.ColumnName = "NumRows";
                colvarNumRows.DataType = DbType.Int32;
                colvarNumRows.MaxLength = 0;
                colvarNumRows.AutoIncrement = false;
                colvarNumRows.IsNullable = true;
                colvarNumRows.IsPrimaryKey = false;
                colvarNumRows.IsForeignKey = false;
                colvarNumRows.IsReadOnly = false;

                colvarNumRows.DefaultSetting = @"((10))";
                colvarNumRows.ForeignKeyTableName = "";
                schema.Columns.Add(colvarNumRows);

                TableSchema.TableColumn colvarShowElapsedTime = new TableSchema.TableColumn(schema);
                colvarShowElapsedTime.ColumnName = "ShowElapsedTime";
                colvarShowElapsedTime.DataType = DbType.Boolean;
                colvarShowElapsedTime.MaxLength = 0;
                colvarShowElapsedTime.AutoIncrement = false;
                colvarShowElapsedTime.IsNullable = true;
                colvarShowElapsedTime.IsPrimaryKey = false;
                colvarShowElapsedTime.IsForeignKey = false;
                colvarShowElapsedTime.IsReadOnly = false;

                colvarShowElapsedTime.DefaultSetting = @"((0))";
                colvarShowElapsedTime.ForeignKeyTableName = "";
                schema.Columns.Add(colvarShowElapsedTime);

                TableSchema.TableColumn colvarFeaturedOnly = new TableSchema.TableColumn(schema);
                colvarFeaturedOnly.ColumnName = "FeaturedOnly";
                colvarFeaturedOnly.DataType = DbType.Boolean;
                colvarFeaturedOnly.MaxLength = 0;
                colvarFeaturedOnly.AutoIncrement = false;
                colvarFeaturedOnly.IsNullable = true;
                colvarFeaturedOnly.IsPrimaryKey = false;
                colvarFeaturedOnly.IsForeignKey = false;
                colvarFeaturedOnly.IsReadOnly = false;
                colvarFeaturedOnly.DefaultSetting = @"";
                colvarFeaturedOnly.ForeignKeyTableName = "";
                schema.Columns.Add(colvarFeaturedOnly);

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
                colvarCreatedBy.DataType = DbType.AnsiString;
                colvarCreatedBy.MaxLength = 100;
                colvarCreatedBy.AutoIncrement = false;
                colvarCreatedBy.IsNullable = true;
                colvarCreatedBy.IsPrimaryKey = false;
                colvarCreatedBy.IsForeignKey = false;
                colvarCreatedBy.IsReadOnly = false;
                colvarCreatedBy.DefaultSetting = @"";
                colvarCreatedBy.ForeignKeyTableName = "";
                schema.Columns.Add(colvarCreatedBy);

                TableSchema.TableColumn colvarModifiedOn = new TableSchema.TableColumn(schema);
                colvarModifiedOn.ColumnName = "ModifiedOn";
                colvarModifiedOn.DataType = DbType.DateTime;
                colvarModifiedOn.MaxLength = 0;
                colvarModifiedOn.AutoIncrement = false;
                colvarModifiedOn.IsNullable = true;
                colvarModifiedOn.IsPrimaryKey = false;
                colvarModifiedOn.IsForeignKey = false;
                colvarModifiedOn.IsReadOnly = false;
                colvarModifiedOn.DefaultSetting = @"";
                colvarModifiedOn.ForeignKeyTableName = "";
                schema.Columns.Add(colvarModifiedOn);

                TableSchema.TableColumn colvarModifiedBy = new TableSchema.TableColumn(schema);
                colvarModifiedBy.ColumnName = "ModifiedBy";
                colvarModifiedBy.DataType = DbType.AnsiString;
                colvarModifiedBy.MaxLength = 100;
                colvarModifiedBy.AutoIncrement = false;
                colvarModifiedBy.IsNullable = true;
                colvarModifiedBy.IsPrimaryKey = false;
                colvarModifiedBy.IsForeignKey = false;
                colvarModifiedBy.IsReadOnly = false;
                colvarModifiedBy.DefaultSetting = @"";
                colvarModifiedBy.ForeignKeyTableName = "";
                schema.Columns.Add(colvarModifiedBy);

                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["WebModules"].AddSchema("MasterDetail_MiniSummarySettings", schema);
            }
        }
        #endregion

        #region Props

        [XmlAttribute("ModuleId")]
        [Bindable(true)]
        public int ModuleId
        {
            get { return GetColumnValue<int>(Columns.ModuleId); }
            set { SetColumnValue(Columns.ModuleId, value); }
        }

        [XmlAttribute("StartingPageId")]
        [Bindable(true)]
        public int? StartingPageId
        {
            get { return GetColumnValue<int?>(Columns.StartingPageId); }
            set { SetColumnValue(Columns.StartingPageId, value); }
        }

        [XmlAttribute("NumRows")]
        [Bindable(true)]
        public int? NumRows
        {
            get { return GetColumnValue<int?>(Columns.NumRows); }
            set { SetColumnValue(Columns.NumRows, value); }
        }

        [XmlAttribute("ShowElapsedTime")]
        [Bindable(true)]
        public bool? ShowElapsedTime
        {
            get { return GetColumnValue<bool?>(Columns.ShowElapsedTime); }
            set { SetColumnValue(Columns.ShowElapsedTime, value); }
        }

        [XmlAttribute("FeaturedOnly")]
        [Bindable(true)]
        public bool? FeaturedOnly
        {
            get { return GetColumnValue<bool?>(Columns.FeaturedOnly); }
            set { SetColumnValue(Columns.FeaturedOnly, value); }
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

        [XmlAttribute("ModifiedOn")]
        [Bindable(true)]
        public DateTime? ModifiedOn
        {
            get { return GetColumnValue<DateTime?>(Columns.ModifiedOn); }
            set { SetColumnValue(Columns.ModifiedOn, value); }
        }

        [XmlAttribute("ModifiedBy")]
        [Bindable(true)]
        public string ModifiedBy
        {
            get { return GetColumnValue<string>(Columns.ModifiedBy); }
            set { SetColumnValue(Columns.ModifiedBy, value); }
        }

        #endregion




        #region ForeignKey Properties

        #endregion



        //no ManyToMany tables defined (0)



        #region ObjectDataSource support


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void Insert(int varModuleId, int? varStartingPageId, int? varNumRows, bool? varShowElapsedTime, bool? varFeaturedOnly, DateTime? varCreatedOn, string varCreatedBy, DateTime? varModifiedOn, string varModifiedBy)
        {
            MasterDetailMiniSummarySetting item = new MasterDetailMiniSummarySetting();

            item.ModuleId = varModuleId;

            item.StartingPageId = varStartingPageId;

            item.NumRows = varNumRows;

            item.ShowElapsedTime = varShowElapsedTime;

            item.FeaturedOnly = varFeaturedOnly;

            item.CreatedOn = varCreatedOn;

            item.CreatedBy = varCreatedBy;

            item.ModifiedOn = varModifiedOn;

            item.ModifiedBy = varModifiedBy;


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
        public static MasterDetailMiniSummarySetting InsertAndReturnObject(int varModuleId, int? varStartingPageId, int? varNumRows, bool? varShowElapsedTime, bool? varFeaturedOnly, DateTime? varCreatedOn, string varCreatedBy, DateTime? varModifiedOn, string varModifiedBy)
        {
            MasterDetailMiniSummarySetting item = new MasterDetailMiniSummarySetting();

            item.ModuleId = varModuleId;

            item.StartingPageId = varStartingPageId;

            item.NumRows = varNumRows;

            item.ShowElapsedTime = varShowElapsedTime;

            item.FeaturedOnly = varFeaturedOnly;

            item.CreatedOn = varCreatedOn;

            item.CreatedBy = varCreatedBy;

            item.ModifiedOn = varModifiedOn;

            item.ModifiedBy = varModifiedBy;


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
        public static void Update(int varModuleId, int? varStartingPageId, int? varNumRows, bool? varShowElapsedTime, bool? varFeaturedOnly, DateTime? varCreatedOn, string varCreatedBy, DateTime? varModifiedOn, string varModifiedBy)
        {
            MasterDetailMiniSummarySetting item = new MasterDetailMiniSummarySetting();

            item.ModuleId = varModuleId;

            item.StartingPageId = varStartingPageId;

            item.NumRows = varNumRows;

            item.ShowElapsedTime = varShowElapsedTime;

            item.FeaturedOnly = varFeaturedOnly;

            item.CreatedOn = varCreatedOn;

            item.CreatedBy = varCreatedBy;

            item.ModifiedOn = varModifiedOn;

            item.ModifiedBy = varModifiedBy;

            item.IsNew = false;
            if (System.Web.HttpContext.Current != null)
                item.Save(System.Web.HttpContext.Current.User.Identity.Name);
            else
                item.Save(System.Threading.Thread.CurrentPrincipal.Identity.Name);
        }
        #endregion



        #region Typed Columns


        public static TableSchema.TableColumn ModuleIdColumn
        {
            get { return Schema.Columns[0]; }
        }



        public static TableSchema.TableColumn StartingPageIdColumn
        {
            get { return Schema.Columns[1]; }
        }



        public static TableSchema.TableColumn NumRowsColumn
        {
            get { return Schema.Columns[2]; }
        }



        public static TableSchema.TableColumn ShowElapsedTimeColumn
        {
            get { return Schema.Columns[3]; }
        }



        public static TableSchema.TableColumn FeaturedOnlyColumn
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



        public static TableSchema.TableColumn ModifiedOnColumn
        {
            get { return Schema.Columns[7]; }
        }



        public static TableSchema.TableColumn ModifiedByColumn
        {
            get { return Schema.Columns[8]; }
        }



        #endregion
        #region Columns Struct
        public struct Columns
        {
            public static string ModuleId = @"ModuleId";
            public static string StartingPageId = @"StartingPageId";
            public static string NumRows = @"NumRows";
            public static string ShowElapsedTime = @"ShowElapsedTime";
            public static string FeaturedOnly = @"FeaturedOnly";
            public static string CreatedOn = @"CreatedOn";
            public static string CreatedBy = @"CreatedBy";
            public static string ModifiedOn = @"ModifiedOn";
            public static string ModifiedBy = @"ModifiedBy";

        }
        #endregion

        #region Update PK Collections

        #endregion

        #region Deep Save

        #endregion
    }
}
