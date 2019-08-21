using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart.Admin
{
    /// <summary>
    /// Summary description for WS_ReplaceProgramMeals
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class WS_ReplaceProgramMeals : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ReplaceProgramMeals(List<DefaultMenu> defaultMenus)
        {
            try
            {
                bool tryBool;
                bool isCancel = false;
                bool isComplete = false;
                int cartItemId = int.Parse(this.Context.Request.QueryString["cid"]);

                hccCartItem cartItem = hccCartItem.GetById(cartItemId);
                if (cartItem != null)
                {
                    if (this.Context.Request.QueryString["can"] != null)
                    {
                        tryBool = bool.TryParse(this.Context.Request.QueryString["can"], out isCancel);
                        cartItem.IsCancelled = isCancel;
                    }

                    if (this.Context.Request.QueryString["cmplt"] != null)
                    {
                        tryBool = bool.TryParse(this.Context.Request.QueryString["cmplt"], out isComplete);
                    }
                    hccCartMenuExPref cartMenuExPref = hccCartMenuExPref.GetById(cartItem.CartItemID, defaultMenus[0].DayNumber);
                    if (cartMenuExPref == null)
                    {
                        hccCartMenuExPref hccCartMenuExPref = new hccCartMenuExPref();
                        hccCartMenuExPref.CartItemID = cartItem.CartItemID;
                        hccCartMenuExPref.DayNumber = defaultMenus[0].DayNumber;
                        hccCartMenuExPref.IsModified = true;
                        hccCartMenuExPref.Save();
                    }
                    cartItem.Save();
                }

                decimal cals1 = 0.0m, fat1 = 0.0m, prtn1 = 0.0m, carb1 = 0.0m, fbr1 = 0.0m, sod1=0.0m;

                foreach (DefaultMenu defaultMenuItem in defaultMenus)
                {
                    hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(cartItem.CartItemID, defaultMenuItem.CalendarID);

                    if (cartCal != null)
                    {
                        if (isComplete != cartCal.IsFulfilled)
                        {
                            cartCal.IsFulfilled = isComplete;
                            cartCal.Save();
                        }

                        // get original defaultMenuItem for comparison of menuItem
                        hccProgramDefaultMenu origDefaultMenu =
                            hccProgramDefaultMenu.GetBy(cartCal.CalendarID, defaultMenuItem.ProgramID,
                                defaultMenuItem.DayNumber, defaultMenuItem.MealTypeID, defaultMenuItem.Ordinal);

                        hccCartDefaultMenuException existMenuEx = hccCartDefaultMenuException.GetBy(defaultMenuItem.DefaultMenuID, cartCal.CartCalendarID);

                        if (origDefaultMenu != null
                            && origDefaultMenu.MenuItemID == defaultMenuItem.MenuItemID
                            && origDefaultMenu.MenuItemSizeID == defaultMenuItem.MenuItemSizeID)
                        {
                            if (string.IsNullOrWhiteSpace(defaultMenuItem.Prefs))
                            {
                                if (existMenuEx != null)
                                {
                                    List<hccCartDefaultMenuExPref> prefs = hccCartDefaultMenuExPref.GetBy(existMenuEx.DefaultMenuExceptID);
                                    prefs.ForEach(a => a.Delete());
                                    existMenuEx.Delete();
                                }
                            }
                            else
                            {
                                if (existMenuEx == null)
                                {   // create exception menuItem
                                    existMenuEx = new hccCartDefaultMenuException
                                    {
                                        CartCalendarID = cartCal.CartCalendarID,
                                        DefaultMenuID = defaultMenuItem.DefaultMenuID
                                    };
                                }

                                existMenuEx.MenuItemID = defaultMenuItem.MenuItemID;
                                existMenuEx.MenuItemSizeID = defaultMenuItem.MenuItemSizeID;
                                existMenuEx.Save();

                                List<hccCartDefaultMenuExPref> prefs = hccCartDefaultMenuExPref.GetBy(existMenuEx.DefaultMenuExceptID);
                                prefs.ForEach(a => a.Delete());

                                if (!string.IsNullOrWhiteSpace(defaultMenuItem.Prefs))
                                {
                                    List<string> prefIds = defaultMenuItem.Prefs.Split(',').ToList();
                                    prefIds.ForEach(delegate(string prefId)
                                    {
                                        hccCartDefaultMenuExPref exPref = new hccCartDefaultMenuExPref
                                        {
                                            DefaultMenuExceptID = existMenuEx.DefaultMenuExceptID,
                                            PreferenceID = int.Parse(prefId)
                                        };

                                        exPref.Save();
                                    });
                                }
                            }
                        }
                        else
                        {
                            if (existMenuEx == null)
                            {   // create exception menuItem
                                existMenuEx = new hccCartDefaultMenuException
                                {
                                    CartCalendarID = cartCal.CartCalendarID,
                                    DefaultMenuID = defaultMenuItem.DefaultMenuID
                                };
                            }

                            existMenuEx.MenuItemID = defaultMenuItem.MenuItemID;
                            existMenuEx.MenuItemSizeID = defaultMenuItem.MenuItemSizeID;
                            existMenuEx.Save();

                            List<hccCartDefaultMenuExPref> exPrefs = hccCartDefaultMenuExPref.GetBy(existMenuEx.DefaultMenuExceptID);
                            exPrefs.ForEach(a => a.Delete());

                            if (!string.IsNullOrWhiteSpace(defaultMenuItem.Prefs))
                            {
                                List<string> prefIds = defaultMenuItem.Prefs.Split(',').ToList();
                                prefIds.ForEach(delegate(string prefId)
                                {
                                    hccCartDefaultMenuExPref exPref = new hccCartDefaultMenuExPref
                                    {
                                        DefaultMenuExceptID = existMenuEx.DefaultMenuExceptID,
                                        PreferenceID = int.Parse(prefId)
                                    };

                                    exPref.Save();
                                });
                            }
                        }

                        hccMenuItemNutritionData nutrition;

                        if (existMenuEx == null)
                            nutrition = hccMenuItemNutritionData.GetBy(defaultMenuItem.MenuItemID);
                        else
                            nutrition = hccMenuItemNutritionData.GetBy(existMenuEx.MenuItemID);

                        if (nutrition != null)
                        {
                            cals1 = cals1 + nutrition.Calories;
                            fat1 = fat1 + nutrition.TotalFat;
                            prtn1 = prtn1 + nutrition.Protein;
                            carb1 = carb1 + nutrition.TotalCarbohydrates;
                            fbr1 = fbr1 + nutrition.DietaryFiber;
                            sod1 = sod1 + nutrition.Sodium;
                        }
                    }
                }

                string nutri = cals1.ToString("f2") + "|" + fat1.ToString("f2") + "|" + prtn1.ToString("f2") + "|" + carb1.ToString("f2") + "|" + fbr1.ToString("f2") + "|" + sod1.ToString("f2");

                return nutri;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ResetTheProgramDefaultmenu(List<ResetMenu> resetMenuList)
        {
            string _result = string.Empty;
            try
            {
                int cartItemId = int.Parse(this.Context.Request.QueryString["cid"]);
                foreach (var resetMenu in resetMenuList)
                {
                    hccCartDefaultMenuException existMenuEx = hccCartDefaultMenuException.GetBy(resetMenu.DefaultMenuID, resetMenu.CartCalendarID);
                    
                        if (existMenuEx != null)
                        {
                            List<hccCartDefaultMenuExPref> prefs = hccCartDefaultMenuExPref.GetBy(resetMenu.DefaultMenuExceptionId);
                            prefs.ForEach(a => a.Delete());
                            existMenuEx.Delete();
                        }
                    //hccCartMenuExPref hccCartMenuExPref = new hccCartMenuExPref()
                    //{
                    //    CartItemID = cartItemId
                    //};
                    var hccCartMenuExPrefs = hccCartMenuExPref.GetByCartItemId(cartItemId, resetMenu.DayNumber);
                    hccCartMenuExPrefs.ForEach(a => a.Delete());
                }
                _result = "Successfully reset the menu";
            }
            catch (Exception ex)
            {
                _result = "Not reset the menu";
            }
            return _result;
        }

        public struct DefaultMenu
        {
            public int ProgramID { get; set; }
            public int CalendarID { get; set; }
            public int MenuItemSizeID { get; set; }
            public int DayNumber { get; set; }
            public int MealTypeID { get; set; }
            public int Ordinal { get; set; }
            public int MenuItemID { get; set; }
            public int DefaultMenuID { get; set; }
            public string Prefs { get; set; }
        }
        public struct ResetMenu
        {
            public int DefaultMenuExceptionId{ get; set; }

            public int DefaultMenuID { get; set; }

            public int CartCalendarID { get; set; }

            public int DayNumber { get; set; }
        }
    }
}
