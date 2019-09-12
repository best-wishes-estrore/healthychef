using HealthyChef.AuthNet;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.DAL.Extensions;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using HealthyChefCreationsMVC.CustomModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;

namespace HealthyChefCreationsMVC.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart
        [HttpGet]
        public ActionResult Index()
        {
            CartViewModel cartViewModel = new CartViewModel();
            if(cartViewModel.IsExpiredOrder)
            {
                if (cartViewModel.IsExpiredCartItems != null)
                {
                    foreach (var cartItem in cartViewModel.IsExpiredCartItems)
                    {
                        cartItem.Delete(((List<hccRecurringOrder>)Session["autorenew"]));
                        ViewBag.ExpiredOrderCartitems = "Item(s) removed from cart due to expiration of availability.";
                    }
                }
            }
            return View(cartViewModel);
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            CheckoutViewModel checkoutViewModel = new CheckoutViewModel();
            return View(checkoutViewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult CartCheckout()
        {
            // CartViewModel cartViewModel = new CartViewModel();
            CartCheckoutViewModel cartCheckoutViewModel = new CartCheckoutViewModel();
            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user != null)
                {
                    var parentprofile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);
                    if (parentprofile != null && parentprofile.ShippingAddressID != null && parentprofile.BillingAddressID != null)
                    {
                        var hccshippingaddress = hccAddress.GetById(Convert.ToInt32(parentprofile.ShippingAddressID));
                        var hccbillingaddress = hccAddress.GetById(Convert.ToInt32(parentprofile.BillingAddressID));
                        if (hccbillingaddress != null && hccshippingaddress != null)
                        {
                            if (hccbillingaddress.PostalCode != null && hccbillingaddress.PostalCode != "" && hccshippingaddress.PostalCode != "" && hccshippingaddress.PostalCode != null)
                            {
                                return View(cartCheckoutViewModel);
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "Please fill the billing info and shipping info";
                                return View(cartCheckoutViewModel);
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "Please fill the billing info and shipping info";
                            return View(cartCheckoutViewModel);
                        }
                        //return View(cartCheckoutViewModel);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Please fill the billing info and shipping info";
                        return View(cartCheckoutViewModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(cartCheckoutViewModel);
        }

        [HttpPost]
        public JsonResult ChangeSubProfile(string ordernumber, string profileid)
        {
            bool _quantityUpdated = false;
            string _message = "";

            try
            {
                List<hccCartItem> ChangedProfilecartItemsList = hccCartItem.GetByOrderNumber(ordernumber);
                foreach (var cartitem in ChangedProfilecartItemsList)
                {
                    cartitem.UserProfileID = Convert.ToInt32(profileid);
                    cartitem.Save();
                }
                _quantityUpdated = true;
                _message = "Your subprofile has been changed";
            }
            catch (Exception E)
            {
                _message = "Error in updating cart item quantity " + E.Message;
            }

            return Json(new { Success = _quantityUpdated, Message = _message }, JsonRequestBehavior.AllowGet);
        }
        //[]
        //public ActionResult CompletePuchase()
        //{

        //}

        [HttpGet]
        public ActionResult ClearCart()
        {
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
                Session["autorenew"] = null;
                CurrentCart.RemoveCartItems(null);
                CurrentCart.IsDefaultCouponRemoved = false;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult ConfirmCancel()
        {
            var CurrentCart = new hccCart();
            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    if (user == null)
                        CurrentCart = hccCart.GetCurrentCart();
                    else
                        CurrentCart = hccCart.GetCurrentCart(user);
                }
                if (CurrentCart != null)
                {
                    CurrentCart.StatusID = (int)Enums.CartStatus.Cancelled;
                    CurrentCart.Save();
                }
            }
            catch (Exception E)
            {

            }
            return Redirect("/order-now.aspx");
        }

        [HttpPost]
        public JsonResult UpdateCartItemQuantity(int cartItemId, int newQty)
        {
            bool _quantityUpdated = false;
            string _message = "";

            try
            {
                hccCartItem cartItem = hccCartItem.GetById(cartItemId);
                if (cartItem != null)
                {
                    if (cartItem.ItemTypeID == 1 && newQty < 2 && cartItem.Plan_IsAutoRenew == true)
                    {
                        var isquantityupdate = cartItem.AdjustQuantity(newQty);
                        if (isquantityupdate)
                        {
                            cartItem.Plan_IsAutoRenew = false;
                            cartItem.Quantity = newQty;
                            cartItem.Save();
                            _quantityUpdated = true;
                            _message = "Quantity updated successfully";
                        }
                    }
                    else if (newQty > 0)
                    {
                        List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(cartItem.CartID);

                        if (cartItem.AdjustQuantity(newQty))
                        {
                            _quantityUpdated = true;
                            _message = "Quantity updated successfully";
                        }

                    }
                    else
                    {
                        cartItem.Delete(null);
                        _quantityUpdated = true;
                        _message = "Cart Item deleted successfully";
                    }
                }
                else
                {
                    _message = "The Cart Item not found or deleted";
                }
            }
            catch (Exception E)
            {
                _message = "Error in updating cart item quantity " + E.Message;
            }

            return Json(new { Success = _quantityUpdated, Message = _message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DeleteCartItem(int cartItemId)
        {
            bool _cartItemDeleted = false;
            string _message = "";

            try
            {
                hccCartItem cartItem = hccCartItem.GetById(cartItemId);

                if (cartItem != null)
                {
                    cartItem.Delete(null);
                    _cartItemDeleted = true;
                    _message = "Cart Item Deleted Successfully";
                }
                else
                {
                    _message = "Could not find the cart Item";
                }
            }
            catch (Exception E)
            {
                _message = "Error in deleting cart item " + E.Message;
            }

            return Json(new { Success = _cartItemDeleted, Message = _message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteAutoRenewItem(int cartItemId)
        {
            bool _cartItemDeleted = false;
            string _message = "";

            try
            {
                hccCartItem hccCartItem = hccCartItem.GetById(cartItemId);
                hccCart hccCart = hccCart.GetById(hccCartItem.CartID);
                if (hccCartItem != null)
                {
                    if (hccCartItem.ItemTypeID == 2)
                    {
                        if (hccCartItem.Plan_IsAutoRenew == true)
                        {
                            hccCartItem.Plan_IsAutoRenew = false;
                            hccCartItem.DiscountAdjPrice = Convert.ToDecimal("0.00");
                            hccCartItem.DiscountPerEach = Convert.ToDecimal("0.00");
                            hccCartItem.Save();
                            if (hccCart != null)
                            {
                                hccRecurringOrder.DeleteByCartId(hccCart.CartID);
                            }
                            _cartItemDeleted = true;
                            _message = "Auto renew Item Deleted Successfully";
                        }
                        else
                        {
                            hccRecurringOrder hccrecurringOrder = new hccRecurringOrder
                            {
                                CartID = hccCartItem.CartID,
                                CartItemID = hccCartItem.CartItemID,
                                UserProfileID = hccCartItem.UserProfileID,
                                AspNetUserID = hccCart.AspNetUserID,
                                PurchaseNumber = hccCart.PurchaseNumber,
                                TotalAmount = Math.Round(Convert.ToDecimal(Convert.ToDouble(hccCartItem.ItemPrice) - Convert.ToDouble(hccCartItem.ItemPrice) * 0.05), 2)
                            };

                            hccCartItem.Plan_IsAutoRenew = true;
                            hccCartItem.DiscountAdjPrice = Convert.ToDecimal(hccCartItem.ItemPrice);
                            hccCartItem.DiscountPerEach = Convert.ToDecimal(Convert.ToDouble(hccCartItem.ItemPrice) * 0.05);
                            hccCartItem.Save();
                            hccrecurringOrder.Save();
                            _cartItemDeleted = true;
                            _message = "Auto renew Item Created Successfully";
                        }
                    }
                    else if (hccCartItem.ItemTypeID == 1)
                    {
                        if (hccCartItem.Plan_IsAutoRenew == true)
                        {
                            hccCartItem.Plan_IsAutoRenew = false;
                            hccCartItem.DiscountAdjPrice = Convert.ToDecimal("0.00");
                            hccCartItem.DiscountPerEach = Convert.ToDecimal("0.00");
                            hccCartItem.Save();
                            _cartItemDeleted = true;
                            _message = "Auto renew Item Deleted Successfully";
                        }
                        else
                        {
                            if (hccCartItem.Quantity >= 2)
                            {
                                hccCartItem.Plan_IsAutoRenew = true;
                                hccCartItem.DiscountAdjPrice = Convert.ToDecimal(hccCartItem.ItemPrice);
                                hccCartItem.DiscountPerEach = Convert.ToDecimal(Convert.ToDouble(hccCartItem.ItemPrice) * 0.10);
                                hccCartItem.Save();
                                _cartItemDeleted = true;
                                _message = "Auto renew Item Created Successfully";
                            }
                            else
                            {
                                _cartItemDeleted = false;
                                _message = "Family Style will deliver 2 or more portions in one serving dish.This makes heating meals for the whole family more convenient, reduces packaging, and allows you to save 10% on the price.";
                                return Json(new { Success = _cartItemDeleted, Message = _message }, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
                //var recurringorder = (List<hccRecurringOrder>)Session["autorenew"];
                ////hccRecurringOrder recurringorder = hccRecurringOrder.GetByCartItemId(cartItemId);
                ////hccCartItem hccCartItem = hccCartItem.GetById(cartItemId);
                //hccRecurringOrder IsExistingrecurringorder = null;
                //if (recurringorder != null && recurringorder.Count() > 0)
                //{
                //    IsExistingrecurringorder = recurringorder.Where(x => x.CartItemID == cartItemId).FirstOrDefault();
                //}

                //var isfamilystyle = hccCartItem.Plan_IsAutoRenew;
                //if (IsExistingrecurringorder != null || (isfamilystyle == true && hccCartItem.ItemTypeID == 1))
                //{
                //    if (recurringorder != null)
                //    {
                //        var Deleterecurringorder = recurringorder.Where(x => x.CartItemID == cartItemId).FirstOrDefault();
                //        if (Deleterecurringorder != null)
                //        {
                //            recurringorder.Remove(Deleterecurringorder);
                //        }
                //        //recurringorder.Delete();
                //    }
                //    if (hccCartItem != null)
                //    {
                //        hccCartItem.Plan_IsAutoRenew = false;
                //        hccCartItem.DiscountAdjPrice = Convert.ToDecimal("0.00");
                //        hccCartItem.DiscountPerEach = Convert.ToDecimal("0.00");
                //        hccCartItem.Save();
                //    }
                //    _cartItemDeleted = true;
                //    _message = "Auto renew Item Deleted Successfully";
                //}
                //else
                //{
                //    hccCart hccCart1 = hccCart.GetById(hccCartItem.CartID);
                //    if (hccCart1 != null)
                //    {
                //        List<hccRecurringOrder> lstRo = null;
                //        if (hccCartItem.ItemTypeID != 1)
                //        {
                //            if (Session["autorenew"] != null)
                //                lstRo = ((List<hccRecurringOrder>)Session["autorenew"]);
                //            else
                //                lstRo = new List<hccRecurringOrder>();

                //            hccRecurringOrder hccrecurringOrder = new hccRecurringOrder
                //            {
                //                CartID = hccCartItem.CartID,
                //                CartItemID = hccCartItem.CartItemID,
                //                UserProfileID = hccCartItem.UserProfileID,
                //                AspNetUserID = hccCart.AspNetUserID,
                //                PurchaseNumber = hccCart.PurchaseNumber,
                //                TotalAmount = Math.Round(Convert.ToDecimal(Convert.ToDouble(hccCartItem.ItemPrice) - Convert.ToDouble(hccCartItem.ItemPrice) * 0.05), 2)
                //            };
                //            //hccrecurringOrder.Save();
                //            lstRo.Add(hccrecurringOrder);

                //            Session["autorenew"] = lstRo;
                //        }
                //        if (hccCartItem != null)
                //        {
                //            hccCartItem.Plan_IsAutoRenew = true;
                //            hccCartItem.DiscountAdjPrice = Convert.ToDecimal(hccCartItem.ItemPrice);
                //            if (hccCartItem.ItemTypeID != 1)
                //            {
                //                hccCartItem.DiscountPerEach = Convert.ToDecimal(Convert.ToDouble(hccCartItem.ItemPrice) * 0.05);
                //            }
                //            else if (hccCartItem.ItemTypeID == 1)
                //            {
                //                if (hccCartItem.Quantity >= 2)
                //                {
                //                    hccCartItem.DiscountPerEach = Convert.ToDecimal(Convert.ToDouble(hccCartItem.ItemPrice) * 0.10);
                //                }
                //                else
                //                {
                //                    _cartItemDeleted = false;
                //                    _message = "Family Style will deliver 2 or more portions in one serving dish.This makes heating meals for the whole family more convenient, reduces packaging, and allows you to save 10% on the price.";
                //                    return Json(new { Success = _cartItemDeleted, Message = _message }, JsonRequestBehavior.AllowGet);
                //                }
                //            }
                //            hccCartItem.Save();
                //        }
                //        _cartItemDeleted = true;
                //        _message = "Auto renew Item Created Successfully";
                //    }

                //}
            }
            catch (Exception E)
            {
                _message = "Error in deleting cart item " + E.Message;
                if(E.Message.Contains("Value cannot be null."))
                {
                    _cartItemDeleted = true;
                }
            }

            return Json(new { Success = _cartItemDeleted, Message = _message }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AddCouponCode(string CouponCode)
        {
            bool _couponCodeAdded = false;
            string _message = "";
            var CurrentCart = new hccCart();

            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    if (user == null)
                        CurrentCart = hccCart.GetCurrentCart();
                    else
                        CurrentCart = hccCart.GetCurrentCart(user);
                }

                if (CurrentCart != null)
                {
                    hccCoupon coupon = hccCoupon.GetBy(CouponCode.Trim());

                    if (coupon != null && coupon.IsActive && (coupon.EndDate == null || coupon.EndDate > DateTime.Now) && (coupon.StartDate == null || coupon.StartDate <= DateTime.Now))
                    {
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
                                        || (couponCarts.Count == 1 && couponCarts[0].CartID != CurrentCart.CartID))
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
                            CurrentCart.IsEditCoupon = 1;
                            CurrentCart.Save();
                            _couponCodeAdded = true;
                            _message = "Coupon applied successfully";
                        }
                        else
                        {
                            _message = "The coupon code entered is for limited use only, and has already been used by this account.";
                        }
                    }
                    else
                    {
                        _message = "The coupon code entered does not exist or is not active.";
                    }
                }
                else
                {
                    _message = "Your cart is empty, please Login and try";
                }
            }
            catch (Exception E)
            {
                _message = "Error in Adding coupon code " + E.Message;
            }

            return Json(new { Success = _couponCodeAdded, Message = _message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RemoveCouponCode()
        {
            bool _couponRemoved = false;
            string _message = "";
            var CurrentCart = new hccCart();

            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    if (user == null)
                        CurrentCart = hccCart.GetCurrentCart();
                    else
                        CurrentCart = hccCart.GetCurrentCart(user);
                }
                if (CurrentCart != null)
                {
                    int? defaultCouponId = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value).DefaultCouponId;
                    int? currentCouponId = CurrentCart.CouponID;
                    CurrentCart.IsDefaultCouponRemoved = currentCouponId == defaultCouponId;
                    CurrentCart.CouponID = currentCouponId != defaultCouponId ? defaultCouponId : null;
                    CurrentCart.Save();

                    _couponRemoved = true;
                    _message = "Coupon removed successfully";
                }
                else
                {
                    _message = "Your cart is empty, please Login and try";
                }

            }
            catch (Exception E)
            {
                _message = "Error in removing coupon code " + E.Message;
            }

            return Json(new { Success = _couponRemoved, Message = _message }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult AddRedeemGift(string RedeemGiftCode)
        {
            bool _redeemGiftCodeAdded = false;
            string _message = "";
            var CurrentCart = new hccCart();

            try
            {
                hccCartItem giftItem = hccCartItem.GetGiftBy(RedeemGiftCode);
                ImportedGiftCert cert = ImportedGiftCert.GetBy(RedeemGiftCode);

                bool isimport = false;
                bool updateLedger = false;

                if (giftItem == null)
                {
                    if (cert != null)
                    {
                        if (cert.is_used == "Y")
                        {
                            _message = "The gift certificate code entered has already been redeemed.";
                        }
                        else
                        {
                            isimport = true;
                            updateLedger = true;
                        }
                    }
                    else
                    {
                        _message = "The gift certificate code entered is not recognized.";
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
                            _message = "The gift certificate code entered has already been redeemed.";
                        }
                        else
                        {
                            updateLedger = true;
                        }
                    }
                    else
                    {
                        _message = "The gift certificate code entered has not yet been purchased.";
                    }
                }

                if (updateLedger)
                {
                    MembershipUser user = Helpers.LoggedUser;
                    if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                    {
                        if (user == null)
                            CurrentCart = hccCart.GetCurrentCart();
                        else
                            CurrentCart = hccCart.GetCurrentCart(user);
                    }

                    hccUserProfile profile = new hccUserProfile();
                    if (CurrentCart != null)
                    {
                        profile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                    }

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

                            _redeemGiftCodeAdded = true;
                            _message = "The gift certificate " + cert.code +
                                " has been redeemed, and the amount of " + Math.Round(Convert.ToDecimal(cert.amount), 2) + " has been credited to your account.";
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

                            _redeemGiftCodeAdded = true;
                            _message = "The gift certificate " + giftItem.Gift_RedeemCode +
                                " has been redeemed, and the amount of " + giftItem.ItemPrice.ToString("c") + " has been credited to your account.";
                        }
                    }
                }
            }
            catch (Exception E)
            {
                _message = "Error in Redeemed gift certificate " + E.Message;
            }

            return Json(new { Success = _redeemGiftCodeAdded, Message = _message }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CalculateShipping(string ZipCode)
        {
            bool _shippingCalculated = false;
            string _message = "";
            var CurrentCart = new hccCart();
            string Subtotal = "$0";

            bool chkIsPickup = false;
            string ShipDelType = string.Empty;
            string GrandTotal = string.Empty;
            string PaymentDue = string.Empty;
            string Shipping = string.Empty;

            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    if (user == null)
                        CurrentCart = hccCart.GetCurrentCart();
                    else
                        CurrentCart = hccCart.GetCurrentCart(user);
                }

                if (CurrentCart != null)
                {
                    Subtotal = CurrentCart.TotalAmount.ToString("c");
                }

                if (ZipCode.Contains("-"))
                {
                    ZipCode = ZipCode.Split('-')[0];
                }

                Session["ZipCodewithoutLogin"] = ZipCode;
                hccShippingZone hccshopin = new hccShippingZone();
                DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());  //ZoneId from On Select Zone Dropdown
                    DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                    if (ds1.Rows.Count > 0)
                    {
                        string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();
                        decimal MINFee = Convert.ToDecimal(ds1.Rows[0]["MinFee"]);
                        decimal MAXFee = Convert.ToDecimal(ds1.Rows[0]["MaxFee"]);
                        decimal multiplier = Convert.ToDecimal(ds1.Rows[0]["Multiplier"]);
                        string Shiptype = ds1.Rows[0]["TypeName"].ToString();//lblShipDelType
                        ShipDelType = Shiptype;
                        if (IsPickup == "True")
                        {
                            chkIsPickup = true;
                        }
                        else
                        {
                            chkIsPickup = false;
                        }
                        if (IsPickup == "True")
                        {
                            chkIsPickup = true;
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
                                Shipping = PickupFee1.ToString("c");

                                var Subval = Subtotal.Replace("$", "");
                                var Shipval = PickupFee1;
                                decimal totalval = Convert.ToDecimal(Subval) + Convert.ToDecimal(Shipval);
                                GrandTotal = totalval.ToString("c");
                                PaymentDue = totalval.ToString("c");
                            }

                            return Json(new
                            {
                                Success = _shippingCalculated,
                                Message = _message,
                                chkIsPickup = chkIsPickup,
                                ShipDelType = ShipDelType,
                                GrandTotal = GrandTotal,
                                PaymentDue = PaymentDue,
                                Shipping = Shipping
                            }, JsonRequestBehavior.AllowGet);

                        }
                        else
                        {
                            chkIsPickup = false;
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
                                            Shipping = FeeCost.ToString("c");
                                        }
                                        Shipping = FeeCost.ToString("c");
                                        var Subval1 = Subtotal.Replace("$", "");
                                        var Shipval1 = FeeCost;
                                        decimal totalval1 = Convert.ToDecimal(Subval1) + Convert.ToDecimal(Shipval1);
                                        GrandTotal = totalval1.ToString("c");
                                        PaymentDue = totalval1.ToString("c");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    _message = "Zipcode entered is invalid";
                }
            }
            catch (Exception E)
            {
                _message = "Error in calculating shipping " + E.Message;
            }

            return Json(new
            {
                Success = _shippingCalculated,
                Message = _message,
                chkIsPickup = chkIsPickup,
                ShipDelType = ShipDelType,
                GrandTotal = GrandTotal,
                PaymentDue = PaymentDue,
                Shipping = Shipping
            }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ThankYou(int purchaseNum)
        {
            hccCart CurrentCart = new hccCart();
            string CurrentCity = string.Empty;
            string CurrentState = string.Empty;
            string CurrentCountry = string.Empty;
            ThankYouPageModel thankYouPageModel = new ThankYouPageModel();
            try
            {
                if (purchaseNum != 0)
                {
                    CurrentCart = hccCart.GetBy(purchaseNum);
                }
                if (CurrentCart != null && Helpers.LoggedUser != null
                    && (CurrentCart.AspNetUserID == (Guid)Helpers.LoggedUser.ProviderUserKey
                            || Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Administrators")))
                {


                    hccUserProfile billProf = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                    if (billProf != null)
                    {
                        hccAddress billAddr = null;
                        if (billProf.BillingAddressID.HasValue)
                        {
                            billAddr = hccAddress.GetById(billProf.BillingAddressID.Value);
                        }

                        if (billAddr != null)
                        {
                            CurrentCity = billAddr.City;
                            CurrentState = billAddr.State;
                            CurrentCountry = billAddr.Country;
                            //CurrentUserName = billAddr.FirstName;
                            //CurrentUserEmail = billProf.aspnet_Membership.Email;
                        }
                        else
                        {
                            CurrentCity = string.Empty;
                            CurrentState = string.Empty;
                            CurrentCountry = string.Empty;
                            //CurrentUserName = string.Empty;
                            //CurrentUserEmail = string.Empty;
                        }
                    }
                    else
                    {
                        CurrentCity = string.Empty;
                        CurrentState = string.Empty;
                        CurrentCountry = string.Empty;
                        //CurrentUserName = string.Empty;
                        //CurrentUserEmail = string.Empty;
                    }
                    //string TrackAmount = "'" + CurrentCart.TotalAmount.ToString("f2") + "'";
                    //Session["trackAmount"] = TrackAmount;
                    //track_id.Attributes["src"] = ResolveUrl("https://shareasale.com/sale.cfm?amount=TrackAmount&tracking=user@shareasale.com&transtype=lead&merchantID=11");

                    StringBuilder cartItem_sb = new StringBuilder();
                    List<hccCartItem> hcccartItems = new List<hccCartItem>();
                    hcccartItems = hccCartItem.GeCartItemsByPurchaseNumber(purchaseNum);
                    var lastItem = hcccartItems.Last();
                    foreach (var items in hcccartItems)
                    {
                        cartItem_sb.AppendLine("'sku': '" + items.CartItemID + "',");
                        cartItem_sb.AppendLine("'name': '" + items.ItemName + "',");
                        if (items.ItemTypeID == 1)
                        {
                            cartItem_sb.AppendLine("'category': 'A La Carte',");
                        }
                        else if (items.ItemTypeID == 2)
                        {
                            cartItem_sb.AppendLine("'category': 'Program',");
                        }
                        else
                        {
                            cartItem_sb.AppendLine("'category': 'Others',");
                        }
                        cartItem_sb.AppendLine("'price':" + Math.Round((items.ItemPrice - items.DiscountPerEach), 2) + ",");
                        cartItem_sb.AppendLine("'quantity':" + items.Quantity);
                        if (!items.Equals(lastItem))
                        {
                            cartItem_sb.AppendLine("},{");
                        }
                    }

                    if(billProf != null)
                    {
                        hccAddress billAddr12 = hccAddress.GetById(billProf.ShippingAddressID.Value);
                        List<hccCartItem> cartItem = hccCartItem.GeCartItemsByPurchaseNumber(purchaseNum);
                        if (billAddr12.State == "FL")
                        {
                            if (cartItem.FirstOrDefault().TaxRateAssigned > 0)
                            {
                                var taxRatePercent = cartItem.FirstOrDefault().TaxRateAssigned / 100;
                                CurrentCart.TaxAmount = Math.Round(Convert.ToDecimal(taxRatePercent * CurrentCart.TaxableAmount), 2);
                                CurrentCart.Save();
                            }
                        }
                    }
                   
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("window.dataLayer = window.dataLayer || [];");
                    sb.AppendLine("dataLayer.push({");
                    sb.AppendLine("'event':'transactionComplete',");
                    sb.AppendLine("'transactionId': '" + purchaseNum + "',");
                    sb.AppendLine("'transactionAffiliation': 'HealthyChef Creations',");
                    sb.AppendLine("'transactionTotal': " + Math.Round(CurrentCart.TotalAmount, 2) + ",");
                    sb.AppendLine("'transactionTax': " + Math.Round(CurrentCart.TaxAmount, 2) + ",");
                    sb.AppendLine("'transactionShipping': " + Math.Round(CurrentCart.ShippingAmount, 2) + ",");
                    sb.AppendLine("'transactionProducts': [{");
                    sb.AppendLine(cartItem_sb.ToString());
                    sb.AppendLine("}]");
                    sb.AppendLine(" }); ");
                    sb.AppendLine("(function() {");
                    sb.AppendLine("var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;");
                    sb.AppendLine("ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';");
                    sb.AppendLine("var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);");
                    sb.AppendLine("})();");

                    var listofcarts = hccCart.GetBy(CurrentCart.AspNetUserID.Value).Where(x => x.StatusID == (int)Enums.CartStatus.Paid || x.StatusID == (int)Enums.CartStatus.Cancelled).ToList();
                    if (listofcarts.Count > 1)
                    {
                        // ClientScript.RegisterStartupScript(this, this.GetType(), "GoogleTrack", sb.ToString(), true);

                        //if (!ClientScript.IsStartupScriptRegistered("GoogleTrack"))
                        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "GoogleTrack", sb.ToString(), true);
                        //Response.Redirect(string.Format("~/topic/thankyou.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}&Isnewcustomer={7}",
                        //     CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                        //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City,
                        //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State,
                        //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country, 0), true);
                        thankYouPageModel.PurchaseNumber = CurrentCart.PurchaseNumber;
                        thankYouPageModel.TotalAmount = CurrentCart.TotalAmount;
                        thankYouPageModel.TaxableAmount = CurrentCart.TaxableAmount;
                        thankYouPageModel.ShippingAmount = CurrentCart.ShippingAmount;
                        thankYouPageModel.GoogleTrackScript = sb.ToString();
                        thankYouPageModel.Email = Helpers.LoggedUser.Email;
                        var currentDate = hccProductionCalendar.GetNext4Calendars();
                        if(currentDate != null)
                        {
                            thankYouPageModel.DeliveryDate = currentDate[0].DeliveryDate;
                        }
                        thankYouPageModel.UserName = hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).FirstName == null ? "" : hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).FirstName;

                        if (CurrentCart.OwnerProfile.BillingAddressID != null)
                        {
                            thankYouPageModel.City = hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City == null ? "Ntng" : hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City;
                            thankYouPageModel.State = hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State == null ? "Ntng" : hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State;
                            thankYouPageModel.Country = hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country == null ? "Ntng" : hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country;
                        }
                        else
                        {
                            thankYouPageModel.City = "Ntng";
                            thankYouPageModel.State = "Ntng";
                            thankYouPageModel.Country = "Ntng";
                        }
                        thankYouPageModel.Isnewcustomer = 0;
                    }
                    else
                    {
                        //if (!ClientScript.IsStartupScriptRegistered("GoogleTrack"))
                        //    Page.ClientScript.RegisterStartupScript(this.GetType(), "GoogleTrack", sb.ToString(), true);
                        //Response.Redirect(string.Format("~/topic/thankyou.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}&Isnewcustomer={7}",
                        //     CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                        //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City,
                        //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State,
                        //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country, 1), true);
                        thankYouPageModel.PurchaseNumber = CurrentCart.PurchaseNumber;
                        thankYouPageModel.TotalAmount = CurrentCart.TotalAmount;
                        thankYouPageModel.TaxableAmount = CurrentCart.TaxableAmount;
                        thankYouPageModel.ShippingAmount = CurrentCart.ShippingAmount;
                        thankYouPageModel.GoogleTrackScript = sb.ToString();
                        if (CurrentCart.OwnerProfile.BillingAddressID != null)
                        {
                            thankYouPageModel.City = hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City == null ? "Ntng" : hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City;
                            thankYouPageModel.State = hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State == null ? "Ntng" : hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State;
                            thankYouPageModel.Country = hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country == null ? "Ntng" : hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country;
                        }
                        else
                        {
                            thankYouPageModel.City = "Ntng";
                            thankYouPageModel.State = "Ntng";
                            thankYouPageModel.Country = "Ntng";
                        }
                        thankYouPageModel.Isnewcustomer = 1;
                        if (Membership.GetUser() != null)
                        {
                            var customeremailid = Membership.GetUser().Email;
                            if (!customeremailid.Contains("@facebook.com"))
                            {
                                AddnewCustomerToMailChimp(customeremailid);
                            }
                        }
                    }
                }
                else
                {
                    var result = "You are not authorized to view this information.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View(thankYouPageModel);
        }
        [HttpPost]
        public JsonResult CompletePurchase(bool MarketingOptIn = false)
        {
            bool _isPurchaseCompleted = false;
            bool isLoggedIn = true;
            string _message = "";
            var CurrentCart = new hccCart();
            bool AuthNetSuccessful = false;
            bool IsForPublic = true;
            int CurrentCartId = 0;

            try
            {
                MembershipUser user = Helpers.LoggedUser;
                if (user == null || (user != null && Roles.IsUserInRole(user.UserName, "Customer")))
                {
                    if (user == null)
                        CurrentCart = hccCart.GetCurrentCart();
                    else
                        CurrentCart = hccCart.GetCurrentCart(user);
                }

                if (CurrentCart != null)
                {
                    CurrentCartId = CurrentCart.CartID;
                    hccUserProfile profile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                    hccAddress billAddr = null;

                    if (CurrentCart.StatusID == (int)Enums.CartStatus.Unfinalized)
                    {
                        if (profile != null)
                        {
                            profile.CanyonRanchCustomer = MarketingOptIn;
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
                                            _isPurchaseCompleted = true;
                                            isAuthNet = true;
                                            AuthNetSuccessful = true;
                                        }
                                        else
                                        {
                                            _message = rsp.Message;
                                        }
                                        CurrentCart.Save();
                                    }
                                    else
                                    {
                                        _message = "No payment profile found.";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    _message = ex.Message;
                                    if (ex is InvalidOperationException)
                                    {
                                        ExceptionLogging.SendErrorToText(ex.InnerException);
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

                                            _message = ex.Message;
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
                                _isPurchaseCompleted = true;
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
                                        _isPurchaseCompleted = true;
                                    }
                                    // Temporarily commented out to create a deployment for Recurring order fixes 11-04-20013
                                    //using (var hccEntity = new healthychefEntities())
                                    //{
                                    //    hccEntity.hcc_IsCanyonRanchCustomer(CurrentCart.CartID, CurrentCart.OwnerProfile.UserProfileID);
                                    //}
                                    // Temporarily commented out to create a deployment for Recurring order fixes 11-04-20013
                                    try
                                    {
                                        HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                                        ec.SendMail_OrderConfirmationMerchant(profile.FirstName + " " + profile.LastName, CurrentCart.ToHtml(), CurrentCartId);
                                        ec.SendMail_OrderConfirmationCustomer(profile.ASPUser.Email, profile.FirstName + " " + profile.LastName, CurrentCart.ToHtml());
                                    }
                                    catch (Exception ex)
                                    { BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Send Mail Failed", this, ex); }//throw; }

                                    if (IsForPublic)
                                    {
                                        //Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                                        //    CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                                        //    billAddr.City, billAddr.State, billAddr.Country), false);
                                        return Json(new { Success = _isPurchaseCompleted, IsLoggedIn = isLoggedIn, Message = _message, data = CurrentCart.PurchaseNumber }, JsonRequestBehavior.AllowGet);
                                    }
                                    else
                                    {
                                        CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                                        CurrentCartId = CurrentCart.CartID;
                                    }
                                }
                            }
                            else
                            {
                                BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Duplicate transaction attempted: " + CurrentCart.PurchaseNumber.ToString(), this, new Exception("Duplicate transaction attempted by:" + Helpers.LoggedUser.UserName));
                            }
                        }
                        else
                        {
                            isLoggedIn = false;
                        }
                    }
                    else
                    {
                        if (IsForPublic)
                        {
                            //Response.Redirect("~/cart/order-confirmation.aspx?cid=" + CurrentCartId.ToString(), false);
                            //Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                            //                CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                            //                billAddr.City, billAddr.State, billAddr.Country), false);
                            return Json(new { Success = _isPurchaseCompleted, IsLoggedIn = isLoggedIn, Message = _message, data = CurrentCart.PurchaseNumber }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                            CurrentCartId = CurrentCart.CartID;

                        }
                    }
                }
            }
            catch (Exception E)
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

                    //Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                    //                    CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                    //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).City,
                    //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).State,
                    //                    hccAddress.GetById(CurrentCart.OwnerProfile.BillingAddressID.Value).Country), false);

                }
                _message = "Error in completing purchase " + E.Message;
                ExceptionLogging.SendErrorToText(E.InnerException);
                return Json(new { Success = _isPurchaseCompleted, IsLoggedIn = isLoggedIn, Message = _message, data = CurrentCart.PurchaseNumber }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Success = _isPurchaseCompleted, IsLoggedIn = isLoggedIn, Message = _message, data = CurrentCart.PurchaseNumber }, JsonRequestBehavior.AllowGet);
        }

        private void AddnewCustomerToMailChimp(string email)
        {
            var apiKey = "dce72633770f294645268fa9bc4ba4d2-us16"; // your API key here (no, this is not a real one!)
            var listId = "5ac7aabd86"; // your list ID here
            var subscribeRequest = new
            {
                apikey = apiKey,
                id = listId,
                email = new
                {
                    email = email
                },
                double_optin = false,
            };
            var requestJson = JsonConvert.SerializeObject(subscribeRequest);
            var responseString = CallMailChimpApi("lists/subscribe.json", requestJson);
            dynamic responseObject = JsonConvert.DeserializeObject(responseString);
            if ((responseObject.email != null) && (responseObject.euid != null))
            {
                // successful!
            }
            else
            {
                string name = responseObject.name;
                if (name == "List_AlreadySubscribed")
                {
                    Trace.TraceInformation("Mailchimp already subscribed");
                }
                else
                {
                    Trace.TraceError("Mailchimp subscribe error {0}", responseObject.error);
                }
            }
        }
        private static string CallMailChimpApi(string method, string requestJson)
        {
            var endpoint = String.Format("https://{0}.api.mailchimp.com/2" +
                "" +
                "" +
                ".0/{1}", "us16", method);
            var wc = new WebClient();
            try
            {
                return wc.UploadString(endpoint, requestJson);
            }
            catch (WebException we)
            {
                using (var sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}