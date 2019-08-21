using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AuthorizeNet;
using HealthyChef.AuthNet;
using HealthyChef.Common;
using HealthyChef.DAL;
using ZipToTaxService;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    public partial class RecurringOrdersManager : System.Web.UI.Page
    {
        public DateTime MinCutOffDate { get; set; }

        public void Page_Init(object sender, EventArgs e)
        {
            //lvRecurringOrders.DataBinding += lvRecurringOrders_DataBinding;
            //lvRecurringOrders.ItemDataBound += lvRecurringOrders_ItemDataBound;

            //lvAllRecurringOrders.DataBinding += lvAllRecurringOrders_DataBinding;
            //lvAllRecurringOrders.ItemDataBound += lvAllRecurringOrders_ItemDataBound;

            try
            {
                using (var hcE = new healthychefEntities())
                {
                    var roDeliveryDate = hcE.hcc_RecurringOrderByDeliveryDate();

                    MinCutOffDate = ((DateTime) roDeliveryDate.Min(x => x.maxcutoffdate)).AddDays(1);
                    //lvAllRecurringOrders.DataSource = hcE.hccRecurringOrders.ToList();
                    //lvAllRecurringOrders.DataSource = hcE.hcc_RecurringOrderByDeliveryDate().ToList();
                    //lvAllRecurringOrders.DataBind();
                }
            }
            catch
            {
            }

        }

        //void lvRecurringOrders_ItemDataBound(object sender, ListViewItemEventArgs e)
        //{

        //}

        //void lvRecurringOrders_DataBinding(object sender, EventArgs e)
        //{

        //}

        private void lvAllRecurringOrders_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        private void lvAllRecurringOrders_DataBinding(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //using (var hcE = new healthychefEntities())
            //{
            //    //lvAllRecurringOrders.DataSource = hcE.hccRecurringOrders.ToList();
            //    lvAllRecurringOrders.DataSource = hcE.hcc_RecurringOrderByDeliveryDate().ToList();
            //    lvAllRecurringOrders.DataBind();
            //}    
        }


        //public void btnGenerate_Click(object sender, EventArgs e)
        //{
        //    var orders = hccRecurringOrder.GetExpiringOrders();

        //    //Invoke Method to Call SP [hcc_CloneOrder] use the output parameter to call ProcessNewOrder in the line below...
        //    using (var hcE = new healthychefEntities())
        //    {
        //        var ordersAdded = new List<int>();
        //        foreach (var order in orders)
        //        {
        //            var recurringOrder = hcE.hccRecurringOrders.SingleOrDefault(r => r.CartID == order.CartID);
        //            var newCartId = new ObjectParameter("newCartID", typeof(int));

        //            var taxRate = 0.00m;
        //            var customer = hccUserProfile.GetById(recurringOrder.UserProfileID ?? 0);
        //            if (customer != null)
        //            {
        //                var shippingAddr = hccAddress.GetById(customer.ShippingAddressID ?? 0);
        //                taxRate = GetProfileTaxRate(shippingAddr);
        //            }

        //            hcE.hcc_CloneOrder(order.CartID, order.CartItemID, taxRate, newCartId);
        //            ordersAdded.Add((int)newCartId.Value);
        //        }
        //        //Unique Orders 
        //        if (ordersAdded.Count > 0)
        //        {
        //            var ordersToProcess = ordersAdded.Distinct();
        //            foreach (var order in ordersToProcess)
        //            {
        //                ProcessNewOrder(order);
        //            }
        //        }

        //        lvAllRecurringOrders.DataSource = hcE.hcc_RecurringOrderByDeliveryDate().ToList();
        //        lvAllRecurringOrders.DataBind();
        //    }

        //    // Output the number of orders that are processed
        //    if (orders.Count > 0)
        //    {
        //        litNumRecurringLabel.Visible = true;
        //        litNumRecurringOrders.Text = orders.Count.ToString();
        //        litNumRecurringOrders.Visible = true;
        //    }

        //}

        public void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateOrders(false);
        }

        public void btnTestGenerate_Click(object sender, EventArgs e)
        {
            GenerateOrders(true);
        }


        void GenerateOrders(bool testMode)
        {
            var orders = testMode ? hccRecurringOrder.GetTestExpiringOrders() : hccRecurringOrder.GetExpiringOrders();
            litNoOrders.Visible = orders.Count == 0;


            //Invoke Method to Call SP [hcc_CloneOrder] use the output parameter to call ProcessNewOrder in the line below...
            using (var hcE = new healthychefEntities())
            {
                var ordersAdded = new List<int>();
                foreach (var order in orders)
                {
                    var newCartId = new ObjectParameter("newCartID", typeof (int));

                    var taxRate = 0.00m;
                    var customer = hccUserProfile.GetById(order.UserProfileID ?? 0);
                    if (customer != null)
                    {
                        var shippingAddr = hccAddress.GetById(customer.ShippingAddressID ?? 0);
                        taxRate = GetProfileTaxRate(shippingAddr);
                    }
                   
                    hcE.hcc_CloneOrder(order.CartID, order.CartItemID, taxRate, newCartId);
                    ordersAdded.Add((int)newCartId.Value);                    
                }
                //Unique Orders 
                if (ordersAdded.Count > 0)
                {
                    var ordersToProcess = ordersAdded.Distinct();
                    foreach (var order in ordersToProcess)
                    {
                        ProcessNewOrder(order);        
                    }                    
                }
                
                //lvAllRecurringOrders.DataSource = hcE.hcc_RecurringOrderByDeliveryDate().ToList();
                //lvAllRecurringOrders.DataBind();
            }

            // Output the number of orders that are processed
            if (orders.Count > 0)
            {
                litNumRecurringLabel.Visible = true;
                litNumRecurringOrders.Text = orders.Count.ToString();
                litNumRecurringOrders.Visible = true;
            }
        }

        public string GetCustomer(int cartId)
        {
            //var sb = new StringBuilder();

            //sb.Append("<a href='mailto:" + hccUserProfile.GetParentProfileBy((Guid)hccCart.GetById(cartId).AspNetUserID).ASPUser.Email + "'>");
            //sb.Append(hccUserProfile.GetParentProfileBy((Guid)hccCart.GetById(cartId).AspNetUserID).FullName);
            //sb.Append("</a>");
            try
            {
                return hccUserProfile.GetParentProfileBy((Guid) hccCart.GetById(cartId).AspNetUserID).ParentProfileName;
                    //.ASPUser.Email
            }
            catch (Exception)
            {
                return "Anonymous User";
            }

        }

        public string GetProfileName(int userProfileId)
        {
            var sb = new StringBuilder();
            try
            {
                //sb.Append("<a href='mailto:" + hccUserProfile.GetParentProfileBy((Guid)hccCart.GetById(cartId).AspNetUserID).ASPUser.Email + "'>");
                //sb.Append(hccUserProfile.GetParentProfileBy((Guid)hccCart.GetById(cartId).AspNetUserID).ProfileName);
                //sb.Append("</a>");
                var profile = hccUserProfile.GetById(userProfileId);
                sb.Append(profile.ProfileName);

                return sb.ToString();
            }
            catch (Exception)
            {
                return "Anonymous User";
            }
        }

        public string GetCartItemMeal(int cartItemId)
        {
            try
            {
                var cartItem = hccCartItem.GetById(cartItemId);
                return cartItem.ItemName;
            }
            catch (Exception)
            {
                return "Item not available";
            }
        }

        public string GetNextRecurringDate(int cartId, int cartItemId)
        {
            using (var hcE = new healthychefEntities())
            {
                try
                {
                    var recurringItem = hcE.hcc_RecurringOrderStartDate(cartId, cartItemId).SingleOrDefault();

                    return ((DateTime) recurringItem.MaxDeliveryDate).ToShortDateString();
                }
                catch (Exception)
                {
                    return "Date not available";
                }

            }
        }

        protected void btnDeleteOnCommand(object sender, CommandEventArgs e)
        {
            using (var hcE = new healthychefEntities())
            {
                var cartId = int.Parse(e.CommandArgument.ToString().Split('_')[0]);
                var cartItemId = int.Parse(e.CommandArgument.ToString().Split('_')[1]);

                var rOrder = hcE.hccRecurringOrders.FirstOrDefault(i => i.CartID == cartId && i.CartItemID == cartItemId);
                hcE.hccRecurringOrders.DeleteObject(rOrder);
                hcE.SaveChanges();

                //lvAllRecurringOrders.DataSource = hcE.hcc_RecurringOrderByDeliveryDate().ToList();
                //lvAllRecurringOrders.DataBind();
            }

        }

        private void ProcessNewOrder(int cartId)
        {
            //bool dupTransaction = false;
            hccCart CurrentCart = null;
            try
            {
                // TODO: Check the cart for more then one recurring item

                CurrentCart = hccCart.GetById(cartId);

                hccUserProfile profile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                hccAddress billAddr = null;

                var ppName =
                    hccUserProfile.GetParentProfileBy((Guid) hccCart.GetById(cartId).AspNetUserID).ParentProfileName;
                var pName = hccUserProfile.GetParentProfileBy((Guid) hccCart.GetById(cartId).AspNetUserID).ASPUser.Email;

                //if (CurrentCart.StatusID == (int)Enums.CartStatus.Unfinalized)
                if (CurrentCart.StatusID == (int) Enums.CartStatus.Unfinalized)
                {
                    if (profile != null)
                    {
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
                            {
                                // if total balance remains
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
                                    order.Description = "Healthy Chef Creations Purchase #" +
                                                        CurrentCart.PurchaseNumber.ToString();
                                    // Add a PO number to make purchases unique as subsequent transactions with the same amount are rejected by Auth.net as duplicate  
                                    // order.PONumber = "PO" + CurrentCart.PurchaseNumber.ToString();

                                    AuthorizeNet.IGatewayResponse rsp = cim.AuthorizeAndCapture(order);

                                    try
                                    {
                                        CurrentCart.AuthNetResponse = rsp.ResponseCode + "|" + rsp.Approved.ToString()
                                                                      + "|" + rsp.AuthorizationCode + "|" +
                                                                      rsp.InvoiceNumber + "|" + rsp.Message
                                                                      + "|" + rsp.TransactionID + "|" +
                                                                      rsp.Amount.ToString() + "|" + rsp.CardNumber;
                                    }
                                    catch (Exception)
                                    {
                                    }

                                    if (rsp.ResponseCode.StartsWith("1"))
                                    {
                                        CurrentCart.ModifiedBy = (Guid) Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.ModifiedDate = DateTime.Now;
                                        CurrentCart.PurchaseBy = (Guid) Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.PurchaseDate = DateTime.Now;
                                        CurrentCart.PaymentProfileID = activePaymentProfile.PaymentProfileID;
                                        CurrentCart.StatusID = (int) Enums.CartStatus.Paid;

                                        isAuthNet = true;
                                    }
                                    else if (rsp.Message.Contains("E00027")) // Duplicate transaction
                                    {
                                        order = new AuthorizeNet.Order(profile.AuthNetProfileID,
                                            activePaymentProfile.AuthNetPaymentProfileID, null)
                                        {
                                            Amount = CurrentCart.PaymentDue - .01m,
                                            // Subtract a penny from payment to make the value distinct
                                            InvoiceNumber = CurrentCart.PurchaseNumber.ToString(),
                                            Description =
                                                "Healthy Chef Creations Purchase #" +
                                                CurrentCart.PurchaseNumber.ToString()
                                        };

                                        // charge CIM account with PaymentDue balance
                                        rsp = cim.AuthorizeAndCapture(order);

                                        try
                                        {
                                            CurrentCart.AuthNetResponse = rsp.ResponseCode + "|" +
                                                                          rsp.Approved.ToString()
                                                                          + "|" + rsp.AuthorizationCode + "|" +
                                                                          rsp.InvoiceNumber + "|" + rsp.Message
                                                                          + "|" + rsp.TransactionID + "|" +
                                                                          rsp.Amount.ToString() + "|" + rsp.CardNumber;

                                            if (rsp.ResponseCode.StartsWith("1"))
                                            {
                                                //CurrentCart.PaymentDue = CurrentCart.PaymentDue - .01m;
                                                CurrentCart.ModifiedBy = (Guid) Helpers.LoggedUser.ProviderUserKey;
                                                CurrentCart.ModifiedDate = DateTime.Now;
                                                CurrentCart.PurchaseBy = (Guid) Helpers.LoggedUser.ProviderUserKey;
                                                CurrentCart.PurchaseDate = DateTime.Now;
                                                CurrentCart.PaymentProfileID = activePaymentProfile.PaymentProfileID;
                                                CurrentCart.StatusID = (int) Enums.CartStatus.Paid;

                                                isAuthNet = true;
                                            }
                                            else
                                            {
                                                lblConfirmFeedback.Text += "Authorize.Net " + rsp.Message + @" (" +
                                                                           ppName + @", " + pName + @")" + @"<br />";
                                                    // CurrentCart.AuthNetResponse;
                                                lblConfirmFeedback.ForeColor = System.Drawing.Color.Red;
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }
                                    }
                                    else
                                    {
                                        lblConfirmFeedback.Text += "Authorize.Net " + rsp.Message + @" (" + ppName +
                                                                   @", " + pName + @")" + @"<br />";
                                            // CurrentCart.AuthNetResponse;
                                        lblConfirmFeedback.ForeColor = System.Drawing.Color.Red;
                                    }
                                    CurrentCart.Save();
                                }
                                else
                                {
                                    lblConfirmFeedback.Text += "No payment profile found." + @" (" + ppName + @", " +
                                                               pName + @")" + @"<br />";
                                }
                            }
                            catch (Exception ex)
                            {
                                lblConfirmFeedback.Text += "Authorize.Net " + ex.Message + @" (" + ppName + @", " +
                                                           pName + @")" + @"<br />";
                                lblConfirmFeedback.ForeColor = System.Drawing.Color.Red;
                                if (ex is InvalidOperationException)
                                {
                                    if (CurrentCart.IsTestOrder)
                                    {
                                        CurrentCart.ModifiedBy = (Guid) Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.ModifiedDate = DateTime.Now;
                                        CurrentCart.PaymentProfileID = activePaymentProfile.PaymentProfileID;
                                        CurrentCart.AuthNetResponse = ex.Message;
                                        CurrentCart.StatusID = (int) Enums.CartStatus.Unfinalized;
                                        CurrentCart.Save();
                                    }
                                    else
                                    {
                                        BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(ex.Message, this, ex);
                                        lblConfirmFeedback.Visible = true;
                                        lblConfirmFeedback.Text += "Authorize.Net " + ex.Message + @" (" + ppName +
                                                                   @", " + pName + @")" + @"<br />";
                                        lblConfirmFeedback.ForeColor = System.Drawing.Color.Red;
                                    }
                                }
                                else
                                {
                                    throw;
                                }
                            }
                        }
                        else
                        {
                            // no balance left to pay on order, set as paid
                            CurrentCart.AuthNetResponse = "Paid with account balance.";
                            CurrentCart.ModifiedBy = (Guid) Helpers.LoggedUser.ProviderUserKey;
                            CurrentCart.ModifiedDate = DateTime.Now;
                            CurrentCart.PurchaseBy = (Guid) Helpers.LoggedUser.ProviderUserKey;
                            CurrentCart.PurchaseDate = DateTime.Now;
                            CurrentCart.StatusID = (int) Enums.CartStatus.Paid;
                            CurrentCart.Save();
                        }

                        if ((Enums.CartStatus) CurrentCart.StatusID == Enums.CartStatus.Paid)
                            //&& !isDuplicateTransaction
                        {
                            hccLedger ledger = new hccLedger
                            {
                                //PaymentDue = dupTransaction ? CurrentCart.PaymentDue : CurrentCart.PaymentDue - .01m,
                                //TotalAmount = dupTransaction ? CurrentCart.TotalAmount : CurrentCart.TotalAmount - .01m,
                                PaymentDue = CurrentCart.PaymentDue,
                                TotalAmount = CurrentCart.TotalAmount,
                                AspNetUserID = CurrentCart.AspNetUserID.Value,
                                AsscCartID = CurrentCart.CartID,
                                CreatedBy = (Guid) Helpers.LoggedUser.ProviderUserKey,
                                CreatedDate = DateTime.Now,
                                Description =
                                    "Cart Order Payment - Purchase Number: " + CurrentCart.PurchaseNumber.ToString(),
                                TransactionTypeID = (int) Enums.LedgerTransactionType.Purchase
                            };

                            if (CurrentCart.IsTestOrder)
                                ledger.Description += " - Test Mode";

                            if (CurrentCart.CreditAppliedToBalance > 0)
                            {
                                profile.AccountBalance = profile.AccountBalance - CurrentCart.CreditAppliedToBalance;
                                ledger.CreditFromBalance = CurrentCart.CreditAppliedToBalance;
                            }

                            hccLedger lastLedger =
                                hccLedger.GetByMembershipID(profile.MembershipID, null)
                                    .OrderByDescending(a => a.CreatedDate)
                                    .FirstOrDefault();
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
                                    CartId = cartId,
                                    MembershipId = profile.MembershipID,
                                    LedgerId = ledger.LedgerID,
                                    AccountBalance = profile.AccountBalance,
                                    AuthNetProfileId = profile.AuthNetProfileID,
                                    CreatedBy = (Guid) Helpers.LoggedUser.ProviderUserKey,
                                    CreatedDate = DateTime.Now,
                                    DefaultCouponId = profile.DefaultCouponId,
                                    Email = profile.ASPUser.Email,
                                    FirstName = profile.FirstName,
                                    LastName = profile.LastName,
                                    ProfileName = profile.ProfileName,
                                    AuthNetPaymentProfileId =
                                        (isAuthNet == true ? activePaymentProfile.AuthNetPaymentProfileID : string.Empty),
                                    CardTypeId = (isAuthNet == true ? activePaymentProfile.CardTypeID : 0),
                                    CCLast4 = (isAuthNet == true ? activePaymentProfile.CCLast4 : string.Empty),
                                    ExpMon = (isAuthNet == true ? activePaymentProfile.ExpMon : 0),
                                    ExpYear = (isAuthNet == true ? activePaymentProfile.ExpYear : 0),
                                    NameOnCard = (isAuthNet == true ? activePaymentProfile.NameOnCard : string.Empty)
                                };
                                snap.Save();

                                hccUserProfile parentProfile =
                                    hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                                if (parentProfile.BillingAddressID.HasValue)
                                {
                                    billAddr = hccAddress.GetById(parentProfile.BillingAddressID.Value);
                                }

                                hccAddress snapBillAddr = new hccAddress
                                {
                                    Address1 = billAddr.Address1,
                                    Address2 = billAddr.Address2,
                                    AddressTypeID = (int) Enums.AddressType.BillingSnap,
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

                                cartItems.ToList().ForEach(delegate(hccCartItem ci)
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
                                            AddressTypeID = (int) Enums.AddressType.ShippingSnap,
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


                                try
                                {
                                    Email.EmailController ec = new Email.EmailController();
                                    ec.SendMail_OrderConfirmationMerchant(profile.FirstName + " " + profile.LastName,
                                        CurrentCart.ToHtml(), cartId);
                                    ec.SendMail_OrderConfirmationCustomer(profile.ASPUser.Email,
                                        profile.FirstName + " " + profile.LastName, CurrentCart.ToHtml());
                                }
                                catch (Exception ex)
                                {
                                    BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Send Mail Failed", this, ex);
                                } //throw; }

                                //if (IsForPublic)
                                //{
                                //    Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                                //        CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                                //        billAddr.City, billAddr.State, billAddr.Country), false);
                                //}
                                //else
                                //{
                                //    CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                                //    CurrentCartId = CurrentCart.CartID;

                                //    pnlCartDisplay.Visible = true;
                                //    pnlConfirm.Visible = false;

                                //    Clear();
                                //    Bind();
                                //}
                                //OnCartSaved(new CartEventArgs(CurrentCartId));
                            }
                        }
                        //else
                        //{
                        //    BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise("Duplicate transaction attempted: " + CurrentCart.PurchaseNumber.ToString(), this, new Exception("Duplicate transaction attempted by:" + Helpers.LoggedUser.UserName));
                        //}
                    }
                    else
                    {
                        Response.Redirect("~/login.aspx", true);
                    }
                }
                //else
                //{
                //if (IsForPublic)
                //{
                //    //Response.Redirect("~/cart/order-confirmation.aspx?cid=" + CurrentCartId.ToString(), false);
                //    Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                //                    CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                //                    billAddr.City, billAddr.State, billAddr.Country), false);
                //}
                //else
                //{
                //    CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                //    CurrentCartId = CurrentCart.CartID;

                //    pnlCartDisplay.Visible = true;
                //    pnlConfirm.Visible = false;

                //    Clear();
                //    Bind();

                //    OnCartSaved(new CartEventArgs(CurrentCartId));
                //}
                //}

            }
            catch (Exception ex)
            {
                BayshoreSolutions.WebModules.WebModulesAuditEvent.Raise(ex.Data + " " + ex.InnerException, this,
                    new Exception("Recurring order error in method ProcessNewOrder: " + Helpers.LoggedUser.UserName));
            }
        }

        public decimal GetProfileTaxRate(hccAddress shippAddress)
        {
            var taxRate = 0.00m;
            // if shipping address is FL, add tax for taxable items
            if (shippAddress != null && shippAddress.State == "FL")
            {
                TaxLookup taxInfo = TaxLookup.RequestTax(shippAddress.PostalCode);
                if (!taxInfo.State.Contains("Error"))
                {
                    taxRate = decimal.Parse(taxInfo.Sales_Tax_Rate);
                    return taxRate;
                }
                return taxRate;
            }
            return taxRate;
        }
    }
}