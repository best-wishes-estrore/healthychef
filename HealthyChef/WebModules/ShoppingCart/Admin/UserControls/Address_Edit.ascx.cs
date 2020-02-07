using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;
using System.Data;
using HealthyChef.DAL.Extensions;
using System.Threading;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class Address_Edit : FormControlBase
    {   // Note: this.PrimaryKeyIndex as hccAddress.AddressId

        public Enums.AddressType AddressType
        {
            get
            {
                if (ViewState["AddressType"] == null)
                    ViewState["AddressType"] = Enums.AddressType.Unknown;

                return (Enums.AddressType)Enum.Parse(typeof(Enums.AddressType), ViewState["AddressType"].ToString());
            }
            set
            {
                Enums.AddressType addrType = (Enums.AddressType)Enum.Parse(typeof(Enums.AddressType), value.ToString());

                ViewState["AddressType"] = addrType;
            }
        }

        public hccAddress CurrentAddress
        {
            get
            {
                if (ViewState["CurrentAddress"] == null)
                {
                    if (this.PrimaryKeyIndex > 0)
                        return hccAddress.GetById(this.PrimaryKeyIndex);
                    else
                        return null;
                }
                else
                    return (hccAddress)ViewState["CurrentAddress"];
            }
            set
            {
                ViewState["CurrentAddress"] = value;
            }
        }

        /// <summary>
        /// Determines whether the Save button should be displayed. Default = false.
        /// </summary>
        public bool ShowSave
        {
            get
            {
                if (ViewState["ShowSave"] == null)
                    ViewState["ShowSave"] = false;

                return bool.Parse(ViewState["ShowSave"].ToString());
            }
            set
            {
                ViewState["ShowSave"] = value;
            }
        }

        /// <summary>
        /// Sets the text value of the Save button. Default = "Submit".
        /// </summary>
        public string SaveText
        {
            get
            {
                if (ViewState["SaveText"] == null)
                    ViewState["SaveText"] = "Submit";

                return ViewState["SaveText"].ToString();
            }
            set
            {
                ViewState["SaveText"] = value;
            }
        }

        public bool ShowShippingText
        {
            get
            {
                if (ViewState[this.ID + "ShowShippingText"] == null)
                {
                    ViewState[this.ID + "ShowShippingText"] = false;
                }
                return (bool)ViewState[this.ID + "ShowShippingText"];
            }
            set
            {
                ViewState[this.ID + "ShowShippingText"] = value;
            }

        }
        /// <summary>
        /// Determines whether the IsBusiness field should be displayed. Default = false.
        /// </summary>
        public bool ShowIsBusiness
        {
            get
            {
                if (ViewState["ShowIsBusiness"] == null)
                    ViewState["ShowIsBusiness"] = false;

                return bool.Parse(ViewState["ShowIsBusiness"].ToString());
            }
            set
            {
                ViewState["ShowIsBusiness"] = value;
            }
        }

        public bool ShowDeliveryTypes
        {
            get
            {
                if (ViewState["ShowDeliveryTypes"] == null)
                    ViewState["ShowDeliveryTypes"] = false;

                return bool.Parse(ViewState["ShowDeliveryTypes"].ToString());
            }
            set
            {
                ViewState["ShowDeliveryTypes"] = value;
            }
        }

        /// <summary>
        /// Determines whether the fields in this control should be enabled. Default = true.
        /// </summary>
        public bool EnableFields
        {
            get
            {
                if (ViewState["EnableFields"] == null)
                    ViewState["EnableFields"] = true;

                return bool.Parse(ViewState["EnableFields"].ToString());
            }
            set
            {
                ViewState["EnableFields"] = value;
                SetEnabledFields(value);
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += base.SubmitButtonClick;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetButtons();
                BindDdlStates();
                BindddlDeliveryTypes();
                //if (this.PrimaryKeyIndex == 0)
                //{
                //    if (SaveAddressButton.Visible == false)
                //    {
                //            SaveAddressButton.Visible = true;
                //        //Savesubprofileshipping.Visible = false;
                //    }
                //    else
                //    {
                //         SaveAddressButton.Visible = false;
                //        //Savesubprofileshipping.Visible = true;
                //        //Savesubprofileshipping.Disabled = true;
                //    }
                //}
            }
            btnSave.Visible = false;
            string dataVal = "addr" + this.AddressType;
            lblFeedback0.Attributes.Add("data-ctrl", dataVal);
            pnlAddressEdit.Attributes.Add("data-ctrl", dataVal);
            if (Enums.GetEnumDescription((Enums.AddressType)Convert.ToInt32(Enums.AddressType.Billing)) == ViewState["AddressType"].ToString())
            {
                SaveAddressButton.Visible = false;
                SaveAddressBillingInfoButton.Visible = true;
                billinginfonote.Visible = true;
            }
        }

        protected override void LoadForm()
        {
            SetButtons();
            BindDdlStates();
            BindddlDeliveryTypes();
            if (ShowShippingText)
            {
                ShippingPlaceHolder.Visible = true;
            }
            if (CurrentAddress != null)
            {
                txtFirstName.Text = CurrentAddress.FirstName;
                txtLastName.Text = CurrentAddress.LastName;
                txtAddress1.Text = CurrentAddress.Address1;
                txtAddress2.Text = CurrentAddress.Address2;
                txtCity.Text = CurrentAddress.City;
                ddlUSStates.SelectedIndex = ddlUSStates.Items.IndexOf(ddlUSStates.Items.FindByValue(CurrentAddress.State));
                txtZipCode.Text = CurrentAddress.PostalCode;
                txtPhone.Text = CurrentAddress.Phone;
                hdnAddId.Value = CurrentAddress.AddressID.ToString();
                if (ShowIsBusiness)
                    chkIsBusiness.Checked = CurrentAddress.IsBusiness;

                if (ddlDeliveryTypes.Items.FindByValue(CurrentAddress.DefaultShippingTypeID.ToString()) != null)
                    ddlDeliveryTypes.SelectedIndex = ddlDeliveryTypes.Items.IndexOf(ddlDeliveryTypes.Items.FindByValue(CurrentAddress.DefaultShippingTypeID.ToString()));
            }
        }
        
        protected override void SaveForm()
        {
            try
            {
                if(hdnAddId.Value==null || hdnAddId.Value == "")
                {
                    hdnAddId.Value = "0";
                }

                int AddressId = Convert.ToInt32(hdnAddId.Value);

                hccAddress address;

                if (CurrentAddress == null)
                    address = new hccAddress { Country = "US", AddressTypeID = (int)this.AddressType };
                else
                    address = CurrentAddress;
                int addrId = address.AddressID;
                address.FirstName = txtFirstName.Text.Trim();
                address.LastName = txtLastName.Text.Trim();
                address.Address1 = txtAddress1.Text.Trim();
                address.Address2 = txtAddress2.Text.Trim();
                address.City = txtCity.Text.Trim();
                address.State = ddlUSStates.SelectedValue;
                address.PostalCode = txtZipCode.Text.Trim();
                address.Phone = txtPhone.Text.Trim();

                if (ShowIsBusiness)
                    address.IsBusiness = chkIsBusiness.Checked;
                else
                    address.IsBusiness = false;

                if (ddlDeliveryTypes.Items.Count > 0)
                    address.DefaultShippingTypeID = int.Parse(ddlDeliveryTypes.SelectedItem.Value);

                if (ddlDeliveryTypes.SelectedItem.Value == "2")
                {
                    string ZipCode = txtZipCode.Text;
                    hccShippingZone hccshopin = new hccShippingZone();
                    DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
                    int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());
                    DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                    if (IsPickup == "True")
                    {
                        address.Save();

                        lblFeedback0.Text = ValidationMessagePrefix + " information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");

                        lblFeedback0.ForeColor = System.Drawing.Color.Green;

                        this.PrimaryKeyIndex = address.AddressID;

                        OnSaved(new Common.Events.ControlSavedEventArgs(this.PrimaryKeyIndex));
                    }
                    else
                    {
                        address.Save();
                        lblFeedback0.Text = "Customer pickup is not available at this Zip Code";

                        lblFeedback0.ForeColor = System.Drawing.Color.Red;

                        this.PrimaryKeyIndex = address.AddressID;
                        OnSaved(new Common.Events.ControlSavedEventArgs(this.PrimaryKeyIndex));
                    }
                }
                else
                {
                    address.Save();

                    lblFeedback0.Text = ValidationMessagePrefix + " information saved: " + DateTime.Now.ToString("MM/dd/yyyy h:mm:ss");

                    lblFeedback0.ForeColor = System.Drawing.Color.Green;

                    this.PrimaryKeyIndex = address.AddressID;

                    OnSaved(new Common.Events.ControlSavedEventArgs(this.PrimaryKeyIndex));
                }

            }
            catch
            { throw; }
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            chkIsBusiness.Checked = false;
            txtFirstName.Text = string.Empty;
            txtLastName.Text = string.Empty;
            txtAddress1.Text = string.Empty;
            txtAddress2.Text = string.Empty;
            txtCity.Text = string.Empty;
            ddlUSStates.ClearSelection();
            txtZipCode.Text = string.Empty;
            txtPhone.Text = string.Empty;
            ddlDeliveryTypes.ClearSelection();
        }

        protected override void SetButtons()
        {
            if (ShowSave)
            {
                btnSave.Visible = ShowSave;
                btnSave.Text = SaveText;
            }

            divIsBusiness.Visible = ShowIsBusiness;

            divShowDeliveryTypes.Visible = ShowDeliveryTypes;
            if (ShowDeliveryTypes)
                BindddlDeliveryTypes();
        }

        void BindDdlStates()
        {
            if (ddlUSStates.Items.Count == 0)
            {
                ddlUSStates.DataSource = Helpers.US_States;
                ddlUSStates.DataTextField = "Name";
                ddlUSStates.DataValueField = "Abbr";
                ddlUSStates.DataBind();

                ddlUSStates.Items.Insert(0, new ListItem("Select a state...", "-1"));
            }
        }

        void SetEnabledFields(bool enabled)
        {
            txtFirstName.Enabled = enabled;
            rfvFirstName.Enabled = enabled;
            txtLastName.Enabled = enabled;
            rfvLastName.Enabled = enabled;
            txtAddress1.Enabled = enabled;
            rfvAddress1.Enabled = enabled;
            txtAddress2.Enabled = enabled;
            txtCity.Enabled = enabled;
            rfvCity.Enabled = enabled;
            ddlUSStates.Enabled = enabled;
            rfvUSStates.Enabled = enabled;
            txtZipCode.Enabled = enabled;
            rfvZipCode.Enabled = enabled;
            txtPhone.Enabled = enabled;
        }

        void BindddlDeliveryTypes()
        {
            if (ddlDeliveryTypes.Items.Count == 0)
            {
                var types = Enums.GetEnumAsTupleList(typeof(Enums.DeliveryTypes));

                if (Helpers.LoggedUser == null || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Customer"))
                    types.RemoveAt(types.IndexOf(types.Where(a => a.Item2 == (int)Enums.DeliveryTypes.LocalDelivery).Single()));

                ddlDeliveryTypes.DataSource = types;
                ddlDeliveryTypes.DataTextField = "Item1";
                ddlDeliveryTypes.DataValueField = "Item2";
                ddlDeliveryTypes.DataBind();
            }
        }

        public hccAddress GetCloningAddress()
        {
            try
            {
                hccAddress cloneAddress = new hccAddress
                {
                    Address1 = txtAddress1.Text.Trim(),
                    Address2 = txtAddress2.Text.Trim(),
                    City = txtCity.Text.Trim(),
                    FirstName = txtFirstName.Text.Trim(),
                    LastName = txtLastName.Text.Trim(),
                    Phone = txtPhone.Text.Trim(),
                    PostalCode = txtZipCode.Text.Trim(),
                    State = ddlUSStates.SelectedItem.Value,
                    DefaultShippingTypeID = int.Parse(ddlDeliveryTypes.SelectedValue)
                };
                return cloneAddress;
            }
            catch { return null; }
        }
    }
}