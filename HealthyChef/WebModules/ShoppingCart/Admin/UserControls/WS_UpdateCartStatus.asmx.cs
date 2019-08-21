using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using HealthyChef.AuthNet;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    /// <summary>
    /// Summary description for WS_UpdateCartStatus
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WS_UpdateCartStatus : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateCarts(List<UpdateCartItem> carts)
        {
            try
            {
                UpdateCartItem updatecart = carts[0];
                hccCart CurrentCart = hccCart.GetById(updatecart.cartId);
                hccUserProfile ownerProfile = CurrentCart.OwnerProfile;
                hccAddress snapBillAddr = null;
                List<hccCartItem> cartItems = null;
                bool isAuthNet = false;
                hccLedger ledger = null;
                string retVal = string.Empty;

                if (CurrentCart != null && ownerProfile != null)
                {
                    hccAddress billAddr = null;
                    hccUserProfilePaymentProfile activePaymentProfile = ownerProfile.ActivePaymentProfile;

                    if (updatecart.updateStatus && CurrentCart.StatusID != updatecart.statusId)
                    {
                        CurrentCart.StatusID = updatecart.statusId;
                        CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                        CurrentCart.ModifiedDate = DateTime.Now;

                        if (updatecart.statusId == (int)Enums.CartStatus.Paid)
                        {
                            if (!CurrentCart.PurchaseBy.HasValue)
                                CurrentCart.PurchaseBy = (Guid)Helpers.LoggedUser.ProviderUserKey;

                            if (!CurrentCart.PurchaseDate.HasValue)
                                CurrentCart.PurchaseDate = DateTime.Now;
                        }
                        CurrentCart.Save();
                    }

                    if (updatecart.updateAddresses) // re-snap addresses
                    {
                        if (ownerProfile.BillingAddressID.HasValue)
                        {
                            billAddr = hccAddress.GetById(ownerProfile.BillingAddressID.Value);

                            if (billAddr != null)
                            {
                                snapBillAddr = new hccAddress
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
                                    ProfileName = ownerProfile.ProfileName
                                };
                                snapBillAddr.Save();
                            }
                        }
                        else
                        { retVal += "Profile has no billing address on record."; }

                        if (cartItems == null)
                            cartItems = hccCartItem.GetBy(CurrentCart.CartID);

                        cartItems.ForEach(delegate(hccCartItem ci)
                        {
                            hccAddress shipAddr = null;

                            if (snapBillAddr != null)
                                ci.SnapBillAddrId = snapBillAddr.AddressID;

                            if (ci.UserProfile == null)
                                ci.UserProfileID = ownerProfile.UserProfileID;

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

                            ci.Save();
                        });
                    }

                    if (updatecart.rerunAuthNet)
                    {
                        CurrentCart.StatusID = (int)Enums.CartStatus.Unfinalized;
                        CurrentCart.PurchaseBy = null;
                        CurrentCart.PurchaseDate = null;

                        if (ownerProfile != null)
                        {
                            AuthNetConfig ANConfig = new AuthNetConfig();

                            if (ANConfig.Settings.TestMode)
                                CurrentCart.IsTestOrder = true;

                            if (CurrentCart.PaymentDue > 0.00m)
                            {
                                try
                                {   // if total balance remains
                                    CustomerInformationManager cim = new CustomerInformationManager();

                                    if (activePaymentProfile != null)
                                    {
                                        AuthorizeNet.Order order = new AuthorizeNet.Order(ownerProfile.AuthNetProfileID,
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
                                        }
                                        CurrentCart.Save();
                                    }
                                    else
                                    {
                                        return "No active payment profile.";
                                    }
                                }
                                catch (Exception ex)
                                {
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
                                        }
                                    }
                                    else
                                    { throw; }
                                }
                            }
                            else
                            {      // no balance left to pay on order, set as paid
                                CurrentCart.AuthNetResponse = "Paid with account balance.";
                                CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                CurrentCart.ModifiedDate = DateTime.Now;
                                CurrentCart.PurchaseBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                CurrentCart.PurchaseDate = DateTime.Now;
                                CurrentCart.StatusID = (int)Enums.CartStatus.Paid;
                                CurrentCart.Save();
                            }
                        }
                    }

                    if (updatecart.createLedgerEntry)
                    {
                        if (ledger == null)
                            ledger = hccLedger.GetBy(CurrentCart.CartID);

                        if (ledger == null)
                        {
                            SaveLedger(ledger, CurrentCart, ownerProfile);
                        }
                    }

                    if (updatecart.createNewSnapshot)
                    {
                        if (((Enums.CartStatus)CurrentCart.StatusID) == Enums.CartStatus.Paid)
                        {
                            if (ledger == null)
                                ledger = hccLedger.GetBy(CurrentCart.CartID);

                            if (ledger == null) // it still equals null
                            {
                                SaveLedger(ledger, CurrentCart, ownerProfile);
                            }

                            // create snapshot here
                            hccCartSnapshot snap = new hccCartSnapshot
                            {
                                CartId = CurrentCart.CartID,
                                MembershipId = ownerProfile.MembershipID,
                                LedgerId = ledger.LedgerID,
                                AccountBalance = ownerProfile.AccountBalance,
                                AuthNetProfileId = ownerProfile.AuthNetProfileID,
                                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                DefaultCouponId = ownerProfile.DefaultCouponId,
                                Email = ownerProfile.ASPUser.Email,
                                FirstName = ownerProfile.FirstName,
                                LastName = ownerProfile.LastName,
                                ProfileName = ownerProfile.ProfileName
                            };

                            if (isAuthNet)
                            {
                                if (activePaymentProfile != null)
                                {
                                    snap.AuthNetPaymentProfileId = activePaymentProfile.AuthNetPaymentProfileID;
                                    snap.CardTypeId = activePaymentProfile.CardTypeID;
                                    snap.CCLast4 = activePaymentProfile.CCLast4;
                                    snap.ExpMon = activePaymentProfile.ExpMon;
                                    snap.ExpYear = activePaymentProfile.ExpYear;
                                    snap.NameOnCard = activePaymentProfile.NameOnCard;
                                }
                                else
                                {
                                    return "No active payment profile.";
                                }
                            }
                            else
                            {
                                snap.AuthNetPaymentProfileId = string.Empty;
                                snap.CardTypeId = 0;
                                snap.CCLast4 = string.Empty;
                                snap.ExpMon = 0;
                                snap.ExpYear = 0;
                                snap.NameOnCard = string.Empty;
                            }
                            snap.Save();

                            if (billAddr == null && ownerProfile.BillingAddressID.HasValue)
                            {
                                billAddr = hccAddress.GetById(ownerProfile.BillingAddressID.Value);
                            }

                            if (billAddr != null)
                            {
                                snapBillAddr = new hccAddress
                                {
                                    Address1 = billAddr.Address1,
                                    Address2 = billAddr.Address2,
                                    AddressTypeID = billAddr.AddressTypeID,
                                    City = billAddr.City,
                                    Country = billAddr.Country,
                                    DefaultShippingTypeID = billAddr.DefaultShippingTypeID,
                                    FirstName = billAddr.FirstName,
                                    IsBusiness = billAddr.IsBusiness,
                                    LastName = billAddr.LastName,
                                    Phone = billAddr.Phone,
                                    PostalCode = billAddr.PostalCode,
                                    State = billAddr.State,
                                    ProfileName = ownerProfile.ProfileName
                                };
                                snapBillAddr.Save();
                            }
                            else
                            { retVal += "Profile has no billing address on record."; }

                            // copy and replace of all addresses for snapshot
                            if (cartItems == null)
                                cartItems = hccCartItem.GetBy(CurrentCart.CartID);

                            cartItems.ToList().ForEach(delegate(hccCartItem ci)
                            {
                                if (snapBillAddr != null)
                                    ci.SnapBillAddrId = snapBillAddr.AddressID;

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
                                        AddressTypeID = shipAddr.AddressTypeID,
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
                                else
                                { retVal += "Profile has no billing address on record."; }

                                ci.Save();
                            });
                        }
                    }

                    if (updatecart.sendCustomerEmail)
                    {
                        try
                        {
                            Email.EmailController ec = new Email.EmailController();
                            ec.SendMail_OrderConfirmationMerchant(ownerProfile.FirstName + " " + ownerProfile.LastName, CurrentCart.ToHtml(), CurrentCart.CartID);
                        }
                        catch (Exception ex)
                        { BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Send Merchant Mail Failed", this, ex); }
                    }

                    if (updatecart.sendMerchantEmail)
                    {
                        try
                        {
                            Email.EmailController ec = new Email.EmailController();
                            ec.SendMail_OrderConfirmationCustomer(ownerProfile.ASPUser.Email, ownerProfile.FirstName + " " + ownerProfile.LastName, CurrentCart.ToHtml());
                        }
                        catch (Exception ex)
                        { BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Send customer Mail Failed", this, ex); }
                    }

                    if (updatecart.repairCartCals)
                    {
                        if (cartItems == null)
                            cartItems = hccCartItem.GetBy(CurrentCart.CartID);
                        // wrap in programs

                        cartItems.ForEach(delegate(hccCartItem ci)
                        {
                            if (ci.ItemType == Enums.CartItemType.DefinedPlan)
                            {
                                hccProgramPlan cp = hccProgramPlan.GetById(ci.Plan_PlanID.Value);

                                if (cp != null)
                                {
                                    for (int i = 0; i < cp.NumWeeks; i++)
                                    {
                                        hccProductionCalendar cal;
                                        cal = hccProductionCalendar.GetBy(ci.DeliveryDate.AddDays(7 * i));

                                        if (cal != null)
                                        {
                                            hccCartItemCalendar existCal = hccCartItemCalendar.GetBy(ci.CartItemID, cal.CalendarID);
                                            if (existCal == null)
                                            {
                                                hccCartItemCalendar cartCal = new hccCartItemCalendar { CalendarID = cal.CalendarID, CartItemID = ci.CartItemID, IsFulfilled = false };
                                                cartCal.Save();
                                            }
                                        }
                                        else
                                            BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(
                                                "No production calendar found for Delivery Date: " + ci.DeliveryDate.AddDays(7 * i).ToShortDateString(), this);
                                    }
                                }
                            }
                        });
                    }

                    if (updatecart.reCalcItemTax)
                    {
                        try
                        {
                            if (cartItems == null)
                                cartItems = hccCartItem.GetBy(CurrentCart.CartID);

                            List<ProfileCart> CurrentProfileCarts = new List<ProfileCart>();

                            if (cartItems.Count > 0)
                            {
                                hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                                //List<hccProductionCalendar> pc = new List<hccProductionCalendar>();

                                foreach (hccCartItem cartItem in cartItems)
                                {
                                    ProfileCart profCart;
                                    int shippingAddressId;

                                    //if (!pc.Exists(a => a.DeliveryDate == cartItem.DeliveryDate))
                                    //    pc.Add(hccProductionCalendar.GetBy(cartItem.DeliveryDate));

                                    //hccProductionCalendar cpc = pc.SingleOrDefault(a => a.DeliveryDate == cartItem.DeliveryDate);

                                    //if (cpc != null && (cpc.OrderCutOffDate.AddDays(1) >= DateTime.Now || (HttpContext.Current.Request.Url.OriginalString.Contains("Admin"))))
                                    //{
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

                                    profCart.CartItems.Add(cartItem);
                                    //}
                                    //else
                                    //{
                                    //    //cartItem.Delete();
                                    //    //lblFeedbackCart.Text = "Item(s) removed from cart due to expiration of availability.";

                                    //}
                                }
                            }

                            //// display totals
                            CurrentCart.CalculateTotals(CurrentProfileCarts);
                        }
                        catch (Exception ex)
                        { BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Attempt to recalculate taz for cart items failed.", this, ex); }
                    }
                }
                return "Cart Updated: " + DateTime.Now;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void SaveLedger(hccLedger ledger, hccCart CurrentCart, hccUserProfile profile)
        {
            ledger = new hccLedger
            {
                PaymentDue = CurrentCart.PaymentDue,
                TotalAmount = CurrentCart.TotalAmount,
                AspNetUserID = CurrentCart.AspNetUserID.Value,
                AsscCartID = CurrentCart.CartID,
                CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                CreatedDate = DateTime.Now,
                Description = "Cart Order Payment - Purchase Number: " + CurrentCart.PurchaseNumber.ToString() + " - from re-snapshot of order.",
                TransactionTypeID = (int)Enums.LedgerTransactionType.Purchase
            };

            if (CurrentCart.IsTestOrder)
                ledger.Description += " - Test Mode";

            if (CurrentCart.CreditAppliedToBalance > 0)
            {
                profile.AccountBalance = profile.AccountBalance - CurrentCart.CreditAppliedToBalance;
                ledger.CreditFromBalance = CurrentCart.CreditAppliedToBalance;
            }

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
            }
        }
    }

    public class UpdateCartItem
    {
        public int cartId { get; set; }
        public bool updateStatus { get; set; }
        public int statusId { get; set; }
        public bool updateAddresses { get; set; }
        public bool rerunAuthNet { get; set; }
        public bool createLedgerEntry { get; set; }
        public bool createNewSnapshot { get; set; }
        public bool sendCustomerEmail { get; set; }
        public bool sendMerchantEmail { get; set; }
        public bool repairCartCals { get; set; }
        public bool reCalcItemTax { get; set; }
    }
}
