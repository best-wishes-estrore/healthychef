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
namespace BayshoreSolutions.WebModules.SlideShow
{
	/// <summary>
	/// Strongly-typed collection for the SlideShowImage class.
	/// </summary>
    [Serializable]
	public partial class SlideShowImageCollection : ActiveList<SlideShowImage, SlideShowImageCollection>
	{	   
		public SlideShowImageCollection() {
		    base.ProviderName = "WebModules";
		
		}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SlideShowImageCollection</returns>
		public SlideShowImageCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SlideShowImage o = this[i];
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
	/// This is an ActiveRecord class which wraps the SlideShow_Image table.
	/// </summary>
	[Serializable]
	public partial class SlideShowImage : ActiveRecord<SlideShowImage>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SlideShowImage()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SlideShowImage(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SlideShowImage(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SlideShowImage(string columnName, object columnValue)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByParam(columnName,columnValue);
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
			if(!IsSchemaInitialized)
			{
				//Schema declaration
				TableSchema.Table schema = new TableSchema.Table("SlideShow_Image", TableType.Table, DataService.GetInstance("WebModules"));
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
				
					colvarModuleId.ForeignKeyTableName = "SlideShow_Module";
				schema.Columns.Add(colvarModuleId);
				
				TableSchema.TableColumn colvarImageFileName = new TableSchema.TableColumn(schema);
				colvarImageFileName.ColumnName = "ImageFileName";
				colvarImageFileName.DataType = DbType.String;
				colvarImageFileName.MaxLength = 500;
				colvarImageFileName.AutoIncrement = false;
				colvarImageFileName.IsNullable = true;
				colvarImageFileName.IsPrimaryKey = false;
				colvarImageFileName.IsForeignKey = false;
				colvarImageFileName.IsReadOnly = false;
				colvarImageFileName.DefaultSetting = @"";
				colvarImageFileName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageFileName);
				
				TableSchema.TableColumn colvarSortOrder = new TableSchema.TableColumn(schema);
				colvarSortOrder.ColumnName = "SortOrder";
				colvarSortOrder.DataType = DbType.Int32;
				colvarSortOrder.MaxLength = 0;
				colvarSortOrder.AutoIncrement = false;
				colvarSortOrder.IsNullable = false;
				colvarSortOrder.IsPrimaryKey = false;
				colvarSortOrder.IsForeignKey = false;
				colvarSortOrder.IsReadOnly = false;
				
						colvarSortOrder.DefaultSetting = @"((1))";
				colvarSortOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSortOrder);
				
				TableSchema.TableColumn colvarLinkUrl = new TableSchema.TableColumn(schema);
				colvarLinkUrl.ColumnName = "LinkUrl";
				colvarLinkUrl.DataType = DbType.String;
				colvarLinkUrl.MaxLength = 1000;
				colvarLinkUrl.AutoIncrement = false;
				colvarLinkUrl.IsNullable = true;
				colvarLinkUrl.IsPrimaryKey = false;
				colvarLinkUrl.IsForeignKey = false;
				colvarLinkUrl.IsReadOnly = false;
				colvarLinkUrl.DefaultSetting = @"";
				colvarLinkUrl.ForeignKeyTableName = "";
				schema.Columns.Add(colvarLinkUrl);
				
				TableSchema.TableColumn colvarSlideTextContent = new TableSchema.TableColumn(schema);
				colvarSlideTextContent.ColumnName = "SlideTextContent";
				colvarSlideTextContent.DataType = DbType.String;
				colvarSlideTextContent.MaxLength = -1;
				colvarSlideTextContent.AutoIncrement = false;
				colvarSlideTextContent.IsNullable = true;
				colvarSlideTextContent.IsPrimaryKey = false;
				colvarSlideTextContent.IsForeignKey = false;
				colvarSlideTextContent.IsReadOnly = false;
				colvarSlideTextContent.DefaultSetting = @"";
				colvarSlideTextContent.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSlideTextContent);
				
				TableSchema.TableColumn colvarSlideTextContentName = new TableSchema.TableColumn(schema);
				colvarSlideTextContentName.ColumnName = "SlideTextContentName";
				colvarSlideTextContentName.DataType = DbType.String;
				colvarSlideTextContentName.MaxLength = 500;
				colvarSlideTextContentName.AutoIncrement = false;
				colvarSlideTextContentName.IsNullable = true;
				colvarSlideTextContentName.IsPrimaryKey = false;
				colvarSlideTextContentName.IsForeignKey = false;
				colvarSlideTextContentName.IsReadOnly = false;
				colvarSlideTextContentName.DefaultSetting = @"";
				colvarSlideTextContentName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSlideTextContentName);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WebModules"].AddSchema("SlideShow_Image",schema);
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
		  
