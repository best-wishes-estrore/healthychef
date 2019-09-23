using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class CartViewModel
    {
        public List<ProfileCart> profCart { get; set; }
        public List<hccCartItem> cartItems { get; set; }
        public hccCart CurrentCart { get; set; }
        public hccUserProfile parentProfile { get; set; }

        public string lblFeedbackCart { get; set; }
        public string ShippingMethod { get; set; }
        public string ShippingDeliveryType { get; set; }
        public string BillingAddress { get; set; }
        public bool IsExpiredOrder { get; set; }

        public List<hccCartItem> IsExpiredCartItems { get; set; }

        public string SubTotal { get; set; }
        public string Tax { get; set; }
        public string Shipping { get; set; }

        public hccCoupon ActiveCoupon
        {
            get
            {
                return _getActiveCoupon();
            }
        }

        public string UserProfileString
        {
            get
            {
                return _getUserProfileString();
            }
        }

        public decimal cartTotal
        {
            get
            {
                if(this.CurrentCart != null)
                    return CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount + CurrentCart.TaxAmount + CurrentCart.ShippingAmount;
                else
                    return decimal.Zero;
            }
        }

        public decimal creditAppliedToBalance
        {
            get
            {
                decimal acctBalance = parentProfile != null ? parentProfile.AccountBalance : 0.00m;
                if (acctBalance >= cartTotal)
                {
                    return cartTotal;
                }
                else
                {
                    return acctBalance;
                }
            }
        }
        public string PaymentAmount
        {
            get
            {
                var _paymentDue = cartTotal - creditAppliedToBalance;
                return _paymentDue.ToString("c");
            }
        }

        //public decimal ShippingAmount
        //{
        //    get
        //    {
        //        if (this.CurrentCart != null)
        //            return CurrentCart.ShippingAmount;
        //        else
        //            return decimal.Zero;
        //    }
        //}

        public CreditCardInfo CurrentCardInfo { get; set; }
        public string Discount { set; get; }
        public string SubTotalAdj { set; get; }

        public string GrandTotal { set; get; }
        public string AcctBalance { set; get; }
        public string PaymentDue { set; get; }
        public string RemainAcctBalance { set; get; }
        public string Subtotal { set; get; }
        public string mockSubTotal { get; set; }
        public bool isUserLoggedIn { set; get; }
        public string ShippingAmount { set; get; }


        public CartViewModel()
        {
            try
            {
                MembershipUser user = Helpers.LoggedUser;
                var parentprofile = new hccUserProfile();
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    if (user == null)
                    {
                        this.CurrentCart = hccCart.GetCurrentCart();
                    }
                    else
                    {
                        this.CurrentCart = hccCart.GetCurrentCart(user);
                        parentprofile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);
                        if (parentprofile != null)
                        {
                            if (this.CurrentCart.IsEditCoupon!=1)
                            {
                                this.CurrentCart.CouponID = parentprofile.DefaultCouponId;
                                this.CurrentCart.Save();
                            }
                        }
                    }
                    if (this.CurrentCart != null)
                    {
                        this.cartItems = hccCartItem.GetWithoutSideItemsBy(this.CurrentCart.CartID);                        
                    }
                    else
                    {
                        this.cartItems = new List<hccCartItem>();
                    }
                }

                if (CurrentCart != null)
                {
                    this.parentProfile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                }

                if (this.parentProfile != null)
                {
                    //fetch card details
                    GetCardInfo();
                    hccAddress addr = hccAddress.GetById(parentProfile.ShippingAddressID ?? 0);
                    if (addr != null)
                    {
                        this.BillingAddress = addr.ToString();
                    }
                }

                string txtCalZipCode = "";
                //string deliveryType = "";
                string lblBillingAddress = "";
                int deliveryTypeId = 0;

                if (this.parentProfile != null)
                {
                    if (this.parentProfile.ShippingAddressID.HasValue)
                    {
                        //hccAddress addr = hccAddress.GetById(parentProfile.BillingAddressID.Value);
                        hccAddress addr = hccAddress.GetById(parentProfile.ShippingAddressID.Value);
                        //divCalculateShipping.Visible = false;
                        if (addr != null)
                            lblBillingAddress = addr.ToString();

                        var types = Enums.GetEnumAsTupleList(typeof(Enums.DeliveryTypes));
                        //addr = hccAddress.GetById(addr.AddressID);
                        if (Helpers.LoggedUser == null || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Customer"))
                            types.RemoveAt(types.IndexOf(types.Where(a => a.Item2 == (int)Enums.DeliveryTypes.LocalDelivery).Single()));

                        if (addr.DefaultShippingTypeID == 3 && types.Count == 2)
                        {
                            txtCalZipCode = addr.PostalCode;
                            ShippingDeliveryType = "Local Delivery";
                            deliveryTypeId = 3;
                        }
                        else
                        {
                            ShippingDeliveryType = types.Where(a => a.Item2 == addr.DefaultShippingTypeID).FirstOrDefault().Item1;
                            txtCalZipCode = addr.PostalCode;
                            deliveryTypeId = addr.DefaultShippingTypeID;
                        }
                    }
                }
                IsExpiredCartItems = new List<hccCartItem>();
                this.profCart = new List<ProfileCart>();
                //populate profcarts
                if (this.cartItems != null)
                {
                    if (cartItems.Count > 0)
                    {
                        List<hccProductionCalendar> pc = new List<hccProductionCalendar>();
                        decimal? discre = 0.00m;
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

                                    profCart = this.profCart
                                        .SingleOrDefault(a => a.ShippingAddressId == shippingAddressId && a.DeliveryDate == cartItem.DeliveryDate);
                                }
                                else
                                {
                                    profCart = this.profCart
                                   .SingleOrDefault(a => a.ShippingAddressId == 0
                                       && a.DeliveryDate == cartItem.DeliveryDate);

                                    shippingAddressId = 0;
                                }
                               
                                if (profCart == null)
                                {
                                    profCart = new ProfileCart(shippingAddressId, cartItem.DeliveryDate);
                                    this.profCart.Add(profCart);
                                }
                                profCart.CartItems.Add(cartItem);

                                decimal ShoppingCartAmt = 0;
                                if (cartItem.UserProfile != null)
                                {
                                    if (cartItem.UserProfile.BillingAddressID != null)
                                    {
                                        ShoppingCartAmt = ShippingAddressFee(txtCalZipCode, cartItems, ShippingDeliveryType, deliveryTypeId);
                                    }
                                    else
                                    {
                                        ShoppingCartAmt = CurrentCart.ShippingAmount;
                                    }
                                }
                                HttpContext.Current.Session["ShippingAmount"] = ShoppingCartAmt;

                                CurrentCart.CalculateTotals(this.profCart);

                                //// subtotal               
                                SubTotal = CurrentCart.SubTotalAmount.ToString("c");

                                ////tax
                                Tax = CurrentCart.TaxAmount.ToString("c");

                                //discount  //Coupon Info
                                if (CurrentCart.CouponID.HasValue)
                                {
                                    hccCoupon coup = hccCoupon.GetById(CurrentCart.CouponID.Value);

                                    if (coup != null)
                                    {
                                        if ((!coup.StartDate.HasValue || coup.StartDate <= DateTime.Now) && (!coup.EndDate.HasValue || coup.EndDate >= DateTime.Now))
                                        {
                                            if (CurrentCart.SubTotalDiscount > 0.00m)
                                            {
                                                Discount = CurrentCart.SubTotalDiscount.ToString("c");
                                                SubTotalAdj = (CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount).ToString("c");
                                                mockSubTotal = SubTotalAdj;
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
                                    Discount = CurrentCart.SubTotalDiscount == Convert.ToDecimal("0.00") ? (0.00m).ToString("c") : CurrentCart.SubTotalDiscount.ToString("c");
                                    SubTotalAdj = (0.00m).ToString("c");
                                }
                                
                                //shipping
                                ShippingAmount = CurrentCart.ShippingAmount.ToString("c");

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
                                if (cartItem.DiscretionaryTaxAmount != null)
                                {
                                    discre += cartItem.DiscretionaryTaxAmount;
                                    CurrentCart.DiscretionaryTaxAmount = discre.Value;
                                }
                                CurrentCart.Save();

                                GrandTotal = CurrentCart.PaymentDue.ToString("c");
                                Subtotal = CurrentCart.SubTotalAmount.ToString("c");
                                mockSubTotal = (CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount).ToString("c");
                                AcctBalance = acctBalance.ToString("c");
                                PaymentDue = paymentDue.ToString("c");
                                RemainAcctBalance = remainAcctBalance.ToString("c");
                            }
                            else
                            {
                                IsExpiredCartItems.Add(cartItem);
                            }

                        }
                        var currentcartdetails = hccCart.GetById(this.CurrentCart.CartID);
                        if (currentcartdetails != null)
                        {
                            currentcartdetails.TaxableAmount = 0;
                            currentcartdetails.Save();
                        }
                        foreach (var profileCart in profCart)
                        {

                            foreach (var cartItem in profileCart.CartItems)
                            {
                                double discountstring = 0;
                                if (cartItem.Plan_IsAutoRenew != null)
                                {
                                    if (cartItem.Plan_IsAutoRenew == true&& cartItem.Plan_IsAutoRenew != false)
                                    {
                                        if (Discount != null)
                                        {
                                            if(Discount != "$0.00")
                                                discountstring = Convert.ToDouble(Discount.Replace("$", ""));
                                        }
                                        double discountpereachamount = 0.0;
                                        if (cartItem.ItemTypeID != 1)
                                        {
                                             discountpereachamount = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.05), 2));
                                        }
                                        else if (cartItem.ItemTypeID == 1)
                                        {
                                            discountpereachamount = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.10), 2));
                                        }
                                        Discount = "$" + (discountstring + discountpereachamount).ToString("f2");
                                        var cart = hccCart.GetById(cartItem.CartID);
                                        if(cart!=null)
                                        {
                                            cart.SubTotalDiscount += Convert.ToDecimal(discountpereachamount);
                                            cart.TotalAmount = Convert.ToDecimal(Convert.ToDouble(cart.TotalAmount) - Convert.ToDouble(discountpereachamount));
                                            cart.PaymentDue = Convert.ToDecimal(Convert.ToDouble(cart.PaymentDue) - Convert.ToDouble(discountpereachamount));
                                            decimal acctBalance = parentProfile != null ? parentProfile.AccountBalance : 0.00m;
                                            decimal creditAppliedToBalance = 0.00m;
                                            decimal remainAcctBalance = 0.00m;
                                            decimal paymentDue = 0.00m;

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
                                            GrandTotal = cart.TotalAmount.ToString("c");
                                            mockSubTotal = (Convert.ToDecimal(SubTotal.TrimStart('$')) - cart.SubTotalDiscount).ToString("c");
                                            AcctBalance = acctBalance.ToString("c");
                                            PaymentDue = paymentDue.ToString("c");
                                            RemainAcctBalance = remainAcctBalance.ToString("c");
                                        }
                                    }
                                }

                                double discountpereachamountForItem = 0.0;
                                double discountpereachamountForPrograms = 0.0;
                                var currentcart = hccCart.GetById(this.CurrentCart.CartID);
                                if (currentcart != null)
                                {
                                    if (cartItem.TaxableAmount != null && cartItem.TaxableAmount != 0)
                                    {
                                        currentcart.TaxableAmount += ((cartItem.DiscountAdjPrice) * cartItem.Quantity);
                                    }
                                    else
                                    {
                                        currentcart.TaxableAmount += 0;
                                    }
                                    
                                    if(this.parentProfile != null)
                                    {
                                        //If Item is Familystyle.
                                        hccAddress cartAddress = hccAddress.GetById(this.parentProfile.ShippingAddressID.Value);
                                        if (cartAddress.State == "FL")
                                        {
                                            if (cartItem.Plan_IsAutoRenew == true)
                                            {
                                                if (cartItem.ItemTypeID != 1)
                                                {
                                                    discountpereachamountForPrograms = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.05), 2));
                                                    cartItem.TaxableAmount = cartItem.ItemPrice * cartItem.Quantity - Convert.ToDecimal(discountpereachamountForPrograms);
                                                }
                                                if (cartItem.ItemTypeID == 1)
                                                {
                                                    discountpereachamountForItem = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.10), 2));
                                                    cartItem.TaxableAmount = cartItem.ItemPrice * cartItem.Quantity - Convert.ToDecimal(discountpereachamountForItem);
                                                }

                                                if (cartItem.ItemTypeID == 1)
                                                {
                                                    if (currentcart.CouponID != null)
                                                    {
                                                        if (cartItem.Plan_IsAutoRenew == true)
                                                        {
                                                            var newCoupons = hccCoupon.GetById((int)currentcart.CouponID);
                                                            var copounDiscount = newCoupons.Amount / 100;
                                                            var discountPrice = Math.Round(Convert.ToDecimal(cartItem.ItemPrice * cartItem.Quantity * copounDiscount), 2);
                                                            discountpereachamountForItem = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.10), 2));
                                                            var taxableDiscountPrice = Math.Round(discountPrice + Convert.ToDecimal(discountpereachamountForItem), 2);
                                                            cartItem.TaxableAmount = Math.Round(Convert.ToDecimal(cartItem.ItemPrice * cartItem.Quantity - taxableDiscountPrice), 2);
                                                        }
                                                    }
                                                }

                                                if (cartItem.ItemTypeID != 1)
                                                {
                                                    if (currentcart.CouponID != null)
                                                    {
                                                        if (cartItem.Plan_IsAutoRenew == true)
                                                        {
                                                            var newCoupons = hccCoupon.GetById((int)currentcart.CouponID);
                                                            var copounDiscount = newCoupons.Amount / 100;
                                                            var discountPrice = Math.Round(Convert.ToDecimal(cartItem.ItemPrice * cartItem.Quantity * copounDiscount), 2);
                                                            discountpereachamountForItem = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.05), 2));
                                                            var taxableDiscountPrice = Math.Round(discountPrice + Convert.ToDecimal(discountpereachamountForItem), 2);
                                                            cartItem.TaxableAmount = Math.Round(Convert.ToDecimal(cartItem.ItemPrice * cartItem.Quantity - taxableDiscountPrice), 2);
                                                        }
                                                    }
                                                }

                                                if (cartItem.TaxRateAssigned > 6)
                                                {
                                                    var descAmt = cartItem.TaxRateAssigned - 6;
                                                    var cartItems = Convert.ToDecimal(((descAmt * cartItem.TaxableAmount)) / 100);
                                                    cartItem.DiscretionaryTaxAmount = Convert.ToDecimal(Math.Round(cartItems, 2));
                                                }
                                                cartItem.Save();
                                            }
                                        }

                                        if (cartItem.Plan_IsAutoRenew == false)
                                        {
                                            if (currentcart.CouponID != null)
                                            {
                                                var newCoupons = hccCoupon.GetById((int)currentcart.CouponID);
                                                var copounDiscount = newCoupons.Amount / 100;
                                                var discountPrice = Math.Round(Convert.ToDecimal(cartItem.ItemPrice * cartItem.Quantity * copounDiscount), 2);
                                                cartItem.TaxableAmount = Math.Round(Convert.ToDecimal(cartItem.TaxableAmount - discountPrice), 2);
                                                if (cartItem.TaxRateAssigned > 6)
                                                {
                                                    var descAmt = cartItem.TaxRateAssigned - 6;
                                                    var cartItems = Convert.ToDecimal(((descAmt * cartItem.TaxableAmount)) / 100);
                                                    cartItem.DiscretionaryTaxAmount = Convert.ToDecimal(Math.Round(cartItems, 2));
                                                }
                                                cartItem.Save();
                                            }
                                        }
                                    }
                                    currentcart.IsEditCoupon = 0;
                                    currentcart.Save();
                                }
                                GrandTotal = currentcart.TotalAmount.ToString("c");
                            }
                        }
                        var ccart = hccCart.GetCurrentCart();
                        if(ccart != null)
                        {
                            ccart.TaxableAmount = ccart.SubTotalAmount - ccart.SubTotalDiscount;
                            if(this.parentProfile != null)
                            {
                                if (CurrentCart.CouponID.HasValue || cartItems.FirstOrDefault().Plan_IsAutoRenew == false || cartItems.FirstOrDefault().Plan_IsAutoRenew == true)
                                {
                                    hccAddress cartAddress = hccAddress.GetById(this.parentProfile.ShippingAddressID.Value);
                                    if (cartAddress.State == "FL")
                                    {
                                        decimal acctBalance = parentProfile != null ? parentProfile.AccountBalance : 0.00m;
                                        decimal creditAppliedToBalance = 0.00m;
                                        decimal remainAcctBalance = 0.00m;
                                        decimal paymentDue = 0.00m;

                                        if (cartItems.FirstOrDefault().TaxRateAssigned > 0)
                                        {
                                            var taxRatePercent = cartItems.FirstOrDefault().TaxRateAssigned / 100;
                                            ccart.TaxAmount = Math.Round(Convert.ToDecimal(taxRatePercent * ccart.TaxableAmount), 2);
                                            Tax = ccart.TaxAmount.ToString("c");
                                            //Total Amount
                                            ccart.TotalAmount = ccart.TaxableAmount + ccart.TaxAmount + ccart.ShippingAmount;
                                            if (acctBalance >= ccart.TotalAmount)
                                            {
                                                creditAppliedToBalance = ccart.TotalAmount;
                                            }
                                            else
                                            {
                                                creditAppliedToBalance = acctBalance;
                                            }

                                            remainAcctBalance = acctBalance - creditAppliedToBalance;
                                            paymentDue = ccart.TotalAmount - creditAppliedToBalance;
                                            ccart.CreditAppliedToBalance = Math.Round(creditAppliedToBalance, 2);
                                            ccart.PaymentDue = Math.Round(paymentDue, 2);
                                            GrandTotal = ccart.TotalAmount.ToString("c");
                                            mockSubTotal = (Convert.ToDecimal(SubTotal.TrimStart('$')) - ccart.SubTotalDiscount).ToString("c");
                                            AcctBalance = acctBalance.ToString("c");
                                            PaymentDue = paymentDue.ToString("c");
                                            RemainAcctBalance = remainAcctBalance.ToString("c");
                                        }
                                    }
                                }
                            }
                            ccart.Save();
                        }
                    }
                }
                if (IsUserLoggedIn())
                {
                    isUserLoggedIn = true;
                }
                else
                {
                    isUserLoggedIn = false;
                }
                if(IsExpiredCartItems.Count>0)
                {
                    this.IsExpiredOrder = true;
                }
                else
                {
                    this.IsExpiredOrder = false;
                }
            }
            catch(Exception ex)
            {
                if (IsUserLoggedIn())
                {
                    isUserLoggedIn = true;
                }
                else
                {
                    isUserLoggedIn = false;
                }
                if (ex.InnerException != null
                     && ((ex.InnerException is SqlException && ex.InnerException.Message.Contains("Arithmetic overflow"))
                     || (ex.InnerException is ArgumentException && ex.InnerException.Message.Contains(" is out of range"))))
                {
                    this.IsExpiredOrder = true;
                    lblFeedbackCart = "Total calculated greater than allowable by site. Please reduce the number of items in your cart. If you would like to make a high value purchase, please contact Healthy Chef Customer Service.";

                }
                else if (ex.Message.Contains("Tax Lookup could not determine the tax rate"))
                {
                    lblFeedbackCart= ex.Message + ". Please update your profile shipping address that uses this zip code; or contact Healthy Chef Customer Service.";

                }
                else
                    throw ex;
            }
        }

        public bool IsUserLoggedIn()
        {
            try
            {
                // in order to apply coupon code user must be logged in
                if (Helpers.LoggedUser == null)
                    return false;
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

        void GetCardInfo()
        {
            var _cardInfo = new CreditCardInfo();
            if (this.CurrentCardInfo == null && this.parentProfile != null)
            {
                this.CurrentCardInfo = new CreditCardInfo();
                hccUserProfilePaymentProfile payProfile = hccUserProfilePaymentProfile.GetBy(this.parentProfile.UserProfileID);
                if (payProfile != null)
                {
                    var _currentCardInfo = payProfile.ToCardInfo();
                    this.CurrentCardInfo.NameOnCard = _currentCardInfo.NameOnCard;

                    if (_currentCardInfo.CardNumber.Length >= 4)
                        this.CurrentCardInfo.CardNumber = "************" + _currentCardInfo.CardNumber.Substring(_currentCardInfo.CardNumber.Length - 4, 4);

                    this.CurrentCardInfo.CardType = _currentCardInfo.CardType.ToString();

                    this.CurrentCardInfo.Expires = _currentCardInfo.ExpMonth + "/" + _currentCardInfo.ExpYear;
                }
            }
        }

        string _getUserProfileString()
        {
            string _delDate = string.Empty;
            List<string> profileNameLists = new List<string>();

            if (this.profCart != null && this.profCart.Count != 0)
            {
                foreach (var CurrentProfileCart in this.profCart)
                {
                    CurrentProfileCart.CartItems.ForEach(delegate (hccCartItem item)
                    {
                        if (item.UserProfile != null && !profileNameLists.Contains(item.UserProfile.ProfileName))
                            profileNameLists.Add(item.UserProfile.ProfileName);
                    });
                }

                _delDate = this.profCart[0].CartItems[0].DeliveryDate.ToShortDateString();
            }
            string profileNames = string.Empty;
            profileNameLists.ForEach(a => profileNames += a + ", ");
            profileNames = profileNames.Trim().TrimEnd(new char[] { ',' });
            if (string.IsNullOrEmpty(profileNames))
            {
                return _delDate;
            }
            return profileNames + " : " + _delDate;
        }

        hccCoupon _getActiveCoupon()
        {
            hccCoupon coup = null;
            if (this.CurrentCart != null)
            {
                coup = hccCoupon.GetById(this.CurrentCart.CouponID ?? 0);
                if (coup != null)
                {
                    if ((!coup.StartDate.HasValue || coup.StartDate <= DateTime.Now) && (!coup.EndDate.HasValue || coup.EndDate >= DateTime.Now))
                    {
                        return coup;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return coup;
                }
            }
            else
            {
                return coup;
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



                    decimal PickupFee1 = 0;
                    if (IsPickup == "True")
                    {

                    }
                    else
                    {

                    }
                    if (IsPickup == "True")
                    {
                        // chkIsPickup.Checked = true;
                        DataTable BoxZones = hccshopin.BindBoxToShippingZoneFee(ZoneId);
                        if (BoxZones.Rows.Count > 0)
                        {
                            string PickupFee = BoxZones.Rows[0]["PickupFee"].ToString();
                            var value = string.IsNullOrWhiteSpace(PickupFee) ? "0" : PickupFee;
                            PickupFee1 = Convert.ToDecimal(value);
                            //  lblShipping.Text = PickupFee1.ToString("c");
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
                        // chkIsPickup.Checked = false;
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
                                        //  lblShipping.Text = FeeCost.ToString("c");
                                    }
                                    // lblShipping.Text = FeeCost.ToString("c");
                                }
                            }
                        }
                    }
                }
            }
            return Convert.ToDecimal(FeeCost);
        }


    }

    public class CreditCardInfo
    {
        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string Expires { get; set; }
    }

    public class ProfileCartModel
    {
        public ProfileCart profCart { get; set; }
        public string UserProfileString { get; set; }
    }

    public class CartCheckoutViewModel
    {
        public string HtmlString { get; set; }

        public CartCheckoutViewModel()
        {
            this.HtmlString = string.Empty;
            MembershipUser user = Helpers.LoggedUser;
            hccCart CurrentCart = new hccCart();
            if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
            {
                if (user == null)
                    CurrentCart = hccCart.GetCurrentCart();
                else
                    CurrentCart = hccCart.GetCurrentCart(user);
            }
            if (CurrentCart != null)
            {
                this.HtmlString = CurrentCart.ToHtml();
            }
        }
    }
}