using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using HealthyChef.Common;
using HealthyChef.DAL;

namespace HealthyChef.WebModules.ShoppingCart
{
    public partial class MenuItemsByDate : System.Web.UI.UserControl
    {
        public Guid CartUserASPNetId
        {
            get
            {
                if (ViewState["CartUserASPNetId"] == null)
                    ViewState["CartUserASPNetId"] = Guid.Empty;

                return Guid.Parse(ViewState["CartUserASPNetId"].ToString());
            }
            set { ViewState["CartUserASPNetId"] = value; }
        }

        public int MealTabSelected
        {
            get
            {
                if (ViewState["MealTabSelected"] == null)
                    ViewState["MealTabSelected"] = 0;

                return int.Parse(ViewState["MealTabSelected"].ToString());
            }
            set { ViewState["MealTabSelected"] = value; }
        }

        private IEnumerable<hcc_AlcMenu2_Result> _iresult;
        private IEnumerable<hcc_AlcMenu2_Result> _unionResult;

        DateTime DeliveryDate { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            ddlDeliveryDate.SelectedIndexChanged += ddlDeliveryDate_SelectedIndexChanged;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MealTabSelected = (int)Enums.MealTabItems.Breakfast;
                BindCalendarDates();
                BindPanels(Enums.MealTabItems.Breakfast);
                btnBreakfast.CssClass = "alc-button-submit active";
            }
            else
            {
                
            }
        }

        protected void lbBreakfastClick(object sender, EventArgs e)
        {
            MealTabSelected = (int)Enums.MealTabItems.Breakfast;

            BindPanels(Enums.MealTabItems.Breakfast);
            RemoveButtonStyle();
            btnBreakfast.CssClass = "alc-button-submit active";
        }

        protected void lbLunchClick(object sender, EventArgs e)
        {
            MealTabSelected = (int)Enums.MealTabItems.Lunch;

            BindPanels(Enums.MealTabItems.Lunch);
            RemoveButtonStyle();
            btnLunch.CssClass = "alc-button-submit active";
        }

        protected void lbDinnerClick(object sender, EventArgs e)
        {
            MealTabSelected = (int)Enums.MealTabItems.Dinner;

            BindPanels(Enums.MealTabItems.Dinner);
            RemoveButtonStyle();
            btnDinner.CssClass = "alc-button-submit active";
        }

        protected void lbChildClick(object sender, EventArgs e)
        {
            MealTabSelected = (int)Enums.MealTabItems.Child;

            BindPanels(Enums.MealTabItems.Child);
            RemoveButtonStyle();
            btnChild.CssClass = "alc-button-submit active";
        }

        protected void lbDessertClick(object sender, EventArgs e)
        {
            MealTabSelected = (int)Enums.MealTabItems.Dessert;

            BindPanels(Enums.MealTabItems.Dessert);
            RemoveButtonStyle();
            btnDessert.CssClass = "alc-button-submit active";
        }

        protected void lbOtherClick(object sender, EventArgs e)
        {
            MealTabSelected = (int)Enums.MealTabItems.Other;

            BindPanels(Enums.MealTabItems.Other);
            RemoveButtonStyle();
            btnOther.CssClass = "alc-button-submit active";
        }

        private void RemoveButtonStyle()
        {
            btnBreakfast.CssClass = "alc-button-submit";
            btnLunch.CssClass = "alc-button-submit";
            btnDinner.CssClass = "alc-button-submit";
            btnChild.CssClass = "alc-button-submit";
            btnDessert.CssClass = "alc-button-submit";
            btnOther.CssClass = "alc-button-submit";
        }
        private void BindPanels(Enums.MealTabItems mealTabType)
        {
            //get mealTabType from ViewState
            mealTabType = (Enums.MealTabItems)MealTabSelected;
            RemoveButtonStyle();

            try
            {
                // Get the menus available for the date
                DeliveryDate = DateTime.Parse(ddlDeliveryDate.SelectedValue);

                using (var hcc = new healthychefEntities())
                {
                    switch (mealTabType)
                    {
                        case Enums.MealTabItems.Breakfast:
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.BreakfastEntree));
                            var _breakfastSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.BreakfastSide);
                            if (_unionResult == null)
                                _unionResult = _iresult;

                            MealTabRepeater.LoadAlcMenuItems(DeliveryDate, Enums.MealTypes.BreakfastEntree, _unionResult, _breakfastSides);
                            break;
                        case Enums.MealTabItems.Lunch:
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.LunchEntree));
                            var _lunchSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.LunchSide);
                            if (_unionResult == null)
                                _unionResult = _iresult;

                            MealTabRepeater.LoadAlcMenuItems(DeliveryDate, Enums.MealTypes.LunchEntree, _unionResult, _lunchSides);
                            break;
                        case Enums.MealTabItems.Dinner:
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.DinnerEntree));
                            var _dinnerSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.DinnerSide);
                            if (_unionResult == null)
                                _unionResult = _iresult;

                            MealTabRepeater.LoadAlcMenuItems(DeliveryDate, Enums.MealTypes.DinnerEntree, _unionResult, _dinnerSides);
                            break;
                        case Enums.MealTabItems.Child:
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.ChildEntree));
                            var _childSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.ChildSide);
                            if (_unionResult == null)
                                _unionResult = _iresult;

                            MealTabRepeater.LoadAlcMenuItems(DeliveryDate, Enums.MealTypes.ChildEntree, _unionResult, _childSides);
                            break;
                        case Enums.MealTabItems.Dessert:
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Dessert));
                            if (_unionResult == null)
                                _unionResult = _iresult;

                            MealTabRepeater.LoadAlcMenuItems(DeliveryDate, Enums.MealTypes.Dessert, _unionResult);
                            break;
                        case Enums.MealTabItems.Other:
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.OtherEntree));
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Soup));
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Salad));
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Beverage));
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Goods));
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Snack));
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Supplement));
                            CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Miscellaneous));
                            var _otherSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.OtherSide);
                            if (_unionResult == null)
                                _unionResult = _iresult;

                            MealTabRepeater.LoadAlcMenuItems(DeliveryDate, Enums.MealTypes.OtherEntree, _unionResult, _otherSides);
                            break;
                        default:
                            break;
                    }
                }
                return;
            }
            catch (FormatException)
            {
                
            }
        }

        public void CombineDataResults(IEnumerable<hcc_AlcMenu2_Result> resultDataSet)
        {
            if (_iresult == null)
                _iresult = resultDataSet.ToList();
            else if (_unionResult != null)
                _unionResult = _unionResult.Concat(resultDataSet.ToList());
            else
                _unionResult = _iresult.Concat(resultDataSet.ToList());

            if (_unionResult != null)
                _unionResult.ToList();
        }

        void BindCalendarDates()
        {
            //Get the valid delivery dates
            List<hccProductionCalendar> calendar = hccProductionCalendar.GetNext4Calendars();
            ddlDeliveryDate.DataSource = calendar;
            ddlDeliveryDate.DataTextField = "DeliveryDate";
            ddlDeliveryDate.DataValueField = "DeliveryDate";
            ddlDeliveryDate.DataBind();
        }

        void ddlDeliveryDate_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbBreakfastClick(sender, e);
        }
    }
}