using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;

namespace HealthyChef.DAL
{
    public partial class hccMenu
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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccMenus", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccMenus", this);
                    }
                    else
                    {
                        cont.hccMenus.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public static List<hccMenu> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenus
                        .OrderBy(a => a.Name)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccMenu GetById(int menuId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenus.SingleOrDefault(a => a.MenuID == menuId);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static hccMenu GetBy(string menuName)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccMenus.SingleOrDefault(a => a.Name == menuName);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<hccMenu> GetByMenuItemId(int menuItemId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    var m = cont.hccMenuItems.SingleOrDefault(a => a.MenuItemID == menuItemId);

                    return m.hccMenus.ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<hccMenuItem> GetMenuItems(bool? isRetired)
        {
            try
            {
                if (isRetired.HasValue)
                {
                    return hccMenuItem.GetByMenuId(this.MenuID)
                        .Where(a => a.IsRetired == isRetired)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
                else
                {
                    return hccMenuItem.GetByMenuId(this.MenuID)
                        .OrderBy(b => b.Name)
                        .ToList();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<hccMenuItem> GetMenuItems(bool? isRetired, HealthyChef.Common.Enums.MealTypes mealType)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return GetMenuItems(isRetired)
                        .Where(a => a.MealTypeID == (int)mealType)
                        .OrderBy(b => b.Name)
                        .ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void RemoveItems()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    var t = cont.hccMenus.SingleOrDefault(a => a.MenuID == this.MenuID);
                    List<hccMenuItem> items = t.hccMenuItems.ToList();

                    items.ForEach(a => t.hccMenuItems.Remove(a));
                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }

        public void AddItems(List<hccMenuItem> addItems)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    var t = cont.hccMenus.SingleOrDefault(a => a.MenuID == this.MenuID);
                    addItems.ForEach(delegate(hccMenuItem addItem)
                    {
                        cont.AttachTo("hccMenuItems", addItem);
                        t.hccMenuItems.Add(addItem);
                    });
                    cont.SaveChanges();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
