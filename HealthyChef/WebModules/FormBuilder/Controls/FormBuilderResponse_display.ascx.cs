using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Reflection;
using System.IO;
using System.Configuration;
using System.Data.Linq;
using BayshoreSolutions.WebModules.Cms.Localization.Controls;
using BayshoreSolutions.WebModules.FormBuilder;

/*
    Add the following to web.config to disable form spam protection validation:
     
    <add key="FormValidation_DontUseSessionVariable" value="true" />
    <add key="FormValidation_DontUseDummyFields" value="true" />
    <add key="FormValidation_DontValidateReferrer" value="true" />
*/
namespace BayshoreSolutions.WebModules.FormBuilder.Controls
{
    public partial class FormBuilderResponse_display : BayshoreSolutions.WebModules.WebModuleBase
    {
        //track each field and its respective control, so we can retrieve the postback values.
        Dictionary<FormBuilder_Field, WebControl> _fields_controls = new Dictionary<FormBuilder_Field, WebControl>();
        public int ResponseId
        {
            get { return ((int)(Session["ResponseId"] ?? 0)); }
            set { Session["ResponseId"] = value; }
        }

        //create controls *before* viewstate is restored.
        //ASP.NET restores the viewstate tree based on the control tree. As long as
        //we re-create the control tree identically on each postback, the viewstate
        //will get restored automatically.
        //see http://geekswithblogs.net/FrostRed/archive/2007/02/17/106547.aspx
        protected override void OnInit(EventArgs e)
        {
            FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Module settings = (from m in dc.FormBuilder_Modules
                                           where m.ModuleId == this.ModuleId
                                           select m).Single();


            List<FormBuilder_Field> lFields = settings.FormBuilder_Fields.OrderBy(x => x.SortOrder).ToList();
            IList<Panel> pnlCollection = GetPanelControls(lFields);
            if (pnlCollection != null && pnlCollection.Count > 0)
            {
                #region Form Page Creation
                int pnlCount = pnlCollection.Count;
                if (pnlCount >= 1)
                {
                    for (int i = 0; i < pnlCount; i++)
                    {
                        string sortOrder = String.Empty;
                        string clientId = String.Empty;
                        try
                        {
                            if (pnlCollection[i + 1] != null)
                            {
                                sortOrder = pnlCollection[i + 1].Attributes["SortOrder"];
                                clientId = pnlCollection[i + 1].ID;
                            }
                        }
                        catch (ArgumentOutOfRangeException)
                        {

                            sortOrder = (lFields[lFields.Count - 1].SortOrder + 1).ToString();
                        }

                        FormBuilderResponseValidationSummary.ValidationGroup = FormBuilder_Module.GetValidationGroup(this.ModuleId);

                        List<FormBuilder_Field> childCollection = GetChildCollection(lFields, pnlCollection[i].Attributes["SortOrder"], sortOrder);
                        foreach (FormBuilder_Field f in childCollection)
                        {
                            FormBuilderFieldInput_display ctl = (FormBuilderFieldInput_display)this.LoadControl("FormBuilderFieldInput_display.ascx");
                            if (ctl != null) ctl.StyleName = settings.StyleName;
                            FieldInputList.Controls.Add(pnlCollection[i]);
                            pnlCollection[i].Controls.Add(ctl);

                            _fields_controls.Add(f, ctl.LoadField(f));

                        }

                        Button btnNext = BuildButton("Next", i, clientId);
                        Button btnPrev = BuildButton("Previous", i, (i - 1) > 0 ? pnlCollection[i - 1].ID : pnlCollection[0].ID);

                        if (i == 0)
                        {
                            pnlCollection[i].Visible = true;
                            if (pnlCollection.Count > 1) pnlCollection[i].Controls.Add(btnNext);
                        }
                        else if (i == pnlCount - 1)
                        {
                            pnlCollection[i].Controls.Add(btnPrev);
                            pnlCollection[i].Controls.Add(BuildButton(String.IsNullOrEmpty(settings.SubmitButtonText) ? "Submit" : settings.SubmitButtonText, i, pnlCollection[i].ID));
                        }
                        else // All other panels
                        {
                            pnlCollection[i].Controls.Add(btnPrev);
                            pnlCollection[i].Controls.Add(btnNext);
                        }
                    }

                }
                #endregion
            }
            else
            {
                #region Form Creation
                foreach (FormBuilder_Field f in lFields)
                {
                    FormBuilderFieldInput_display fieldCtl = (FormBuilderFieldInput_display)this.LoadControl("FormBuilderFieldInput_display.ascx");
                    if (fieldCtl != null) fieldCtl.StyleName = settings.StyleName;
                    FieldInputList.Controls.Add(fieldCtl);

                    _fields_controls.Add(f, fieldCtl.LoadField(f));
                }

                FormBuilderResponseValidationSummary.ValidationGroup =
                    FormBuilderResponse_SaveButton.ValidationGroup =
                        FormBuilder_Module.GetValidationGroup(this.ModuleId);
                buttonDiv.Visible = true;

                FormBuilderResponse_SaveButton.Visible = (null != lFields && lFields.Count > 0);
                if ((settings != null) && (!string.IsNullOrEmpty(settings.SubmitButtonText)))
                {
                    FormBuilderResponse_SaveButton.Text = settings.SubmitButtonText;
                }

                string buttonDivClass;
                if (!string.IsNullOrEmpty(settings.StyleName) && settings.StyleName.ToLower() != "block")
                {
                    buttonDivClass = "field-" + settings.StyleName;
                }
                else
                {
                    buttonDivClass = "field";
                }
                buttonDiv.Attributes.Add("class", buttonDivClass);
                #endregion
            }

            base.OnInit(e);
        }

