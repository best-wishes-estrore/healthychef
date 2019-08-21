using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    /// <summary>
    /// Summary description for WS_UpdateDefaultMenu
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WS_UpdateDefaultMenu : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateMenus(List<DefaultMenu> hccProgramDefaultMenus)
        {
            try
            {
                hccProgramDefaultMenus.ForEach(delegate(DefaultMenu defaultMenuItem)
                {
                    hccProgramDefaultMenu origMenuItem = hccProgramDefaultMenu.GetById(defaultMenuItem.DefaultMenuID);

                    if (origMenuItem == null)
                        origMenuItem = hccProgramDefaultMenu.GetBy(defaultMenuItem.CalendarID, defaultMenuItem.ProgramID, defaultMenuItem.DayNumber, defaultMenuItem.MealTypeID, defaultMenuItem.Ordinal);

                    if (origMenuItem != null)
                    {
                        origMenuItem.CalendarID = defaultMenuItem.CalendarID;
                        origMenuItem.DayNumber = defaultMenuItem.DayNumber;
                        origMenuItem.MealTypeID = defaultMenuItem.MealTypeID;
                        origMenuItem.MenuItemID = defaultMenuItem.MenuItemID;
                        origMenuItem.MenuItemSizeID = defaultMenuItem.MenuItemSizeID;
                        origMenuItem.Ordinal = defaultMenuItem.Ordinal;
                        origMenuItem.ProgramID = defaultMenuItem.ProgramID;
                        origMenuItem.Save();
                    }
                    else
                    {
                        hccProgramDefaultMenu hccprogramdefaultmenuitem = new hccProgramDefaultMenu
                        {
                            CalendarID = defaultMenuItem.CalendarID,
                            DayNumber = defaultMenuItem.DayNumber,
                            MealTypeID = defaultMenuItem.MealTypeID,
                            MenuItemID = defaultMenuItem.MenuItemID,
                            MenuItemSizeID = defaultMenuItem.MenuItemSizeID,
                            Ordinal = defaultMenuItem.Ordinal,
                            ProgramID = defaultMenuItem.ProgramID
                        };
                        hccprogramdefaultmenuitem.Save();
                    }
                });

                decimal cals1 = 0.0m, fat1 = 0.0m, prtn1 = 0.0m, carb1 = 0.0m, fbr1 = 0.0m, sod1=0.0m;
                decimal cals2 = 0.0m, fat2 = 0.0m, prtn2 = 0.0m, carb2 = 0.0m, fbr2 = 0.0m, sod2=0.0m;
                decimal cals3 = 0.0m, fat3 = 0.0m, prtn3 = 0.0m, carb3 = 0.0m, fbr3 = 0.0m, sod3=0.0m;
                decimal cals4 = 0.0m, fat4 = 0.0m, prtn4 = 0.0m, carb4 = 0.0m, fbr4 = 0.0m, sod4=0.0m;
                decimal cals5 = 0.0m, fat5 = 0.0m, prtn5 = 0.0m, carb5 = 0.0m, fbr5 = 0.0m, sod5=0.0m;
                decimal cals6 = 0.0m, fat6 = 0.0m, prtn6 = 0.0m, carb6 = 0.0m, fbr6 = 0.0m, sod6=0.0m;
                decimal cals7 = 0.0m, fat7 = 0.0m, prtn7 = 0.0m, carb7 = 0.0m, fbr7 = 0.0m, sod7=0.0m;

                hccProgramDefaultMenus.ForEach(delegate (DefaultMenu defaultMenuItem)
                {
                    hccMenuItemNutritionData nutrition = hccMenuItemNutritionData.GetBy(defaultMenuItem.MenuItemID);

                    if (nutrition != null)
                    {
                        if (defaultMenuItem.DayNumber == 1)
                        {
                            cals1 = cals1 + nutrition.Calories;
                            fat1 = fat1 + nutrition.TotalFat;
                            prtn1 = prtn1 + nutrition.Protein;
                            carb1 = carb1 + nutrition.TotalCarbohydrates;
                            fbr1 = fbr1 + nutrition.DietaryFiber;
                            sod1 = sod1 + nutrition.Sodium;
                        }

                        if (defaultMenuItem.DayNumber == 2)
                        {
                            cals2 = cals2 + nutrition.Calories;
                            fat2 = fat2 + nutrition.TotalFat;
                            prtn2 = prtn2 + nutrition.Protein;
                            carb2 = carb2 + nutrition.TotalCarbohydrates;
                            fbr2 = fbr2 + nutrition.DietaryFiber;
                            sod2 = sod2 + nutrition.Sodium;
                        }

                        if (defaultMenuItem.DayNumber == 3)
                        {
                            cals3 = cals3 + nutrition.Calories;
                            fat3 = fat3 + nutrition.TotalFat;
                            prtn3 = prtn3 + nutrition.Protein;
                            carb3 = carb3 + nutrition.TotalCarbohydrates;
                            fbr3 = fbr3 + nutrition.DietaryFiber;
                            sod3 = sod3 + nutrition.Sodium;
                        }

                        if (defaultMenuItem.DayNumber == 4)
                        {
                            cals4 = cals4 + nutrition.Calories;
                            fat4 = fat4 + nutrition.TotalFat;
                            prtn4 = prtn4 + nutrition.Protein;
                            carb4 = carb4 + nutrition.TotalCarbohydrates;
                            fbr4 = fbr4 + nutrition.DietaryFiber;
                            sod4 = sod4 + nutrition.Sodium;
                        }

                        if (defaultMenuItem.DayNumber == 5)
                        {
                            cals5 = cals5 + nutrition.Calories;
                            fat5 = fat5 + nutrition.TotalFat;
                            prtn5 = prtn5 + nutrition.Protein;
                            carb5 = carb5 + nutrition.TotalCarbohydrates;
                            fbr5 = fbr5 + nutrition.DietaryFiber;
                            sod5 = sod5 + nutrition.Sodium;
                        }

                        if (defaultMenuItem.DayNumber == 6)
                        {
                            cals6 = cals6 + nutrition.Calories;
                            fat6 = fat6 + nutrition.TotalFat;
                            prtn6 = prtn6 + nutrition.Protein;
                            carb6 = carb6 + nutrition.TotalCarbohydrates;
                            fbr6 = fbr6 + nutrition.DietaryFiber;
                            sod6 = sod6 + nutrition.Sodium;
                        }

                        if (defaultMenuItem.DayNumber == 7)
                        {
                            cals7 = cals7 + nutrition.Calories;
                            fat7 = fat7 + nutrition.TotalFat;
                            prtn7 = prtn7 + nutrition.Protein;
                            carb7 = carb7 + nutrition.TotalCarbohydrates;
                            fbr7 = fbr7 + nutrition.DietaryFiber;
                            sod7 = sod7 + nutrition.Sodium;
                        }
                    }
                });

                //get preferences
                hccProgramDefaultMenus.ForEach(delegate (DefaultMenu defaultMenuItem)
                {
                    List<hccProgramDefaultMenuExPref> prefs = hccProgramDefaultMenuExPref.GetBy(defaultMenuItem.DefaultMenuID);
                    prefs.ForEach(a => a.Delete());
                    if (defaultMenuItem.preferenceID.Length > 0 && defaultMenuItem.DefaultMenuID != 0)
                    {
                        foreach (var preferenceid in defaultMenuItem.preferenceID)
                        {
                            var ProgramDefaultMenuExPref = hccProgramDefaultMenuExPref.GetBy(defaultMenuItem.DefaultMenuID, preferenceid);
                            if (ProgramDefaultMenuExPref == null)
                            {
                                hccProgramDefaultMenuExPref exPref = new hccProgramDefaultMenuExPref
                                {
                                    PreferenceId = preferenceid,
                                    DefaultMenuId = defaultMenuItem.DefaultMenuID
                                };
                                exPref.Save();
                            }
                        }
                    }
                });


                string nutri = cals1.ToString("f2") + "|" + fat1.ToString("f2") + "|" + prtn1.ToString("f2") + "|" + carb1.ToString("f2") + "|" + fbr1.ToString("f2") + "|" + sod1.ToString("f2");
                nutri += "|" + cals2.ToString("f2") + "|" + fat2.ToString("f2") + "|" + prtn2.ToString("f2") + "|" + carb2.ToString("f2") + "|" + fbr2.ToString("f2") + "|" + sod2.ToString("f2");
                nutri += "|" + cals3.ToString("f2") + "|" + fat3.ToString("f2") + "|" + prtn3.ToString("f2") + "|" + carb3.ToString("f2") + "|" + fbr3.ToString("f2") + "|" + sod3.ToString("f2");
                nutri += "|" + cals4.ToString() + "|" + fat4.ToString() + "|" + prtn4.ToString() + "|" + carb4.ToString() + "|" + fbr4.ToString() + "|" + sod4.ToString();
                nutri += "|" + cals5.ToString() + "|" + fat5.ToString() + "|" + prtn5.ToString() + "|" + carb5.ToString() + "|" + fbr5.ToString() + "|" + sod5.ToString();
                nutri += "|" + cals6.ToString() + "|" + fat6.ToString() + "|" + prtn6.ToString() + "|" + carb6.ToString() + "|" + fbr6.ToString() + "|" + sod6.ToString();
                nutri += "|" + cals7.ToString() + "|" + fat7.ToString() + "|" + prtn7.ToString() + "|" + carb7.ToString() + "|" + fbr7.ToString() + "|" + sod7.ToString();

                return nutri;
            }
            catch (Exception)
            {
                throw;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ShowingPreferencesByMenuitemid(DefaultMenu Defaultmenudetails)
        {
            List<hccPreference> itemPrefs = new List<hccPreference>();
            try
            {
                string menuitempreferences = "";
                if (Defaultmenudetails.MenuItemID != 0)
                {
                    hccMenuItem hccMenuItem = new hccMenuItem
                    {
                        MenuItemID = Convert.ToInt16(Defaultmenudetails.MenuItemID)
                    };
                    itemPrefs = hccMenuItem.GetPreferences();
                    if (itemPrefs.Count > 0)
                    {
                        foreach (var preference in itemPrefs)
                        {
                            hccProgramDefaultMenuExPref excPref = hccProgramDefaultMenuExPref.GetBy(Defaultmenudetails.DefaultMenuID, preference.PreferenceID);
                            if (excPref != null)
                                menuitempreferences += preference.PreferenceID.ToString() + "|" + 1.ToString() + "|" + preference.Name.ToString() + "&" + Defaultmenudetails.MealTypeID.ToString() + "|" + Defaultmenudetails.CalendarID.ToString() + "|" + Defaultmenudetails.ProgramID.ToString() + "|" + Defaultmenudetails.DayNumber.ToString() + "*";
                            else
                                menuitempreferences += preference.PreferenceID.ToString() + "|" + 0.ToString() + "|" + preference.Name.ToString() + "&" + Defaultmenudetails.MealTypeID.ToString() + "|" + Defaultmenudetails.CalendarID.ToString() + "|" + Defaultmenudetails.ProgramID.ToString() + "|" + Defaultmenudetails.DayNumber.ToString() + "*";
                        }
                    }
                    else
                    {
                        menuitempreferences = 0.ToString() + "|" + 0.ToString() + "|" + " " + "&" + Defaultmenudetails.MealTypeID.ToString() + "|" + Defaultmenudetails.CalendarID.ToString() + "|" + Defaultmenudetails.ProgramID.ToString() + "|" + Defaultmenudetails.DayNumber.ToString() + "*";
                    }
                }
                else
                {
                    menuitempreferences = 0.ToString() + "|" + 0.ToString() + "|" + " " + "&" + Defaultmenudetails.MealTypeID.ToString() + "|" + Defaultmenudetails.CalendarID.ToString() + "|" + Defaultmenudetails.ProgramID.ToString() + "|" + Defaultmenudetails.DayNumber.ToString() + "*";
                }
                return menuitempreferences;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public struct DefaultMenu
        {
            //public List<hccProgramDefaultMenu> hccProgramDefaultMenus { get; set; }

            //public List<hccProgramDefaultMenuExPref> hccProgramDefaultMenuExPref { get; set; }
            public int DefaultMenuID { get; set; }
            public int ProgramID { get; set; }
            public int CalendarID { get; set; }
            public int MenuItemID { get; set; }
            public int MenuItemSizeID { get; set; }
            public int MealTypeID { get; set; }
            public int DayNumber { get; set; }
            public int Ordinal { get; set; }
            public int[] preferenceID { get; set; }
        }
    }
}
