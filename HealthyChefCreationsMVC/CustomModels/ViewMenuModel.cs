using HealthyChef.Common;
using HealthyChef.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class ViewMenuModel
    {
        private static IList<hcc_AlcMenu2_Result> _alcSides = null;
        private static IList<hcc_ProductionDescription_Result> _alcSides1 = null;
        private IEnumerable<hcc_AlcMenu2_Result> _iresult;
        private IEnumerable<hcc_ProductionDescription_Result> _iresult1;
        private IEnumerable<hcc_AlcMenu2_Result> _unionResult;
        private IEnumerable<hcc_ProductionDescription_Result> _unionResult1;
        public List<CartViewModel> cartViewModel { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int CartCount { get; set; }
        public List<int> ActiveMenuItemIds = new List<int>();
        public bool isActive { get; set; }
        //public IEnumerable<hcc_AlcMenu2_Result> alcMenu
        //{
        //    get
        //    {
        //        return _iresult;
        //    }
        //}

        public List<ALCMealModel> alcMenu { get; set; }
        public List<ALCMealModelForProductDescription> alcMenuForProductDesc { get; set; }

        public IEnumerable<hcc_AlcMenu2_Result> alcSides
        {
            get
            {
                return _alcSides;
            }
        }
        public List<hcc_ProductionDescription_Result> alcSides1
        {
            get
            {
                if (_alcSides1 != null)
                {
                    return (_alcSides1).Distinct().ToList();
                }
                else return null;
            }
        }

        public List<hccProductionCalendar> calendar { get; set; }

        public List<hccUserProfile> activeProfiles { get; set; }

        public List<SelectListItem> calendarDropdown { get; set; }

        public int ActiveTab { get; set; }
        public ViewMenuModel()
        {

        }
        public string mockSubTotal { get; set; }
        public ViewMenuModel(int _mealTypeId, string deliveryDate)
       {
            //re-initialize sides
            _alcSides = null;
            CartViewModel cartitems = new CartViewModel();
            this.cartViewModel =new List<CartViewModel>();
            cartViewModel.Add(cartitems);
            foreach(var item in cartViewModel)
            {
                mockSubTotal = item.mockSubTotal;
            }
            this.calendar = hccProductionCalendar.GetNext4Calendars();

            this.ActiveTab = _mealTypeId;

            DateTime DeliveryDate;

            bool isConverted= DateTime.TryParse(deliveryDate, out DeliveryDate);
            if(isConverted == false || string.IsNullOrEmpty(deliveryDate))
            {
                DeliveryDate = this.calendar.FirstOrDefault().DeliveryDate;
            }

            using (var hcc = new healthychefEntities())
            {
                switch (_mealTypeId)
                {
                    case 0:
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.BreakfastEntree));
                        var _breakfastSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.BreakfastSide);
                        if (_unionResult == null)
                            _unionResult = _iresult;

                        SetAvailableSides(_breakfastSides);
                        break;
                    case 1:
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.LunchEntree));
                        var _lunchSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.LunchSide);
                        if (_unionResult == null)
                            _unionResult = _iresult;

                        SetAvailableSides(_lunchSides);
                        break;
                    case 2:
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.DinnerEntree));
                        var _dinnerSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.DinnerSide);
                        if (_unionResult == null)
                            _unionResult = _iresult;

                        SetAvailableSides(_dinnerSides);
                        break;
                    case 3:
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.ChildEntree));
                        var _childSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.ChildSide);
                        if (_unionResult == null)
                            _unionResult = _iresult;

                        SetAvailableSides(_childSides);
                        break;
                    case 4:
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Dessert));
                        if (_unionResult == null)
                            _unionResult = _iresult;

                        //SetAvailableSides(_unionResult); //commented to not to show sides for desserts
                        break;
                    case 5:
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.OtherEntree));
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Soup));
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Salad));
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Beverage));
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Goods));
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Snack));
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Supplement));
                        CombineDataResults(hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.Miscellaneous));
                        List<hcc_AlcMenu2_Result> _otherSides = hcc.hcc_AlcMenu2(DeliveryDate, (int)Enums.MealTypes.OtherSide).ToList<hcc_AlcMenu2_Result>();
                        if (_unionResult == null)
                            _unionResult = _iresult;
                        else
                            _iresult = _unionResult;

                        SetAvailableSides(_otherSides);
                        break;
                    default:
                        break;
                }
            }

            if (_iresult != null)
            {
                this.alcMenu = new List<ALCMealModel>();
                foreach (var _m in _iresult)
                {
                    _m.ImageUrl = GetImageUrlwithBase(_m.ImageUrl);

                    this.alcMenu.Add(new ALCMealModel() { meal = _m });
                }
            }

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

            this.calendarDropdown = new List<SelectListItem>();
            foreach (var c in this.calendar)
            {
                var _dateVal = c.DeliveryDate.ToString("MM/dd/yyyy h:mm:ss tt");
                var _dateText = c.DeliveryDate.ToString("MMMM d, yyyy (ddd)");
                bool _isSelected = _dateVal == DeliveryDate.ToString("MM/dd/yyyy h:mm:ss tt");

                var _selectItem = new SelectListItem()
                {
                    Value = _dateVal,
                    Text = _dateText,
                    Selected = _isSelected
                };

                this.calendarDropdown.Add(_selectItem);
            }
        }

        public ViewMenuModel(string ItemName)
        {
            hccMenuItem menuItem = hccMenuItem.GetByItemName(ItemName);
            int _MealTYpeID = 0;
            int _MealTypeIdSide = 0;
            //if menuitem price is null
            if (menuItem.CostLarge == 0.0000M || menuItem.CostChild == 0.0000M || menuItem.CostRegular == 0.0000M || menuItem.CostSmall == 0.0000M)
            {
                menuItem = hccMenuItem.GetByItemNameLast(ItemName);
            }
            switch (menuItem.MealTypeID)
            {
                case 10:
                    _MealTYpeID = 10;
                    _MealTypeIdSide = 20;
                    break;
                case 30:
                    _MealTYpeID = 30;
                    _MealTypeIdSide = 40;
                    break;
                case 50:
                    _MealTYpeID = 50;
                    _MealTypeIdSide = 60;
                    break;
                case 90:
                    _MealTYpeID = 90;
                    _MealTypeIdSide = 100;
                    break;
                default:
                    _MealTYpeID = 00;
                    _MealTypeIdSide = 00;
                    break;
            }
            _alcSides = null;
            //CartViewModel cartitems = new CartViewModel();
            //this.cartViewModel = new List<CartViewModel>();
            //cartViewModel.Add(cartitems);
            //foreach (var item in cartViewModel)
            //{
            //    mockSubTotal = item.mockSubTotal;
            //}
            this.calendar = hccProductionCalendar.GetNext4Calendars();
            using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("hcc_GetAllItemsForNextDeliverDates", conn))
                {
                    conn.Open();

                    SqlParameter prm1 = new SqlParameter("@start", SqlDbType.DateTime);
                    prm1.Value = this.calendar[0].DeliveryDate;
                    SqlParameter prm2 = new SqlParameter("@end", SqlDbType.DateTime);
                    prm2.Value = this.calendar[3].DeliveryDate;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(prm1);
                    cmd.Parameters.Add(prm2);

                    SqlDataReader t = cmd.ExecuteReader();

                    if (t != null)
                    {
                        while (t.Read())
                        {
                            int id= int.Parse(t["MenuItemID"].ToString());
                            this.ActiveMenuItemIds.Add(id);
                        }
                    }
                    else
                        ActiveMenuItemIds = null;
                    cmd.Dispose();
                    conn.Close();
                    conn.Dispose();
                }
            }
            
            using (var hcc = new healthychefEntities())
            {
                if (menuItem.MealTypeID == 10 || menuItem.MealTypeID == 30 || menuItem.MealTypeID == 50 || menuItem.MealTypeID == 90)
                {
                    DescriptionCombineDataResults(hcc.hcc_ProductionDescription(ItemName, _MealTYpeID, null).Take(1));
                    var _Sides = hcc.hcc_ProductionDescription(null, _MealTypeIdSide, this.calendar[0].DeliveryDate);
                    if (_unionResult1 == null)
                        _unionResult1 = _iresult1;
                    else
                        _iresult1 = _unionResult1;
                    PDescriptionSetAvailableSides(_Sides);
                }
                else
                {
                    DescriptionCombineDataResults(hcc.hcc_ProductionDescription(ItemName, menuItem.MealTypeID, null).Take(1));
                    //var _Sides = hcc.hcc_ProductionDescription(null, menuItem.MealTypeID, this.calendar[1].DeliveryDate);
                    if (_unionResult1 == null)
                        _unionResult1 = _iresult1;
                    else
                        _iresult1 = _unionResult1;
                    //PDescriptionSetAvailableSides(_Sides);
                }
            }
            if (_iresult1 != null)
            {
                this.alcMenuForProductDesc = new List<ALCMealModelForProductDescription>();
                foreach (var _m in _iresult1)
                {
                    _m.ImageUrl = GetImageUrlwithBase(_m.ImageUrl);
                    this.alcMenuForProductDesc.Add(new ALCMealModelForProductDescription() { meal = _m });
                }
            }

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
            foreach(var id in ActiveMenuItemIds)
            {
                if(id==menuItem.MenuItemID)
                {
                    this.isActive = true;
                    break;
                }
                else
                {
                    this.isActive = false;
                }

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
        public void DescriptionCombineDataResults(IEnumerable<hcc_ProductionDescription_Result> resultDataSet)
        {
            if (_iresult1 == null)
                _iresult1 = resultDataSet.ToList();
            else if (_unionResult1 != null)
                _unionResult1 = _unionResult1.Concat(resultDataSet.ToList());
            else
                _unionResult1 = _iresult1.Concat(resultDataSet.ToList());

            if (_unionResult1 != null)
                _unionResult1.ToList();
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
        private void PDescriptionSetAvailableSides(IEnumerable<hcc_ProductionDescription_Result> alcSides1)
        {
            if (alcSides1 == null)
            {
                _alcSides1 = null;
                return;
            }

            _alcSides1 = alcSides1.ToList();

            if (_alcSides1.Count == 0)
            {
                _alcSides1 = null;
            }
            else if (_alcSides1.First().MenuItemID != -1)
            {
                _alcSides1.Insert(0, new hcc_ProductionDescription_Result() { MenuItemID = -1, Name = hccMenuItem.DefaultMealSideName });
            }
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


    public class ALCMealModel
    {

        public hcc_AlcMenu2_Result meal { get; set; }


        public List<hccMenuItemPreference> prefsList
        {
            get
            {
                return _getPrefList();
            }
        }

        public List<MealSize> mealSizes
        {
            get
            {
                return _getmealSizes();
            }
        }

        public string Price
        {
            get
            {
                if (this.mealSizes != null&&this.mealSizes.Count()>0)
                    return mealSizes[0].Price.ToString("c");
                else
                    return "";
            }
        }

        public string DiscountedMockPrice
        {
            get
            {
                if (this.mealSizes != null && this.mealSizes.Count() > 0)
                    return (Convert.ToDouble(mealSizes[0].Price)*0.9).ToString("c");
                else
                    return "";
            }
        }

        public string AllergenList
        {
            get
            {
                return _getAllergenList();
            }
        }
        //Like we are showing description, allergen info for each meal in view menu.. We have to show the meal nutrition info as well.[BWE 17/7/2018]
        public NutritionData NutritionInfo
        {
            get
            {
                return _getNutritionInfoData();
            }
        }
        List<hccMenuItemPreference> _getPrefList()
        {
            if (this.meal != null)
                return hccMenuItemPreference.GetBy(this.meal.MenuItemID).ToList();
            else
                return null;
        }

        List<MealSize> _getmealSizes()
        {
            List<MealSize> mealSizes = new List<MealSize>();

            if (this.meal != null)
            {
                if (this.meal.UseCostRegular)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.RegularSize),
                        Price = Convert.ToDecimal(this.meal.CostRegular),
                        Description = "Regular",
                        MenuId = this.meal.MenuItemID
                    });

                if (this.meal.UseCostSmall)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.SmallSize),
                        Price = Convert.ToDecimal(this.meal.CostSmall),
                        Description = "Small",
                        MenuId = this.meal.MenuItemID
                    });

                if (this.meal.UseCostLarge)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.LargeSize),
                        Price = Convert.ToDecimal(this.meal.CostLarge),
                        Description = "Large",
                        MenuId = this.meal.MenuItemID
                    });

                if (this.meal.UseCostChild)
                    mealSizes.Add(new MealSize()
                    {
                        SizeId = Convert.ToInt32(Enums.CartItemSize.ChildSize),
                        Price = Convert.ToDecimal(this.meal.CostChild),
                        Description = "Child",
                        MenuId = this.meal.MenuItemID
                    });

            }

            return mealSizes;
        }
        string _getAllergenList()
        {
            var _allergenList = string.Empty;
            if (this.meal != null)
            {
                _allergenList = this.meal.AllergensList.ToString().Trim().Substring(this.meal.AllergensList.ToString().Trim().Length - 1, 1).Contains(",")
                                ? this.meal.AllergensList.ToString().Trim().Substring(0, this.meal.AllergensList.ToString().Trim().Length - 1)
                                : this.meal.AllergensList;
            }
            return _allergenList;
        }
        NutritionData _getNutritionInfoData()
        {
            NutritionData _Nutritioninfo = new NutritionData();
            if (this.meal != null)
            {
                var NutritionData = hccMenuItemNutritionData.GetBy(this.meal.MenuItemID);
                if(NutritionData!=null)
                {
                    _Nutritioninfo.Calories = Convert.ToInt32(NutritionData.Calories);
                    _Nutritioninfo.Fat = Convert.ToInt32(NutritionData.TotalFat);
                    _Nutritioninfo.Protein = Convert.ToInt32(NutritionData.Protein);
                    _Nutritioninfo.Carbohydrates = Convert.ToInt32(NutritionData.TotalCarbohydrates);
                    _Nutritioninfo.DietaryFiber = Convert.ToInt32(NutritionData.DietaryFiber);
                    _Nutritioninfo.Sodium = Convert.ToInt32(NutritionData.Sodium);
                }
            }
            return _Nutritioninfo;
        }
    }
    public class ALCMealModelForProductDescription
    {
        public hcc_ProductionDescription_Result meal { get; set; }
        public List<hccMenuItemPreference> prefsList
        {
            get
            {
                return _getPrefList();
            }
        }
        public List<MealSize> mealSizes
        {
            get
            {
                return _getmealSizes();
            }
        }
        public string Price
        {
            get
            {
                if (this.mealSizes != null && this.mealSizes.Count() > 0)
                    return mealSizes[0].Price.ToString("c");
                else
                    return "";
            }
        }
        public string DiscountedMockPrice
        {
            get
            {
                if (this.mealSizes != null && this.mealSizes.Count() > 0)
                    return (Convert.ToDouble(mealSizes[0].Price) * 0.9).ToString("c");
                else
                    return "";
            }
        }
        public string AllergenList
        {
            get
            {
                return _getAllergenList();
            }
        }
        public NutritionData NutritionInfo
        {
            get
            {
                return _getNutritionInfoData();
            }
        }
        public List<SelectListItem> AvailableDeliverDates
        {
            get
            {
                return _AvailableDeliverDates();
            }
        }
        List<SelectListItem> _AvailableDeliverDates()
        {
            List<SelectListItem> AvailableDates = new List<SelectListItem>();
            try
            {
                var dates = hccProductionCalendar.GetNext4Calendars();
                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModules"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("AvailableDeliveryDatesForItem", conn))
                    {
                        conn.Open();
                        SqlParameter prm1 = new SqlParameter("@ItemName", SqlDbType.VarChar);
                        prm1.Value = this.meal.Name;
                        SqlParameter prm2 = new SqlParameter("@StartDate", SqlDbType.DateTime);
                        prm2.Value = dates[0].DeliveryDate;
                        SqlParameter prm3 = new SqlParameter("@EndDate", SqlDbType.DateTime);
                        prm3.Value = dates[3].DeliveryDate;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add(prm1);
                        cmd.Parameters.Add(prm2);
                        cmd.Parameters.Add(prm3);
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                SelectListItem AvailableDate = new SelectListItem();
                                DateTime tempDate = DateTime.Parse(reader["Value"].ToString());
                                AvailableDate.Value = tempDate.ToString("MM/dd/yyyy h:mm:ss tt");
                                AvailableDate.Text = tempDate.ToString("MMMM d, yyyy (ddd)");
                                AvailableDates.Add(AvailableDate);
                            }
                        }
                        else
                        {
                            AvailableDates = null;
                        }
                        cmd.Dispose();
                        conn.Close();
                        conn.Dispose();
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return AvailableDates;
        }
           
        List<hccMenuItemPreference> _getPrefList()
        {
            if (this.meal != null)
                return hccMenuItemPreference.GetBy(this.meal.MenuItemID).ToList();
            else
                return null;
        }
        List<MealSize> _getmealSizes()
        {
            List<MealSize> mealSizes = new List<MealSize>();
            if (this.meal != null)
            {
                if (this.meal.MealTypeID == 10 || this.meal.MealTypeID == 30 || this.meal.MealTypeID == 50 || this.meal.MealTypeID == 70)
                {
                    if (this.meal.UseCostRegular)
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.RegularSize),
                            Price = Convert.ToDecimal(this.meal.CostRegular),
                            Description = "Regular",
                            MenuId = this.meal.MenuItemID
                        });
                    if (this.meal.UseCostSmall)
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.SmallSize),
                            Price = Convert.ToDecimal(this.meal.CostSmall),
                            Description = "Small",
                            MenuId = this.meal.MenuItemID
                        });
                    if (this.meal.UseCostLarge)
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.LargeSize),
                            Price = Convert.ToDecimal(this.meal.CostLarge),
                            Description = "Large",
                            MenuId = this.meal.MenuItemID
                        });
                    if (this.meal.UseCostChild)
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.ChildSize),
                            Price = Convert.ToDecimal(this.meal.CostChild),
                            Description = "Child",
                            MenuId = this.meal.MenuItemID
                        });
                }
                else
                {
                    if (this.meal.UseCostChild)
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.ChildSize),
                            Price = Convert.ToDecimal(this.meal.CostChild),
                            Description = "Child",
                            MenuId = this.meal.MenuItemID
                        });
                    if (this.meal.UseCostSmall)
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.SmallSize),
                            Price = Convert.ToDecimal(this.meal.CostSmall),
                            Description = "Small",
                            MenuId = this.meal.MenuItemID
                        });
                    if(this.meal.UseCostRegular)
                    {
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.RegularSize),
                            Price = Convert.ToDecimal(this.meal.CostRegular),
                            Description = "Regular",
                            MenuId = this.meal.MenuItemID
                        });
                    }
                    if(this.meal.UseCostLarge)
                    {
                        mealSizes.Add(new MealSize()
                        {
                            SizeId = Convert.ToInt32(Enums.CartItemSize.LargeSize),
                            Price = Convert.ToDecimal(this.meal.CostLarge),
                            Description = "Large",
                            MenuId = this.meal.MenuItemID
                        });
                    }
                }

            }
            return mealSizes;
        }
        string _getAllergenList()
        {
            var _allergenList = string.Empty;
            if (this.meal != null)
            {
                _allergenList = this.meal.AllergensList.ToString().Trim().Substring(this.meal.AllergensList.ToString().Trim().Length - 1, 1).Contains(",")
                                ? this.meal.AllergensList.ToString().Trim().Substring(0, this.meal.AllergensList.ToString().Trim().Length - 1)
                                : this.meal.AllergensList;
            }
            return _allergenList;
        }
        NutritionData _getNutritionInfoData()
        {
            NutritionData _Nutritioninfo = new NutritionData();
            if (this.meal != null)
            {
                var NutritionData = hccMenuItemNutritionData.GetBy(this.meal.MenuItemID);
                if (NutritionData != null)
                {
                    _Nutritioninfo.Calories = Convert.ToInt32(NutritionData.Calories);
                    _Nutritioninfo.Fat = Convert.ToInt32(NutritionData.TotalFat);
                    _Nutritioninfo.Protein = Convert.ToInt32(NutritionData.Protein);
                    _Nutritioninfo.Carbohydrates = Convert.ToInt32(NutritionData.TotalCarbohydrates);
                    _Nutritioninfo.DietaryFiber = Convert.ToInt32(NutritionData.DietaryFiber);
                    _Nutritioninfo.Sodium = Convert.ToInt32(NutritionData.Sodium);
                }
            }
            return _Nutritioninfo;
        }
        


    }

    public class MealSize
    {
        public int MenuId { get; set; }
        public int SizeId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
    }

    public class NutritionData
    {
        public decimal Calories { get; set; }
        public decimal Fat { get; set; }
        public decimal Carbohydrates { get; set; }
        public decimal DietaryFiber { get; set; }
        public decimal Protein { get; set; }
        public decimal Sodium { get; set; }
    }


    public class AddItemToCartModel
    {
        public int menuItemID { get; set; }
        public int ProfileID { get; set; }
        public int sizeId { get; set; }
        public int[] prefIds { get; set; }
        public int sideDish1 { get; set; }
        public int sideDish2 { get; set; }
        public int Quantity { get; set; }
        public string DeliveryDate { get; set; }

        public int sideDishString1Id { get; set; }
        public int sideDishString2Id { get; set; }
        public string sideDishString1 { get; set; }
        public string sideDishString2 { get; set; }
        public bool isFamilyStyle { get; set; }
    }
}