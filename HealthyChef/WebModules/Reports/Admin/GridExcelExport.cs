using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BayshoreSolutions.Common.Web;

namespace HealthyChef.WebModules.Reports.Admin
{
    public static class GridExcelExport
    {
        public static void ExcelExport(GridView grid, HttpResponse response, string filename)
        {
            grid.AllowPaging = false;
            grid.Columns[0].Visible = false;
            const string attachment = "attachment; filename=";
            response.ClearContent();
            response.AddHeader("content-disposition", attachment + filename);
            response.ContentType = "application/ms-excel";
            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            response.Write(sw.ToString());
            response.End();
            grid.AllowPaging = true;
            grid.Columns[0].Visible = true;
        
        }
 
    }
}