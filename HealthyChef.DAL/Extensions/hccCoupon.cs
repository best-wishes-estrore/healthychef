using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;

namespace HealthyChef.DAL
{
    public partial class hccCoupon
    {
        public Enums.CouponUsageType UsageType
        {
            get { return (Enums.CouponUsageType)this.UsageTypeID; }
        }

        public Enums.CouponDiscountType DiscountType
        {
            get { return (Enums.CouponDiscountType)this.DiscountTypeID; }
        }

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
                    System.Data.EntityKey key = cont.CreateEntityKey("hccCoupons", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccCoupons", this);
                    }
                    else
                    {
                        cont.hccCoupons.AddObject(this);
                    }

                    cont.SaveChanges();
                }
            }
            catch { throw; }
        }

        public void Activate(bool UseReactivateBehavior)
        {
            try
            {
                this.IsActive = UseReactivateBehavior;
                Save();
            }
            catch
            {
                throw;
            }
        }

        public static hccCoupon GetById(int couponId)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCoupons.FirstOrDefault(a => a.CouponID == couponId);
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCoupon> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCoupons
                        .OrderBy(a => a.RedeemCode)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCoupon> GetActive()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCoupons
                        .Where(a => a.IsActive)
                        .OrderBy(a => a.RedeemCode)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static List<hccCoupon> GetInactive()
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCoupons
                        .Where(a => !a.IsActive)
                        .OrderBy(a => a.RedeemCode)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccCoupon GetBy(string redeemCode)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCoupons
                        .SingleOrDefault(a => a.RedeemCode.ToUpper() == redeemCode.ToUpper());
                }
            }
            catch
            {
                throw;
            }
        }

        public string ToString(bool fullDetails)
        {

            StringBuilder retVal = new StringBuilder();
            Enums.CouponDiscountType type = (Enums.CouponDiscountType)this.DiscountTypeID;

            retVal.Append("Coupon: " + this.RedeemCode + "<br>");

            if (fullDetails)
            {
                retVal.Append("Title: " + this.Title + "<br>");

                if (type == Enums.CouponDiscountType.Monetary)
                    retVal.Append("Amount: " + this.Amount.ToString("c") + "<br>");
                else if (type == Enums.CouponDiscountType.Percentage)
                    retVal.Append("Amount: " + this.Amount + "%<br>");

                retVal.Append("Usage: " + ((Enums.CouponUsageType)this.UsageTypeID).ToString() + "<br>");
            }
            return retVal.ToString();
        }


        /// <summary>
        /// return cartIDs of carts matching coupon and user
        /// </summary>
        /// <param name="couponId"></param>
        /// <param name="aspNetUserID"></param>
        /// <returns></returns>
        public static List<hccCart> HasBeenUsedByUser(int couponId, Guid aspNetUserID)
        {
            try
            {
                using (var cont = new healthychefEntities())
                {
                    return cont.hccCarts.Where(a => a.CouponID == couponId && a.AspNetUserID == aspNetUserID).ToList();
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
