using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HealthyChef.AuthNet;
using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChef.WebModules.ShoppingCart.Admin.UserControls;
using HealthyChefCreationsMVC.CustomModels;
using PayPal.Api;

namespace HealthyChefCreationsMVC.Controllers
{
    public class PaypalController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            //getting the apiContext
            //ExceptionLogging.ProcessingFalied("step1");
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            bool _isPurchaseCompleted = false;
            bool isLoggedIn = true;
            string _message = "";
            var CurrentCart = new hccCart();
            bool AuthNetSuccessful = false;
            bool IsForPublic = true;
            int CurrentCartId = 0;
            var executedPayment = new Payment();
            try
            {

                //ExceptionLogging.ProcessingFalied("step2");
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
                            profile.CanyonRanchCustomer = true;
                            profile.Save();
                            AuthNetConfig ANConfig = new AuthNetConfig();
                            bool isDuplicateTransaction = false;
                            bool isAuthNet = false;

                            if (ANConfig.Settings.TestMode)
                                CurrentCart.IsTestOrder = true;

                            // Check for existing account balance, calculate total balance
                            if (CurrentCart.PaymentDue > 0.00m)
                            {
                                try
                                {
                                    //A resource representing a Payer that funds a payment Payment Method as paypal
                                    //Payer Id will be returned when payment proceeds or click to pay
                                    string payerId = Request.Params["PayerID"];

                                    if (string.IsNullOrEmpty(payerId))
                                    {
                                        ExceptionLogging.ProcessingFalied("step3");
                                        //this section will be executed first because PayerID doesn't exist
                                        //it is returned by the create function call of the payment class

                                        // Creating a payment
                                        // baseURL is the url on which paypal sendsback the data.
                                        string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/PaymentWithPaypal.aspx?";
                                                 // "/Paypal/PaymentWithPayPal?";
                                        //ExceptionLogging.ProcessingFalied("step4" + baseURI);
                                        //here we are generating guid for storing the paymentID received in session
                                        //which will be used in the payment execution

                                        var guid = Convert.ToString((new Random()).Next(100000));
                                        //CreatePayment function gives us the payment approval url
                                        //on which payer is redirected for paypal account payment

                                        var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);

                                        //get links returned from paypal in response to Create function call
                                        //ExceptionLogging.ProcessingFalied("step8");
                                        var links = createdPayment.links.GetEnumerator();

                                        string paypalRedirectUrl = null;

                                        while (links.MoveNext())
                                        {
                                            Links lnk = links.Current;

                                            if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                                            {
                                                //saving the payapalredirect URL to which user will be redirected for payment
                                                paypalRedirectUrl = lnk.href;
                                            }
                                        }

                                        // saving the paymentID in the key guid
                                        Session.Add(guid, createdPayment.id);
                                        //ExceptionLogging.ProcessingFalied("step9");
                                        return Redirect(paypalRedirectUrl);
                                    }
                                    else
                                    {

                                        // This function exectues after receving all parameters for the payment

                                        var guid = Request.Params["guid"];

                                        executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                                        //ExceptionLogging.ProcessingFalied("step9" + executedPayment.state.ToLower());
                                        //If executed payment failed then we will show payment failure message to user
                                        if (executedPayment.state.ToLower() != "approved")
                                        {
                                            return View("FailureView");
                                        }
                                    }


                                    if (executedPayment.state.ToLower() == "approved")
                                    {
                                        CurrentCart.AuthNetResponse = "payerid" + payerId + "|" + "State" + executedPayment.state.ToString()
                                               + "|" + "tokenid" + executedPayment.token + "|" + "TransactionId:" + Request.Params["paymentId"] + "|" + executedPayment.create_time;

                                        CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.ModifiedDate = DateTime.Now;
                                        CurrentCart.PurchaseBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                        CurrentCart.PurchaseDate = DateTime.Now;
                                        CurrentCart.PaymentProfileID = 0;
                                        CurrentCart.StatusID = (int)Enums.CartStatus.Paid;

                                        isAuthNet = true;
                                        AuthNetSuccessful = true;
                                    }
                                    else
                                    {
                                        //_message = executedPayment.failure_reason;
                                    }
                                    CurrentCart.Save();
                                }
                                catch (Exception ex)
                                {
                                    //ExceptionLogging.SendErrorToText(ex);
                                    _message = ex.Message;
                                    if (ex is InvalidOperationException)
                                    {
                                        if (CurrentCart.IsTestOrder)
                                        {
                                            CurrentCart.ModifiedBy = (Guid)Helpers.LoggedUser.ProviderUserKey;
                                            CurrentCart.ModifiedDate = DateTime.Now;
                                            CurrentCart.PaymentProfileID = 0;
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
                                CurrentCart.PaymentProfileID = 0;
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
                                        ProfileName = profile.ProfileName
                                    };
                                    snap.Save();

                                    hccUserProfile parentProfile = hccUserProfile.GetParentProfileBy(CurrentCart.AspNetUserID.Value);
                                    if (parentProfile.BillingAddressID.HasValue)
                                    {
                                        billAddr = hccAddress.GetById(parentProfile.BillingAddressID.Value);
                                    }
                                    hccAddress snapBillAddr = new hccAddress();
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
                                            ProfileName = parentProfile.ProfileName
                                        };
                                        snapBillAddr.Save();
                                    }

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
                            Response.Redirect("~/cart/order-confirmation.aspx?cid=" + CurrentCartId.ToString(), false);
                            Response.Redirect(string.Format("~/cart/order-confirmation.aspx?pn={0}&tl={1}&tx={2}&ts={3}&ct={4}&st={5}&cy={6}",
                                            CurrentCart.PurchaseNumber, CurrentCart.TotalAmount, CurrentCart.TaxableAmount, CurrentCart.ShippingAmount,
                                            billAddr.City, billAddr.State, billAddr.Country), false);
                        }
                        else
                        {
                            CurrentCart = hccCart.GetCurrentCart(profile.ASPUser);
                            CurrentCartId = CurrentCart.CartID;

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);
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
                return View("FailureView");
            }

            //on successful payment, show success page to user.
            //return View("SuccessView");
            //return View("~/Cart/ThankYou/?purchaseNum=" + CurrentCart.PurchaseNumber);
            return Redirect("/ThankYou.aspx?purchaseNum="+CurrentCart.PurchaseNumber+"");

        }

        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, paymentExecution);
        }

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            try
            {
                //ExceptionLogging.ProcessingFalied("step5");
                var CurrentCart = new hccCart();
                int CurrentCartId = 0;
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
                    var listofcartitems = hccCartItem.GetBy(CurrentCartId);
                    decimal totalamount = 0;
                    //create itemlist and add item objects to it
                    var itemList = new ItemList() { items = new List<Item>() };

                    //Adding Item Details like name, currency, price etc
                    //foreach (var cartitem in listofcartitems)
                    //{
                    //    decimal discountforautorenew = 0;
                    //    decimal subtotalcartitemdiscount = CalculateDiscountForItemByCart(CurrentCart, cartitem, CurrentCart.SubTotalAmount);
                    //    if (cartitem.Plan_IsAutoRenew ==true)
                    //    {
                    //         discountforautorenew = Math.Round(Convert.ToDecimal((Convert.ToDouble(cartitem.ItemPrice) * cartitem.Quantity) * 0.05), 2);
                    //    }
                    //    else
                    //    {
                    //        discountforautorenew = 0;
                    //    }
                    //    itemList.items.Add(new Item()
                    //    {
                    //        name = cartitem.ItemName,
                    //        currency = "USD",
                    //        price = cartitem.DiscountAdjPrice.ToString("0.00")=="0.00"?subtotalcartitemdiscount.ToString("0.00") : (cartitem.ItemPrice-subtotalcartitemdiscount- discountforautorenew).ToString("0.00"),
                    //        quantity = cartitem.Quantity.ToString(),
                    //        sku = cartitem.SimpleName
                    //    });
                    //    totalamount = totalamount + cartitem.ItemPrice;
                    //}
                    decimal acctBalance = 0.00m;
                    var parentprofile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);
                    if (parentprofile != null)
                    {
                        acctBalance = parentprofile != null ? parentprofile.AccountBalance : 0.00m;
                    }
                    itemList.items.Add(new Item()
                    {
                        name = "Healthychef creations order",
                        currency = "USD",
                        price = (CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount - acctBalance).ToString("0.00"),
                        quantity = 1.ToString(),
                        sku = "HCC Orders"
                    });
                    var payer = new Payer() { payment_method = "paypal" };

                    // Configure Redirect Urls here with RedirectUrls object
                    var redirUrls = new RedirectUrls()
                    {
                        cancel_url = redirectUrl + "&Cancel=true",
                        return_url = redirectUrl
                    };
                    
                        // Adding Tax, shipping and Subtotal details
                        var details = new Details()
                    {
                        tax = CurrentCart.TaxAmount.ToString("0.00"),
                        shipping = CurrentCart.ShippingAmount.ToString("0.00"),
                        subtotal = (CurrentCart.SubTotalAmount - CurrentCart.SubTotalDiscount- acctBalance).ToString("0.00")
                    };

                    //Final amount with details
                    var amount = new Amount()
                    {
                        currency = "USD",
                        total = CurrentCart.PaymentDue.ToString("0.00"), // Total must be equal to sum of tax, shipping and subtotal.
                        details = details
                    };
                    //ExceptionLogging.ProcessingFalied("step6");
                    var transactionList = new List<Transaction>();
                    // Adding description about the transaction
                    transactionList.Add(new Transaction()
                    {
                        description = "Healthy Chef Creations Purchase #" + CurrentCart.PurchaseNumber.ToString(),
                        invoice_number = Guid.NewGuid().ToString() + "_PaypalPayment", //Generate an Invoice No
                        amount = amount,
                        item_list = itemList
                    });


                    this.payment = new Payment()
                    {
                        intent = "sale",
                        payer = payer,
                        transactions = transactionList,
                        redirect_urls = redirUrls
                    };
                }
                //ExceptionLogging.ProcessingFalied("step7");
                // Create a payment using a APIContext
                return this.payment.Create(apiContext);

            }
            catch (Exception ex)
            {
                //ExceptionLogging.SendErrorToText(ex);

                return null;
            }

        }
        public static decimal CalculateDiscountForItemByCart(hccCart cart, hccCartItem cartItem, decimal totalNA)
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

                                //discountRation = Helpers.TruncateDecimal((cartItem.ItemPrice * (curCoupon.Amount / 100)), 2);
                                discountRation = Math.Round((cartItem.ItemPrice * cartItem.Quantity * (curCoupon.Amount / 100)), 2);
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
            return discountRation;
        }

    }
}