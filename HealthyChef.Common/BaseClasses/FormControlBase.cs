using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Security;
using System.Security.Permissions;
using System.Web.UI;
using HealthyChef.Common.Events;
using System.Data;

namespace HealthyChef.Common
{
    /// <summary> // Read this before using class
    /// This class serves as the base class for any admin-side form controls, with concentration on controls 
    /// that represent CRUD functionality for data objects, that are generally used in Add/Edit situations
    /// on various "listings" pages. 
    /// 
    /// Usage Recommendations:
    /// 1) Inherit new control(ascx) from this class.
    /// 2) Create form html. Ex. DeliveryDateMenuEdit.ascx.cs.
    /// 3) Include at least one validation summary.
    /// 4) Include at least one submit button with a click event of "SubmitButtonClick".
    /// 5) Add control validators. Do not provide ValidationGroup property value. Set Display=None.
    /// 
    /// Conventions Assumed: 
    /// Base Class: Assume the use of a single validation group for all elements in the form.
    /// Base Class: Assume that the data object related to the form can be represented with a single int primary key.
    /// General Validation: If a field control in your form does not appear to be firing validation, ensure the control's type is defined in SetFormFieldValidationGroup()
    /// DropDownList Validation: Assume value -1 to be the value of the list blank.
    ///  
    /// </summary>

    public abstract class FormControlBase : System.Web.UI.UserControl
    {
        /// <summary>
        /// This property will determine whether the Deactive/Reactivate button is rendered in the control.
        /// </summary>       
        public virtual bool ShowDeactivate
        {
            get
            {
                if (ViewState["ShowDeactivate"] == null)
                    ViewState["ShowDeactivate"] = false;

                return bool.Parse(ViewState["ShowDeactivate"].ToString());
            }
            set { ViewState["ShowDeactivate"] = value; }
        }

        /// <summary>
        /// This property will determine whether the Deactive/Reactivate button performs an (re)activation or a deactivation action.
        /// </summary>
        public virtual bool UseReactivateBehavior
        {
            get
            {
                if (ViewState["UseReactivateBehavior"] == null)
                    ViewState["UseReactivateBehavior"] = false;

                return bool.Parse(ViewState["UseReactivateBehavior"].ToString());
            }
            set { ViewState["UseReactivateBehavior"] = value; }
        }

        public bool ShowValidationSummary
        {
            get
            {
                if (ViewState["ShowValidationSummary"] == null)
                    ViewState["ShowValidationSummary"] = true;

                return bool.Parse(ViewState["ShowValidationSummary"].ToString());
            }
            set { ViewState["ShowValidationSummary"] = value; }
        }

        public string ValidationGroup
        {
            get
            {
                if (ViewState["ValidationGroup"] == null)
                    ViewState["ValidationGroup"] = string.Empty;

                return ViewState["ValidationGroup"].ToString();
            }
            set
            {
                ViewState["ValidationGroup"] = value;
                //SetFormFieldValidationGroup(value);
            }
        }

        public string ValidationMessagePrefix
        {
            get
            {
                if (ViewState["ValidationMessagePrefix"] == null)
                    ViewState["ValidationMessagePrefix"] = string.Empty;

                return ViewState["ValidationMessagePrefix"].ToString();
            }
            set { ViewState["ValidationMessagePrefix"] = value; }
        }

        public int PrimaryKeyIndex
        {
            get
            {
                if (ViewState["PrimaryKeyIndex"] == null)
                    ViewState["PrimaryKeyIndex"] = 0;

                return int.Parse(ViewState["PrimaryKeyIndex"].ToString());
            }
            set { ViewState["PrimaryKeyIndex"] = value; }
        }

        public void Bind()
        {
            LoadForm();
        }

        public void Save()
        {
            SaveForm();
        }

        public void Save(bool validateForm)
        {
            if (validateForm)
                SubmitButtonClick(this, new EventArgs());
            else
                SaveForm();
        }

        public void Clear()
        {
            ClearForm();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (!string.IsNullOrWhiteSpace(ValidationGroup))
                SetFormFieldValidationGroup(ValidationGroup);

            SetValidationSummaryVisibility();
        }

        public event ControlSavedEventHandler ControlSaved;
        public event ControlCancelledEventHandler ControlCancelled;

        protected virtual void OnSaved(ControlSavedEventArgs e)
        {
            if (ControlSaved != null)
                ControlSaved(this, e);
        }

        protected virtual void OnCancelled()
        {
            if (ControlCancelled != null)
                ControlCancelled(this);
        }

        protected void SubmitButtonClick(object sender, EventArgs e)
        {
            //List<IValidator> iVals = Helpers.GetValidatorsByGroup(Page.Validators, this.ValidationGroup);
            Page.Validate(this.ValidationGroup);
           

            if (Page.IsValid)
            {
                SaveForm();
            }
            //else
            //{
            //    List<IValidator> val = Page.Validators.Cast<IValidator>().Where(a => !a.IsValid).ToList();
            //}
        }

        protected void CancelButtonClick(object sender, EventArgs e)
        {
            this.OnCancelled();
        }

        /// <summary>
        /// Override this method with code to load the fields in the control's form from a data object.
        /// </summary>
        protected abstract void LoadForm();

        /// <summary>
        /// Override this method with code to save the fields in the control's form to a data object.
        /// </summary>
        protected abstract void SaveForm();

