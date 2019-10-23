using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HealthyChef.Common;
using ZipToTaxService;

namespace HealthyChef.DAL
{
    [Serializable]
    public class ProfileCart
    {
        /// <summary>
        /// SubTotal of ProfileCart CartItems.ItemSubTotalNA
        /// </summary>
        public decimal SubTotalNA
        {
            get
            {
                decimal profSubTotal = 0.00m;
                this.CartItems.ForEach(a => profSubTotal += a.ItemSubTotalNA);
                return profSubTotal;
            }
        }

        public double MockProfileSubTotal
        {
            get
            {
                double MockProfileSubTotal = 0.00;
                this.CartItems.ForEach(a => MockProfileSubTotal += a.mockTotalPrice);
                return MockProfileSubTotal;
            }
            set
            {

            }
        }

        /// <summary>
        /// SubTotal of ProfileCart CartItems.ItemSubTotalNA
        /// </summary>
        public decimal SubTotalAdj
        {
            get
            {
                decimal profAdjSubTotal = 0.00m;
                this.CartItems.ForEach(a => profAdjSubTotal += a.ItemSubTotalAdj);
                return profAdjSubTotal;
            }
        }

        /// <summary>
        /// SubTotal of ProfileCart CartItems.ItemSubTotalNA
        /// </summary>
        public decimal SubDiscountAmount
        {
            get
            {
                decimal profSubDiscountAmount = 0.00m;
                this.CartItemsWithMealSides.ForEach(a => profSubDiscountAmount += (a.DiscountPerEach));
                return profSubDiscountAmount;
            }
        }

        /// <summary>
        /// SubTotal of ProfileCart CartItems.SubMealsCount
        /// </summary>
        public decimal SubMealsCount
        {
            get
            {
                decimal profSubMealsCount = 0.00m;
                this.CartItems.ForEach(a => profSubMealsCount += (a.NumberOfMeals * a.Quantity));
                return profSubMealsCount;
            }
        }

