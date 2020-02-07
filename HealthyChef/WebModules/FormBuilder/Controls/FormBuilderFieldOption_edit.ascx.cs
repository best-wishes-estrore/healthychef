using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BayshoreSolutions.WebModules.FormBuilder.Controls
{
    public partial class FormBuilderFieldOption_edit : System.Web.UI.UserControl
    {
        public List<string> FieldOptions
        {
            get
            {
                List<string> L = (List<string>)ViewState["FieldOptions"];
                if (null == L)
                {
                    L = new List<string>();
                    ViewState["FieldOptions"] = L;
                }
                return L;
            }
            set
            {
                ViewState["FieldOptions"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //need a unique validation group for each instance of this control.
            OptionName.ValidationGroup = this.ClientID + "_OptionsList";
            OptionAdd_Button.ValidationGroup = this.ClientID + "_OptionsList";
            OptionsListRfv1.ValidationGroup = this.ClientID + "_OptionsList";
        }

        public override void DataBind()
        {
            OptionsList.DataSource = this.FieldOptions;
            OptionsList.DataBind();

            base.DataBind();
        }

        protected void OptionAdd_Button_Click(object sender, EventArgs e)
        {
            this.FieldOptions.Add(OptionName.Text);
            this.FieldOptions.Sort(StringComparer.OrdinalIgnoreCase);
            this.DataBind();
            OptionName.Text = "";
        }

        protected void OptionsList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            string optionName = e.CommandArgument.ToString();
            if (e.CommandName == "Delete")
            {
                this.FieldOptions.RemoveAll(delegate(string s) { return s == optionName; });
            }
            this.DataBind();
        }
    }
}