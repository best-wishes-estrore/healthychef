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
using System.Text;

namespace BayshoreSolutions.WebModules.Components.ImagePicker
{
    public partial class ImagePicker : System.Web.UI.UserControl
    {
        //removed. this is a misfeature; a blank is more intuitive. jkeyes 20070830.
        /*
        private string _NoImageText = "[none]";
        /// <summary>
        /// The text to display when no image is selected.
        /// </summary>
        public string NoImageText
        {
            get { return _NoImageText; }
            set
            {
                if (value == "") return;
                if (this.txtImageURL.Text == _NoImageText) txtImageURL.Text = value;
                _NoImageText = value;
            }
        }
        */
        public string ImagePath
        {
            get
            {
                //if (txtImageURL.Text == NoImageText) return "";
                return txtImageURL.Text;
            }
            set
            {
                //if ((value == null) || (value == ""))
                //    txtImageURL.Text = NoImageText;
                //else
                txtImageURL.Text = value;
            }
        }
        public bool IsRequired
        {
            get { return uxRequiredFieldValidator.Visible; }
            set { uxRequiredFieldValidator.Visible = value; }
        }
        public string ValidationGroup
        {
            get { return uxRequiredFieldValidator.ValidationGroup; }
            set { uxRequiredFieldValidator.ValidationGroup = value; }
        }
        public Unit Width
        {
            get { return txtImageURL.Width; }
            set { txtImageURL.Width = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string clearScript = txtImageURL.ClientID + ".value='';";// +NoImageText + "';";
            btnClear.Attributes.Add("onclick", clearScript);

            string ckFinderBasePath = Page.ResolveUrl("~/WebModules/Components/TextEditor/fckeditor_ckfinder-1.4.3/");
            Page.ClientScript.RegisterClientScriptInclude("ckFinderJS", ckFinderBasePath + "ckfinder.js");

            string ckFinderScriptFxn = "openCKFinder";
            if (!Page.ClientScript.IsClientScriptBlockRegistered(ckFinderScriptFxn))
            {
                string ckFinderScript = @"
var _imageUrl_el_id = null;
function " + ckFinderScriptFxn + @"(imageUrl_el_id)
{
    //set element id to be used by the callback.
    _imageUrl_el_id = imageUrl_el_id;

    //CKFinder.Popup(basePath, width, height, selectFunction);
    CKFinder.Popup('" + ckFinderBasePath + @"', 700, 600, setFileField) ;
}
function setFileField(fileUrl)
{
	document.getElementById(_imageUrl_el_id).value = fileUrl;
}
";
                Page.ClientScript.RegisterClientScriptBlock(typeof(ImagePicker), ckFinderScriptFxn, ckFinderScript, true);
            }
            btnSelect.Attributes.Add("onclick",
                string.Format("{0}('{1}');", ckFinderScriptFxn, txtImageURL.ClientID));
        }
    }
}