        private void LoadInitialData()
        {
            if (ResponseId > 0)
            {
                FormBuilderDataContext dc = new FormBuilderDataContext();
                var response = (from r in dc.FormBuilder_Responses
                                where r.ModuleId == this.ModuleId
                                && r.Id == ResponseId
                                select r).FirstOrDefault();

                if (response != null)
                {
                    // load the data
                    WebControl ctl = null;

                    foreach (var fi in response.FormBuilder_FieldInputs)
                    {
                        if (TryGetDictionaryValue(fi.FormBuilder_Field, out ctl))
                        {
                            if (fi.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.ShortText
                                || fi.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.LongText)
                                ((TextBox)ctl).Text = fi.InputValue;
                            else if (fi.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.CheckBox)
                                ((CheckBoxList)ctl).SelectedIndex =
                                    ((CheckBoxList)ctl).Items.IndexOf(
                                        ((CheckBoxList)ctl).Items.FindByValue(
                                            fi.InputValue));
                            else if (fi.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.DropDownList)
                                ((DropDownList)ctl).SelectedIndex =
                                    ((DropDownList)ctl).Items.IndexOf(
                                        ((DropDownList)ctl).Items.FindByValue(
                                            fi.InputValue));
                            else if (fi.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.RadioButton)
                                ((RadioButtonList)ctl).SelectedIndex =
                                    ((RadioButtonList)ctl).Items.IndexOf(
                                        ((RadioButtonList)ctl).Items.FindByValue(
                                            fi.InputValue));
                            else if (fi.FormBuilder_Field.Type == (int)FormBuilder_Field.FieldType.FileUpload)
                                ((FileUpload)ctl).Attributes["value"] = fi.InputValue;
                        }
                    }
                }
            }
        }

        private bool TryGetDictionaryValue(FormBuilder_Field formBuilderField, out WebControl ctl)
        {
            ctl = null;

            foreach (KeyValuePair<FormBuilder_Field, WebControl> keyValuePair in _fields_controls)
            {
                if (keyValuePair.Key.GetInputControlId().EndsWith(formBuilderField.Id.ToString()))
                {
                    ctl = keyValuePair.Value;
                    break;
                }
            }

            return ctl != null;
        }

        private Button BuildButton(string buttonText, int position, string clientId)
        {
            Button btn = new Button();
            btn.ID = String.Format("btn{0}_{1}", buttonText.Substring(0, buttonText.Length > 4 ? 4 : buttonText.Length), position.ToString());
            btn.Text = btn.CommandName = buttonText;
            btn.CommandArgument = clientId;
            btn.CausesValidation = true;
            if (buttonText.ToLower() == "previous") btn.CausesValidation = false;
            else btn.ValidationGroup = FormBuilder_Module.GetValidationGroup(this.ModuleId);
            btn.Click += new EventHandler(LoadPage);

            return btn;
        }

