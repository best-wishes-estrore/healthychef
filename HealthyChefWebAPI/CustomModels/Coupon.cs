using HealthyChef.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class Coupon
    {
        public int CouponID { get; set; }
        public int DiscountTypeID { get; set; }
        public int UsageTypeID { get; set; }
        public string RedeemCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public string StartDateString { get; set; }
        public string EndDateString { get; set; }

        public string EffectiveDates
        {
            get
            {
                return _getEffectiveDatestring();
            }
        }
        public string Details
        {
            get
            {
                return _getDetailstring();
            }
        }

        string _getEffectiveDatestring()
        {
            string tempDates = string.Empty;
            try
            {
                if (this.StartDate.HasValue)
                {
                    tempDates = "Start: " + this.StartDate.Value.ToShortDateString();

                    if (this.EndDate.HasValue)
                        tempDates += " - End: " + this.EndDate.Value.ToShortDateString();
                }
                else
                {
                    if (this.EndDate.HasValue)
                        tempDates += "End: " + this.EndDate.Value.ToShortDateString();
                }
                return tempDates;
            }
            catch
            {
                return tempDates;
            }
        }

        string _getDetailstring()
        {
            string tempDetails = string.Empty;
            try
            {
                switch (this.DiscountTypeID)
                {
                    case 1:
                        tempDetails = string.Format("{0:c} for {1}", this.Amount, Enums.GetEnumDescription((Enums.CouponUsageType)UsageTypeID));
                        break;
                    case 2:
                        tempDetails = string.Format("{0:p} for {1}", this.Amount / 100, Enums.GetEnumDescription((Enums.CouponUsageType)UsageTypeID));
                        break;
                    default: break;
                }

                return tempDetails;
            }
            catch
            {
                return tempDetails;
            }
        }
    }


    public class CouponItem : Helpers.PostHttpResponse
    {
        public int CouponID { get; set; }
        public int DiscountTypeID { get; set; }
        public int UsageTypeID { get; set; }
        public string RedeemCode { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public bool IsActive { get; set; }
        public Guid? CreatedBy { get; set; }


        public CouponItem()
        {

        }

        public CouponItem(Coupon _couponToValidate)
        {
            if (string.IsNullOrEmpty(_couponToValidate.RedeemCode))
            {
                this.AddValidationError("A redeem code is required.");
                this.isValid = false;
            }
            if (string.IsNullOrEmpty(_couponToValidate.Title))
            {
                this.AddValidationError("A title is required.");
                this.isValid = false;
            }
            //if (_couponToValidate.Amount == decimal.Zero)
            //{
            //    this.AddValidationError("Coupon Amount is Required");
            //    this.isValid = false;
            //}

            if (_couponToValidate.DiscountTypeID == -1)
            {
                this.AddValidationError("An amount is required.");
                this.isValid = false;
            }

            if (_couponToValidate.UsageTypeID == -1)
            {
                this.AddValidationError("A discount type is required.");
                this.isValid = false;
            }

        }
    }
}