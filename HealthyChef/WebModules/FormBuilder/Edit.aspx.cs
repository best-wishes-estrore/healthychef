using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using BayshoreSolutions.WebModules.FormBuilder.Controls;
using System.Linq;
using System.Data.Linq;

using Bss = BayshoreSolutions.Common;
using cms = BayshoreSolutions.WebModules.Cms;

namespace BayshoreSolutions.WebModules.FormBuilder
{
    public partial class Edit : System.Web.UI.Page
    {
        /// <summary>
        /// The module instance id assigned by the CMS.
        /// </summary>
        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }

        /// <summary>
        /// The instance id (navigation id) of the page that contains the module.
        /// </summary>
        public int PageNavigationId
        {
            get { return (int)(ViewState["PageNavigationId"] ?? -1); }
            set { ViewState["PageNavigationId"] = value; }
        }

        private void InitModule()
        {
            int moduleId = 0;
            int.TryParse(Request["ModuleId"], out moduleId);
            this.ModuleId = moduleId;

            WebModuleInfo module = WebModule.GetModule(this.ModuleId);
            WebpageInfo page = null;
            if (this.ModuleId <= 0
                || null == module
                || null == (page = module.Webpage))
            {
                cms.Admin.RedirectToMainMenu();
            }
            this.PageNavigationId = page.InstanceId;

            //check user permissions.
            if (!BayshoreSolutions.WebModules.Security.NavigationRole.IsUserAuthorized(this.PageNavigationId, Page.User))
                throw new System.Security.SecurityException("The current user does not have permission to access this resource.");

            ModuleName.Text = module.Name;
            ModulTypeName.Text = module.WebModuleType.Name;
            Page.Title = module.WebModuleType.Name + " Module";
            MainMenuLink.HRef = cms.Admin.GetMainMenuUrl(this.PageNavigationId);

            EnsureModule();
        }

        /// <summary>
        /// Checks that the custom module data exists. If the custom module 
        /// object cannot be retrieved (e.g., this is the initial creation of 
        /// the module), then a new module object is created using the module 
        /// id assigned by the CMS.
        /// </summary>
        private void EnsureModule()
        {
            FormBuilder.FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Module formBuilderModule = (from m in dc.FormBuilder_Modules
                                                    where m.ModuleId == this.ModuleId
                                                    select m).FirstOrDefault();
            if (formBuilderModule == null)
            {
                formBuilderModule = new FormBuilder_Module();
                formBuilderModule.ModuleId = this.ModuleId;
                dc.FormBuilder_Modules.InsertOnSubmit(formBuilderModule);
                dc.SubmitChanges();
            }
        }

        private void LoadModule()
        {
            FormBuilder.FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Module FormBuilderModule = (from m in dc.FormBuilder_Modules
                                                    where m.ModuleId == this.ModuleId
                                                    select m).Single();

            NotifyEmailCtl.Text = FormBuilderModule.NotifyEmail;
            ConfirmationPageIdCtl.SelectedNavigationId = FormBuilderModule.ConfirmationPageId.HasValue
                ? FormBuilderModule.ConfirmationPageId.Value
                : -1;
            StyleDropDown.SelectedValue = FormBuilderModule.StyleName;
            tbSubmitButtonText.Text = FormBuilderModule.SubmitButtonText;
            FormBuilderField_edit1.ModuleId = this.ModuleId;

            chkAcknowledgementEnabled.Checked = pnlEmailAcknowledgement.Visible = FormBuilderModule.Ack_Enabled;
            txtAcknowledgementBody.Text = FormBuilderModule.Ack_Body;
            txtAcknowledgementEmailField.Text = FormBuilderModule.Ack_EmailAddressFieldLabel;
            txtAcknowledgementFromEmail.Text = FormBuilderModule.Ack_FromEmailAddress;
            txtAcknowledgementSubject.Text = FormBuilderModule.Ack_Subject;

            LoadFields();
        }

        private void LoadFields()
        {
            FormBuilder.FormBuilderDataContext dc = new FormBuilderDataContext();
            var fields = from f in dc.FormBuilder_Fields
                         where f.ModuleId == this.ModuleId
                         orderby f.SortOrder
                         select f;

            FieldsList.DataSource = fields;
            FieldsList.DataBind();
        }

        bool Reload()
        {
            return (bool)(Context.Items["FormBuilder__magic_reloadFlag"] ?? false);
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            if (Reload() || !IsPostBack)
                LoadFields();

            base.OnLoadComplete(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FormBuilderDataContext dc = new FormBuilderDataContext();
                ddlTemplates.DataSource = from t in dc.FormBuilder_Templates
                                          select t;
                ddlTemplates.DataBind();

                InitModule();
                LoadModule();
            }
        }

        /// <summary>
        /// Gets the current object and fills its with the form input values.
        /// If input is not valid, returns null.
        /// </summary>
        public void GetInput_FormBuilderModule(FormBuilderDataContext dc, out FormBuilder_Module module)
        {
            module = (from m in dc.FormBuilder_Modules
                      where m.ModuleId == this.ModuleId
                      select m).Single();

            module.NotifyEmail = NotifyEmailCtl.Text.Trim();
            module.ConfirmationPageId = ConfirmationPageIdCtl.SelectedNavigationId > 0
                ? (int?)ConfirmationPageIdCtl.SelectedNavigationId
                : null;
            module.StyleName = StyleDropDown.SelectedValue;
            module.SubmitButtonText = tbSubmitButtonText.Text.Trim();

            module.Ack_Enabled = chkAcknowledgementEnabled.Checked;
            module.Ack_Body = txtAcknowledgementBody.Text.Length > 4000 ? txtAcknowledgementBody.Text.Trim().Substring(0, 4000) : txtAcknowledgementBody.Text.Trim();
            module.Ack_EmailAddressFieldLabel = txtAcknowledgementEmailField.Text.Trim();
            module.Ack_FromEmailAddress = txtAcknowledgementFromEmail.Text.Trim();
            module.Ack_Subject = txtAcknowledgementSubject.Text.Trim();
        }

