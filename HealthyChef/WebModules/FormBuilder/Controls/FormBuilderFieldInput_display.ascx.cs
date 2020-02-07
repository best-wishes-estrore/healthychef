using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.FormBuilder.Controls
{
    public partial class FormBuilderFieldInput_display : System.Web.UI.UserControl
    {
        public bool FieldHasOptions
        {
            get { return (bool)(ViewState["FieldHasOptions"] ?? false); }
            set { ViewState["FieldHasOptions"] = value; }
        }

        string _styleName;
        public string StyleName
        {
            get { return _styleName; }
            set { _styleName = value; }

        }

        public WebControl LoadField(FormBuilder_Field field)
        {
            this.FieldHasOptions = field.HasOptions();
            WebControl fieldCtl = field.GetInputControl();
            _clientId = fieldCtl.ClientID;
            if (field.Type == (int)FormBuilder_Field.FieldType.SectionHeader)
            {
                FieldLabel.Visible = FieldInputPlaceHolder.Visible = false;
                litPlaceHolder.Text = String.Format("<h3 class=\"sectionHeader\">{0}</h3>", field.Name);
            }
            else
            {
                FieldInputPlaceHolder.Controls.Add(fieldCtl);
                FieldLabel.Visible = FieldInputPlaceHolder.Visible = true;

                litPlaceHolder.Visible = false;
                if (field.IsRequired)
                {
                    BaseValidator fieldCtlValidator = field.GetInputValidator();
                    fieldCtlValidator.SetFocusOnError = true;

                    if (field.Type == (int)FormBuilder_Field.FieldType.DropDownList)
                    {
                        RequiredFieldValidator requiredValidator = fieldCtlValidator as RequiredFieldValidator;
                        if (null != requiredValidator)
                            requiredValidator.InitialValue = "";
                    }

                    FieldInputPlaceHolder.Controls.Add(fieldCtlValidator);
                }

                FieldLabel.Text = field.Name;
                FieldLabel.AssociatedControlID = _clientId;
            }

            string strSafeFieldName = string.Empty;
            if (!string.IsNullOrEmpty(field.Name))
            {
                System.Text.StringBuilder sbFieldName = new System.Text.StringBuilder();
                char[] achFieldName = field.Name.ToLower().ToCharArray();
                foreach (char ch in achFieldName)
                {
                    if (ch >= 'a' && ch <= 'z')
                    {
                        sbFieldName.Append(ch);
                    }
                }
                if (sbFieldName.Length > 0)
                {
                    strSafeFieldName = string.Format("wm-field-name-{0}", sbFieldName.ToString());
                }
            }

            string strClassName = string.Format("wm-field wm-field-id-{0} {1}", field.Id, strSafeFieldName);

            fieldDiv.Attributes["class"] = strClassName;

            return fieldCtl;
        }

        string _clientId;
        string GetClassName()
        {
            if (!string.IsNullOrEmpty(_styleName) && _styleName.ToLower() != "block")
                return String.Concat("field-", _styleName);
            else
                return "field";

        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}