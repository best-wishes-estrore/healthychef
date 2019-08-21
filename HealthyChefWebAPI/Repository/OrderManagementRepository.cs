using HealthyChef.DAL;
using HealthyChefWebAPI.Constants;
using HealthyChefWebAPI.CustomModels;
using HealthyChefWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using static HealthyChefWebAPI.Helpers.Enums;

namespace HealthyChefWebAPI.Repository
{
    public class OrderManagementRepository
    {
        public static string GetAllPurchases()
        {
            try
            {
                List<PurchaseDetails> retVals = new List<PurchaseDetails>();

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(SPs.GETALLPURCHASES, conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new PurchaseDetails()
                            {
                                PurchaseNumber = DBUtil.GetIntField(t, "PurchaseNumber"),
                                CartID = DBUtil.GetIntField(t, "CartID"),
                                AspNetUserID = DBUtil.GetGuidField(t, "AspNetUserID"),
                                FirstName = DBUtil.GetCharField(t, "FirstName"),
                                LastName = DBUtil.GetCharField(t, "LastName"),
                                Email = DBUtil.GetCharField(t, "Email"),
                                PurchaseDate = DBUtil.GetCharField(t, "PurchaseDate"),
                                StatusID = DBUtil.GetIntField(t, "StatusID"),
                                TotalAmount = DBUtil.GetDecimalField(t, "TotalAmount"),
                                DeliveryDate = DBUtil.GetCharField(t, "DeliveryDate"),
                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderByDescending(d => d.PurchaseNumber).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception E)
            {
                return string.Empty;
            }

        }



        public static string GetOrderFullfillment()
        {
            try
            {
                List<OrderFullFillMent> retVals = new List<OrderFullFillMent>();

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GETORDERFULLFILLMENT", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new OrderFullFillMent()
                            {
                                CartItemID = DBUtil.GetIntField(t, "CartItemID"),
                                CartID = DBUtil.GetIntField(t, "CartID"),
                                UserProfileID = DBUtil.GetIntField(t, "UserProfileID"),
                                ItemTypeID = DBUtil.GetIntField(t, "ItemTypeID"),
                                ItemName = DBUtil.GetCharField(t, "ItemName"),
                                ItemDesc = DBUtil.GetCharField(t, "ItemDesc"),
                                ItemPrice = DBUtil.GetDecimalField(t, "ItemPrice"),
                                Quantity = DBUtil.GetIntField(t, "Quantity"),
                                IsTaxable = DBUtil.GetBoolField(t, "IsTaxable"),
                                OrderNumber = DBUtil.GetCharField(t, "OrderNumber"),
                                DeliveryDate = DBUtil.GetDateTimeField(t, "DeliveryDate"),
                                Gift_RedeemCode = DBUtil.GetCharField(t, "Gift_RedeemCode"),
                                Gift_IssuedTo = DBUtil.GetGuidField(t, "Gift_IssuedTo"),
                                Gift_IssuedDate = DBUtil.GetDateTimeField(t, "Gift_IssuedDate"),
                                Gift_RedeemedBy = DBUtil.GetGuidField(t, "Gift_RedeemedBy"),
                                Gift_RedeemedDate = DBUtil.GetDateTimeField(t, "Gift_RedeemedDate"),
                                Gift_RecipientAddressId = DBUtil.GetIntField(t, "Gift_RecipientAddressId"),
                                Gift_RecipientEmail = DBUtil.GetCharField(t, "Gift_RecipientEmail"),
                                Gift_RecipientMessage = DBUtil.GetCharField(t, "Gift_RecipientMessage"),
                                Meal_MenuItemID = DBUtil.GetIntField(t, "Meal_MenuItemID"),
                                Meal_MealSizeID = DBUtil.GetIntField(t, "Meal_MealSizeID"),
                                Meal_ShippingCost = DBUtil.GetDecimalField(t, "Meal_ShippingCost"),
                                Plan_PlanID = DBUtil.GetIntField(t, "Plan_PlanID"),
                                Plan_ProgramOptionID = DBUtil.GetIntField(t, "Plan_ProgramOptionID"),
                                Plan_IsAutoRenew = DBUtil.GetBoolField(t, "Plan_IsAutoRenew"),
                                CreatedBy = DBUtil.GetGuidField(t, "CreatedBy"),
                                CreatedDate = DBUtil.GetDateTimeField(t, "CreatedDate"),
                                IsCompleted = DBUtil.GetBoolField(t, "IsCompleted"),
                                IsCancelled = DBUtil.GetBoolField(t, "IsCancelled"),
                                IsFulfilled = DBUtil.GetBoolField(t, "IsFulfilled"),
                                DiscountPerEach = DBUtil.GetDecimalField(t, "DiscountPerEach"),
                                DiscountAdjPrice = DBUtil.GetDecimalField(t, "DiscountAdjPrice"),
                                SnapBillAddrId = DBUtil.GetIntField(t, "SnapBillAddrId"),
                                SnapShipAddrId = DBUtil.GetIntField(t, "SnapShipAddrId"),
                                TaxRate = DBUtil.GetDecimalField(t, "TaxRate"),
                                TaxableAmount = DBUtil.GetDecimalField(t, "TaxableAmount"),
                                DiscretionaryTaxAmount = DBUtil.GetDecimalField(t, "DiscretionaryTaxAmount"),
                                TaxRateAssigned = DBUtil.GetDecimalField(t, "TaxRateAssigned")

                            });
                        }
                        conn.Close();
                    }
                }