        private List<FormBuilder_Field> GetChildCollection(List<FormBuilder_Field> fields, string sortOrderStart, string sortOrderEnd)
        {
            var list = fields.Where(x => x.Type != (int)FormBuilder_Field.FieldType.PageHeader
                            && x.SortOrder > int.Parse(sortOrderStart)
                            && x.SortOrder <= int.Parse(sortOrderEnd))
                .ToList();

            return list;
        }

        private IList<Panel> GetPanelControls(List<FormBuilder_Field> fields)
        {
            IList<Panel> pnlCollection = new List<Panel>();
            foreach (FormBuilder_Field f in fields)
                if ((FormBuilder_Field.FieldType)f.Type == FormBuilder_Field.FieldType.PageHeader)
                {
                    var pnl = new Panel() { ID = "pnl_" + f.Name.Replace(" ", "_") + "_" + f.Id.ToString() };
                    pnl.Attributes.Add("SortOrder", f.SortOrder.ToString());
                    pnl.Attributes.Add("ControlId", f.Id.ToString());
                    pnl.Attributes.Add("PageName", f.Name);
                    pnl.Visible = false;

                    pnlCollection.Add(pnl);
                }
            return pnlCollection;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareValidation();
            AddStyleSheetToHeader();
            FormPanel.DefaultButton = FormBuilderResponse_SaveButton.ID;
            // Set the title using the form page name if this is a multipage form
            formtitle.Text = GetFormTitle(FormPanel.Controls);
            if (!IsPostBack) LoadInitialData();
        }

