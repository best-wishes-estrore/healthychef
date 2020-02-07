using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class LooseWeightModel
    {
        public int WeightGoal { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string CurrentLifeStyle { get; set; }
        public string TypicalLunch { get; set; }
        public string TypicalDay { get; set; }
        public bool BackIssue { get; set; }
        public bool IsDiabetes { get; set; }
        public bool IsAntibiotic { get; set; }
        public string LivingArea { get; set; }
        public string EmailID { get; set; }
        List<Risk> risks = new List<Risk>();
        
    }
    public class Risk
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}