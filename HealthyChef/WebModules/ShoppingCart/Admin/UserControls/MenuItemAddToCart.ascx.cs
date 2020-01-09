using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.Common.Events;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class MenuItemAddToCart : FormControlBase
    {// primaryKey as hccCartId
        protected override void OnInit(EventArgs e)
        {
            this.Page.MaintainScrollPositionOnPostBack = true;

            base.OnInit(e);

            ddlDeliveryDates.SelectedIndexChanged += ddlDeliveryDates_SelectedIndexChanged;
            ddlMenuItems.SelectedIndexChanged += ddlMenuItems_SelectedIndexChanged;

            btnSave.Click += base.SubmitButtonClick;
            btnCancel.Click += base.CancelButtonClick;



        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
            }
            if ((ddlMenuItems.SelectedValue == "" || ddlMenuItems.SelectedValue == "-1"))
            {
                Session["NoOfSide"] = null;
            }
            if (Session["NoOfSide"] != null)
            {

                int NoofSideDishes = Convert.ToInt32(Session["NoOfSide"]);
                lblNoOfSideItems.Text = Convert.ToString(Session["NoOfSide"]);

                if (NoofSideDishes == 0)
                {
                    ddlMealSide1MenuItems.Enabled = false;
                    ddlMealSide2MenuItems.Enabled = false;
                }
                else if (NoofSideDishes == 1)
                {
                    ddlMealSide1MenuItems.Enabled = true;
                    ddlMealSide2MenuItems.Enabled = false;
                }
                else if (NoofSideDishes == 2)
                {
                    ddlMealSide1MenuItems.Enabled = true;
                    ddlMealSide2MenuItems.Enabled = true;
                }


            }
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

        private void AddUpdateCartALCMenuItem(hccCartItem parentCartItem, DropDownList ddlSideControl, int ordinal)
        {
            if (ddlSideControl == null || string.IsNullOrEmpty(ddlSideControl.SelectedValue))
                return;

            var menuItemId = int.Parse(ddlSideControl.SelectedValue.ToString());

            if (menuItemId == -1)
                return; // "None" selected

            var menuItem = hccMenuItem.GetById(menuItemId);

            AddUpdateMealSideToCart(parentCartItem, menuItem, ordinal);
        }

        private string GetMealSides(Enums.MealTypes entreeMealType)
        {
            if (!hccMenuItem.EntreeMealTypes.Contains(entreeMealType))
                return string.Empty;

            var sideMealType = hccMenuItem.EntreeSideMealTypes[entreeMealType];
            var sideMealTypePrefix = sideMealType.ToString() + " - ";
            var result = string.Empty;

            if (ddlMealSide1MenuItems.SelectedItem != null)
                result += ddlMealSide1MenuItems.SelectedItem.Text.Replace(sideMealTypePrefix, string.Empty);
            if (ddlMealSide2MenuItems.SelectedItem != null)
                result += (string.IsNullOrEmpty(result) ? string.Empty : ", ") + ddlMealSide2MenuItems.SelectedItem.Text.Replace(sideMealTypePrefix, string.Empty);

            return result;
        }

        private void AddUpdateCartALCMenuItem(hccCartItem cartItem)
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

            AddUpdateCartALCMenuItem(cartItem, ddlMealSide1MenuItems, 1);
            AddUpdateCartALCMenuItem(cartItem, ddlMealSide2MenuItems, 2);
        }

        protected override void LoadForm()
        {
            BindddlDeliveryDates();
            BindddlProfiles();
        }

        protected override void SaveForm()
        {
            try
            {
                hccCart userCart = hccCart.GetById(this.PrimaryKeyIndex);
                MembershipUser user = Membership.GetUser(userCart.AspNetUserID);
                bool isFamilyStyle = false;

                if (user != null)
                {
                    hccMenuItem menuItem = hccMenuItem.GetById(int.Parse(ddlMenuItems.SelectedValue));
                    var itemSize = (Enums.CartItemSize)(int.Parse(ddlOptions.SelectedValue));
                    if (chkFamilyStyle.Checked)
                    {
                        isFamilyStyle = true;
                    }
                    int profileId = 0;
                    if (divProfiles.Visible)
                    {
                        profileId = int.Parse(ddlProfiles.SelectedValue);
                    }
                    else
                        profileId = hccUserProfile.GetParentProfileBy(userCart.AspNetUserID.Value).UserProfileID;

                    if (userCart != null)
                    {
                        hccCartItem cartItem = new hccCartItem
                        {
                            CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey,
                            CreatedDate = DateTime.Now,
                            IsTaxable = menuItem.IsTaxEligible,
                            ItemDesc = menuItem.Description,
                            ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, (int)itemSize),
                            ItemTypeID = (int)Enums.CartItemType.AlaCarte,
                            DeliveryDate = DateTime.Parse(ddlDeliveryDates.SelectedItem.Text),
                            Meal_MealSizeID = (int)itemSize,
                            Meal_MenuItemID = menuItem.MenuItemID,
                            Meal_ShippingCost = hccDeliverySetting.GetBy(menuItem.MealType).ShipCost,
                            UserProfileID = profileId,
                            Quantity = int.Parse(txtQuantity.Text.Trim()),
                            Plan_IsAutoRenew = isFamilyStyle,
                            IsCompleted = false
                        };

                        cartItem.GetOrderNumber(userCart);

                        List<hccCartItemMealPreference> prefsList = new List<hccCartItemMealPreference>();

                        foreach (ListItem item in cblPreferences.Items)
                        {
                            if (item.Selected)
                            {
                                hccCartItemMealPreference pref =
                                    new hccCartItemMealPreference { CartItemID = cartItem.CartItemID, PreferenceID = int.Parse(item.Value) };
                                prefsList.Add(pref);
                            }
                        }

                        var prefsString = string.Empty;

                        if (prefsList.Count > 0)
                            prefsString = prefsList
                                .Select(a => hccPreference.GetById(a.PreferenceID))
                                .Select(a => a.Name).DefaultIfEmpty(string.Empty).Aggregate((a, b) => a + ", " + b);

                        cartItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, itemSize, menuItem.Name, GetMealSides(menuItem.MealType), prefsString, cartItem.DeliveryDate);//,cartItem.Quantity

                        hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, cartItem.ItemName, profileId);

                        if (existItem == null)
                        {
                            cartItem.CartID = userCart.CartID;
                            cartItem.Save();

                            if (cartItem.CartItemID > 0)
                                prefsList.ForEach(delegate (hccCartItemMealPreference cartPref)
                                {
                                    cartPref.CartItemID = cartItem.CartItemID;
                                    cartPref.Save();
                                });

                            AddUpdateCartALCMenuItem(cartItem);
                            OnSaved(new ControlSavedEventArgs(cartItem.CartItemID));
                            chkFamilyStyle.Checked = false;
                        }
                        else
                        {
                            existItem.Quantity += cartItem.Quantity;
                            if (existItem.AdjustQuantity(existItem.Quantity))
                            {
                                AddUpdateCartALCMenuItem(existItem);
                                OnSaved(new ControlSavedEventArgs(existItem.CartItemID));
                                chkFamilyStyle.Checked = false;
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void ClearForm()
        {
            ddlDeliveryDates.ClearSelection();
            ddlMenuItems.ClearSelection();
            ddlMenuItems.Enabled = false;
            ddlOptions.ClearSelection();
            ddlOptions.Enabled = false;
            divPreferences.Visible = false;
            cblPreferences.ClearSelection();
            ddlProfiles.ClearSelection();
            divProfiles.Visible = false;
            txtQuantity.Text = "1";
            chkFamilyStyle.Checked = false;
        }

        void BindddlDeliveryDates()
        {
            if (ddlDeliveryDates.Items.Count == 0)
            {
                ddlDeliveryDates.ClearSelection();
                ddlDeliveryDates.Items.Clear();

                ddlDeliveryDates.DataSource = hccProductionCalendar.GetNext4Last2Calendars(); //Manoj_14.04.2017
                ddlDeliveryDates.DataTextField = "DeliveryDate";
                ddlDeliveryDates.DataTextFormatString = "{0:MM/dd/yyy}";
                ddlDeliveryDates.DataValueField = "CalendarID";
                ddlDeliveryDates.DataBind();

                ddlDeliveryDates.Items.Insert(0, new ListItem("Select a Delivery Date...", "-1"));
                ddlDeliveryDates.Enabled = true;

                ddlMenuItems.Items.Insert(0, new ListItem("Select a Menu Item...", "-1"));
                ddlOptions.Items.Insert(0, new ListItem("Select a Size...", "-1"));
            }
        }

        private static hccProductionCalendar cal = null;

        void ddlDeliveryDates_SelectedIndexChanged(object sender, EventArgs e)
        {
            int prodCalId = int.Parse(ddlDeliveryDates.SelectedValue);

            if (prodCalId > 0)
            {
                cal = hccProductionCalendar.GetById(prodCalId);

                BindddlMenuItems(cal.MenuID);
            }
        }

        void BindddlMenuItems(int menuId)
        {
            ddlMenuItems.ClearSelection();
            ddlMenuItems.Items.Clear();

            List<hccMenuItem> menuItems =
                hccMenuItem.GetByMenuId(menuId).Where(a => (a.UseCostChild || a.UseCostLarge || a.UseCostRegular || a.UseCostSmall) && !hccMenuItem.SideMealTypes.Contains(a.MealType) && !a.IsRetired).ToList();

            ddlMenuItems.DataSource = menuItems;
            ddlMenuItems.DataTextField = "TypeAndName";
            ddlMenuItems.DataValueField = "MenuItemID";
            ddlMenuItems.DataBind();

            ddlMenuItems.Items.Insert(0, new ListItem("Select a Menu Item...", "-1"));
            ddlMenuItems.Enabled = true;
        }

        void ddlMenuItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            int menuItemId = int.Parse(ddlMenuItems.SelectedValue);

            if (menuItemId > 0)
            {
                BindddlOptions(menuItemId);
                BindcblPreferences(menuItemId);
                BindMealSidesMenuItems(menuItemId);
            }
            else
            {
                ddlOptions.Enabled = false;
                divPreferences.Visible = false;
                pnlMealSides.Visible = false;
            }
        }

        void BindMealSideMenuItems(DropDownList mealSideControl, List<hccMenuItem> menuItems)
        {
            mealSideControl.DataSource = menuItems;
            mealSideControl.DataTextField = "TypeAndName";
            mealSideControl.DataValueField = "MenuItemID";
            mealSideControl.DataBind();

            mealSideControl.Items.Insert(0, new ListItem(hccMenuItem.DefaultMealSideName, "-1"));
        }

        void BindMealSidesMenuItems(int menuItemId)
        {


            ddlMealSide1MenuItems.Items.Clear();
            ddlMealSide2MenuItems.Items.Clear();

            var menuItem = hccMenuItem.GetById(menuItemId);
            Session["NoOfSide"] = menuItem.NoofSideDishes;
            lblNoOfSideItems.Text = Convert.ToString(menuItem.NoofSideDishes);
            if (menuItem.NoofSideDishes == 0)
            {
                ddlMealSide1MenuItems.Enabled = false;
                ddlMealSide2MenuItems.Enabled = false;
            }
            else if (menuItem.NoofSideDishes == 1)
            {
                ddlMealSide1MenuItems.Enabled = true;
                ddlMealSide2MenuItems.Enabled = false;
            }
            else if (menuItem.NoofSideDishes == 2)
            {
                ddlMealSide1MenuItems.Enabled = true;
                ddlMealSide2MenuItems.Enabled = true;
            }
            if (menuItem == null)
                return;

            if (!hccMenuItem.EntreeSideMealTypes.Keys.Contains(menuItem.MealType))
            {
                pnlMealSides.Visible = false;
                return;
            }

            if (cal == null)
                return;

            //var menuItems = hccMenuItem.GetByMenuId(cal.MenuID).Where(a => (a.UseCostChild || a.UseCostLarge || a.UseCostRegular || a.UseCostSmall) && a.MealType == hccMenuItem.EntreeSideMealTypes[menuItem.MealType] && !a.IsRetired).ToList();//!a.IsRetired Manoj
            var menuItems = hccMenuItem.GetByMenuId(cal.MenuID).Where(a => a.MealType == hccMenuItem.EntreeSideMealTypes[menuItem.MealType] && !a.IsRetired).ToList();//!a.IsRetired Manoj

            BindMealSideMenuItems(ddlMealSide1MenuItems, menuItems);
            BindMealSideMenuItems(ddlMealSide2MenuItems, menuItems);

            pnlMealSides.Visible = true;
        }

        void BindddlOptions(int menuItemId)
        {
            ddlOptions.ClearSelection();
            ddlOptions.Items.Clear();

            List<Tuple<string, int>> itemSizes = Enums.GetEnumAsTupleList(typeof(Enums.CartItemSize));
            List<Tuple<string, int>> pricedSizes = new List<Tuple<string, int>>();
            itemSizes.Remove(itemSizes.Single(a => ((Enums.CartItemSize)(a.Item2)) == Enums.CartItemSize.NoSize));

            hccMenuItem menuItem = hccMenuItem.GetById(menuItemId);

            if (menuItem != null)
            {
                if (!menuItem.UseCostChild)
                    itemSizes.Remove(itemSizes.Single(a => ((Enums.CartItemSize)(a.Item2)) == Enums.CartItemSize.ChildSize));

                if (!menuItem.UseCostSmall)
                    itemSizes.Remove(itemSizes.Single(a => ((Enums.CartItemSize)(a.Item2)) == Enums.CartItemSize.SmallSize));

                if (!menuItem.UseCostRegular)
                    itemSizes.Remove(itemSizes.Single(a => ((Enums.CartItemSize)(a.Item2)) == Enums.CartItemSize.RegularSize));

                if (!menuItem.UseCostLarge)
                    itemSizes.Remove(itemSizes.Single(a => ((Enums.CartItemSize)(a.Item2)) == Enums.CartItemSize.LargeSize));

                if (itemSizes.Count > 0)
                {
                    itemSizes.ForEach(delegate (Tuple<string, int> sizeItem)
                    {
                        Enums.CartItemSize cis = ((Enums.CartItemSize)(sizeItem.Item2));

                        switch (cis)
                        {
                            case Enums.CartItemSize.ChildSize:
                                pricedSizes.Add(new Tuple<string, int>(sizeItem.Item1 + " - " + menuItem.CostChild.ToString("c"), sizeItem.Item2));
                                break;
                            case Enums.CartItemSize.SmallSize:
                                pricedSizes.Add(new Tuple<string, int>(sizeItem.Item1 + " - " + menuItem.CostSmall.ToString("c"), sizeItem.Item2));
                                break;
                            case Enums.CartItemSize.RegularSize:
                                pricedSizes.Add(new Tuple<string, int>(sizeItem.Item1 + " - " + menuItem.CostRegular.ToString("c"), sizeItem.Item2));
                                break;
                            case Enums.CartItemSize.LargeSize:
                                pricedSizes.Add(new Tuple<string, int>(sizeItem.Item1 + " - " + menuItem.CostLarge.ToString("c"), sizeItem.Item2));
                                break;
                            default: break;
                        }
                    });

                    ddlOptions.DataSource = pricedSizes;
                    ddlOptions.DataTextField = "Item1";
                    ddlOptions.DataValueField = "Item2";
                    ddlOptions.DataBind();

                    if (pricedSizes.Count > 1)
                    {
                        ddlOptions.Items.Insert(0, new ListItem("Select Size...", "-1"));
                        ddlOptions.Enabled = true;
                    }
                }
                else
                {
                    itemSizes.Add(new Tuple<string, int>(Enums.CartItemSize.NoSize.ToString(), (int)Enums.CartItemSize.NoSize));
                    ddlOptions.DataSource = itemSizes;
                    ddlOptions.DataTextField = "Item1";
                    ddlOptions.DataValueField = "Item2";
                    ddlOptions.DataBind();

                    ddlOptions.Enabled = false;
                }
            }
        }

        void BindcblPreferences(int menuItemId)
        {
            cblPreferences.ClearSelection();
            cblPreferences.Items.Clear();

            hccMenuItem menuItem = hccMenuItem.GetById(menuItemId);

            if (menuItem != null)
            {
                List<hccPreference> prefs = menuItem.GetPreferences();

                if (prefs.Count > 0)
                {
                    cblPreferences.DataSource = prefs;
                    cblPreferences.DataTextField = "Name";
                    cblPreferences.DataValueField = "PreferenceID";
                    cblPreferences.DataBind();

                    divPreferences.Visible = true;
                }
                else
                    divPreferences.Visible = false;
            }
        }

        void BindddlProfiles()
        {
            // get all profiles under the CartUserASPNetId
            hccCart userCart = hccCart.GetById(this.PrimaryKeyIndex);
            List<hccUserProfile> memberProfiles = hccUserProfile.GetBy(userCart.AspNetUserID.Value, true);

            if (memberProfiles.Count > 1)
            {
                ddlProfiles.DataSource = memberProfiles;
                ddlProfiles.DataTextField = "ProfileName";
                ddlProfiles.DataValueField = "UserProfileID";
                ddlProfiles.DataBind();

                ddlProfiles.Items.Insert(0, new ListItem("Select a Profile...", "-1"));
                divProfiles.Visible = true;

                rfvProfiles.ValidationGroup = this.ValidationGroup;
            }
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }
    }
}