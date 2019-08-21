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
    /// Controller class for SlideShow_Module
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SlideShowModuleController
    {
        // Preload our schema..
        SlideShowModule thisSchemaLoad = new SlideShowModule();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public SlideShowModuleCollection FetchAll()
        {
            SlideShowModuleCollection coll = new SlideShowModuleCollection();
            Query qry = new Query(SlideShowModule.Schema);
// Begin Bayshore custom code block (rread 6/26/07)
// Ignore records marked for deletion when doing a FetchAll
           	if (SlideShowModule.Schema.GetColumn("IsDeleted") != null)
        	{
        		qry.WHERE("IsDeleted <> true");
			}
			else if (SlideShowModule.Schema.GetColumn("Deleted") != null)
			{
				qry.WHERE("Deleted <> true");				
			}
// End Bayshore custom code block
            
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SlideShowModuleCollection FetchByID(object ModuleId)
        {
            SlideShowModuleCollection coll = new SlideShowModuleCollection().Where("ModuleId", ModuleId).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SlideShowModuleCollection FetchByQuery(Query qry)
        {
            SlideShowModuleCollection coll = new SlideShowModuleCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object ModuleId)
        {
            return (SlideShowModule.Destroy(ModuleId) == 1);
        }
        
        
		[DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object ModuleId)
        {
            return (SlideShowModule.Delete(ModuleId) == 1);
        }        
    	
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int ModuleId,short Height,short Width,short ImageDisplayTime,short ImageDisplayOrder,bool ImageLooping,short ImageFadeTime,int ImageXPosition,int ImageYPosition,string FlashFileName,int NavType,int WrapType,string SlideShowClassName)
	    {
		    SlideShowModule item = new SlideShowModule();
		    
            item.ModuleId = ModuleId;
            
            item.Height = Height;
            
            item.Width = Width;
            
            item.ImageDisplayTime = ImageDisplayTime;
            
            item.ImageDisplayOrder = ImageDisplayOrder;
            
            item.ImageLooping = ImageLooping;
            
            item.ImageFadeTime = ImageFadeTime;
            
            item.ImageXPosition = ImageXPosition;
            
            item.ImageYPosition = ImageYPosition;
            
            item.FlashFileName = FlashFileName;
            
            item.NavType = NavType;
            
            item.WrapType = WrapType;
            
            item.SlideShowClassName = SlideShowClassName;
            
	    
		    item.Save(UserName);
	    }
// Begin Bayshore custom code block (rread 7/16/07)
// Insert a record and return the Identity.
	    /// <summary>
	    /// Inserts a record and returns the Identity, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
	    public void InsertAndReturnIdentity(int ModuleId,short Height,short Width,short ImageDisplayTime,short ImageDisplayOrder,bool ImageLooping,short ImageFadeTime,int ImageXPosition,int ImageYPosition,string FlashFileName,int NavType,int WrapType,string SlideShowClassName, out object newId)
	    {
		    SlideShowModule item = new SlideShowModule();
		    
            item.ModuleId = ModuleId;
            
            item.Height = Height;
            
            item.Width = Width;
            
            item.ImageDisplayTime = ImageDisplayTime;
            
            item.ImageDisplayOrder = ImageDisplayOrder;
            
            item.ImageLooping = ImageLooping;
            
            item.ImageFadeTime = ImageFadeTime;
            
            item.ImageXPosition = ImageXPosition;
            
            item.ImageYPosition = ImageYPosition;
            
            item.FlashFileName = FlashFileName;
            
            item.NavType = NavType;
            
            item.WrapType = WrapType;
            
            item.SlideShowClassName = SlideShowClassName;
            
	    
		    item.Save(UserName);
		    
		    newId = item.ModuleId;
		    
	    }
// End Bayshore custom code block
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int ModuleId,short Height,short Width,short ImageDisplayTime,short ImageDisplayOrder,bool ImageLooping,short ImageFadeTime,int ImageXPosition,int ImageYPosition,string FlashFileName,int NavType,int WrapType,string SlideShowClassName)
	    {
		    SlideShowModule item = new SlideShowModule();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.ModuleId = ModuleId;
				
			item.Height = Height;
				
			item.Width = Width;
				
			item.ImageDisplayTime = ImageDisplayTime;
				
			item.ImageDisplayOrder = ImageDisplayOrder;
				
			item.ImageLooping = ImageLooping;
				
			item.ImageFadeTime = ImageFadeTime;
				
			item.ImageXPosition = ImageXPosition;
				
			item.ImageYPosition = ImageYPosition;
				
			item.FlashFileName = FlashFileName;
				
			item.NavType = NavType;
				
			item.WrapType = WrapType;
				
			item.SlideShowClassName = SlideShowClassName;
				
	        item.Save(UserName);
	    }
    }
}