                retVals = retVals.OrderByDescending(d => d.OrderNumber).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return string.Empty;
            }

        }

        /// <summary>
        /// New functionality for order-fullfillment using DAL
        /// </summary>
        /// <param name="lastName"></param>
        /// <param name="email"></param>
        /// <param name="purchNum"></param>
        /// <param name="deliveryDate"></param>
        /// <param name="includeSnapshots"></param>
        /// <param name="includeGiftCerts"></param>
        /// <returns></returns>
        public static string GetOrderFullfillmentDetails(string deliveryDateString = "")
        {
            string Content = string.Empty;
            List<OrderFullFillMentDetails> retVals = new List<OrderFullFillMentDetails>();

            string lastName = string.Empty;
            string email = string.Empty;
            int? purchNum = null;
            bool includeSnapshots = false;
            bool includeGiftCerts = true;

            DateTime? deliveryDate = null;
            if (!string.IsNullOrEmpty(deliveryDateString))
            {
                DateTime d = new DateTime();
                bool _isParsed = DateTime.TryParse(deliveryDateString.Trim(), out d);
                if (_isParsed)
                {
                    deliveryDate = d;
                }
            }

            try
            {
                using (var cont = new healthychefEntities())
                {
                    List<AggrCartItem> aggrItems = new List<AggrCartItem>();

                    List<hccCartItem> alcgc;

                    if (includeGiftCerts)
                        alcgc = cont.hcc_OrderFulfillSearch_ALCnGC(deliveryDate, purchNum, lastName, email).Select(x => new hccCartItem
                        {

                            CartItemID = x.CartItemID,
                            CartID = Convert.ToInt32(x.CartID),
                            UserProfileID = Convert.ToInt32(x.UserProfileID),
                            ItemTypeID = Convert.ToInt32(x.ItemTypeID),
                            ItemName = x.ItemName,
                            ItemDesc = x.ItemDesc,
                            ItemPrice = Convert.ToDecimal(x.ItemPrice),
                            Quantity = Convert.ToInt32(x.Quantity),
                            IsTaxable = Convert.ToBoolean(x.IsTaxable),
                            OrderNumber = x.OrderNumber,
                            DeliveryDate = Convert.ToDateTime(x.DeliveryDate),
                            Gift_RedeemCode = x.Gift_RedeemCode,
                            Gift_IssuedTo = x.Gift_IssuedTo,
                            Gift_IssuedDate = x.Gift_IssuedDate,
                            Gift_RedeemedBy = x.Gift_RedeemedBy,
                            Gift_RedeemedDate = x.Gift_RedeemedDate,
                            Gift_RecipientAddressId = x.Gift_RecipientAddressId,
                            Gift_RecipientEmail = x.Gift_RecipientEmail,
                            Gift_RecipientMessage = x.Gift_RecipientMessage,
                            Meal_MenuItemID = x.Meal_MenuItemID,
                            Meal_MealSizeID = x.Meal_MealSizeID,
                            Meal_ShippingCost = x.Meal_ShippingCost,
                            Plan_PlanID = x.Plan_PlanID,
                            Plan_ProgramOptionID = x.Plan_ProgramOptionID,
                            Plan_IsAutoRenew = x.Plan_IsAutoRenew,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = Convert.ToDateTime(x.CreatedDate),
                            IsCompleted = Convert.ToBoolean(x.IsCompleted),
                            IsCancelled = Convert.ToBoolean(x.IsCancelled),
                            IsFulfilled = Convert.ToBoolean(x.IsFulfilled),
                            DiscountPerEach = Convert.ToDecimal(x.DiscountPerEach),
                            DiscountAdjPrice = Convert.ToDecimal(x.DiscountAdjPrice),
                            SnapBillAddrId = x.SnapBillAddrId,
                            SnapShipAddrId = x.SnapShipAddrId,
                            TaxRate = x.TaxRate,
                            TaxableAmount = x.TaxableAmount,
                            DiscretionaryTaxAmount = x.DiscretionaryTaxAmount,
                            TaxRateAssigned = x.TaxRateAssigned
                        }).ToList();
                    else
                        alcgc = cont.hcc_OrderFulfillSearch_ALCnGC(deliveryDate, purchNum, lastName, email).ToList().Where(a => a.ItemTypeID != Convert.ToInt32(Enums.CartItemType.GiftCard)).Select(x => new hccCartItem
                        {

                            CartItemID = x.CartItemID,
                            CartID = Convert.ToInt32(x.CartID),
                            UserProfileID = Convert.ToInt32(x.UserProfileID),
                            ItemTypeID = Convert.ToInt32(x.ItemTypeID),
                            ItemName = x.ItemName,
                            ItemDesc = x.ItemDesc,
                            ItemPrice = Convert.ToDecimal(x.ItemPrice),
                            Quantity = Convert.ToInt32(x.Quantity),
                            IsTaxable = Convert.ToBoolean(x.IsTaxable),
                            OrderNumber = x.OrderNumber,
                            DeliveryDate = Convert.ToDateTime(x.DeliveryDate),
                            Gift_RedeemCode = x.Gift_RedeemCode,
                            Gift_IssuedTo = x.Gift_IssuedTo,
                            Gift_IssuedDate = x.Gift_IssuedDate,
                            Gift_RedeemedBy = x.Gift_RedeemedBy,
                            Gift_RedeemedDate = x.Gift_RedeemedDate,
                            Gift_RecipientAddressId = x.Gift_RecipientAddressId,
                            Gift_RecipientEmail = x.Gift_RecipientEmail,
                            Gift_RecipientMessage = x.Gift_RecipientMessage,
                            Meal_MenuItemID = x.Meal_MenuItemID,
                            Meal_MealSizeID = x.Meal_MealSizeID,
                            Meal_ShippingCost = x.Meal_ShippingCost,
                            Plan_PlanID = x.Plan_PlanID,
                            Plan_ProgramOptionID = x.Plan_ProgramOptionID,
                            Plan_IsAutoRenew = x.Plan_IsAutoRenew,
                            CreatedBy = x.CreatedBy,
                            CreatedDate = Convert.ToDateTime(x.CreatedDate),
                            IsCompleted = Convert.ToBoolean(x.IsCompleted),
                            IsCancelled = Convert.ToBoolean(x.IsCancelled),
                            IsFulfilled = Convert.ToBoolean(x.IsFulfilled),
                            DiscountPerEach = Convert.ToDecimal(x.DiscountPerEach),
                            DiscountAdjPrice = Convert.ToDecimal(x.DiscountAdjPrice),
                            SnapBillAddrId = x.SnapBillAddrId,
                            SnapShipAddrId = x.SnapShipAddrId,
                            TaxRate = x.TaxRate,
                            TaxableAmount = x.TaxableAmount,
                            DiscretionaryTaxAmount = x.DiscretionaryTaxAmount,
                            TaxRateAssigned = x.TaxRateAssigned
                        }).ToList();

                    aggrItems.AddRange(AggrCartItem.GetFromRange(alcgc, includeSnapshots));

                    List<hccCartItemCalendar> progs = cont.hcc_OrderFulfillSearch_Programs(deliveryDate, purchNum, lastName, email).ToList();

                    aggrItems.AddRange(AggrCartItem.GetFromRange(progs, includeSnapshots));

                    aggrItems = aggrItems.OrderBy(a => a.CartItem.OrderNumber).ThenBy(a => a.DeliveryDate).ToList();

                    foreach (var ai in aggrItems)
                    {
                        OrderFullFillMentDetails _ofd = new OrderFullFillMentDetails();
                        _ofd.OrderNum = ai.CartItem.OrderNumber;
                        _ofd.CustomerName = ai.CartItem.UserProfile.ParentProfileName;
                        _ofd.Type = ai.SimpleName;
                        _ofd.Quantity = ai.TotalQuantity;
                        _ofd.DeliveryDateObj = ai.DeliveryDate;
                        _ofd.Status = ai.SimpleStatus;
                        _ofd.ItemType = ai.CartItem.ItemType;
                        _ofd.CartItemID = ai.CartItem.CartItemID;
                        _ofd.CartID = ai.CartItem.CartID;

                        retVals.Add(_ofd);
                    }

                    Content = DBHelper.ConvertDataToJson(retVals);
                }
            }
            catch (Exception ex)
            {
                return "";
            }

            return Content;
        }



        public static string GetRecurringOrder()
        {
            try
            {
                List<RecurringOrder> retVals = new List<RecurringOrder>();

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GetRecurringOrder", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new RecurringOrder()
                            {
                                cartid = DBUtil.GetIntField(t, "cartid"),
                                cartitemid = DBUtil.GetIntField(t, "cartitemid"),
                                userprofileid = DBUtil.GetIntField(t, "userprofileid"),
                                aspnetuserid = DBUtil.GetGuidField(t, "aspnetuserid"),
                                ProfileName = DBUtil.GetCharField(t, "ProfileName"),
                                CustomerName = DBUtil.GetCharField(t, "CustomerName"),
                                ItemName = DBUtil.GetCharField(t, "ItemName"),
                                purchasenumber = DBUtil.GetIntField(t, "purchasenumber"),
                                maxdeliverydate = DBUtil.GetCharField(t, "maxdeliverydate"),
                                maxcutoffdate = DBUtil.GetCharField(t, "maxcutoffdate"),
                                quantity = DBUtil.GetIntField(t, "quantity")

                            });
                        }
                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(d => d.maxdeliverydate).ToList();

                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception Ex)
            {
                return string.Empty;
            }

        }

        public static string GetCancellation(int PurchaseNumber)
        {
            try
            {
                List<Cancellations> retVals = new List<Cancellations>();
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("GETCANCELLATIONSORDER", conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@PURCHASENUMBER", PurchaseNumber);
                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new Cancellations()
                            {
                                ProfileName = DBUtil.GetCharField(t, "ProfileName"),
                                ItemName = DBUtil.GetCharField(t, "ItemName"),
                                Quantity = DBUtil.GetIntField(t, "Quantity"),
                                DeliveryDate = DBUtil.GetDateField(t, "DeliveryDate"),
                                IsCancelled = DBUtil.GetBoolField(t, "IsCancelled"),
                                OrderNumber = DBUtil.GetCharField(t, "OrderNumber"),
                                ItemCount = DBUtil.GetIntField(t, "ItemCount"),
                                StatusId = DBUtil.GetIntField(t, "StatusId")

                            });

                        }
                        conn.Close();
                    }
                }
                retVals = retVals.OrderByDescending(d => d.OrderNumber).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static PostHttpResponse CancelCartItems(int PurchaseNumber, string OrderNumber)
        {
            var res = new PostHttpResponse();
            try
            {
                hccCart cart = hccCart.GetBy(PurchaseNumber);
                bool isCancel = false;
                if (cart != null)
                {
                    List<hccCartItem> cartitems = hccCartItem.GetBy(cart.CartID);

                    List<hccCartItem> ordItems = cartitems.Where(a => a.OrderNumber == OrderNumber).ToList();
                    ordItems.ForEach(delegate (hccCartItem item) { item.IsCancelled = true; item.Save(); });

                    if (cartitems.Count(a => !a.IsCancelled) == 0)
                    {
                        cart.StatusID = (int)CartStatus.Cancelled;
                        cart.Save();
                        isCancel = true;
                    }
                    else
                    {
                        cart.StatusID = (int)CartStatus.Paid;
                        cart.Save();
                        isCancel = true;
                    }
                }
                if (isCancel)
                {
                    res.IsSuccess = true;
                    res.StatusCode = System.Net.HttpStatusCode.OK;
                    res.Message = "Successfully cancel the cartitems";
                }
                else
                {
                    res.Message = "No cart items found for the order " + OrderNumber;
                }

            }
            catch (Exception ex)
            {
                res.Message = "Error in cancel the cartitems : " + Environment.NewLine + ex.Message;
            }
            return res;
        }

        public static string CartItemsDetails(string OrderNumber)
        {
            List<CartItem> _cartitemsdetailsadded = new List<CartItem>();
            try
            {
                if (OrderNumber != null)
                {
                    List<hccCartItem> cartItems = hccCartItem.GetBy(OrderNumber);
                    if (cartItems.Count > 0)
                    {
                        foreach (var cartitem in cartItems)
                        {
                            CartItem cartItemResponse = new CartItem();
                            var Getprofilename = hccUserProfile.GetBy(cartitem.CartItemID);
                            if (Getprofilename != null)
                            {
                                cartItemResponse.ProfileName = Getprofilename.ProfileName;
                            }
                            else
                            {
                                cartItemResponse.ProfileName = "";
                            }
                            cartItemResponse.ItemName = cartitem.ItemName;
                            cartItemResponse.Quantity = cartitem.Quantity;
                            cartItemResponse.DeliveryDate = cartitem.DeliveryDate;
                            cartItemResponse.IsCancelled = cartitem.IsCancelled;
                            cartItemResponse.CartItemID = cartitem.CartItemID;
                            cartItemResponse.CartID = cartitem.CartID;
                            _cartitemsdetailsadded.Add(cartItemResponse);
                        }
                    }
                }
                _cartitemsdetailsadded = _cartitemsdetailsadded.OrderByDescending(d => d.OrderNumber).ToList();

                return DBHelper.ConvertDataToJson(_cartitemsdetailsadded);
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public static PostHttpResponse CancelItemDetails(int cancelcartitemid)
        {
            var res = new PostHttpResponse();
            try
            {
                hccCartItem item = hccCartItem.GetById(cancelcartitemid);
                if (item != null)
                {

                    item.IsCancelled = true;
                    item.Save();

                    res.IsSuccess = true;
                    res.StatusCode = System.Net.HttpStatusCode.OK;
                    res.Message = "Successfully cancelled the cartitem";
                }
                else
                {
                    res.Message = "No cart item found with id " + cancelcartitemid;
                }
            }
                
            catch (Exception ex)
            {
                res.Message = "Error in cancel the cartitem : " + Environment.NewLine + ex.Message;
            }
            return res;
        }

        public static PostHttpResponse CancelAutorenew(int cartitemid, int cartid)
        {
            var res = new PostHttpResponse();
            bool IsDeleted = false;
            try
            {
                using (var hcE = new healthychefEntities())
                {
                    var rOrder = hcE.hccRecurringOrders.FirstOrDefault(i => i.CartID == cartid && i.CartItemID == cartitemid);
                    if (rOrder != null)
                    {

                        hcE.hccRecurringOrders.DeleteObject(rOrder);
                        hcE.SaveChanges();
                        IsDeleted = true;
                    }
                }
                if (IsDeleted)
                {
                    res.IsSuccess = true;
                    res.StatusCode = System.Net.HttpStatusCode.OK;
                    res.Message = "Successfully cancel auto re-new";
                }
                else
                {
                    res.Message = "No records found to auto renew";
                }
            }
            catch (Exception ex)
            {
                res.Message = "Error in cancel auto re-new : " + Environment.NewLine + ex.Message;
            }
            return res;
        }


        public static PostHttpResponse PurchaseDetails(int Cartid)
        {
            var _res = new PostHttpResponse();
            hccCart cart = hccCart.GetById(Cartid);
            try
            {
                if (cart != null)
                {
                    _res.StatusCode = System.Net.HttpStatusCode.OK;
                    _res.IsSuccess = true;
                    _res.Message = cart.ToHtml();
                }
                else
                {
                    _res.Message = "No records found for cart id : " + Cartid;
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in getting cart details : " + Environment.NewLine + ex.Message;
            }
            return _res;
        }

        public static PostHttpResponse PostponeOrderFullfilment(int cartItemID, string delDateStr)
        {
            var _res = new PostHttpResponse();
            try
            {
                int cartItemId = cartItemID;
                DateTime delDate = DateTime.Parse(delDateStr);

                hccCartItemCalendar oldCartCal = hccCartItemCalendar.GetBy(cartItemId, delDate);

                if (oldCartCal != null)
                {
                    // remove any pre-defined program exceptions, as they are tied to a specific defaultmenu, re: delivery date
                    List<hccCartDefaultMenuException> defExs = hccCartDefaultMenuException.GetBy(oldCartCal.CartCalendarID);
                    defExs.ForEach(delegate (hccCartDefaultMenuException defEx)
                    {
                        List<hccCartDefaultMenuExPref> prefs = hccCartDefaultMenuExPref.GetBy(defEx.DefaultMenuExceptID);
                        prefs.ForEach(a => a.Delete());
                        defEx.Delete();
                    });

                    var otherCals = hccCartItemCalendar.GetByCartItemID(cartItemId);
                    var lastCal = otherCals.Last();

                    hccCartItemCalendar newCartCal = lastCal.GetNextCartCalendar(DayOfWeek.Friday);

                    if (newCartCal != null)
                        oldCartCal.Delete();

                    _res.StatusCode = System.Net.HttpStatusCode.OK;
                    _res.IsSuccess = true;
                    _res.Message = "Successfully postponed the order";
                }
                else
                {
                    _res.Message = "No order found for the given cartItemId";
                }
            }
            catch (Exception ex)
            {
                _res.Message = "Error while postponing Order :" + Environment.NewLine + ex.Message;
            }

            return _res;
        }
    }
}