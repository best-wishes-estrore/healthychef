using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Reflection;
using AuthorizeNet;
using System.Web.Security;

namespace HealthyChef.DAL
{
    public partial class ImportedGiftCert
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
                    System.Data.EntityKey key = cont.CreateEntityKey("ImportedGiftCerts", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("ImportedGiftCerts", this);
                    }
                    else
                    {
                        cont.ImportedGiftCerts.AddObject(this);
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
                this.is_used = isRetired ? "Y" : "N";
                this.Save();
            }
            catch
            {
                throw;
            }
        }

        public static ImportedGiftCert GetById(int id)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.ImportedGiftCerts.FirstOrDefault(a => a.ImportsGCID == id);
                }
            }
            catch
            {
                throw;
            }
        }

        public static ImportedGiftCert GetBy(string code)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    long inCode;
                    bool rt = long.TryParse(code, out inCode);

                    if (rt)
                    {
                        var p = cont.ImportedGiftCerts
                            .Where(a => a.code.Value == inCode)
                            .SingleOrDefault();

                        return p;
                    }
                    else
                        return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<ImportedGiftCert> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.ImportedGiftCerts.ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<ImportedGiftCert> GetActive()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.ImportedGiftCerts
                        .Where(a => a.is_used == "N")
                        .OrderBy(b => b.code)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<ImportedGiftCert> GetInactive()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.ImportedGiftCerts.Where(a => a.is_used == "Y").ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
