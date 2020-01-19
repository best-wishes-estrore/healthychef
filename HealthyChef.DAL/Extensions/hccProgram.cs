using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccProgram
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccPrograms", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccPrograms", this);
                    }
                    else
                    {
                        cont.hccPrograms.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public void Retire(bool isRetired)
        {
            try
            {
                this.IsActive = !isRetired;
                Save();
            }
            catch
            {
                throw;
            }
        }

        public decimal GetCheapestPlanPrice()
        {
            var plans = hccProgramPlan.GetBy(this.ProgramID, true).ToList();
            decimal p = 0.00m;

            if (plans.Count > 0)
            {
                p = plans.Min(a => a.PricePerDay);
            }

            return p;
        }

        public int GetMealsPerDay()
        {
            int mealsPerDay = 0;

            var mealTypes = hccProgramMealType.GetBy(this.ProgramID);

            mealTypes.ForEach(delegate(hccProgramMealType mtype)
            {
                if (mtype.MealTypeID == (int)Enums.MealTypes.BreakfastEntree
                    || mtype.MealTypeID == (int)Enums.MealTypes.LunchEntree
                    || mtype.MealTypeID == (int)Enums.MealTypes.DinnerEntree
                    || mtype.MealTypeID == (int)Enums.MealTypes.ChildEntree
                    || mtype.MealTypeID == (int)Enums.MealTypes.OtherEntree)
                    mealsPerDay += mtype.RequiredQuantity;
            });

            return mealsPerDay;
        }

        public static List<hccProgram> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPrograms.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// This method gets all programs, with a specifier pertaining to program activation.
        /// </summary>
        /// <param name="isActive">Determines whether the method is to return records depending upon their IsActive flag; Active(true), Inactive(false), All(null)</param>
        /// <returns>List&lt;hccProgram&gt;</returns>
        /// <exception cref="System.Exception">re-thrown exception.</exception>
        public static List<hccProgram> GetBy(bool? isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    if (isActive.HasValue)
                    {
                        return cont.hccPrograms
                            .Where(a => a.IsActive == isActive)
                            .ToList();
                    }
                    else
                    {
                        return cont.hccPrograms
                            .ToList();
                    }
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProgram GetBy(string programName)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPrograms
                        .Where(a => a.Name == programName)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProgram GetById(int id)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccPrograms.SingleOrDefault(a => a.ProgramID == id);
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<ProgramPlanCountItemHolder> GetProgramPlanCounts(DateTime startDate, DateTime endDate)
        {
            try
            {
                List<ProgramPlanCountItemHolder> retVals = new List<ProgramPlanCountItemHolder>();

                List<MealItemReportItem> mealItems = ReportSprocs.GetMenuItemsByDateRange(startDate, endDate)
                    .Where(a => ((Enums.MealTypes)a.MealTypeId).ToString().Contains("Entree"))
                    .OrderBy(a => a.DeliveryDate).ThenBy(a => a.CartItemId).ThenBy(a => a.DayNum).ThenBy(a => a.MealTypeId).ToList();

                mealItems.ForEach(delegate(MealItemReportItem result)
                {
                    if (retVals.Count(a => a.DeliveryDate == result.DeliveryDate
                        && a.PlanId == result.PlanId) == 0)
                    {
                        ProgramPlanCountItemHolder newVal = new ProgramPlanCountItemHolder
                        {
                            DeliveryDate = result.DeliveryDate,
                            PlanId = result.PlanId
                        };

                        retVals.Add(newVal);
                    }
                });

                retVals.ForEach(delegate(ProgramPlanCountItemHolder holder)
                {
                    if (holder.PlanId == 0)
                    {
                        holder.PlanName = "ALC";
                        holder.ProgramName = "ALC";
                    }
                    else
                    {
                        hccProgramPlan plan = hccProgramPlan.GetById(holder.PlanId);

                        if (plan != null)
                        {
                            holder.PlanName = plan.Name;

                            hccProgram program = hccProgram.GetById(plan.ProgramID);

                            if (program != null)
                                holder.ProgramName = program.Name;
                        }
                    }

                    var oc = mealItems.Where(a => a.DeliveryDate == holder.DeliveryDate
                        && a.PlanId == holder.PlanId)
                        .GroupBy(a => a.OrderNumber).Distinct();
                    holder.OrderCount = oc.Count();

                    int mealCount = 0;
                    oc.ToList().ForEach(delegate(IGrouping<string, MealItemReportItem> orderNumGroup)
                    {
                        var g = orderNumGroup.ToList();

                        g.ForEach(delegate(MealItemReportItem result)
                        {
                            mealCount += result.Quantity;
                        });
                    });

                    holder.MealCount = mealCount;
                });

                return retVals.OrderBy(a => a.DeliveryDate).ThenBy(a => a.ProgramName).ThenBy(a => a.PlanName).ToList();

            }
            catch
            {
                throw;
            }
        }
    }

    public class ProgramPlanCountItemHolder
    {
        public DateTime DeliveryDate { get; set; }
        public int PlanId { get; set; }
        public string ProgramName { get; set; }
        public string PlanName { get; set; }
        public int OrderCount { get; set; }
        public int MealCount { get; set; }

        public ProgramPlanCountItemHolder()
        { }
    }
}
