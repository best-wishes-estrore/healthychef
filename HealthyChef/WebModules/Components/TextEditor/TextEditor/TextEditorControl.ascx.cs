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
using System.ComponentModel;

namespace BayshoreSolutions.Components.TextEditor
{
    [DefaultProperty("Text"), ControlValueProperty("Text"), ValidationProperty("Text")]
    public partial class TextEditorControl : System.Web.UI.UserControl
    {
        public string Text
        {
            get { return fckEditor.Value; }
            set { fckEditor.Value = value; }
        }
        public string ToolbarSet
        {
            get { return fckEditor.ToolbarSet; }
            set { fckEditor.ToolbarSet = value; }
        }
        public Unit Width
        {
            get { return fckEditor.Width; }
            set { fckEditor.Width = value; }
        }
        public Unit Height
        {
            get { return fckEditor.Height; }
            set { fckEditor.Height = value; }
        }
        public bool IsRequired
        {
            get { return fckRequiredFieldValidator.Visible; }
            set { fckRequiredFieldValidator.Visible = value; }
        }
        public string ValidationGroup
        {
            get { return fckRequiredFieldValidator.ValidationGroup; }
            set { fckRequiredFieldValidator.ValidationGroup = value; }
        }
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// If true, try to warn the user if s/he tries to navigate away from 
        /// the page without saving changes.
        /// [IE quirk: message is displayed twice if user enters the 
        /// URL in the browser address bar.]
        /// </summary>
        public bool BeforeUnloadWarning
        {
            get { return (bool)(ViewState["BeforeUnloadWarning"] ?? false); }
            set { ViewState["BeforeUnloadWarning"] = value; }
        }

        //prevents user from accidentally leaving the page if they have unsaved work.
        public static void IncludeBeforeUnloadWarningScript(Page page,
            FredCK.FCKeditorV2.FCKeditor fckEditor)
        {
            string genericKey = "OnBeforeUnloadWarningScript";
            //this script is _not_ instance-specific; it need only be registered once per page.
            string genericScript = @"
//
//prevent loss of unsaved work.
//

var _wm_textEditor_enableBeforeUnloadWarning = true; 
var _wm_textEditor_old_onbeforeunload = window.onbeforeunload;
var _wm_textEditor_fckEditorIdList = new Array(); //used to track all editor IDs.

//used to disable the warning in certain cases, such as a Save button.
function wm_textEditor_disableBeforeUnloadWarning(){
    _wm_textEditor_enableBeforeUnloadWarning = false;
}

window.onbeforeunload = function (oEvent) {
    var fckEditor = null; //editor object
    var fckEditorId = null; //string

    //IE
    if(!oEvent) oEvent = window.event;

    if(!oEvent) return;

    if(_wm_textEditor_enableBeforeUnloadWarning){
        //iterate through all of the editor IDs.
        for(i in _wm_textEditor_fckEditorIdList){
            fckEditorId = _wm_textEditor_fckEditorIdList[i];
            fckEditor = FCKeditorAPI.GetInstance(fckEditorId);

            if (fckEditor && fckEditor.IsDirty()) {
                oEvent.returnValue = ""You have not saved your work. All changes will be lost."";
                break; //there is no need to continue the loop now.
            }
        }
    }

    if(_wm_textEditor_old_onbeforeunload)
        _wm_textEditor_old_onbeforeunload(oEvent);
};
";
            if (!page.ClientScript.IsStartupScriptRegistered(genericKey))
                page.ClientScript.RegisterStartupScript(typeof(TextEditorControl), genericKey, genericScript, true);

            //unique key for each instance of the editor.
            string instanceKey = genericKey + "_" + fckEditor.ClientID;
            //this script _is_ instance-specific; it needs to be registered 
            //for each instance of the control on the page. the control IDs
            //are tracked in a javascript array.
            string instanceScript = @"_wm_textEditor_fckEditorIdList.push(""" + fckEditor.ClientID + @""");
";
            if (!page.ClientScript.IsStartupScriptRegistered(instanceKey))
                page.ClientScript.RegisterStartupScript(typeof(TextEditorControl), instanceKey, instanceScript, true);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.BeforeUnloadWarning)
                IncludeBeforeUnloadWarningScript(this.Page, fckEditor);

            //set up CKFinder.
            CKFinder.FileBrowser fileBrowser = new CKFinder.FileBrowser();
            fileBrowser.BasePath = Page.ResolveUrl("~/WebModules/Components/TextEditor/fckeditor_ckfinder-1.4.3/");
            fileBrowser.SetupFCKeditor(fckEditor);

            //REMOVED: no longer necessary as of ckfinder 1.2.3.
            //HACK: fix the ImageBrowserURL. CKFinder.FileBrowser.SetupFCKeditor() is hard-coded to set FCKeditor.ImageBrowserURL to the "Images" resource type.
            //fckEditor.ImageBrowserURL = fckEditor.Config["ImageBrowserURL"].Replace("type=Images", "type=Image");

            //HACK: fix the 'quick upload' URL so that it writes to 
            //      /userfiles/files/ instead of /userfiles/.
            //NOTE: do this _after_ CKFinder.FileBrowser.SetupFCKeditor().
            fckEditor.Config["LinkUploadURL"] = fckEditor.Config["LinkUploadURL"] + "&type=Files";

            if (this.IsRequired)
            {
                //FCKeditor does not work with asp:RequiredFieldValidator, so we must do a custom validator.
                //see: http://wiki.fckeditor.net/Troubleshooting#head-9b3ef5962fb1f578c84005f3bff3ff725d3f84c4

                string fxnName = "fckEditor_requiredFieldValidator_callback";
                if (!Page.ClientScript.IsClientScriptBlockRegistered(fxnName))
                {
                    string requiredFieldValidator_script = @"
function " + fxnName + @"(sender, args){
    var validator = document.getElementById(sender.id);
    var fckEditor = FCKeditorAPI.GetInstance(validator.controltovalidate);
    //valid if >0 non-whitespace characters.
    args.IsValid = (fckEditor.GetHTML().replace(/^\s+|\s+$/, '').length > 0);
}
";
                    Page.ClientScript.RegisterClientScriptBlock(typeof(TextEditorControl), fxnName, requiredFieldValidator_script, true);
                }

                fckRequiredFieldValidator.ClientValidationFunction = fxnName;
            }
        }
        protected void fckRequiredFieldValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            args.IsValid = (fckEditor.Value.Trim().Length > 0);
        }
    }
}