		[XmlAttribute("ImageFileName")]
		[Bindable(true)]
		public string ImageFileName 
		{
			get { return GetColumnValue<string>(Columns.ImageFileName); }
			set { SetColumnValue(Columns.ImageFileName, value); }
		}
		  
		[XmlAttribute("SortOrder")]
		[Bindable(true)]
		public int SortOrder 
		{
			get { return GetColumnValue<int>(Columns.SortOrder); }
			set { SetColumnValue(Columns.SortOrder, value); }
		}
		  
		[XmlAttribute("LinkUrl")]
		[Bindable(true)]
		public string LinkUrl 
		{
			get { return GetColumnValue<string>(Columns.LinkUrl); }
			set { SetColumnValue(Columns.LinkUrl, value); }
		}
		  
		[XmlAttribute("SlideTextContent")]
		[Bindable(true)]
		public string SlideTextContent 
		{
			get { return GetColumnValue<string>(Columns.SlideTextContent); }
			set { SetColumnValue(Columns.SlideTextContent, value); }
		}
		  
		[XmlAttribute("SlideTextContentName")]
		[Bindable(true)]
		public string SlideTextContentName 
		{
			get { return GetColumnValue<string>(Columns.SlideTextContentName); }
			set { SetColumnValue(Columns.SlideTextContentName, value); }
		}
		
		#endregion
		
		
			
		
		#region ForeignKey Properties
		
		/// <summary>
		/// Returns a SlideShowModule ActiveRecord object related to this SlideShowImage
		/// 
		/// </summary>
		public BayshoreSolutions.WebModules.SlideShow.SlideShowModule SlideShowModule
		{
			get { return BayshoreSolutions.WebModules.SlideShow.SlideShowModule.FetchByID(this.ModuleId); }
			set { SetColumnValue("ModuleId", value.ModuleId); }
		}
		
		
		#endregion
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varModuleId,string varImageFileName,int varSortOrder,string varLinkUrl,string varSlideTextContent,string varSlideTextContentName)
		{
			SlideShowImage item = new SlideShowImage();
			
			item.ModuleId = varModuleId;
			
			item.ImageFileName = varImageFileName;
			
			item.SortOrder = varSortOrder;
			
			item.LinkUrl = varLinkUrl;
			
			item.SlideTextContent = varSlideTextContent;
			
			item.SlideTextContentName = varSlideTextContentName;
			
		
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
	    public static SlideShowImage InsertAndReturnObject(int varModuleId,string varImageFileName,int varSortOrder,string varLinkUrl,string varSlideTextContent,string varSlideTextContentName)
	    {
		    SlideShowImage item = new SlideShowImage();
		    
            item.ModuleId = varModuleId;
            
            item.ImageFileName = varImageFileName;
            
            item.SortOrder = varSortOrder;
            
            item.LinkUrl = varLinkUrl;
            
            item.SlideTextContent = varSlideTextContent;
            
            item.SlideTextContentName = varSlideTextContentName;
            
	    
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
		public static void Update(int varId,int varModuleId,string varImageFileName,int varSortOrder,string varLinkUrl,string varSlideTextContent,string varSlideTextContentName)
		{
			SlideShowImage item = new SlideShowImage();
			
				item.Id = varId;
			
				item.ModuleId = varModuleId;
			
				item.ImageFileName = varImageFileName;
			
				item.SortOrder = varSortOrder;
			
				item.LinkUrl = varLinkUrl;
			
				item.SlideTextContent = varSlideTextContent;
			
				item.SlideTextContentName = varSlideTextContentName;
			
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
        
        
        
        public static TableSchema.TableColumn ImageFileNameColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn SortOrderColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn LinkUrlColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn SlideTextContentColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn SlideTextContentNameColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string Id = @"Id";
			 public static string ModuleId = @"ModuleId";
			 public static string ImageFileName = @"ImageFileName";
			 public static string SortOrder = @"SortOrder";
			 public static string LinkUrl = @"LinkUrl";
			 public static string SlideTextContent = @"SlideTextContent";
			 public static string SlideTextContentName = @"SlideTextContentName";
						
		}
		#endregion
		
		#region Update PK Collections
		
        #endregion
    
        #region Deep Save
		
        #endregion
	}
}
