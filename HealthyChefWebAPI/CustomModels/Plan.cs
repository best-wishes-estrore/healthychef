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

        public PlanItem()
        {

        }

        public PlanItem(Plan _planToValidate)
        {
            if (string.IsNullOrEmpty(_planToValidate.Name))
            {
                this.AddValidationError("Plan Name is Required");
                this.isValid = false;
            }
            if (_planToValidate.ProgramID == -1)
            {
                this.AddValidationError("Plan Program is Required");
                this.isValid = false;
            }
            if (_planToValidate.PricePerDay == decimal.Zero)
            {
                this.AddValidationError("Plan Price Per Day is Required");
                this.isValid = false;
            }
            if (_planToValidate.NumWeeks == 0)
            {
                this.AddValidationError("Plan Number of Weeks is Required");
                this.isValid = false;
            }
            if (_planToValidate.NumDaysPerWeek == 0)
            {
                this.AddValidationError("Plan Days of Meals Per Week is Required");
                this.isValid = false;
            }
            if (string.IsNullOrEmpty(_planToValidate.Description))
            {
                this.AddValidationError("Plan Description is Required");
                this.isValid = false;
            }
        }
    }
}