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
                using (var cont = new healthychefEntities())
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
            var plans = hccProgramPlan.GetBy(this.ProgramID, true);
            int numberofweeks = plans.Where(x => x.NumWeeks < 5).Max(x => x.NumWeeks);
            plans = hccProgramPlan.GetBy(this.ProgramID, true).Where(x=>x.NumWeeks<= numberofweeks).ToList();
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
                using (var cont = new healthychefEntities())
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
                using (var cont = new healthychefEntities())
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

        public static List<hccCartItem> GetCartItemsByCartId(int cartId, DateTime deliveryDate)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartItems.Where(c => c.CartID == cartId && c.ItemPrice> 0 && c.DeliveryDate== deliveryDate).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static hccProgram GetBy(string programName)
        {
            try
            {
                using (var cont = new healthychefEntities())
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
        public static hccProgram GetByMoreInfoId(int Moreinfoid)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccPrograms
                        .Where(a => a.MoreInfoNavID == Moreinfoid&&a.DisplayOnWebsite==true&&a.IsActive==true)
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
                using (var cont = new healthychefEntities())
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
                    if (result.PlanId != 0)
                    {
                        if (retVals.Count(a => a.DeliveryDate == result.DeliveryDate && a.PlanId == result.PlanId) == 0)
                        {
                            ProgramPlanCountItemHolder newVal = new ProgramPlanCountItemHolder
                            {
                                DeliveryDate = result.DeliveryDate,
                                PlanId = result.PlanId
                            };

                            retVals.Add(newVal);
                        }
                    }
                    else
                    {
                        if (retVals.Count(a => a.DeliveryDate == result.DeliveryDate && a.PlanId == result.PlanId && a.IsFamilyStyle==result.IsFamilyStyle ) == 0)
                        {
                            ProgramPlanCountItemHolder newVal = new ProgramPlanCountItemHolder
                            {
                                DeliveryDate = result.DeliveryDate,
                                PlanId = result.PlanId,
                                IsFamilyStyle=result.IsFamilyStyle,
                                CartId=result.CartId
                            };

                            retVals.Add(newVal);
                        }
                    }
                });
                int UpdateOrderCount = 0;
                int UpdateCount = 0;
                bool IsUpdate = false;
                List<int> CartIds;
                hccCartItem hcccartItem = new hccCartItem();
                List<hccCartItem> hcccartItems = new List<hccCartItem>();
                retVals.ForEach(delegate(ProgramPlanCountItemHolder holder)
                {
                    if (holder.PlanId == 0 && holder.IsFamilyStyle==false)
                    {
                        holder.PlanName = "ALC Individual Portions";
                        holder.ProgramName = "ALC Individual Portions";
                    }
                    else if (holder.PlanId == 0 && holder.IsFamilyStyle == true)
                    {
                        holder.PlanName = "ALC Family Style";
                        holder.ProgramName = "ALC Family Style";
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
                    if (holder.PlanId == 0 && holder.IsFamilyStyle == false)
                    {
                        oc = mealItems.Where(a => a.DeliveryDate == holder.DeliveryDate
                        && a.PlanId == holder.PlanId && a.IsFamilyStyle==holder.IsFamilyStyle)
                        .GroupBy(a => a.OrderNumber).Distinct();
                    }
                    else if (holder.PlanId == 0 && holder.IsFamilyStyle == true)
                    {
                        oc = mealItems.Where(a => a.DeliveryDate == holder.DeliveryDate
                        && a.PlanId == holder.PlanId && a.IsFamilyStyle == holder.IsFamilyStyle)
                        .GroupBy(a => a.OrderNumber).Distinct();
                    }
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
                    if (holder.PlanId == 0 && holder.IsFamilyStyle == false)
                    {
                        CartIds = mealItems.Where(x => x.PlanId == 0 && x.DeliveryDate == holder.DeliveryDate).Select(x => x.CartId).Distinct().ToList();
                        foreach (var id in CartIds)
                        {
                            hcccartItems = GetCartItemsByCartId(id, holder.DeliveryDate);

                            if (hcccartItems.Count() > 1)
                            {
                                foreach (var item in hcccartItems)
                                {
                                    if (item.Plan_IsAutoRenew == true && item.ItemTypeID == 1)
                                    {
                                        UpdateOrderCount++;
                                    }
                                    else if(item.Plan_IsAutoRenew==false && item.ItemTypeID==1)
                                    {
                                        IsUpdate = true;
                                    }
                                }
                                if(UpdateOrderCount>0 && IsUpdate)
                                {
                                    UpdateCount++;
                                    UpdateOrderCount = 0;
                                }
                            }
                            IsUpdate = false;
                        }
                        if (UpdateCount > 0)
                        {
                            var items = retVals.Where(x => x.PlanName == "ALC Individual Portions" && x.DeliveryDate == holder.DeliveryDate).FirstOrDefault();
                            items.OrderCount = items.OrderCount - UpdateCount;
                            UpdateCount = 0;
                            //foreach (var item in retVals)
                            //{
                            //    if (item.PlanName == "ALC Individual Portions" && item.DeliveryDate==holder.DeliveryDate)
                            //    {
                            //        item.OrderCount = item.OrderCount - UpdateOrderCount;
                            //    }
                            //}
                        }
                    }
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
        public bool IsFamilyStyle { get; set; }
        public int CartId { get; set; }
        public ProgramPlanCountItemHolder()
        { }
    }
}
