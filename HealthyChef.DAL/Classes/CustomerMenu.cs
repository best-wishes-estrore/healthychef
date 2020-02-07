using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public class CustomerMenu
    {
        List<Day> days = new List<Day>();

        public List<hccProgramDefaultMenu> defaultMenuSelections = new List<hccProgramDefaultMenu>();

        public int CurrentCalendarId { get; set; }

        public hccCartItem _thisItem;

        public hccCartItem thisItem
        {
            get
            {
                if (_thisItem == null)
                    _thisItem = hccCartItem.GetById(494);

                return (hccCartItem)_thisItem;
            }
            set
            {
                _thisItem = value;
            }
        }

        public List<Day> BindWeeklyGlance(List<hccProgramDefaultMenu> defaultMenuSelections, int numDaysPerWeek, int cartItemId)
        {
            StringBuilder sb = new StringBuilder();
            List<hccMenuItem> lstMI = new List<hccMenuItem>();
            Day d = new Day();
            MealTypes mT = new MealTypes();           

            string currentMealType = "";
            int i, currentDay = 1;
            for (i = 1; i <= numDaysPerWeek; i++)
            {
                int lastTypeId = 0;

                defaultMenuSelections.Where(a => a.DayNumber == i)
                    .OrderBy(a => a.MealTypeID).ToList().ForEach(delegate(hccProgramDefaultMenu defaultSelection)
                    {
                        int menuItemId = defaultSelection.MenuItemID;
                        int menuItemSizeId = defaultSelection.MenuItemSizeID;

                        hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(cartItemId, CurrentCalendarId);

                        hccCartDefaultMenuException menuItemExc
                            = hccCartDefaultMenuException.GetBy(defaultSelection.DefaultMenuID, cartCal.CartCalendarID);

                        if (menuItemExc != null)
                        {
                            menuItemId = menuItemExc.MenuItemID;
                            menuItemSizeId = menuItemExc.MenuItemSizeID;

                            List<hccCartDefaultMenuExPref> prefs = hccCartDefaultMenuExPref.GetBy(menuItemExc.DefaultMenuExceptID);
                        }
                        if (menuItemId > 0)
                        {
                            hccMenuItem menuItem = hccMenuItem.GetById(menuItemId);
                            
                            //if (defaultSelection.MealTypeID != lastTypeId)
                            //{
                                lastTypeId = defaultSelection.MealTypeID;

                                if (((Enums.MealTypes)lastTypeId).ToString().ToLower().Contains("breakfast"))
                                    currentMealType = "breakfast";
                                else if (((Enums.MealTypes)lastTypeId).ToString().ToLower().Contains("lunch"))
                                    currentMealType = "lunch";
                                else if (((Enums.MealTypes)lastTypeId).ToString().ToLower().Contains("dinner"))
                                    currentMealType = "dinner";
                                else if (((Enums.MealTypes)lastTypeId).ToString().ToLower().Contains("dessert"))
                                    currentMealType = "dessert";
                                else if (((Enums.MealTypes)lastTypeId).ToString().ToLower().Contains("snack"))
                                    currentMealType = "snack";

                                if (mT.MealType != currentMealType && mT.MealType != null)
                                {
                                    if (sb.ToString().EndsWith(",<br />") == true)
                                    {
                                        mT.MealInfo = sb.ToString().Substring(0, sb.ToString().Length - 7);
                                    }
                                    //mT.MealInfo = sb.ToString().Trim().TrimEnd(',');

                                    d.MealTypes.Add(mT);

                                    mT = new MealTypes();
                                    sb = new StringBuilder();
                                }
                                
                                mT.MealType = currentMealType;

                                if (menuItem != null)
                                {
                                    sb.Append(menuItem.Name);
                                    sb.Append(",<br />");
                                }
                                /*if (((Enums.CartItemSize)menuItemSizeId) != Enums.CartItemSize.NoSize)
                                {
                                    sb.Append(" - " + Enums.GetEnumDescription(((Enums.CartItemSize)menuItemSizeId)));
                                    sb.Append(", ");
                                } */
                                if (currentDay != defaultSelection.DayNumber) // only create a new day when daynumber changes
                                {
                                    sb.ToString().Trim().TrimEnd(',');
                                    d.DayTitle = currentDay.ToString();
                                    //d.DayTitle = "Day: " + currentDay.ToString();
                                    d.DayNumber = currentDay;                            
                                    days.Add(d);
                                    d = new Day();
                                    currentDay = defaultSelection.DayNumber;
                                }
                            //}
                            menuItemId = 0;
                        }
                    });
            }
            if (sb.ToString().EndsWith(",<br />") == true)
            {
                mT.MealInfo = sb.ToString().Substring(0, sb.ToString().Length - 7);
            }
            //mT.MealInfo = sb.ToString().Trim().TrimEnd(',');
            d.MealTypes.Add(mT);
            d.DayTitle = currentDay.ToString();
            //d.DayTitle = "Day: " + currentDay.ToString();
            d.DayNumber = currentDay;
            days.Add(d);

            return days;
        }

        public class Day
        {
            public string DayTitle { get; set; }
            public int DayNumber { get; set; }
            public List<MealTypes> MealTypes = new List<MealTypes>();
        }

        public class MealTypes
        {
            public string MealType { get; set; }
            public string MealInfo { get; set; }
        }
    }
}