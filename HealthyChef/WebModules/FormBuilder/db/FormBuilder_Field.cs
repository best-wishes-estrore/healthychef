using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Xml.Serialization;
using System.Web.UI.WebControls;
using BayshoreSolutions.Common.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.FormBuilder
{
    public partial class FormBuilder_Field
    {
        public enum FieldType
        {
            ShortText,
            LongText,
            DropDownList,
            CheckBox,
            RadioButton,
            PageHeader,
            SectionHeader,
            FileUpload
        }

        private void FillListControlWithOptions(ListControl ctl, List<string> options, string strClass)
        {
            foreach (string s in options)
            {
                ListItem li = new ListItem(s, s);
                li.Attributes["class"] = strClass;
                ctl.Items.Add(li);
            }
        }

        public string GetInputControlId()
        {
            return "FormBuilderCtl" + Id;
        }

        public WebControl GetInputControl()
        {
            List<string> options = GetFieldOptions();
            int repeatColumns = 1;
            if (options.Count > 4)
            {
                if (options.Count > 8)
                    repeatColumns = 3;
                else
                    repeatColumns = 2;
            }
            WebControl ctl;
            switch ((FieldType)Type)
            {
                case FieldType.DropDownList:
                    ctl = new DropDownList { CssClass = "wm_DropDownList" };
                    FillListControlWithOptions((ListControl)ctl, options, "wm_DropDownListItem");
                    ((ListControl)ctl).Items.Insert(0, "");
                    break;
                case FieldType.RadioButton:
                    ctl = new RadioButtonList { CssClass = "wm_RadioButton" };
                    ((RadioButtonList)ctl).RepeatColumns = repeatColumns;
                    ((RadioButtonList)ctl).RepeatDirection = RepeatDirection.Vertical;

                    FillListControlWithOptions((ListControl)ctl, options, "wm_RadioButtonItem");
                    break;
                case FieldType.CheckBox:
                    ctl = new CheckBoxList { CssClass = "wm_Checkbox" };
                    ((CheckBoxList)ctl).RepeatColumns = repeatColumns;
                    ((CheckBoxList)ctl).RepeatDirection = RepeatDirection.Vertical;
                    FillListControlWithOptions((ListControl)ctl, options, "wm_CheckboxItem");
                    break;
                case FieldType.ShortText:
                    ctl = new TextBox { CssClass = "wm_ShortText" };
                    ((TextBox)ctl).TextMode = TextBoxMode.SingleLine;
                    break;
                case FieldType.LongText:
                    ctl = new TextBox { CssClass = "wm_Longtext" };
                    ((TextBox)ctl).TextMode = TextBoxMode.MultiLine;
                    break;
                case FieldType.PageHeader:
                    ctl = new Panel { CssClass = "wm_Panel" };
                    break;
                case FieldType.SectionHeader:
                    ctl = new WebControl(System.Web.UI.HtmlTextWriterTag.H2) { CssClass = "wm_SectionHeader" };
                    break;
                case FieldType.FileUpload:
                    ctl = new FileUpload { CssClass = "wm_FileUpload" };
                    break;
                default:
                    return null;
            }
            if (ctl != null)
                ctl.Width = new Unit((double)(Width > 0 ? Width : 150), UnitType.Pixel);
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
            validator.ValidationGroup = FormBuilder_Module.GetValidationGroup(this.ModuleId);
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
