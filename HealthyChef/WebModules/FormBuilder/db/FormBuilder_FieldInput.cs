using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.FormBuilder
{
    public partial class FormBuilder_FieldInput
    {
        public static string GetControlValue(WebControl ctl)
        {
            if (ctl is CheckBoxList)
            { //return a comma-separated list of all selected items.
                List<string> selectedItems = new List<string>();
                foreach (ListItem li in ((CheckBoxList)ctl).Items)
                    if (li.Selected)
                        selectedItems.Add(li.Value);
                return string.Join(", ", selectedItems.ToArray());
            }

            if (ctl is ListControl) //RadioButtonList, DropDownList
                return ((ListControl)ctl).Text;

            if (ctl is TextBox)
                return ((TextBox)ctl).Text;

            return null;
        }

        public void SetInputValueFromControlValue(WebControl ctl)
        {
            this.InputValue = GetControlValue(ctl);
        }

    }
}
