using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Plan
    {
        public int PlanID { get; set; }
        public int ProgramID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int NumDaysPerWeek { get; set; }
        public int NumWeeks { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsTaxEligible { get; set; }
        public bool IsDefault { get; set; }
        public String ProgramName { get; set; }       
    }

    public class PlanItem : Helpers.PostHttpResponse
    {
        public int PlanID { get; set; }
        public int ProgramID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int NumDaysPerWeek { get; set; }
        public int NumWeeks { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsTaxEligible { get; set; }
        public bool IsDefault { get; set; }
    }
}