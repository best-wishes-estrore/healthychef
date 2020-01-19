using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccProgramPlan
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        public string NameAndPrice
        {
            get
            {
                return string.Format("{0} ({1})", this.Name, this.PricePerDay.ToString("c"));
            }
        }

        public int MealsPerDay
        {
            get { return hccProgram.GetById(this.ProgramID).GetMealsPerDay(); }
        }

        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccProgramPlans", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccProgramPlans", this);
                    }
                    else
                    {
                        cont.hccProgramPlans.AddObject(this);
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
                this.Save();
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProgramPlan> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramPlans
                        .Join(cont.hccPrograms, a => a.ProgramID, b => b.ProgramID, (j1, j2) => new { Plan= j1, Program=j2})
                        .OrderBy(a => a.Program.Name)
                            .ThenBy(a => a.Plan.NumWeeks)
                            .ThenBy(a => a.Plan.NumDaysPerWeek)
                        .Select(a=> a.Plan)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProgramPlan> GetBy(int programId, bool? isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    if (isActive.HasValue)
                    {
                        return cont.hccProgramPlans
                            .Where(a => a.ProgramID == programId && a.IsActive == isActive)
                            .OrderBy(a => a.NumWeeks)
                                .ThenBy(a => a.NumDaysPerWeek)
                            .ToList();
                    }
                    else
                    {
                        return cont.hccProgramPlans
                            .Where(a => a.ProgramID == programId)
                            .OrderBy(a => a.NumWeeks)
                                .ThenBy(a => a.NumDaysPerWeek)
                            .ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<hccProgramPlan> GetBy(bool? isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    if (isActive.HasValue)
                    {
                        return cont.hccProgramPlans
                            .Where(a => a.IsActive == isActive)
                            .OrderBy(a => a.hccProgram.Name)
                                .ThenBy(a => a.NumWeeks)
                                .ThenBy(a => a.NumDaysPerWeek)
                            .ToList();
                    }
                    else
                    {
                        return cont.hccProgramPlans
                            .OrderBy(a => a.hccProgram.Name)
                                .ThenBy(a => a.NumWeeks)
                                .ThenBy(a => a.NumDaysPerWeek)
                            .ToList();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccProgramPlan GetById(int id)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramPlans
                        .Where(a => a.PlanID == id)
                        .SingleOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccProgramPlan GetBy(string planName)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var p = cont.hccProgramPlans
                        .Where(a => a.Name == planName)
                        .SingleOrDefault();

                    return p;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
