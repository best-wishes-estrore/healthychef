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
	/// Strongly-typed collection for the SlideShowModule class.
	/// </summary>
    [Serializable]
	public partial class SlideShowModuleCollection : ActiveList<SlideShowModule, SlideShowModuleCollection>
	{	   
		public SlideShowModuleCollection() {
		    base.ProviderName = "WebModules";
		
		}
        
        /// <summary>
		/// Filters an existing collection based on the set criteria. This is an in-memory filter
		/// Thanks to developingchris for this!
        /// </summary>
        /// <returns>SlideShowModuleCollection</returns>
		public SlideShowModuleCollection Filter()
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                SlideShowModule o = this[i];
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
	/// This is an ActiveRecord class which wraps the SlideShow_Module table.
	/// </summary>
	[Serializable]
	public partial class SlideShowModule : ActiveRecord<SlideShowModule>, IActiveRecord
	{
		#region .ctors and Default Settings
		
		public SlideShowModule()
		{
		  SetSQLProps();
		  InitSetDefaults();
		  MarkNew();
		}
		
		private void InitSetDefaults() { SetDefaults(); }
		
		public SlideShowModule(bool useDatabaseDefaults)
		{
			SetSQLProps();
			if(useDatabaseDefaults)
				ForceDefaults();
			MarkNew();
		}
        
		public SlideShowModule(object keyID)
		{
			SetSQLProps();
			InitSetDefaults();
			LoadByKey(keyID);
		}
		 
		public SlideShowModule(string columnName, object columnValue)
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
				TableSchema.Table schema = new TableSchema.Table("SlideShow_Module", TableType.Table, DataService.GetInstance("WebModules"));
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
				colvarModuleId.IsForeignKey = false;
				colvarModuleId.IsReadOnly = false;
				colvarModuleId.DefaultSetting = @"";
				colvarModuleId.ForeignKeyTableName = "";
				schema.Columns.Add(colvarModuleId);
				
				TableSchema.TableColumn colvarHeight = new TableSchema.TableColumn(schema);
				colvarHeight.ColumnName = "Height";
				colvarHeight.DataType = DbType.Int16;
				colvarHeight.MaxLength = 0;
				colvarHeight.AutoIncrement = false;
				colvarHeight.IsNullable = false;
				colvarHeight.IsPrimaryKey = false;
				colvarHeight.IsForeignKey = false;
				colvarHeight.IsReadOnly = false;
				
						colvarHeight.DefaultSetting = @"((100))";
				colvarHeight.ForeignKeyTableName = "";
				schema.Columns.Add(colvarHeight);
				
				TableSchema.TableColumn colvarWidth = new TableSchema.TableColumn(schema);
				colvarWidth.ColumnName = "Width";
				colvarWidth.DataType = DbType.Int16;
				colvarWidth.MaxLength = 0;
				colvarWidth.AutoIncrement = false;
				colvarWidth.IsNullable = false;
				colvarWidth.IsPrimaryKey = false;
				colvarWidth.IsForeignKey = false;
				colvarWidth.IsReadOnly = false;
				
						colvarWidth.DefaultSetting = @"((100))";
				colvarWidth.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWidth);
				
				TableSchema.TableColumn colvarImageDisplayTime = new TableSchema.TableColumn(schema);
				colvarImageDisplayTime.ColumnName = "ImageDisplayTime";
				colvarImageDisplayTime.DataType = DbType.Int16;
				colvarImageDisplayTime.MaxLength = 0;
				colvarImageDisplayTime.AutoIncrement = false;
				colvarImageDisplayTime.IsNullable = false;
				colvarImageDisplayTime.IsPrimaryKey = false;
				colvarImageDisplayTime.IsForeignKey = false;
				colvarImageDisplayTime.IsReadOnly = false;
				
						colvarImageDisplayTime.DefaultSetting = @"((6))";
				colvarImageDisplayTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageDisplayTime);
				
				TableSchema.TableColumn colvarImageDisplayOrder = new TableSchema.TableColumn(schema);
				colvarImageDisplayOrder.ColumnName = "ImageDisplayOrder";
				colvarImageDisplayOrder.DataType = DbType.Int16;
				colvarImageDisplayOrder.MaxLength = 0;
				colvarImageDisplayOrder.AutoIncrement = false;
				colvarImageDisplayOrder.IsNullable = false;
				colvarImageDisplayOrder.IsPrimaryKey = false;
				colvarImageDisplayOrder.IsForeignKey = false;
				colvarImageDisplayOrder.IsReadOnly = false;
				
						colvarImageDisplayOrder.DefaultSetting = @"((0))";
				colvarImageDisplayOrder.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageDisplayOrder);
				
				TableSchema.TableColumn colvarImageLooping = new TableSchema.TableColumn(schema);
				colvarImageLooping.ColumnName = "ImageLooping";
				colvarImageLooping.DataType = DbType.Boolean;
				colvarImageLooping.MaxLength = 0;
				colvarImageLooping.AutoIncrement = false;
				colvarImageLooping.IsNullable = false;
				colvarImageLooping.IsPrimaryKey = false;
				colvarImageLooping.IsForeignKey = false;
				colvarImageLooping.IsReadOnly = false;
				
						colvarImageLooping.DefaultSetting = @"((1))";
				colvarImageLooping.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageLooping);
				
				TableSchema.TableColumn colvarImageFadeTime = new TableSchema.TableColumn(schema);
				colvarImageFadeTime.ColumnName = "ImageFadeTime";
				colvarImageFadeTime.DataType = DbType.Int16;
				colvarImageFadeTime.MaxLength = 0;
				colvarImageFadeTime.AutoIncrement = false;
				colvarImageFadeTime.IsNullable = false;
				colvarImageFadeTime.IsPrimaryKey = false;
				colvarImageFadeTime.IsForeignKey = false;
				colvarImageFadeTime.IsReadOnly = false;
				
						colvarImageFadeTime.DefaultSetting = @"((1))";
				colvarImageFadeTime.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageFadeTime);
				
				TableSchema.TableColumn colvarImageXPosition = new TableSchema.TableColumn(schema);
				colvarImageXPosition.ColumnName = "ImageXPosition";
				colvarImageXPosition.DataType = DbType.Int32;
				colvarImageXPosition.MaxLength = 0;
				colvarImageXPosition.AutoIncrement = false;
				colvarImageXPosition.IsNullable = false;
				colvarImageXPosition.IsPrimaryKey = false;
				colvarImageXPosition.IsForeignKey = false;
				colvarImageXPosition.IsReadOnly = false;
				
						colvarImageXPosition.DefaultSetting = @"((0))";
				colvarImageXPosition.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageXPosition);
				
				TableSchema.TableColumn colvarImageYPosition = new TableSchema.TableColumn(schema);
				colvarImageYPosition.ColumnName = "ImageYPosition";
				colvarImageYPosition.DataType = DbType.Int32;
				colvarImageYPosition.MaxLength = 0;
				colvarImageYPosition.AutoIncrement = false;
				colvarImageYPosition.IsNullable = false;
				colvarImageYPosition.IsPrimaryKey = false;
				colvarImageYPosition.IsForeignKey = false;
				colvarImageYPosition.IsReadOnly = false;
				
						colvarImageYPosition.DefaultSetting = @"((0))";
				colvarImageYPosition.ForeignKeyTableName = "";
				schema.Columns.Add(colvarImageYPosition);
				
				TableSchema.TableColumn colvarFlashFileName = new TableSchema.TableColumn(schema);
				colvarFlashFileName.ColumnName = "FlashFileName";
				colvarFlashFileName.DataType = DbType.String;
				colvarFlashFileName.MaxLength = 500;
				colvarFlashFileName.AutoIncrement = false;
				colvarFlashFileName.IsNullable = true;
				colvarFlashFileName.IsPrimaryKey = false;
				colvarFlashFileName.IsForeignKey = false;
				colvarFlashFileName.IsReadOnly = false;
				colvarFlashFileName.DefaultSetting = @"";
				colvarFlashFileName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarFlashFileName);
				
				TableSchema.TableColumn colvarNavType = new TableSchema.TableColumn(schema);
				colvarNavType.ColumnName = "NavType";
				colvarNavType.DataType = DbType.Int32;
				colvarNavType.MaxLength = 0;
				colvarNavType.AutoIncrement = false;
				colvarNavType.IsNullable = false;
				colvarNavType.IsPrimaryKey = false;
				colvarNavType.IsForeignKey = false;
				colvarNavType.IsReadOnly = false;
				
						colvarNavType.DefaultSetting = @"((0))";
				colvarNavType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarNavType);
				
				TableSchema.TableColumn colvarWrapType = new TableSchema.TableColumn(schema);
				colvarWrapType.ColumnName = "WrapType";
				colvarWrapType.DataType = DbType.Int32;
				colvarWrapType.MaxLength = 0;
				colvarWrapType.AutoIncrement = false;
				colvarWrapType.IsNullable = false;
				colvarWrapType.IsPrimaryKey = false;
				colvarWrapType.IsForeignKey = false;
				colvarWrapType.IsReadOnly = false;
				
						colvarWrapType.DefaultSetting = @"((0))";
				colvarWrapType.ForeignKeyTableName = "";
				schema.Columns.Add(colvarWrapType);
				
				TableSchema.TableColumn colvarSlideShowClassName = new TableSchema.TableColumn(schema);
				colvarSlideShowClassName.ColumnName = "SlideShowClassName";
				colvarSlideShowClassName.DataType = DbType.String;
				colvarSlideShowClassName.MaxLength = 30;
				colvarSlideShowClassName.AutoIncrement = false;
				colvarSlideShowClassName.IsNullable = true;
				colvarSlideShowClassName.IsPrimaryKey = false;
				colvarSlideShowClassName.IsForeignKey = false;
				colvarSlideShowClassName.IsReadOnly = false;
				colvarSlideShowClassName.DefaultSetting = @"";
				colvarSlideShowClassName.ForeignKeyTableName = "";
				schema.Columns.Add(colvarSlideShowClassName);
				
				BaseSchema = schema;
				//add this schema to the provider
				//so we can query it later
				DataService.Providers["WebModules"].AddSchema("SlideShow_Module",schema);
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
		  
		[XmlAttribute("Height")]
		[Bindable(true)]
		public short Height 
		{
			get { return GetColumnValue<short>(Columns.Height); }
			set { SetColumnValue(Columns.Height, value); }
		}
		  
		[XmlAttribute("Width")]
		[Bindable(true)]
		public short Width 
		{
			get { return GetColumnValue<short>(Columns.Width); }
			set { SetColumnValue(Columns.Width, value); }
		}
		  
		[XmlAttribute("ImageDisplayTime")]
		[Bindable(true)]
		public short ImageDisplayTime 
		{
			get { return GetColumnValue<short>(Columns.ImageDisplayTime); }
			set { SetColumnValue(Columns.ImageDisplayTime, value); }
		}
		  
		[XmlAttribute("ImageDisplayOrder")]
		[Bindable(true)]
		public short ImageDisplayOrder 
		{
			get { return GetColumnValue<short>(Columns.ImageDisplayOrder); }
			set { SetColumnValue(Columns.ImageDisplayOrder, value); }
		}
		  
		[XmlAttribute("ImageLooping")]
		[Bindable(true)]
		public bool ImageLooping 
		{
			get { return GetColumnValue<bool>(Columns.ImageLooping); }
			set { SetColumnValue(Columns.ImageLooping, value); }
		}
		  
		[XmlAttribute("ImageFadeTime")]
		[Bindable(true)]
		public short ImageFadeTime 
		{
			get { return GetColumnValue<short>(Columns.ImageFadeTime); }
			set { SetColumnValue(Columns.ImageFadeTime, value); }
		}
		  
		[XmlAttribute("ImageXPosition")]
		[Bindable(true)]
		public int ImageXPosition 
		{
			get { return GetColumnValue<int>(Columns.ImageXPosition); }
			set { SetColumnValue(Columns.ImageXPosition, value); }
		}
		  
		[XmlAttribute("ImageYPosition")]
		[Bindable(true)]
		public int ImageYPosition 
		{
			get { return GetColumnValue<int>(Columns.ImageYPosition); }
			set { SetColumnValue(Columns.ImageYPosition, value); }
		}
		  
		[XmlAttribute("FlashFileName")]
		[Bindable(true)]
		public string FlashFileName 
		{
			get { return GetColumnValue<string>(Columns.FlashFileName); }
			set { SetColumnValue(Columns.FlashFileName, value); }
		}
		  
		[XmlAttribute("NavType")]
		[Bindable(true)]
		public int NavType 
		{
			get { return GetColumnValue<int>(Columns.NavType); }
			set { SetColumnValue(Columns.NavType, value); }
		}
		  
		[XmlAttribute("WrapType")]
		[Bindable(true)]
		public int WrapType 
		{
			get { return GetColumnValue<int>(Columns.WrapType); }
			set { SetColumnValue(Columns.WrapType, value); }
		}
		  
		[XmlAttribute("SlideShowClassName")]
		[Bindable(true)]
		public string SlideShowClassName 
		{
			get { return GetColumnValue<string>(Columns.SlideShowClassName); }
			set { SetColumnValue(Columns.SlideShowClassName, value); }
		}
		
		#endregion
		
		
		#region PrimaryKey Methods		
		
        protected override void SetPrimaryKey(object oValue)
        {
            base.SetPrimaryKey(oValue);
            
            SetPKValues();
        }
        
		
		public BayshoreSolutions.WebModules.SlideShow.SlideShowImageCollection SlideShowImageRecords
		{
			get { return new BayshoreSolutions.WebModules.SlideShow.SlideShowImageCollection().Where(SlideShowImage.Columns.ModuleId, ModuleId).Load(); }
		}
		#endregion
		
			
		
		//no foreign key tables defined (0)
		
		
		
		//no ManyToMany tables defined (0)
		
        
        
		#region ObjectDataSource support
		
		
		/// <summary>
		/// Inserts a record, can be used with the Object Data Source
		/// </summary>
		public static void Insert(int varModuleId,short varHeight,short varWidth,short varImageDisplayTime,short varImageDisplayOrder,bool varImageLooping,short varImageFadeTime,int varImageXPosition,int varImageYPosition,string varFlashFileName,int varNavType,int varWrapType,string varSlideShowClassName)
		{
			SlideShowModule item = new SlideShowModule();
			
			item.ModuleId = varModuleId;
			
			item.Height = varHeight;
			
			item.Width = varWidth;
			
			item.ImageDisplayTime = varImageDisplayTime;
			
			item.ImageDisplayOrder = varImageDisplayOrder;
			
			item.ImageLooping = varImageLooping;
			
			item.ImageFadeTime = varImageFadeTime;
			
			item.ImageXPosition = varImageXPosition;
			
			item.ImageYPosition = varImageYPosition;
			
			item.FlashFileName = varFlashFileName;
			
			item.NavType = varNavType;
			
			item.WrapType = varWrapType;
			
			item.SlideShowClassName = varSlideShowClassName;
			
		
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
	    public static SlideShowModule InsertAndReturnObject(int varModuleId,short varHeight,short varWidth,short varImageDisplayTime,short varImageDisplayOrder,bool varImageLooping,short varImageFadeTime,int varImageXPosition,int varImageYPosition,string varFlashFileName,int varNavType,int varWrapType,string varSlideShowClassName)
	    {
		    SlideShowModule item = new SlideShowModule();
		    
            item.ModuleId = varModuleId;
            
            item.Height = varHeight;
            
            item.Width = varWidth;
            
            item.ImageDisplayTime = varImageDisplayTime;
            
            item.ImageDisplayOrder = varImageDisplayOrder;
            
            item.ImageLooping = varImageLooping;
            
            item.ImageFadeTime = varImageFadeTime;
            
            item.ImageXPosition = varImageXPosition;
            
            item.ImageYPosition = varImageYPosition;
            
            item.FlashFileName = varFlashFileName;
            
            item.NavType = varNavType;
            
            item.WrapType = varWrapType;
            
            item.SlideShowClassName = varSlideShowClassName;
            
	    
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
		public static void Update(int varModuleId,short varHeight,short varWidth,short varImageDisplayTime,short varImageDisplayOrder,bool varImageLooping,short varImageFadeTime,int varImageXPosition,int varImageYPosition,string varFlashFileName,int varNavType,int varWrapType,string varSlideShowClassName)
		{
			SlideShowModule item = new SlideShowModule();
			
				item.ModuleId = varModuleId;
			
				item.Height = varHeight;
			
				item.Width = varWidth;
			
				item.ImageDisplayTime = varImageDisplayTime;
			
				item.ImageDisplayOrder = varImageDisplayOrder;
			
				item.ImageLooping = varImageLooping;
			
				item.ImageFadeTime = varImageFadeTime;
			
				item.ImageXPosition = varImageXPosition;
			
				item.ImageYPosition = varImageYPosition;
			
				item.FlashFileName = varFlashFileName;
			
				item.NavType = varNavType;
			
				item.WrapType = varWrapType;
			
				item.SlideShowClassName = varSlideShowClassName;
			
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
        
        
        
        public static TableSchema.TableColumn HeightColumn
        {
            get { return Schema.Columns[1]; }
        }
        
        
        
        public static TableSchema.TableColumn WidthColumn
        {
            get { return Schema.Columns[2]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageDisplayTimeColumn
        {
            get { return Schema.Columns[3]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageDisplayOrderColumn
        {
            get { return Schema.Columns[4]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageLoopingColumn
        {
            get { return Schema.Columns[5]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageFadeTimeColumn
        {
            get { return Schema.Columns[6]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageXPositionColumn
        {
            get { return Schema.Columns[7]; }
        }
        
        
        
        public static TableSchema.TableColumn ImageYPositionColumn
        {
            get { return Schema.Columns[8]; }
        }
        
        
        
        public static TableSchema.TableColumn FlashFileNameColumn
        {
            get { return Schema.Columns[9]; }
        }
        
        
        
        public static TableSchema.TableColumn NavTypeColumn
        {
            get { return Schema.Columns[10]; }
        }
        
        
        
        public static TableSchema.TableColumn WrapTypeColumn
        {
            get { return Schema.Columns[11]; }
        }
        
        
        
        public static TableSchema.TableColumn SlideShowClassNameColumn
        {
            get { return Schema.Columns[12]; }
        }
        
        
        
        #endregion
		#region Columns Struct
		public struct Columns
		{
			 public static string ModuleId = @"ModuleId";
			 public static string Height = @"Height";
			 public static string Width = @"Width";
			 public static string ImageDisplayTime = @"ImageDisplayTime";
			 public static string ImageDisplayOrder = @"ImageDisplayOrder";
			 public static string ImageLooping = @"ImageLooping";
			 public static string ImageFadeTime = @"ImageFadeTime";
			 public static string ImageXPosition = @"ImageXPosition";
			 public static string ImageYPosition = @"ImageYPosition";
			 public static string FlashFileName = @"FlashFileName";
			 public static string NavType = @"NavType";
			 public static string WrapType = @"WrapType";
			 public static string SlideShowClassName = @"SlideShowClassName";
						
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
