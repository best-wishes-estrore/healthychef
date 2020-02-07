using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using HealthyChef.AuthNet;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using System.Web.Security;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using System.Globalization;
using System.Data.SqlClient;
using HealthyChef.DAL.Extensions;
using System.Data;
using System.Text.RegularExpressions;

namespace HealthyChef.WebModules.ShoppingCart.Controls.Cart
{
    public partial class CartDisplay : System.Web.UI.UserControl
    {
        static string Subtotal;
        private Button sender;

        public event CartSavedEventHandler CartSaved;

        public void OnCartSaved(CartEventArgs e)
        {
            if (CartSaved != null)
                CartSaved(this, e);
        }

        public int CurrentCartId
        {
            get
            {
                if (ViewState["CurrentCartId"] == null)
                    ViewState["CurrentCartId"] = 0;

                return int.Parse(ViewState["CurrentCartId"].ToString());
            }
            set
            {
                ViewState["CurrentCartId"] = value;
            }
        }

        public bool IsForPublic
        {
            get
            {
                if (ViewState["IsForPublic"] == null)
                    ViewState["IsForPublic"] = false;

                return bool.Parse(ViewState["IsForPublic"].ToString());
            }
            set
            {
                ViewState["IsForPublic"] = value;
            }
        }

        protected hccCart CurrentCart { get; set; }

        protected List<ProfileCart> CurrentProfileCarts { get; set; }

        public bool DisplayCouponDetails
        {
            get
            {
                if (ViewState["DisplayCouponDetails"] == null)
                    ViewState["DisplayCouponDetails"] = false;

                return bool.Parse(ViewState["DisplayCouponDetails"].ToString());
            }
            set
            {
                ViewState["DisplayCouponDetails"] = value;
            }
        }

        public bool ShowContinueButton
        {
            get
            {
                if (ViewState["ShowContinueButton"] == null)
                    ViewState["ShowContinueButton"] = false;

                return bool.Parse(ViewState["ShowContinueButton"].ToString());
            }
            set
            {
                ViewState["ShowContinueButton"] = value;
            }
        }

        public static bool AuthNetSuccessful { get; set; }

        //New event added to check if the cart is valid, has an amount and is not paid, otherwise redirect to the home page.
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // if user is anonymous this will raise invalid use of null.
                if (Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators")) return;
            }
            catch { }


            // ***** Remove this due to issues with adminstrative operations
            //var cartCheck = hccCart.GetById(CurrentCartId);

            //if (cartCheck == null)
            //    Response.Redirect("/");
            //else
            //{
            //    if (cartCheck.PaymentDue == 0) Response.Redirect("/");
            //    if (cartCheck.Status == Enums.CartStatus.Paid) Response.Redirect("/");
            //}
            // ***** Remove this due to issues with adminstrative operations


            if (IsUserLoggedIn())
            {
                txtCouponCode.Enabled = true;
                btnAddCouponCode.Enabled = true;
                txtRedeemCode.Enabled = true;
                btnRedeemGift.Enabled = true;
                ddlCalczipcode.Enabled = true; //dropdown for shipping zone
            }
            else
            {
                txtCouponCode.Text = "Log in to ";
                txtCouponCode.ToolTip = "You must login to your account to apply a coupon code or gift certificate value to your purchase.  Please click on Member Login above to log in or create a new account.";
                txtCouponCode.Style.Add("text-align", "right");
                txtCouponCode.Enabled = false;
                btnAddCouponCode.Enabled = false;
                txtRedeemCode.Text = "Log in to ";
                txtRedeemCode.Style.Add("text-align", "right");
                txtRedeemCode.ToolTip = "You must login to your account to apply a coupon code or gift certificate value to your purchase.  Please click on Member Login above to log in or create a new account.";
                txtRedeemCode.Enabled = false;
                btnRedeemGift.Enabled = false;
            }
            // Control used for cart error display with specific contorls hidden
            //if (!ShowCartError) return;
            //ShowCartIsError();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnAddCouponCode.Click += btnAddCouponCode_Click;
            btnRedeemGift.Click += btnRedeemGift_Click;
            lvwCart.ItemDataBound += lvwCart_ItemDataBound;

            btnContinue.Click += btnContinue_Click;
            btnClearCart.Click += btnClearCart_Click;
            btnCheckOut.Click += btnCheckOut_Click;

            btnConfirmCancel.Click += btnConfirmCancel_Click;
            btnConfirmComplete.Click += btnConfirmComplete_Click;

