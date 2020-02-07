using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccMenuItem
    {
        public static readonly Enums.MealTypes[] EntreeMealTypes = { Enums.MealTypes.BreakfastEntree, Enums.MealTypes.ChildEntree, Enums.MealTypes.DinnerEntree, Enums.MealTypes.LunchEntree, Enums.MealTypes.OtherEntree };
        public static readonly Enums.MealTypes[] SideMealTypes = { Enums.MealTypes.BreakfastSide, Enums.MealTypes.ChildSide, Enums.MealTypes.DinnerSide, Enums.MealTypes.LunchSide, Enums.MealTypes.OtherSide };
        public static readonly Dictionary<Enums.MealTypes, Enums.MealTypes>  EntreeSideMealTypes = new Dictionary<Enums.MealTypes, Enums.MealTypes>
        {
            { Enums.MealTypes.BreakfastEntree, Enums.MealTypes.BreakfastSide },
            { Enums.MealTypes.ChildEntree, Enums.MealTypes.ChildSide },
            { Enums.MealTypes.DinnerEntree, Enums.MealTypes.DinnerSide },
            { Enums.MealTypes.LunchEntree, Enums.MealTypes.LunchSide },
            { Enums.MealTypes.OtherEntree, Enums.MealTypes.OtherSide }
        };
        public const string DefaultMealSideName = "Chef to Choose";

        public Enums.MealTypes MealType { get { return ((Enums.MealTypes)this.MealTypeID); } }
        //public decimal AverageRating { get { return this.GetAverageRating(); } }
        public string TypeAndName { get { return this.MealType.ToString() + " - " + this.Name; } }

        public string AllergensList
        {
            get
            {
                string allergens = this.GetAllergens()
                    .OrderBy(s => s.Name)
                    .Select(b => b.Name)
                    .Distinct()
                    .DefaultIfEmpty("None")
                    .Aggregate((c, d) => c + ", " + d);

                return allergens;
            }
        }

        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccMenuItems", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccMenuItems", this);
                    }
                    else
                    {
                        cont.hccMenuItems.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccMenuItem> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItems.ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccMenuItem> GetBy(HealthyChef.Common.Enums.MealTypes mealType)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItems
                        .Where(a => (HealthyChef.Common.Enums.MealTypes)a.MealTypeID == mealType
                            && a.IsRetired == false)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccMenuItem> GetBy(string name, HealthyChef.Common.Enums.MealTypes mealType)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    var r = cont.hccMenuItems
                        .Where(a => a.Name.ToLower() == name.ToLower()
                            && (HealthyChef.Common.Enums.MealTypes)a.MealTypeID == mealType)
                        .OrderBy(b => b.Name)
                        .ToList();

                    return r;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccMenuItem GetBy(string name, int mealTypeId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    var r = cont.hccMenuItems
                        .SingleOrDefault(a => a.Name.ToLower() == name.ToLower()
                            && a.MealTypeID == mealTypeId);

                    return r;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccMenuItem> GetBy(bool isRetired)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItems
                        .Where(a => a.IsRetired == isRetired)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccMenuItem> GetBy(int ingredientId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItemIngredients
                        .Where(c => c.IngredientID == ingredientId)
                        .Join(cont.hccMenuItems, b => b.MenuItemID, a => a.MenuItemID, (a, b) => b)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccMenuItem> GetByMenuId(int menuId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    var menu = cont.hccMenus
                        .Where(c => c.MenuID == menuId)
                        .SingleOrDefault();

                    if (menu != null)
                        return menu.hccMenuItems
                        .OrderBy(a => a.MealType.ToString())
                        .ThenBy(a => a.Name)
                        .ToList();
                    else
                        return new List<hccMenuItem>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccMenuItem GetById(int menuItemId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItems.SingleOrDefault(a => a.MenuItemID == menuItemId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccMenuItem GetByItemName(string menuItemName)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    //return (from menuitem in cont.hccMenuItems
                    //        where menuitem.Name.Contains(menuItemName)
                    //        select menuitem).ToList();
                    return cont.hccMenuItems.Where(a => a.Name == menuItemName).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static hccMenuItem GetByItemNameLast(string menuItemName)
        {
            try
            {
                List<hccMenuItem> Items = new List<hccMenuItem>();
                using (var cont = new healthychefEntities())
                {
                    var MenuItems = (from menuitem in cont.hccMenuItems
                                     where menuitem.Name == menuItemName
                                     select menuitem).ToList();
                    if(MenuItems.Count > 1)
                    {
                        foreach (var data in MenuItems)
                        {
                            if (data.CostLarge != 0.0000M && data.CostChild != 0.0000M && data.CostRegular != 0.0000M && data.CostSmall != 0.0000M)
                            {
                                Items.Add(data);
                            }
                        }
                        return Items.FirstOrDefault();
                    }
                    else
                    {
                        return MenuItems.FirstOrDefault();
                    }                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<hccIngredient> GetIngredients()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    List<hccIngredient> items = cont.hccMenuItemIngredients
                        .Where(a => a.MenuItemID == this.MenuItemID)
                        .Join(cont.hccIngredients,
                            j1 => j1.IngredientID,
                            j2 => j2.IngredientID,
                            (j1, j2) => j2)
                        .ToList();

                    return items;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<hccAllergen> GetAllergens()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItemIngredients
                       .Where(a => a.MenuItemID == this.MenuItemID)
                        .Join(cont.hccIngredients, j1 => j1.IngredientID, j2 => j2.IngredientID, (j1, j2) => j2)
                        .Join(cont.hccIngredientAllergens, j1 => j1.IngredientID, j2 => j2.IngredientID, (j1, j2) => j2)
                        .Join(cont.hccAllergens, j1 => j1.AllergenID, j2 => j2.AllergenID, (j1, j2) => j2)
                        .OrderBy(a => a.Name)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<hccPreference> GetPreferences()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItemPreferences
                        .Where(a => a.MenuItemID == this.MenuItemID)
                        .Join(cont.hccPreferences,
                            j1 => j1.PreferenceID,
                            j2 => j2.PreferenceID,
                            (j1, j2) => j2)
                            .OrderBy(a => a.Name)
                        .ToList();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static decimal GetItemPriceBySize(hccMenuItem menuItem, int? itemSize)
        {
            switch ((Enums.CartItemSize)itemSize)
            {
                case Enums.CartItemSize.ChildSize:
                    return menuItem.CostChild;
                case Enums.CartItemSize.SmallSize:
                    return menuItem.CostSmall;
                case Enums.CartItemSize.RegularSize:
                    return menuItem.CostRegular;
                case Enums.CartItemSize.LargeSize:
                    return menuItem.CostLarge;
                default: return 0.00m;
            }
        }

        public hccMenuItemNutritionData GetNutritionData()
        {
            try
            {
                return hccMenuItemNutritionData.GetBy(this.MenuItemID);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Retire(bool useUnretire)
        {
            this.IsRetired = !useUnretire;
            Save();
        }

        public static List<ChefProdItem> GetChefProdItems(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<MealItemReportItem> results = ReportSprocs.GetMenuItemsByDateRange(startDate, endDate).ToList();
                //List<ChefProdItem> chefItems = new List<ChefProdItem>();
                List<ChefProdItem> retItems = new List<ChefProdItem>();

                foreach (MealItemReportItem result in results)
                {
                    ChefProdItem existItem = retItems
                        .SingleOrDefault(a => a.DeliveryDate == result.DeliveryDate
                            && a.MenuItemId == result.MenuItemId
                            && a.MealTypeId == result.MealTypeId
                            && a.MealSizeId == result.MealSizeId
                            && a.Prefs == result.Prefs);

                    if (existItem == null)
                    {
                        retItems.Add(new ChefProdItem
                        {
                            MenuItemId = result.MenuItemId,
                            MealTypeId = result.MealTypeId,
                            MealSizeId = result.MealSizeId,
                            ItemName = result.ItemName,
                            MealType = result.MealTypeName,
                            MealSize = result.MealSizeName,
                            Quantity = result.Quantity,
                            Prefs = string.IsNullOrWhiteSpace(result.Prefs) ? string.Empty : result.Prefs,
                            DeliveryDate = result.DeliveryDate,
                            OrderNumber = result.OrderNumber
                        });
                    }
                    else
                    {
                        existItem.Quantity += result.Quantity;
                    }
                }

                //var s = chefItems.OrderBy(a => a.MealType).ThenBy(a => a.ItemName).ThenBy(a => a.MealSize).ThenBy(a => a.Prefs).ToList();

                //foreach (ChefProdItem chefItem in s)
                //{
                //    ChefProdItem existItem = retItems.SingleOrDefault(a => a.MealType == chefItem.MealType
                //        && a.ItemName == chefItem.ItemName && a.MealSize == chefItem.MealSize && a.Prefs == chefItem.Prefs);

                //    if (existItem == null)
                //    {
                //        retItems.Add(new ChefProdItem
                //        {
                //            ParentTypeId = 0,
                //            ParentId = 0,
                //            MenuItemId = chefItem.MenuItemId,
                //            ItemName = chefItem.ItemName,
                //            MealType = chefItem.MealType,
                //            MealSize = chefItem.MealSize,
                //            Quantity = chefItem.Quantity,
                //            Prefs = chefItem.Prefs,
                //            DeliveryDate = chefItem.DeliveryDate,
                //            OrderNumber = chefItem.OrderNumber,
                //            MealSizeId = chefItem.MealSizeId,
                //            MealTypeId = chefItem.MealTypeId
                //        });
                //    }
                //    else
                //    {
                //        existItem.Quantity += chefItem.Quantity;
                //    }
                //}

                var r = retItems.OrderBy(a => a.MealType).ThenBy(a => a.ItemName).ThenByDescending(a => a.MealSize).ThenByDescending(a => a.Prefs).ToList();
                return r;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<ChefProdItem> GetWaaGItems(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<ChefProdItem> chefItems = new List<ChefProdItem>();
                List<ChefProdItem> retItems = new List<ChefProdItem>();

                List<MealItemReportItem> results = ReportSprocs.GetMenuItemsByDateRange(startDate, endDate).ToList();

                if (results != null && results.Count > 0)
                {
                    foreach (MealItemReportItem result in results)
                    {
                        chefItems.Add(new ChefProdItem
                        {
                            ParentTypeId = result.ParentTypeId,
                            ParentId = result.ParentId,
                            MenuItemId = result.MenuItemId,
                            MealTypeId = result.MealTypeId,
                            MealSizeId = result.MealSizeId,
                            ItemName = result.ItemName,
                            MealType = Enums.GetEnumDescription(((Enums.MealTypes)result.MealTypeId)),
                            MealSize = Enums.GetEnumDescription(((Enums.CartItemSize)result.MealSizeId)),
                            Quantity = result.Quantity,
                            DeliveryDate = result.DeliveryDate,
                            OrderNumber = result.OrderNumber
                        });
                    }

                    var s = chefItems.OrderBy(a => a.MealType).ThenBy(a => a.ItemName).ThenBy(a => a.MealSize).ThenBy(a => a.Prefs).ToList();

                    foreach (ChefProdItem chefItem in s)
                    {
                        ChefProdItem existItem = retItems.SingleOrDefault(a => a.MealType == chefItem.MealType
                            && a.ItemName == chefItem.ItemName && a.MealSize == chefItem.MealSize);

                        if (existItem == null)
                        {
                            retItems.Add(new ChefProdItem
                            {
                                ParentTypeId = 0,
                                ParentId = 0,
                                MenuItemId = chefItem.MenuItemId,
                                ItemName = chefItem.ItemName,
                                MealType = chefItem.MealType,
                                MealSize = chefItem.MealSize,
                                Quantity = chefItem.Quantity,
                                DeliveryDate = chefItem.DeliveryDate,
                                MealSizeId = chefItem.MealSizeId,
                                MealTypeId = chefItem.MealTypeId,
                                OrderNumber = chefItem.OrderNumber
                            });
                        }
                        else
                        {
                            existItem.Quantity += chefItem.Quantity;
                        }
                    }
                }

                var r = retItems.OrderBy(a => a.MealType).ThenBy(a => a.ItemName).ThenBy(a => a.MealSize).ToList();
                return r;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<ChefProdItem> GetPackSlipItems(string orderNumber, DateTime deliveryDate)
        {
            try
            {
                List<MealItemReportItem> results = null;
                List<ChefProdItem> retItems = new List<ChefProdItem>();

                results = ReportSprocs.GetMenuItemsByOrderNumber(orderNumber, deliveryDate).ToList();
                results = results.Where(ri => !hccMenuItem.SideMealTypes.Contains((Enums.MealTypes)ri.MealTypeId)).ToList();

                var cartItems = hccCartItem.GetByIds(results.Select(i => i.CartItemId).ToList());

                if (results != null && results.Count > 0)
                {
                    foreach (MealItemReportItem result in results)
                    {
                        if (result.MealSizeId == 0)
                            result.MealSizeId = 3; // update noSize to regular size

                        var cartItem = cartItems.FirstOrDefault(i => i.CartItemID == result.CartItemId);

                        if (cartItem == null)
                            continue; // should not happen

                        var newItem = new ChefProdItem
                        {
                            ParentTypeId = result.ParentTypeId,
                            ParentId = result.ParentId,
                            MenuItemId = result.MenuItemId,
                            MealTypeId = result.MealTypeId,
                            MealSizeId = result.MealSizeId,
                            ItemName = result.ItemName, // + "-" + result.OrderNumber + " - " + result.ParentTypeId + "-" + result.ParentId,
                            MealType = Enums.GetEnumDescription(((Enums.MealTypes)result.MealTypeId)),
                            MealSize = Enums.GetEnumDescription(((Enums.CartItemSize)result.MealSizeId)),
                            Quantity = result.Quantity,
                            DeliveryDate = result.DeliveryDate,
                            OrderNumber = result.OrderNumber,
                            DayNumber = result.DayNum,
                            Prefs = string.IsNullOrWhiteSpace(result.Prefs) ? "" : result.Prefs,
                            CartItemId = result.CartItemId,
                            CartItemType = Enums.GetEnumDescription(((Enums.CartItemTypeAbbr)cartItem.ItemTypeID))
                        };

                        if(cartItem.ItemTypeID== Convert.ToInt32((Enums.CartItemType.AlaCarte)) && cartItem.Plan_IsAutoRenew==true)
                        {
                            newItem.IsFamilyStyle = "Yes";
                        }
                        else if (cartItem.ItemTypeID == Convert.ToInt32((Enums.CartItemType.AlaCarte)) && cartItem.Plan_IsAutoRenew == false)
                        {
                            newItem.IsFamilyStyle = "No";
                        }
                        else
                        {
                            newItem.IsFamilyStyle = "N/A";
                        }
                        hccUserProfile prof = hccUserProfile
                            .GetById(result.UserProfileId);

                        if (prof != null)
                        {
                            newItem.UserName = prof.ParentProfileName;
                            newItem.ProfileName = prof.ProfileName;
                        }

                        hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(newItem.CartItemId, newItem.DeliveryDate);

                        if (newItem.ParentTypeId == 1) //a la carte
                        {
                            newItem.Sides = cartItem.GetMealSideMenuItemsAsSectionString(", ");
                            newItem.Side1 = cartItem.GetMealSide1MenuItemName();
                            newItem.Side2 = cartItem.GetMealSide2MenuItemName();
                            retItems.Add(newItem);
                        }
                        else //is program plan
                        {
                            Enums.MealTypes mealType = (Enums.MealTypes)result.MealTypeId;

                            if (mealType == Enums.MealTypes.BreakfastEntree
                                || mealType == Enums.MealTypes.LunchEntree
                                || mealType == Enums.MealTypes.DinnerEntree
                                || mealType == Enums.MealTypes.OtherEntree
                                || mealType == Enums.MealTypes.ChildEntree
                                || mealType == Enums.MealTypes.Beverage
                                || mealType == Enums.MealTypes.Dessert
                                || mealType == Enums.MealTypes.Goods
                                || mealType == Enums.MealTypes.Miscellaneous
                                || mealType == Enums.MealTypes.Salad
                                || mealType == Enums.MealTypes.Snack
                                || mealType == Enums.MealTypes.Soup
                                || mealType == Enums.MealTypes.Supplement) // Non-Side Types
                            {
                                hccProgramDefaultMenu defMenu = null;
                                List<hccProgramDefaultMenu> defMenus;

                                if (newItem.ParentTypeId == 2) //plan with exception
                                {
                                    hccCartDefaultMenuException defEx = hccCartDefaultMenuException.GetById(newItem.ParentId);

                                    if (defEx != null)
                                    {
                                        defMenu = hccProgramDefaultMenu.GetById(defEx.DefaultMenuID);

                                        //newItem.Prefs = hccCartDefaultMenuExPref.GetPrefsBy(defEx.DefaultMenuExceptID)
                                        //    .Select(a => a.Name).DefaultIfEmpty("None").Aggregate((c, d) => c + ", " + d);
                                    }
                                }
                                else if (newItem.ParentTypeId == 3) //plan default
                                {
                                    defMenu = hccProgramDefaultMenu.GetById(newItem.ParentId);
                                    newItem.Prefs = "";
                                }

                                //newItem.DayNumber = defMenu.DayNumber;
                                if (defMenu == null)
                                {
                                    retItems.Add(newItem);
                                    continue;
                                }

                                defMenus = hccProgramDefaultMenu.GetBy(defMenu.CalendarID, defMenu.ProgramID);

                                //cheat to find related sides
                                List<hccProgramDefaultMenu> sides = new List<hccProgramDefaultMenu>();
                                string sideStr = string.Empty;

                                if (newItem.MealTypeId < 100)
                                    sides = defMenus.Where(a => a.DayNumber == newItem.DayNumber && (a.MealTypeID == (newItem.MealTypeId + 10))).ToList();

                                sides.ForEach(delegate(hccProgramDefaultMenu sideDefMenu)
                                {
                                    hccCartDefaultMenuException sideEx = hccCartDefaultMenuException.GetBy(sideDefMenu.DefaultMenuID, cartCal.CartCalendarID);
                                    hccMenuItem sideItem;
                                    Enums.CartItemSize sidePortionSize;
                                    string prefsString = string.Empty;

                                    if (sideEx == null)
                                    {
                                        sideItem = hccMenuItem.GetById(sideDefMenu.MenuItemID);
                                        sidePortionSize = (Enums.CartItemSize)sideDefMenu.MenuItemSizeID;
                                        prefsString = string.Empty;
                                    }
                                    else
                                    {
                                        sideItem = hccMenuItem.GetById(sideEx.MenuItemID);
                                        sidePortionSize = (Enums.CartItemSize)defMenu.MenuItemSizeID;
                                        prefsString = hccCartDefaultMenuExPref.GetPrefsBy(sideEx.DefaultMenuExceptID)
                                            .Select(a => a.Name).DefaultIfEmpty("None").Aggregate((c, d) => c + ", " + d);
                                    }

                                    if (sideItem != null)
                                    {
                                        switch (sideDefMenu.Ordinal)
                                        {
                                            case 1:
                                                {
                                                    newItem.Side1 = sideItem.Name;
                                                    if (!string.IsNullOrWhiteSpace(prefsString))
                                                        newItem.Side1 += " - " + prefsString;
                                                    break;
                                                }
                                            case 2:
                                                {
                                                    newItem.Side2 = sideItem.Name;
                                                    if (!string.IsNullOrWhiteSpace(prefsString))
                                                        newItem.Side2 += " - " + prefsString;
                                                    break;
                                                }
                                            default:
                                                {
                                                    // not supported side
                                                    break;
                                                }
                                        }
                                    }

                                    if (sidePortionSize == Enums.CartItemSize.NoSize)
                                        sidePortionSize = Enums.CartItemSize.RegularSize;

                                    if (string.IsNullOrWhiteSpace(sideStr))
                                    {
                                        if (sideItem != null)
                                            sideStr = sideItem.Name;

                                        //sideStr += " - " + sidePortionSize.ToString();

                                        if (!string.IsNullOrWhiteSpace(prefsString))
                                            sideStr += " - " + prefsString;
                                    }
                                    else
                                    {
                                        if (sideItem != null)
                                            sideStr += ", " + sideItem.Name;

                                        //sideStr += " - " + sidePortionSize.ToString();

                                        if (!string.IsNullOrWhiteSpace(prefsString))
                                            sideStr += " - " + prefsString;
                                    }
                                });

								if(string.IsNullOrWhiteSpace(sideStr) || string.Equals(sideStr, "None", StringComparison.InvariantCultureIgnoreCase)) 
								{
									sideStr = string.Empty;
								}
                                
								newItem.Sides = sideStr;
                                retItems.Add(newItem);
                            }
                            else // it is a side item
                            { // skip it
                            }
                        }
                    }
                }

                return retItems;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hcc_WeeklyMenuReport_Result> GetWeeklyMenuReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    List<hcc_WeeklyMenuReport_Result> retVals = new List<hcc_WeeklyMenuReport_Result>();
                    List<hcc_WeeklyMenuReport_Result> inVals = cont.hcc_WeeklyMenuReport(startDate, endDate).ToList();

                    inVals.ForEach(delegate(hcc_WeeklyMenuReport_Result inVal)
                    {
                        hcc_WeeklyMenuReport_Result retVal = new hcc_WeeklyMenuReport_Result
                        {
                            Allergens = string.IsNullOrWhiteSpace(inVal.Allergens) ? string.Empty : "Allergens: " + inVal.Allergens,
                            DeliveryDate = inVal.DeliveryDate,
                            Ingredients = string.IsNullOrWhiteSpace(inVal.Ingredients) ? string.Empty : "Ingredients: " + inVal.Ingredients,
                            MealTypeDesc = inVal.MealTypeDesc,
                            MealTypeID = inVal.MealTypeID,
                            MenuID = inVal.MenuID,
                            MenuItemDesc = inVal.MenuItemDesc,
                            MenuItemID = inVal.MenuItemID,
                            MenuItemName = inVal.MenuItemName,
                            NutritionalInfo = string.IsNullOrWhiteSpace(inVal.NutritionalInfo) ? string.Empty : "Nutrition Info: " + inVal.NutritionalInfo,
                            Preferences = string.IsNullOrWhiteSpace(inVal.Preferences) ? string.Empty : "Preferences: " + inVal.Preferences
                        };

                        retVals.Add(retVal);
                    });

                    return retVals;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        static string MealDelimiter { get { return Environment.NewLine + Environment.NewLine; } }
        static string ServedWith { get { return ", "; } }

        private static string AddAsEntree(MealItemReportItem reportItem)
        {
            var itemName = MealDelimiter/*support several items per day*/ + reportItem.ItemName;

            if (!string.IsNullOrWhiteSpace(reportItem.Prefs))
                itemName += " - " + reportItem.Prefs;

            return itemName;
        }

        private static string AddAsSide(MealItemReportItem reportItem)
        {
            var itemName = ServedWith/*link Sides to Entree*/ + reportItem.ItemName;

            if (!string.IsNullOrWhiteSpace(reportItem.Prefs))
                itemName += " - " + reportItem.Prefs;

            return itemName;
        }

        private static CustCalDay AgregateByDay(CustCalDay ccd, MealItemReportItem reportItem, bool init)
        {
            switch ((Enums.MealTypes)reportItem.MealTypeId)
            {
                case Enums.MealTypes.BreakfastEntree:
                    ccd.Breakfast += AddAsEntree(reportItem);
                    break;
                case Enums.MealTypes.BreakfastSide:
                    ccd.Breakfast += AddAsSide(reportItem);
                    break;
                case Enums.MealTypes.Snack:
                    ccd.Snack += AddAsEntree(reportItem);
                    break;
                case Enums.MealTypes.Dessert:
                    ccd.Dessert += AddAsEntree(reportItem);
                    break;
                case Enums.MealTypes.DinnerEntree:
                    ccd.Dinner += AddAsEntree(reportItem);
                    break;
                case Enums.MealTypes.DinnerSide:
                    ccd.Dinner += AddAsSide(reportItem);
                    break;
                case Enums.MealTypes.LunchEntree:
                    ccd.Lunch += AddAsEntree(reportItem);
                    break;
                case Enums.MealTypes.LunchSide:
                    ccd.Lunch += AddAsSide(reportItem);
                    break;
                case Enums.MealTypes.OtherEntree:
                case Enums.MealTypes.ChildEntree:
                case Enums.MealTypes.Beverage:
                case Enums.MealTypes.Salad:
                case Enums.MealTypes.Soup:
                case Enums.MealTypes.Supplement:
                case Enums.MealTypes.Goods:
                case Enums.MealTypes.Miscellaneous:
                    ccd.Other += AddAsEntree(reportItem);
                    break;
                case Enums.MealTypes.OtherSide:
                case Enums.MealTypes.ChildSide:
                    ccd.Other += AddAsSide(reportItem);
                    break;
            }
            return ccd;
        }

        private static string LeftTrim(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (value.StartsWith(MealDelimiter))
                    value = value.Remove(0, MealDelimiter.Length);

                if (value.StartsWith(ServedWith))
                    value = value.Remove(0, ServedWith.Length);
            }
            return value;
        }

        private static CustCalDay LeftTrim(CustCalDay ccd)
        {
            ccd.Beverage = LeftTrim(ccd.Beverage);
            ccd.Breakfast = LeftTrim(ccd.Breakfast);
            ccd.Dessert = LeftTrim(ccd.Dessert);
            ccd.Dinner = LeftTrim(ccd.Dinner);
            ccd.Lunch = LeftTrim(ccd.Lunch);
            ccd.Snack = LeftTrim(ccd.Snack);
            ccd.Other = LeftTrim(ccd.Other);
            return ccd;
        }

        public static List<CustCalDay> GetCustomerCalendars(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<CustCalDay> CustCals = new List<CustCalDay>();
                List<MealItemReportItem> menuItems = ReportSprocs.GetMenuItemsByDateRange(startDate, endDate, false).Where(a => a.ParentTypeId != 1)
                    .OrderBy(a => a.DeliveryDate).ThenBy(a => a.CartItemId).ThenBy(a => a.DayNum).ThenBy(a => a.MealTypeId).ToList();

                menuItems.ForEach(delegate(MealItemReportItem result)
                {
                    CustCalDay ccd = CustCals.SingleOrDefault(a => a.UserProfileId == result.UserProfileId && a.DeliveryDate == result.DeliveryDate
                       && a.OrderNumber == result.OrderNumber && a.PlanName == result.PlanName && a.DayNumber == result.DayNum);

                    if (ccd == null)
                    {
                        hccUserProfile prof = hccUserProfile.GetById(result.UserProfileId);

                        if (prof != null)
                        {
                            ccd = new CustCalDay
                            {
                                DeliveryDate = result.DeliveryDate,
                                CartItemId = result.CartItemId,
                                OrderNumber = result.OrderNumber,
                                PlanName = result.PlanName,
                                DayNumber = result.DayNum,
                                ProfileName = prof.ProfileName,
                                UserName = prof.FullName,
                                LastName = prof.LastName,
                                FirstName = prof.FirstName,
                                UserProfileId = result.UserProfileId
                            };

                            ccd = AgregateByDay(ccd, result, true);

                            CustCals.Add(ccd);
                        }
                    }
                    else
                    {
                        ccd = AgregateByDay(ccd, result, false);
                    }
                });

                // trim the crap from the begins
                foreach (CustCalDay day in CustCals)
                {
                    LeftTrim(day);
                }

                // add missed days to show in the report (for example: for 3 day plan add 4-7 days with empty lists)
                var groupsByUser = CustCals.GroupBy(i => new { i.UserName, i.DeliveryDate, i.OrderNumber, i.PlanName }).Select(r => new
                {
                    UserName = r.Key,
                    DaysPerWeek = r.GroupBy(g => g.DayNumber).Count(),
                    FirstItemInGroup = r.FirstOrDefault()
                }).ToList();

                foreach (var group in groupsByUser)
                {
                    if (group.DaysPerWeek == 7 || group.FirstItemInGroup == null)
                        continue;

                    AddMissedDays(group.DaysPerWeek, CustCals, group.FirstItemInGroup);
                }

                return CustCals;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void AddMissedDays(int daysPerWeek, List<CustCalDay> custCals, CustCalDay firstCCD)
        {
            const int DaysInWeek = 7;

            if (firstCCD == null)
                return;

            for(int i=0; i<DaysInWeek-daysPerWeek; i++)
            {
                CustCalDay ccd = new CustCalDay
                {
                    DeliveryDate = firstCCD.DeliveryDate,
                    OrderNumber = firstCCD.OrderNumber,
                    PlanName = firstCCD.PlanName,
                    DayNumber = daysPerWeek + i + 1,
                    ProfileName = firstCCD.ProfileName,
                    UserName = firstCCD.UserName,
                    LastName = firstCCD.LastName,
                    FirstName = firstCCD.FirstName
                };

                custCals.Add(ccd);
            }
        }
    }

    public class ChefProdItem
    {
        public int ParentTypeId { get; set; }
        public int ParentId { get; set; }
        public int MenuItemId { get; set; }
        public int MealTypeId { get; set; }
        public int MealSizeId { get; set; }
        public string MealType { get; set; }
        public string MealSize { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public string Prefs { get; set; }
        public string Side1 { get; set; }
        public string Side2 { get; set; }
        public string Sides { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string OrderNumber { get; set; }
        public int CartItemId { get; set; }
        public string UserName { get; set; }
        public string ProfileName { get; set; }
        public int DayNumber { get; set; }
        public string CartItemType { get; set; }
        public string IsFamilyStyle { get; set; }
        public bool? IsAutoRenew { get; set; }
    }

    public class CustCalDay
    {
        public int CartItemId { get; set; }
        public int UserProfileId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string OrderNumber { get; set; }
        public string PlanName { get; set; }
        public string UserName { get; set; }
        public string ProfileName { get; set; }
        public int DayNumber { get; set; }
        public string Breakfast { get; set; }
        public string Beverage { get; set; }
        public string Lunch { get; set; }
        public string Snack { get; set; }
        public string Dinner { get; set; }
        public string Dessert { get; set; }
        public string Other { get; set; }

        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}
