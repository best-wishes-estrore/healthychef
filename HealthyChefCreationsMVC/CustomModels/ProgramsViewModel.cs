using HealthyChef.Common;
using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class ProgramsViewModel
    {
        public List<ProgramViewModel> programs { get; set; }


        public ProgramsViewModel()
        {
            var _programs = hccProgram.GetBy(true).Where(p => p.DisplayOnWebsite).ToList();
            this.programs = new List<ProgramViewModel>();

            foreach (var _p in _programs)
            {
                var newProgram = new ProgramViewModel();
                newProgram.ProgramID = _p.ProgramID;
                newProgram.Name = _p.Name;
                newProgram.ImagePath = _p.ImagePath;
                newProgram.Description = _p.Description;
                newProgram.MoreInfoNavID = _p.MoreInfoNavID ?? 0;
                newProgram.Price = _p.GetCheapestPlanPrice().ToString("c");
                if (_p.ProgramID == 49)
                {
                    newProgram.orderDisplay = 1;

                    //var programsData = hccProgram.GetById(49);
                    //if(programsData != null)
                    //{
                    //    newProgram.orderDisplay = 1;
                    //    newProgram.Name = programsData.Name;
                    //    newProgram.ImagePath = programsData.ImagePath;
                    //    newProgram.Description = programsData.Description;
                    //}
                }
                if (_p.ProgramID == 33)
                {
                    newProgram.orderDisplay = 2;
                }
                if (_p.ProgramID == 59)
                {
                    newProgram.orderDisplay = 3;
                }
                if (_p.ProgramID == 34)
                {
                    newProgram.orderDisplay = 4;
                }
                if (_p.ProgramID == 61)
                {
                    newProgram.orderDisplay = 5;
                }
                this.programs.Add(newProgram);


                //this.programs.Add(new ProgramViewModel()
                //{
                //    ProgramID = _p.ProgramID,
                //    Name = _p.Name,
                //    ImagePath = _p.ImagePath,
                //    Description = _p.Description,
                //    MoreInfoNavID = _p.MoreInfoNavID ?? 0,
                //    Price = _p.GetCheapestPlanPrice().ToString("c"),
                //});
            }
            this.programs = this.programs.OrderBy(x => x.orderDisplay).ToList();
        }
    }

    public class ProgramViewModel
    {
        public int ProgramID { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public int MoreInfoNavID { get; set; }
        public int orderDisplay { get; set; }
    }

    public class hccProgramDefaultMenuDetails
    {
        public string DayNumber { get; set; }
        public int MealTypeID { get; set; }
        public string MenuItemName { get; set; }
        public string Description { get; set; }
        public string Calories { get; set; }
        public string Fat { get; set; }
        public string Carbohydrates { get; set; }
        public string Protein { get; set; }
        public string Sodium { get; set; }
        public string DietaryFiber { get; set; }
        public string MenuItemImageUrl { get; set; }
        public int Noofweeksddl { get; set; }
    }

    public class ProgramDetailsViewModel
    {
        public int ProgramId { get; set; }

        public string ProgramName
        {
            get
            {
                if (this.Program != null)
                    return this.Program.Name;
                else
                    return "";
            }
        }

        public ContentViewModel contentViewModels { get; set; }
        public hccProgram Program { get; set; }
        public List<hccProgramPlan> PlanTypes { get; set; }
        public List<hccProgramOption> PlanOptions { get; set; }
        public List<hccProductionCalendar> calendar { get; set; }
        public List<hccUserProfile> activeProfiles { get; set; }
        public List<hccProgramMealType> programmealtypes { get; set; }
        public List<hccProgramMealType> programMealTypesForDefaultMenu { get; set; }
        public List<hccProgramDefaultMenu> programdefaultmenu { get; set; }
        public List<hccProgramDefaultMenuDetails> programdefaultmenudetails = new List<hccProgramDefaultMenuDetails>();

        public List<hccProgramDefaultMenuDetails> DefaultBreakfastMenuDetails
        {
            get
            {
                return programdefaultmenudetails?.Where(x => x.MealTypeID == (int)Enums.MealTypes.BreakfastEntree)?.ToList();
            }
        }
        public List<hccProgramDefaultMenuDetails> DefaultLunchMenuDetails
        {
            get
            {
                return programdefaultmenudetails.Where(x => x.MealTypeID == (int)Enums.MealTypes.LunchEntree)?.ToList();
            }
        }
        public List<hccProgramDefaultMenuDetails> DefaultDinnerMenuDetails
        {
            get
            {
                return programdefaultmenudetails?.Where(x => x.MealTypeID == (int)Enums.MealTypes.DinnerEntree)?.ToList();
            }
        }
        public List<hccProgramDefaultMenuDetails> DefaultOtherMenuDetails
        {
            get
            {
                return programdefaultmenudetails?.Where(x => x.MealTypeID == (int)Enums.MealTypes.OtherEntree)?.ToList();
            }
        }
        public List<hccProgramPlan> plantypesfornoofweeks { get; set; }
        public List<hccProgramDefaultMenuDetails> Viewmenudetails = new List<hccProgramDefaultMenuDetails>();

        public ProgramDetailsViewModel()
        {

        }

        public ProgramDetailsViewModel(int _programId, int calendarid)
        {
            try
            {
                this.ProgramId = _programId;
                this.Program = hccProgram.GetById(_programId);
                this.PlanTypes = hccProgramPlan.GetBy(this.Program.ProgramID, Program.IsActive).Where(prog => prog.PricePerDay > 0).ToList();
                int numberofweeks = this.PlanTypes.Where(x => x.NumWeeks < 5).Max(x => x.NumWeeks);
                this.PlanTypes = this.PlanTypes.Where(x => x.NumWeeks <= numberofweeks).ToList();
                this.plantypesfornoofweeks = hccProgramPlan.GetBy(this.Program.ProgramID, Program.IsActive).Where(prog => prog.PricePerDay > 0 && prog.NumWeeks < 5).GroupBy(x => x.NumWeeks).Select(x => x.FirstOrDefault()).ToList();
                this.PlanOptions = hccProgramOption.GetBy(this.Program.ProgramID).Where(a => !string.IsNullOrWhiteSpace(a.OptionText)).ToList();
                this.calendar = hccProductionCalendar.GetNext4Calendars();
                this.programmealtypes = hccProgramMealType.GetBy(this.Program.ProgramID).Where(x => x.MealTypeID == 10 || x.MealTypeID == 70 || x.MealTypeID == 30 || x.MealTypeID == 50 || x.MealTypeID == 90).Where(x => x.RequiredQuantity > 0).ToList();
                this.contentViewModels = new ContentViewModel((int)Program.MoreInfoNavID);
                //this.programdefaultmenu = hccProgramDefaultMenu.GetBy(calendarid != 0 ? calendarid : calendar[0].CalendarID, this.Program.ProgramID).Where(x => x.MealTypeID == 10 || x.MealTypeID == 70 || x.MealTypeID == 30 || x.MealTypeID == 50 || x.MealTypeID == 90).Where(x =>x.DayNumber == 1).ToList();
                //New Implementation for Getting All Weeks Menu Data
                var allWeeksMenu = GetAllWeeks(calendarid !=0? calendarid:calendar[0].CalendarID,this.ProgramId);
                List<hccProgramDefaultMenuDetails> programdefaultmenudetailsbymenuitemid = new List<hccProgramDefaultMenuDetails>();
                foreach (var defaultmenu in allWeeksMenu)
                {
                    hccProgramDefaultMenuDetails menuItemdetails = new hccProgramDefaultMenuDetails();
                    hccMenuItem menuItem = hccMenuItem.GetById(defaultmenu.MenuItemID);
                    if (menuItem != null)
                    {
                        hccMenuItemNutritionData menuItemNutritionData = hccMenuItemNutritionData.GetBy(defaultmenu.MenuItemID);
                        if (menuItemNutritionData != null)
                        {
                            menuItemdetails.MenuItemImageUrl = GetImageUrlwithBase(menuItem.ImageUrl);
                            //menuItemdetails.DayNumber = defaultmenu.DayNumber.ToString();
                            menuItemdetails.Description = menuItem.Description;
                            menuItemdetails.MenuItemName = menuItem.Name;
                            menuItemdetails.MealTypeID = defaultmenu.MealTypeID;
                            menuItemdetails.Calories = "Calories:" + " " + Convert.ToInt32(menuItemNutritionData.Calories);
                            menuItemdetails.Fat = "Fat:" + " " + Convert.ToInt32(menuItemNutritionData.TotalFat) + " " + "g";
                            menuItemdetails.Protein = "Protein:" + " " + Convert.ToInt32(menuItemNutritionData.Protein) + " " + "g";
                            menuItemdetails.Sodium = "Sodium:" + " " + Convert.ToInt32(menuItemNutritionData.Sodium) + " " + "mg";
                            menuItemdetails.Carbohydrates = "Carbs:" + " " + Convert.ToInt32(menuItemNutritionData.TotalCarbohydrates) + " " + "g" /*+ menuItemNutritionData.Protein*/ ;
                            //  menuItemdetails.DietaryFiber = "DietaryFiber:" + Convert.ToInt32(menuItemNutritionData.DietaryFiber);
                        }
                        programdefaultmenudetailsbymenuitemid.Add(menuItemdetails);
                    }
                }
                this.programdefaultmenudetails = programdefaultmenudetailsbymenuitemid;
                //ViewMenuModel items shows in planwidget start
                //List<ViewMenuModel> listviewmenu = new List<ViewMenuModel>();
                //listviewmenu.Add(new ViewMenuModel(0, this.calendar.Where(x => x.CalendarID == (calendarid != 0 ? calendarid : calendar[0].CalendarID)).FirstOrDefault().DeliveryDate.ToString()));
                //listviewmenu.Add(new ViewMenuModel(1, this.calendar.Where(x => x.CalendarID == (calendarid != 0 ? calendarid : calendar[0].CalendarID)).FirstOrDefault().DeliveryDate.ToString()));
                //listviewmenu.Add(new ViewMenuModel(2, this.calendar.Where(x => x.CalendarID == (calendarid != 0 ? calendarid : calendar[0].CalendarID)).FirstOrDefault().DeliveryDate.ToString()));
                //foreach (var viewMenuModel in listviewmenu)
                //{
                //    if (viewMenuModel.alcMenu.FirstOrDefault() != null)
                //    {
                //        var alcmenudetails = viewMenuModel.alcMenu.FirstOrDefault();
                //        this.Viewmenudetails.Add(new hccProgramDefaultMenuDetails
                //        {
                //            MealTypeID = alcmenudetails.meal.MealTypeID,
                //            MenuItemName = alcmenudetails.meal.Name,
                //            Description = alcmenudetails.meal.Description,
                //            Calories = "Calories:" + alcmenudetails.NutritionInfo.Calories,
                //            Protein = "Protein:" + alcmenudetails.NutritionInfo.Protein,
                //            Carbohydrates = "Calories:" + alcmenudetails.NutritionInfo.Carbohydrates,
                //            DietaryFiber = "DietaryFiber:" + alcmenudetails.NutritionInfo.DietaryFiber,
                //        });
                //    }
                //}
                //ViewMenuModel items shows in planwidget end
                MembershipUser user = Helpers.LoggedUser;

                if (user != null)
                {
                    var CartUserASPNetId = (Guid)user.ProviderUserKey;
                    this.activeProfiles = (from profile in hccUserProfile.GetBy(CartUserASPNetId) where profile.IsActive select profile).ToList();
                }
                else
                {
                    this.activeProfiles = new List<hccUserProfile>();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public List<hcc_AlcMenu2_Result> GetAllWeeksMenu(DateTime deliveryDate)
        //{
        //    List<hcc_AlcMenu2_Result> weeksMenu = new List<hcc_AlcMenu2_Result>();
        //    using (var hcc = new healthychefEntities())
        //    {
        //        //Weekly Menu For the BreakFast,Lunch and Dinner.
        //        var breakFastMenuList = hcc.hcc_AlcMenu2(deliveryDate, (int)Enums.MealTypes.BreakfastEntree).Where(x => x.UseCostRegular).ToList();

        //        var lunchMenuList = hcc.hcc_AlcMenu2(deliveryDate, (int)Enums.MealTypes.LunchEntree).Where(x => x.UseCostRegular).ToList();

        //        var dinnerList = hcc.hcc_AlcMenu2(deliveryDate, (int)Enums.MealTypes.DinnerEntree).Where(x => x.UseCostRegular).ToList();

        //        weeksMenu = weeksMenu.Concat(breakFastMenuList)
        //                  .Concat(lunchMenuList)
        //                  .Concat(dinnerList).ToList();

        //    }
        //    return weeksMenu;
        //}

        public List<hccMenuItem> GetAllWeeks(int CurrentCalendarId, int currentProgramId)
        {
            List<hccMenuItem> hccMenuItems = new List<hccMenuItem>();
            hccProductionCalendar cal = hccProductionCalendar.GetById(CurrentCalendarId);
            hccMenu menu = cal.GetMenu();

            this.programMealTypesForDefaultMenu = hccProgramMealType.GetBy(this.Program.ProgramID).Where(x => x.MealTypeID == 10 || x.MealTypeID == 70 || x.MealTypeID == 30 || x.MealTypeID == 50 || x.MealTypeID == 90).Where(x => x.RequiredQuantity >= 0).ToList();
            if (this.programMealTypesForDefaultMenu.Count > 0)
            {
                this.programMealTypesForDefaultMenu.ForEach(delegate (hccProgramMealType mealType)
                {
                   List<hccMenuItem> menuItems = hccMenuItem.GetByMenuId(menu.MenuID)
                                           .Where(a => a.MealTypeID == mealType.MealTypeID).OrderBy(a => a.Name).ToList();
                    hccMenuItems = hccMenuItems.Concat(menuItems).ToList();
                });
            }
            return hccMenuItems;
        }

        public static string GetImageUrlwithBase(string _imageUrl)
        {
            string _baseUrl = System.Configuration.ConfigurationManager.AppSettings["imagebaseurl"];
            if (!string.IsNullOrEmpty(_baseUrl) && !string.IsNullOrEmpty(_imageUrl))
            {
                return _baseUrl + _imageUrl;
            }
            return "/userfiles/images/genericMenu.jpg";
        }
    }

    public class ProgramAddToCartModel
    {
        public int itemId { get; set; }
        public int optionId { get; set; }
        public string deliveryDateString { get; set; }
        public int Quantity { get; set; }
        public int profileId { get; set; }
        public bool isRecurring { get; set; }
    }

    public class PlanOption
    {
        public int OptionId { get; set; }
        public decimal PricePerDay { get; set; }
        public string Description { get; set; }
    }
    public class Meals
    {
        public int CartID { get; set; }
        public int MealCount { get; set; }
        public int NoOfWeeks { get; set; }
    }

    public class ResponseModel
    {
        public object result { set; get; }
        public int statusCode { set; get; }
        public string errorMessage { set; get; }
        public string successMessage { set; get; }
    }

}