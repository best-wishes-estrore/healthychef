using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HealthyChef.DAL
{
    public partial class hccCartALCMenuItem
    {
        /// <summary>
        /// Saves alc menu item of cart to the database.
        /// </summary>
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var key = cont.CreateEntityKey("hccCartALCMenuItems", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCartALCMenuItems", this);
                    }
                    else
                    {
                        cont.hccCartALCMenuItems.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Deletes alc menu item of cart from the database.
        /// </summary>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>Returns void.</returns>
        public void Delete()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var key = cont.CreateEntityKey("hccCartALCMenuItems", this);
                    object originalItem = null;

                    if (cont.TryGetObjectByKey(key, out originalItem))
                    {
                        var cartItem = (hccCartALCMenuItem)originalItem;

                        cont.hccCartALCMenuItems.DeleteObject(cartItem);
                        cont.SaveChanges();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static hccCartALCMenuItem GetByCartItemID(int parentCartItemId, int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartALCMenuItems.FirstOrDefault(c => c.ParentCartItemID == parentCartItemId && c.CartItemID == cartItemId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCartALCMenuItem GetByCartItemID(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartALCMenuItems.FirstOrDefault(c => c.CartItemID == cartItemId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCartALCMenuItem GetByOrdinal(int parentCartItemId, int ordinal)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccCartALCMenuItems.FirstOrDefault(c => c.ParentCartItemID == parentCartItemId && c.Ordinal == ordinal);
                }
            }
            catch
            {
                throw;
            }
        }
    }
}