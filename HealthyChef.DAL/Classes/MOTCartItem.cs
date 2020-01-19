using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public class MOTCartItem
    {
        public hccCartItem CartItem { get; set; }
        public hccMenuItem MenuItem { get; set; }
        public int CartItemId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public DateTime BestByDate { get { return DeliveryDate.AddDays(7); } }
        public string DeliveryMethod { get; set; }
        public string CustomerName { get; set; }
        public string ProfileName { get; set; }
        public string ItemName { get; set; }
        public string PortionSize { get; set; }
        public string Servings { get; set; }
        public string Sides { get; set; }
        public string Preferences { get; set; }
        public int Quantity { get; set; }
        public int DayNumber { get; set; }
        public Enums.MealTypes MealType { get; set; }
        public string ItemTypeName
        {
            get
            {
                switch (CartItem.ItemType)
                {
                    case Enums.CartItemType.AlaCarte:
                        return Enums.GetEnumDescription(Enums.CartItemType.AlaCarte);
                    case Enums.CartItemType.DefinedPlan:
                        return hccProgramPlan.GetById(CartItem.Plan_PlanID.Value).Name;
                    case Enums.CartItemType.GiftCard:
                        return "Gift Certificate";
                }
                return Enums.GetEnumDescription(CartItem.ItemType);
            }
        }
        public string MenuItemSize
        {
            get
            {
                if (CartItem.ItemType == Enums.CartItemType.AlaCarte)
                {
                    return Enums.GetEnumDescription((Enums.CartItemSize)CartItem.Meal_MealSizeID);
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        public string MealTypeString { get { return Enums.GetEnumDescription(MealType); } }
        public string HTMLString { get { return this.ToString(); } }
        public string PackSlipString { get { return this.GetPackListString(); } }
        public int Index { get; set; }
        public string OrderNumber { get; set; }

        public MOTCartItem() { }

        public MOTCartItem(hccCartItem cartItem)
        {
            try
            {
                if (cartItem.ItemType == Common.Enums.CartItemType.AlaCarte)
                {
                    hccMenuItem mi = hccMenuItem.GetById(cartItem.Meal_MenuItemID.Value);
                    MenuItem = mi;
                    CartItem = cartItem;
                    CartItemId = cartItem.CartItemID;
                    OrderNumber = cartItem.OrderNumber + "-ALC";
                    DeliveryDate = cartItem.DeliveryDate;

                    if (cartItem.UserProfile != null)
                    {
                        CustomerName = cartItem.UserProfile.ParentProfileName;
                        ProfileName = cartItem.UserProfile.ProfileName;
                    }

                    ItemName = mi.Name;
                    MealType = mi.MealType;

                    if (cartItem.Meal_MealSizeID.HasValue)
                    {
                        Enums.CartItemSize size = (Enums.CartItemSize)cartItem.Meal_MealSizeID;

                        if (size == Enums.CartItemSize.NoSize)
                            size = Enums.CartItemSize.RegularSize;

                        PortionSize = Enums.GetEnumDescription(size);
                    }
                    else
                    {
                        PortionSize = Enums.GetEnumDescription(Enums.CartItemSize.RegularSize);
                    }

                    Servings = cartItem.Quantity.ToString();
                    Preferences = hccCartItemMealPreference.GetPrefsBy(cartItem.CartItemID)
                        .Select(a => a.Name).DefaultIfEmpty("None").Aggregate((c, d) => c + ", " + d);
                    Sides = "None";

                    if (hccMenuItem.EntreeMealTypes.Contains(MealType) && cartItem.MealSideMenuItems.Count > 0)
                    {
                        Sides = cartItem.GetMealSideMenuItemsAsSectionString(", ");
                    }

                    if (cartItem.SnapShipAddrId.HasValue)
                    {
                        hccAddress addr = hccAddress.GetById(cartItem.SnapShipAddrId.Value);
                        DeliveryMethod = ((Enums.DeliveryTypes)addr.DefaultShippingTypeID).ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MOTCartItem(hccCartItemCalendar cartCalendar)
        {
            try
            {
                CartItem = hccCartItem.GetById(cartCalendar.CartItemID);
                CartItemId = CartItem.CartItemID;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MOTCartItem> GetFromRange(List<hccCartItem> cartItems)
        {
            List<MOTCartItem> MOTCartItems = new List<MOTCartItem>();

            foreach (hccCartItem cartItem in cartItems)
            {
                if (cartItem.ItemType == Common.Enums.CartItemType.AlaCarte)
                {
                    MOTCartItems.Add(new MOTCartItem(cartItem));
                }
            }

            return MOTCartItems;
        }

        public static List<MOTCartItem> GetFromRange(List<hccCartItemCalendar> cartCalendars)
        {
            List<MOTCartItem> MOTCartItems = new List<MOTCartItem>();

            foreach (hccCartItemCalendar cartCal in cartCalendars)
            {
                MOTCartItems.AddRange(GetFromCartCalendar(cartCal));
            }

            return MOTCartItems;
        }

        public static List<MOTCartItem> GetFromRangeForMOT(List<hccCartItemCalendar> cartCalendars)
        {
            List<MOTCartItem> MOTCartItems = new List<MOTCartItem>();

            foreach (hccCartItemCalendar cartCal in cartCalendars)
            {
                MOTCartItems.AddRange(GetFromCartCalendarForMOT(cartCal));
            }

            return MOTCartItems;
        }

        public static List<MOTCartItem> GetFromRangeForWaag(List<hccCartItemCalendar> cartCalendars)
        {
            List<MOTCartItem> MOTCartItems = new List<MOTCartItem>();

            foreach (hccCartItemCalendar cartCal in cartCalendars)
            {
                MOTCartItems.AddRange(GetFromCartCalendarForWaaG(cartCal));
            }

            return MOTCartItems;
        }

        public static List<MOTCartItem> GetFromCartCalendar(hccCartItemCalendar cartCal)
        {
            List<MOTCartItem> retMotItems = new List<MOTCartItem>();
            hccCartItem cartItem = hccCartItem.GetById(cartCal.CartItemID);
            hccProductionCalendar prodCal = hccProductionCalendar.GetById(cartCal.CalendarID);
            hccProgramPlan plan = hccProgramPlan.GetById(cartItem.Plan_PlanID.Value);
            List<hccProgramDefaultMenu> defMenus = hccProgramDefaultMenu.GetBy(cartCal.CalendarID, plan.ProgramID);
            List<hccMenuItem> planMeals = new List<hccMenuItem>();

            defMenus.ForEach(delegate(hccProgramDefaultMenu defMenu)
            {
                hccCartDefaultMenuException cartDefMenuEx = hccCartDefaultMenuException.GetBy(defMenu.DefaultMenuID, cartCal.CartCalendarID);

                hccMenuItem selItem;
                Enums.CartItemSize selPortionSize;
                string prefsString = string.Empty;

                // Get the current menuItem
                if (cartDefMenuEx == null)
                {
                    selItem = hccMenuItem.GetById(defMenu.MenuItemID);
                    selPortionSize = (Enums.CartItemSize)defMenu.MenuItemSizeID;
                    prefsString = "None";
                }
                else
                {
                    selItem = hccMenuItem.GetById(cartDefMenuEx.MenuItemID);
                    selPortionSize = (Enums.CartItemSize)cartDefMenuEx.MenuItemSizeID;
                    prefsString = hccCartDefaultMenuExPref.GetPrefsBy(cartDefMenuEx.DefaultMenuExceptID)
                       .Select(a => a.Name).DefaultIfEmpty("None").Aggregate((c, d) => c + ", " + d);
                }

                MOTCartItem curMotItem = null;

                if (selItem != null)
                {
                    if (selPortionSize == Enums.CartItemSize.NoSize)
                        selPortionSize = Enums.CartItemSize.RegularSize;

                    // determine if current item is an Entree or side
                    if (selItem.MealType == Enums.MealTypes.BreakfastEntree
                        || selItem.MealType == Enums.MealTypes.LunchEntree
                        || selItem.MealType == Enums.MealTypes.DinnerEntree
                        || selItem.MealType == Enums.MealTypes.OtherEntree
                        || selItem.MealType == Enums.MealTypes.ChildEntree
                        || selItem.MealType == Enums.MealTypes.Beverage
                        || selItem.MealType == Enums.MealTypes.Dessert
                        || selItem.MealType == Enums.MealTypes.Goods
                        || selItem.MealType == Enums.MealTypes.Miscellaneous
                        || selItem.MealType == Enums.MealTypes.Salad
                        || selItem.MealType == Enums.MealTypes.Snack
                        || selItem.MealType == Enums.MealTypes.Soup
                        || selItem.MealType == Enums.MealTypes.Supplement)
                    {   // Entrees

                        // determine if this type of entree has been added in previous loop
                        curMotItem = retMotItems.FirstOrDefault(a => a.DayNumber == defMenu.DayNumber
                            && a.MealType == selItem.MealType);

                        if (curMotItem == null)
                        {
                            //if not added in previous loop, add new 
                            curMotItem = new MOTCartItem
                            {
                                MenuItem = selItem,
                                CartItem = cartItem,
                                CartItemId = cartItem.CartItemID,
                                CustomerName = cartItem.UserProfile.ParentProfileName,
                                OrderNumber = cartItem.OrderNumber,
                                DeliveryDate = prodCal.DeliveryDate,
                                DayNumber = defMenu.DayNumber,
                                ItemName = selItem.Name,
                                MealType = selItem.MealType,
                                PortionSize = Enums.GetEnumDescription(selPortionSize),
                                Preferences = prefsString,
                                ProfileName = cartItem.UserProfile.ProfileName,
                                Servings = cartItem.Quantity.ToString(),
                            };

                            retMotItems.Add(curMotItem);
                        }
                        else
                        {
                            //if entree added in previous loop, add as a side 
                            curMotItem.Sides += selPortionSize + " - " + selItem.Name;
                        }
                    }
                    else
                    {   // Sides

                        // get parent entree type
                        Enums.MealTypes parentType = Enums.MealTypes.Unknown;

                        if (selItem.MealType == Enums.MealTypes.BreakfastSide)
                        {
                            parentType = Enums.MealTypes.BreakfastEntree;
                        }
                        else if (selItem.MealType == Enums.MealTypes.LunchSide)
                        {
                            parentType = Enums.MealTypes.LunchEntree;
                        }
                        else if (selItem.MealType == Enums.MealTypes.DinnerSide)
                        {
                            parentType = Enums.MealTypes.DinnerEntree;
                        }
                        else if (selItem.MealType == Enums.MealTypes.OtherSide)
                        {
                            parentType = Enums.MealTypes.OtherEntree;
                        }
                        else if (selItem.MealType == Enums.MealTypes.ChildSide)
                        {
                            parentType = Enums.MealTypes.ChildEntree;
                        }

                        // determine if this type of entree has been added in previous loop
                        curMotItem = retMotItems.FirstOrDefault(a => a.DayNumber == defMenu.DayNumber
                            && a.MealType == parentType);

                        if (curMotItem == null)
                        {
                            curMotItem = retMotItems.FirstOrDefault(a => a.DayNumber == defMenu.DayNumber);

                            if (curMotItem == null)
                            {
                                curMotItem = new MOTCartItem
                                {
                                    MenuItem = selItem,
                                    CartItem = cartItem,
                                    CartItemId = cartItem.CartItemID,
                                    CustomerName = cartItem.UserProfile.ParentProfileName,
                                    OrderNumber = cartItem.OrderNumber,
                                    DeliveryDate = prodCal.DeliveryDate,
                                    DayNumber = defMenu.DayNumber,
                                    ItemName = selItem.Name,
                                    MealType = selItem.MealType,
                                    PortionSize = Enums.GetEnumDescription(selPortionSize),
                                    Preferences = prefsString,
                                    ProfileName = cartItem.UserProfile.ProfileName,
                                    Servings = cartItem.Quantity.ToString(),
                                    Sides = "NoneB"
                                };


                                retMotItems.Add(curMotItem);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(curMotItem.Sides))
                            {
                                curMotItem.Sides += selItem.Name;

                                if (prefsString != "None" && !string.IsNullOrWhiteSpace(prefsString))
                                    curMotItem.Sides += " (" + prefsString + ")";
                            }
                            else
                            {
                                curMotItem.Sides += ", " + selItem.Name;

                                if (prefsString != "None" && !string.IsNullOrWhiteSpace(prefsString))
                                    curMotItem.Sides += " (" + prefsString + ")";
                            }
                        }
                    }
                }
            });

            return retMotItems;
        }

        public static List<MOTCartItem> GetFromCartCalendarForMOT(hccCartItemCalendar cartCal)
        {
            try
            {
                List<MOTCartItem> retMotItems = new List<MOTCartItem>();
                List<MOTCartItem> addlMotItems = new List<MOTCartItem>();

                hccCartItem cartItem = hccCartItem.GetById(cartCal.CartItemID);
                hccProductionCalendar prodCal = hccProductionCalendar.GetById(cartCal.CalendarID);
                hccProgramPlan plan = hccProgramPlan.GetById(cartItem.Plan_PlanID.Value);
                List<hccProgramDefaultMenu> defMenus = hccProgramDefaultMenu.GetBy(cartCal.CalendarID, plan.ProgramID, plan.NumDaysPerWeek);
                List<hccMenuItem> planMeals = new List<hccMenuItem>();

                defMenus.ForEach(delegate(hccProgramDefaultMenu defMenu)
                {
                    hccCartDefaultMenuException cartDefMenuEx = hccCartDefaultMenuException.GetBy(defMenu.DefaultMenuID, cartCal.CartCalendarID);

                    hccMenuItem selItem;
                    Enums.CartItemSize selPortionSize = Enums.CartItemSize.NoSize;
                    string prefsString = string.Empty;

                    if (cartDefMenuEx == null)
                    {
                        selItem = hccMenuItem.GetById(defMenu.MenuItemID);
                        selPortionSize = (Enums.CartItemSize)defMenu.MenuItemSizeID;
                        prefsString = "None";
                    }
                    else
                    {
                        selItem = hccMenuItem.GetById(cartDefMenuEx.MenuItemID);
                        selPortionSize = (Enums.CartItemSize)cartDefMenuEx.MenuItemSizeID;

                        try
                        {
                            prefsString = string.Empty;
                            List<hccPreference> prefs = hccCartDefaultMenuExPref.GetPrefsBy(cartDefMenuEx.DefaultMenuExceptID);
                            prefsString = prefs.Select(a => a.Name).DefaultIfEmpty("None").Aggregate((c, d) => c + ", " + d);
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }

                    if (selPortionSize == Enums.CartItemSize.NoSize)
                        selPortionSize = Enums.CartItemSize.RegularSize;

                    MOTCartItem curMotItem = null;

                    if (selItem != null)
                    {
                        if (selItem.MealType == Enums.MealTypes.BreakfastEntree
                            || selItem.MealType == Enums.MealTypes.LunchEntree
                            || selItem.MealType == Enums.MealTypes.DinnerEntree
                            || selItem.MealType == Enums.MealTypes.ChildEntree
                            || selItem.MealType == Enums.MealTypes.Beverage
                            || selItem.MealType == Enums.MealTypes.Dessert
                            || selItem.MealType == Enums.MealTypes.Goods
                            || selItem.MealType == Enums.MealTypes.Miscellaneous
                            || selItem.MealType == Enums.MealTypes.Salad
                            || selItem.MealType == Enums.MealTypes.Snack
                            || selItem.MealType == Enums.MealTypes.Soup
                            || selItem.MealType == Enums.MealTypes.Supplement)
                        {
                            curMotItem = retMotItems.FirstOrDefault(a => a.DayNumber == defMenu.DayNumber
                                && a.MealType == selItem.MealType);

                            if (curMotItem == null || curMotItem.MealType == Enums.MealTypes.Snack)
                            {
                                curMotItem = new MOTCartItem
                                {
                                    CartItem = cartItem,
                                    CartItemId = cartItem.CartItemID,
                                    CustomerName = cartItem.UserProfile.ParentProfileName,
                                    OrderNumber = cartItem.OrderNumber + "-PRG",
                                    DeliveryDate = prodCal.DeliveryDate,
                                    DayNumber = defMenu.DayNumber,
                                    ItemName = selItem.Name,
                                    MealType = selItem.MealType,
                                    PortionSize = Enums.GetEnumDescription(selPortionSize),
                                    Preferences = prefsString,
                                    ProfileName = cartItem.UserProfile != null ? cartItem.UserProfile.ProfileName : string.Empty,
                                    Servings = cartItem.Quantity.ToString(),
                                    MenuItem = selItem,
                                    Quantity = cartItem.Quantity,
                                    Sides = "None"
                                };

                                if (cartItem.SnapShipAddrId.HasValue)
                                {
                                    hccAddress addr = hccAddress.GetById(cartItem.SnapShipAddrId.Value);
                                    curMotItem.DeliveryMethod = ((Enums.DeliveryTypes)addr.DefaultShippingTypeID).ToString();
                                }

                                retMotItems.Add(curMotItem);
                            }
                            else
                            {
                                curMotItem.Sides += selPortionSize + " - " + selItem.Name;
                            }

                        }
                        else
                        {
                            // get parent entree type
                            Enums.MealTypes parentType = Enums.MealTypes.Unknown;

                            if (selItem.MealType == Enums.MealTypes.BreakfastSide)
                            {
                                parentType = Enums.MealTypes.BreakfastEntree;
                            }
                            else if (selItem.MealType == Enums.MealTypes.LunchSide)
                            {
                                parentType = Enums.MealTypes.LunchEntree;
                            }
                            else if (selItem.MealType == Enums.MealTypes.DinnerSide)
                            {
                                parentType = Enums.MealTypes.DinnerEntree;
                            }
                            else if (selItem.MealType == Enums.MealTypes.OtherSide)
                            {
                                parentType = Enums.MealTypes.OtherEntree;
                            }
                            else if (selItem.MealType == Enums.MealTypes.ChildSide)
                            {
                                parentType = Enums.MealTypes.ChildEntree;
                            }

                            curMotItem = retMotItems.FirstOrDefault(a => a.DayNumber == defMenu.DayNumber
                                && a.MealType == parentType);

                            if (curMotItem == null)
                            {
                                curMotItem = retMotItems.FirstOrDefault(a => a.DayNumber == defMenu.DayNumber);

                                if (curMotItem == null)
                                {
                                    curMotItem = new MOTCartItem
                                    {
                                        CartItem = cartItem,
                                        CartItemId = cartItem.CartItemID,
                                        CustomerName = cartItem.UserProfile.ParentProfileName,
                                        OrderNumber = cartItem.OrderNumber + "-PRG",
                                        DeliveryDate = prodCal.DeliveryDate,
                                        DayNumber = defMenu.DayNumber,
                                        ItemName = selItem.Name,
                                        MealType = selItem.MealType,
                                        PortionSize = Enums.GetEnumDescription(selPortionSize),
                                        Preferences = prefsString,
                                        ProfileName = cartItem.UserProfile != null ? cartItem.UserProfile.ProfileName : string.Empty,
                                        Servings = cartItem.Quantity.ToString(),
                                        MenuItem = selItem,
                                        Quantity = cartItem.Quantity,
                                        Sides = "None"
                                    };

                                    if (cartItem.SnapShipAddrId.HasValue)
                                    {
                                        hccAddress addr = hccAddress.GetById(cartItem.SnapShipAddrId.Value);
                                        curMotItem.DeliveryMethod = ((Enums.DeliveryTypes)addr.DefaultShippingTypeID).ToString();
                                    }

                                    retMotItems.Add(curMotItem);
                                }
                            }
                            else
                            {
                                if (string.IsNullOrWhiteSpace(curMotItem.Sides))
                                {
                                    curMotItem.Sides = selItem.Name;

                                    if (prefsString != "None" && !string.IsNullOrWhiteSpace(prefsString))
                                        curMotItem.Sides += " (" + prefsString + ")";
                                }
                                else
                                {
                                    if (curMotItem.Sides == "None")
                                        curMotItem.Sides = selItem.Name;
                                    else
                                        curMotItem.Sides += ", " + selItem.Name;

                                    if (prefsString != "None" && !string.IsNullOrWhiteSpace(prefsString))
                                        curMotItem.Sides += " (" + prefsString + ")";
                                }
                            }
                        }
                    }
                });

                List<MOTCartItem> removeDupItems = new List<MOTCartItem>();

                retMotItems.ForEach(delegate(MOTCartItem motItem)
                {

                    if (motItem.MealType == Enums.MealTypes.BreakfastEntree
                           || motItem.MealType == Enums.MealTypes.LunchEntree
                           || motItem.MealType == Enums.MealTypes.DinnerEntree
                           || motItem.MealType == Enums.MealTypes.ChildEntree
                           || motItem.MealType == Enums.MealTypes.Beverage
                           || motItem.MealType == Enums.MealTypes.Dessert
                           || motItem.MealType == Enums.MealTypes.Goods
                           || motItem.MealType == Enums.MealTypes.Miscellaneous
                           || motItem.MealType == Enums.MealTypes.Salad
                           || motItem.MealType == Enums.MealTypes.Snack
                           || motItem.MealType == Enums.MealTypes.Soup
                           || motItem.MealType == Enums.MealTypes.Supplement)
                    {
                        if (motItem.CartItem.Quantity > 1)
                        {
                            for (int i = 1; i <= motItem.CartItem.Quantity; i++)
                            {
                                MOTCartItem copyMotItem = new MOTCartItem
                                {
                                    CartItem = motItem.CartItem,
                                    CartItemId = motItem.CartItemId,
                                    OrderNumber = motItem.OrderNumber,
                                    DeliveryDate = motItem.DeliveryDate,
                                    CustomerName = motItem.CustomerName,
                                    DayNumber = motItem.DayNumber,
                                    ItemName = motItem.ItemName,
                                    MealType = motItem.MealType,
                                    PortionSize = motItem.PortionSize,
                                    Preferences = motItem.Preferences,
                                    ProfileName = motItem.ProfileName,
                                    MenuItem = motItem.MenuItem,
                                    Index = motItem.Index,
                                    Quantity = motItem.Quantity,
                                    Sides = motItem.Sides,
                                    DeliveryMethod = motItem.DeliveryMethod,
                                    Servings = "1" //motItem.Servings
                                };

                                addlMotItems.Add(copyMotItem);
                            }

                            removeDupItems.Add(motItem);
                        }
                    }
                });

                if (addlMotItems.Count > 0)
                    retMotItems.AddRange(addlMotItems);

                if (removeDupItems.Count > 0)
                    removeDupItems.ForEach(a => retMotItems.Remove(a));

                var t = retMotItems.ToList();
                return t;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MOTCartItem> GetFromCartCalendarForWaaG(hccCartItemCalendar cartCal)
        {
            List<MOTCartItem> retMotItems = new List<MOTCartItem>();
            hccCartItem cartItem = hccCartItem.GetById(cartCal.CartItemID);
            hccProductionCalendar prodCal = hccProductionCalendar.GetById(cartCal.CalendarID);
            hccProgramPlan plan = hccProgramPlan.GetById(cartItem.Plan_PlanID.Value);
            List<hccProgramDefaultMenu> defMenus = hccProgramDefaultMenu.GetBy(cartCal.CalendarID, plan.ProgramID);
            List<hccMenuItem> planMeals = new List<hccMenuItem>();

            defMenus.ForEach(delegate(hccProgramDefaultMenu defMenu)
            {
                hccCartDefaultMenuException cartDefMenuEx = hccCartDefaultMenuException.GetBy(defMenu.DefaultMenuID, cartCal.CartCalendarID);

                hccMenuItem selItem;
                Enums.CartItemSize selPortionSize;
                string prefsString = string.Empty;

                if (cartDefMenuEx == null)
                {
                    selItem = hccMenuItem.GetById(defMenu.MenuItemID);
                    selPortionSize = (Enums.CartItemSize)defMenu.MenuItemSizeID;
                    prefsString = "None";
                }
                else
                {
                    selItem = hccMenuItem.GetById(cartDefMenuEx.MenuItemID);
                    selPortionSize = (Enums.CartItemSize)cartDefMenuEx.MenuItemSizeID;
                    prefsString = hccCartDefaultMenuExPref.GetPrefsBy(cartDefMenuEx.DefaultMenuExceptID)
                       .Select(a => a.Name).DefaultIfEmpty("None").Aggregate((c, d) => c + ", " + d);
                }

                MOTCartItem curMotItem = null;

                if (selItem != null)
                {
                    if (selPortionSize == Enums.CartItemSize.NoSize)
                        selPortionSize = Enums.CartItemSize.RegularSize;

                    curMotItem = new MOTCartItem
                    {
                        CartItem = cartItem,
                        CartItemId = cartItem.CartItemID,
                        CustomerName = cartItem.UserProfile.ParentProfileName,
                        OrderNumber = cartItem.OrderNumber,
                        DeliveryDate = prodCal.DeliveryDate,
                        DayNumber = defMenu.DayNumber,
                        ItemName = selItem.Name,
                        MealType = selItem.MealType,
                        PortionSize = Enums.GetEnumDescription(selPortionSize),
                        Preferences = prefsString,
                        ProfileName = cartItem.UserProfile.ProfileName,
                        Servings = cartItem.Quantity.ToString()
                    };
                    
                    retMotItems.Add(curMotItem);
                }
            });

            return retMotItems;
        }

        public static Tuple<List<MOTCartItem>, List<MOTCartItem>> SortOddEven(List<MOTCartItem> origItems)
        {
            List<MOTCartItem> evens = new List<MOTCartItem>(), odds = new List<MOTCartItem>();
            int i = 1;
            origItems.ForEach(delegate(MOTCartItem item)
            {
                item.Index = i;

                if (i % 2 != 0)
                    odds.Add(item);
                else
                    evens.Add(item);

                i++;
            });

            return new Tuple<List<MOTCartItem>, List<MOTCartItem>>(odds, evens);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(DeliveryDate.ToLongDateString() + "<br/>");
            sb.Append("Order Number: " + OrderNumber + "<br>");
            sb.Append("Customer Name: <strong>" + CustomerName + "</strong><br>");
            sb.Append("Profile Name: " + ProfileName + "<br>");
            sb.Append("Main Item: " + Enums.GetEnumDescription(MealType) + ": <strong>" + ItemName + "</strong><br>");
            sb.Append("Sides: " + (string.IsNullOrWhiteSpace(Sides) ? "<strong>None</strong>" : "<strong>" + Sides + "</strong>") + "<br>");
            sb.Append("Portion Size: <strong>" + PortionSize + "</strong><br>");
            //sb.Append("Allergens: " + Allergens + "<br>");
            sb.Append("Servings: " + Servings + "<br>");
            sb.Append("Preferences: <strong><em>" + (string.IsNullOrWhiteSpace(Preferences) ? "None" : Preferences) + "</em></strong>");
            return sb.ToString();
        }

        public string GetPackListString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<strong>" + ItemName + "</strong><br>");
            sb.Append("Sides: " + (string.IsNullOrWhiteSpace(Sides) ? "None" : Sides) + "<br>");
            //sb.Append("Portion Size: " + PortionSize + "<br>");
            ////sb.Append("Allergens: " + Allergens + "<br>");
            //sb.Append("Servings: " + Servings + "<br>");
            sb.Append("Preferences: " + (string.IsNullOrWhiteSpace(Preferences) ? "None" : Preferences));
            return sb.ToString();
        }
    }
}