            //BindZipCodeDDL();       //Get Zip Code Dropdown
            //BindZipCodeDDLNew();
        }
        protected void CheckBoxRequired_ServerValidate(object sender, ServerValidateEventArgs e)
        {
            e.IsValid = cbTermsAndConditions.Checked;
        }
        protected void ReassignOrderNumbers()
        {
            try
            {
                List<Tuple<string, int, DateTime, int>> orderNums = new List<Tuple<string, int, DateTime, int>>();
                List<hccCartItem> cartItems = hccCartItem.GetBy(CurrentCart.CartID);

                cartItems.ForEach(delegate (hccCartItem item)
                {
                    if (item.UserProfile.ShippingAddressID.HasValue)
                    {
                        if (orderNums.Where(a => a.Item1 == item.OrderNumber).Count() > 0)
                        {
                            var t = orderNums.Where(a => a.Item1 == item.OrderNumber).First();

                            if (t.Item2 != item.UserProfile.ShippingAddressID || t.Item3 != item.DeliveryDate)
                            {
                                // reassign item order number
                                item.OrderNumber = string.Empty;
                                item.GetOrderNumber(CurrentCart);
                            }
                        }
                        else if (orderNums.Where(a => a.Item2 == item.UserProfile.ShippingAddressID
                            && a.Item3 == item.DeliveryDate).Count() > 0)
                        {
                            var t = orderNums.Where(a => a.Item2 == item.UserProfile.ShippingAddressID
                            && a.Item3 == item.DeliveryDate).First();

                            if (t.Item1 != item.OrderNumber)
                            {
                                // reassign item order number
                                item.OrderNumber = t.Item1;
                            }
                        }
                        else
                        {
                            orderNums.Add(new Tuple<string, int, DateTime, int>(item.OrderNumber, item.UserProfile.ShippingAddressID.Value, item.DeliveryDate, item.NumberOfMeals));
                        }
                    }
                });
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Bind()
        {
            try
            {
                pnlCartDisplay.Visible = true;
                pnlConfirm.Visible = false;

                btnContinue.Visible = ShowContinueButton;

                CurrentProfileCarts = new List<ProfileCart>();
                string deliveryType = "";
                int deliveryTypeId;
                if (CurrentCart == null)
                {
                    CurrentCart = hccCart.GetById(CurrentCartId);
                    CurrentCart.IsDefaultCouponRemoved = false;
                }

                if (CurrentCart == null)
                {
                    //Set the DataSource of the ListView to an empty String List to produce empty cart messages
                    lvwCart.DataSource = new List<String>();
                    lvwCart.DataBind();
                }
                else
                {

                    if (SessionManager.CurrentUserProfileInfoChanged == true)
                    {//check for updates to user profile that warrant changes to item order numbering
                        ReassignOrderNumbers();
                        SessionManager.CurrentUserProfileInfoChanged = false;
                    }

                    if (CurrentCart.StatusID == (int)Enums.CartStatus.Unfinalized)
                    {
                        CurrentCart.ModifiedDate = DateTime.Now;

                        if (Helpers.LoggedUser != null)
                            CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                    }

                    List<hccCartItem> cartItems = hccCartItem.GetBy(CurrentCart.CartID);


                    hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);

                    if (parentProfile != null)
                    {
                        divRedeemGift.Visible = true;
                        trBillingInfo.Visible = true;

                        if (CurrentCart.CouponID == null && !CurrentCart.IsDefaultCouponRemoved && parentProfile.DefaultCouponId != null)
                        {
                            hccCoupon coupon = hccCoupon.GetById(parentProfile.DefaultCouponId.Value);
                            if ((coupon.StartDate <= DateTime.Now || !coupon.StartDate.HasValue) && (coupon.EndDate >= DateTime.Now || !coupon.EndDate.HasValue))
                            {
                                CurrentCart.CouponID = parentProfile.DefaultCouponId;
                            }
                        }

                        if (!HttpContext.Current.Request.Url.OriginalString.Contains("Admin"))
                        {
                            aBillEdit1.Visible = true;
                            aBillEdit2.Visible = true;
                        }

                        if (parentProfile.ShippingAddressID.HasValue)
                        {
                            //hccAddress addr = hccAddress.GetById(parentProfile.BillingAddressID.Value);
                            hccAddress addr = hccAddress.GetById(parentProfile.ShippingAddressID.Value);
                            divCalculateShipping.Visible = false;
                            if (addr != null)
                                lblBillingAddress.Text = addr.ToString();

                            var types = Enums.GetEnumAsTupleList(typeof(Enums.DeliveryTypes));
                            //addr = hccAddress.GetById(addr.AddressID);
                            if (Helpers.LoggedUser == null || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Customer"))
                                types.RemoveAt(types.IndexOf(types.Where(a => a.Item2 == (int)Enums.DeliveryTypes.LocalDelivery).Single()));

                            if (addr.DefaultShippingTypeID == 3 && types.Count == 2)
                            {
                                txtCalZipCode.Text = addr.PostalCode;
                                deliveryType = "Local Delivery";
                                deliveryTypeId = 3;
                            }
                            else
                            {
                                deliveryType = types.Where(a => a.Item2 == addr.DefaultShippingTypeID).FirstOrDefault().Item1;
                                txtCalZipCode.Text = addr.PostalCode;
                                deliveryTypeId = addr.DefaultShippingTypeID;
                            }
                            CurrentCart.ShippingAmount = ShippingAddressFee(txtCalZipCode.Text, cartItems, deliveryType, deliveryTypeId);

                            Session["ShippingAmount"] = CurrentCart.ShippingAmount;
                        }

                        // load CardInfo
                        hccUserProfilePaymentProfile payProfile = hccUserProfilePaymentProfile.GetBy(parentProfile.UserProfileID);

                        if (payProfile != null)
                        {
                            CreditCardDisplay1.CurrentCardInfo = payProfile.ToCardInfo();
                            CreditCardDisplay1.Bind();
                        }
                    }
                    else
                    {
                        txtCalZipCode.Text = Session["ZipCodewithoutLogin"] == null ? null : Session["ZipCodewithoutLogin"].ToString();
                        //txtCalZipCode.Text = string.Empty;
                        if (txtCalZipCode.Text != "")
                        {
                            deliveryType = "Customer Pickup";
                            deliveryTypeId = 2;
                            CurrentCart.ShippingAmount = ShippingAddressFee(txtCalZipCode.Text, cartItems, deliveryType, deliveryTypeId);
                            if (CurrentCart.ShippingAmount != 0)
                            {
                                Session["ShippingAmount"] = CurrentCart.ShippingAmount;
                            }
                            else
                            {
                                txtCalZipCode.Text = string.Empty;
                                Session["ShippingAmount"] = CurrentCart.ShippingAmount;
                            }
                        }
                        else
                        {
                            txtCalZipCode.Text = string.Empty;
                            CurrentCart.ShippingAmount = 0;
                            Session["ShippingAmount"] = CurrentCart.ShippingAmount;
                        }
                    }

                    if (cartItems !=null && cartItems.Count > 0)
                    {
                        btnClearCart.Visible = true;
                        btnCheckOut.Visible = true;
                        List<hccProductionCalendar> pc = new List<hccProductionCalendar>();

                        foreach (hccCartItem cartItem in cartItems)
                        {
                            ProfileCart profCart;
                            int shippingAddressId;

                            if (cartItem.IsMealSide)
                                continue;

                            if (!pc.Exists(a => a.DeliveryDate == cartItem.DeliveryDate))
                                pc.Add(hccProductionCalendar.GetBy(cartItem.DeliveryDate));

                            hccProductionCalendar cpc = pc.SingleOrDefault(a => a.DeliveryDate == cartItem.DeliveryDate);

                            if (cpc != null && (cpc.OrderCutOffDate.AddDays(1) >= DateTime.Now || (HttpContext.Current.Request.Url.OriginalString.Contains("Admin"))))
                            {
                                if (cartItem.UserProfile != null && (cartItem.UserProfile.UseParentShipping || cartItem.UserProfile.ShippingAddressID.HasValue))
                                {
                                    if (cartItem.UserProfile.UseParentShipping)
                                        shippingAddressId = parentProfile.ShippingAddressID.Value;
                                    else
                                        shippingAddressId = cartItem.UserProfile.ShippingAddressID.Value;

                                    profCart = CurrentProfileCarts
                                        .SingleOrDefault(a => a.ShippingAddressId == shippingAddressId && a.DeliveryDate == cartItem.DeliveryDate);
                                }
                                else
                                {
                                    profCart = CurrentProfileCarts
                                   .SingleOrDefault(a => a.ShippingAddressId == 0
                                       && a.DeliveryDate == cartItem.DeliveryDate);

                                    shippingAddressId = 0;
                                }

                                if (profCart == null)
                                {
                                    profCart = new ProfileCart(shippingAddressId, cartItem.DeliveryDate);
                                    CurrentProfileCarts.Add(profCart);
                                }
                                if (Session["id"] != null && Session["meals"] != null)
                                {
                                    cartItem.CartID = (int)Session["id"];
                                    cartItem.NumberOfMeals = (int)Session["meals"];
                                }
                                profCart.CartItems.Add(cartItem);
                            }
                            else
                            {
                                cartItem.Delete(((List<hccRecurringOrder>)Session["autorenew"]));
                                lblFeedbackCart.Text = "Item(s) removed from cart due to expiration of availability.";
                                btnCheckOut.Visible = false;
                                btnClearCart.Visible = false;

                                if (IsForPublic)
                                {
                                    HealthyChef.Templates.HealthyChef.Controls.TopHeader header =
                                    (HealthyChef.Templates.HealthyChef.Controls.TopHeader)this.Page.Master.FindControl("TopHeader1");

                                    if (header != null)
                                        header.SetCartCount();
                                }
                            }
                        }
                    }
                    else
                    {
                        btnClearCart.Visible = false;
                        btnCheckOut.Visible = false;
                    }

                    //// display totals
                    CurrentCart.CalculateTotals(CurrentProfileCarts);

                    lvwCart.DataSource = CurrentProfileCarts;
                    lvwCart.DataBind();



                    ////Number of meals
                    //lblNoMeals.Text = cartItems[0].NumberOfMeals.ToString();

                    ////discount  //Coupon Info
                    if (CurrentCart.CouponID.HasValue)
                    {
                        hccCoupon coup = hccCoupon.GetById(CurrentCart.CouponID.Value);

                        if (coup != null)
                        {
                            if ((!coup.StartDate.HasValue || coup.StartDate <= DateTime.Now) && (!coup.EndDate.HasValue || coup.EndDate >= DateTime.Now))
                            {
                                divAddCouponCode.Visible = false;
                                divActiveCoupon.Visible = true;

                                lblActiveCoupon.Text = coup.ToString(DisplayCouponDetails);

                                if (CurrentCart.SubTotalDiscount > 0.00m)
                                {

                                    divDiscounts.Visible = true;
                                    lblDiscount.ForeColor = System.Drawing.Color.Red;
                                    lblDiscount.Text = "(" + "-" + CurrentCart.SubTotalDiscount.ToString("c") + ")";

                                    divSubTotalAdj.Visible = true;
                                    lblSubTotalAdj.Text = (CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount).ToString("c");
                                }
                                else
                                {
                                    divSubTotalAdj.Visible = true;
                                    lblSubTotalAdj.Text = (0.00m).ToString("c");
                                }
                            }
                            else
                            {
                                CurrentCart.CouponID = null;
                                CurrentCart.Save();
                            }
                        }
                    }
                    else
                    {
                        divAddCouponCode.Visible = true;
                        divActiveCoupon.Visible = false;

                        lblDiscount.Text = (0.00m).ToString("c");
                        divSubTotalAdj.Visible = false;

                        lblSubTotalAdj.Text = (0.00m).ToString("c");
                        divDiscounts.Visible = false;
                    }

                    //shipping
                    lblShipping.Text = CurrentCart.ShippingAmount.ToString("c");

                    //total
                    decimal cartTotal = CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount + CurrentCart.TaxAmount + CurrentCart.ShippingAmount;
                    CurrentCart.TotalAmount = Math.Round(cartTotal, 2);

                    // CurrentCart.AccountBalanceCredit;
                    decimal acctBalance = parentProfile != null ? parentProfile.AccountBalance : 0.00m;
                    decimal creditAppliedToBalance = 0.00m;
                    decimal remainAcctBalance = 0.00m;
                    decimal paymentDue = 0.00m;

                    if (acctBalance >= cartTotal)
                    {
                        creditAppliedToBalance = cartTotal;
                    }
                    else
                    {
                        creditAppliedToBalance = acctBalance;
                    }

                    remainAcctBalance = acctBalance - creditAppliedToBalance;
                    paymentDue = cartTotal - creditAppliedToBalance;

                    CurrentCart.CreditAppliedToBalance = Math.Round(creditAppliedToBalance, 2);
                    CurrentCart.PaymentDue = Math.Round(paymentDue, 2);
                    CurrentCart.Save();

                    string Discount = Discount = CurrentCart.SubTotalDiscount == Convert.ToDecimal("0.00") ? (0.00m).ToString("c") : CurrentCart.SubTotalDiscount.ToString("c");
                    var cart = hccCart.GetById(CurrentCartId);
                    if (cart != null)
                    {
                        cart.TaxableAmount = 0;
                        cart.Save();
                    }
                    foreach (var procart in CurrentProfileCarts)
                    {
                        foreach (var cartItem in procart.CartItems)
                        {
                            double discountstring = 0;
                            if (cartItem.Plan_IsAutoRenew != null && cartItem.Plan_IsAutoRenew.Value)
                            {
                                if (Discount != null)
                                {
                                    if (Discount != "$0.00")
                                    {
                                        if (Discount.Contains("($"))
                                        {
                                            Discount = Discount.Replace("($", "").Replace(")", "");
                                        }
                                        else
                                        {
                                            Discount = Discount.Replace("$", "");
                                        }
                                        discountstring = Convert.ToDouble(Discount);
                                    }
                                    else
                                    {
                                        if (Discount.Contains("($"))
                                        {
                                            Discount = Discount.Replace("($", "").Replace(")", "");
                                        }
                                        else
                                        {
                                            Discount = Discount.Replace("$", "");
                                        }
                                        discountstring = Convert.ToDouble(Discount);
                                    }
                                }
                                double discountpereachamount = 0.0;
                                if (cartItem.ItemTypeID == 2)
                                {
                                    discountpereachamount = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.05), 2));
                                }
                                else
                                {
                                    discountpereachamount = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.1), 2));
                                }
                                Discount = "($" + (discountstring + discountpereachamount).ToString("f2") + ")";

                                if (cart != null)
                                {
                                    cart.SubTotalDiscount += Convert.ToDecimal(discountpereachamount);
                                    cart.TotalAmount = Convert.ToDecimal(Convert.ToDouble(cart.TotalAmount) - Convert.ToDouble(discountpereachamount));
                                    cart.PaymentDue = Convert.ToDecimal(Convert.ToDouble(cart.PaymentDue) - Convert.ToDouble(discountpereachamount));
                                    acctBalance = parentProfile != null ? parentProfile.AccountBalance : 0.00m;
                                    creditAppliedToBalance = 0.00m;
                                    remainAcctBalance = 0.00m;
                                    paymentDue = 0.00m;

                                    if (acctBalance >= cart.TotalAmount)
                                    {
                                        creditAppliedToBalance = cart.TotalAmount;
                                    }
                                    else
                                    {
                                        creditAppliedToBalance = acctBalance;
                                    }
                                    remainAcctBalance = acctBalance - creditAppliedToBalance;
                                    paymentDue = cart.TotalAmount - creditAppliedToBalance;
                                    cart.CreditAppliedToBalance = Math.Round(creditAppliedToBalance, 2);
                                    cart.PaymentDue = Math.Round(paymentDue, 2);
                                    cart.Save();
                                }
                                CurrentCart = cart;
                            }
                        }
                    }
                    if (CurrentCart != null)
                    {
                        CurrentCart.TaxableAmount = CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount;
                        CurrentCart.Save();
                    }

                    //// subtotal               
                    lblSubTotal.Text = cart.SubTotalAmount.ToString("c");
                    lblmocksubtotal.Text = (cart.SubTotalAmount - cart.SubTotalDiscount).ToString("c");
                    ////tax
                    lblTax.Text = cart.TaxAmount.ToString("c");
                    divDiscounts.Visible = true;
                    if (cart.SubTotalDiscount > 0.00m)
                    {
                        lblDiscount.ForeColor = System.Drawing.Color.Red;
                        lblDiscount.Text = "-(" + cart.SubTotalDiscount.ToString("c") + ")";
                    }
                    else
                    {
                        lblDiscount.ForeColor = System.Drawing.Color.Black;
                        lblDiscount.Text = cart.SubTotalDiscount.ToString("c");
                    }
                    lblGrandTotal.Text = cart.TotalAmount.ToString("c");
                    Subtotal = cart.TotalAmount.ToString("c");
                    lblAcctBalance.Text = acctBalance.ToString("c");
                    lblPaymentDue.Text = cart.PaymentDue.ToString("c");
                    lblRemainAcctBalance.Text = remainAcctBalance.ToString("c");

                    if (Request.QueryString["confirm"] != null)
                        btnCheckOut_Click(this, new EventArgs());
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null
                     && ((ex.InnerException is SqlException && ex.InnerException.Message.Contains("Arithmetic overflow"))
                     || (ex.InnerException is ArgumentException && ex.InnerException.Message.Contains(" is out of range"))))
                {
                    lblFeedbackCart.Text = "Total calculated greater than allowable by site. Please reduce the number of items in your cart. If you would like to make a high value purchase, please contact Healthy Chef Customer Service.";
                    btnCheckOut.Visible = false;
                }
                else if (ex.Message.Contains("Tax Lookup could not determine the tax rate"))
                {
                    lblFeedbackCart.Text = ex.Message + ". Please update your profile shipping address that uses this zip code; or contact Healthy Chef Customer Service.";
                    btnCheckOut.Visible = false;
                }
                else
                    throw ex;
            }
        }

        //List items Binding Function...
        protected void lvwCart_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListViewItemType.DataItem)
                {
                    ProfileCart profCart = (ProfileCart)e.Item.DataItem;

                    if (profCart != null)
                    {
                        CartItemList CartItemList1 = (CartItemList)e.Item.FindControl("CartItemList1");

                        if (CartItemList1 != null)
                        {
                            CartItemList1.CurrentProfileCart = profCart;

                            if (e.Item.DataItemIndex == 0)
                                CartItemList1.DisplayHeader = true;

                            CartItemList1.Bind();
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void CartItemList1_CartItemListItemUpdated()
        {
            HealthyChef.Templates.HealthyChef.Controls.TopHeader header =
                         (HealthyChef.Templates.HealthyChef.Controls.TopHeader)this.Page.Master.FindControl("TopHeader1");

            if (header != null)
                header.SetCartCount();

            Bind();
        }

        protected void btnAddCouponCode_Click(object sender, EventArgs e)
        {
            try
            {
                hccCoupon coupon = hccCoupon.GetBy(txtCouponCode.Text.Trim());

                if (coupon != null && coupon.IsActive && (coupon.EndDate == null || coupon.EndDate > DateTime.Now) && (coupon.StartDate == null || coupon.StartDate <= DateTime.Now))
                {
                    if (CurrentCart == null)
                        CurrentCart = hccCart.GetById(CurrentCartId);

                    // check coupon usage against userId
                    Enums.CouponUsageType usageType = (Enums.CouponUsageType)coupon.UsageTypeID;
                    bool canUseCoupon = true;

                    // check CouponsUsed
                    if (usageType == Enums.CouponUsageType.FirstPurchaseOnly || usageType == Enums.CouponUsageType.OneTimeUse)
                    {
                        if (string.IsNullOrWhiteSpace(CurrentCart.AnonymousID))
                        {
                            List<hccCart> couponCarts = hccCoupon.HasBeenUsedByUser(coupon.CouponID, CurrentCart.AspNetUserID.Value);

                            if (couponCarts.Count > 0)
                            {   // previous carts not this cart
                                if (couponCarts.Count > 1
                                    || (couponCarts.Count == 1 && couponCarts[0].CartID != CurrentCartId))
                                    canUseCoupon = false;
                            }

                            if (usageType == Enums.CouponUsageType.FirstPurchaseOnly)
                            {   // previous paid/completed carts
                                if (hccCart.GetBy(CurrentCart.AspNetUserID.Value).Where(a => a.Status == Enums.CartStatus.Paid || a.Status == Enums.CartStatus.Fulfilled).Count() > 0)
                                    canUseCoupon = false;
                            }
                        }
                    }

                    if (canUseCoupon)
                    {
                        CurrentCart.CouponID = coupon.CouponID;
                        CurrentCart.Save();
                        Bind();
                    }
                    else
                    {
                        lblFeedbackCoupon.Text = "The coupon code entered is for limited use only, and has already been used by this account.";
                    }
                }
                else
                {
                    lblFeedbackCoupon.Text = "The coupon code entered does not exist or is not active.";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected bool IsUserLoggedIn()
        {
            try
            {
                // in order to apply coupon code user must be logged in
                if (Helpers.LoggedUser == null)
                    return false;
                //if (CurrentCart.AnonymousID != null)
                //    if (CurrentCart.AnonymousID.Contains("Anon") && CurrentCart.AnonymousID != null)
                //        return false;
            }
            catch (NullReferenceException)
            {
                return false;
            }
            catch (Exception)
            {
                return true;
            }
            return true;
        }

        protected void btnRedeemGift_Click(object sender, EventArgs e)
        {
            try
            {
                hccCartItem giftItem = hccCartItem.GetGiftBy(txtRedeemCode.Text.Trim());
                ImportedGiftCert cert = ImportedGiftCert.GetBy(txtRedeemCode.Text);

                bool isimport = false;
                bool updateLedger = false;

                if (giftItem == null)
                {
                    if (cert != null)
                    {
                        if (cert.is_used == "Y")
                        {
                            lblFeedbackRedeemGift.Text = "The gift certificate code entered has already been redeemed.";
                            lblFeedbackRedeemGift.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            isimport = true;
                            updateLedger = true;
                        }
                    }
                    else
                    {
                        lblFeedbackRedeemGift.Text = "The gift certificate code entered is not recognized.";
                        lblFeedbackRedeemGift.ForeColor = System.Drawing.Color.Red;
                    }
                }
                else
                {
                    hccCart giftCart = hccCart.GetById(giftItem.CartID);

                    if (giftCart != null
                        && (giftCart.Status == Enums.CartStatus.Paid || giftCart.Status == Enums.CartStatus.Fulfilled))
                    {
                        if (giftItem.Gift_RedeemedDate.HasValue)
                        {
                            lblFeedbackRedeemGift.Text = "The gift certificate code entered has already been redeemed.";
                            lblFeedbackRedeemGift.ForeColor = System.Drawing.Color.Red;
                        }
                        else
                        {
                            updateLedger = true;
                        }
                    }
                    else
                    {
                        lblFeedbackRedeemGift.Text = "The gift certificate code entered has not yet been purchased.";
                        lblFeedbackRedeemGift.ForeColor = System.Drawing.Color.Red;
                    }
                }

                if (updateLedger)
                {
                    if (CurrentCart == null)
                        CurrentCart = hccCart.GetById(CurrentCartId);

                    hccUserProfile profile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);

                    if (profile == null)
                    {
                        if (isimport)
                        {
                            // add amount to cart account balance temp field hccCart.AnonGiftRedeemCredit
                            if (cert.amount.HasValue)
                                CurrentCart.AnonGiftRedeemCredit = cert.amount.Value;

                            CurrentCart.RedeemedGiftCertCode = cert.code.ToString();
                            CurrentCart.Save();

                            cert.date_used = DateTime.Now.ToString();
                            cert.is_used = "Y";
                            cert.Save();
                        }
                        else
                        {
                            // add amount to cart account balance temp field hccCart.AnonGiftRedeemCredit
                            CurrentCart.AnonGiftRedeemCredit = giftItem.ItemPrice;
                            CurrentCart.RedeemedGiftCertCode = giftItem.Gift_RedeemCode;
                            CurrentCart.Save();

                            giftItem.Gift_RedeemedDate = DateTime.Now;
                            giftItem.Save();
                        }
                    }
                    else
                    {
                        if (isimport)
                        {
                            CurrentCart.RedeemedGiftCertCode = cert.code.ToString();
                            CurrentCart.Save();

                            cert.used_by = profile.UserProfileID;
                            cert.date_used = DateTime.Now.ToString();
                            cert.is_used = "Y";
                            cert.Save();

                            // add amount to profile account balance
                            profile.AccountBalance += cert.amount.Value;
                            profile.Save();

                            hccLedger ledger = new hccLedger
                            {
                                PostBalance = profile.AccountBalance,
                                GiftRedeemCode = cert.code.ToString(),
                                AspNetUserID = profile.MembershipID,
                                TotalAmount = cert.amount.Value,
                                AsscCartID = CurrentCart.CartID,
                                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                Description = "Gift Certificate Redemption: " + cert.code,
                                TransactionTypeID = (int)Enums.LedgerTransactionType.RedeemGiftCertificate
                            };
                            ledger.Save();

                            Bind();
                            OnCartSaved(new CartEventArgs(CurrentCartId));

                            lblFeedbackRedeemGift.Text = "The gift certificate " + cert.code +
                                " has been redeemed, and the amount of " + cert.amount + " has been credited to your account.";
                            lblFeedbackRedeemGift.ForeColor = System.Drawing.Color.Green;
                        }
                        else
                        {
                            CurrentCart.RedeemedGiftCertCode = giftItem.Gift_RedeemCode;
                            CurrentCart.Save();

                            giftItem.Gift_RedeemedBy = CurrentCart.AspNetUserID;
                            giftItem.Gift_RedeemedDate = DateTime.Now;
                            giftItem.Save();

                            // add amount to profile account balance
                            profile.AccountBalance += giftItem.ItemPrice;
                            profile.Save();

                            hccLedger ledger = new hccLedger
                            {
                                PostBalance = profile.AccountBalance,
                                GiftRedeemCode = giftItem.Gift_RedeemCode,
                                AspNetUserID = profile.MembershipID,
                                TotalAmount = giftItem.ItemPrice,
                                AsscCartID = CurrentCart.CartID,
                                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                Description = "Gift Certificate Redemption: " + giftItem.Gift_RedeemCode,
                                TransactionTypeID = (int)Enums.LedgerTransactionType.RedeemGiftCertificate
                            };
                            ledger.Save();

                            Bind();
                            OnCartSaved(new CartEventArgs(CurrentCartId));

                            lblFeedbackRedeemGift.Text = "The gift certificate " + giftItem.Gift_RedeemCode +
                                " has been redeemed, and the amount of " + giftItem.ItemPrice.ToString("c") + " has been credited to your account.";
                            lblFeedbackRedeemGift.ForeColor = System.Drawing.Color.Green;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void btnRemoveCoupon_Click(object sender, EventArgs e)
        {
            try
            {
                removeCoupon();
                Bind();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void removeCoupon()
        {
            if (CurrentCart == null)
            {
                CurrentCart = hccCart.GetById(CurrentCartId);
            }

            int? defaultCouponId = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value).DefaultCouponId;

            int? currentCouponId = CurrentCart.CouponID;

            CurrentCart.IsDefaultCouponRemoved = currentCouponId == defaultCouponId;

            CurrentCart.CouponID = currentCouponId != defaultCouponId ? defaultCouponId : null;

            CurrentCart.Save();

            lblDiscount.Text = string.Empty;
        }

        protected void btnClearCart_Click(object sender, EventArgs e)
        {
            Clear();

            if (CurrentCart == null)
                CurrentCart = hccCart.GetById(CurrentCartId);


            Session["autorenew"] = null;
            CurrentCart.RemoveCartItems(null);
            CurrentCart.IsDefaultCouponRemoved = false;

            if (IsForPublic)
            {
                HealthyChef.Templates.HealthyChef.Controls.TopHeader header =
                (HealthyChef.Templates.HealthyChef.Controls.TopHeader)this.Page.Master.FindControl("TopHeader1");

                if (header != null)
                    header.SetCartCount();
            }
            removeCoupon();
            Bind();
        }

        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            Page.Validate("CheckoutGroup");

            if (Page.IsValid)
            {
                if (CurrentCart == null)
                    CurrentCart = hccCart.GetById(CurrentCartId);

                hccUserProfile profile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);

                if (profile != null)
                {
                    pnlCartDisplay.Visible = false;
                    pnlConfirm.Visible = true;
                    //if (HasPlan(CurrentCart.CartID)) cbxRecurring.Visible = true;
                    lblOrderDetails.Text = CurrentCart.ToHtml();
                }
                else
                {
                    Response.Redirect("~/login.aspx?fc=1", true);
                }
            }
        }

        protected void cstCheckOut_ServerValidate(object source, ServerValidateEventArgs args)
        {
            cstCheckOut.ErrorMessage = string.Empty;

            if (CurrentCart == null)
                CurrentCart = hccCart.GetById(CurrentCartId);

            hccUserProfile profile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);

            if (profile != null)
            {
                // Check for billing address
                if (profile.BillingAddressID.HasValue)
                {
                    hccAddress billAddr = hccAddress.GetById(profile.BillingAddressID.Value);

                    if (billAddr == null)
                    {
                        args.IsValid = false;
                        cstCheckOut.ErrorMessage += "This profile does not has a valid billing address. ";
                    }
                }
                else
                {
                    args.IsValid = false;
                    cstCheckOut.ErrorMessage += "This profile does not has a valid billing address. ";
                }

                CurrentCart.GetProfilesUsedInOrder().ForEach(delegate (hccUserProfile prof)
                {   // Check for shipping address, for each profile used in the order
                    hccAddress shipAddr = null;
                    if (prof.ShippingAddressID.HasValue)
                    {
                        shipAddr = hccAddress.GetById(prof.ShippingAddressID.Value);
                    }
                    if (shipAddr == null || !shipAddr.IsValid)
                    {
                        args.IsValid = false;
                        cstCheckOut.ErrorMessage += "The profile: " + prof.ProfileName + " does not has a valid shipping address. ";
                    }
                });

                // Check for credit card, create CIM account
                if (CurrentCart.TotalAmount - profile.AccountBalance > 0.00m)
                {
                    if (CurrentCart.OwnerProfile != null)
                    {
                        hccUserProfilePaymentProfile payProf = hccUserProfilePaymentProfile.GetBy(CurrentCart.OwnerProfile.UserProfileID);

                        if (payProf == null)
                        {
                            args.IsValid = false;
                            cstCheckOut.ErrorMessage += "This profile does not has a credit card on file. ";
                        }
                    }
                }

                divValSum.Visible = !args.IsValid;
            }
            else
            {
                //args.IsValid = false;
                //cstCheckOut.ErrorMessage += "This profile is not known. ";
                Response.Redirect("~/login.aspx?fc=1", true);
            }

        }

        void btnContinue_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/order-now.aspx", true);
        }

        protected void btnConfirmCancel_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentCart == null)
                    CurrentCart = hccCart.GetById(CurrentCartId);

                Session["autorenew"] = null;

                if (CurrentCart != null)
                {
                    CurrentCart.StatusID = (int)Enums.CartStatus.Cancelled;
                    CurrentCart.Save();
                    //ShowCartError = false;
                    Response.Redirect("~/order-now.aspx");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void btnConfirmComplete_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentCart == null)
                    CurrentCart = hccCart.GetById(CurrentCartId);

                hccUserProfile profile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                hccAddress billAddr = null;

                if (CurrentCart.StatusID == (int)Enums.CartStatus.Unfinalized)
                {
                    if (profile != null)
                    {
                        profile.CanyonRanchCustomer = cbMarketingOptIn.Checked;
                        profile.Save();
                        AuthNetConfig ANConfig = new AuthNetConfig();
                        hccUserProfilePaymentProfile activePaymentProfile = profile.ActivePaymentProfile;
                        bool isDuplicateTransaction = false;
                        bool isAuthNet = false;

                        if (ANConfig.Settings.TestMode)
                            CurrentCart.IsTestOrder = true;

                        // Check for existing account balance, calculate total balance
                        if (CurrentCart.PaymentDue > 0.00m)
                        {
                            try
                            {   // if total balance remains
                                CustomerInformationManager cim = new CustomerInformationManager();

                                if (activePaymentProfile != null)
                                {
                                    // do not validate, per Duncan, YouTrack HC1-339
                                    //string valProfile = cim.ValidateProfile(profile.AuthNetProfileID,
                                    //    activePaymentProfile.AuthNetPaymentProfileID, AuthorizeNet.ValidationMode.TestMode);

                                    AuthorizeNet.Order order = new AuthorizeNet.Order(profile.AuthNetProfileID,
                                        activePaymentProfile.AuthNetPaymentProfileID, null);

                                    // charge CIM account with PaymentDue balance
                                    order.Amount = CurrentCart.PaymentDue;
                                    order.InvoiceNumber = CurrentCart.PurchaseNumber.ToString();
                                    order.Description = "Healthy Chef Creations Purchase #" + CurrentCart.PurchaseNumber.ToString();

                                    AuthorizeNet.IGatewayResponse rsp = cim.AuthorizeAndCapture(order);

                                    try
                                    {
                                        CurrentCart.AuthNetResponse = rsp.ResponseCode + "|" + rsp.Approved.ToString()
                                       + "|" + rsp.AuthorizationCode + "|" + rsp.InvoiceNumber + "|" + rsp.Message
                                       + "|" + rsp.TransactionID + "|" + rsp.Amount.ToString() + "|" + rsp.CardNumber;
                                    }
                                    catch (Exception) { }

                                    if (rsp.ResponseCode.StartsWith("1"))
                                    {
                                        CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.ModifiedDate = DateTime.Now;
                                        CurrentCart.PurchaseBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.PurchaseDate = DateTime.Now;
                                        CurrentCart.PaymentProfileID = activePaymentProfile.PaymentProfileID;
                                        CurrentCart.StatusID = (int)Enums.CartStatus.Paid;

                                        isAuthNet = true;
                                        AuthNetSuccessful = true;
                                    }
                                    else
                                    {
                                        lblConfirmFeedback.Text = rsp.Message; // CurrentCart.AuthNetResponse;
                                        lblConfirmFeedback.ForeColor = System.Drawing.Color.Red;
                                    }
                                    CurrentCart.Save();
                                }
                                else
                                {
                                    lblConfirmFeedback.Text = "No payment profile found.";
                                }
                            }
                            catch (Exception ex)
                            {
                                lblConfirmFeedback.Text = ex.Message;
                                lblConfirmFeedback.ForeColor = System.Drawing.Color.Red;
                                if (ex is InvalidOperationException)
                                {
                                    if (CurrentCart.IsTestOrder)
                                    {
                                        CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.ModifiedDate = DateTime.Now;
                                        CurrentCart.PaymentProfileID = activePaymentProfile.PaymentProfileID;
                                        CurrentCart.AuthNetResponse = ex.Message;
                                        CurrentCart.StatusID = (int)Enums.CartStatus.Unfinalized;
                                        CurrentCart.Save();
                                    }
                                    else
                                    {
                                        BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(ex.Message, this, ex);
                                        lblConfirmFeedback.Visible = true;
                                        lblConfirmFeedback.Text = ex.Message;
                                        lblConfirmFeedback.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                                else
                                {
                                    throw;
                                    // TODO: If code falls here, then remaining information to complete transaction is not finished (i.e. the code below is skipped)
                                    // 1.  Auth.net transaction needs to be rolled back
                                    // 2.  Push the use to an error page that directs the user how to resolve the error
                                    // 3.  On the error page redisplay the cart item with the instructions and warning for the user 
                                    //ShowCartError = true;
                                    //Response.Redirect("/Cart-Error.aspx", true);
                                    //throw;
                                }
                            }
                        }
                        else
                        {
                            // no balance left to pay on order, set as paid
                            CurrentCart.AuthNetResponse = "Paid with account balance.";
                            CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                            CurrentCart.ModifiedDate = DateTime.Now;
                            CurrentCart.PurchaseBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                            CurrentCart.PurchaseDate = DateTime.Now;
                            CurrentCart.PaymentProfileID = activePaymentProfile.PaymentProfileID;
                            CurrentCart.StatusID = (int)Enums.CartStatus.Paid;
                            CurrentCart.Save();
                        }
                        if (((Enums.CartStatus)CurrentCart.StatusID) == Enums.CartStatus.Paid && !isDuplicateTransaction)
                        {
                            hccLedger ledger = new hccLedger
                            {
                                PaymentDue = CurrentCart.PaymentDue,
                                TotalAmount = CurrentCart.TotalAmount,
                                AspNetUserID = CurrentCart.AspNetUserID.Value,
                                AsscCartID = CurrentCart.CartID,
                                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                Description = "Cart Order Payment - Purchase Number: " + CurrentCart.PurchaseNumber.ToString(),
                                TransactionTypeID = (int)Enums.LedgerTransactionType.Purchase
                            };

                            if (CurrentCart.IsTestOrder)
                                ledger.Description += " - Test Mode";

                            profile.AccountBalance = profile.AccountBalance - CurrentCart.CreditAppliedToBalance;
                            ledger.CreditFromBalance = CurrentCart.CreditAppliedToBalance;

                            hccLedger lastLedger = hccLedger.GetByMembershipID(profile.MembershipID, null).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                            bool isDuplicateLedger = false;

                            if (lastLedger != null)
                            {
                                if (ledger.CreatedBy == lastLedger.CreatedBy
                                    && ledger.CreditFromBalance == lastLedger.CreditFromBalance
                                    && ledger.Description == lastLedger.Description
                                    && ledger.PaymentDue == lastLedger.PaymentDue
                                    && ledger.TransactionTypeID == lastLedger.TransactionTypeID
                                    && ledger.TotalAmount == lastLedger.TotalAmount)
                                    isDuplicateLedger = true;
                            }

                            if (!isDuplicateLedger)
                            {
                                ledger.PostBalance = profile.AccountBalance;
                                ledger.Save();
                                profile.Save();

                                // create snapshot here
                                hccCartSnapshot snap = new hccCartSnapshot
                                {
                                    CartId = CurrentCartId,
                                    MembershipId = profile.MembershipID,
                                    LedgerId = ledger.LedgerID,
                                    AccountBalance = profile.AccountBalance,
                                    AuthNetProfileId = profile.AuthNetProfileID,
                                    CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                    CreatedDate = DateTime.Now,
                                    DefaultCouponId = profile.DefaultCouponId,
                                    Email = profile.ASPUser.Email,
                                    FirstName = profile.FirstName,
                                    LastName = profile.LastName,
                                    ProfileName = profile.ProfileName,
                                    AuthNetPaymentProfileId = (isAuthNet == true ? activePaymentProfile.AuthNetPaymentProfileID : string.Empty),
                                    CardTypeId = (isAuthNet == true ? activePaymentProfile.CardTypeID : 0),
                                    CCLast4 = (isAuthNet == true ? activePaymentProfile.CCLast4 : string.Empty),
                                    ExpMon = (isAuthNet == true ? activePaymentProfile.ExpMon : 0),
                                    ExpYear = (isAuthNet == true ? activePaymentProfile.ExpYear : 0),
                                    NameOnCard = (isAuthNet == true ? activePaymentProfile.NameOnCard : string.Empty)
                                };
                                snap.Save();

                                hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                                if (parentProfile.BillingAddressID.HasValue)
                                {
                                    billAddr = hccAddress.GetById(parentProfile.BillingAddressID.Value);
                                }

                                hccAddress snapBillAddr = new hccAddress
                                {
                                    Address1 = billAddr.Address1,
                                    Address2 = billAddr.Address2,
                                    AddressTypeID = (int)Enums.AddressType.BillingSnap,
                                    City = billAddr.City,
                                    Country = billAddr.Country,
                                    DefaultShippingTypeID = billAddr.DefaultShippingTypeID,
                                    FirstName = billAddr.FirstName,
                                    IsBusiness = billAddr.IsBusiness,
                                    LastName = billAddr.LastName,
                                    Phone = billAddr.Phone,
                                    PostalCode = billAddr.PostalCode,
                                    State = billAddr.State,
                                    ProfileName = parentProfile.ProfileName
                                };
                                snapBillAddr.Save();

                                // copy and replace of all addresses for snapshot
                                List<hccCartItem> cartItems = hccCartItem.GetBy(CurrentCart.CartID);

                                cartItems.ToList().ForEach(delegate (hccCartItem ci)
                                {
                                    hccAddress shipAddr = null;
                                    if (ci.UserProfile.ShippingAddressID.HasValue)
                                    {
                                        shipAddr = hccAddress.GetById(ci.UserProfile.ShippingAddressID.Value);
                                    }

                                    if (shipAddr != null)
                                    {
                                        hccAddress snapShipAddr = new hccAddress
                                        {
                                            Address1 = shipAddr.Address1,
                                            Address2 = shipAddr.Address2,
                                            AddressTypeID = (int)Enums.AddressType.ShippingSnap,
                                            City = shipAddr.City,
                                            Country = shipAddr.Country,
                                            DefaultShippingTypeID = shipAddr.DefaultShippingTypeID,
                                            FirstName = shipAddr.FirstName,
                                            IsBusiness = shipAddr.IsBusiness,
                                            LastName = shipAddr.LastName,
                                            Phone = shipAddr.Phone,
                                            PostalCode = shipAddr.PostalCode,
                                            State = shipAddr.State,
                                            ProfileName = ci.UserProfile.ProfileName
                                        };
                                        snapShipAddr.Save();
                                        ci.SnapShipAddrId = snapShipAddr.AddressID;
                                    }

                                    ci.SnapBillAddrId = snapBillAddr.AddressID;
                                    ci.Save();

                                });


                                List<hccRecurringOrder> lstRo = null;
                                if (Session["autorenew"] != null)
                                    lstRo = ((List<hccRecurringOrder>)Session["autorenew"]);
                                else
                                    lstRo = new List<hccRecurringOrder>();

                                foreach (var recurringOrder in lstRo.Select(item => new hccRecurringOrder
                                {
                                    CartID = item.CartID,
                                    CartItemID = item.CartItemID,
                                    UserProfileID = item.UserProfileID,
                                    AspNetUserID = CurrentCart.AspNetUserID.Value,
                                    PurchaseNumber = CurrentCart.PurchaseNumber,
                                    TotalAmount = item.TotalAmount
                                }))
                                {

                                    recurringOrder.Save();
                                }
                                // Temporarily commented out to create a deployment for Recurring order fixes 11-04-20013
                                //using (var hccEntity = new healthychefEntities())
                                //{
                                //    hccEntity.hcc_IsCanyonRanchCustomer(CurrentCart.CartID, CurrentCart.OwnerProfile.UserProfileID);
                                //}
                                // Temporarily commented out to create a deployment for Recurring order fixes 11-04-20013
                                try
                                {
                                    Email.EmailController ec = new Email.EmailController();
                                    ec.SendMail_OrderConfirmationMerchant(profile.FirstName + " " + profile.LastName, CurrentCart.ToHtml(), CurrentCartId);
                                    ec.SendMail_OrderConfirmationCustomer(profile.ASPUser.Email, profile.FirstName + " " + profile.LastName, CurrentCart.ToHtml());
                                }
                                catch (Exception ex)
                                { BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Send Mail Failed", this, ex); }//throw; }

                                if (IsForPublic)
                                {
                                    Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                                        CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                                        billAddr.City, billAddr.State, billAddr.Country), false);
                                }
                                else
                                {
                                    CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                                    CurrentCartId = CurrentCart.CartID;

                                    pnlCartDisplay.Visible = true;
                                    pnlConfirm.Visible = false;

                                    Clear();
                                    Bind();
                                }
                                OnCartSaved(new CartEventArgs(CurrentCartId));
                            }
                        }
                        else
                        {
                            BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Duplicate transaction attempted: " + CurrentCart.PurchaseNumber.ToString(), this, new Exception("Duplicate transaction attempted by:" + Helpers.LoggedUser.UserName));
                        }
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx", true);
                    }
                }
                else
                {
                    if (IsForPublic)
                    {
                        //Response.Redirect("~/cart/order-confirmation.aspx?cid=" + CurrentCartId.ToString(), false);
                        Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                                        CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                                        billAddr.City, billAddr.State, billAddr.Country), false);
                    }
                    else
                    {
                        CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                        CurrentCartId = CurrentCart.CartID;

                        pnlCartDisplay.Visible = true;
                        pnlConfirm.Visible = false;

                        Clear();
                        Bind();

                        OnCartSaved(new CartEventArgs(CurrentCartId));
                    }
                }
            }
            catch (Exception)
            {
                //This is where the issue is with the Account Manager
                if (AuthNetSuccessful && CurrentCart.StatusID != (int)Enums.CartStatus.Paid)
                {
                    var ucStatus = new WS_UpdateCartStatus();
                    var lstUci = new List<UpdateCartItem>();
                    var ucI = new UpdateCartItem()
                    {
                        cartId = CurrentCart.CartID,
                        updateStatus = true,
                        statusId = 20,
                        updateAddresses = false,
                        rerunAuthNet = false,
                        createLedgerEntry = false,
                        createNewSnapshot = false,
                        sendCustomerEmail = false,
                        sendMerchantEmail = false,
                        repairCartCals = false,
                        reCalcItemTax = false
                    };
                    lstUci.Add(ucI);
                    ucStatus.UpdateCarts(lstUci);

                    Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                                        CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                                        hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City,
                                        hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State,
                                        hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country), false);

                }
                //ShowCartError = true;
                //Response.Redirect("/Cart-Error.aspx", true);
                //throw;
            }
        }

        void Clear()
        {
            lblSubTotal.Text = string.Empty;
            lblmocksubtotal.Text = string.Empty;
            lblTax.Text = string.Empty;
            lblSubTotalAdj.Text = string.Empty;
            lblShipping.Text = string.Empty;
            lblGrandTotal.Text = string.Empty;
            lblAcctBalance.Text = string.Empty;
            lblPaymentDue.Text = string.Empty;
        }

        //Will Martinez - Changes as of 8/1/2013
        bool HasPlan(int cartID)
        {
            var cartItems = hccCartItem.GetBy(CurrentCart.CartID);
            return cartItems.Any(item => item.ItemType == Enums.CartItemType.DefinedPlan);
        }
        /// <summary>
        /// Bind Zip Code in Dropdown List
        /// </summary>
        protected void BindZipCodeDDL()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.ZipCodeDDL();
            //ddlcalcZipcode.DataSource = ds;
            //ddlcalcZipcode.DataTextField = "ZoneName";
            //ddlcalcZipcode.DataValueField = "ZoneID";
            //ddlcalcZipcode.DataBind();
            //ddlcalcZipcode.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void BindZipCodeDDLNew()
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.ZipCodeDDLNew();
            ddlCalczipcode.DataSource = ds;
            ddlCalczipcode.DataTextField = "ZipZoneID";
            ddlCalczipcode.DataValueField = "ZipCodeFrom";
            ddlCalczipcode.DataValueField = "ZipCodeTo";
            ddlCalczipcode.DataBind();
            ddlCalczipcode.Items.Insert(0, new ListItem("--Select--", "0"));
        }
        protected void OncalcZipcode_SelectedIndexChanged(object sender, EventArgs e)
        {
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.ZipCodeDDLNew();
            ddlCalczipcode.DataSource = ds;
            ddlCalczipcode.DataTextField = "ZoneName";
            ddlCalczipcode.DataValueField = "ZoneID";
            ddlCalczipcode.DataBind();
            ddlCalczipcode.Items.Insert(0, new ListItem("--Select--", "0"));
        }

        protected void txtCalZipCode_TextChanged(object sender, EventArgs e)
        {
            string ZipCode = Convert.ToString(txtCalZipCode.Text);
            int noofmeals = 0;
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCalczipcode.DataSource = ds;
                ddlCalczipcode.DataTextField = ds.Tables[0].Columns[0].ColumnName; //"ZoneName"
                ddlCalczipcode.DataValueField = ds.Tables[0].Columns[1].ColumnName; //"ZoneID"
                ddlCalczipcode.DataBind();
                int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());  //ZoneId from On Select Zone Dropdown
                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                if (ds1.Rows.Count > 0)
                {
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();
                    if (IsPickup == "True")
                    {
                        chkIsPickup.Checked = true;
                    }
                    else
                    {
                        chkIsPickup.Checked = false;
                    }
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId

                    MembershipUser user = Helpers.LoggedUser;
                    if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                    {
                        //aCart.Visible = true;
                        hccCart cart = null;

                        if (user == null)
                            cart = hccCart.GetCurrentCart();
                        else
                            cart = hccCart.GetCurrentCart(user);
                        if (cart != null)
                        {
                            List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                            if (cartItems.Count > 0)
                            {
                                foreach (var item in cartItems)
                                {
                                    if (item.ItemTypeID == 1)
                                    {
                                        noofmeals = Convert.ToInt32(item.ItemName.Split('-').Last().Replace(" ", ""));
                                    }
                                    else
                                    {
                                        noofmeals += Convert.ToInt32(item.ItemName.Split('&').Last().Replace(" ", ""));
                                    }
                                }
                            }
                        }
                    }
                    var total = noofmeals;
                    var Large = 28;
                    var Medium = 20;
                    var Small = 8;
                    int largeBoxCost = 0;

                    while (total >= Large)
                    {
                        if (total == 0)
                        {
                            break;
                        }
                        total = total - Large;
                        string a = BoxZone.Rows[2]["Cost"].ToString();
                        a = a.Replace(".00", "");
                        largeBoxCost += Convert.ToInt32(a);

                    }

                    while (total >= Medium)
                    {
                        if (total == 0)
                        {
                            break;
                        }
                        total = total - Medium;
                        string a = BoxZone.Rows[1]["Cost"].ToString();
                        a = a.Replace(".00", "");
                        largeBoxCost += Convert.ToInt32(a);
                    }
                    while (total == Small)
                    {
                        if (total == 0)
                        {
                            break;
                        }
                        total = Small - total;
                        string a = BoxZone.Rows[0]["Cost"].ToString();
                        a = a.Replace(".00", "");
                        largeBoxCost += Convert.ToInt32(a);
                    }

                    while (total <= Small)
                    {
                        if (total == 0)
                        {
                            break;
                        }
                        total = Small - total;
                        string a = BoxZone.Rows[0]["Cost"].ToString();
                        a = a.Replace(".00", "");
                        largeBoxCost += Convert.ToInt32(a);
                        if (total <= Small)
                        {
                            break;
                        }
                    }
                    while (total >= Small)
                    {
                        if (total == 0)
                        {
                            break;
                        }
                        total = total - Small;
                        string a = BoxZone.Rows[0]["Cost"].ToString();
                        a = a.Replace(".00", "");
                        largeBoxCost += Convert.ToInt32(a);
                        if (total <= Small)
                        {
                            total = total - Small;
                            a = BoxZone.Rows[0]["Cost"].ToString();
                            a = a.Replace(".00", "");
                            largeBoxCost += Convert.ToInt32(a);

                            break;
                        }
                    }
                    lblShipping.Text = largeBoxCost.ToString("c");
                    var Subval = Subtotal.Replace("$", "");
                    var Shipval = largeBoxCost + ".00";
                    decimal totalval = Convert.ToDecimal(Subval) + Convert.ToDecimal(Shipval);
                    lblGrandTotal.Text = totalval.ToString("c");
                    lblPaymentDue.Text = totalval.ToString("c");
                }
            }
        }

        protected void btnSearchZipCode_Click(object sender, EventArgs e)
        {
            chkShippingMethod.AutoPostBack = true;
            string ZipCode = Convert.ToString(txtCalZipCode.Text);
            if (ZipCode.Contains("-"))
            {
                ZipCode = ZipCode.Split('-')[0];
            }
            MembershipUser user = Helpers.LoggedUser;
            Session["ZipCodewithoutLogin"] = ZipCode;
            txtCalZipCode.Text = ZipCode;
            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCalczipcode.DataSource = ds;
                ddlCalczipcode.DataTextField = ds.Tables[0].Columns[0].ColumnName; //"ZoneName"
                ddlCalczipcode.DataValueField = ds.Tables[0].Columns[1].ColumnName; //"ZoneID"
                ddlCalczipcode.DataBind();
                int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());  //ZoneId from On Select Zone Dropdown
                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                if (ds1.Rows.Count > 0)
                {
                    string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();
                    decimal MINFee = Convert.ToDecimal(ds1.Rows[0]["MinFee"]);
                    decimal MAXFee = Convert.ToDecimal(ds1.Rows[0]["MaxFee"]);
                    decimal multiplier = Convert.ToDecimal(ds1.Rows[0]["Multiplier"]);
                    string Shiptype = ds1.Rows[0]["TypeName"].ToString();//lblShipDelType
                    lblShipDelType.Text = Shiptype;
                    if (IsPickup == "True")
                    {
                        chkIsPickup.Checked = true;
                        chkIsPickup.Style.Add("display", "none");//block changed for Shipping pickup
                        lblIsPickup.Style.Add("display", "none");//block
                        lblShipMenthod.Style.Add("display", "none");
                        lblShipDelType.Style.Add("display", "none");
                    }
                    else
                    {
                        chkIsPickup.Checked = false;
                        chkIsPickup.Style.Add("display", "none");
                        lblIsPickup.Style.Add("display", "none");
                    }
                    if (IsPickup == "True")
                    {
                        chkIsPickup.Checked = true;
                        DataTable BoxZones = hccshopin.BindBoxToShippingZoneFee(ZoneId);
                        if (BoxZones.Rows.Count > 0)
                        {
                            string PickupFee = BoxZones.Rows[0]["PickupFee"].ToString();
                            var value = string.IsNullOrWhiteSpace(PickupFee) ? "0" : PickupFee;
                            decimal PickupFee1 = Convert.ToDecimal(value);
                            if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                            {
                                hccCart cart = null;
                                if (user == null)
                                    cart = hccCart.GetCurrentCart();
                                else
                                    cart = hccCart.GetCurrentCart(user);
                                if (cart != null)
                                {
                                    List<hccCartItem> cartItemList = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                                    if (cartItemList != null)
                                    {
                                        int maxweekfordelivery1 = 0;
                                        foreach (var items in cartItemList)
                                        {
                                            if (maxweekfordelivery1 < hccshopin.GetNumWeeks(items.CartItemID))
                                            {
                                                maxweekfordelivery1 = hccshopin.GetNumWeeks(items.CartItemID);
                                            }
                                        }
                                        PickupFee1 = maxweekfordelivery1 * PickupFee1;
                                    }
                                }
                            }
                            lblShipping.Text = PickupFee1.ToString("c");

                            var Subval = Subtotal.Replace("$", "");
                            var Shipval = PickupFee1;
                            decimal totalval = Convert.ToDecimal(Subval) + Convert.ToDecimal(Shipval);
                            lblGrandTotal.Text = totalval.ToString("c");
                            lblPaymentDue.Text = totalval.ToString("c");
                        }
                        return;
                    }
                    else
                    {
                        chkIsPickup.Checked = false;
                    }
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId

                    if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                    {
                        hccCart cart = null;

                        if (user == null)
                            cart = hccCart.GetCurrentCart();
                        else
                            cart = hccCart.GetCurrentCart(user);
                        if (cart != null)
                        {
                            List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cart.CartID);
                            if (cartItems.Count > 0)
                            {
                                decimal FeeCost = 0;
                                decimal FeeCost1 = 0;
                                int large = Convert.ToInt32(BoxZone.Rows[2]["MaxNoMeals"]);//28;
                                int medium = Convert.ToInt32(BoxZone.Rows[1]["MaxNoMeals"]);//20;
                                int small = Convert.ToInt32(BoxZone.Rows[0]["MaxNoMeals"]);//8;
                                decimal fee = 0;
                                int LargeBoxCost = 0;
                                int MediumBoxCost = 0;
                                int SmallBoxCost = 0;
                                //int maxweekfordelivery = 0;
                                int noofmealsperweek = 0;
                                //Code for delivery dates
                                DateTime deliverydate;
                                //int CountDelivery = 1;
                                List<DateTime> MinMaxDates = new List<DateTime>();
                                foreach (var items in cartItems)
                                {
                                    if (items.ItemTypeID == 1)
                                    {
                                        deliverydate = items.DeliveryDate;
                                        if (!MinMaxDates.Contains(deliverydate))
                                        {
                                            MinMaxDates.Add(deliverydate);
                                        }
                                    }
                                    else if (items.ItemTypeID == 2)
                                    {
                                        int planids = Convert.ToInt32(items.Plan_PlanID);
                                        hccProgramPlan plans = hccProgramPlan.GetById(planids);
                                        hccProgram progs = hccProgram.GetById(plans.ProgramID);
                                        for (int i = 0; i < plans.NumWeeks; i++)
                                        {
                                            deliverydate = items.DeliveryDate.AddDays(i * 7);
                                            if (!MinMaxDates.Contains(deliverydate))
                                            {
                                                MinMaxDates.Add(deliverydate);
                                            }
                                        }
                                    }

                                }
                                foreach (var date in MinMaxDates)
                                {
                                    noofmealsperweek = 0;
                                    foreach (var items in cartItems)
                                    {
                                        if (items.ItemTypeID == 1)
                                        {
                                            if (items.IsMealSide == false)
                                            {
                                                if (items.DeliveryDate == date)
                                                {
                                                    noofmealsperweek = noofmealsperweek + (1 * items.Quantity);
                                                }
                                            }
                                        }
                                        else if (items.ItemTypeID == 2)
                                        {
                                            int tempnoofmeals = 0;
                                            int planid = Convert.ToInt32(items.Plan_PlanID);
                                            hccProgramPlan plan = hccProgramPlan.GetById(planid);
                                            hccProgram prog = hccProgram.GetById(plan.ProgramID);
                                            if (date >= items.DeliveryDate && date < items.DeliveryDate.AddDays(plan.NumWeeks * 7))//(date <= items.DeliveryDate) && (
                                            {
                                                int numDays = plan.NumDaysPerWeek;
                                                tempnoofmeals = numDays * plan.MealsPerDay;
                                                noofmealsperweek = noofmealsperweek + tempnoofmeals;
                                            }
                                        }
                                        else
                                        {
                                            FeeCost1 = items.ItemPrice;
                                        }
                                    }
                                    int tempLargeBoxcount = 0;
                                    if (noofmealsperweek > 0)
                                    {
                                        while (noofmealsperweek > large)
                                        {
                                            noofmealsperweek = noofmealsperweek - large;
                                            tempLargeBoxcount++;
                                        }
                                        if (tempLargeBoxcount > 0)
                                        {
                                            string largebox = BoxZone.Rows[2]["Cost"].ToString();
                                            largebox = largebox.Replace(".00", "");
                                            LargeBoxCost = Convert.ToInt32(largebox);
                                            fee += LargeBoxCost * tempLargeBoxcount;
                                        }
                                        if (noofmealsperweek <= large && noofmealsperweek > medium)
                                        {
                                            string largebox = BoxZone.Rows[2]["Cost"].ToString();
                                            largebox = largebox.Replace(".00", "");
                                            LargeBoxCost = Convert.ToInt32(largebox);
                                            fee += LargeBoxCost;
                                        }
                                        if (noofmealsperweek <= small)
                                        {
                                            string smallbox = BoxZone.Rows[0]["Cost"].ToString();
                                            smallbox = smallbox.Replace(".00", "");
                                            SmallBoxCost = Convert.ToInt32(smallbox);
                                            fee += SmallBoxCost;
                                        }
                                        if (noofmealsperweek <= medium && noofmealsperweek > small)
                                        {
                                            string mediumbox = BoxZone.Rows[1]["Cost"].ToString();
                                            mediumbox = mediumbox.Replace(".00", "");
                                            MediumBoxCost = Convert.ToInt32(mediumbox);
                                            fee += MediumBoxCost;
                                        }
                                        fee = fee * multiplier;
                                        if (fee <= MINFee)
                                        {
                                            FeeCost += MINFee;
                                        }
                                        else if (fee >= MAXFee)
                                        {
                                            FeeCost += MAXFee;
                                        }
                                        else
                                        {
                                            FeeCost += fee;
                                        }
                                        fee = 0;
                                    }
                                    else
                                    {
                                        FeeCost = 0;
                                        lblShipping.Text = FeeCost.ToString("c");
                                    }
                                    lblShipping.Text = FeeCost.ToString("c");
                                    var Subval1 = Subtotal.Replace("$", "");
                                    var Shipval1 = FeeCost;
                                    decimal totalval1 = Convert.ToDecimal(Subval1) + Convert.ToDecimal(Shipval1);
                                    lblGrandTotal.Text = totalval1.ToString("c");
                                    lblPaymentDue.Text = totalval1.ToString("c");
                                }
                            }
                        }
                    }
                }
            }
        }

        public decimal ShippingAddressFee(string zipcodeID, List<hccCartItem> cart, string deliveryType, int deliveryTypeId)
        {
            string ZipCode = Convert.ToString(zipcodeID);
            if (ZipCode.Contains("-"))
            {
                ZipCode = ZipCode.Split('-')[0];
            }
            decimal FeeCost = 0;
            decimal FeeCost1 = 0;

            hccShippingZone hccshopin = new hccShippingZone();
            DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
            if (ds.Tables[0].Rows.Count > 0)
            {
                ddlCalczipcode.DataSource = ds;
                ddlCalczipcode.DataTextField = ds.Tables[0].Columns[0].ColumnName; //"ZoneName"
                ddlCalczipcode.DataValueField = ds.Tables[0].Columns[1].ColumnName; //"ZoneID"
                ddlCalczipcode.DataBind();
                int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());  //ZoneId from On Select Zone Dropdown
                DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);

                if (ds1.Rows.Count > 0)
                {
                    string IsPickup;
                    if (deliveryTypeId == 2)
                    {
                        IsPickup = "True";
                    }
                    else
                    {
                        IsPickup = "False";
                    }
                    //string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();
                    decimal MINFee = Convert.ToDecimal(ds1.Rows[0]["MinFee"]);
                    decimal MAXFee = Convert.ToDecimal(ds1.Rows[0]["MaxFee"]);
                    decimal multiplier = Convert.ToDecimal(ds1.Rows[0]["Multiplier"]);
                    string Shiptype = ds1.Rows[0]["TypeName"].ToString();
                    lblShipDelType.Text = deliveryType;

                    decimal PickupFee1 = 0;
                    if (IsPickup == "True")
                    {
                        chkIsPickup.Checked = true;
                        chkIsPickup.Style.Add("display", "none");//block
                        lblIsPickup.Style.Add("display", "none");//block
                        lblShipMenthod.Style.Add("display", "none");
                        lblShipDelType.Style.Add("display", "none");
                    }
                    else
                    {
                        chkIsPickup.Checked = false;
                        chkIsPickup.Style.Add("display", "none");
                        lblIsPickup.Style.Add("display", "none");
                    }
                    if (IsPickup == "True")
                    {
                        chkIsPickup.Checked = true;
                        DataTable BoxZones = hccshopin.BindBoxToShippingZoneFee(ZoneId);
                        if (BoxZones.Rows.Count > 0)
                        {
                            string PickupFee = BoxZones.Rows[0]["PickupFee"].ToString();
                            var value = string.IsNullOrWhiteSpace(PickupFee) ? "0" : PickupFee;
                            PickupFee1 = Convert.ToDecimal(value);
                            lblShipping.Text = PickupFee1.ToString("c");
                        }
                        if (cart.Count > 0)
                        {
                            List<hccCartItem> cartItemList = cart;
                            if (cartItemList != null)
                            {
                                int maxweekfordelivery1 = 0;
                                foreach (var items in cartItemList)
                                {
                                    if (maxweekfordelivery1 < hccshopin.GetNumWeeks(items.CartItemID))
                                    {
                                        maxweekfordelivery1 = hccshopin.GetNumWeeks(items.CartItemID);
                                    }
                                }
                                PickupFee1 = maxweekfordelivery1 * PickupFee1;
                            }
                        }
                        return PickupFee1;
                    }
                    else
                    {
                        chkIsPickup.Checked = false;
                    }
                    DataTable BoxZone = hccshopin.BindBoxToShippingZoneFee(ZoneId); //Get Box To Shipping Zone Fee List by ZoneId

                    MembershipUser user = Helpers.LoggedUser;
                    if (cart.Count > 0)
                    {
                        List<hccCartItem> cartItems = cart;
                        if (cartItems != null)
                        {
                            if (cartItems.Count > 0)
                            {
                                int large = Convert.ToInt32(BoxZone.Rows[2]["MaxNoMeals"]);//28;
                                int medium = Convert.ToInt32(BoxZone.Rows[1]["MaxNoMeals"]);//20;
                                int small = Convert.ToInt32(BoxZone.Rows[0]["MaxNoMeals"]);//8;
                                decimal fee = 0;
                                int LargeBoxCost = 0;
                                int MediumBoxCost = 0;
                                int SmallBoxCost = 0;
                                //int maxweekfordelivery = 0;
                                int noofmealsperweek = 0;
                                DateTime deliverydate;
                                //List<hccProductionCalendar> calendar = hccProductionCalendar.GetNext4Calendars();
                                List<DateTime> MinMaxDates = new List<DateTime>();
                                foreach (var items in cartItems)
                                {
                                    if (items.ItemTypeID == 1)
                                    {
                                        deliverydate = items.DeliveryDate;
                                        if (!MinMaxDates.Contains(deliverydate))
                                        {
                                            MinMaxDates.Add(deliverydate);
                                        }
                                    }
                                    else if (items.ItemTypeID == 2)
                                    {
                                        int planids = Convert.ToInt32(items.Plan_PlanID);
                                        hccProgramPlan plans = hccProgramPlan.GetById(planids);
                                        hccProgram progs = hccProgram.GetById(plans.ProgramID);
                                        for (int i = 0; i < plans.NumWeeks; i++)
                                        {
                                            deliverydate = items.DeliveryDate.AddDays(i * 7);
                                            if (!MinMaxDates.Contains(deliverydate))
                                            {
                                                MinMaxDates.Add(deliverydate);
                                            }
                                        }
                                    }

                                }
                                foreach (var date in MinMaxDates)
                                {
                                    noofmealsperweek = 0;
                                    foreach (var items in cartItems)
                                    {
                                        if (items.ItemTypeID == 1)
                                        {
                                            if (items.IsMealSide == false)
                                            {
                                                if (items.DeliveryDate == date)
                                                {
                                                    noofmealsperweek = noofmealsperweek + (1 * items.Quantity);
                                                }
                                            }
                                        }
                                        else if (items.ItemTypeID == 2)
                                        {
                                            int tempnoofmeals = 0;
                                            int planid = Convert.ToInt32(items.Plan_PlanID);
                                            hccProgramPlan plan = hccProgramPlan.GetById(planid);
                                            hccProgram prog = hccProgram.GetById(plan.ProgramID);
                                            if (date >= items.DeliveryDate && date < items.DeliveryDate.AddDays(plan.NumWeeks * 7))//(date <= items.DeliveryDate) && (
                                            {
                                                int numDays = plan.NumDaysPerWeek;
                                                tempnoofmeals = numDays * plan.MealsPerDay;
                                                noofmealsperweek = noofmealsperweek + tempnoofmeals;
                                            }
                                        }
                                        else
                                        {
                                            FeeCost1 = items.ItemPrice;
                                        }
                                    }
                                    int tempLargeBoxcount = 0;
                                    if (noofmealsperweek > 0)
                                    {
                                        while (noofmealsperweek > large)
                                        {
                                            noofmealsperweek = noofmealsperweek - large;
                                            tempLargeBoxcount++;
                                        }
                                        if (tempLargeBoxcount > 0)
                                        {
                                            string largebox = BoxZone.Rows[2]["Cost"].ToString();
                                            largebox = largebox.Replace(".00", "");
                                            LargeBoxCost = Convert.ToInt32(largebox);
                                            fee += LargeBoxCost * tempLargeBoxcount;
                                        }
                                        if (noofmealsperweek <= large && noofmealsperweek > medium)
                                        {
                                            string largebox = BoxZone.Rows[2]["Cost"].ToString();
                                            largebox = largebox.Replace(".00", "");
                                            LargeBoxCost = Convert.ToInt32(largebox);
                                            fee += LargeBoxCost;
                                        }
                                        if (noofmealsperweek <= small)
                                        {
                                            string smallbox = BoxZone.Rows[0]["Cost"].ToString();
                                            smallbox = smallbox.Replace(".00", "");
                                            SmallBoxCost = Convert.ToInt32(smallbox);
                                            fee += SmallBoxCost;
                                        }
                                        if (noofmealsperweek <= medium && noofmealsperweek > small)
                                        {
                                            string mediumbox = BoxZone.Rows[1]["Cost"].ToString();
                                            mediumbox = mediumbox.Replace(".00", "");
                                            MediumBoxCost = Convert.ToInt32(mediumbox);
                                            fee += MediumBoxCost;
                                        }
                                        fee = fee * multiplier;
                                        if (fee <= MINFee)
                                        {
                                            FeeCost += MINFee;
                                        }
                                        else if (fee >= MAXFee)
                                        {
                                            FeeCost += MAXFee;
                                        }
                                        else
                                        {
                                            FeeCost += fee;
                                        }
                                        fee = 0;
                                    }
                                    else
                                    {
                                        FeeCost = 0;
                                        lblShipping.Text = FeeCost.ToString("c");
                                    }
                                    lblShipping.Text = FeeCost.ToString("c");
                                }
                            }
                        }
                    }
                }
            }
            return Convert.ToDecimal(FeeCost);
        }
    }
}
