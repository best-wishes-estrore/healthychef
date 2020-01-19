using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin.UserControls
{
    public partial class ProgramDefaultMenu_Edit : FormControlBase
    {
        hccMenuItem CurrentMenuItem { get; set; }
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

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

        protected override void LoadForm()
        {
            try
            {

                List<hccProgramDefaultMenu> defaultMenuSelections = hccProgramDefaultMenu.GetBy(CurrentCalendarId, CurrentProgramId);
                List<DropDownList> ddls = new List<DropDownList>();
                List<CheckBoxList> chklists = new List<CheckBoxList>();

                List<DailyNutrition> dailyNuts = new List<DailyNutrition>();
                List<HtmlGenericControl> nutDisplays = new List<HtmlGenericControl>();

                BindDropDownLists();

                List<Panel> pnls = pnlDefaultMenu.Controls.OfType<Panel>().ToList();
                int i = 1;
                pnls.ForEach(delegate (Panel pnl)
                {
                    //foreach(var control in pnl.Controls.Count)
                    ddls.AddRange(pnl.Controls.OfType<DropDownList>());
                    chklists.AddRange(pnl.Controls.OfType<CheckBoxList>());
                    //pnl.FindControl("mealItemDdl");
                    string divName = "divNutrition" + i.ToString();
                    nutDisplays.Add((HtmlGenericControl)pnl.FindControl(divName));
                    i++;
                });

                if (defaultMenuSelections.Count != ddls.Count)
                    lblFeedback.Text += "No default values have been saved for the selected date and program combination. Select the default items and press 'Save' to set the default items.";

                defaultMenuSelections.ForEach(delegate (hccProgramDefaultMenu selection)
                {
                    if (dailyNuts.Where(a => a.DayNumber == selection.DayNumber).SingleOrDefault() == null)
                        dailyNuts.Add(new DailyNutrition { DayNumber = selection.DayNumber });

                    DailyNutrition dailyNut = dailyNuts.Where(a => a.DayNumber == selection.DayNumber).Single();
                    hccMenuItemNutritionData menuNut = hccMenuItemNutritionData.GetBy(selection.MenuItemID);

                    if (menuNut != null)
                    {
                        dailyNut.Calories += menuNut.Calories;
                        dailyNut.Carbs += menuNut.TotalCarbohydrates;
                        dailyNut.Fat += menuNut.TotalFat;
                        dailyNut.Fiber += menuNut.DietaryFiber;
                        dailyNut.Protein += menuNut.Protein;
                        dailyNut.Sodium += menuNut.Sodium;
                    }

                    DropDownList ddl = ddls.Where(a => a.Attributes["day"] == selection.DayNumber.ToString()
                            && a.Attributes["type"] == selection.MealTypeID.ToString()
                            && a.Attributes["ord"] == selection.Ordinal.ToString())
                        .SingleOrDefault();

                    CheckBoxList chklist = chklists.Where(a => a.Attributes["day"] == selection.DayNumber.ToString()
                           && a.Attributes["type"] == selection.MealTypeID.ToString()
                           && a.Attributes["ord"] == selection.Ordinal.ToString())
                       .SingleOrDefault();

                   
                    if (ddl != null)
                    {
                        hccMenuItem hccmenuitem = ReturnNoOfSide(selection.MenuItemID);
                        string value = string.Empty;
                        if (hccmenuitem == null)
                        {
                             value = selection.MenuItemID.ToString() + "-" + selection.MenuItemSizeID.ToString()+"-"+0;
                             value = selection.MenuItemID.ToString() + "-" + selection.MenuItemSizeID.ToString()+"-"+0;
                        }
                        else
                        {
                             value = selection.MenuItemID.ToString() + "-" + selection.MenuItemSizeID.ToString() + "-" + hccmenuitem.NoofSideDishes;
                        }
                        

                        ddl.SelectedIndex = ddl.Items.IndexOf(ddl.Items.FindByValue(value));
                        ddl.Attributes.Add("defMenuId", selection.DefaultMenuID.ToString());
                    }
                    if (chklist != null)
                    {
                        List<ListItem> listItem = new List<ListItem>();
                        hccMenuItem hccMenuItem = new hccMenuItem
                        {
                            MenuItemID = Convert.ToInt32(selection.MenuItemID)
                        };
                        List<hccPreference> itemPrefs = hccMenuItem.GetPreferences();
                        if (itemPrefs.Count > 0 && ddl.SelectedIndex>0)
                        {
                            itemPrefs.ForEach(delegate (hccPreference pref)
                            {
                                ListItem chkPref = new ListItem();
                                chkPref.Value = pref.PreferenceID.ToString();
                                chkPref.Text = pref.Name;
                                chkPref.Attributes.Add("prefID", pref.PreferenceID.ToString());
                                hccProgramDefaultMenuExPref excPref = hccProgramDefaultMenuExPref.GetBy(selection.DefaultMenuID, pref.PreferenceID);
                                if (excPref != null)
                                    chkPref.Selected = true;
                                chklist.Items.Add(chkPref);
                            });
                        }
                        else
                        {
                            ListItem li = new ListItem("", "");
                            li.Selected = false;
                            li.Attributes.Add("Style", " max-height: 170px;display: block;overflow - y: auto; width: 100 %;min - height: 170px;display: none;");
                            chklist.Items.Add(li);
                        }

                    }
                });

                nutDisplays.ForEach(delegate (HtmlGenericControl ctrl)
                {
                    int nutDay = int.Parse(ctrl.Attributes["day"]);

                    DailyNutrition dNut = dailyNuts.Where(a => a.DayNumber == nutDay).SingleOrDefault();

                    if (dNut == null)
                    {
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Cals##", "0");
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Fats##", "0");
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Ptrns##", "0");
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Carbs##", "0");
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Fbrs##", "0");
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Sod##", "0");
                    }
                    else
                    {
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Cals##", dNut.Calories.ToString("f2"));
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Fats##", dNut.Fat.ToString("f2"));
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Ptrns##", dNut.Protein.ToString("f2"));
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Carbs##", dNut.Carbs.ToString("f2"));
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Fbrs##", dNut.Fiber.ToString("f2"));
                        ctrl.InnerHtml = ctrl.InnerHtml.Replace("##Sod##", dNut.Sodium.ToString("f2"));
                    }
                });

            }
            catch (Exception ex)
            {
                throw;
            }
        }
       
        protected override void SaveForm()
        {
            // Save handled via Jquery.Ajax
        }

        protected override void ClearForm()
        {
            pnlDefaultMenu.Controls.Clear();
        }

        protected override void SetButtons()
        {
            throw new NotImplementedException();
        }

        void BindDropDownLists()
        {
            var sideId = 1; 
            if (CurrentProgramId > 0 && CurrentCalendarId > 0)
            {
                hccProductionCalendar cal = hccProductionCalendar.GetById(CurrentCalendarId);
                hccMenu menu = cal.GetMenu();
                if (menu == null)
                {
                    lblFeedback.Text += "The selected delivery date does not have an associated menu. Please select a menu to use for the Production Calendar corresponding with this delivery date.";
                }
                else
                {
                    hccProgram program = hccProgram.GetById(CurrentProgramId);

                    if (program != null)
                    {
                        List<hccProgramMealType> mt = hccProgramMealType.GetBy(program.ProgramID);
                        List<hccProgramMealType> requiredMealTypes = mt.Where(a => a.RequiredQuantity > 0).ToList();
                      
                        // Display for 7 days
                        for (int day = 1; day <= 7; day++)
                        {
                            Panel pnlDay = new Panel();
                            pnlDay.CssClass = "dayPanel";
                            HtmlGenericControl pName = new HtmlGenericControl("p");
                            pName.InnerHtml = "<center><b>Day " + day.ToString() + "</b></center>";
                            pnlDay.Controls.Add(pName);
                            pnlDay.Controls.Add(new HtmlGenericControl("hr"));

                          

                            requiredMealTypes.ForEach(delegate (hccProgramMealType mealType)
                            {
                                if (day == 1) // list on the first item
                                {
                                    HtmlGenericControl liType = new HtmlGenericControl("li");
                                    liType.InnerText = ((Enums.MealTypes)mealType.MealTypeID).ToString() + ": " + mealType.RequiredQuantity.ToString();
                                    ulReqTypes.Controls.Add(liType);

                                    HtmlGenericControl mealTypespan = new HtmlGenericControl("h4");
                                    mealTypespan.InnerHtml = "<b>" + ((Enums.MealTypes)mealType.MealTypeID).ToString() + "</b>";
                                    pnlDay.Controls.Add(mealTypespan);

                                    
                                }
                                else
                                {
                                    HtmlGenericControl mealTypespan = new HtmlGenericControl("h4");
                                    mealTypespan.InnerHtml = "<div>&nbsp;</div>";
                                    pnlDay.Controls.Add(mealTypespan);
                                }
                                
                                int i = 1;
                                while (i <= mealType.RequiredQuantity)
                                {
                                    DropDownList ddlMealItem = new DropDownList();
                                    ddlMealItem.CssClass = "mealItemDdl";
                                    ddlMealItem.Attributes.Add("calId", CurrentCalendarId.ToString());
                                    ddlMealItem.Attributes.Add("progId", CurrentProgramId.ToString());
                                    ddlMealItem.Attributes.Add("day", day.ToString());
                                    ddlMealItem.Attributes.Add("type", mealType.MealTypeID.ToString());
                                    ddlMealItem.Attributes.Add("ord", i.ToString());
                                    ddlMealItem.Attributes.Add("ddltype", ((Enums.MealTypes)mealType.MealTypeID).ToString()+"-"+day);
                                    
                                    List<hccMenuItem> menuItems = hccMenuItem.GetByMenuId(menu.MenuID)
                                        .Where(a => a.MealTypeID == mealType.MealTypeID).OrderBy(a => a.Name).ToList();
                                    
                                    List<ListItem> menuItemsWithSizes = new List<ListItem>();

                                    
                                    menuItems.ForEach(delegate (hccMenuItem mainItem)
                                    {
                                        ListItem l1 = new ListItem(mainItem.Name + " : "
                                             + Enums.CartItemSize.ChildSize.ToString(), mainItem.MenuItemID.ToString() + "-" + ((int)Enums.CartItemSize.ChildSize).ToString()+"-"+mainItem.NoofSideDishes);
                                        l1.Attributes.Add("data-Noofside", Convert.ToString(mainItem.NoofSideDishes));
                                        menuItemsWithSizes.Add(l1);

                                        ListItem l2 = new ListItem(mainItem.Name + " : "
                                            + Enums.CartItemSize.SmallSize.ToString(), mainItem.MenuItemID.ToString() + "-" + ((int)Enums.CartItemSize.SmallSize).ToString() + "-" + mainItem.NoofSideDishes);
                                        l2.Attributes.Add("data-Noofside", Convert.ToString(mainItem.NoofSideDishes));

                                        menuItemsWithSizes.Add(l2);
                                        ListItem l3 = new ListItem(mainItem.Name + " : "
                                            + Enums.CartItemSize.RegularSize.ToString(), mainItem.MenuItemID.ToString() + "-" + ((int)Enums.CartItemSize.RegularSize).ToString() + "-" + mainItem.NoofSideDishes);
                                        l3.Attributes.Add("data-Noofside", Convert.ToString(mainItem.NoofSideDishes));
                                        menuItemsWithSizes.Add(l3);

                                        ListItem l4 = new ListItem(mainItem.Name + " : "
                                            + Enums.CartItemSize.LargeSize.ToString(), mainItem.MenuItemID.ToString() + "-" + ((int)Enums.CartItemSize.LargeSize).ToString() + "-" + mainItem.NoofSideDishes);
                                        l4.Attributes.Add("data-Noofside", Convert.ToString(mainItem.NoofSideDishes));
                                        menuItemsWithSizes.Add(l4);
                                      
                                    });

                                    ddlMealItem.DataSource = menuItemsWithSizes;
                                    ddlMealItem.DataTextField = "Text";
                                    ddlMealItem.DataValueField = "Value";
                                    


                                    ddlMealItem.DataBind();

                                    ddlMealItem.Items.Insert(0, new ListItem("None", "0"));
                                    pnlDay.Controls.Add(ddlMealItem);
                                  
                                    CheckBoxList menuitempreferencescheckBoxList = new CheckBoxList();
                                    menuitempreferencescheckBoxList.CssClass = "mealItemchk";
                                    menuitempreferencescheckBoxList.Attributes.Add("calId", CurrentCalendarId.ToString());
                                    menuitempreferencescheckBoxList.Attributes.Add("progId", CurrentProgramId.ToString());
                                    menuitempreferencescheckBoxList.Attributes.Add("day", day.ToString());
                                    menuitempreferencescheckBoxList.Attributes.Add("type", mealType.MealTypeID.ToString());
                                    menuitempreferencescheckBoxList.Attributes.Add("ord", i.ToString());
                                    pnlDay.Controls.Add(menuitempreferencescheckBoxList);


                                    pnlDay.Controls.Add(new HtmlGenericControl("p"));

                                    foreach (var item in menuItems)
                                    {
                                        if (Convert.ToString(item.MealType) == "BreakfastEntree")
                                        {
                                            HtmlGenericControl PNofsides = new HtmlGenericControl("p");
                                            var sideID = sideId;
                                            PNofsides.InnerHtml = "<center id=centreBreakfastEntree" + day + "><b>Number Of Sides:<label id=lblBreakfastEntree-" + day + ">lblBreakfastEntree-" + day + "</center>";
                                           

                                            pnlDay.Controls.Add(PNofsides);
                                        }
                                        if ( Convert.ToString(item.MealType) == "LunchEntree")
                                        {
                                            HtmlGenericControl PNofsides = new HtmlGenericControl("p");
                                            var sideID = sideId;
                                            PNofsides.InnerHtml = "<center id=centreLunchEntree" + day + "><b>Number Of Sides:<label id=lblLunchEntree-" + day + ">lblLunchEntree-" + day + "</center>";


                                            pnlDay.Controls.Add(PNofsides);
                                        }
                                        if ( Convert.ToString(item.MealType) == "DinnerEntree")
                                        {
                                            HtmlGenericControl PNofsides = new HtmlGenericControl("p");
                                            var sideID = sideId;
                                            PNofsides.InnerHtml = "<center id=centreDinnerEntree" + day + "><b>Number Of Sides:<label id=lblDinnerEntree-" + day + ">lblDinnerEntree-" + day +"</center>";
                                 

                                            pnlDay.Controls.Add(PNofsides);
                                        }

                                        break;

                                    }

                                    //pnlDay.Controls.Add(new HtmlGenericControl("hr"));

                                    i++;

                                }
                                
                                pnlDay.Controls.Add(new HtmlGenericControl("hr"));
                                sideId++;
                            });

                            HtmlGenericControl nut = new HtmlGenericControl("div");
                            nut.ID = "divNutrition" + day.ToString();
                            nut.Attributes.Add("day", day.ToString());
                            nut.Attributes.Add("class", "nutrition" + day);
                            nut.InnerHtml = "<fieldset><legend>Calories</legend><p id='cals" + day.ToString() + "'>##Cals##</p></fieldset>" +
                                "<fieldset><legend>Fat</legend><p id='fat" + day.ToString() + "'>##Fats##</p></fieldset>" +
                                "<fieldset><legend>Protein</legend><p id='prtn" + day.ToString() + "'>##Ptrns##</p></fieldset>" +
                                "<fieldset><legend>Carbohydrates</legend><p id='carb" + day.ToString() + "'>##Carbs##</p></fieldset>" +
                                "<fieldset><legend>Dietary Fiber</legend><p id='fbr" + day.ToString() + "'>##Fbrs##</p></fieldset>"+
                                "<fieldset><legend>Sodium</legend><p id='sod" + day.ToString() + "'>##Sod##</p></fieldset>";
                            pnlDay.Controls.Add(nut);

                            pnlDefaultMenu.Controls.Add(pnlDay);
                        }
                       
                        if (cal.DeliveryDate < DateTime.Now)
                        {
                            pnlDefaultMenu.Enabled = true;                     //[BWE]
                            pnlDefaultMenu.Attributes.Add("class", "enabled"); //[BWE]
                        }
                        else
                        {
                            pnlDefaultMenu.Enabled = true;
                            pnlDefaultMenu.Attributes.Remove("class");
                        }
                    }
                }
            }
        }
    }
 
    public class DailyNutrition
    {
        public decimal DayNumber { get; set; }
        public decimal Calories { get; set; }
        public decimal Fat { get; set; }
        public decimal Protein { get; set; }
        public decimal Carbs { get; set; }
        public decimal Fiber { get; set; }
        public decimal Sodium { get; set; }
    }
}