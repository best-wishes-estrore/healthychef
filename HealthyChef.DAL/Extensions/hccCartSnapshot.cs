using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCartSnapshot
    {
        //static healthychefEntities cont
        //{
        //    get { return healthychefEntities.Default; }
        //}

        //int currencyDecimals = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalDigits;

        /// <summary>
        /// Saves shopping cart header to the database.  If there is not a cart for this UserID it will be created. 
        /// </summary>
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    EntityKey key = cont.CreateEntityKey("hccCartSnapshots", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartSnapshots", this);
                    }
                    else
                    {
                        cont.hccCartSnapshots.AddObject(this);
                    }

                    cont.SaveChanges();
                    //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, this);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes the hccCart object and related hccCartItem entries.
        /// NOTE: Delete cascades to hccCartItems via Foreign Key Update/Delete Specs 
        /// </summary>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns void.</returns>
        //public void Delete()
        //{
        //    try
        //    {
        //        using (var cont = new healthychefEntities())
        //        {
        //            EntityKey key = cont.CreateEntityKey("hccCartSnapshots", this);
        //            object originalItem = null;

        //            if (cont.TryGetObjectByKey(key, out originalItem))
        //            {
        //                cont.hccCartSnapshots.DeleteObject((hccCartSnapshot)originalItem);
        //                cont.SaveChanges();

        //                //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, cont.hccCarts);
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public static hccCartSnapshot GetBy(int cartId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCartSnapshots
                        .FirstOrDefault(c => c.CartId == cartId);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
