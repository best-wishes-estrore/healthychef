using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccProgramOption
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccProgramOptions", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccProgramOptions", this);
                    }
                    else
                    {
                        cont.hccProgramOptions.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public string FullText
        {
            get { return string.Format("{0} ({1})", this.OptionText, this.OptionValue.ToString("f2")); }
        }

        public static hccProgramOption GetById(int programOptionId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramOptions
                        .SingleOrDefault(i => i.ProgramOptionID == programOptionId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccProgramOption> GetBy(int programId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccProgramOptions
                        .Where(a => a.ProgramID == programId)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