        protected void FormBuilderModule_SaveButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;


            FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Module FormBuilderModule;
            GetInput_FormBuilderModule(dc, out FormBuilderModule);
            dc.SubmitChanges();

            cms.Admin.RedirectToMainMenu(this.PageNavigationId);
        }

        protected void FormBuilder_Module_CancelButton_Click(object sender, EventArgs e)
        {
            cms.Admin.RedirectToMainMenu(this.PageNavigationId);
        }
        protected void btnViewSubmissions_Click(object sender, EventArgs e)
        {
            Response.Redirect("GetSubmissions.aspx?ModuleId=" + this.ModuleId + "&ReturnUrl=" + Server.UrlEncode(Request.Url.AbsoluteUri));

        }

        protected void FieldsList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                BayshoreSolutions.WebModules.FormBuilder.Controls.FormBuilderField_edit
                    ctl = e.Item.FindControl("FormBuilderField_edit2")
                        as BayshoreSolutions.WebModules.FormBuilder.Controls.FormBuilderField_edit;
                ctl.FieldId = (int)DataBinder.Eval(e.Item.DataItem, "Id");
                ctl.DataBind();
            }
        }

        protected void TemplateList_DataBound(object sender, EventArgs e)
        {
            ddlTemplates.Items.Insert(0, new ListItem("< Select a template >", "-1"));
        }

        protected void LoadTemplate_Click(object sender, EventArgs e)
        {
            int nTemplateId = int.Parse(ddlTemplates.SelectedValue);

            FormBuilderDataContext dc = new FormBuilderDataContext();
            var templateFields = from tf in dc.FormBuilder_Template_Fields
                                 where tf.TemplateId == nTemplateId
                                 orderby tf.SortOrder
                                 select tf;

            int highestSortOrder = (from f in dc.FormBuilder_Fields
                                    where f.ModuleId == this.ModuleId
                                    orderby f.SortOrder descending
                                    select f.SortOrder).FirstOrDefault();

            foreach (var templateField in templateFields)
            {
                FormBuilder_Field field = new FormBuilder_Field();

                field.ModuleId = this.ModuleId;
                dc.FormBuilder_Fields.InsertOnSubmit(field);

                highestSortOrder += 1;

                field.SortOrder = highestSortOrder; //add to the bottom of the form

                field.Name = templateField.Name;
                field.Type = templateField.Type;
                field.Options = templateField.Options;
                field.HelpText = templateField.HelpText;
                field.IsRequired = templateField.IsRequired;
                field.Width = templateField.Width;
            }
            dc.SubmitChanges();
            LoadFields();
        }

        protected void DeleteTemplate_Click(object sender, EventArgs e)
        {
            FormBuilderDataContext dc = new FormBuilderDataContext();

            int nTemplateId = int.Parse(ddlTemplates.SelectedValue);

            FormBuilder_Template template = (from t in dc.FormBuilder_Templates
                                             where t.Id == nTemplateId
                                             select t).FirstOrDefault();

            // Cascade delete takes care of field records
            if (template != null)
            {
                dc.FormBuilder_Templates.DeleteOnSubmit(template);
                dc.SubmitChanges();
            }

            ddlTemplates.DataSource = from t in dc.FormBuilder_Templates
                                      select t;
            ddlTemplates.DataBind();
        }

        protected void SaveTemplate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbTemplateName.Text.Trim()))
            {
                // Make the sure entered a template name
                ScriptManager.RegisterStartupScript(this, this.GetType(), "TemplateNameMessage",
                        "alert('Please enter a template name.');", true);
                return;
            }

            FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Template template = (from t in dc.FormBuilder_Templates
                                             where t.Name == tbTemplateName.Text.Trim()
                                             select t).FirstOrDefault();

            if (template != null)
            {
                dc.FormBuilder_Template_Fields.DeleteAllOnSubmit(template.FormBuilder_Template_Fields);
            }
            else
            {
                template = new FormBuilder_Template();
                template.Name = tbTemplateName.Text.Trim();
                dc.FormBuilder_Templates.InsertOnSubmit(template);
            }
            dc.SubmitChanges();
            int templateId = template.Id;

            int sortOrder = 1;
            foreach (RepeaterItem repItem in FieldsList.Items)
            {
                FormBuilderField_edit editCtl = repItem.FindControl("FormBuilderField_edit2") as FormBuilderField_edit;
                editCtl.SaveAsTemplateField(templateId, sortOrder++);
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "TemplateSaveMessage",
                                      "alert('Your template has been saved.');", true);

            ddlTemplates.DataSource = from t in dc.FormBuilder_Templates
                                      select t;
            ddlTemplates.DataBind();	// update template list
        }

        protected void chkAcknowledgementEnabled_CheckedChanged(object sender, EventArgs e)
        {
            pnlEmailAcknowledgement.Visible = chkAcknowledgementEnabled.Checked;
            cvAckowledgementBody.Enabled = chkAcknowledgementEnabled.Checked;
        }

        protected void cvAcknowledgementBody_ServerValidate(object sender, ServerValidateEventArgs args)
        {
            bool bValid = true;
            if (chkAcknowledgementEnabled.Checked)
            {
                if (!string.IsNullOrEmpty(txtAcknowledgementBody.Text))
                {
                    if (txtAcknowledgementBody.Text.Length > 4000)
                    {
                        bValid = false;
                    }
                }
            }

            args.IsValid = bValid;
        }
    }

}
