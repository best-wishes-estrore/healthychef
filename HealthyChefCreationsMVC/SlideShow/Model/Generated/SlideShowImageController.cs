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
    /// Controller class for SlideShow_Image
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class SlideShowImageController
    {
        // Preload our schema..
        SlideShowImage thisSchemaLoad = new SlideShowImage();
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
        public SlideShowImageCollection FetchAll()
        {
            SlideShowImageCollection coll = new SlideShowImageCollection();
            Query qry = new Query(SlideShowImage.Schema);
// Begin Bayshore custom code block (rread 6/26/07)
// Ignore records marked for deletion when doing a FetchAll
           	if (SlideShowImage.Schema.GetColumn("IsDeleted") != null)
        	{
        		qry.WHERE("IsDeleted <> true");
			}
			else if (SlideShowImage.Schema.GetColumn("Deleted") != null)
			{
				qry.WHERE("Deleted <> true");				
			}
// End Bayshore custom code block
            
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public SlideShowImageCollection FetchByID(object Id)
        {
            SlideShowImageCollection coll = new SlideShowImageCollection().Where("Id", Id).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public SlideShowImageCollection FetchByQuery(Query qry)
        {
            SlideShowImageCollection coll = new SlideShowImageCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object Id)
        {
            return (SlideShowImage.Destroy(Id) == 1);
        }
        
        
		[DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object Id)
        {
            return (SlideShowImage.Delete(Id) == 1);
        }        
    	
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(int ModuleId,string ImageFileName,int SortOrder,string LinkUrl,string SlideTextContent,string SlideTextContentName)
	    {
		    SlideShowImage item = new SlideShowImage();
		    
            item.ModuleId = ModuleId;
            
            item.ImageFileName = ImageFileName;
            
            item.SortOrder = SortOrder;
            
            item.LinkUrl = LinkUrl;
            
            item.SlideTextContent = SlideTextContent;
            
            item.SlideTextContentName = SlideTextContentName;
            
	    
		    item.Save(UserName);
	    }
// Begin Bayshore custom code block (rread 7/16/07)
// Insert a record and return the Identity.
	    /// <summary>
	    /// Inserts a record and returns the Identity, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, false)]
	    public void InsertAndReturnIdentity(int ModuleId,string ImageFileName,int SortOrder,string LinkUrl,string SlideTextContent,string SlideTextContentName, out object newId)
	    {
		    SlideShowImage item = new SlideShowImage();
		    
            item.ModuleId = ModuleId;
            
            item.ImageFileName = ImageFileName;
            
            item.SortOrder = SortOrder;
            
            item.LinkUrl = LinkUrl;
            
            item.SlideTextContent = SlideTextContent;
            
            item.SlideTextContentName = SlideTextContentName;
            
	    
		    item.Save(UserName);
		    
		    newId = item.Id;
		    
	    }
// End Bayshore custom code block
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int Id,int ModuleId,string ImageFileName,int SortOrder,string LinkUrl,string SlideTextContent,string SlideTextContentName)
	    {
		    SlideShowImage item = new SlideShowImage();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.Id = Id;
				
			item.ModuleId = ModuleId;
				
			item.ImageFileName = ImageFileName;
				
			item.SortOrder = SortOrder;
				
			item.LinkUrl = LinkUrl;
				
			item.SlideTextContent = SlideTextContent;
				
			item.SlideTextContentName = SlideTextContentName;
				
	        item.Save(UserName);
	    }
    }
}
