using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class CartItem
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
        public Guid Gift_IssuedTo { get; set; }
        public DateTime Gift_IssuedDate { get; set; }
        public Guid Gift_RedeemedBy { get; set; }
        public int Gift_RedeemedDate { get; set; }
        public int Gift_RecipientAddressId { get; set; }
        public string Gift_RecipientEmail { get; set; }
        public string Gift_RecipientMessage { get; set; }
        public int Meal_MenuItemID { get; set; }
        public int Meal_MealSizeID { get; set; }
        public decimal Meal_ShippingCost { get; set; }
        public int Plan_PlanID { get; set; }
        public int Plan_ProgramOptionID { get; set; }
        public bool Plan_IsAutoRenew { get; set; }
        public Guid CreatedBy { get; set; }
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
        public string ProfileName { get; set; }
    }


    public class CartItemResponse : Helpers.PostHttpResponse
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
        public Guid Gift_IssuedTo { get; set; }
        public DateTime Gift_IssuedDate { get; set; }
        public Guid Gift_RedeemedBy { get; set; }
        public int Gift_RedeemedDate { get; set; }
        public int Gift_RecipientAddressId { get; set; }
        public string Gift_RecipientEmail { get; set; }
        public string Gift_RecipientMessage { get; set; }
        public int Meal_MenuItemID { get; set; }
        public int Meal_MealSizeID { get; set; }
        public decimal Meal_ShippingCost { get; set; }
        public int Plan_PlanID { get; set; }
        public int Plan_ProgramOptionID { get; set; }
        public bool Plan_IsAutoRenew { get; set; }
        public Guid CreatedBy { get; set; }
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
}