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
using System.Text.RegularExpressions;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class MenuItem_Edit : HealthyChef.Common.FormControlBase
    {
        hccMenuItem CurrentMenuItem { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            btnSave.Click += new EventHandler(base.SubmitButtonClick);
            btnDeactivate.Click += new EventHandler(btnDeactivate_Click);
            btnCancel.Click += new EventHandler(base.CancelButtonClick);

            cstMenuItemName.ServerValidate += new ServerValidateEventHandler(cstMenuItemName_ServerValidate);

        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindddlMealType();

                BindlstPreferences();
                BindlstIngredients();

                SetButtons();

            }
        }

        protected void DisplayMenuItemOptions(hccMenuItem hmI)
        {
            // Because the form is reused, have to explicitly set checkboxes to false as state is saved between menu item selection
            cbCanyonRanchRecipe.Checked = false;
            cbCanyonRanchApproved.Checked = false;
            cbVegetarianOption.Checked = false;
            cbVeganOption.Checked = false;
            cbGlutenFreeOption.Checked = false;

            if (hmI.CanyonRanchRecipe != null)
                cbCanyonRanchRecipe.Checked = hmI.CanyonRanchRecipe.Value;
            if (hmI.CanyonRanchApproved != null)
                cbCanyonRanchApproved.Checked = hmI.CanyonRanchApproved.Value;
            if (hmI.VegetarianOptionAvailable != null)
                cbVegetarianOption.Checked = hmI.VegetarianOptionAvailable.Value;
            if (hmI.VeganOptionAvailable != null)
                cbVeganOption.Checked = hmI.VeganOptionAvailable.Value;
            if (hmI.GlutenFreeOptionAvailable != null)
                cbGlutenFreeOption.Checked = hmI.GlutenFreeOptionAvailable.Value;
        }

        protected void SetMenuItemOptions()
        {
            CurrentMenuItem.CanyonRanchRecipe = cbCanyonRanchRecipe.Checked;

            CurrentMenuItem.CanyonRanchApproved = cbCanyonRanchApproved.Checked;

            CurrentMenuItem.VegetarianOptionAvailable = cbVegetarianOption.Checked;

            CurrentMenuItem.VeganOptionAvailable = cbVeganOption.Checked;

            CurrentMenuItem.GlutenFreeOptionAvailable = cbGlutenFreeOption.Checked;            
        }

        protected override void LoadForm()
        {
            try
            {
                BindddlMealType();

                CurrentMenuItem = hccMenuItem.GetById(this.PrimaryKeyIndex);

                DisplayMenuItemOptions(CurrentMenuItem);

                if (CurrentMenuItem == null)
                    CurrentMenuItem = new hccMenuItem();

                if (CurrentMenuItem.IsRetired)
                    btnDeactivate.Text = "Reactivate";
                else
                    btnDeactivate.Text = "Retire";

                txtMenuItemName.Text = CurrentMenuItem.Name;
                txtDescription.Text = CurrentMenuItem.Description;
                chkIsTaxEligible.Checked = CurrentMenuItem.IsTaxEligible;

                if (CurrentMenuItem.ImageUrl!=null)
                {
                    MenuItemImageName.Value = CurrentMenuItem.ImageUrl.Split('/')[2];
                }
                else
                {
                    MenuItemImageName.Value = string.Empty;
                }
                chkUsePriceChild.Checked = CurrentMenuItem.UseCostChild;
                txtCostChild.Text = CurrentMenuItem.CostChild.ToString("f2");
                chkUsePriceSmall.Checked = CurrentMenuItem.UseCostSmall;
                txtCostSmall.Text = CurrentMenuItem.CostSmall.ToString("f2");
                chkUsePriceRegular.Checked = CurrentMenuItem.UseCostRegular;
                txtCostRegular.Text = CurrentMenuItem.CostRegular.ToString("f2");
                chkUsePriceLarge.Checked = CurrentMenuItem.UseCostLarge;
                txtCostLarge.Text = CurrentMenuItem.CostLarge.ToString("f2");

                ddlMealType.SelectedIndex = ddlMealType.Items.IndexOf(ddlMealType.Items.FindByValue(((int)CurrentMenuItem.MealType).ToString()));
                ddlsidedishes.SelectedIndex = ddlsidedishes.Items.IndexOf(ddlsidedishes.Items.FindByValue(((int)CurrentMenuItem.NoofSideDishes).ToString()));
                hccMenuItemNutritionData nutData = hccMenuItemNutritionData.GetBy(CurrentMenuItem.MenuItemID);
                if (nutData != null)
                {
                    txtCalories.Text = nutData.Calories.ToString();
                    txtDietaryFiber.Text = nutData.DietaryFiber.ToString();
                    txtProtein.Text = nutData.Protein.ToString();
                    txtTotalCarbohydrates.Text = nutData.TotalCarbohydrates.ToString();
                    txtTotalFat.Text = nutData.TotalFat.ToString();
                    txtSodium.Text = nutData.Sodium.ToString();
                }

                BindlstIngredients();
                BindlstPreferences();
                BindgvwItemUsage();

                SetButtons();
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
                if (CurrentMenuItem == null)
                    CurrentMenuItem = hccMenuItem.GetById(this.PrimaryKeyIndex);

                hccMenuItemNutritionData nutData = null;

                if (CurrentMenuItem == null)
                {
                    CurrentMenuItem = new hccMenuItem();
                }
                else
                {
                    nutData = hccMenuItemNutritionData.GetBy(CurrentMenuItem.MenuItemID);
                }

                if (nutData == null)
                    nutData = new hccMenuItemNutritionData();

                CurrentMenuItem.UseCostChild = chkUsePriceChild.Checked;
                CurrentMenuItem.CostChild = string.IsNullOrWhiteSpace(txtCostChild.Text.Trim()) ?
                    0.00m : decimal.Parse(txtCostChild.Text.Trim());

                CurrentMenuItem.UseCostSmall = chkUsePriceSmall.Checked;
                CurrentMenuItem.CostSmall = string.IsNullOrWhiteSpace(txtCostSmall.Text.Trim()) ?
                    0.00m : decimal.Parse(txtCostSmall.Text.Trim());

                CurrentMenuItem.UseCostRegular = chkUsePriceRegular.Checked;
                CurrentMenuItem.CostRegular = string.IsNullOrWhiteSpace(txtCostRegular.Text.Trim()) ?
                    0.00m : decimal.Parse(txtCostRegular.Text.Trim());

                CurrentMenuItem.UseCostLarge = chkUsePriceLarge.Checked;
                CurrentMenuItem.CostLarge = string.IsNullOrWhiteSpace(txtCostLarge.Text.Trim()) ?
                    0.00m : decimal.Parse(txtCostLarge.Text.Trim());

                CurrentMenuItem.Description = string.IsNullOrWhiteSpace(txtDescription.Text.Trim()) ? null : txtDescription.Text.Trim();
                CurrentMenuItem.Name = txtMenuItemName.Text.Trim();
                CurrentMenuItem.IsTaxEligible = chkIsTaxEligible.Checked;
                CurrentMenuItem.MealTypeID = int.Parse(ddlMealType.SelectedValue);
                CurrentMenuItem.NoofSideDishes = int.Parse(ddlsidedishes.SelectedValue); 
                //CurrentMenuItem
                //Check if File is available.
                if (Request.Files.Count>0)
                {
                    if (MenuItemImage.Value != "")
                    {
                        //Save the File.
                        string filename = "";
                        string imagesfolder = "/Images";
                        String path = HttpContext.Current.Server.MapPath("~"+ imagesfolder); //Path
                        //if (CurrentMenuItem.Name.Contains("&"))
                        //{
                        //    filename = CurrentMenuItem.Name.Replace('&', ' ');
                        //}
                        //else
                        //if (CurrentMenuItem.Name.Contains(","))
                        //{
                        //    filename = CurrentMenuItem.Name.Replace(',', ' ');
                        //}
                        //else
                        //{
                        //    filename = CurrentMenuItem.Name;
                        //}
                        filename=RemoveSpecialChars(CurrentMenuItem.Name);
                        string fileName = filename + "_" + CurrentMenuItem.MenuItemID + ".png";
                        string  filePath = path + "/" + fileName;
                        //Check if directory exist
                        if (!System.IO.Directory.Exists(path))
                        {
                            System.IO.Directory.CreateDirectory(path); //Create directory if it doesn't exist
                        }
                        MenuItemImage.PostedFile.SaveAs(filePath);
                        CurrentMenuItem.ImageUrl = imagesfolder + "/" + fileName;
                    }
                }

                decimal temp;

                if (decimal.TryParse(txtCalories.Text.Trim(), out temp))
                { nutData.Calories = temp; }

                if (decimal.TryParse(txtDietaryFiber.Text.Trim(), out temp))
                { nutData.DietaryFiber = temp; }

                if (decimal.TryParse(txtProtein.Text.Trim(), out temp))
                { nutData.Protein = temp; }

                if (decimal.TryParse(txtTotalCarbohydrates.Text.Trim(), out temp))
                { nutData.TotalCarbohydrates = temp; }

                if (decimal.TryParse(txtTotalFat.Text.Trim(), out temp))
                { nutData.TotalFat = temp; }

                if (decimal.TryParse(txtSodium.Text.Trim(), out temp))
                { nutData.Sodium = temp; }

                List<int> currentIngredients = CurrentMenuItem.GetIngredients().Select(a => a.IngredientID).ToList();
                List<int> selectedIngredients = lstIngredients.GetSelectedKeys();
                List<int> delIngredients = currentIngredients.Except(selectedIngredients).ToList();

                SetMenuItemOptions();
                CurrentMenuItem.Save();
                nutData.MenuItemID = CurrentMenuItem.MenuItemID;
                nutData.Save();

                selectedIngredients.ForEach(delegate(int ingredientId)
                {
                    hccMenuItemIngredient sel = hccMenuItemIngredient.GetBy(ingredientId, CurrentMenuItem.MenuItemID);

                    if (sel == null)
                        sel = new hccMenuItemIngredient { IngredientID = ingredientId, MenuItemID = CurrentMenuItem.MenuItemID, };

                    sel.Save();
                });

                delIngredients.ForEach(delegate(int ingredientId)
                {
                    hccMenuItemIngredient del = hccMenuItemIngredient.GetBy(ingredientId, CurrentMenuItem.MenuItemID);
                    if (del != null) del.Delete();
                });

                List<int> currentPrefs = CurrentMenuItem.GetPreferences().Select(a => a.PreferenceID).ToList();
                List<int> selectedPrefs = lstPreferences.GetSelectedKeys();
                List<int> delPrefs = currentPrefs.Except(selectedPrefs).ToList();

                selectedPrefs.ForEach(delegate(int prefId)
                {
                    hccMenuItemPreference sel = hccMenuItemPreference.GetBy(prefId, CurrentMenuItem.MenuItemID);

                    if (sel == null)
                        sel = new hccMenuItemPreference { PreferenceID = prefId, MenuItemID = CurrentMenuItem.MenuItemID };

                    sel.Save();
                });

                delPrefs.ForEach(delegate(int prefId)
                {
                    hccMenuItemPreference del = hccMenuItemPreference.GetBy(prefId, CurrentMenuItem.MenuItemID);
                    if (del != null) del.Delete();
                });

                //List<int> currentChildItems = CurrentMenuItem.GetChildren().Select(a => a.MenuItemID).ToList();
                //List<int> selectedChildItems = lstMenuItems.GetSelectedKeys();
                //List<int> delChildItems = currentChildItems.Except(selectedChildItems).ToList();

                //selectedChildItems.ForEach(a => CurrentMenuItem.AddMenuItem(a, 1));
                //delChildItems.ForEach(delegate(int itemId)
                //{
                //    hccMenuItem_MenuItems del = hccMenuItem_MenuItems.GetBy(CurrentMenuItem.MenuItemID, itemId);
                //    if (del != null) del.Delete();
                //});

                //CurrentMenuItem.Save();

                OnSaved(new ControlSavedEventArgs(CurrentMenuItem.MenuItemID));
                Response.Redirect("~/WebModules/ShoppingCart/Admin/ItemManager.aspx", false);

            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string RemoveSpecialChars(string str)
        {
            // Create  a string array and add the special characters you want to remove
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "_", "(", ")", ":", "|", "[", "]" };
            //Iterate the number of times based on the String array length.
            for (int i = 0; i < chars.Length; i++)
            {
                if (str.Contains(chars[i]))
                {
                    str = str.Replace(chars[i], "");
                }
            }
            return str;
        }
        protected override void ClearForm()
        {
            try
            {
                this.PrimaryKeyIndex = 0;
                txtCalories.Text = string.Empty;
                //txtCholsterol.Text = string.Empty;
                chkIsTaxEligible.Checked = false;
                chkUsePriceLarge.Checked = false;
                chkUsePriceLarge.Checked = false;
                chkUsePriceRegular.Checked = false;
                chkUsePriceSmall.Checked = false;
                txtCostChild.Text = string.Empty;
                txtCostSmall.Text = string.Empty;
                txtCostRegular.Text = string.Empty;
                txtCostLarge.Text = string.Empty;
                txtDescription.Text = string.Empty;
                txtDietaryFiber.Text = string.Empty;
                txtMenuItemName.Text = string.Empty;
                txtProtein.Text = string.Empty;
                //txtSaturatedFat.Text = string.Empty;
                //txtSodium.Text = string.Empty;
                //txtSugars.Text = string.Empty;
                //txtTotalCarbohydrates.Text = string.Empty;
                txtTotalFat.Text = string.Empty;
                //txtTransFat.Text = string.Empty;
                ddlMealType.ClearSelection();
                ddlsidedishes.ClearSelection();
                lstIngredients.Reset();
                lstPreferences.Reset();
                //lstMenuItems.Reset();
                BindgvwItemUsage();
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void BindddlMealType()
        {
            if (ddlMealType.Items.Count == 0)
            {
                try
                {
                    ddlMealType.DataSource =
                        HealthyChef.Common.Enums.GetEnumAsTupleList(typeof(HealthyChef.Common.Enums.MealTypes));
                    ddlMealType.DataTextField = "Item1";
                    ddlMealType.DataValueField = "Item2";
                    ddlMealType.DataBind();

                    ddlMealType.Items.Insert(0, new ListItem { Text = "Select a Meal Type...", Value = "-1" });
                }
                catch
                {
                    throw;
                }
            }
        }

        protected void BindlstIngredients()
        {
            List<hccIngredient> ingredients = hccIngredient.GetAll().Where(a => !a.IsRetired).OrderBy(a => a.Name).ToList();
            List<hccIngredient> selected = CurrentMenuItem != null ? CurrentMenuItem.GetIngredients().OrderBy(a => a.Name).ToList() : new List<hccIngredient>();

            lstIngredients.Bind<hccIngredient>(ingredients, selected);
        }

        protected void BindlstPreferences()
        {
            List<hccPreference> preferences = hccPreference.GetBy(Common.Enums.PreferenceType.Meal, false).ToList();
            List<hccPreference> selected = CurrentMenuItem != null ? CurrentMenuItem.GetPreferences() : new List<hccPreference>();

            lstPreferences.Bind<hccPreference>(preferences, selected);
        }

        protected void BindgvwItemUsage()
        {
            if (this.PrimaryKeyIndex > 0)
            {
                List<hccMenu> menus = hccMenu.GetByMenuItemId(this.PrimaryKeyIndex);
                gvwItemUsage.DataSource = menus;
                gvwItemUsage.DataBind();
            }
        }

        //protected void BindMenuItems()
        //{
        //    //List<hccMenuItem> items = hccMenuItem.GetAll((HealthyChef.Common.Enums.MealTypes)CurrentMenuItem.MealType).Where(a => a.MenuItemID != CurrentMenuItem.MenuItemID).ToList();
        //    List<hccMenuItem> items = hccMenuItem.GetAll().Where(a => a.MenuItemID != CurrentMenuItem.MenuItemID).ToList();
        //    List<hccMenuItem> selected = CurrentMenuItem.GetChildren();

        //    lstMenuItems.DataBind<hccMenuItem>(items, selected);
        //}

        protected void btnDeactivate_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentMenuItem = hccMenuItem.GetById(PrimaryKeyIndex);
                CurrentMenuItem.Retire(CurrentMenuItem.IsRetired);

                SetButtons();

                OnSaved(new ControlSavedEventArgs(CurrentMenuItem.MenuItemID));
            }
            catch (Exception)
            {
                throw;
            }
        }

        void ddlMealType_SelectedIndexChanged(object sender, EventArgs e)
        {
            int mealType = -1;

            if (int.TryParse(ddlMealType.SelectedValue, out mealType) && mealType > 0)
            {
                HealthyChef.Common.Enums.MealTypes selectedMealType = (HealthyChef.Common.Enums.MealTypes)mealType;
                //lstMenuItems.DataBind<hccMenuItem>(hccMenuItem.GetBy(selectedMealType), new List<hccMenuItem>());
            }
        }

        protected override void SetButtons()
        {
            ShowDeactivate = false;

            if (this.PrimaryKeyIndex > 0)
            {
                ShowDeactivate = true;
                btnDeactivate.Attributes.Add("onclick", "return confirm('Are you sure that you want to reactivate/retire this menu item?');");

                if (CurrentMenuItem == null)
                    CurrentMenuItem = hccMenuItem.GetById(this.PrimaryKeyIndex);

                if (CurrentMenuItem != null)
                {
                    if (CurrentMenuItem.IsRetired)
                        btnDeactivate.Text = "Reactivate";
                    else
                        btnDeactivate.Text = "Retire";
                }
            }

            btnDeactivate.Visible = ShowDeactivate;
        }

        void cstMenuItemName_ServerValidate(object source, ServerValidateEventArgs args)
        {
            Enums.MealTypes mealType = (Enums.MealTypes)(int.Parse(ddlMealType.SelectedValue));
            List<hccMenuItem> existItem = hccMenuItem.GetBy(txtMenuItemName.Text.Trim(), mealType);
            args.IsValid = false;

            if (existItem.Count > 0)
            {
                existItem.ForEach(delegate(hccMenuItem item)
                {
                    if (item.MenuItemID == this.PrimaryKeyIndex) args.IsValid = true;
                });
            }
            else
                args.IsValid = true;
        }
    }
}