        public decimal SubTax
        {
            get
            {
                decimal profSubTax = 0.00m;
                // if shipping address is FL, add tax for taxable items
                if (ShippingAddress != null && ShippingAddress.State == "FL")
                {
                    try
                    {
                        TaxLookup taxInfo = TaxLookup.RequestTax(ShippingAddress.PostalCode);
                        if (!taxInfo.State.Contains("Error"))
                        {
                            decimal taxRate = decimal.Parse(taxInfo.Sales_Tax_Rate);

                            if (this.CartItemsWithMealSides.Count > 0)
                            {

                                this.CartItemsWithMealSides.ForEach(delegate (hccCartItem cartItem)
                                {
                                    if (cartItem.IsTaxable)
                                    {
                                        decimal itemTax = 0.00m;
                                        decimal taxableAmt = 0.00m;
                                        double discountpereachamount = 0.0;

                                        //If Item is FamilyStyle
                                        if (cartItem.Plan_IsAutoRenew == true)
                                        {
                                            if (cartItem.ItemTypeID == 1)
                                            {
                                                discountpereachamount = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.10), 2));
                                                taxableAmt = cartItem.ItemPrice * cartItem.Quantity - Convert.ToDecimal(discountpereachamount);
                                            }
                                            else
                                            {
                                                discountpereachamount = Convert.ToDouble(Math.Round(Convert.ToDecimal((Convert.ToDouble(cartItem.ItemPrice) * cartItem.Quantity) * 0.05), 2));
                                                taxableAmt = cartItem.ItemPrice * cartItem.Quantity - Convert.ToDecimal(discountpereachamount);
                                            }
                                        }
                                        else
                                        {
                                            taxableAmt = cartItem.ItemPrice * cartItem.Quantity;
                                        }
                                        taxableAmt -= cartItem.DiscountPerEach;

                                        if (taxableAmt == cartItem.ItemPrice)
                                        {
                                            itemTax = Math.Round((taxableAmt * (taxRate / 100)), 2);
                                        }
                                        else
                                        {
                                            itemTax = Math.Round((taxableAmt * (taxRate / 100)), 2);
                                        }
                                        profSubTax += itemTax;

                                        decimal baseTaxAmt = Math.Round(taxableAmt * .06m, 2);
                                        decimal discTaxAmt = itemTax - baseTaxAmt;

                                        SubTaxableAmount += taxableAmt;
                                        SubDiscretionaryTaxAmount += discTaxAmt;

                                        cartItem.TaxRateAssigned = taxRate;
                                        cartItem.TaxableAmount = taxableAmt;
                                        cartItem.DiscretionaryTaxAmount = discTaxAmt;
                                        cartItem.Save();
                                    }
                                });
                            }
                            profSubTax = Math.Round((SubTaxableAmount * (taxRate / 100)), 2);
                        }
                        else
                        {
                            throw new Exception("Tax Lookup could not determine the tax rate for this zip code: " + ShippingAddress.PostalCode);
                        }
                    }
                    catch { throw; }
                }
                return profSubTax;
            }
        }

        public decimal SubTaxableAmount { get; set; }
        public decimal SubDiscretionaryTaxAmount { get; set; }
        public decimal ShippingFee { get; set; }

        /// <summary>
        /// this used for calculations purposes only
        /// </summary>
        public decimal SubShippingActual
        {
            get
            {
                decimal profActShip = 0.00m;

                try
                {
                    this.CartItemsWithMealSides.ForEach(delegate (hccCartItem cartItem)
                    {
                        if (cartItem.ItemType == Common.Enums.CartItemType.AlaCarte)
                        {
                            if (ShippingAddress == null)
                            {
                                if (cartItem.Meal_ShippingCost.HasValue)
                                    profActShip = Math.Round(profActShip + (cartItem.Meal_ShippingCost.Value * cartItem.Quantity), 2);
                                else
                                    throw new Exception("ProfileCart.SubActualShipping - MealItem does not have a shipping amount.");
                            }
                            else
                            {
                                if (ShippingAddress.DefaultShippingTypeID == 0)
                                {
                                    ShippingAddress.DefaultShippingTypeID = (int)Enums.DeliveryTypes.Delivery;
                                    ShippingAddress.Save();
                                }

                                if ((Enums.DeliveryTypes)ShippingAddress.DefaultShippingTypeID == Enums.DeliveryTypes.LocalPickUp)
                                    profActShip = 0.00m;
                                else
                                {
                                    if (cartItem.Meal_ShippingCost.HasValue)
                                        profActShip = Math.Round(profActShip + (cartItem.Meal_ShippingCost.Value * cartItem.Quantity), 2);
                                    else
                                        throw new Exception("ProfileCart.SubActualShipping - MealItem does not have a shipping amount.");
                                }
                            }
                        }
                    });
                }
                catch { throw; }

                return profActShip;
            }
        }

        public decimal SubShipping
        {
            get
            {
                decimal profSubShip = 0.00m;

                try
                {
                    if (SubShippingActual > 0.00m)
                    {
                        hccGlobalSetting globals = hccGlobalSetting.GetSettings();

                        if (SubShippingActual >= globals.DeliveryMinCost && SubShippingActual <= globals.DeliveryMaxCost)
                        { profSubShip = SubShippingActual; }
                        else
                        {
                            if (HasProgramPlan)
                            {
                                if (SubShippingActual > globals.DeliveryMaxCost)
                                    profSubShip = globals.DeliveryMaxCost;
                                else
                                    profSubShip = SubShippingActual;
                            }
                            else
                            {
                                if (SubShippingActual < globals.DeliveryMinCost)
                                    profSubShip = globals.DeliveryMinCost;

                                if (SubShippingActual > globals.DeliveryMaxCost)
                                    profSubShip = globals.DeliveryMaxCost;
                            }
                        }
                    }
                }
                catch { throw; }

                return profSubShip;
            }
        }

        public ProfileCart()
        {
            CartItems = new List<hccCartItem>();
        }

        public ProfileCart(int shippingAddressId, DateTime deliveryDate)
            : this()
        {
            ShippingAddressId = shippingAddressId;
            DeliveryDate = deliveryDate;
        }

        public int ShippingAddressId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public List<hccCartItem> CartItems { get; set; }


        public List<hccCartItem> CartItemsWithMealSides
        {
            get
            {
                var cartItems = new List<hccCartItem>();

                if (this.CartItems == null)
                    return cartItems;

                this.CartItems.ForEach(delegate (hccCartItem cartItem)
                {
                    cartItems.Add(cartItem);
                    cartItems.AddRange(cartItem.MealSideItems);
                });

                return cartItems;
            }
        }
        public hccAddress ShippingAddress { get { if (ShippingAddressId > 0) { return hccAddress.GetById(ShippingAddressId); } else { return null; } } }
        public bool HasProgramPlan { get { return CartItems.Exists(a => a.ItemType == Enums.CartItemType.DefinedPlan); } }
    }
}
