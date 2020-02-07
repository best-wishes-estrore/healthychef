using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class OrderFulfillmentAlaCarte_Edit : FormControlBase
    {   // Note: this.PrimaryKeyIndex as CartItemId
        public int CurrentCalendarId
        {
            get
            {
                if (ViewState["CurrentCalendarId"] == null)
                    ViewState["CurrentCalendarId"] = 0;

                return int.Parse(ViewState["CurrentCalendarId"].ToString());
            }
            set { ViewState["CurrentCalendarId"] = value; }
        }
        public int CurrentDay
        {
            get
            {
                if (ViewState["CurrentDay"] == null)
                    ViewState["CurrentDay"] = 0;

                return int.Parse(ViewState["CurrentDay"].ToString());
            }
            set { ViewState["CurrentDay"] = value; }
        }

        public hccCartItem _currentCartItem;
        public hccCartItem CurrentCartItem
        {
            get
            {
                if (_currentCartItem == null)
                    _currentCartItem = hccCartItem.GetById(this.PrimaryKeyIndex);

                return (hccCartItem)_currentCartItem;
            }
            set { ViewState["CurrentCartItem"] = value; }
        }
      
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += base.SubmitButtonClick;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {

                }
                catch (Exception)
                {

                    throw;
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
                    ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)parentCartItem.Meal_MealSizeID, menuItem.Name, string.Empty, string.Empty, parentCartItem.DeliveryDate),//,parentCartItem.Quantity
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
                    cartItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)parentCartItem.Meal_MealSizeID, menuItem.Name, string.Empty, string.Empty, parentCartItem.DeliveryDate);//,parentCartItem.Quantity
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
            {
                var alcMenuItem = hccCartALCMenuItem.GetByOrdinal(parentCartItem.CartItemID, ordinal);
                if (alcMenuItem != null)
                {
                    var cartItem = hccCartItem.GetById(alcMenuItem.CartItemID);
                    alcMenuItem.Delete();
                    if (cartItem != null)
                        cartItem.Delete(null);
                }
                return;
            }

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
            try
            {
                if (Request.QueryString["dd"] != null && !string.IsNullOrEmpty(Request.QueryString["dd"]))
                {
                    DateTime _delDate = DateTime.ParseExact(Request.QueryString["dd"].ToString(), "M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    hccProductionCalendar cal = hccProductionCalendar.GetBy(_delDate);
                    //hccProductionCalendar cal = hccProductionCalendar.GetBy(DateTime.Parse(Request.QueryString["dd"].ToString()));

                    if (cal != null)
                    {
                        CurrentCalendarId = cal.CalendarID;

                        if (cal.DeliveryDate < DateTime.Now)
                        {
                            //chkIsCancelled.Enabled = false;
                            btnSave.Enabled = false;
                        }
                    }

                    lkbBack.PostBackUrl += "?cal=" + cal.CalendarID.ToString();
                }

                if (CurrentCartItem != null)
                {
                    // load user profile data
                    hccUserProfile profile = CurrentCartItem.UserProfile;

                    if (profile != null)
                    {
                        hccUserProfile parent = hccUserProfile.GetParentProfileBy(profile.MembershipID);
                        if (parent != null)
                            lblCustomerName.Text = parent.FirstName + " " + parent.LastName;

                        lblProfileName.Text = profile.ProfileName;
                        lblOrderNumber.Text = CurrentCartItem.OrderNumber;
                        lblDeliveryDate.Text = CurrentCartItem.DeliveryDate.ToShortDateString();
                        chkIsCancelledDisplay.Checked = CurrentCartItem.IsCancelled;
                        chkIsComplete.Checked = CurrentCartItem.IsCompleted;

                        lvwAllrgs.DataSource = profile.GetAllergens();
                        lvwAllrgs.DataBind();
                        ProfileNotesEdit_AllergenNote.CurrentUserProfileId = profile.UserProfileID;
                        ProfileNotesEdit_AllergenNote.Bind();

                        lvwPrefs.DataSource = profile.GetPreferences();
                        lvwPrefs.DataBind();
                        ProfileNotesEdit_PreferenceNote.CurrentUserProfileId = profile.UserProfileID;
                        ProfileNotesEdit_PreferenceNote.Bind();
                    }

                    BindgvwALCItems();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void SaveForm()
        {
            try
            {
                Page.Validate(this.ValidationGroup);

                if(Page.IsValid)
                {
                    if (CurrentCartItem != null)
                    {
                        if (chkIsComplete.Enabled)
                            CurrentCartItem.IsCompleted = chkIsComplete.Checked;

                        hccMenuItem menuItem = null;
                        var itemSize = Enums.CartItemSize.NoSize;

                        if (!string.IsNullOrWhiteSpace(ddlMenuItems.SelectedValue))
                            menuItem = hccMenuItem.GetById(int.Parse(ddlMenuItems.SelectedValue));

                        if (menuItem != null)
                        {
                            if (menuItem.MenuItemID != CurrentCartItem.Meal_MenuItemID)
                                CurrentCartItem.Meal_MenuItemID = menuItem.MenuItemID;

                            int selMenuOptionId = int.Parse(ddlOptions.SelectedValue);

                            if (selMenuOptionId != CurrentCartItem.Meal_MealSizeID)
                                CurrentCartItem.Meal_MealSizeID = selMenuOptionId;

                            itemSize = (Enums.CartItemSize)(CurrentCartItem.Meal_MealSizeID);

                            CurrentCartItem.ItemDesc = menuItem.Description;
                            CurrentCartItem.ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, (int)itemSize);
                        }

                        var prefsString = string.Empty;

                        var prefs = hccCartItemMealPreference.GetBy(CurrentCartItem.CartItemID);
                        prefs.ForEach(a => a.Delete());

                        foreach (ListItem item in cblPreferences.Items)
                        {
                            if (item.Selected)
                            {
                                var pref = new hccCartItemMealPreference { CartItemID = CurrentCartItem.CartItemID, PreferenceID = int.Parse(item.Value) };
                                pref.Save();

                                if (string.IsNullOrWhiteSpace(prefsString))
                                    prefsString += item.Text;
                                else
                                    prefsString += ", " + item.Text;
                            }
                        }

                        CurrentCartItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, itemSize, menuItem.Name, GetMealSides(menuItem.MealType), prefsString, CurrentCartItem.DeliveryDate);//,CurrentCartItem.Quantity
                        CurrentCartItem.Save();
                        AddUpdateCartALCMenuItem(CurrentCartItem);

                        lblFeedback.Text = "Item Saved: " + DateTime.Now.ToString();
                        lblFeedback.ForeColor = Color.Green;
                        BindgvwALCItems();
                        btnSave.Visible = false;
                        pnlItemEdit.Visible = false;
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
            chkIsCancelledDisplay.Checked = false;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void BindgvwALCItems()
        {
            gvwALCItems.DataSource = hccCartItem.GetBy(CurrentCartItem.OrderNumber).Where(a => a.ItemType == Enums.CartItemType.AlaCarte && !a.IsMealSide);
            gvwALCItems.DataBind();
        }

        void BindddlMenuItems()
        {
            hccProductionCalendar cal = hccProductionCalendar.GetBy(CurrentCartItem.DeliveryDate);

            ddlMenuItems.ClearSelection();
            ddlMenuItems.Items.Clear();

            ddlMenuItems.DataSource = hccMenuItem.GetByMenuId(cal.MenuID).Where(mi => !hccMenuItem.SideMealTypes.Contains(mi.MealType) && !mi.IsRetired);
            ddlMenuItems.DataTextField = "TypeAndName";
            ddlMenuItems.DataValueField = "MenuItemID";
            ddlMenuItems.DataBind();

            ddlMenuItems.Items.Insert(0, new ListItem("Select a Menu Item...", "-1"));
            ddlMenuItems.Enabled = true;
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
                    itemSizes.ForEach(delegate(Tuple<string, int> sizeItem)
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

                ddlOptions.SelectedIndex = ddlOptions.Items.IndexOf(ddlOptions.Items.FindByValue(CurrentCartItem.Meal_MealSizeID.ToString()));
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

            List<hccPreference> cartprefs = hccCartItemMealPreference.GetPrefsBy(CurrentCartItem.CartItemID);
            
            foreach (hccPreference pref in cartprefs)
            {
                var t = cblPreferences.Items.FindByValue(pref.PreferenceID.ToString());

                if (t != null)
                    t.Selected = true;
            }
        }

        protected void gvwALCItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PrimaryKeyIndex = int.Parse(gvwALCItems.DataKeys[gvwALCItems.SelectedIndex].Value.ToString());

            if (CurrentCartItem != null)
            {
                btnSave.Visible = true;
                BindddlMenuItems();
                pnlItemEdit.Visible = true;

                ddlMenuItems.SelectedIndex = ddlMenuItems.Items.IndexOf(ddlMenuItems.Items.FindByValue(CurrentCartItem.Meal_MenuItemID.ToString()));
                BindddlOptions(CurrentCartItem.Meal_MenuItemID.Value);
                BindcblPreferences(CurrentCartItem.Meal_MenuItemID.Value);
                BindMealSidesMenuItems(CurrentCartItem.Meal_MenuItemID.Value);

                chkIsComplete.Checked = CurrentCartItem.IsCompleted;
                chkIsCancelledDisplay.Checked = CurrentCartItem.IsCancelled;
                chkIsComplete.Enabled = true;
                //chkIsCancelled.Attributes.Add("onclick", "javascript: return confirm('Are you sure that you want to change this order item's Cancellation status?')");
            }
        }

        protected void ddlMenuItems_SelectedIndexChanged(object sender, EventArgs e)
        {
            int menuItemId = int.Parse(ddlMenuItems.SelectedValue);

            BindddlOptions(menuItemId);
            BindcblPreferences(menuItemId);
            BindMealSidesMenuItems(menuItemId);
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
            pnlMealSides.Visible = false;
            ddlMealSide1MenuItems.Items.Clear();
            ddlMealSide2MenuItems.Items.Clear();

            var menuItem = hccMenuItem.GetById(menuItemId);

            if (menuItem == null)
                return;

            if (!hccMenuItem.EntreeSideMealTypes.Keys.Contains(menuItem.MealType))
                return;

            var cal = hccProductionCalendar.GetBy(CurrentCartItem.DeliveryDate);
            if (cal == null)
                return;

            //var menuItems = hccMenuItem.GetByMenuId(cal.MenuID).Where(a => (a.UseCostChild || a.UseCostLarge || a.UseCostRegular || a.UseCostSmall) && a.MealType == hccMenuItem.EntreeSideMealTypes[menuItem.MealType] && !a.IsRetired).ToList();
            var menuItems = hccMenuItem.GetByMenuId(cal.MenuID).Where(a => a.MealType == hccMenuItem.EntreeSideMealTypes[menuItem.MealType] && !a.IsRetired).ToList();
            BindMealSideMenuItems(ddlMealSide1MenuItems, menuItems);
            BindMealSideMenuItems(ddlMealSide2MenuItems, menuItems);

            if (CurrentCartItem.MealSideMenuItems.Count > 0)
                ddlMealSide1MenuItems.SelectedIndex = ddlMealSide1MenuItems.Items.IndexOf(ddlMealSide1MenuItems.Items.FindByValue(CurrentCartItem.MealSideMenuItems[0].MenuItemID.ToString()));

            if (CurrentCartItem.MealSideMenuItems.Count > 1)
                ddlMealSide2MenuItems.SelectedIndex = ddlMealSide2MenuItems.Items.IndexOf(ddlMealSide2MenuItems.Items.FindByValue(CurrentCartItem.MealSideMenuItems[1].MenuItemID.ToString()));

            pnlMealSides.Visible = true;
        }
    }
}