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
    /// Strongly-typed collection for the MasterDetailSetting class.
    /// </summary>
    [Serializable]
    public partial class MasterDetailSettingCollection : ActiveList<MasterDetailSetting, MasterDetailSettingCollection>
    {
        public MasterDetailSettingCollection()
        {
            base.ProviderName = "WebModules";

        }

        /// <summary>
        /// Filters an existing collection based on the set criteria. This is an in-memory filter
        /// Thanks to developingchris for this!
        /// </summary>
        /// <returns>MasterDetailSettingCollection</returns>
        public MasterDetailSettingCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                MasterDetailSetting o = this[i];
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
    /// This is an ActiveRecord class which wraps the MasterDetail_Settings table.
    /// </summary>
    [Serializable]
    public partial class MasterDetailSetting : ActiveRecord<MasterDetailSetting>, IActiveRecord
    {
        #region .ctors and Default Settings

        public MasterDetailSetting()
        {
            SetSQLProps();
            InitSetDefaults();
            MarkNew();
        }

        private void InitSetDefaults() { SetDefaults(); }

        public MasterDetailSetting(bool useDatabaseDefaults)
        {
            SetSQLProps();
            if (useDatabaseDefaults)
                ForceDefaults();
            MarkNew();
        }

        public MasterDetailSetting(object keyID)
        {
            SetSQLProps();
            InitSetDefaults();
            LoadByKey(keyID);
        }

        public MasterDetailSetting(string columnName, object columnValue)
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
                TableSchema.Table schema = new TableSchema.Table("MasterDetail_Settings", TableType.Table, DataService.GetInstance("WebModules"));
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

                TableSchema.TableColumn colvarIsPostDateVisible = new TableSchema.TableColumn(schema);
                colvarIsPostDateVisible.ColumnName = "IsPostDateVisible";
                colvarIsPostDateVisible.DataType = DbType.Boolean;
                colvarIsPostDateVisible.MaxLength = 0;
                colvarIsPostDateVisible.AutoIncrement = false;
                colvarIsPostDateVisible.IsNullable = false;
                colvarIsPostDateVisible.IsPrimaryKey = false;
                colvarIsPostDateVisible.IsForeignKey = false;
                colvarIsPostDateVisible.IsReadOnly = false;

                colvarIsPostDateVisible.DefaultSetting = @"((0))";
                colvarIsPostDateVisible.ForeignKeyTableName = "";
                schema.Columns.Add(colvarIsPostDateVisible);

                TableSchema.TableColumn colvarItemsPerPage = new TableSchema.TableColumn(schema);
                colvarItemsPerPage.ColumnName = "ItemsPerPage";
                colvarItemsPerPage.DataType = DbType.Int32;
                colvarItemsPerPage.MaxLength = 0;
                colvarItemsPerPage.AutoIncrement = false;
                colvarItemsPerPage.IsNullable = false;
                colvarItemsPerPage.IsPrimaryKey = false;
                colvarItemsPerPage.IsForeignKey = false;
                colvarItemsPerPage.IsReadOnly = false;

                colvarItemsPerPage.DefaultSetting = @"((10))";
                colvarItemsPerPage.ForeignKeyTableName = "";
                schema.Columns.Add(colvarItemsPerPage);

                TableSchema.TableColumn colvarRequireAuthentication = new TableSchema.TableColumn(schema);
                colvarRequireAuthentication.ColumnName = "RequireAuthentication";
                colvarRequireAuthentication.DataType = DbType.Boolean;
                colvarRequireAuthentication.MaxLength = 0;
                colvarRequireAuthentication.AutoIncrement = false;
                colvarRequireAuthentication.IsNullable = false;
                colvarRequireAuthentication.IsPrimaryKey = false;
                colvarRequireAuthentication.IsForeignKey = false;
                colvarRequireAuthentication.IsReadOnly = false;

                colvarRequireAuthentication.DefaultSetting = @"((0))";
                colvarRequireAuthentication.ForeignKeyTableName = "";
                schema.Columns.Add(colvarRequireAuthentication);

                TableSchema.TableColumn colvarAllowComments = new TableSchema.TableColumn(schema);
                colvarAllowComments.ColumnName = "AllowComments";
                colvarAllowComments.DataType = DbType.Boolean;
                colvarAllowComments.MaxLength = 0;
                colvarAllowComments.AutoIncrement = false;
                colvarAllowComments.IsNullable = false;
                colvarAllowComments.IsPrimaryKey = false;
                colvarAllowComments.IsForeignKey = false;
                colvarAllowComments.IsReadOnly = false;

                colvarAllowComments.DefaultSetting = @"((0))";
                colvarAllowComments.ForeignKeyTableName = "";
                schema.Columns.Add(colvarAllowComments);

                TableSchema.TableColumn colvarShowTagFilter = new TableSchema.TableColumn(schema);
                colvarShowTagFilter.ColumnName = "ShowTagFilter";
                colvarShowTagFilter.DataType = DbType.Boolean;
                colvarShowTagFilter.MaxLength = 0;
                colvarShowTagFilter.AutoIncrement = false;
                colvarShowTagFilter.IsNullable = false;
                colvarShowTagFilter.IsPrimaryKey = false;
                colvarShowTagFilter.IsForeignKey = false;
                colvarShowTagFilter.IsReadOnly = false;

                colvarShowTagFilter.DefaultSetting = @"((0))";
                colvarShowTagFilter.ForeignKeyTableName = "";
                schema.Columns.Add(colvarShowTagFilter);

                TableSchema.TableColumn colvarShowImageIfBlank = new TableSchema.TableColumn(schema);
                colvarShowImageIfBlank.ColumnName = "ShowImageIfBlank";
                colvarShowImageIfBlank.DataType = DbType.Boolean;
                colvarShowImageIfBlank.MaxLength = 0;
                colvarShowImageIfBlank.AutoIncrement = false;
                colvarShowImageIfBlank.IsNullable = false;
                colvarShowImageIfBlank.IsPrimaryKey = false;
                colvarShowImageIfBlank.IsForeignKey = false;
                colvarShowImageIfBlank.IsReadOnly = false;

                colvarShowImageIfBlank.DefaultSetting = @"((0))";
                colvarShowImageIfBlank.ForeignKeyTableName = "";
                schema.Columns.Add(colvarShowImageIfBlank);

                TableSchema.TableColumn colvarTemplate = new TableSchema.TableColumn(schema);
                colvarTemplate.ColumnName = "Template";
                colvarTemplate.DataType = DbType.AnsiString;
                colvarTemplate.MaxLength = 50;
                colvarTemplate.AutoIncrement = false;
                colvarTemplate.IsNullable = true;
                colvarTemplate.IsPrimaryKey = false;
                colvarTemplate.IsForeignKey = false;
                colvarTemplate.IsReadOnly = false;
                colvarTemplate.DefaultSetting = @"";
                colvarTemplate.ForeignKeyTableName = "";
                schema.Columns.Add(colvarTemplate);

                TableSchema.TableColumn colvarPageVisibleInNavigation = new TableSchema.TableColumn(schema);
                colvarPageVisibleInNavigation.ColumnName = "PageVisibleInNavigation";
                colvarPageVisibleInNavigation.DataType = DbType.Boolean;
                colvarPageVisibleInNavigation.MaxLength = 0;
                colvarPageVisibleInNavigation.AutoIncrement = false;
                colvarPageVisibleInNavigation.IsNullable = false;
                colvarPageVisibleInNavigation.IsPrimaryKey = false;
                colvarPageVisibleInNavigation.IsForeignKey = false;
                colvarPageVisibleInNavigation.IsReadOnly = false;
                colvarPageVisibleInNavigation.DefaultSetting = @"";
                colvarPageVisibleInNavigation.ForeignKeyTableName = "";
                schema.Columns.Add(colvarPageVisibleInNavigation);

                BaseSchema = schema;
                //add this schema to the provider
                //so we can query it later
                DataService.Providers["WebModules"].AddSchema("MasterDetail_Settings", schema);
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

        [XmlAttribute("IsPostDateVisible")]
        [Bindable(true)]
        public bool IsPostDateVisible
        {
            get { return GetColumnValue<bool>(Columns.IsPostDateVisible); }
            set { SetColumnValue(Columns.IsPostDateVisible, value); }
        }

        [XmlAttribute("ItemsPerPage")]
        [Bindable(true)]
        public int ItemsPerPage
        {
            get { return GetColumnValue<int>(Columns.ItemsPerPage); }
            set { SetColumnValue(Columns.ItemsPerPage, value); }
        }

        [XmlAttribute("RequireAuthentication")]
        [Bindable(true)]
        public bool RequireAuthentication
        {
            get { return GetColumnValue<bool>(Columns.RequireAuthentication); }
            set { SetColumnValue(Columns.RequireAuthentication, value); }
        }

        [XmlAttribute("AllowComments")]
        [Bindable(true)]
        public bool AllowComments
        {
            get { return GetColumnValue<bool>(Columns.AllowComments); }
            set { SetColumnValue(Columns.AllowComments, value); }
        }

        [XmlAttribute("ShowTagFilter")]
        [Bindable(true)]
        public bool ShowTagFilter
        {
            get { return GetColumnValue<bool>(Columns.ShowTagFilter); }
            set { SetColumnValue(Columns.ShowTagFilter, value); }
        }

        [XmlAttribute("ShowImageIfBlank")]
        [Bindable(true)]
        public bool ShowImageIfBlank
        {
            get { return GetColumnValue<bool>(Columns.ShowImageIfBlank); }
            set { SetColumnValue(Columns.ShowImageIfBlank, value); }
        }

        [XmlAttribute("Template")]
        [Bindable(true)]
        public string Template
        {
            get { return GetColumnValue<string>(Columns.Template); }
            set { SetColumnValue(Columns.Template, value); }
        }

        [XmlAttribute("PageVisibleInNavigation")]
        [Bindable(true)]
        public bool PageVisibleInNavigation
        {
            get { return GetColumnValue<bool>(Columns.PageVisibleInNavigation); }
            set { SetColumnValue(Columns.PageVisibleInNavigation, value); }
        }

        #endregion




        #region ForeignKey Properties

        #endregion



        //no ManyToMany tables defined (0)



        #region ObjectDataSource support


        /// <summary>
        /// Inserts a record, can be used with the Object Data Source
        /// </summary>
        public static void Insert(int varModuleId, bool varIsPostDateVisible, int varItemsPerPage, bool varRequireAuthentication, bool varAllowComments, bool varShowTagFilter, bool varShowImageIfBlank, string varTemplate, bool varPageVisibleInNavigation)
        {
            MasterDetailSetting item = new MasterDetailSetting();

            item.ModuleId = varModuleId;

            item.IsPostDateVisible = varIsPostDateVisible;

            item.ItemsPerPage = varItemsPerPage;

            item.RequireAuthentication = varRequireAuthentication;

            item.AllowComments = varAllowComments;

            item.ShowTagFilter = varShowTagFilter;

            item.ShowImageIfBlank = varShowImageIfBlank;

            item.Template = varTemplate;

            item.PageVisibleInNavigation = varPageVisibleInNavigation;


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
        public static MasterDetailSetting InsertAndReturnObject(int varModuleId, bool varIsPostDateVisible, int varItemsPerPage, bool varRequireAuthentication, bool varAllowComments, bool varShowTagFilter, bool varShowImageIfBlank, string varTemplate, bool varPageVisibleInNavigation)
        {
            MasterDetailSetting item = new MasterDetailSetting();

            item.ModuleId = varModuleId;

            item.IsPostDateVisible = varIsPostDateVisible;

            item.ItemsPerPage = varItemsPerPage;

            item.RequireAuthentication = varRequireAuthentication;

            item.AllowComments = varAllowComments;

            item.ShowTagFilter = varShowTagFilter;

            item.ShowImageIfBlank = varShowImageIfBlank;

            item.Template = varTemplate;

            item.PageVisibleInNavigation = varPageVisibleInNavigation;


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
        public static void Update(int varModuleId, bool varIsPostDateVisible, int varItemsPerPage, bool varRequireAuthentication, bool varAllowComments, bool varShowTagFilter, bool varShowImageIfBlank, string varTemplate, bool varPageVisibleInNavigation)
        {
            MasterDetailSetting item = new MasterDetailSetting();

            item.ModuleId = varModuleId;

            item.IsPostDateVisible = varIsPostDateVisible;

            item.ItemsPerPage = varItemsPerPage;

            item.RequireAuthentication = varRequireAuthentication;

            item.AllowComments = varAllowComments;

            item.ShowTagFilter = varShowTagFilter;

            item.ShowImageIfBlank = varShowImageIfBlank;

            item.Template = varTemplate;

            item.PageVisibleInNavigation = varPageVisibleInNavigation;

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



        public static TableSchema.TableColumn IsPostDateVisibleColumn
        {
            get { return Schema.Columns[1]; }
        }



        public static TableSchema.TableColumn ItemsPerPageColumn
        {
            get { return Schema.Columns[2]; }
        }



        public static TableSchema.TableColumn RequireAuthenticationColumn
        {
            get { return Schema.Columns[3]; }
        }



        public static TableSchema.TableColumn AllowCommentsColumn
        {
            get { return Schema.Columns[4]; }
        }



        public static TableSchema.TableColumn ShowTagFilterColumn
        {
            get { return Schema.Columns[5]; }
        }



        public static TableSchema.TableColumn ShowImageIfBlankColumn
        {
            get { return Schema.Columns[6]; }
        }



        public static TableSchema.TableColumn TemplateColumn
        {
            get { return Schema.Columns[7]; }
        }



        public static TableSchema.TableColumn PageVisibleInNavigationColumn
        {
            get { return Schema.Columns[8]; }
        }



        #endregion
        #region Columns Struct
        public struct Columns
        {
            public static string ModuleId = @"ModuleId";
            public static string IsPostDateVisible = @"IsPostDateVisible";
            public static string ItemsPerPage = @"ItemsPerPage";
            public static string RequireAuthentication = @"RequireAuthentication";
            public static string AllowComments = @"AllowComments";
            public static string ShowTagFilter = @"ShowTagFilter";
            public static string ShowImageIfBlank = @"ShowImageIfBlank";
            public static string Template = @"Template";
            public static string PageVisibleInNavigation = @"PageVisibleInNavigation";

        }
        #endregion

        #region Update PK Collections

        #endregion

        #region Deep Save

        #endregion
    }
}
