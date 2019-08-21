using HealthyChefWebAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Menus
    {
        public string Name { get; set; }
        public int MenuID { get; set; }
    }
    public class Menu
    {
        public string Name { get; set; }
        public int MenuID { get; set; }
        public List<int> BreakfastEntrees { get; set; }
        public List<int> BreakfastSides   { get; set; }
        public List<int> LunchEntrees     { get; set; }
        public List<int> LunchSides       { get; set; }
        public List<int> DinnerEntrees { get; set; }
        public List<int> DinnerSides { get; set; }
        public List<int> ChildEntrees { get; set; }
        public List<int> ChildSides { get; set; }
        public List<int> Desserts { get; set; }
        public List<int> OtherEntrees { get; set; }
        public List<int> OtherSides { get; set; }
        public List<int> Soups { get; set; }
        public List<int> Salads { get; set; }
        public List<int> Beverages { get; set; }
        public List<int> Snacks { get; set; }
        public List<int> Supplements { get; set; }
        public List<int> Goods { get; set; }
        public List<int> Miscellaneous { get; set; }
    }

    public class MenuResult: PostHttpResponse
    {
        public string Name { get; set; }
        public int MenuID { get; set; }
        public List<int> BreakfastEntrees { get; set; }
        public List<int> BreakfastSides { get; set; }
        public List<int> LunchEntrees { get; set; }
        public List<int> LunchSides { get; set; }
        public List<int> DinnerEntrees { get; set; }
        public List<int> DinnerSides { get; set; }
        public List<int> ChildEntrees { get; set; }
        public List<int> ChildSides { get; set; }
        public List<int> Desserts { get; set; }
        public List<int> OtherEntrees { get; set; }
        public List<int> OtherSides { get; set; }
        public List<int> Soups { get; set; }
        public List<int> Salads { get; set; }
        public List<int> Beverages { get; set; }
        public List<int> Snacks { get; set; }
        public List<int> Supplements { get; set; }
        public List<int> Goods { get; set; }
        public List<int> Miscellaneous { get; set; }
    }

}