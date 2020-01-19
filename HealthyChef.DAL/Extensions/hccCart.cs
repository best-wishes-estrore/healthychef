using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCart
    {
        //static healthychefEntities defcon
        //{
        //    get { return healthychefEntities.Default; }
        //}

        //int currencyDecimals = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits;

        public bool IsDefaultCouponRemoved { get; set; }

        /// <summary>
        /// Saves shopping cart header to the database.  If there is not a cart for this UserID it will be created. 
        /// </summary>
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    EntityKey key = cont.CreateEntityKey("hccCarts", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCarts", this);
                    }
                    else
                    {
                        cont.hccCarts.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the hccCart object and related hccCartItem entries.
        /// NOTE: Delete cascades to hccCartItems via Foreign Key Update/Delete Specs 
        /// </summary>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns void.</returns>
        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    EntityKey key = cont.CreateEntityKey("hccCarts", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        cont.hccCarts.DeleteObject((hccCart)originalItem);
                        cont.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Enums.CartStatus Status
        {
            get
            {
                return (Enums.CartStatus)this.StatusID;
            }
        }

        public hccUserProfile OwnerProfile
        {
            get
            {
                hccUserProfile prof = null;

                if (this.AspNetUserID.HasValue)
                    prof = hccUserProfile.GetParentProfileBy(this.AspNetUserID.Value);

                return prof;
            }
        }

        public hccUserProfilePaymentProfile PaymentProfile
        {
            get
            {
                if (this.PaymentProfileID.HasValue)
                    return hccUserProfilePaymentProfile.GetById(this.PaymentProfileID.Value);
                else
                    return null;
            }
        }

        public void RemoveCartItems(List<hccRecurringOrder> recurringItemList)
        {
            try
            {
                List<hccCartItem> cartItems = hccCartItem.GetBy(this.CartID);

                if (recurringItemList != null)
                    cartItems.ForEach(ci => ci.Delete(recurringItemList));
                else
                {
                    cartItems.ForEach(ci => ci.Delete(null));
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<hccUserProfile> GetProfilesUsedInOrder()
        {
            try
            {
                List<int> profileIds = new List<int>();
                List<hccUserProfile> profiles = new List<hccUserProfile>();
                List<hccCartItem> cartItems = hccCartItem.GetBy(this.CartID);

                cartItems.ForEach(delegate (hccCartItem item)
                {
                    if (item.UserProfileID > 0)
                        if (!profileIds.Contains(item.UserProfileID))
                            profileIds.Add(item.UserProfileID);
                });

                profileIds.ForEach(a => profiles.Add(hccUserProfile.GetById(a)));

                return profiles;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the hccCart object with the specified Id.  
        /// If cart does not exists return null
        /// </summary>
        /// <param name="id">The id to use in order to locate the Cart.</param>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns the Cart that matches the id.</returns>
        public static hccCart GetById(int cartId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCarts
                        .SingleOrDefault(c => c.CartID == cartId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCart> GetBy(Guid aspNetUserId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCarts
                        .Where(c => c.AspNetUserID == aspNetUserId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCart GetBy(int purchaseNumber)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCarts
                        .SingleOrDefault(c => c.PurchaseNumber == purchaseNumber);
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCart GetBy(string orderNumber)
        {
            try
            {
                int purchaseNumber = int.Parse(orderNumber.Split('-')[0]);

                return GetBy(purchaseNumber);
            }
            catch
            {
                throw;
            }
        }

        public static hccCart GetCurrentCart()
        {
            try
            {
                MembershipUser user = Helpers.LoggedUser;
                return GetCurrentCart(user);
            }
            catch
            {
                throw;
            }
        }

        public static hccCart GetCurrentCart(MembershipUser user)
        {
            try
            {
                List<hccCart> openCarts = new List<hccCart>();
                hccCart returnCart = null;
                List<hccCart> cancelCarts = new List<hccCart>();
                string anonId = string.Empty;

                if (user == null)
                {
                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - User null.", "hccCart.GetCurrentCart");

                    try
                    {
                        if (HttpContext.Current != null
                            && HttpContext.Current.Request != null
                            && !string.IsNullOrWhiteSpace(HttpContext.Current.Request.AnonymousID))
                        {
                            anonId = HttpContext.Current.Request.AnonymousID;
                            openCarts = GetUnfinalizedByIP(anonId, false);
                        }
                        //else
                        //{
                        //    if (HttpContext.Current == null)
                        //        BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - HttpContext.Current == null.", "hccCart.GetCurrentCart");

                        //    if (HttpContext.Current.Request == null)
                        //        BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - HttpContext.Current.Request == null.", "hccCart.GetCurrentCart");

                        //    if (string.IsNullOrWhiteSpace(HttpContext.Current.Request.AnonymousID))
                        //        BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - HttpContext.Current.Request.AnonymousID == null/empty.", "hccCart.GetCurrentCart");
                        //}
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("hccCart.GetCurrentCart - Error getting anonymous cart.", ex);
                    }

                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - " + openCarts.Count + " for user " + HttpContext.Current.Request.AnonymousID + ".", "hccCart.GetCurrentCart");
                }
                else
                {
                    try
                    {
                        //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - User not null.", "hccCart.GetCurrentCart");
                        openCarts = GetUnfinalizedByUser((Guid)user.ProviderUserKey);
                        //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - " + openCarts.Count + " for user " + user.UserName + ".", "hccCart.GetCurrentCart");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("hccCart.GetCurrentCart - Error getting open carts for user " + user.UserName + ".", ex);
                    }
                }

                if (openCarts.Count > 0)
                {
                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Open carts count == " + openCarts.Count().ToString() + ".", "hccCart.GetCurrentCart");
                    // Multiple incomplete orders may result due to unexpected browser closures, etc
                    // Set return order as the latest started, and then update any others as incomplete. - RC
                    returnCart = openCarts.FirstOrDefault();
                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart -  Return cart #" + returnCart.PurchaseNumber.ToString() + ".", "hccCart.GetCurrentCart");

                    cancelCarts = openCarts.Where(a => a.CartID != returnCart.CartID).ToList();

                    if (cancelCarts.Count > 0)
                        cancelCarts.ForEach(delegate (hccCart cancelCart)
                        {
                            try
                            {
                                //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Attempting cart cancel for cart #" + cancelCart.PurchaseNumber.ToString() + ".", "hccCart.GetCurrentCart");
                                cancelCart.StatusID = (int)Enums.CartStatus.Cancelled;
                                cancelCart.Save();
                                //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Cart cancelled for cart #" + cancelCart.PurchaseNumber.ToString() + ".", "hccCart.GetCurrentCart");

                            }
                            catch { /* do nothing, empty set */}
                        });
                }

                if (returnCart == null)
                {
                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Return cart is null. Creat new cart.", "hccCart.GetCurrentCart");

                    returnCart = new hccCart
                    {
                        StatusID = (int)Enums.CartStatus.Unfinalized,
                        CreatedDate = DateTime.Now,
                        AspNetUserID = (user == null ? Guid.Empty : (Guid)user.ProviderUserKey),
                        CreatedBy = (user == null ? Guid.Empty : (Guid)user.ProviderUserKey),
                        AnonGiftRedeemCredit = 0.00m,
                        DiscretionaryTaxAmount = 0.00m,
                        ModifiedBy = (user == null ? Guid.Empty : (Guid)user.ProviderUserKey),
                        ModifiedDate = DateTime.Now
                    };

                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Return cart creating.", "hccCart.GetCurrentCart");

                    if (user == null)
                    {
                        returnCart.AnonymousID = anonId;
                    }
                    else
                    {
                        hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                        if (parentProfile != null && parentProfile.DefaultCouponId.HasValue)
                        {
                            returnCart.CouponID = parentProfile.DefaultCouponId;
                            //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Return cart default coupon added.", "hccCart.GetCurrentCart");
                        }
                    }

                    //bool successSaveNewCart = false;
                    //int i = 0;

                    //while (successSaveNewCart == false)
                    //{
                    try // try to save new cart, except when purchase number has been claimed by another process
                    {
                        returnCart.PurchaseNumber = hccCart.GetNextPurchaseNumber();
                        returnCart.Save();
                        //successSaveNewCart = true;
                        //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Return cart #" + returnCart.PurchaseNumber + " saved.", "hccCart.GetCurrentCart");
                    }
                    catch (Exception ex)
                    {
                        //i++;
                        //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(ex.Message, "hccCart.GetCurrentCart", ex);

                        //// if the purchase number has already been used, try again with the next number
                        //if (ex.InnerException != null && ex.InnerException.Message.Contains("UNIQUE KEY constraint 'IX_hccCarts'"))
                        //{
                        //    HttpContext.Current.Server.ClearError();
                        //}
                        throw ex;
                    }
                    //}

                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Return cart #" + returnCart.PurchaseNumber + " create successfully.", "hccCart.GetCurrentCart");
                }
                else
                {
                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Return cart #" + returnCart.PurchaseNumber + " acquired successfully.", "hccCart.GetCurrentCart");
                    //BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("hccCart.GetCurrentCart - Return cart is not null.", "hccCart.GetCurrentCart");
                }

                return returnCart;
            }
            catch (Exception ex)
            {
                BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(ex.Message, "hccCart.GetCurrentCart", ex);
                return null;
            }
        }

        protected static List<hccCart> GetUnfinalizedByUser(Guid aspNetUserId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var orders = cont.hccCarts
                           .Where(c => c.AspNetUserID == aspNetUserId
                               && c.StatusID == (int)Enums.CartStatus.Unfinalized)
                           .OrderBy(d => d.CreatedDate).ToList();

                    return orders;
                }
            }
            catch
            {
                throw;
            }
        }

        protected static List<hccCart> GetUnfinalizedByIP(string anonId, bool allowAspUsers)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var orders = cont.hccCarts
                           .Where(c => c.AnonymousID == anonId
                               && c.StatusID == (int)Enums.CartStatus.Unfinalized)
                           .OrderBy(d => d.CreatedDate).ToList();

                    if (!allowAspUsers)
                        orders = orders.Where(a => a.AspNetUserID == Guid.Empty).ToList();

                    return orders;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<hccCart> GetBy(Enums.CartStatus cartStatus)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var orders = cont.hccCarts
                           .Where(c => c.StatusID == (int)cartStatus)
                           .OrderByDescending(d => d.PurchaseNumber)
                           .ToList();
                    return orders;
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hcc_SalesReportCarts_Result> GetSalesReportCarts(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var carts = cont.hcc_SalesReportCarts(startDate, endDate).ToList();
                    return carts;
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public static List<hccCart> GetCompleted(Guid aspNetUserId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var orders = cont.hccCarts
                           .Where(c => c.AspNetUserID == aspNetUserId
                               && (c.StatusID == (int)Enums.CartStatus.Paid || c.StatusID == (int)Enums.CartStatus.Fulfilled
                                || (c.StatusID == (int)Enums.CartStatus.Cancelled && c.PurchaseDate.HasValue)
                               ))
                           .OrderByDescending(d => d.PurchaseDate)
                           .ToList();

                    return orders;
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCart> GetCompletedWithCoupon(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var orders = cont.hccCarts
                            .Where(c => c.CouponID.HasValue
                                && (c.PurchaseDate >= startDate && c.PurchaseDate <= endDate)
                                && (c.StatusID == (int)Enums.CartStatus.Paid || c.StatusID == (int)Enums.CartStatus.Fulfilled))
                           .OrderBy(d => d.PurchaseDate).ToList();

                    return orders;
                }
            }
            catch
            {
                throw;
            }
        }

        public static int GetNextPurchaseNumber()
        {
            try
            {
                int pn = hccPurchaseNumber.AddNew();
                return pn;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccCart> Search(string email, string purchaseNumber, DateTime? purchaseDate, string lastName, DateTime? deliveryDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    List<hccCart> retVals = new List<hccCart>();

                    retVals = cont.hcc_PurchasesSearch(email, purchaseNumber, purchaseDate, lastName, deliveryDate).ToList();

                    return retVals.OrderByDescending(d => d.PurchaseNumber).ToList();
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public static void CalculateDiscountForItemByCart(hccCart cart, hccCartItem cartItem, decimal totalNA)
        {
            decimal adjItemPrice = 0.00m;
            decimal discountRation = 0.00m;

            // adjust price for discount                        
            if (cart.CouponID.HasValue)
            {
                hccCoupon curCoupon = hccCoupon.GetById(cart.CouponID.Value);

                if (curCoupon != null)
                {
                    // check coupon usage against userId

                    Enums.CouponUsageType usageType = curCoupon.UsageType;
                    bool canUseCoupon = true;

                    // check CouponsUsed
                    if (usageType == Enums.CouponUsageType.FirstPurchaseOnly || usageType == Enums.CouponUsageType.OneTimeUse)
                    {
                        List<hccCart> couponCarts = hccCoupon.HasBeenUsedByUser(curCoupon.CouponID, cart.AspNetUserID.Value);

                        if (couponCarts.Count > 0)
                        {   // previous carts not this cart
                            if (couponCarts.Count > 1
                                || (couponCarts.Count == 1 && couponCarts[0].CartID != cart.CartID))
                                canUseCoupon = false;
                        }

                        if (usageType == Enums.CouponUsageType.FirstPurchaseOnly)
                        {   // previous paid/completed carts
                            if (hccCart.GetBy(cart.AspNetUserID.Value).Where(a => a.Status == Enums.CartStatus.Paid || a.Status == Enums.CartStatus.Fulfilled).Count() > 0)
                                canUseCoupon = false;
                        }
                    }

                    if (canUseCoupon)
                    {
                        Enums.CouponDiscountType couponType = curCoupon.DiscountType;

                        switch (couponType)
                        {
                            case Enums.CouponDiscountType.Monetary:
                                decimal itemPctOfTotalNA = 0.00m;
                                itemPctOfTotalNA = cartItem.ItemPrice / totalNA;

                                discountRation = curCoupon.Amount * itemPctOfTotalNA;
                                adjItemPrice = Helpers.TruncateDecimal(cartItem.ItemPrice - discountRation, 2);

                                break;
                            case Enums.CouponDiscountType.Percentage:

                                discountRation = Helpers.TruncateDecimal((cartItem.ItemPrice * (curCoupon.Amount / 100)), 2);
                                adjItemPrice = cartItem.ItemPrice - discountRation;
                                break;
                            default: break;
                        }
                    }
                    else
                    {
                        cart.CouponID = null;
                        //lblFeedbackCoupon.Text = "The coupon for this cart was invalid and has been removed.";
                    }
                }
            }

            cartItem.DiscountAdjPrice = adjItemPrice;
            cartItem.DiscountPerEach = discountRation;

            if (cartItem.DiscountAdjPrice == 0.00m)
                cartItem.DiscountAdjPrice = cartItem.ItemPrice; // reflect item price here to ease calculations later of dealing with  DiscountAdjPrice == 0.00m ?? cartItem.ItemPrice
        }

        public void CalculateTotals(List<ProfileCart> profileCarts)
        {
            try
            {

                this.SubTotalAmount = 0.00m;
                this.TaxAmount = 0.00m;
                if (HttpContext.Current.Session["ShippingAmount"] != null)
                {
                    this.ShippingAmount = Convert.ToDecimal(HttpContext.Current.Session["ShippingAmount"].ToString());
                }
                else
                {
                    this.ShippingAmount = 0.00m;
                }
                this.SubTotalDiscount = 0.00m;
                this.ShippingDiscount = 0.00m;
                //this.SubTotalDiscount = 0.00m;
                this.TaxDiscount = 0.00m;
                this.TotalDiscount = 0.00m;
                this.TaxableAmount = 0.00m;
                this.DiscretionaryTaxAmount = 0.00m;

                // non-adjusted total price
                decimal totalNA = 0.00m;
                string itemName1 = string.Empty;
                profileCarts.ForEach(delegate (ProfileCart profCart)
                { totalNA += profCart.SubTotalNA; });

                profileCarts.ForEach(delegate (ProfileCart profCart)
                {
                    profCart.CartItemsWithMealSides.ForEach(delegate (hccCartItem cartItem)
                    {
                        CalculateDiscountForItemByCart(this, cartItem, totalNA);//,itemName1
                    });

                    this.SubTotalAmount += profCart.SubTotalNA;
                    this.TaxAmount += profCart.SubTax;
                    this.ShippingAmount += profCart.ShippingFee;
                    //this.ShippingAmount += profCart.SubShipping;
                    this.SubTotalDiscount += profCart.SubDiscountAmount;
                    this.TaxableAmount += profCart.SubTaxableAmount;
                    this.DiscretionaryTaxAmount += profCart.SubDiscretionaryTaxAmount;
                });
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null
                    && ex.InnerException is SqlException
                    && ex.InnerException.Message.Contains("Arithmetic overflow"))
                {
                    throw ex.InnerException;
                }
                else
                {
                    throw ex;
                }
            }
        }

        public override string ToString()
        {
            StringBuilder retVal = new StringBuilder();
            hccUserProfile user = null;

            retVal.Append("Purchase #:" + this.PurchaseNumber + "<br>");
            retVal.Append("Total:" + this.TotalAmount + "<br>");
            //retVal.Append("NumberOfMeals" + this.hccCartItems.First().ItemName + "<br>");

            if (this.AspNetUserID.HasValue)
                user = hccUserProfile.GetBy(this.AspNetUserID.Value).SingleOrDefault(a => a.IsChildProfile == false);

            if (user != null)
                retVal.Append("Customer: " + user.FirstName + " " + user.LastName + "<br>");

            return retVal.ToString();
        }

        public string ToHtml()
        {
            try
            {
                string templatePath = HostingEnvironment.MapPath(@"~/HtmlTemplates/OrderInvoiceTemplate.htm");
                string invoiceHtml = File.ReadAllText(templatePath);

                invoiceHtml = invoiceHtml.Replace("##PurchaseNumber##", this.PurchaseNumber.ToString() + (this.IsTestOrder ? " : Test Order" : string.Empty));
                invoiceHtml = invoiceHtml.Replace("##OrderStatus##", ((Enums.CartStatus)this.StatusID).ToString());
                invoiceHtml = invoiceHtml.Replace("##OrderDate##", this.PurchaseDate.HasValue ? "Purchased Date: " + this.PurchaseDate.ToString()
                    : (this.ModifiedDate.HasValue ? "Modified Date: " + this.ModifiedDate.ToString() : string.Empty));

                string t = string.Empty;
                if (this.AuthNetResponse != null)
                {
                    if (this.AuthNetResponse.Contains('|'))
                    {
                        try
                        {
                            if (this.AuthNetResponse.StartsWith("1"))
                            {
                                if (string.IsNullOrWhiteSpace(this.AuthNetResponse.Split('|')[5]))
                                    t = this.AuthNetResponse.Split('|')[0];
                                else
                                    t = "Transaction ID: " + this.AuthNetResponse.Split('|')[5];
                            }
                            else
                                t = this.AuthNetResponse.Split('|')[4];
                        }
                        catch (Exception) { }
                    }
                    else
                    {
                        t = this.AuthNetResponse;
                    }
                }

                if (this.PurchaseDate.HasValue)
                    t += "<br>Date: " + this.PurchaseDate.ToString();

                invoiceHtml = invoiceHtml.Replace("##OrderTransactionInfo##", t);

                if (this.Status == Enums.CartStatus.Paid || this.Status == Enums.CartStatus.Fulfilled)
                {
                    hccCartSnapshot snap = hccCartSnapshot.GetBy(this.CartID);

                    if (snap != null)
                    {
                        hccAddress billAddr = null; ;
                        List<hccAddress> shipAddrs = new List<hccAddress>();
                        List<hccCartItem> cartItems = hccCartItem.GetBy(this.CartID);

                        cartItems.ForEach(delegate (hccCartItem ci)
                        {
                            if (billAddr == null && ci.SnapBillAddrId.HasValue)
                                billAddr = hccAddress.GetById(ci.SnapBillAddrId.Value);

                            hccAddress shipAddr = null;

                            if (ci.SnapShipAddrId.HasValue)
                                shipAddr = hccAddress.GetById(ci.SnapShipAddrId.Value);

                            if (shipAddr != null && shipAddrs.Count(a => a == shipAddr) == 0)
                                shipAddrs.Add(shipAddr);
                        });

                        if (billAddr != null)
                            invoiceHtml = invoiceHtml.Replace("##OrderBillingAddressToHtml##", billAddr.ToHtml());

                        // Shipping addresses ##OrderShippingAddressesToHtml##                       
                        string shippingHtml = string.Empty;

                        shipAddrs.ForEach(delegate (hccAddress addr)
                        {
                            shippingHtml += "<div style='padding-right:15px;display:inline;float:left;'>Profile:" + addr.ProfileName + "<br/>" + addr.ToHtml() + "</div>";
                        });
                        invoiceHtml = invoiceHtml.Replace("##OrderShippingAddressesToHtml##", shippingHtml);
                    }
                }
                else
                {
                    hccUserProfile parent = hccUserProfile.GetParentProfileBy((Guid)this.AspNetUserID);

                    if (parent != null && parent.BillingAddressID.HasValue)
                    {
                        hccAddress billAddr = hccAddress.GetById(parent.BillingAddressID.Value);
                        if (billAddr != null)
                            invoiceHtml = invoiceHtml.Replace("##OrderBillingAddressToHtml##", billAddr.ToHtml());
                        else
                            invoiceHtml = invoiceHtml.Replace("##OrderBillingAddressToHtml##", "No address found.");
                    }
                    else
                        invoiceHtml = invoiceHtml.Replace("##OrderBillingAddressToHtml##", "Anonymous User");

                    // Shipping addresses ##OrderShippingAddressesToHtml## 
                    List<hccUserProfile> profilesUsed = this.GetProfilesUsedInOrder();
                    string shippingHtml = string.Empty;

                    profilesUsed.ForEach(delegate (hccUserProfile profile)
                    {
                        hccAddress shipAddr = null;
                        if (profile.ShippingAddressID.HasValue)
                        {
                            shipAddr = hccAddress.GetById(profile.ShippingAddressID.Value);
                        }
                        if (shipAddr != null)
                            shippingHtml += "<div style='padding-right:15px;display:inline;float:left;'>Profile:" + profile.ProfileName + "<br/>" + shipAddr.ToHtml() + "</div>";
                    });
                    invoiceHtml = invoiceHtml.Replace("##OrderShippingAddressesToHtml##", shippingHtml);
                }

                invoiceHtml = invoiceHtml.Replace("##ItemsToHtml##", this.ItemsToHtml());

                return invoiceHtml;
            }
            catch
            {
                throw;
            }
        }

        protected string ItemsToHtml()
        {
            try
            {
                string sOut = string.Empty;
                StringBuilder sb = new StringBuilder();

                sb.Append("<table class='table table-hover table-bordered'>");
                sb.Append("<tr>");
                sb.Append("<th>Order #</th>");
                sb.Append("<th>Item</th>");
                sb.Append("<th>Quantity</th>");
                sb.Append("<th>Unit Price</th>");
                sb.Append("<th>Sub-Total</th>");
                //sb.Append("<th style='text-align:right;width:100px;'>Sub-TotalMeals</th>");
                sb.Append("</tr><td>");

                string profileRow = "<tr><td>Profile:&nbsp;{0} : {1}</td></tr>";
                string itemRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>";
                string itemRowAlt = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td></tr>";
                string subTotalRow = "<tr><td colspan='6'>Profile Sub-Total:&nbsp;{0}</td></tr>";
                // subTotalMeal = "<tr><td colspan='6' style='text-align:right; padding: 2px; font-weight: bold;'>Profile Sub-TotalMeal:&nbsp;{0}</td></tr>";
                string formatToUse = itemRow;

                List<hccCartItem> cartItems = hccCartItem.GetBy(this.CartID);
                List<ProfileCart> CurrentProfileCarts = new List<ProfileCart>();

                foreach (hccCartItem cartItem in cartItems)
                {
                    ProfileCart profCart;
                    int shippingAddressId;

                    if (cartItem.IsMealSide)
                        continue;

                    if (cartItem.UserProfile != null && (cartItem.UserProfile.UseParentShipping || cartItem.UserProfile.ShippingAddressID.HasValue))
                    {
                        if (cartItem.UserProfile.UseParentShipping && OwnerProfile != null)
                            shippingAddressId = OwnerProfile.ShippingAddressID.Value;
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

                    profCart.CartItems.Add(cartItem);
                }

                CurrentProfileCarts.ForEach(delegate (ProfileCart profCart)
                {
                    List<string> profiles = new List<string>();

                    profCart.CartItems.ForEach(delegate (hccCartItem ci)
                    {

                        if (ci.UserProfile != null && !profiles.Contains(ci.UserProfile.ProfileName))
                            profiles.Add(ci.UserProfile.ProfileName);
                    });

                    if (profCart.CartItems[0] != null && profCart.CartItems[0].UserProfile != null)
                        sb.AppendFormat(profileRow, profiles.Aggregate((c, d) => c + ", " + d), profCart.CartItems[0].DeliveryDate.ToShortDateString(),"");//, profCart.CartItems[0].NumberOfMeals

                    bool isEven = true;

                    profCart.CartItems.ForEach(delegate (hccCartItem cartItem)
                    {
                        if (isEven)
                        {
                            formatToUse = itemRow;
                        }
                        else
                        {
                            formatToUse = itemRowAlt;
                        }

                        string fullName = cartItem.ItemName;
                        if (cartItem.IsCancelled)
                            fullName += " - <b>Cancelled<b>";

                        sb.AppendFormat(formatToUse, cartItem.OrderNumber, fullName, cartItem.Quantity.ToString(),
                            cartItem.TotalItemPrice.ToString("c"), cartItem.ItemSubTotalNA.ToString("c"),"");// cartItem.ItemSubMealTotal.ToString("c")
                        isEven = !isEven;
                    });

                    sb.AppendFormat(subTotalRow, profCart.SubTotalNA.ToString("c"));
                    //sb.AppendFormat(subTotalMeal, profCart.SubMealsCount.ToString("c"));
                    sb.Append("<tr><td colspan='6'><hr /></td></tr>");
                });

                StringBuilder sb2 = new StringBuilder();
                bool useSB2 = false;
                sb2.Append("<tr><td rowspan='10'><table width='200px'>");

                if (this.CouponID.HasValue)
                {
                    hccCoupon coup = hccCoupon.GetById(this.CouponID.Value);
                    if (coup != null)
                    {
                        useSB2 = true;
                        sb2.Append("<tr><td><b>Coupon Used</b>:</td></tr>");
                        sb2.AppendFormat("<tr><td>{0}</td></tr>", coup.RedeemCode);
                    }
                }

                if (!string.IsNullOrWhiteSpace(this.RedeemedGiftCertCode))
                {
                    useSB2 = true;
                    sb2.Append("<tr><td><b>Gift Certificate Redeemed</b>:</td></tr>");
                    sb2.AppendFormat("<tr><td>{0}</td></tr>", this.RedeemedGiftCertCode);
                }

                if (this.PaymentProfileID.HasValue)
                {
                    useSB2 = true;
                    sb2.Append("<tr><td><b>Payment Card Used:</b>:</td></tr>");
                    sb2.AppendFormat("<tr><td>{0}{1}</td></tr>", this.PaymentProfile == null ? string.Empty : this.PaymentProfile.ToString(), this.IsTestOrder ? " - Test Mode" : string.Empty);
                }

                sb2.Append("</table></td>");

                hccLedger cartLedger = hccLedger.GetBy(this.CartID);

                if (useSB2)
                {
                    sb.Append(sb2.ToString());
                    sb.Append("<td>&nbsp;</td><td colspan='3' style='text-align:right;'><b>Subtotal:</td><td style='text-align:right;'>" + this.SubTotalAmount.ToString("c") + "</td></tr>");
                    sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;'>Tax:</td><td style='text-align:right;'>" + this.TaxAmount.ToString("c") + "</td></tr>");
                    sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;'>Shipping:</td><td style='text-align:right;'>" + this.ShippingAmount.ToString("c") + "</td></tr>");
                    sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;'>Discount:</td><td style='text-align:right;'>(" + this.SubTotalDiscount.ToString("c") + ")</td></tr>");
                    sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;border-top:1px solid black;'>Total:</td><td style='text-align:right;border-top:1px solid black;'>" + this.TotalAmount.ToString("c") + "</td></tr>");

                    if (cartLedger != null)
                    {
                        sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;'>Account Balance:</td><td style='text-align:right;'>" + (cartLedger.PostBalance + cartLedger.CreditFromBalance).ToString("c") + "</td></tr>");
                        sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;border-top:1px solid black;'>Credit From Balance:</td><td style='text-align:right;border-top:1px solid black;'>" + (cartLedger.CreditFromBalance).ToString("c") + "</td></tr>");
                    }
                    else
                        sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;'>Account Balance:</td><td style='text-align:right;'>" + (this.OwnerProfile != null ? this.OwnerProfile.AccountBalance : 0.00m).ToString("c") + "</td></tr>");

                    sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;'>Payment Amount:</td><td style='text-align:right;'>" + this.PaymentDue.ToString("c") + "</td></tr>");

                    if (cartLedger != null)
                    {
                        sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;border-top:1px solid black;'>Remaining Account Balance:</td><td style='text-align:right;border-top:1px solid black;'>" + (cartLedger.PostBalance).ToString("c") + "</td></tr>");
                    }
                    else
                        sb.Append("<tr><td>&nbsp;</td><td colspan='3' style='text-align:right; font-weight: bold;border-top:1px solid black;'>Remaining Account Balance:</td><td style='text-align:right;border-top:1px solid black;'>" + (this.OwnerProfile != null ? (this.OwnerProfile.AccountBalance - this.CreditAppliedToBalance) : 0.00m).ToString("c") + "</td></tr>");

                }
                else
                {
                    sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right;'><b>Subtotal:</td><td style='text-align:right;'>" + this.SubTotalAmount.ToString("c") + "</td></tr>");
                    sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;'>Tax:</td><td style='text-align:right;'>" + this.TaxAmount.ToString("c") + "</td></tr>");
                    sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;'>Shipping:</td><td style='text-align:right;'>" + this.ShippingAmount.ToString("c") + "</td></tr>");
                    sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;'>Discount:</td><td style='text-align:right;'>(" + this.SubTotalDiscount.ToString("c") + ")</td></tr>");
                    sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;border-top:1px solid black;'>Total:</td><td style='text-align:right;border-top:1px solid black;'>" + this.TotalAmount.ToString("c") + "</td></tr>");

                    if (cartLedger != null)
                    {
                        sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;'>Credit From Balance:</td><td style='text-align:right;'>" + (cartLedger.CreditFromBalance).ToString("c") + "</td></tr>");
                        sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;'>Account Balance:</td><td style='text-align:right;'>" + (cartLedger.PostBalance + cartLedger.CreditFromBalance).ToString("c") + "</td></tr>");
                    }
                    else
                        sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;'>Account Balance:</td><td style='text-align:right;'>"
                            + (this.OwnerProfile == null ? 0.00m : this.OwnerProfile.AccountBalance).ToString("c") + "</td></tr>");

                    sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;'>Payment Amount:</td><td style='text-align:right;'>" + this.PaymentDue.ToString("c") + "</td></tr>");

                    if (cartLedger != null)
                        sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;border-top:1px solid black;'>Remaining Account Balance:</td><td style='text-align:right;border-top:1px solid black;'>" + (cartLedger.PostBalance).ToString("c") + "</td></tr>");
                    else
                        sb.Append("<tr><td colspan='2'>&nbsp;</td><td colspan='2' style='text-align:right; font-weight: bold;border-top:1px solid black;'>Remaining Account Balance:</td><td style='text-align:right;border-top:1px solid black;'>"
                            + (this.OwnerProfile == null ? 0.00m : (this.OwnerProfile.AccountBalance - this.CreditAppliedToBalance)).ToString("c") + "</td></tr>");
                }

                sb.Append("</table>");

                sOut = sb.ToString();

                return sOut;
            }
            catch
            {
                throw;
            }
        }
    }
}