        private string GetFormTitle(ControlCollection collection)
        {
            var defaultTitle = String.Empty;
            if (collection == null) collection = FormPanel.Controls;

            foreach (Control ctl in collection)
            {
                if (ctl.GetType() == typeof(Panel) && ctl.Visible)
                {
                    defaultTitle = ((Panel)ctl).Attributes.Count > 0 ?
                        ((Panel)ctl).Attributes["PageName"] : Page.Title;
                    break;
                }
                if (ctl.HasControls())
                {
                    defaultTitle = GetFormTitle(ctl.Controls);
                    if (!String.IsNullOrEmpty(defaultTitle)) break;
                }
            }

            return defaultTitle;
        }
        private void AddStyleSheetToHeader()
        {
            HtmlLink cssLink = new HtmlLink();
            cssLink.Href = "~/WebModules/FormBuilder/public/css/FormBuilder.css";
            cssLink.Attributes["rel"] = "stylesheet";
            cssLink.Attributes["type"] = "text/css";
            this.Page.Header.Controls.AddAt(1, cssLink);
        }
        protected void FormBuilderResponse_SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (ValidateRequest())
                {
                    FormBuilderDataContext dc = new FormBuilderDataContext();
                    FormBuilder_Module form = (from f in dc.FormBuilder_Modules
                                               where f.ModuleId == this.ModuleId
                                               select f).Single();


                    //save the form object.
                    FormBuilder_Response formResponse = (from r in dc.FormBuilder_Responses
                                                         where r.ModuleId == this.ModuleId
                                                               && r.Id == ResponseId
                                                         select r).SingleOrDefault();

                    if (formResponse == null)
                    {
                        formResponse = new FormBuilder_Response();
                        dc.FormBuilder_Responses.InsertOnSubmit(formResponse);
                        formResponse.ModuleId = this.ModuleId;
                        formResponse.CreatedBy = this.Page.User.Identity.Name;
                        formResponse.CreatedOn = DateTime.Now;
                        formResponse.IPAddress = this.Page.Request.UserHostAddress;
                        dc.SubmitChanges();
                    }

                    // save the referrer info (if available)
                    if (Session["session_referrer"] != null)
                    {
                        string referringUrl = Session["session_referringUrl"] as string;
                        string landingUrl = Session["session_landingUrl"] as string;
                        string domain = Session["session_referrer"] as string;
                        string query = Session["session_keywords"] as string;

                        FormBuilder_ResponseReferrer referrer = new FormBuilder_ResponseReferrer();
                        dc.FormBuilder_ResponseReferrers.InsertOnSubmit(referrer);

                        referrer.ResponseId = formResponse.Id;
                        referrer.referringUrl = referringUrl;
                        referrer.landingUrl = landingUrl;
                        referrer.domain = domain;
                        referrer.query = query;
                        dc.SubmitChanges();
                    }

                    //save input for each form field.
                    foreach (FormBuilder_Field f in _fields_controls.Keys)
                    {
                        // Find any existing field input and update it.
                        FormBuilder_FieldInput fieldInput = (from fi in dc.FormBuilder_FieldInputs
                                                             where fi.ResponseId == formResponse.Id
                                                                   && fi.FieldId == f.Id
                                                             select fi).SingleOrDefault();
                        if (fieldInput == null)
                        {
                            fieldInput = new FormBuilder_FieldInput();
                            dc.FormBuilder_FieldInputs.InsertOnSubmit(fieldInput);
                            fieldInput.FieldId = f.Id;
                            fieldInput.ResponseId = formResponse.Id;
                        }

                        if (f.Type == (int)FormBuilder_Field.FieldType.FileUpload)
                        {
                            WebControl fi;
                            _fields_controls.TryGetValue(f, out fi);

                            if (((FileUpload)fi).HasFile)
                            {
                                var fiUploadFile = new FileInfo(((FileUpload)fi).FileName);
                                var sFileName = fiUploadFile.Name.Substring(0, fiUploadFile.Name.LastIndexOf("."))
                                                + "_" + Guid.NewGuid() + fiUploadFile.Extension;
                                string strUploadDirectory = this.MapPath(ConfigurationManager.AppSettings["FormBuilder_UploadDirectory"]);
                                EnsureDirectory(strUploadDirectory);
                                var uploadPath = Path.Combine(strUploadDirectory, sFileName);
                                ((FileUpload)fi).SaveAs(uploadPath);
                                fieldInput.InputValue = ConfigurationManager.AppSettings["FormBuilder_UploadDirectory"] + "\\" + sFileName;
                            }
                        }
                        else fieldInput.SetInputValueFromControlValue(_fields_controls[f]);
                    }

                    formResponse.IsComplete = true;
                    dc.SubmitChanges();

                    //send email notification.
                    formResponse.EmailNotifyAdmin();
                    formResponse.EmailAcknowledgement();
                    ResponseId = 0;

                    //redirect to confirmation page.
                    string redirectPath = "~";
                    if (form.ConfirmationPageId.HasValue)
                    {
                        WebpageInfo page = Webpage.GetWebpage(form.ConfirmationPageId.Value);
                        if (null != page && !string.IsNullOrEmpty(page.Path))
                            redirectPath = page.Path;
                    }
                    Response.Redirect(redirectPath);
                }
            }
        }

        protected void PrepareValidation()
        {
            bool bUseSessionVariable;
            bool bUseDummyFields;
            bool bValidateReferrer;
            GetValidationSettings(out bUseSessionVariable, out bUseDummyFields, out bValidateReferrer);

            if (bUseSessionVariable)
            {
                Session.Add("AntiSpamvar", "0");
            }

            if (bUseDummyFields)
            {
                string strDummyStyle = @"
<style type=""text/css""> 
.sbshtst38126 { display:none; }
</style>
";
                literalStyle.Text = strDummyStyle;

                string strDummyFields = @"
        <div class=""sbshtst38126"">
            <label for=""url"">*url:</label>
            <input name=""url"" type=""text"" value="""" />
        </div>
        <div class=""sbshtst38126"">
            <label for=""email"">*email:</label>
            <input name=""email"" type=""text"" value="""" />
        </div>
    ";
                literalFields.Text = strDummyFields;
            }
        }

        protected void GetValidationSettings(out bool bUseSessionVariable, out bool bUseDummyFields, out bool bValidateReferrer)
        {
            bUseSessionVariable = true;
            bUseDummyFields = true;
            bValidateReferrer = true;

            string strDontUseSessionVariable = System.Configuration.ConfigurationManager.AppSettings["FormValidation_DontUseSessionVariable"];
            if (!string.IsNullOrEmpty(strDontUseSessionVariable))
            {
                if ((string.Compare(strDontUseSessionVariable, "true", true) == 0) || (string.Compare(strDontUseSessionVariable, "1", true) == 0))
                {
                    bUseSessionVariable = false;
                }
            }

            string strDontUseDummyFields = System.Configuration.ConfigurationManager.AppSettings["FormValidation_DontUseDummyFields"];
            if (!string.IsNullOrEmpty(strDontUseDummyFields))
            {
                if ((string.Compare(strDontUseDummyFields, "true", true) == 0) || (string.Compare(strDontUseDummyFields, "1", true) == 0))
                {
                    bUseDummyFields = false;
                }
            }

            string strDontValidateReferrer = System.Configuration.ConfigurationManager.AppSettings["FormValidation_DontValidateReferrer"];
            if (!string.IsNullOrEmpty(strDontValidateReferrer))
            {
                if ((string.Compare(strDontValidateReferrer, "true", true) == 0) || (string.Compare(strDontValidateReferrer, "1", true) == 0))
                {
                    bValidateReferrer = false;
                }
            }
        }

        protected bool ValidateRequest()
        {
            bool bValid = false;


            bool bFailedAnyTest = false;

            bool bUseSessionVariable;
            bool bUseDummyFields;
            bool bValidateReferrer;
            GetValidationSettings(out bUseSessionVariable, out bUseDummyFields, out bValidateReferrer);

            if (!bFailedAnyTest)
            {
                if (bUseDummyFields)
                {
                    // look for dummy fields
                    string strEmail = Request.Form["email"];
                    string strUrl = Request.Form["url"];
                    if (!string.IsNullOrEmpty(strEmail) || !string.IsNullOrEmpty(strUrl))
                    {
                        WebModules.WebModulesAuditEvent.Raise("Form validation failed:  values found in bot protection field(s).  To bypass this test, add appSetting FormValidation_DontUseDummyFields = true to web.config.", this);
                        bFailedAnyTest = true;
                    }
                }
            }

            if (!bFailedAnyTest)
            {
                if (bUseSessionVariable)
                {
                    // look for anti spam variable
                    object objAntiSpamVar = Session["AntiSpamVar"];
                    if (objAntiSpamVar == null)
                    {
                        WebModules.WebModulesAuditEvent.Raise("Form validation failed:  anti spam var missing.  To bypass this test, add appSetting FormValidation_DontUseSessionVariable = true to web.config.", this);
                        bFailedAnyTest = true;
                    }
                }
            }

            if (!bFailedAnyTest)
            {
                if (bValidateReferrer)
                {
                    bool bReferrerValid = false;

                    string strRequestAuthority = Request.Url.Authority;
                    string strReferralAuthority = null;
                    string strThisPath = null;
                    string strReferringPath = null;

                    if (Request.UrlReferrer != null)
                    {
                        strReferralAuthority = Request.UrlReferrer.Authority;
                        if (string.Compare(strRequestAuthority, strReferralAuthority, true) == 0)
                        {
                            // check referring page name matches
                            strThisPath = ResolveUrl(this.WebModuleInfo.Webpage.Path);
                            strThisPath = strThisPath.TrimStart(new char[] { '~', '/', '\\' });
                            if (string.Compare(strThisPath, "default.aspx", true) == 0)
                            {
                                strThisPath = string.Empty;
                            }

                            if (Request.UrlReferrer != null)
                            {
                                strReferringPath = Request.UrlReferrer.AbsolutePath;
                                strReferringPath = strReferringPath.TrimStart(new char[] { '~', '/', '\\' });
                                if (string.Compare(strReferringPath, "default.aspx", true) == 0)
                                {
                                    strReferringPath = string.Empty;
                                }


                                if (string.Compare(strReferringPath, strThisPath, true) == 0)
                                {
                                    bReferrerValid = true;
                                }
                            }
                        }
                    }

                    if (!bReferrerValid)
                    {
                        string strMessage = string.Format("Form validation failed:  referrer '{0}{1}' does not match this path '{2}{3}'.  To bypass this test, add appSetting FormValidation_DontValidateReferrer = true to web.config.",
                            strReferralAuthority,
                            strReferringPath,
                            strRequestAuthority,
                            strThisPath);

                        WebModules.WebModulesAuditEvent.Raise(strMessage, this);
                        bFailedAnyTest = true;
                    }
                }
            }

            if (!bFailedAnyTest)
            {
                bValid = true;
            }

            return bValid;
        }


        protected void EnsureDirectory(string strUploadDirectory)
        {
            try
            {
                strUploadDirectory = strUploadDirectory.TrimEnd(new char[] { '\\' });
                DirectoryInfo di = new DirectoryInfo(strUploadDirectory);
                if (!di.Exists)
                {
                    int nLastSlash = strUploadDirectory.LastIndexOf('\\');
                    if (nLastSlash > 0)
                    {
                        string strParentDirectory = strUploadDirectory.Substring(0, nLastSlash);
                        EnsureDirectory(strParentDirectory);
                    }
                    di.Create();
                }
            }
            catch (Exception ex)
            {
                string strMessage = string.Format("Failed creating directory '{0}'.", strUploadDirectory);
                BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(strMessage, this, ex);
            }
        }

        public void LoadPage(object sender, EventArgs e)
        {
            Button btn = ((Button)sender);
            var currentPanel = btn.Parent as Panel;
            switch (btn.CommandName.ToLower())
            {
                case "next":
                case "previous":
                    var nextPanel = FieldInputList.FindControl(btn.CommandArgument) as Panel;
                    if (currentPanel != null) currentPanel.Visible = false;
                    if (nextPanel != null) nextPanel.Visible = true;
                    formtitle.Text = GetFormTitle(FormPanel.Controls);
                    SaveIncrementalFormData();
                    break;
                default:
                    FormBuilderResponse_SaveButton_Click(sender, null);
                    break;
            }
        }

        private void SaveIncrementalFormData()
        {
            FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Module form = (from m in dc.FormBuilder_Modules
                                       where m.ModuleId == this.ModuleId
                                       select m).Single();

            //save the form object.
            FormBuilder_Response formResponse = (from r in dc.FormBuilder_Responses
                                                 where r.Id == ResponseId
                                                 select r).FirstOrDefault();

            if (formResponse == null)
            {
                formResponse = new FormBuilder_Response();
                dc.FormBuilder_Responses.InsertOnSubmit(formResponse);
                formResponse.ModuleId = this.ModuleId;
                formResponse.CreatedBy = this.Page.User.Identity.Name;
                formResponse.CreatedOn = DateTime.Now;
                formResponse.IPAddress = this.Page.Request.UserHostAddress;

                dc.SubmitChanges();
                ResponseId = formResponse.Id;
            }

            //save input for each form field.
            foreach (FormBuilder_Field f in _fields_controls.Keys)
            {
                FormBuilder_FieldInput fieldInput = (from fi in dc.FormBuilder_FieldInputs
                                                     where fi.ResponseId == ResponseId
                                                     where fi.FieldId == f.Id
                                                     select fi).FirstOrDefault();
                if (fieldInput == null)
                {
                    fieldInput = new FormBuilder_FieldInput();
                    dc.FormBuilder_FieldInputs.InsertOnSubmit(fieldInput);
                }

                fieldInput.ResponseId = formResponse.Id;
                fieldInput.FieldId = f.Id;
                if (f.Type == (int)FormBuilder_Field.FieldType.FileUpload)
                {
                    WebControl fi;
                    _fields_controls.TryGetValue(f, out fi);

                    if (((FileUpload)fi).HasFile)
                    {
                        var fiUploadFile = new FileInfo(((FileUpload)fi).FileName);
                        var sFileName = fiUploadFile.Name.Substring(0, fiUploadFile.Name.LastIndexOf("."))
                                        + "_" + Guid.NewGuid() + fiUploadFile.Extension;
                        var uploadPath = this.MapPath(ConfigurationManager.AppSettings["FormBuilder_UploadDirectory"]) +
                                         "\\" + sFileName;
                        ((FileUpload)fi).SaveAs(uploadPath);
                        fieldInput.InputValue = ConfigurationManager.AppSettings["FormBuilder_UploadDirectory"] + "\\" + sFileName;
                    }
                }
                else
                {
                    fieldInput.SetInputValueFromControlValue(_fields_controls[f]);
                }

                dc.SubmitChanges();
            }
        }

        private Control FindFormControl(ControlCollection collection, string controlName)
        {
            Control ctl = null;
            if (String.IsNullOrEmpty(controlName)) return null;

            foreach (Control control in collection)
            {
                if (control != null && control.ID.EndsWith(controlName))
                {
                    ctl = control;
                    break;
                }
                if (control != null && control.HasControls())
                {
                    ctl = FindFormControl(control.Controls, controlName);
                    if (ctl != null && ctl.ID.EndsWith(controlName)) break;
                }
            }

            return ctl;
        }

    }
}