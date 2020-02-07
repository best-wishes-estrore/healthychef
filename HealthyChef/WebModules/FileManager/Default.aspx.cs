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
using System.Web.Configuration;

namespace BayshoreSolutions.WebModules.Cms.FileManager
{
    public partial class _default : System.Web.UI.Page
    {
        private void InitMaxRequestLengthCtl()
        {
            int maxRequestLength = 4096; //bytes

            HttpRuntimeSection httpRuntimeSection =
                ConfigurationManager.GetSection("system.web/httpRuntime") as HttpRuntimeSection;

            if (httpRuntimeSection != null && httpRuntimeSection.MaxRequestLength >= 0)
                maxRequestLength = httpRuntimeSection.MaxRequestLength;

            //truncate
            MaxRequestLengthCtl.Text = (maxRequestLength / 1024).ToString();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string ckFinderBasePath = Page.ResolveUrl("~/WebModules/Components/TextEditor/fckeditor_ckfinder-1.4.3/");
            Page.ClientScript.RegisterClientScriptInclude("ckFinderJS", ckFinderBasePath + "ckfinder.js");

            string ckFinderScriptKey = "initCKFinder";
            if (!Page.ClientScript.IsStartupScriptRegistered(ckFinderScriptKey))
            {
                string ckFinderScript =
@"
	//new CKFinder(basePath, width, height, selectFunction) ;
	var ckfinder = new CKFinder('" + ckFinderBasePath + @"', '100%', 600, null) ;
    var editorDiv = document.getElementById('" + editorDiv.ClientID + @"');
	editorDiv.innerHTML = ckfinder.CreateHtml() ;
";
                Page.ClientScript.RegisterStartupScript(typeof(_default), ckFinderScriptKey, ckFinderScript, true);
            }

            if (!Page.IsPostBack)
            {
                InitMaxRequestLengthCtl();
            }
        }
    }
}
