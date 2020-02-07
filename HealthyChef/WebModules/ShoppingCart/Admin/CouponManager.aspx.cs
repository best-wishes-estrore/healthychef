using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using System.Web.Security;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class CouponManager : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddCoupon.Click += new EventHandler(btnAddCoupon_Click);

            //gvwActiveCoupons.SelectedIndexChanged += new EventHandler(gvwActiveCoupons_SelectedIndexChanged);
            //gvwActiveCoupons.PageIndexChanging += new GridViewPageEventHandler(gvwActiveCoupons_PageIndexChanging);
            //gvwActiveCoupons.RowDataBound += new GridViewRowEventHandler(gvwActiveCoupons_RowDataBound);

            //gvwInactiveCoupons.SelectedIndexChanged += new EventHandler(gvwInactiveCoupons_SelectedIndexChanged);
            //gvwInactiveCoupons.PageIndexChanging += new GridViewPageEventHandler(gvwInactiveCoupons_PageIndexChanging);
            //gvwInactiveCoupons.RowDataBound += new GridViewRowEventHandler(gvwInactiveCoupons_RowDataBound);

            CouponEdit1.ControlSaved += new ControlSavedEventHandler(CouponEdit1_ControlSaved);
            CouponEdit1.ControlCancelled += new ControlCancelledEventHandler(CouponEdit1_ControlCancelled);

            btnSearchCoupons.Click += btnSearchCoupons_Click;


        }

        protected void Page_Load(object sender, EventArgs e)
        {
            int CouponID = 0;
            CurrentUserID.Value = Membership.GetUser().ProviderUserKey.ToString();
            if (Request.QueryString.AllKeys.Contains("CouponID"))
            {
                CouponID = int.Parse(Request.QueryString["CouponID"]);
            }
            if (CouponID!=0)
            {
                CouponEdit1.PrimaryKeyIndex = CouponID;
                CouponEdit1.Bind();

                btnAddCoupon.Visible = false;
                divEdit.Visible = true;
                pnlGrids.Visible = false;                
            }
            else if (!IsPostBack)
            {
                //BindgvwActiveCoupons();
                //BindgvwInactiveCoupons();
            }
        }

        protected void btnAddCoupon_Click(object sender, EventArgs e)
        {
            btnAddCoupon.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;
        }

        void BindgvwActiveCoupons()
        {
            var coupons = hccCoupon.GetActive();

            //gvwActiveCoupons.DataSource = coupons;
            //gvwActiveCoupons.DataBind();
        }

        void gvwActiveCoupons_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView gvw = (GridView)sender;

            int certId = int.Parse(gvw.SelectedDataKey.Value.ToString());
            CouponEdit1.PrimaryKeyIndex = certId;
            CouponEdit1.Bind();

            btnAddCoupon.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;
        }

        void gvwActiveCoupons_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwActiveCoupons.PageIndex = e.NewPageIndex;
            BindgvwActiveCoupons();
        }

        void gvwActiveCoupons_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                hccCoupon coupon = (hccCoupon)e.Row.DataItem;
                Enums.CouponDiscountType discType = (Enums.CouponDiscountType)coupon.DiscountTypeID;
                Enums.CouponUsageType discUsage = (Enums.CouponUsageType)coupon.UsageTypeID;
                string tempDetails = string.Empty, tempDates = string.Empty;
                Label lblDetails = (Label)e.Row.FindControl("lblDetails");
                Label lblEffectiveDates = (Label)e.Row.FindControl("lblEffectiveDates");

                try
                {
                    switch (discType)
                    {
                        case Enums.CouponDiscountType.Monetary:
                            tempDetails = string.Format("{0:c} for {1}", coupon.Amount, Enums.GetEnumDescription(discUsage));
                            break;
                        case Enums.CouponDiscountType.Percentage:
                            tempDetails = string.Format("{0:p} for {1}", coupon.Amount / 100, Enums.GetEnumDescription(discUsage));
                            break;
                        default: break;
                    }

                    lblDetails.Text = tempDetails;
                }
                catch { }

                try
                {
                    if (coupon.StartDate.HasValue)
                    {
                        tempDates = "Start: " + coupon.StartDate.Value.ToShortDateString();

                        if (coupon.EndDate.HasValue)
                            tempDates += " - End: " + coupon.EndDate.Value.ToShortDateString();
                    }
                    else
                    {
                        if (coupon.EndDate.HasValue)
                            tempDates += "End: " + coupon.EndDate.Value.ToShortDateString();
                    }

                    lblEffectiveDates.Text = tempDates;
                }
                catch { }
            }
        }

        void BindgvwInactiveCoupons()
        {
            var coupons = hccCoupon.GetInactive();

            //gvwInactiveCoupons.DataSource = coupons;
           // gvwInactiveCoupons.DataBind();
        }

        void gvwInactiveCoupons_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridView gvw = (GridView)sender;

            int certId = int.Parse(gvw.SelectedDataKey.Value.ToString());
            CouponEdit1.PrimaryKeyIndex = certId;
            CouponEdit1.Bind();

            btnAddCoupon.Visible = false;
            divEdit.Visible = true;
            pnlGrids.Visible = false;
        }

        void gvwInactiveCoupons_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //gvwInactiveCoupons.PageIndex = e.NewPageIndex;
            BindgvwInactiveCoupons();
        }

        void gvwInactiveCoupons_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        void btnSearchCoupons_Click(object sender, EventArgs e)
        {
            BindgvwUsedCoupons();
        }

        protected void CouponEdit1_ControlSaved(object sender, ControlSavedEventArgs e)
        {
            CouponEdit1.Clear();

            BindgvwActiveCoupons();
            BindgvwInactiveCoupons();

            btnAddCoupon.Visible = true;
            divEdit.Visible = false;
            pnlGrids.Visible = true;

            lblFeedback.Text = string.Format("Coupon saved: {0}", DateTime.Now);
        }

        protected void CouponEdit1_ControlCancelled(object sender)
        {
            CouponEdit1.Clear();

            btnAddCoupon.Visible = true;
            divEdit.Visible = false;
            pnlGrids.Visible = true;
        }

        protected void BindgvwUsedCoupons()
        {
            DateTime startDate = DateTime.Parse(txtStartDate.Text.Trim());
            DateTime endDate = DateTime.Parse(txtEndDate.Text.Trim());

            //parse date as mm-dd-yyyy
            //DateTime startDate = DateTime.ParseExact(txtStartDate.Text.Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            //DateTime endDate = DateTime.ParseExact(txtEndDate.Text.Trim(), "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);

            if (startDate > endDate)
            {
                DateTime temp = endDate;
                endDate = startDate;
                startDate = temp;

                txtStartDate.Text = startDate.ToShortDateString();
                txtEndDate.Text = endDate.ToShortDateString();
            }

            var c = hccCart.GetCompletedWithCoupon(startDate, endDate)
                .Select(a => new
                {
                    PaymentDue = a.PaymentDue,
                    PurchaseNumber = a.PurchaseNumber,
                    FullName = a.OwnerProfile.FullName,
                    PurchaseDate = a.PurchaseDate,
                    RedeemCode = hccCoupon.GetById(a.CouponID.Value).RedeemCode
                });

            gvwUsedCoupons.DataSource = c;
            gvwUsedCoupons.DataBind();
        }
    }

    public class UsedCoupon
    {
        public UsedCoupon(hccCart cart)
        {
            DateUsed = cart.PurchaseDate.Value;

            if (cart.CouponID.HasValue)
            {
                hccCoupon coup = hccCoupon.GetById(cart.CouponID.Value);

                if (coup != null)
                    CouponInfo = coup.ToString();
            }
            PurchaseInfo = cart.ToString();
        }

        public DateTime DateUsed { get; set; }
        public string CouponInfo { get; set; }
        public string PurchaseInfo { get; set; }

        public List<UsedCoupon> GetFromRange(List<hccCart> carts)
        {
            List<UsedCoupon> outCoupons = new List<UsedCoupon>();

            foreach (hccCart cart in carts)
            {
                outCoupons.Add(new UsedCoupon(cart));
            }

            return outCoupons;
        }
    }
}