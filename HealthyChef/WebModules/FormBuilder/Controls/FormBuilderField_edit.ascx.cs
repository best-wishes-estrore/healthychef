using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data.Linq;

using Bss = BayshoreSolutions.Common;

namespace BayshoreSolutions.WebModules.FormBuilder.Controls
{
    public partial class FormBuilderField_edit : System.Web.UI.UserControl
    {
        void SetReloadFlag(bool reload)
        {
            //resort to magic to avoid massive headaches.
            Context.Items["FormBuilder__magic_reloadFlag"] = reload;
        }

        FormBuilder_Field _field;
        public FormBuilder_Field Field
        {
            get
            {
                if (null == _field && this.FieldId > 0)
                {
                    _field = null;
                    try
                    {
                        BayshoreSolutions.WebModules.FormBuilder.FormBuilderDataContext dc = new FormBuilderDataContext();
                        _field = (from f in dc.FormBuilder_Fields
                                  where f.Id == this.FieldId
                                  select f).Single();
                    }
                    catch
                    {
                    }

                }
                return _field;
            }
        }

        public int FieldId
        {
            get { return (int)(ViewState["FieldId"] ?? -1); }
            set { ViewState["FieldId"] = value; }
        }

        public int ModuleId
        {
            get { return (int)(ViewState["ModuleId"] ?? -1); }
            set { ViewState["ModuleId"] = value; }
        }

        public bool IsNew
        {
            get { return this.FieldId <= 0; }
        }

        protected override void OnInit(EventArgs e)
        {
            Bss.Web.UI.UITool.FillListControl<FormBuilder_Field.FieldType>(FieldTypesList, "");

            //need a unique validation group for each instance of this control.
            FieldNameRequiredFieldValidator.ValidationGroup = this.ClientID + "_FormBuilderField";
            FieldTypesListRequiredFieldValidator.ValidationGroup = this.ClientID + "_FormBuilderField";
            FormBuilderField_SaveButton.ValidationGroup = this.ClientID + "_FormBuilderField";

            base.OnInit(e);
        }

        public override void DataBind()
        {
            if (null == this.Field) return;

            if (!this.IsNew)
            {
                MoveUpButton.Visible = true;
                MoveDownButton.Visible = true;
                FormBuilderField_DeleteButton.Visible = true;
            }

            Options_td.Visible = FormBuilder_Field.IsOptionsFieldType((FormBuilder_Field.FieldType)Field.Type);

            FieldName.Text = this.Field.Name;
            FieldTypesList.Text = this.Field.Type.ToString();
            IsFieldRequired.Checked = this.Field.IsRequired;
            FieldWidth.Text = this.Field.Width.ToString();
            FormBuilderFieldOption_edit1.FieldOptions = this.Field.GetFieldOptions();
            FormBuilderFieldOption_edit1.DataBind();

            base.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void FormBuilderField_SaveButton_Click(object sender, EventArgs e)
        {
            int typeId = int.Parse(FieldTypesList.Text);

            FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Field field = (from f in dc.FormBuilder_Fields
                                       where f.Id == this.FieldId
                                       select f).FirstOrDefault();
            if (field == null)
            {
                field = new FormBuilder_Field();

                field.ModuleId = this.ModuleId;

                var maxSortOrder = (from f in dc.FormBuilder_Fields
                                    where f.ModuleId == field.ModuleId
                                    orderby f.SortOrder descending
                                    select f.SortOrder).FirstOrDefault();
                field.SortOrder = maxSortOrder + 1;

                dc.FormBuilder_Fields.InsertOnSubmit(field);
            }
            else
            {
                field = (from f in dc.FormBuilder_Fields
                         where f.Id == this.FieldId
                         select f).Single();
            }


            field.Name = FieldName.Text.Trim();
            field.Type = typeId;
            field.SetFieldOptions(FormBuilderFieldOption_edit1.FieldOptions);
            field.IsRequired = IsFieldRequired.Checked;
            field.Width = string.IsNullOrEmpty(FieldWidth.Text) ? null : (int?)int.Parse(FieldWidth.Text);
            dc.SubmitChanges();

            if (this.IsNew)
            {
                Bss.Web.UI.UITool.Clear(this);
                FormBuilderFieldOption_edit1.FieldOptions.Clear();
                FormBuilderFieldOption_edit1.DataBind();
                Msg.ShowSuccess(string.Format("Added the '{0}' field to the form.", field.Name));
                SetReloadFlag(true);
            }
            else
            {
                if (field.SortOrder <= 1)
                {
                    FormBuilder_Field.MoveUp(field.Id);
                }
                Msg.ShowSuccess(string.Format("Saved the '{0}' field settings.", field.Name));
            }
        }

        public void SaveAsTemplateField(int templateId, int sortOrder)
        {
            FormBuilderDataContext dc = new FormBuilderDataContext();
            FormBuilder_Template_Field newRec = new FormBuilder_Template_Field();
            dc.FormBuilder_Template_Fields.InsertOnSubmit(newRec);

            newRec.TemplateId = templateId;
            newRec.Name = FieldName.Text.Trim();
            newRec.Type = int.Parse(FieldTypesList.Text); ;
            newRec.SetFieldOptions(FormBuilderFieldOption_edit1.FieldOptions);
            newRec.IsRequired = IsFieldRequired.Checked;
            newRec.SortOrder = sortOrder;
            newRec.Width = string.IsNullOrEmpty(FieldWidth.Text) ? null : (int?)int.Parse(FieldWidth.Text);
            dc.SubmitChanges();

        }

        protected void FormBuilderField_DeleteButton_Click(object sender, EventArgs e)
        {
            FormBuilderDataContext dc = new FormBuilderDataContext();

            var fieldInputsToDelete = from fi in dc.FormBuilder_FieldInputs
                                      where fi.FieldId == this.FieldId
                                      select fi;
            dc.FormBuilder_FieldInputs.DeleteAllOnSubmit(fieldInputsToDelete);

            var fieldToDelete = (from f in dc.FormBuilder_Fields
                                 where f.Id == this.FieldId
                                 select f).Single();
            dc.FormBuilder_Fields.DeleteOnSubmit(fieldToDelete);
            dc.SubmitChanges();

            SetReloadFlag(true);
        }

        public virtual void MoveUpButton_Click(object sender, ImageClickEventArgs e)
        {
            FormBuilder_Field.MoveUp(this.FieldId);
            SetReloadFlag(true);
        }

        public virtual void MoveDownButton_Click(object sender, ImageClickEventArgs e)
        {
            FormBuilder_Field.MoveDown(this.FieldId);
            SetReloadFlag(true);
        }
    }
}