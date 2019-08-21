using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace BayshoreSolutions.WebModules.Cms.Controls
{
    public partial class PathNameEdit : System.Web.UI.UserControl
    {
        public string PathName
        {
            get { return PathNameText.Text; }
            set { PathNameText.Text = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            IncludeClientScript();
        }

        private void IncludeClientScript()
        {
            string editPathNameScript_key = "editPathName";
            string editPathNameScript = @"
function editPathName(edit_path_name_el_id, path_name_el_id){
    var edit_path_name_el = null;
    var path_name_el = null;
    if (document.getElementById){
        path_name_el = document.getElementById(path_name_el_id);
        edit_path_name_el = document.getElementById(edit_path_name_el_id);
        if (path_name_el && edit_path_name_el){
            if (confirm('The path name determines the page URL; changing the \npath name will break any links that point to the old URL.\n\nThe path name is the same for ALL cultures--\nit is NOT culture-dependent.\n\nAre you sure you want to edit the path name?')) {
                //allow user to edit the path name text box.
                path_name_el.disabled = null;
                //hide the edit button.
                edit_path_name_el.style.display = 'none';
            } 
        }
    }
}
";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(typeof(PathNameEdit), editPathNameScript_key))
                Page.ClientScript.RegisterClientScriptBlock(typeof(PathNameEdit), editPathNameScript_key, editPathNameScript, true);
        }
    }
}