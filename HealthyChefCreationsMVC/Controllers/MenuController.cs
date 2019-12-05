using HealthyChef.Common;
using HealthyChef.DAL;
using HealthyChefCreationsMVC.CustomModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthyChefCreationsMVC.Controllers
{
    public class MenuController : Controller
    {
        // GET: Menu
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(Duration = 120,VaryByParam = "mealtype;deliveryDate")]
        public PartialViewResult MealDetailsByMealId(int mealtype = 0, string deliveryDate = "")
        {
            ViewMenuModel viewMenuModel = new ViewMenuModel(mealtype, deliveryDate);
            return PartialView("~/Views/Menu/_GetMealsByMealType.cshtml", viewMenuModel);
        }

        public ActionResult Display(string mealtype, string deliveryDate = "")
        {
            if(mealtype==null)
            {
                mealtype = "2";
            }
            ViewMenuModel viewMenuModel = new ViewMenuModel(Convert.ToInt16(mealtype), deliveryDate);
            return View(viewMenuModel);
        }

        [HttpPost]
        public ActionResult AddMealToCart(AddItemToCartModel _addItemToCartModel)
        {
            
            DateTime CurrentDeliveryDate;

            bool isConverted = DateTime.TryParse(_addItemToCartModel.DeliveryDate, out CurrentDeliveryDate);
            if (isConverted == false || string.IsNullOrEmpty(_addItemToCartModel.DeliveryDate))
            {
                var calendar = hccProductionCalendar.GetNext4Calendars();
                CurrentDeliveryDate = calendar.FirstOrDefault().DeliveryDate;
            }

            var result = string.Empty;
            if (!string.IsNullOrEmpty(_addItemToCartModel.sideDishString1))
                result += _addItemToCartModel.sideDishString1;
            if (!string.IsNullOrEmpty(_addItemToCartModel.sideDishString2))
                result += (string.IsNullOrEmpty(result) ? string.Empty : ", ") + _addItemToCartModel.sideDishString2;


            bool itemAdded = false;
            try
            {
                hccMenuItem menuItem = hccMenuItem.GetById(_addItemToCartModel.menuItemID);
                hccCart userCart = hccCart.GetCurrentCart();
                MembershipUser user = Helpers.LoggedUser;

                hccCartItem newItem = new hccCartItem
                {
                    CartID = userCart.CartID,
                    CreatedBy = (user == null ? Guid.Empty : (Guid)user.ProviderUserKey),
                    CreatedDate = DateTime.Now,
                    IsTaxable = menuItem.IsTaxEligible,
                    ItemDesc = menuItem.Description,
                    ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, _addItemToCartModel.sizeId),
                    ItemTypeID = (int)Enums.CartItemType.AlaCarte,
                    DeliveryDate = CurrentDeliveryDate,
                    Meal_MenuItemID = menuItem.MenuItemID,
                    Meal_MealSizeID = _addItemToCartModel.sizeId,
                    Meal_ShippingCost = hccDeliverySetting.GetBy(menuItem.MealType).ShipCost,
                    UserProfileID = _addItemToCartModel.ProfileID,
                    Quantity = _addItemToCartModel.Quantity,
                    IsCompleted = false
                };

                newItem.GetOrderNumber(userCart);

                string prefsString = string.Empty;
                List<hccCartItemMealPreference> cartPrefs = new List<hccCartItemMealPreference>();

                if (_addItemToCartModel.prefIds != null)
                {
                    foreach (int prefId in _addItemToCartModel.prefIds)
                    {
                        hccPreference pref = hccPreference.GetById(prefId);

                        if (pref != null)
                        {
                            if (string.IsNullOrWhiteSpace(prefsString))
                                prefsString += pref.Name;
                            else
                                prefsString += ", " + pref.Name;

                            cartPrefs.Add(new hccCartItemMealPreference { CartItemID = newItem.CartItemID, PreferenceID = prefId });
                        }
                    }
                }

                //newItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)sizeId, menuItem.Name, GetMealSides(dataItem), prefsString, newItem.DeliveryDate, newItem.Quantity);
                newItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)_addItemToCartModel.sizeId, menuItem.Name, result, prefsString, newItem.DeliveryDate);//, newItem.Quantity

                hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, newItem.ItemName, _addItemToCartModel.ProfileID);

                if (existItem == null)
                {
                    newItem.Save();
                    cartPrefs.ForEach(delegate (hccCartItemMealPreference cartPref) { cartPref.CartItemID = newItem.CartItemID; cartPref.Save(); });
                }
                else
                {
                    existItem.Quantity += newItem.Quantity;
                    existItem.AdjustQuantity(existItem.Quantity);
                }

                //AddCartALCMenuItem(dataItem, existItem ?? newItem, menuItem.MealTypeID);
                var _cartItem = existItem ?? newItem;
                var alcMenuItem = hccCartALCMenuItem.GetByCartItemID(_cartItem.CartItemID, _cartItem.CartItemID);

                if (alcMenuItem == null)
                {
                    alcMenuItem = new hccCartALCMenuItem
                    {
                        CartItemID = _cartItem.CartItemID,
                        ParentCartItemID = _cartItem.CartItemID,
                        Ordinal = 0
                    };
                    alcMenuItem.Save();
                }

                itemAdded = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RedirectToAction("Display");
        }

        [HttpPost]
        public JsonResult AddMealToCartAjax(AddItemToCartModel _addItemToCartModel)
        {
            hccUserProfile parentProfile = new hccUserProfile();
            if (_addItemToCartModel.ProfileID == 0)
            {
                if (Membership.GetUser() != null)
                {
                    string userId = Membership.GetUser().ProviderUserKey.ToString();
                    parentProfile = hccUserProfile.GetParentProfileBy(new Guid(userId));
                    _addItemToCartModel.ProfileID = parentProfile.UserProfileID;
                }
            }
            string _message = "";
            int CartCount = 0;
            bool itemAdded = false;

            DateTime CurrentDeliveryDate;

            if (_addItemToCartModel.Quantity <= 0)
            {
                _message = "Quantity should be grater than zero";
                return Json(new { Success = itemAdded, Message = _message, CartCount = CartCount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if(_addItemToCartModel.DeliveryDate==null)
                {
                    _addItemToCartModel.DeliveryDate= hccProductionCalendar.GetNext4Calendars()[0].DeliveryDate.ToString();
                }
                bool isConverted = DateTime.TryParse(_addItemToCartModel.DeliveryDate, out CurrentDeliveryDate);
                if (isConverted == false || string.IsNullOrEmpty(_addItemToCartModel.DeliveryDate))
                {
                    var calendar = hccProductionCalendar.GetNext4Calendars();
                    CurrentDeliveryDate = calendar.FirstOrDefault().DeliveryDate;
                }

                if(isConverted == false)
                {
                    CurrentDeliveryDate = Convert.ToDateTime(_addItemToCartModel.DeliveryDate);
                }

                var result = string.Empty;
                if (!string.IsNullOrEmpty(_addItemToCartModel.sideDishString1))
                    result += _addItemToCartModel.sideDishString1;
                if (!string.IsNullOrEmpty(_addItemToCartModel.sideDishString2))
                    result += (string.IsNullOrEmpty(result) ? string.Empty : ", ") + _addItemToCartModel.sideDishString2;

                try
                {
                    hccMenuItem menuItem = hccMenuItem.GetById(_addItemToCartModel.menuItemID);
                    hccCart userCart = hccCart.GetCurrentCart();
                    MembershipUser user = Helpers.LoggedUser;



                    hccCartItem newItem = new hccCartItem
                    {
                        CartID = userCart.CartID,
                        CreatedBy = (user == null ? Guid.Empty : (Guid)user.ProviderUserKey),
                        CreatedDate = DateTime.Now,
                        IsTaxable = menuItem.IsTaxEligible,
                        ItemDesc = menuItem.Description,
                        ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, _addItemToCartModel.sizeId),
                        ItemTypeID = (int)Enums.CartItemType.AlaCarte,
                        DeliveryDate = CurrentDeliveryDate,
                        Meal_MenuItemID = menuItem.MenuItemID,
                        Meal_MealSizeID = _addItemToCartModel.sizeId,
                        Meal_ShippingCost = hccDeliverySetting.GetBy(menuItem.MealType).ShipCost,
                        UserProfileID = _addItemToCartModel.ProfileID,
                        Quantity = _addItemToCartModel.Quantity,
                        IsCompleted = false,
                        Plan_IsAutoRenew = _addItemToCartModel.isFamilyStyle
                    };

                    newItem.GetOrderNumber(userCart);

                    string prefsString = string.Empty;
                    List<hccCartItemMealPreference> cartPrefs = new List<hccCartItemMealPreference>();

                    if (_addItemToCartModel.prefIds != null)
                    {
                        foreach (int prefId in _addItemToCartModel.prefIds)
                        {
                            hccPreference pref = hccPreference.GetById(prefId);

                            if (pref != null)
                            {
                                if (string.IsNullOrWhiteSpace(prefsString))
                                    prefsString += pref.Name;
                                else
                                    prefsString += ", " + pref.Name;

                                cartPrefs.Add(new hccCartItemMealPreference { CartItemID = newItem.CartItemID, PreferenceID = prefId });
                            }
                        }
                    }

                    //newItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)sizeId, menuItem.Name, GetMealSides(dataItem), prefsString, newItem.DeliveryDate, newItem.Quantity);
                    newItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)_addItemToCartModel.sizeId, menuItem.Name, result, prefsString, newItem.DeliveryDate);//, newItem.Quantity

                    hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, newItem.ItemName, _addItemToCartModel.ProfileID, newItem.Plan_IsAutoRenew);

                    if (existItem == null)
                    {
                        
                        newItem.Save();
                        if (_addItemToCartModel.sideDishString1Id != 0 || _addItemToCartModel.sideDishString2Id != 0)
                        {
                            AddUpdateCartALCMenuItem(newItem, _addItemToCartModel.sideDishString1Id.ToString(), _addItemToCartModel.sideDishString2Id.ToString());
                        }
                        cartPrefs.ForEach(delegate (hccCartItemMealPreference cartPref) { cartPref.CartItemID = newItem.CartItemID; cartPref.Save(); });
                    }
                    else
                    {
                        existItem.Quantity += newItem.Quantity;
                        existItem.AdjustQuantity(existItem.Quantity);
                        if (_addItemToCartModel.sideDishString1Id != 0 || _addItemToCartModel.sideDishString2Id != 0)
                        {
                            AddUpdateCartALCMenuItem(existItem, _addItemToCartModel.sideDishString1Id.ToString(), _addItemToCartModel.sideDishString2Id.ToString());
                        }
                    }

                    //AddCartALCMenuItem(dataItem, existItem ?? newItem, menuItem.MealTypeID);
                    var _cartItem = existItem ?? newItem;
                    var alcMenuItem = hccCartALCMenuItem.GetByCartItemID(_cartItem.CartItemID, _cartItem.CartItemID);

                    if (alcMenuItem == null)
                    {
                        alcMenuItem = new hccCartALCMenuItem
                        {
                            CartItemID = _cartItem.CartItemID,
                            ParentCartItemID = _cartItem.CartItemID,
                            Ordinal = 0
                        };
                        alcMenuItem.Save();
                    }

                    itemAdded = true;

                    //get cart count
                    //hccCart cart = null;
                    //if (user == null)
                    //    cart = hccCart.GetCurrentCart();
                    //else
                    //{
                    //    cart = hccCart.GetCurrentCart(user);
                    //}

                    if (userCart != null)
                    {
                        List<hccCartItem> cartItems = hccCartItem.GetWithoutSideItemsBy(userCart.CartID);
                        hccCartItem obj = new hccCartItem();

                        CartCount = cartItems.Count;
                    }
                    else
                    {
                        CartCount = 0;
                    }
                }
                catch (Exception ex)
                {
                   _message = "Error in adding meal to cart " + ex.Message;
                    return Json(new { Success = false, Message = _message, CartCount = CartCount }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { Success = itemAdded, Message = RenderRazorViewToString("~/Views/Menu/_YourcartPage.cshtml",new ViewMenuModel(0,"")), CartCount = CartCount }, JsonRequestBehavior.AllowGet);
            }
        }
        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }
        private void AddUpdateCartALCMenuItem(hccCartItem cartItem,string sideDishString1,string sideDishString2)
        {
            var alcMenuItem = hccCartALCMenuItem.GetByCartItemID(cartItem.CartItemID, cartItem.CartItemID);

            if (alcMenuItem == null)
            {
                alcMenuItem = new hccCartALCMenuItem
                {
                    CartItemID = cartItem.CartItemID,
                    ParentCartItemID = cartItem.CartItemID,
                    Ordinal = 0
                };
                alcMenuItem.Save();
            }
            if (sideDishString1 != "0")
            {
                AddUpdateCartALCMenuItem(cartItem, sideDishString1, 1);
            }
            if (sideDishString2 != "0")
            {
                AddUpdateCartALCMenuItem(cartItem, sideDishString2, 2);
            }
        }
        private void AddUpdateCartALCMenuItem(hccCartItem parentCartItem, string sideDishString, int ordinal)
        {
            if (sideDishString == null || string.IsNullOrEmpty(sideDishString))
                return;

            var menuItemId = int.Parse(sideDishString.ToString());

            if (menuItemId == -1)
                return; // "None" selected

            var menuItem = hccMenuItem.GetById(menuItemId);

            AddUpdateMealSideToCart(parentCartItem, menuItem, ordinal);
        }
        private void AddUpdateMealSideToCart(hccCartItem parentCartItem, hccMenuItem menuItem, int ordinal)
        {
            if (menuItem == null || parentCartItem == null)
                throw new ArgumentException();

            var alcMenuItem = hccCartALCMenuItem.GetByOrdinal(parentCartItem.CartItemID, ordinal);
            if (alcMenuItem == null)
            {
                var newItem = new hccCartItem
                {
                    CartID = parentCartItem.CartID,
                    CreatedBy = parentCartItem.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsTaxable = menuItem.IsTaxEligible,
                    ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)parentCartItem.Meal_MealSizeID, menuItem.Name, string.Empty, string.Empty, parentCartItem.DeliveryDate),//, parentCartItem.Quantity
                    ItemDesc = menuItem.Description,
                    ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, parentCartItem.Meal_MealSizeID),
                    ItemTypeID = (int)Enums.CartItemType.AlaCarte,
                    DeliveryDate = parentCartItem.DeliveryDate,
                    Meal_MenuItemID = menuItem.MenuItemID,
                    Meal_MealSizeID = parentCartItem.Meal_MealSizeID,
                    Meal_ShippingCost = hccDeliverySetting.GetBy(menuItem.MealType).ShipCost,
                    UserProfileID = parentCartItem.UserProfileID,
                    Quantity = parentCartItem.Quantity,
                    OrderNumber = parentCartItem.OrderNumber,
                    IsCompleted = false
                };
                newItem.Save();

                alcMenuItem = new hccCartALCMenuItem
                {
                    CartItemID = newItem.CartItemID,
                    ParentCartItemID = parentCartItem.CartItemID,
                    Ordinal = ordinal
                };
                alcMenuItem.Save();
            }
            else
            {
                var cartItem = hccCartItem.GetById(alcMenuItem.CartItemID);

                cartItem.Quantity = parentCartItem.Quantity;
                cartItem.Meal_MealSizeID = parentCartItem.Meal_MealSizeID;

                if (cartItem.Meal_MenuItemID != menuItem.MenuItemID)
                {
                    cartItem.IsTaxable = menuItem.IsTaxEligible;
                    cartItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)parentCartItem.Meal_MealSizeID, menuItem.Name, string.Empty, string.Empty, parentCartItem.DeliveryDate);//, parentCartItem.Quantity
                    cartItem.ItemDesc = menuItem.Description;
                    cartItem.ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, parentCartItem.Meal_MealSizeID);
                    cartItem.Meal_MenuItemID = menuItem.MenuItemID;
                    cartItem.Meal_ShippingCost = hccDeliverySetting.GetBy(menuItem.MealType).ShipCost;
                    cartItem.UserProfileID = parentCartItem.UserProfileID;
                }
                cartItem.Save();
            }
        }

        [HttpGet]
        public ActionResult ProductDescription(string ItemName)
        {
            ItemName = (System.Web.HttpContext.Current.Request.Url).ToString().Split('=')[1];
            //ItemName = System.Web.HttpContext.Current.Server.UrlDecode(System.Web.HttpContext.Current.Request.QueryString["ItemName"]);
            ViewMenuModel viewMenu = new ViewMenuModel(ItemName);
            return View("~/Views/Menu/_ProductDescription.cshtml", model: viewMenu);
        }
    }
}