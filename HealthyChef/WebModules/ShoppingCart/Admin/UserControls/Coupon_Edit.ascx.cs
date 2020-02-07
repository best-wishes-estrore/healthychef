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
    public partial class Coupon_Edit : FormControlBase
    {
        /// <summary>
        /// This property will determine whether the Redeem Code text field is available for entry. 
        /// </summary>
        public bool EnableRedeemCode
        {
            get
            {
                if (ViewState["EnableRedeemCode"] == null)
                    ViewState["EnableRedeemCode"] = true;

                return bool.Parse(ViewState["EnableRedeemCode"].ToString());
            }
            set { ViewState["EnableRedeemCode"] = value; }
        }

        hccCoupon CurrentCoupon { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            txtRedeemCode.TextChanged += new EventHandler(txtRedeemCode_TextChanged);
            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
            btnDeactivate.Click += new EventHandler(btnDeactivate_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            couponID.Value = this.PrimaryKeyIndex.ToString();
            if (!IsPostBack)
            {
                BindddlDiscountTypes();
                BindddlUsageTypes();

                SetButtons();
            }
        }

        void BindddlDiscountTypes()
        {
            if (ddlDiscountTypes.Items.Count == 0)
            {
                try
                {
                    List<Tuple<string, int>> discTypes = Enums.GetEnumAsTupleList(typeof(Enums.CouponDiscountType));

                    ddlDiscountTypes.DataSource = discTypes;
                    ddlDiscountTypes.DataTextField = "Item1";
                    ddlDiscountTypes.DataValueField = "Item2";
                    ddlDiscountTypes.DataBind();

                    ddlDiscountTypes.Items.Insert(0, new ListItem { Text = "Select discount type...", Value = "-1" });
                }
                catch
                { throw; }
            }
        }

        void BindddlUsageTypes()
        {
            if (ddlUsageTypes.Items.Count == 0)
            {
                try
                {
                    List<Tuple<string, int>> discTypes = Enums.GetEnumAsTupleList(typeof(Enums.CouponUsageType));

                    ddlUsageTypes.DataSource = discTypes;
                    ddlUsageTypes.DataTextField = "Item1";
                    ddlUsageTypes.DataValueField = "Item2";
                    ddlUsageTypes.DataBind();

                    ddlUsageTypes.Items.Insert(0, new ListItem { Text = "Select usage type...", Value = "-1" });
                }
                catch { throw; }
            }
        }

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            hccCoupon coupon = hccCoupon.GetById(this.PrimaryKeyIndex);

            if (coupon == null)
                coupon = CurrentCoupon;

            if (coupon != null)
            {
                coupon.Activate(UseReactivateBehavior);
                this.OnSaved(new ControlSavedEventArgs(coupon.CouponID));
            }
        }

        protected void txtRedeemCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string redeemCode = txtRedeemCode.Text.Trim();

                // check if this code exists in the db
                hccCoupon coupon = hccCoupon.GetBy(redeemCode);

                //if so, load coupon to form
                if (coupon == null)
                {
                    string strTemp = txtRedeemCode.Text.Trim().ToUpper();
                    Clear();
                    txtRedeemCode.Text = strTemp;
                    lblcouponvalidation.Visible = false;
                    txtTitle.Enabled = true;
                    txtDescription.Enabled = true;
                    txtAmount.Enabled = true;
                    ddlDiscountTypes.Enabled = true;
                    ddlUsageTypes.Enabled = true;
                    txtStartDate.Enabled = true;
                    txtEndDate.Enabled = true;
                }
                else
                {
                    if (Request.QueryString.AllKeys.Contains("CouponID"))
                    {
                        this.PrimaryKeyIndex = coupon.CouponID;
                        CurrentCoupon = coupon;
                        LoadForm();
                        txtTitle.Enabled = true;
                        txtDescription.Enabled = true;
                        txtAmount.Enabled = true;
                        ddlDiscountTypes.Enabled = true;
                        ddlUsageTypes.Enabled = true;
                        txtStartDate.Enabled = true;
                        txtEndDate.Enabled = true;
                    }
                    else
                    {
                        lblcouponvalidation.Text = "Coupon already exists";
                        lblcouponvalidation.Visible = true;
                        txtTitle.Enabled = false;
                        txtDescription.Enabled = false;
                        txtAmount.Enabled = false;
                        ddlDiscountTypes.Enabled = false;
                        ddlUsageTypes.Enabled = false;
                        txtStartDate.Enabled = false;
                        txtEndDate.Enabled = false;
                    }
                }
            }
            catch { throw; }
        }

        protected override void LoadForm()
        {
            try
            {
                BindddlDiscountTypes();
                BindddlUsageTypes();

                if (CurrentCoupon == null) // try to get from viewstate primary key id
                    CurrentCoupon = hccCoupon.GetById(this.PrimaryKeyIndex);

                if (CurrentCoupon != null)
                {
                    txtRedeemCode.Text = CurrentCoupon.RedeemCode.ToUpper();
                    txtTitle.Text = CurrentCoupon.Title;
                    txtDescription.Text = CurrentCoupon.Description;
                    txtAmount.Text = CurrentCoupon.Amount.ToString("f2");

                    ddlDiscountTypes.SelectedIndex = ddlDiscountTypes.Items.IndexOf(
                        ddlDiscountTypes.Items.FindByValue(CurrentCoupon.DiscountTypeID.ToString()));

                    ddlUsageTypes.SelectedIndex = ddlUsageTypes.Items.IndexOf(
                        ddlUsageTypes.Items.FindByValue(CurrentCoupon.UsageTypeID.ToString()));

                    if (CurrentCoupon.StartDate.HasValue)
                        //txtStartDate.Text = CurrentCoupon.StartDate.Value.ToShortDateString();
                        txtStartDate.Text = CurrentCoupon.StartDate.Value.ToString("MM/dd/yyyy");

                    if (CurrentCoupon.EndDate.HasValue)
                        //txtEndDate.Text = CurrentCoupon.EndDate.Value.ToShortDateString();
                        txtEndDate.Text = CurrentCoupon.EndDate.Value.ToString("MM/dd/yyyy");
                }

                if (CurrentCoupon.IsActive)
                {
                    ShowDeactivate = true;
                    UseReactivateBehavior = false;
                }
                else
                {
                    ShowDeactivate = true;
                    UseReactivateBehavior = true;
                }

                SetButtons();
            }
            catch { throw; }
        }

        protected override void SaveForm()
        {
            try
            {
                hccCoupon coupon = hccCoupon.GetById(this.PrimaryKeyIndex);

                if (coupon == null)
                {
                    coupon = new hccCoupon
                    {
                        CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                        CreatedDate = DateTime.Now,
                        IsActive = true
                    };
                }

                coupon.RedeemCode = txtRedeemCode.Text.Trim().ToUpper();
                coupon.Title = txtTitle.Text.Trim();
                coupon.Description = txtDescription.Text.Trim();
                coupon.Amount = decimal.Parse(txtAmount.Text.Trim());
                coupon.DiscountTypeID = int.Parse(ddlDiscountTypes.SelectedValue);
                coupon.UsageTypeID = int.Parse(ddlUsageTypes.SelectedValue);

                if (string.IsNullOrWhiteSpace(txtStartDate.Text))
                {
                    coupon.StartDate = null;
                }
                else
                {
                    coupon.StartDate = DateTime.Parse(txtStartDate.Text);
                }

                if (string.IsNullOrWhiteSpace(txtEndDate.Text))
                {
                    coupon.EndDate = null;
                }
                else
                {
                    coupon.EndDate = DateTime.Parse(txtEndDate.Text);
                }

                coupon.Save();

                this.OnSaved(new ControlSavedEventArgs(coupon.CouponID));

            }
            catch { throw; }
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtRedeemCode.Text = string.Empty;
            txtTitle.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtAmount.Text = string.Empty;
            ddlDiscountTypes.SelectedIndex = 0;
            ddlUsageTypes.SelectedIndex = 0;
            txtStartDate.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            SetButtons();
        }

        protected override void SetButtons()
        {
            txtRedeemCode.Enabled = EnableRedeemCode;

            ShowDeactivate = false;

            if (this.PrimaryKeyIndex > 0)
            {
                ShowDeactivate = true;

                hccCoupon CurrentCoupon = hccCoupon.GetById(this.PrimaryKeyIndex);

                if (CurrentCoupon != null)
                {
                    if (CurrentCoupon.IsActive)
                    {
                        btnDeactivate.Text = "Retire";
                        UseReactivateBehavior = false;
                    }
                    else
                    {
                        btnDeactivate.Text = "Reactivate";
                        UseReactivateBehavior = true;
                    }
                }
            }

            btnDeactivate.Visible = ShowDeactivate;
        }
    }
}