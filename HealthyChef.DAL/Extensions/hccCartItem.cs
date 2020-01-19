using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCartItem
    {
        public const string StatusComplete = "Complete";
        public const string StatusIncomplete = "Incomplete";
        public const string StatusCancelled = "Cancelled";

        public decimal TotalItemPrice
        {
            get
            {
                return this.ItemPrice + MealSideItems.Sum(s => s.ItemPrice);
            }
        }

        public int NumberOfMeals
        {
            get; set;
        }

        public decimal TotalDiscountAdjPrice
        {
            get
            {
                return this.DiscountAdjPrice + MealSideItems.Sum(s => s.DiscountAdjPrice);
            }
        }

        /// <summary>
        /// SubTotal Non-Adjusted - ItemPrice x Quantity
        /// </summary>
        public decimal ItemSubTotalNA
        {
            get
            {
                return Math.Round(TotalItemPrice * this.Quantity, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// SubTotal Non-Adjusted - NoOfMeals x Quantity
        /// </summary>
        public decimal ItemSubMealTotal
        {
            get
            {
                double a = Convert.ToInt32(NumberOfMeals);
                return (int)Math.Round(a * this.Quantity, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// Return "true" if current item is meal side
        /// </summary>
        public bool IsMealSide
        {
            get
            {
                try
                {
                    var cartALCMenuItem = hccCartALCMenuItem.GetByCartItemID(this.CartItemID);
                    return cartALCMenuItem != null && cartALCMenuItem.ParentCartItemID != this.CartItemID;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// SubTotal of DiscountAdjPrice = - DiscountAdjPrice x Quantity
        /// </summary>
        public decimal ItemSubTotalAdj
        {
            get
            {
                return Math.Round(this.TotalDiscountAdjPrice * this.Quantity, 2, MidpointRounding.AwayFromZero);
            }
        }

        /// <summary>
        /// ItemType
        /// </summary>
        public Enums.CartItemType ItemType { get { return (Enums.CartItemType)this.ItemTypeID; } }

        public hccUserProfile UserProfile { get { return hccUserProfile.GetById(this.UserProfileID); } }

        public string SimpleName
        {
            get
            {
                string retVal = string.Empty;

                if (this.ItemType == Enums.CartItemType.AlaCarte)
                    retVal = "A La Carte";

                if (this.ItemType == Enums.CartItemType.DefinedPlan)
                    retVal = hccProgramPlan.GetById(this.Plan_PlanID.Value).Name;

                if (this.ItemType == Enums.CartItemType.GiftCard)
                    retVal = "Gift Certificate";

                return retVal;
            }
        }

        public string SimpleStatus
        {
            get
            {
                string retVal = string.Empty;

                if (IsCompleted)
                    retVal = StatusComplete;
                else
                    retVal = StatusIncomplete;

                if (IsCancelled)
                    retVal = StatusCancelled;

                return retVal;
            }
        }

        private List<hccCartItem> _mealSideItems = null;

        public List<hccCartItem> MealSideItems
        {
            get
            {
                if (_mealSideItems == null)
                {
                    try
                    {
                        using (var cont = new healthychefEntitiesAPI())
                        {
                            _mealSideItems = cont.hccCartALCMenuItems.Where(s => s.ParentCartItemID == this.CartItemID && s.CartItemID != s.ParentCartItemID).OrderBy(s => s.Ordinal).Select(ami => ami.hccCartItem).ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return _mealSideItems;
            }
        }

        private List<hccMenuItem> _mealSideMenuItems = null;

        public List<hccMenuItem> MealSideMenuItems
        {
            get
            {
                if (_mealSideMenuItems == null)
                {
                    try
                    {
                        using (var cont = new healthychefEntitiesAPI())
                        {
                            _mealSideMenuItems = cont.hccCartALCMenuItems
                                .Where(s => s.ParentCartItemID == this.CartItemID && s.CartItemID != s.ParentCartItemID)
                                .OrderBy(s => s.Ordinal)
                                .Join(cont.hccMenuItems,
                                      cmi => cmi.hccCartItem.Meal_MenuItemID,
                                      mi => mi.MenuItemID,
                                      (cmi, mi) => new { CartMenuItem = cmi, MenuItem = mi })
                                .Select(g => g.MenuItem)
                                .ToList();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                return _mealSideMenuItems;
            }
        }

        public string GetMealSide1MenuItemName()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var mealSide1MenuItem = cont.hccCartALCMenuItems
                        .Where(s => s.ParentCartItemID == this.CartItemID && s.CartItemID != s.ParentCartItemID && s.Ordinal == 1)
                        .Join(cont.hccMenuItems,
                              cmi => cmi.hccCartItem.Meal_MenuItemID,
                              mi => mi.MenuItemID,
                              (cmi, mi) => new { CartMenuItem = cmi, MenuItem = mi })
                        .Select(g => g.MenuItem)
                        .FirstOrDefault();

                    if (mealSide1MenuItem == null)
                        return string.Empty;
                    else
                        return mealSide1MenuItem.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetMealSide2MenuItemName()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var mealSide2MenuItem = cont.hccCartALCMenuItems
                        .Where(s => s.ParentCartItemID == this.CartItemID && s.CartItemID != s.ParentCartItemID && s.Ordinal == 2)
                        .Join(cont.hccMenuItems,
                              cmi => cmi.hccCartItem.Meal_MenuItemID,
                              mi => mi.MenuItemID,
                              (cmi, mi) => new { CartMenuItem = cmi, MenuItem = mi })
                        .Select(g => g.MenuItem)
                        .FirstOrDefault();

                    if (mealSide2MenuItem == null)
                        return string.Empty;
                    else
                        return mealSide2MenuItem.Name;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetMealSideMenuItemsAsSectionString(string delimiter)
        {
            var sections = new StringBuilder();

            foreach (var menuItem in MealSideMenuItems)
            {
                sections.Append((string.IsNullOrEmpty(sections.ToString()) ? string.Empty : delimiter) + menuItem.Name);
            }
            return sections.ToString();
        }

        /// <summary>
        /// Saves a shopping cart line to the database.  If line does not exists is created otherwise is updated. 
        /// </summary>
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartItems", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartItems", this);
                    }
                    else
                    {
                        cont.hccCartItems.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deletes the hccCartItem object.
        /// </summary>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns void.</returns>
        public void Delete(List<hccRecurringOrder> recurringItemList)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCartItems", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        hccCartItem cartItem = (hccCartItem)originalItem;

                        if (((Enums.CartItemType)cartItem.ItemTypeID) == Enums.CartItemType.AlaCarte)
                        {
                            List<hccCartItemMealPreference> prefs = hccCartItemMealPreference.GetBy(cartItem.CartItemID);
                            prefs.ForEach(a => a.Delete());
                        }
                        else if (((Enums.CartItemType)cartItem.ItemTypeID) == Enums.CartItemType.DefinedPlan)
                        {
                            List<hccCartItemMealPreference> prefs = hccCartItemMealPreference.GetBy(cartItem.CartItemID);
                            prefs.ForEach(a => a.Delete());

                            List<hccCartItemCalendar> cartCals = hccCartItemCalendar.GetByCartItemID(cartItem.CartItemID);

                            cartCals.ForEach(delegate (hccCartItemCalendar cartCal)
                            {
                                List<hccCartDefaultMenuException> menuExs = hccCartDefaultMenuException.GetBy(cartCal.CartCalendarID);
                                menuExs.ForEach(a => a.Delete());
                            });

                            cartCals.ForEach(delegate (hccCartItemCalendar cartCal)
                            {
                                cartCal.Delete();
                            });
                        }

                        if (HttpContext.Current.Session["id"] != null && HttpContext.Current.Session["meals"] != null)
                        {
                            cartItem.CartID = (int)HttpContext.Current.Session["id"];
                            cartItem.NumberOfMeals = (int)HttpContext.Current.Session["meals"];
                        }

                        cont.hccCartItems.DeleteObject(cartItem);
                        cont.SaveChanges();

                        // Check for recurring items
                        if (recurringItemList != null)
                        {
                            var itemDeleted = recurringItemList.Find(x => x.CartID == this.CartID && x.CartItemID == this.CartItemID);
                            recurringItemList.Remove(itemDeleted);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool IsValid
        {
            get
            {
                return this.ItemTypeID > 0
                    && this.ItemPrice > 0.00m
                    && this.Quantity > 0;
            }
        }

        /// <summary>
        /// Gets a hccCartItem objects with the specified Ids, CartId.  
        /// </summary>
        /// <param name="cartItemIds">The CartItemIds to use in order to locate the Cart Items.</param>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns the CartItem-s that matches the ids.</returns>
        public static List<hccCartItem> GetByIds(IList<int> cartItemIds)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItems
                        .Where(i => cartItemIds.Contains(i.CartItemID)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets a hccCartItem object with the specified Id, CartId.  
        /// If cart item does not exists return null
        /// </summary>
        /// <param name="cartid">The CartId to use in order to locate the Cart Item parent.</param>
        /// <param name="cartitemid">The CartItemId to use in order to locate the Cart Item.</param>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns the CartItem that matches the ids.</returns>
        public static hccCartItem GetById(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItems
                        .SingleOrDefault(i => i.CartItemID == cartItemId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gets the hccCartItem object for the specified item type and item id.  
        /// If cart does not exists return null
        /// </summary>
        /// <param name="cartId">The itemTypeId to use in order to locate the Cart, via Common.Enums.CartItemType</param>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns the Cart Item that matches the itemTypeId + itemId.</returns>
        public static hccCartItem GetBy(int cartId, string itemName, int? profileId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var ci = cont.hccCartItems.Where(c => c.CartID == cartId && c.ItemName == itemName);

                    if (profileId.HasValue && profileId.Value > 0)
                        ci = ci.Where(a => a.UserProfileID == profileId.Value);

                    return ci.SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string BuildCartItemName(Enums.MealTypes mealType, Enums.CartItemSize itemSize, string menuItemName,
            string mealSides, string preferences, DateTime deliveryDate)//, int Quantity
        {
            string CartItemName = string.Empty;
            CartItemName = Enums.GetEnumDescription(mealType) +
                   (itemSize == Enums.CartItemSize.NoSize ? string.Empty : " - " + Enums.GetEnumDescription(itemSize)) +
                   " - " + menuItemName +
                   (string.IsNullOrEmpty(mealSides) ? string.Empty : " w/ " + mealSides) +
                   (string.IsNullOrEmpty(preferences) ? string.Empty : " - " + preferences) +
                   " - " + deliveryDate.ToShortDateString();// +  " - " + Quantity
            return CartItemName;
        }

        public static List<hccCartItem> GetBy(int cartId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItems
                        .Where(c => c.CartID == cartId)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //finally
            //{
            //    CartList = null;
            //}
        }

        public static List<hccCartItem> GetWithoutSideItemsBy(int cartId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItems
                        .Where(c => c.CartID == cartId && !(c.hccCartALCMenuItems.Any() && c.hccCartALCMenuItems.FirstOrDefault().ParentCartItemID != c.CartItemID))
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<hcc_SalesReportCartItems_Result> GetTaxable(int cartId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hcc_SalesReportCartItems(cartId).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<hccCartItem> GetBy(string orderNumber)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItems
                        .Where(c => c.OrderNumber.Trim().ToLower() == orderNumber.Trim().ToLower())
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MOTCartItem> Search_MealOrderTicketForMOT(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    List<MOTCartItem> motItems = new List<MOTCartItem>();

                    List<hccCartItem> alcgc = cont.hcc_OrderFulfillSearch_ALCnGC_ByRange(startDate, endDate).Select(x => new hccCartItem
                    {

                        CartItemID = Convert.ToInt16(x.CartItemID),
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

                    motItems.AddRange(MOTCartItem.GetFromRange(alcgc));

                    var progs = cont.hcc_OrderFulfillSearch_Programs_ByRange(startDate, endDate).ToList();
                    motItems.AddRange(MOTCartItem.GetFromRangeForMOT(progs));

                    return motItems.OrderBy(a => a.DeliveryDate)
                        .ThenBy(a => a.ItemName).ThenBy(a => a.OrderNumber.Contains("ALC")).ThenBy(a => a.CustomerName).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<AggrCartItem> Search(string lastName, string email, int? purchNum, DateTime? deliveryDate, bool includeSnapshots, bool includeGiftCerts)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
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
                        alcgc = cont.hcc_OrderFulfillSearch_ALCnGC(deliveryDate, purchNum, lastName, email).ToList().Where(a => a.ItemTypeID !=Convert.ToInt32(Enums.CartItemType.GiftCard)).Select(x => new hccCartItem
                        {

                            CartItemID =x.CartItemID,
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

                    return aggrItems;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public bool AdjustQuantity(int newQuantity)
        {
            try
            {
                int result = 0;

                using (var cont = new healthychefEntitiesAPI())
                {
                    result = cont.hcc_CartItem_AdjustQuantity(this.CartItemID, newQuantity);
                }

                if (result > 0)
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static hccCartItem GetGiftBy(string redeemCode)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var ci = cont.hccCartItems
                        .Where(c => c.Gift_RedeemCode.ToLower() == redeemCode.ToLower())
                        .SingleOrDefault();

                    return ci;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static bool RedeemCodeExists(string redeemCode)
        {
            try
            {
                return !(GetGiftBy(redeemCode) == null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static hccCartItem Gift_GenerateNew(int cartId)
        {
            try
            {
                string redeemCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();

                while (RedeemCodeExists(redeemCode))
                {
                    redeemCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                }

                hccCartItem giftCert = new hccCartItem
                {
                    CartID = cartId,
                    ItemTypeID = (int)Enums.CartItemType.GiftCard,
                    IsTaxable = false,
                    Quantity = 1,
                    CreatedBy = (Helpers.LoggedUser == null ? Guid.Empty : (Guid)Helpers.LoggedUser.ProviderUserKey),
                    CreatedDate = DateTime.Now,
                    IsCompleted = false,
                    Gift_RedeemCode = redeemCode
                };

                hccCart userCart = hccCart.GetById(cartId);
                if (userCart.AspNetUserID.HasValue && userCart.AspNetUserID.Value != Guid.Empty)
                {
                    hccUserProfile prof = hccUserProfile.GetParentProfileBy(userCart.AspNetUserID.Value);

                    if (prof != null)
                        giftCert.UserProfileID = prof.UserProfileID;

                    giftCert.Gift_IssuedTo = userCart.AspNetUserID;
                    giftCert.Gift_IssuedDate = DateTime.Now;
                }

                return giftCert;
            }
            catch (Exception ex) { throw ex; }
        }

        public static List<hccCartItem> GetGiftsByRedeemed(bool isRedeemed)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartItems
                        .Join(cont.hccCarts, a => a.CartID, b => b.CartID, (a, b) => new { CartItems = a, Cart = b })
                        .Where(a => a.Cart.StatusID == (int)Enums.CartStatus.Paid
                            && a.CartItems.ItemTypeID == (int)Enums.CartItemType.GiftCard
                            && a.CartItems.Gift_RedeemedBy.HasValue == isRedeemed)
                        .Select(a => a.CartItems)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GetOrderNumber(hccCart cart)
        {
            try
            {
                if (cart != null)
                {
                    if (this.OrderNumber == null || !this.OrderNumber.Contains(cart.PurchaseNumber.ToString()))
                    {
                        List<hccCartItem> cartItems = hccCartItem.GetBy(cart.CartID);

                        if (cartItems.Count == 0)
                        {
                            this.OrderNumber = cart.PurchaseNumber.ToString() + "-01";
                        }
                        else
                        {
                            cartItems.ForEach(delegate (hccCartItem existItem)
                            {
                                if (this.UserProfile != null)
                                {
                                    if ((this.UserProfile.UseParentShipping || this.UserProfile.ShippingAddressID == existItem.UserProfile.ShippingAddressID)
                                        && this.DeliveryDate == existItem.DeliveryDate)
                                    {
                                        this.OrderNumber = existItem.OrderNumber;
                                    }
                                }
                                else
                                {
                                    if (this.DeliveryDate == existItem.DeliveryDate)
                                    {
                                        this.OrderNumber = existItem.OrderNumber;
                                    }
                                }
                            });

                            if (string.IsNullOrWhiteSpace(this.OrderNumber) || !this.OrderNumber.Contains(cart.PurchaseNumber.ToString()))
                            {
                                string minOrderNumber = string.Empty;
                                minOrderNumber = cartItems.Max(a => a.OrderNumber);

                                if (string.IsNullOrWhiteSpace(minOrderNumber))
                                {
                                    this.OrderNumber = cart.PurchaseNumber.ToString() + "-01";
                                }
                                else
                                {
                                    string ord = minOrderNumber.Split(new char[] { '-' })[1];
                                    this.OrderNumber = cart.PurchaseNumber.ToString() + "-" + ((int.Parse(ord)) + 1).ToString("d2");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<int> GetDaysWithAllergens(int cartCalendarId)
        {
            try
            {
                List<int> daysWithAllergens = new List<int>();
                hccProgramPlan plan = hccProgramPlan.GetById(this.Plan_PlanID.Value);
                hccUserProfile profile = hccUserProfile.GetById(this.UserProfileID);
                hccCartItemCalendar cartCal = hccCartItemCalendar.GetById(cartCalendarId);

                if (plan != null && profile != null)
                {
                    // get list of user profile allergens
                    List<hccAllergen> userAllergens = profile.GetAllergens();

                    if (userAllergens.Count > 0)
                    {
                        List<hccProgramDefaultMenu> defaultMenus = hccProgramDefaultMenu.GetBy(cartCal.CalendarID, plan.ProgramID);

                        defaultMenus.ForEach(delegate (hccProgramDefaultMenu defaultMenu)
                        {
                            hccCartDefaultMenuException excMenu = hccCartDefaultMenuException.GetBy(defaultMenu.DefaultMenuID, cartCal.CartCalendarID);
                            List<hccAllergen> menuAllg = new List<hccAllergen>();

                            if (excMenu == null)
                            {
                                hccMenuItem item = hccMenuItem.GetById(defaultMenu.MenuItemID);
                                if (item != null)
                                {
                                    menuAllg = item.GetAllergens();
                                }
                            }
                            else
                            {
                                menuAllg = excMenu.GetMenuItemAllergens();
                            }

                            if (menuAllg.Count > 0)
                            {
                                if (userAllergens.Intersect(menuAllg).Count() > 0)
                                {
                                    daysWithAllergens.Add(defaultMenu.DayNumber);
                                }
                            }
                        });
                    }
                }
                return daysWithAllergens;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<hccCartItem> GetFromRange(List<hccCartItemCalendar> cartCalendars)
        {
            try
            {
                List<hccCartItem> retItems = new List<hccCartItem>();
                cartCalendars.ForEach(a => retItems.Add(hccCartItem.GetById(a.CartItemID)));
                return retItems;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public class MealsDelete
        {
            public int CartID { get; set; }
            public int MealCount { get; set; }
        }
    }
}