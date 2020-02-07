using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using BayshoreSolutions.Common.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;

namespace BayshoreSolutions.WebModules.FormBuilder
{
    public partial class FormBuilder_Template_Field
    {
        public enum FieldType
        {
            ShortText,
            LongText,
            DropDownList,
            CheckBox,
            RadioButton
        }

        private void FillListControlWithOptions(ListControl ctl, List<string> options)
        {
            foreach (string s in options)
                ((ListControl)ctl).Items.Add(s);
        }

        public string GetInputControlId()
        {
            return "FormBuilderCtl" + this.Id;
        }

        public WebControl GetInputControl()
        {
            List<string> options = this.GetFieldOptions();
            int repeatColumns = 1;
            if (options.Count > 4)
            {
                if (options.Count > 8)
                    repeatColumns = 3;
                else
                    repeatColumns = 2;
            }
            WebControl ctl = null;
            switch ((FieldType)this.Type)
            {
                case FieldType.DropDownList:
                    ctl = new DropDownList();
                    ctl.CssClass = "wm_DropDownList";
                    FillListControlWithOptions((ListControl)ctl, options);
                    ((ListControl)ctl).Items.Insert(0, "");
                    break;
                case FieldType.RadioButton:
                    ctl = new RadioButtonList();
                    ctl.CssClass = "wm_RadioButton";
                    ((RadioButtonList)ctl).RepeatColumns = repeatColumns;
                    ((RadioButtonList)ctl).RepeatDirection = RepeatDirection.Vertical;

                    FillListControlWithOptions((ListControl)ctl, options);
                    break;
                case FieldType.CheckBox:
                    ctl = new CheckBoxList();
                    ctl.CssClass = "wm_Checkbox";
                    ((CheckBoxList)ctl).RepeatColumns = repeatColumns;
                    ((CheckBoxList)ctl).RepeatDirection = RepeatDirection.Vertical;
                    FillListControlWithOptions((ListControl)ctl, options);
                    break;
                case FieldType.ShortText:
                    ctl = new TextBox();
                    ctl.CssClass = "wm_ShortText";
                    //((TextBox)ctl).Columns = this.FieldWidth >0 ? this.FieldWidth : 20;

                    ((TextBox)ctl).TextMode = TextBoxMode.SingleLine;
                    break;
                case FieldType.LongText:
                    ctl = new TextBox();
                    ctl.CssClass = "wm_Longtext";
                    ((TextBox)ctl).TextMode = TextBoxMode.MultiLine;
                    break;
                default:
                    return null;
            }
            if (ctl != null)
                ctl.Width = new Unit((double)(this.Width > 0 ? this.Width : 150), UnitType.Pixel);
            ctl.ID = this.GetInputControlId();
            return ctl;
        }

        public BaseValidator GetInputValidator()
        {
            BaseValidator validator = null;

            switch ((FieldType)this.Type)
            {
                case FieldType.CheckBox:
                    validator = new CheckBoxListValidator();
                    break;
                default:
                    validator = new RequiredFieldValidator();
                    break;
            }

            validator.ControlToValidate = this.GetInputControlId();
            validator.ValidationGroup = FormBuilder_Module.GetValidationGroup(this.TemplateId);
            validator.Text = "Required";
            validator.Display = ValidatorDisplay.None;
            validator.ErrorMessage = string.Format("{0} input is required.", this.Name);
            validator.Visible = true;

            return validator;
        }

        public bool HasOptions()
        {
            bool hasOptions = this.GetFieldOptions().Count > 0;
            return hasOptions;
        }

        public static bool IsOptionsFieldType(FieldType fieldType)
        {
            switch (fieldType)
            {
                case FieldType.DropDownList:
                case FieldType.RadioButton:
                case FieldType.CheckBox:
                    return true;
                case FieldType.ShortText:
                case FieldType.LongText:
                default:
                    return false;
            }
        }

        public List<string> GetFieldOptions()
        {
            if (string.IsNullOrEmpty(this.Options))
                return new List<string>();

            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            using (StringReader sr = new StringReader(this.Options))
            {
                return (List<string>)xs.Deserialize(sr);
            }
        }

        public void SetFieldOptions(List<string> options)
        {
            if (null == options
                || options.Count == 0
                || !IsOptionsFieldType((FieldType)this.Type))
            {
                this.Options = null;
                return;
            }

            XmlSerializer xs = new XmlSerializer(typeof(List<string>));
            using (StringWriter sw = new StringWriter())
            {
                xs.Serialize(sw, options);
                this.Options = sw.ToString();
            }
        }

        /// <summary>
        /// Increments the form-field sort-order up, and re-sorts sibling images.
        /// </summary>
        public static void MoveUp(int fieldId)
        {
            MovePosition(fieldId, false);
        }

        /// <summary>
        /// Increments the form-field sort-order down, and re-sorts sibling images.
        /// </summary>
        public static void MoveDown(int fieldId)
        {
            MovePosition(fieldId, true);
        }

        private static void MovePosition(int fieldId, bool direction)
        {
            FormBuilderDataContext dc = new FormBuilderDataContext();
            dc.FormBuilder_Field_MovePosition(fieldId, direction);
        }

    }
}
