using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class ProgramPlanDetails
    {
        public int ProgramId { get; set; }
        public int PlanId { get; set; }
        public int NumDaysPerWeek { get; set; }
        public int NumWeeks { get; set; }
        public int ProgramOptionId { get; set; }
        public decimal Planprice { get; set; }
        public decimal PlanOptionprice { get; set; }
    }
}