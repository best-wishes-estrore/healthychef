using System;
using System.Collections.Generic;
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
    public partial class OrderFulfillmentProgram_Edit : FormControlBase
    {// Note: this.PrimaryKeyIndex as CartItemId

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
        public int CurrentProgramId
        {
            get
            {
                if (ViewState["CurrentProgramId"] == null)
                    ViewState["CurrentProgramId"] = 0;

                return int.Parse(ViewState["CurrentProgramId"].ToString());
            }
            set { ViewState["CurrentProgramId"] = value; }
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
        public int CurrentNumOfDays
        {
            get
            {
                if (ViewState["CurrentNumOfDays"] == null)
                    ViewState["CurrentNumOfDays"] = 0;

                return int.Parse(ViewState["CurrentNumOfDays"].ToString());
            }
            set { ViewState["CurrentNumOfDays"] = value; }
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

        public List<hccProgramDefaultMenu> defaultMenuSelections = new List<hccProgramDefaultMenu>();
        List<Day> days = new List<Day>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            rdoDays.SelectedIndexChanged += ddlDays_SelectedIndexChanged;
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

        protected override void LoadForm()
        {
            try
            {
                hccProductionCalendar cal = null;

                if (Request.QueryString["dd"] != null && !string.IsNullOrEmpty(Request.QueryString["dd"]))
                {
                    DateTime _delDate = DateTime.ParseExact(Request.QueryString["dd"].ToString(), "M/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    cal = hccProductionCalendar.GetBy(_delDate);
                    //cal = hccProductionCalendar.GetBy(DateTime.Parse(Request.QueryString["dd"].ToString()));

                    if (cal != null)
                        CurrentCalendarId = cal.CalendarID;

                    lkbBack.PostBackUrl += "?cal=" + cal.CalendarID.ToString();
                }

                hccProgramPlan plan = hccProgramPlan.GetById(CurrentCartItem.Plan_PlanID.Value);
                hccProgram program = hccProgram.GetById(plan.ProgramID);
                if (plan != null && program != null)
                {
                    CurrentProgramId = program.ProgramID;
                    CurrentNumOfDays = plan.NumDaysPerWeek;

                    // load user profile data
                    hccUserProfile profile = CurrentCartItem.UserProfile;
                    hccUserProfile parent = hccUserProfile.GetParentProfileBy(profile.MembershipID);

                    lblCustomerName.Text = parent.FirstName + " " + parent.LastName;
                    lblProfileName.Text = profile.ProfileName;
                    lblProgram.Text = program.Name;
                    lblPlan.Text = plan.Name;
                    lblPlanOption.Text = hccProgramOption.GetById(CurrentCartItem.Plan_ProgramOptionID.Value).OptionText;
                    lblOrderNumber.Text = CurrentCartItem.OrderNumber;
                    lblQuantity.Text = CurrentCartItem.Quantity.ToString();
                    lblDeliveryDate.Text = cal.DeliveryDate.ToShortDateString();

                    hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(CurrentCartItem.CartItemID, cal.CalendarID);

                    if (cartCal != null)
                        chkIsComplete.Checked = cartCal.IsFulfilled;

                    chkIsCancelledDisplay.Checked = CurrentCartItem.IsCancelled;

                    lvwAllrgs.DataSource = profile.GetAllergens();
                    lvwAllrgs.DataBind();
                    ProfileNotesEdit_AllergenNote.CurrentUserProfileId = profile.UserProfileID;
                    ProfileNotesEdit_AllergenNote.Bind();

                    lvwPrefs.DataSource = profile.GetPreferences();
                    lvwPrefs.DataBind();
                    ProfileNotesEdit_PreferenceNote.CurrentUserProfileId = profile.UserProfileID;
                    ProfileNotesEdit_PreferenceNote.Bind();

                    defaultMenuSelections = hccProgramDefaultMenu.GetBy(CurrentCalendarId, CurrentProgramId);

                    days = BindWeeklyGlance(defaultMenuSelections, CurrentNumOfDays);

                    BindDdlDays(days);

                    BindForm();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public hccMenuItem ReturnNoOfSide(int MenuId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenuItems
                        .Where(a => a.MenuItemID == MenuId)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }
        void BindForm()
        {
            if (defaultMenuSelections.Count == 0)
                defaultMenuSelections = hccProgramDefaultMenu.GetBy(CurrentCalendarId, CurrentProgramId);

            List<DropDownList> ddls = new List<DropDownList>();
            List<HtmlGenericControl> divDdls = new List<HtmlGenericControl>();
            DailyNutrition totalNutInfo = new DailyNutrition { DayNumber = CurrentDay };
            List<HtmlGenericControl> divItemContainers;
            string ddlBorderClass = string.Empty;
            List<hccAllergen> userAllergens;

            userAllergens = hccUserProfileAllergen.GetAllergensBy(CurrentCartItem.UserProfile.UserProfileID, true);

            BindDropDownLists(userAllergens);

            divItemContainers =
                pnlDefaultMenu.Controls.OfType<HtmlGenericControl>().Where(a => a.Attributes["class"] == "divItemContainer").ToList();

            divItemContainers.ForEach(delegate (HtmlGenericControl ctrl)
            { divDdls.AddRange(ctrl.Controls.OfType<HtmlGenericControl>().Where(a => a.Attributes["class"] == "divDdl")); });

            divDdls.ForEach(a => ddls.AddRange(a.Controls.OfType<DropDownList>()));

            int dailyDefaultsCount = defaultMenuSelections.Where(a => a.DayNumber == CurrentDay).Count();

            if (dailyDefaultsCount == 0 || dailyDefaultsCount != ddls.Count)
            {
                lblFeedback.Text += "Not all default values have been created for the selected date and program combination. Select the default items on the Production Management -> Program Default Menus page, and press 'Save' to create the default items.";
                pnlDefaultMenu.Visible = false;
            }
            else
            {
                hccCartItemCalendar cartCalendar = hccCartItemCalendar.GetBy(this.PrimaryKeyIndex, CurrentCalendarId);

                defaultMenuSelections.ForEach(delegate (hccProgramDefaultMenu defaultSelection)
                {
                    try
                    {
                        DropDownList ddl = ddls.Where(a => a.Attributes["day"] == defaultSelection.DayNumber.ToString()
                                && a.Attributes["type"] == defaultSelection.MealTypeID.ToString()
                                && a.Attributes["ord"] == defaultSelection.Ordinal.ToString())
                            .SingleOrDefault();

                        if (ddl != null)
                        {
                            hccMenuItem hccmenuitem = ReturnNoOfSide(defaultSelection.MenuItemID);
                            int menuItemId = defaultSelection.MenuItemID;
                            string value = string.Empty;
                            value = menuItemId.ToString() + "-" + defaultSelection.MenuItemSizeID.ToString();
                            if (hccmenuitem == null)
                            {
                                
                                value = menuItemId.ToString() + "-" + defaultSelection.MenuItemSizeID.ToString() + '-' + 0;
                            }
                            else
                            {
                                value = menuItemId.ToString() + "-" + defaultSelection.MenuItemSizeID.ToString() + '-' + hccmenuitem.NoofSideDishes;
                            }

                            int DefaultMenuExceptID = 0;
                            int cartcalendarid = 0;
                            // bold face the default selection
                            int defaultIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(value));
                            ddl.Items[(defaultIndex <= -1 ? 0 : defaultIndex)].Attributes["class"] += "bold italic";

                            hccCartDefaultMenuException menuItemExc
                                = hccCartDefaultMenuException.GetBy(defaultSelection.DefaultMenuID, cartCalendar.CartCalendarID);

                            if (menuItemExc != null)
                            {
                                menuItemId = menuItemExc.MenuItemID;
                                hccmenuitem = ReturnNoOfSide(menuItemId);
                                if (hccmenuitem == null)
                                {
                                    value = menuItemId.ToString() + "-" + menuItemExc.MenuItemSizeID.ToString() + '-' +0;
                                }
                                else
                                {
                                    value = menuItemId.ToString() + "-" + menuItemExc.MenuItemSizeID.ToString() + '-' + hccmenuitem.NoofSideDishes;
                                }
                                DefaultMenuExceptID = menuItemExc.DefaultMenuExceptID;
                                cartcalendarid = menuItemExc.CartCalendarID;
                            }
                           
                            ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(value));
                            ddl.Attributes.Add("defMenuId", defaultSelection.DefaultMenuID.ToString());
                            ddl.Attributes.Add("defMenuExceptionId", DefaultMenuExceptID.ToString());
                            ddl.Attributes.Add("defMenuCartCalendarId", cartcalendarid.ToString());
                            ddl.SelectedItem.Attributes["class"] += " bold";

                            HtmlGenericControl divItemContainer = (HtmlGenericControl)ddl.Parent.Parent;
                            HtmlGenericControl divNuts =
                                        divItemContainer.Controls.OfType<HtmlGenericControl>().Where(a => a.Attributes["class"] == "divNuts").SingleOrDefault();
                            HtmlGenericControl divAllrgs =
                                        divItemContainer.Controls.OfType<HtmlGenericControl>().Where(a => a.Attributes["class"] == "divAllrgs").SingleOrDefault();
                            HtmlGenericControl divPrefs =
                                       divItemContainer.Controls.OfType<HtmlGenericControl>().Where(a => a.Attributes["class"] == "divPrefs").SingleOrDefault();

                            if (menuItemId > 0)
                            {
                                hccMenuItem menuItem = null;

                                if (menuItemExc == null)
                                {
                                    if (defaultSelection.MenuItemID > 0)
                                        menuItem = hccMenuItem.GetById(defaultSelection.MenuItemID);
                                }
                                else if (menuItemExc.MenuItemID > 0)
                                {
                                    menuItem = hccMenuItem.GetById(menuItemExc.MenuItemID);
                                }

                                if (menuItem != null)
                                {
                                    // nutrition info
                                    if (divNuts != null)
                                    {
                                        string nutInfo = string.Empty;
                                        hccMenuItemNutritionData menuNut = hccMenuItemNutritionData.GetBy(menuItem.MenuItemID);

                                        if (menuNut != null)
                                        {
                                            nutInfo = string.Format("Calories: {0}, Fat: {1}, Protein: {2},  Carbohydrates: {3}, Fiber: {4},Sodium: {5}",
                                                menuNut.Calories.ToString("f2"), menuNut.TotalFat.ToString("f2"), menuNut.Protein.ToString("f2"),
                                                menuNut.TotalCarbohydrates.ToString("f2"), menuNut.DietaryFiber.ToString("f2"), menuNut.Sodium.ToString("f2"));
                                            divNuts.InnerHtml = nutInfo;
                                            totalNutInfo.Calories += menuNut.Calories;
                                            totalNutInfo.Carbs += menuNut.TotalCarbohydrates;
                                            totalNutInfo.Fat += menuNut.TotalFat;
                                            totalNutInfo.Fiber += menuNut.DietaryFiber;
                                            totalNutInfo.Protein += menuNut.Protein;
                                            totalNutInfo.Sodium += menuNut.Sodium;

                                        }
                                        else
                                        {
                                            nutInfo = string.Format("Calories: {0}, Fat: {1}, Protein: {2},  Carbohydrates: {3}, Fiber: {4},Sodium: {5}", 0, 0, 0, 0, 0, 0);
                                        }

                                        divNuts.InnerText = nutInfo;
                                    }

                                    // allergen displays
                                    List<hccAllergen> selectItemAllergens = new List<hccAllergen>();
                                    string allrgsList = string.Empty;
                                    allrgsList = "Allergens: " + menuItem.AllergensList;
                                    selectItemAllergens = menuItem.GetAllergens();

                                    if (divAllrgs != null)
                                        divAllrgs.InnerText = allrgsList;

                                    List<hccAllergen> matchedAllergens = selectItemAllergens.Intersect(userAllergens).ToList();

                                    if (matchedAllergens.Count > 0)
                                    {
                                        ddl.SelectedItem.Attributes["class"] += " bold";
                                        ddlBorderClass = "redBorder";
                                    }
                                    else
                                    {
                                        if (menuItemExc == null)
                                        {
                                            ddlBorderClass = "greenBorder";
                                        }
                                        else
                                        {
                                            ddlBorderClass = "blueBorder";
                                        }
                                    }

                                    // preferences
                                    List<hccPreference> itemPrefs = menuItem.GetPreferences();

                                    if (itemPrefs.Count > 0 && divPrefs != null)
                                    {
                                        itemPrefs.ForEach(delegate (hccPreference pref)
                                        {
                                            HtmlInputCheckBox chkPref = new HtmlInputCheckBox();
                                            chkPref.Value = pref.PreferenceID.ToString();
                                            Label lblPref = new Label();
                                            lblPref.Text = pref.Name;

                                            divPrefs.Controls.Add(chkPref);
                                            divPrefs.Controls.Add(lblPref);

                                            hccCartMenuExPref cartMenuExPref = hccCartMenuExPref.GetById(cartCalendar.CartItemID, defaultSelection.DayNumber);

                                            if (cartMenuExPref != null)
                                            {
                                                if (cartMenuExPref.IsModified)
                                                {
                                                    if (menuItemExc != null)
                                                    {
                                                        hccCartDefaultMenuExPref excPref = hccCartDefaultMenuExPref.GetBy(menuItemExc.DefaultMenuExceptID, pref.PreferenceID);
                                                        if (excPref != null)
                                                            chkPref.Checked = true;
                                                    }
                                                }
                                                //hccCartItem cartItem = hccCartItem.GetById(cartCalendar.CartItemID);
                                                //if (cartItem != null)
                                                //{
                                                //    if (Convert.ToBoolean(cartItem.IsModified))
                                                //    {
                                                //        hccCartDefaultMenuExPref excPref = hccCartDefaultMenuExPref.GetBy(menuItemExc.DefaultMenuExceptID, pref.PreferenceID);
                                                //        if (excPref != null)
                                                //        {
                                                //            chkPref.Checked = true;
                                                //        }
                                                //    }
                                                //    else
                                                //    {
                                                //        hccCartDefaultMenuExPref excPref = hccCartDefaultMenuExPref.GetBy(menuItemExc.DefaultMenuExceptID, pref.PreferenceID);
                                                //        if (excPref != null)
                                                //            chkPref.Checked = true;
                                                //        hccProgramDefaultMenuExPref programmenuexpref = hccProgramDefaultMenuExPref.GetBy(defaultSelection.DefaultMenuID, pref.PreferenceID);
                                                //        if (programmenuexpref != null)
                                                //            chkPref.Checked = true;
                                                //    }
                                                //}
                                            }
                                            else
                                            {
                                                //hccCartItem cartItem = hccCartItem.GetById(cartCalendar.CartItemID);
                                                //if (cartItem != null)
                                                //{
                                                //    if (!Convert.ToBoolean(cartItem.IsModified))
                                                //    {
                                                //        hccProgramDefaultMenuExPref programmenuexpref = hccProgramDefaultMenuExPref.GetBy(defaultSelection.DefaultMenuID, pref.PreferenceID);
                                                //        if (programmenuexpref != null)
                                                //            chkPref.Checked = true;
                                                //    }
                                                //}
                                                hccProgramDefaultMenuExPref programmenuexpref = hccProgramDefaultMenuExPref.GetBy(defaultSelection.DefaultMenuID, pref.PreferenceID);
                                                if (programmenuexpref != null)
                                                    chkPref.Checked = true;
                                            }
                                        });
                                    }
                                }
                                else
                                {
                                    divNuts.InnerText = string.Empty;
                                    divPrefs.InnerText = string.Empty;
                                    divAllrgs.InnerText = string.Empty;
                                }
                                ddl.CssClass = "mealItemDdl " + ddlBorderClass;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                });

                if (totalNutInfo == null)
                {
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Cals##", "0");
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Fats##", "0");
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Ptrns##", "0");
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Carbs##", "0");
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Fbrs##", "0");
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Sod##", "0");

                }
                else
                {
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Cals##", totalNutInfo.Calories.ToString("f2"));
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Fats##", totalNutInfo.Fat.ToString("f2"));
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Ptrns##", totalNutInfo.Protein.ToString("f2"));
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Carbs##", totalNutInfo.Carbs.ToString("f2"));
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Fbrs##", totalNutInfo.Fiber.ToString("f2"));
                    divTotalNutrition.InnerHtml = divTotalNutrition.InnerHtml.Replace("##Sod##", totalNutInfo.Sodium.ToString("f2"));
                }

                divTotalNutrition.Visible = true;
            }
        }

        protected override void SaveForm()
        {
            // handled via jquery ajax
        }

        protected override void ClearForm()
        {
            pnlDefaultMenu.Controls.Clear();
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void BindDropDownLists(List<hccAllergen> userAllergens)
        {
            try
            {
                if (CurrentProgramId > 0 && CurrentCalendarId > 0)
                {
                    hccProgram program = hccProgram.GetById(CurrentProgramId);
                    hccProductionCalendar cal = hccProductionCalendar.GetById(CurrentCalendarId);

                    hccMenu menu = cal.GetMenu();

                    if (program != null && menu != null)
                    {
                        List<hccProgramMealType> requiredMealTypes = hccProgramMealType.GetBy(program.ProgramID)
                            .Where(a => a.RequiredQuantity > 0).ToList();
                        int day = int.Parse(rdoDays.SelectedValue);

                        // Display for selected day
                        pnlDefaultMenu.CssClass += " dayPanel";
                        HtmlGenericControl pName = new HtmlGenericControl("p");
                        pName.Attributes.Add("class", "dayName label");
                        pName.InnerText = "Day " + day.ToString();
                        pnlDefaultMenu.Controls.Add(pName);
                        pnlDefaultMenu.Controls.Add(new HtmlGenericControl("hr"));

                        requiredMealTypes.ForEach(delegate (hccProgramMealType mealType)
                        {
                            HtmlGenericControl liType = new HtmlGenericControl("li");
                            liType.InnerText = ((Enums.MealTypes)mealType.MealTypeID).ToString() + ": " + mealType.RequiredQuantity.ToString();

                            HtmlGenericControl mealTypespan = new HtmlGenericControl("span");
                            mealTypespan.InnerHtml = "<b>" + ((Enums.MealTypes)mealType.MealTypeID).ToString() + "</b>";
                            pnlDefaultMenu.Controls.Add(mealTypespan);
                            


                            int i = 1;
                            while (i <= mealType.RequiredQuantity)
                            {
                                // container for meanu item
                                HtmlGenericControl divItemContainer = new HtmlGenericControl("div");
                                divItemContainer.Attributes.Add("class", "divItemContainer");
                                pnlDefaultMenu.Controls.Add(divItemContainer);

                                // container for ddl
                                HtmlGenericControl divDdl = new HtmlGenericControl("div");
                                divDdl.Attributes.Add("class", "divDdl");
                                divItemContainer.Controls.Add(divDdl);

                                DropDownList ddlMealItem = new DropDownList();
                                ddlMealItem.CssClass = "mealItemDdl";
                                ddlMealItem.Attributes.Add("calId", CurrentCalendarId.ToString());
                                ddlMealItem.Attributes.Add("progId", CurrentProgramId.ToString());
                                ddlMealItem.Attributes.Add("day", day.ToString());
                                ddlMealItem.Attributes.Add("type", mealType.MealTypeID.ToString());
                                ddlMealItem.Attributes.Add("ord", i.ToString());
                                ddlMealItem.Attributes.Add("ddltype", ((Enums.MealTypes)mealType.MealTypeID).ToString());
                                List<hccMenuItem> menuItems = hccMenuItem.GetByMenuId(menu.MenuID)
                                    .Where(a => a.MealTypeID == mealType.MealTypeID).OrderBy(a => a.Name).ToList();

                                List<ListItem> menuItemsWithSizes = new List<ListItem>();

                                menuItems.ForEach(delegate (hccMenuItem mainItem)
                                {
                                    hccMenuItem menuItem = hccMenuItem.GetById(mainItem.MenuItemID);
                                    bool hasAllergen = false;
                                    ListItem lItem = null;

                                    if (menuItem != null)
                                    {
                                        List<hccAllergen> matchAllergens = new List<hccAllergen>();
                                        List<hccAllergen> menuItemAllergens = menuItem.GetAllergens();
                                        matchAllergens = menuItemAllergens.Intersect(userAllergens).ToList();

                                        if (matchAllergens.Count > 0)
                                            hasAllergen = true;
                                    }
                                   
                                    lItem = new ListItem(mainItem.Name + " : "
                                        + Enums.CartItemSize.ChildSize.ToString(), mainItem.MenuItemID.ToString() + "-"
                                        + ((int)Enums.CartItemSize.ChildSize).ToString() + '-' + mainItem.NoofSideDishes);
                                    if (hasAllergen)
                                        lItem.Attributes["class"] += " redFont";
                                    menuItemsWithSizes.Add(lItem);

                                    lItem = new ListItem(mainItem.Name + " : "
                                        + Enums.CartItemSize.SmallSize.ToString(), mainItem.MenuItemID.ToString() + "-"
                                        + ((int)Enums.CartItemSize.SmallSize).ToString() + '-' + mainItem.NoofSideDishes);
                                    if (hasAllergen)
                                        lItem.Attributes["class"] += " redFont";
                                    menuItemsWithSizes.Add(lItem);

                                    lItem = new ListItem(mainItem.Name + " : "
                                        + Enums.CartItemSize.RegularSize.ToString(), mainItem.MenuItemID.ToString() + "-"
                                        + ((int)Enums.CartItemSize.RegularSize).ToString() + '-' + mainItem.NoofSideDishes);
                                    if (hasAllergen)
                                        lItem.Attributes["class"] += " redFont";
                                    menuItemsWithSizes.Add(lItem);

                                    lItem = new ListItem(mainItem.Name + " : "
                                        + Enums.CartItemSize.LargeSize.ToString(), mainItem.MenuItemID.ToString() + "-"
                                        + ((int)Enums.CartItemSize.LargeSize).ToString() + '-' + mainItem.NoofSideDishes);
                                    if (hasAllergen)
                                        lItem.Attributes["class"] += " redFont";
                                    menuItemsWithSizes.Add(lItem);
                                });

                                ddlMealItem.DataSource = new List<ListItem>();
                                ddlMealItem.DataTextField = "Text";
                                ddlMealItem.DataValueField = "Value";
                                ddlMealItem.DataBind();

                                ddlMealItem.Items.AddRange(menuItemsWithSizes.ToArray());
                                ddlMealItem.Items.Insert(0, new ListItem("None", "0"));

                                divDdl.Controls.Add(ddlMealItem);


                                if (((Enums.MealTypes)mealType.MealTypeID).ToString() == "BreakfastEntree"|| ((Enums.MealTypes)mealType.MealTypeID).ToString() == "LunchEntree"|| ((Enums.MealTypes)mealType.MealTypeID).ToString() == "DinnerEntree")
                                {
                                    HtmlGenericControl PNofsides = new HtmlGenericControl("p");
                                    var lblid = "lbl" + ((Enums.MealTypes)mealType.MealTypeID).ToString();

                                    PNofsides.InnerHtml = "<b>Number Of Sides:<label id="+lblid+"-" + day + ">"+lblid+"-" + day + "";


                                pnlDefaultMenu.Controls.Add(PNofsides);
                                }


                                // container for nutrition
                                HtmlGenericControl divNuts = new HtmlGenericControl("div");
                                divNuts.Attributes.Add("class", "divNuts");
                                divItemContainer.Controls.Add(divNuts);

                                // container for allergens
                                HtmlGenericControl divAllrgs = new HtmlGenericControl("div");
                                divAllrgs.Attributes.Add("class", "divAllrgs");
                                divItemContainer.Controls.Add(divAllrgs);

                                // container for prefs
                                HtmlGenericControl divPrefs = new HtmlGenericControl("div");
                                divPrefs.Attributes.Add("class", "divPrefs");
                                divItemContainer.Controls.Add(divPrefs);

                                divItemContainer.Controls.Add(new HtmlGenericControl("p"));

                                i++;
                            }

                            pnlDefaultMenu.Controls.Add(new HtmlGenericControl("hr"));
                        });

                        if (cal.DeliveryDate < DateTime.Now)
                        {
                            pnlDefaultMenu.Enabled = false;
                            chkIsComplete.Enabled = false;
                            //chkIsCancelled.Enabled = false;
                        }
                    }
                    else
                    {
                        if (menu == null)
                        {
                            lblFeedback.Text += "No menu has been assigned to the Production Calendar associated with the Delivery Date assigned to this item. " +
                                "Please assign a menu to the Production Calendar and select a Default Menu for this program, prior to continuing.&nbsp;";
                            pnlDefaultMenu.Visible = false;
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        void BindDdlDays(List<Day> days)
        {
            rdoDays.DataSource = days;
            rdoDays.DataTextField = "DayTitle";
            rdoDays.DataValueField = "DayNumber";
            rdoDays.DataBind();

            foreach (ListItem item in rdoDays.Items)
            {
                item.Attributes.Add("class", "mealDay" + item.Value);
            }

            hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(CurrentCartItem.CartItemID, CurrentCalendarId);
            CurrentDaysWithAllergens(cartCal.CartCalendarID).ForEach(delegate (int a)
            {
                ListItem item = rdoDays.Items.FindByValue(a.ToString());
                if (item != null)
                    item.Attributes["class"] += " redFont";
            });

            if (CurrentDay != 0)
                rdoDays.SelectedValue = CurrentDay.ToString();
            else
            {
                rdoDays.SelectedIndex = 0;
                CurrentDay = int.Parse(rdoDays.SelectedValue);
            }
            //ddlDays_SelectedIndexChanged(this, new EventArgs());
        }

        void ddlDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentDay = int.Parse(rdoDays.SelectedValue);

            foreach (ListItem item in rdoDays.Items)
            {
                item.Attributes.Add("class", "mealDay" + item.Value);
            }

            BindForm();

            hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(CurrentCartItem.CartItemID, CurrentCalendarId);
            CurrentDaysWithAllergens(cartCal.CartCalendarID).ForEach(delegate (int a)
                {

                    ListItem item = rdoDays.Items.FindByValue(a.ToString());
                    if (item != null)
                        item.Attributes["class"] += " redFont";
                });
        }

        List<Day> BindWeeklyGlance(List<hccProgramDefaultMenu> defaultMenuSelections, int numDaysPerWeek)
        {
            int i;
            for (i = 1; i <= numDaysPerWeek; i++)
            {
                StringBuilder sb = new StringBuilder();
                int lastTypeId = 0;

                defaultMenuSelections.Where(a => a.DayNumber == i)
                    .OrderBy(a => a.MealTypeID).ToList().ForEach(delegate (hccProgramDefaultMenu defaultSelection)
                    {
                        int menuItemId = defaultSelection.MenuItemID;
                        int menuItemSizeId = defaultSelection.MenuItemSizeID;

                        //hccProductionCalendar prodCal = hccProductionCalendar.GetBy(CurrentCartItem.DeliveryDate);
                        hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(this.PrimaryKeyIndex, CurrentCalendarId);

                        hccCartDefaultMenuException menuItemExc
                            = hccCartDefaultMenuException.GetBy(defaultSelection.DefaultMenuID, cartCal.CartCalendarID);
                        string prefString = string.Empty;

                        if (menuItemExc != null)
                        {
                            menuItemId = menuItemExc.MenuItemID;
                            menuItemSizeId = menuItemExc.MenuItemSizeID;

                            prefString = hccCartDefaultMenuExPref.GetPrefsBy(menuItemExc.DefaultMenuExceptID)
                              .Select(a => a.Name).DefaultIfEmpty("None").Aggregate((a, b) => a + ", " + b);
                        }

                        if (menuItemId > 0)
                        {
                            hccMenuItem menuItem = hccMenuItem.GetById(menuItemId);

                            if (defaultSelection.MealTypeID != lastTypeId)
                            {
                                lastTypeId = defaultSelection.MealTypeID;
                                sb.Append("<span class='label'>" + ((Enums.MealTypes)lastTypeId).ToString() + "</span><br />");
                            }

                            if (menuItem != null)
                                sb.Append(menuItem.Name);

                            if (((Enums.CartItemSize)menuItemSizeId) != Enums.CartItemSize.NoSize)
                                sb.Append(" - " + Enums.GetEnumDescription(((Enums.CartItemSize)menuItemSizeId)));

                            // prefs
                            if (!string.IsNullOrWhiteSpace(prefString))
                            {
                                sb.Append(" - " + prefString);
                            }

                            try
                            {
                                var t = defaultMenuSelections[defaultMenuSelections.IndexOf(defaultSelection) + 1];
                                if (t.MealTypeID == lastTypeId)
                                    sb.Append(", ");
                                else
                                    sb.Append("<p></p>");
                            }
                            catch { }
                        }
                        else
                        { sb.Append("<p></p>"); }
                    });

                days.Add(new Day { DayTitle = "Day: " + i.ToString(), DayNumber = i, DayInfo = sb.ToString().Trim().TrimEnd(',') });
            }

            lvwWeekGlance.DataSource = days;
            lvwWeekGlance.DataBind();

            return days;
        }

        protected void lkbRefresh_Click(object sender, EventArgs e)
        {
            Bind();
        }

        public List<int> CurrentDaysWithAllergens(int cartCalendarId)
        {
            return CurrentCartItem.GetDaysWithAllergens(cartCalendarId);
        }
    }

    public class Day
    {
        public string DayTitle { get; set; }
        public int DayNumber { get; set; }
        public string DayInfo { get; set; }
    }
}