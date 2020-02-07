using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using HealthyChefWebAPI.Helpers;
using static HealthyChef.Common.Enums;
using HealthyChef.DAL;

namespace HealthyChefWebAPI.CustomModels
{
    public class OrderFullFillMent
    {
        public int CartItemID { get; set; }
        public int CartID { get; set; }
        public int UserProfileID { get; set; }
        public int ItemTypeID { get; set; }
        public string ItemName { get; set; }
        public string ItemDesc { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
        public bool IsTaxable { get; set; }
        public string OrderNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Gift_RedeemCode { get; set; }
        public Guid? Gift_IssuedTo { get; set; }
        public DateTime Gift_IssuedDate { get; set; }
        public Guid? Gift_RedeemedBy { get; set; }
        public DateTime Gift_RedeemedDate { get; set; }
        public int Gift_RecipientAddressId { get; set; }
        public string Gift_RecipientEmail { get; set; }
        public string Gift_RecipientMessage { get; set; }
        public int Meal_MenuItemID { get; set; }
        public int Meal_MealSizeID { get; set; }
        public decimal Meal_ShippingCost { get; set; }
        public int Plan_PlanID { get; set; }
        public int Plan_ProgramOptionID { get; set; }
        public bool Plan_IsAutoRenew { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsCancelled { get; set; }
        public bool IsFulfilled { get; set; }
        public decimal DiscountPerEach { get; set; }
        public decimal DiscountAdjPrice { get; set; }
        public int SnapBillAddrId { get; set; }
        public int SnapShipAddrId { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxableAmount { get; set; }
        public decimal DiscretionaryTaxAmount { get; set; }
        public decimal TaxRateAssigned { get; set; }

    }

    public class OrderFullFillMentDetails
    {
        public int CartID { get; set; }
        public int CartItemID { get; set; }
        public string OrderNum { get; set; }
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public int Quantity { get; set; }
        public DateTime DeliveryDateObj { get; set; }
        public CartItemType ItemType { get; set; }
        public string Status { get; set; }

        public bool Postpone
        {
            get
            {
                return _IsPostponeProperty();
            }
        }

        public string DeliveryDate
        {
            get
            {
                return DeliveryDateObj.ToShortDateString();
            }
        }

        public string Allergens
        {
            get
            {
                return _getAllergensText();
            }
        }


        private string _getAllergensText()
        {
            string _allergenString = string.Empty;

            if (this.ItemType == CartItemType.DefinedPlan)
            {
                hccCartItemCalendar cartCal = hccCartItemCalendar.GetBy(this.CartItemID, this.DeliveryDateObj);
                var _cartItem = hccCartItem.GetById(this.CartItemID);
                if (_cartItem.GetDaysWithAllergens(cartCal.CartCalendarID).Count > 0)
                {
                    _allergenString = "Alert";
                }
                else
                {
                    _allergenString = "Ok";
                }
            } 

            return _allergenString;
        }

        private bool _IsPostponeProperty()
        {
            bool _postpone = false;

            if (this.ItemType == CartItemType.DefinedPlan)
            {
                _postpone = true;
            }
            else
            {
                _postpone = false;
            }

            return _postpone;
        }

    }
}