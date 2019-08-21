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
namespace BayshoreSolutions.WebModules.SlideShow{
    public partial class SPs{
        
        /// <summary>
        /// Creates an object wrapper for the SlideShow_Image_MovePosition Procedure
        /// </summary>
        public static StoredProcedure SlideShowImageMovePosition(int? id, bool? direction)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("SlideShow_Image_MovePosition", DataService.GetInstance("WebModules"), "dbo");
        	
            sp.Command.AddParameter("@id", id, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@direction", direction, DbType.Boolean, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the SlideShow_Image_RebuildSortOrder Procedure
        /// </summary>
        public static StoredProcedure SlideShowImageRebuildSortOrder(int? moduleId)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("SlideShow_Image_RebuildSortOrder", DataService.GetInstance("WebModules"), "dbo");
        	
            sp.Command.AddParameter("@moduleId", moduleId, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
    }
    
}