        /// <summary>
        /// Override this method with code to clear the fields in the control's form.
        /// </summary>
        protected abstract void ClearForm();

        /// <summary>
        /// Override this method with code to set the visibility of the buttons in the control's form.
        /// </summary>
        protected abstract void SetButtons();

        /// <summary>
        /// Override this method with code to set the validationgroup for all applicable form elements of the child control.
        /// </summary>
        public void SetFormFieldValidationGroup(string validationGroup)
        {
            if (string.IsNullOrWhiteSpace(this.ValidationGroup))
                this.ValidationGroup = validationGroup;

            List<WebControl> ctrls = RecursiveFindWebControls(this.Controls.OfType<WebControl>().ToList());

            foreach (WebControl ctrl in ctrls)
            {
                if (ctrl is ValidationSummary)
                    ((ValidationSummary)ctrl).ValidationGroup = this.ValidationGroup;
                else if (ctrl is RequiredFieldValidator)
                {
                    RequiredFieldValidator rfv = (RequiredFieldValidator)ctrl;
                    rfv.ValidationGroup = this.ValidationGroup;

                    if (!string.IsNullOrWhiteSpace(ValidationMessagePrefix))
                        rfv.ErrorMessage = ValidationMessagePrefix + rfv.ErrorMessage;
                }
                else if (ctrl is CompareValidator)
                {
                    CompareValidator cpv = (CompareValidator)ctrl;
                    cpv.ValidationGroup = this.ValidationGroup;

                    if (!string.IsNullOrWhiteSpace(ValidationMessagePrefix))
                        cpv.ErrorMessage = ValidationMessagePrefix + cpv.ErrorMessage;
                }
                else if (ctrl is CustomValidator)
                {
                    CustomValidator cst = (CustomValidator)ctrl;
                    cst.ValidationGroup = this.ValidationGroup;

                    if (!string.IsNullOrWhiteSpace(ValidationMessagePrefix))
                        cst.ErrorMessage = ValidationMessagePrefix + cst.ErrorMessage;
                }
                else if (ctrl is RegularExpressionValidator)
                {
                    RegularExpressionValidator rev = (RegularExpressionValidator)ctrl;
                    rev.ValidationGroup = this.ValidationGroup;

                    if (!string.IsNullOrWhiteSpace(ValidationMessagePrefix))
                        rev.ErrorMessage = ValidationMessagePrefix + rev.ErrorMessage;
                }
                else if (ctrl is RangeValidator)
                {
                    RangeValidator rng = (RangeValidator)ctrl;
                    rng.ValidationGroup = this.ValidationGroup;

                    if (!string.IsNullOrWhiteSpace(ValidationMessagePrefix))
                        rng.ErrorMessage = ValidationMessagePrefix + rng.ErrorMessage;
                }
                else if (ctrl is TextBox)
                {
                    TextBox txt = ((TextBox)ctrl);
                    txt.ValidationGroup = this.ValidationGroup;
                }
                else if (ctrl is DropDownList)
                    ((DropDownList)ctrl).ValidationGroup = this.ValidationGroup;
                else if (ctrl is Button)
                {
                    Button btn = ((Button)ctrl);
                    btn.ValidationGroup = this.ValidationGroup;
                }
                else if (ctrl is CheckBoxList)
                {
                    CheckBoxList cbl = ((CheckBoxList)ctrl);
                    cbl.ValidationGroup = this.ValidationGroup;
                }
                else if (ctrl is RadioButtonList)
                {
                    RadioButtonList cbl = ((RadioButtonList)ctrl);
                    cbl.ValidationGroup = this.ValidationGroup;
                }
                else { /* and so on.....*/ }


                // need to add the various field validators and other common control types - RC 
            }
        }

        /// <summary>
        /// Will set the visibility of the validation summary controls based on the value of the property: ShowValidationSummary
        /// </summary>
        protected void SetValidationSummaryVisibility()
        {
            try
            {
                List<ValidationSummary> valSums = this.Controls.OfType<ValidationSummary>().ToList();

                if (valSums.Count == 0) // Look to see if they are in a parent panel
                {
                    List<Panel> panels = this.Controls.OfType<Panel>().ToList();

                    if (panels.Count() == 1) // controls are in a panel
                        valSums = panels[0].Controls.OfType<ValidationSummary>().ToList();
                }

                valSums.ForEach(a => a.Visible = ShowValidationSummary);
            }
            catch { throw; }
        }

        protected List<WebControl> RecursiveFindWebControls(List<WebControl> parentControls)
        {
            List<WebControl> outCtrls = parentControls;
            List<Panel> panels = outCtrls.OfType<Panel>().ToList();

            if (panels.Count > 0) // controls are in a panel
            {
                foreach (Panel pnl in panels)
                {
                    outCtrls.Remove(pnl);

                    List<WebControl> childCtrls = pnl.Controls.OfType<WebControl>().ToList();
                    outCtrls.AddRange(childCtrls);

                    List<Panel> childPanels = outCtrls.OfType<Panel>().ToList();
                    if (childPanels.Count > 0) // controls are in a panel
                    {
                        outCtrls.AddRange(RecursiveFindWebControls(childCtrls));
                    }
                }
            }

            return outCtrls;
        }
    }
}
