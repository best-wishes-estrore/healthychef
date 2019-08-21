using HealthyChef.Common;
using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class CheckoutViewModel
    {
        public List<ProfileCart> profCart { get; set; }
        public List<hccCartItem> cartItems { get; set; }
        public hccCart CurrentCart { get; set; }
        public hccUserProfile parentProfile { get; set; }

        public string ShippingMethod { get; set; }
        public string ShippingDeliveryType { get; set; }
        public string BillingAddress { get; set; }

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
                if (this.CurrentCart != null)
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

        public CreditCardInfo CurrentCardInfo { get; set; }



        public CheckoutViewModel()
        {
            MembershipUser user = Helpers.LoggedUser;
            if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
            {
                if (user == null)
                    this.CurrentCart = hccCart.GetCurrentCart();
                else
                    this.CurrentCart = hccCart.GetCurrentCart(user);
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
                hccAddress addr = hccAddress.GetById(parentProfile.ShippingAddressID.Value);
                if (addr != null)
                {
                    this.BillingAddress = addr.ToString();
                }
            }

            this.profCart = new List<ProfileCart>();
            //populate profcarts
            if (this.cartItems != null)
            {
                if (cartItems.Count > 0)
                {
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
                            CurrentCart.CalculateTotals(this.profCart);
                        }

                    }
                } 
            }

        }

        void GetCardInfo()
        {
            var _cardInfo = new CreditCardInfo();
            if (this.CurrentCardInfo == null && this.parentProfile != null)
            {
                this.CurrentCardInfo = new CreditCardInfo();
                hccUserProfilePaymentProfile payProfile = hccUserProfilePaymentProfile.GetBy(this.parentProfile.UserProfileID);
                var _currentCardInfo = payProfile.ToCardInfo();
                this.CurrentCardInfo.NameOnCard = _currentCardInfo.NameOnCard;

                if (_currentCardInfo.CardNumber.Length >= 4)
                    this.CurrentCardInfo.CardNumber = "************" + _currentCardInfo.CardNumber.Substring(_currentCardInfo.CardNumber.Length - 4, 4);

                this.CurrentCardInfo.CardType = _currentCardInfo.CardType.ToString();

                this.CurrentCardInfo.Expires = _currentCardInfo.ExpMonth + "/" + _currentCardInfo.ExpYear;
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

    }
}