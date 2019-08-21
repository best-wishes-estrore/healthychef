using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class MessageBox
    {
        public int BoxID { get; set; }
        public string BoxName { get; set; }
        public decimal DIM_W { get; set; }
        public decimal DIM_L { get; set; }
        public decimal DIM_H { get; set; }
        public int MaxNoMeals { get; set; }
    }
}