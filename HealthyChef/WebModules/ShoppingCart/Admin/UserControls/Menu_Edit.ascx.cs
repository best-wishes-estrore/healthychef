using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HealthyChef.DAL;
using HealthyChef.WebModules.Components.HealthyChef;
using HealthyChef.Common;
using HealthyChef.Common.Events;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class Menu_Edit : FormControlBase
    {
        protected hccMenu CurrentMenu { get; set; }
        protected List<EntityPicker> EntityLists
        {
            get
            {
                return this.Controls.OfType<EntityPicker>().ToList();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Bind();
            }
        }

        protected override void LoadForm()
        {
            CurrentMenu = hccMenu.GetById(this.PrimaryKeyIndex);
            LoadPickers();

            if (CurrentMenu != null)
            {
                var items = hccMenuItem.GetAll()
                    .Where(a => !a.IsRetired)
                    .Select(a => new
                    {
                        Name = a.Name,
                        MealType = (HealthyChef.Common.Enums.MealTypes)a.MealTypeID,
                        MenuItemID = a.MenuItemID
                    });

                txtMenuName.Text = CurrentMenu.Name;
            }
        }

        protected override void SaveForm()
        {
            CurrentMenu = hccMenu.GetById(this.PrimaryKeyIndex);

            if (CurrentMenu == null)
                CurrentMenu = new hccMenu() { CreatedBy = (Guid)Helpers.LoggedUser.ProviderUserKey, CreatedDate = DateTime.Now };

            CurrentMenu.Name = txtMenuName.Text.Trim();
            CurrentMenu.Save();

            SavePickers();

            OnSaved(new ControlSavedEventArgs(CurrentMenu.MenuID));
        }

        protected override void ClearForm()
        {
            this.PrimaryKeyIndex = 0;
            txtMenuName.Text = string.Empty;
            EntityLists.ForEach(a => a.Reset());
        }

        protected void LoadPickers()
        {
            //Breakfast
            LoadEntityPicker(epBreakfast_BreakfastEntrees, Enums.MealTypes.BreakfastEntree);
            LoadEntityPicker(epBreakfast_BreakfastSides, Enums.MealTypes.BreakfastSide);
            //LoadEntityPicker(epBreakfast_OtherEntrees, Enums.MealTypes.OtherEntree);
            //LoadEntityPicker(epBreakfast_OtherSides, Enums.MealTypes.OtherSide);
            //LoadEntityPicker(epBreakfast_Misc, Enums.MealTypes.Miscellaneous);

            //Lunch
            LoadEntityPicker(epLunch_LunchEntrees, Enums.MealTypes.LunchEntree);
            LoadEntityPicker(epLunch_LunchSides, Enums.MealTypes.LunchSide);
            //LoadEntityPicker(epLunch_OtherEntrees, Enums.MealTypes.OtherEntree);
            //LoadEntityPicker(epLunch_OtherSides, Enums.MealTypes.OtherSide);
            //LoadEntityPicker(epLunch_Soups, Enums.MealTypes.Soup);
            //LoadEntityPicker(epLunch_Salads, Enums.MealTypes.Salad);
            //LoadEntityPicker(epLunch_Misc, Enums.MealTypes.Miscellaneous);

            //Dinner
            LoadEntityPicker(epDinner_DinnerEntrees, Enums.MealTypes.DinnerEntree);
            LoadEntityPicker(epDinner_DinnerSides, Enums.MealTypes.DinnerSide);
            //LoadEntityPicker(epDinner_OtherEntrees, Enums.MealTypes.OtherEntree);
            //LoadEntityPicker(epDinner_OtherSides, Enums.MealTypes.OtherSide);
            //LoadEntityPicker(epDinner_Soups, Enums.MealTypes.Soup);
            //LoadEntityPicker(epDinner_Salads, Enums.MealTypes.Salad);
            //LoadEntityPicker(epDinner_Misc, Enums.MealTypes.Miscellaneous);

            //kidsmeal
            LoadEntityPicker(epChild_ChildEntrees, Enums.MealTypes.ChildEntree);
            LoadEntityPicker(epChild_ChildSides, Enums.MealTypes.ChildSide);
            //LoadEntityPicker(epChild_OtherEntrees, Enums.MealTypes.OtherEntree);
            //LoadEntityPicker(epChild_OtherSides, Enums.MealTypes.OtherSide);
            //LoadEntityPicker(epChild_Misc, Enums.MealTypes.Miscellaneous);

            //desserts
            LoadEntityPicker(epDeserts, Enums.MealTypes.Dessert);

            //other goods
            LoadEntityPicker(epOther_OtherEntrees, Enums.MealTypes.OtherEntree);
            LoadEntityPicker(epOther_OtherSides, Enums.MealTypes.OtherSide);
            LoadEntityPicker(epOther_Soups, Enums.MealTypes.Soup);
            LoadEntityPicker(epOther_Salads, Enums.MealTypes.Salad);
            LoadEntityPicker(epOther_Beverages, Enums.MealTypes.Beverage);
            LoadEntityPicker(epOther_Snacks, Enums.MealTypes.Snack);
            LoadEntityPicker(epOther_Supplements, Enums.MealTypes.Supplement);
            LoadEntityPicker(epOther_Goods, Enums.MealTypes.Goods);
            LoadEntityPicker(epOther_Misc, Enums.MealTypes.Miscellaneous);
        }

        protected void SavePickers()
        {
            //delete existing
            CurrentMenu.RemoveItems();
            
            //save all pickers
            foreach (EntityPicker picker in EntityLists)
            {
                List<int> selKeys = picker.GetSelectedKeysForCheckBoxItem();
                List<hccMenuItem> newItemList = new List<hccMenuItem>();
                List<ListItem> listItems = picker.GetCheckBoxSelectedItems();

                selKeys.ForEach(delegate(int key)
                {
                    hccMenuItem saveItem = hccMenuItem.GetById(key);
                    newItemList.Add(saveItem);                                 
                });

                CurrentMenu.AddItems(newItemList);

                listItems.ForEach(delegate (ListItem selectedItems)
                {
                    int menuItemId = Convert.ToInt32(selectedItems.Value);

                    CurrentMenu.IsSelectedFalse(menuItemId, selectedItems.Selected);
                });
            }
        }

        protected void LoadEntityPicker(EntityPicker picker, Enums.MealTypes mealType)
        {
            List<hccMenuItem> allItemsList = hccMenuItem.GetBy(mealType);
            List<hccMenuItem> selectedItemsList = new List<hccMenuItem>();

            if (CurrentMenu != null)
                selectedItemsList = CurrentMenu.GetMenuItems(false, mealType);

            picker.BindMenuItems<hccMenuItem>(allItemsList, selectedItemsList);
        }

        protected void cstName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            hccMenu existMenu = hccMenu.GetBy(txtMenuName.Text.Trim());

            if (existMenu != null && existMenu.MenuID != this.PrimaryKeyIndex)
                args.IsValid = false;
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }
    }
}