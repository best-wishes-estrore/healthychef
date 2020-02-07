using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public delegate void PhoneNotSavedEventHandler(PhoneNotSavedEventArgs e);

    public partial class PhoneNumber_Edit : FormControlBase
    {
        public hccUserProfilePhone CurrentPhone { get; set; }

        public event PhoneNotSavedEventHandler PhoneNotSaved;

        public void OnPhoneNotSaved(PhoneNotSavedEventArgs e)
        {
            if (PhoneNotSaved != null)
                PhoneNotSaved(e);
        }

        public int ParentProfileID
        {
            get
            {
                if (ViewState["ParentProfileID"] == null)
                    ViewState["ParentProfileID"] = 0;

                return int.Parse(ViewState["ParentProfileID"].ToString());
            }
            set { ViewState["ParentProfileID"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddPhone.Click += new EventHandler(base.SubmitButtonClick);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindddlPhoneTypes();
            }
        }

        protected void BindddlPhoneTypes()
        {
            if (ddlPhoneTypes.Items.Count == 0)
            {
                ddlPhoneTypes.DataSource = Enums.GetEnumAsTupleList(typeof(Enums.PhoneType));
                ddlPhoneTypes.DataTextField = "Item1";
                ddlPhoneTypes.DataValueField = "Item2";
                ddlPhoneTypes.DataBind();

                ddlPhoneTypes.Items.Insert(0, new ListItem { Text = "Select phone type...", Value = "-1" });
            }
        }

        public void Bind(hccUserProfilePhone phone)
        {
            CurrentPhone = phone;
            Bind();
        }

        protected override void LoadForm()
        {
            try
            {
                if (CurrentPhone == null)
                    CurrentPhone = hccUserProfilePhone.GetById(this.PrimaryKeyIndex);

                if (CurrentPhone != null)
                {
                    txtPhoneNumber.Text = CurrentPhone.PhoneNumber;

                    if (CurrentPhone.PhoneTypeID > 0)
                        ddlPhoneTypes.SelectedIndex =
                            ddlPhoneTypes.Items.IndexOf(ddlPhoneTypes.Items.FindByValue(CurrentPhone.PhoneTypeID.ToString()));

                    chkIsPrimary.Checked = CurrentPhone.IsPrimary;
                }
            }
            catch { throw; }
        }

        protected override void SaveForm()
        {
            try
            {
                hccUserProfilePhone currentPhone = hccUserProfilePhone.GetById(this.PrimaryKeyIndex);
                Enums.PhoneType currentPhoneType = (Enums.PhoneType)Enum.Parse(typeof(Enums.PhoneType), ddlPhoneTypes.SelectedValue);

                if (currentPhone == null)
                {
                    currentPhone = new hccUserProfilePhone
                    {
                        PhoneNumber = txtPhoneNumber.Text,
                        PhoneTypeID = (int)currentPhoneType,
                        IsPrimary = chkIsPrimary.Checked,
                        IsActive = true
                    };

                    if (this.ParentProfileID > 0)
                    {
                        currentPhone.UserProfileID = this.ParentProfileID;
                        currentPhone.Save();

                        OnSaved(new ControlSavedEventArgs(currentPhone.PhoneID));
                    }
                    else
                        OnPhoneNotSaved(new PhoneNotSavedEventArgs(currentPhone));
                }
                else
                {
                    currentPhone.PhoneNumber = txtPhoneNumber.Text.Trim();
                    currentPhone.PhoneTypeID = (int)currentPhoneType;
                    currentPhone.IsPrimary = chkIsPrimary.Checked;
                    currentPhone.IsActive = true;
                    currentPhone.Save();

                    OnSaved(new ControlSavedEventArgs(currentPhone.PhoneID));
                }
            }
            catch { throw; }
        }

        protected override void ClearForm()
        {
            try
            {
                this.PrimaryKeyIndex = 0;
                txtPhoneNumber.Text = string.Empty;
                ddlPhoneTypes.ClearSelection();
                chkIsPrimary.Checked = false;
            }
            catch { throw; }
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// This structure used to pass unsaved phone numbers back the the parent control in the case that no user profile id is yet available to reference.
    /// </summary>
    public class PhoneNotSavedEventArgs : EventArgs
    {
        public hccUserProfilePhone UnSavedPhone { get; set; }

        public PhoneNotSavedEventArgs(hccUserProfilePhone unsavedPhone)
        {
            UnSavedPhone = unsavedPhone;
        }
    }
}