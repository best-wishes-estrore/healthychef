using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart
{
    public partial class MealTabRepeater : System.Web.UI.UserControl
    {
        private static IList<hcc_AlcMenu2_Result> _alcSides = null;

        public DateTime CurrentDeliveryDate
        {
            get
            {
                if (ViewState["DeliveryDate"] == null)
                    ViewState["DeliveryDate"] = DateTime.MinValue;

                return DateTime.Parse(ViewState["DeliveryDate"].ToString());
            }
            set { ViewState["DeliveryDate"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            lvwMealItems.ItemDataBound += lvwMealItems_ItemDataBound;
        }

        public void Bind(List<hccMenuItem> menuItems, DateTime deliveryDate)
        {
            CurrentDeliveryDate = deliveryDate;
            lvwMealItems.DataSource = menuItems;
            lvwMealItems.DataBind();
        }

        private void CheckBindAvailableSides(ListViewDataItem dataItem, int mealTypeID)
        {
            var divSidesDishes = (HtmlGenericControl)dataItem.FindControl("divSidesDishes");
            if (divSidesDishes == null)
                return;

            Enums.MealTypes mealType = (Enums.MealTypes)mealTypeID;

            if (hccMenuItem.EntreeMealTypes.Contains(mealType))
            {
                var ddlSide1 = (DropDownList)dataItem.FindControl("ddlSide1");
                BindAvailableSides(ddlSide1);

                var ddlSide2 = (DropDownList)dataItem.FindControl("ddlSide2");
                BindAvailableSides(ddlSide2);

                divSidesDishes.Visible = ddlSide1.Visible || ddlSide2.Visible;
            }
            else
            {
                divSidesDishes.Visible = false;
            }
        }

        private void BindAvailableSides(DropDownList mealSideControl)
        {
            if (mealSideControl != null && _alcSides != null)
            {
                // empty and re-bind meal side dropdowns every time
                mealSideControl.AppendDataBoundItems = false;
                mealSideControl.Items.Clear();

                mealSideControl.DataSource = _alcSides;
                mealSideControl.DataTextField = "Name";
                mealSideControl.DataValueField = "MenuItemID";
                mealSideControl.DataBind();
                mealSideControl.Visible = true;

                foreach (hcc_AlcMenu2_Result side in _alcSides)
                {
                    var item = mealSideControl.Items.FindByValue(side.MenuItemID.ToString());
                    item.Attributes.Add("cost-child", side.CostChild.ToString());
                    item.Attributes.Add("cost-small", side.CostSmall.ToString());
                    item.Attributes.Add("cost-regular", side.CostRegular.ToString());
                    item.Attributes.Add("cost-large", side.CostLarge.ToString());
                }
            }
        }

        private void SetAvailableSides(IEnumerable<hcc_AlcMenu2_Result> alcSides)
        {
            if (alcSides == null)
            {
                _alcSides = null;
                return;
            }
            
            _alcSides = alcSides.ToList();

            if (_alcSides.Count == 0)
            {
                _alcSides = null;
            }
            else if (_alcSides.First().MenuItemID != -1)
            {
                _alcSides.Insert(0, new hcc_AlcMenu2_Result() { MenuItemID = -1, Name = hccMenuItem.DefaultMealSideName });
            }
        }

        protected  void OnValidate(object sender, EventArgs e)
        {

                
        }

        private void AddMealSideToCart(hccCartItem parentCartItem, hccMenuItem menuItem, int ordinal)
        {
            if (menuItem == null || parentCartItem == null)
                throw new ArgumentNullException("menuItem or parentCartItem");

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
                    cartItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)parentCartItem.Meal_MealSizeID, menuItem.Name, string.Empty, string.Empty, parentCartItem.DeliveryDate);//,cartItem.Quantity
                    cartItem.ItemDesc = menuItem.Description;
                    cartItem.ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, parentCartItem.Meal_MealSizeID);
                    cartItem.Meal_MenuItemID = menuItem.MenuItemID;
                    cartItem.Meal_ShippingCost = hccDeliverySetting.GetBy(menuItem.MealType).ShipCost;
                    cartItem.UserProfileID = parentCartItem.UserProfileID;
                }
                cartItem.Save();
            }
        }

        private void AddCartALCMenuItem(ListViewDataItem dataItem, hccCartItem parentCartItem, string controlId, int ordinal)
        {
            var ddlSideControl = (DropDownList)dataItem.FindControl(controlId);

            if (ddlSideControl == null || string.IsNullOrEmpty(ddlSideControl.SelectedValue))
                return;

            var menuItemId = int.Parse(ddlSideControl.SelectedValue.ToString());

            if (menuItemId == -1)
                return; // "None" selected

            var menuItem = hccMenuItem.GetById(menuItemId);

            AddMealSideToCart(parentCartItem, menuItem, ordinal);
        }

        private string GetMealSides(ListViewDataItem dataItem)
        {
            var ddlSide1Control = (DropDownList)dataItem.FindControl("ddlSide1");
            var ddlSide2Control = (DropDownList)dataItem.FindControl("ddlSide2");

            if (ddlSide1Control == null || ddlSide2Control == null)
                return string.Empty;

            var result = string.Empty;
            if (ddlSide1Control.SelectedItem != null)
                result += ddlSide1Control.SelectedItem.Text;
            if (ddlSide2Control.SelectedItem != null)
                result += (string.IsNullOrEmpty(result) ? string.Empty : ", ") + ddlSide2Control.SelectedItem.Text;
            return result;
        }

        private void AddCartALCMenuItem(ListViewDataItem dataItem, hccCartItem cartItem, int mealTypeID)
        {
            var divSidesDishes = (HtmlGenericControl)dataItem.FindControl("divSidesDishes");
            if (divSidesDishes == null || !divSidesDishes.Visible)
                return;

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
            //Manoj_14.04.2017
            AddCartALCMenuItem(dataItem, cartItem, "ddlSide1", 1);
            AddCartALCMenuItem(dataItem, cartItem, "ddlSide2", 2);

            CheckBindAvailableSides(dataItem, mealTypeID);
        }

        private int GetQuantity(ListViewDataItem dataItem)
        {
            int quantity = 1;
            TextBox txtQuantity = (TextBox)dataItem.FindControl("txtQuantity");
            if (txtQuantity != null)
                quantity = int.Parse(txtQuantity.Text.Trim());
            return quantity;
        }

        private int GetSizeId(Button btnAddToCart)
        {
            ListViewDataItem dataItem = (ListViewDataItem)btnAddToCart.Parent;
            List<RadioButton> rdos = new List<RadioButton>();
            List<RepeaterItem> rptSizes = dataItem.FindControl("rptMealSizes").Controls.OfType<RepeaterItem>().ToList();
            rptSizes.ForEach(delegate(RepeaterItem ri) { rdos.AddRange(ri.Controls.OfType<RadioButton>()); });
            RadioButton rdo = rdos.SingleOrDefault(a => a.Checked);

            if (rdo != null)
            {
                return int.Parse(rdo.Attributes["value"]);
            }
            else
            {
                return int.Parse(btnAddToCart.Attributes["size"]);
            }
        }

        private int GetProfileId(ListViewDataItem dataItem)
        {
            int profileId = 0;
            try
            {
                DropDownList ddlProfiles = (DropDownList)dataItem.FindControl("ddlProfiles");
                if (ddlProfiles != null && ddlProfiles.Visible)
                    profileId = int.Parse(ddlProfiles.SelectedValue);
                else
                {
                    if (Helpers.LoggedUser != null)
                        profileId = hccUserProfile.GetParentProfileBy((Guid)Helpers.LoggedUser.ProviderUserKey).UserProfileID;
                }
                return profileId;
            }
            catch (Exception)
            {
                return profileId;
            }
        }

        protected void btnAddToCartClick(object sender, EventArgs e)
        {
            bool itemAdded = false;
            try
            {
                Button btnAddToCart = (Button)sender;
                ListViewDataItem dataItem = (ListViewDataItem)btnAddToCart.Parent;
                int menuItemId = int.Parse(lvwMealItems.DataKeys[dataItem.DataItemIndex].Value.ToString());
                hccMenuItem menuItem = hccMenuItem.GetById(menuItemId);
                hccCart userCart = hccCart.GetCurrentCart();
                MembershipUser user = Helpers.LoggedUser;
                int profileId = GetProfileId(dataItem);
                int sizeId = GetSizeId(btnAddToCart);

                hccCartItem newItem = new hccCartItem
                {   
                    CartID = userCart.CartID,
                    CreatedBy = (user == null ? Guid.Empty : (Guid)user.ProviderUserKey),
                    CreatedDate = DateTime.Now,
                    IsTaxable = menuItem.IsTaxEligible,
                    ItemDesc = menuItem.Description,
                    ItemPrice = hccMenuItem.GetItemPriceBySize(menuItem, sizeId),
                    ItemTypeID = (int)Enums.CartItemType.AlaCarte,
                    DeliveryDate = CurrentDeliveryDate,
                    Meal_MenuItemID = menuItem.MenuItemID,
                    Meal_MealSizeID = sizeId,
                    Meal_ShippingCost = hccDeliverySetting.GetBy(menuItem.MealType).ShipCost,
                    UserProfileID = profileId,
                    Quantity = GetQuantity(dataItem),
                    IsCompleted = false
                };

                newItem.GetOrderNumber(userCart);

                List<CheckBox> prefChks = new List<CheckBox>();
                List<RepeaterItem> rptPrefs = dataItem.FindControl("rptMealPrefs").Controls.OfType<RepeaterItem>().ToList();
                rptPrefs.ForEach(delegate(RepeaterItem ri) { prefChks.AddRange(ri.Controls.OfType<CheckBox>().Where(a => a.Checked)); });
                string prefsString = string.Empty;
                List<hccCartItemMealPreference> cartPrefs = new List<hccCartItemMealPreference>();

                foreach (CheckBox chkPref in prefChks)
                {
                    int prefId = int.Parse(chkPref.Attributes["value"]);
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

                //newItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)sizeId, menuItem.Name, GetMealSides(dataItem), prefsString, newItem.DeliveryDate, newItem.Quantity);
                newItem.ItemName = hccCartItem.BuildCartItemName(menuItem.MealType, (Enums.CartItemSize)sizeId, menuItem.Name, GetMealSides(dataItem), prefsString, newItem.DeliveryDate);//, newItem.Quantity

                hccCartItem existItem = hccCartItem.GetBy(userCart.CartID, newItem.ItemName, profileId);

                if (existItem == null)
                {
                    newItem.Save();
                    cartPrefs.ForEach(delegate(hccCartItemMealPreference cartPref) { cartPref.CartItemID = newItem.CartItemID; cartPref.Save(); });
                }
                else
                {
                    existItem.Quantity += newItem.Quantity;
                    existItem.AdjustQuantity(existItem.Quantity);
                }

                AddCartALCMenuItem(dataItem, existItem ?? newItem, menuItem.MealTypeID);

                itemAdded = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (itemAdded)
            {
                HealthyChef.Templates.HealthyChef.Controls.TopHeader header =
                         (HealthyChef.Templates.HealthyChef.Controls.TopHeader)this.Page.Master.FindControl("TopHeader1");

                if (header != null)
                    header.SetCartCount();
            }
        }

        public void LoadAlcMenuItems(DateTime deliveryDate, Enums.MealTypes mealType, IEnumerable<hcc_AlcMenu2_Result> alcMenu, IEnumerable<hcc_AlcMenu2_Result> alcSides = null)
        {
            CurrentDeliveryDate = deliveryDate;
            SetAvailableSides(alcSides);

            lvwMealItems.DataSource = alcMenu;
            lvwMealItems.DataBind();
        }

        void lvwMealItems_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                if (Helpers.LoggedUser != null && Roles.IsUserInRole(Helpers.LoggedUser.UserName, "Customer"))
                {
                    List<hccUserProfile> profiles = hccUserProfile.GetBy((Guid)Helpers.LoggedUser.ProviderUserKey, true);
                    if (profiles.Count > 1)
                    {
                        HtmlGenericControl divProfiles = (HtmlGenericControl)e.Item.FindControl("divProfiles");
                        if (divProfiles != null)
                        {
                            DropDownList ddlProfiles = (DropDownList)e.Item.FindControl("ddlProfiles");
                            if (ddlProfiles != null)
                            {
                                ddlProfiles.DataSource = profiles;
                                ddlProfiles.DataTextField = "ProfileName";
                                ddlProfiles.DataValueField = "UserProfileID";
                                ddlProfiles.DataBind();

                                divProfiles.Visible = true;
                            }
                        }
                    }
                }

                hcc_AlcMenu2_Result menuItem = (hcc_AlcMenu2_Result)e.Item.DataItem;

                //Meal Sizes
                List<MealSize> mealSizes = new List<MealSize>();
                if (menuItem.UseCostRegular)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.RegularSize),
                        Price = Convert.ToDecimal(menuItem.CostRegular),
                        Description = "Regular",
                        MenuId = menuItem.MenuItemID
                    });
                if (menuItem.UseCostSmall)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.SmallSize),
                        Price = Convert.ToDecimal(menuItem.CostSmall),
                        Description = "Small",
                        MenuId = menuItem.MenuItemID
                    });
                if (menuItem.UseCostLarge)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.LargeSize),
                        Price = Convert.ToDecimal(menuItem.CostLarge),
                        Description = "Large",
                        MenuId = menuItem.MenuItemID
                    });
                if (menuItem.UseCostChild)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.ChildSize),
                        Price = Convert.ToDecimal(menuItem.CostChild),
                        Description = "Child",
                        MenuId = menuItem.MenuItemID
                    });

                if (mealSizes.Count > 1)
                {
                    HtmlGenericControl divSizes = (HtmlGenericControl)e.Item.FindControl("divSizes");
                    if (divSizes != null) { divSizes.Visible = true; }

                    Repeater rptMealSizes = (Repeater)e.Item.FindControl("rptMealSizes");
                    rptMealSizes.DataSource = mealSizes.OrderBy(a => a.Price);
                    rptMealSizes.DataBind();
                }
                else if (mealSizes.Count == 1)
                {
                    Literal ltlPrice = (Literal)e.Item.FindControl("ltlPrice");
                    if (ltlPrice != null)
                        ltlPrice.Text = (mealSizes[0].Price.ToString("c"));
                    Button btnAddToCart = (Button)e.Item.FindControl("btnAddToCart");
                    btnAddToCart.Attributes.Add("size", mealSizes[0].SizeId.ToString());
                    btnAddToCart.Attributes.Add("price", mealSizes[0].Price.ToString());
                }

                //Meal Preferences
                List<hccMenuItemPreference> prefsList = hccMenuItemPreference.GetBy(menuItem.MenuItemID).ToList();
                if (prefsList.Count > 0)
                {
                    HtmlGenericControl divPrefs = (HtmlGenericControl)e.Item.FindControl("divPrefs");
                    if (divPrefs != null) { divPrefs.Visible = true; }

                    Repeater repeaterMealPrefs = (Repeater)e.Item.FindControl("rptMealPrefs");
                    repeaterMealPrefs.DataSource = prefsList;
                    repeaterMealPrefs.DataBind();
                    repeaterMealPrefs.Visible = true;
                }

                if (e.Item is ListViewDataItem)
                {
                    CheckBindAvailableSides((ListViewDataItem)e.Item, menuItem.MealTypeID);
                }
            }
        }
    }

    public class MealSize
    {
        public int MenuId { get; set; }
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }
}