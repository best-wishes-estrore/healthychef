using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace HealthyChefWebAPI.Helpers
{
    public class Enums
    {
        /// <summary>
        /// Use this method to return the DescriptionAttribute from a particular instance of an enumeration value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetEnumDescription(object value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        /// <summary>
        /// Gets an enum description
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="useDefaultValueOnError"></param>
        /// <returns></returns>
        public static string GetEnumDescription<T>(object value, bool useDefaultValueOnError)
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception("Must be an Enum Type");
            }

            try
            {
                return GetEnumDescription((T)value);
            }
            catch
            {
                if (useDefaultValueOnError)
                {
                    return GetEnumDescription<T>(0, false);
                }

                throw;
            }

        }

        public static string GetComplexEnumDescription<T>(T value)
        {
            Type type = typeof(T);
            if (!type.IsEnum)
            {
                throw new Exception("Must be an Enum Type");
            }

            string t = string.Empty;
            int a, b;

            List<T> items = Enum.GetValues(type).OfType<T>().ToList();
            if (items.Count > 0)
            {
                foreach (var item in items)
                {
                    a = Convert.ToInt32(item);
                    b = Convert.ToInt32(value);

                    if (a == 0)
                    {
                        continue;
                    }

                    if ((a & b) == a)
                    {
                        t += GetEnumDescription(item) + " ";
                    }
                }
            }

            return t.TrimEnd(new char[] { ' ' });
        }

        /// <summary>
        /// Use this method to return a List&lt;Tuple&lt;string, int&gt;&gt; representing the DescriptionAttribute[Item1] and int value[Item2] of all items within an enumeration.  
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns>Tuple&lt;string, string&gt;</returns>
        public static List<Tuple<string, int>> GetEnumAsTupleList(Type enumType)
        {
            List<Tuple<string, int>> discTypes = new List<Tuple<string, int>>();

            foreach (string discName in Enum.GetNames(enumType))
            {
                object discType = Enum.Parse(enumType, discName);

                string s1 = Enums.GetEnumDescription(discType);
                int s2 = (int)discType;

                discTypes.Add(new Tuple<string, int>(s1, s2));
            }

            return discTypes;
        }

        public enum PreferenceType : int // unknown = 0 to match 0 default value used for int in sql server.
        {
            Customer = 0,
            Meal = 1
        }

        public enum AddressType : int
        {
            Unknown = 0,
            Billing = 2,
            BillingSnap = 3,
            Shipping = 4,
            ShippingSnap = 5,
            GiftRecipient = 6
        }

        public enum PhoneType : int
        {
            HomePhone = 1,
            CellPhone = 2,
            WorkPhone = 3,
            Other = 4
        }

        public enum PhoneActivationStatus : int
        {
            Activated = 1,
            Deactivated = 2
        }

        public enum CreditCardType : int
        {
            Unknown = 0,
            Visa = 1,
            [Description("Master Card")]
            MasterCard = 2,
            [Description("American Express")]
            AmericanExpress = 3,
            Discover = 4
        }

        public enum LedgerTransactionType : int
        {
            [Description("HCC Account Credit")]
            HCCAccountCredit = 10,
            [Description("HCC Account Debit")]
            HCCAccountDebit = 20,
            [Description("Purchase")]
            Purchase = 30,
            [Description("Redeem Gift Certificate")]
            RedeemGiftCertificate = 40,
            [Description("Return")]
            Return = 50
        }

        public enum OrderType : int
        {
            [Description("À La Carte")]
            AlaCarte = 0,
            [Description("Defined Plan")]
            DefinedPlan = 1
        }

        public enum MealTypes : int
        {
            [Description("Unknown")]
            Unknown = 0,
            [Description("Breakfast Entree")]
            BreakfastEntree = 10,
            [Description("Breakfast Side")]
            BreakfastSide = 20,
            [Description("Lunch Entree")]
            LunchEntree = 30,
            [Description("Lunch Side")]
            LunchSide = 40,
            [Description("Dinner Entree")]
            DinnerEntree = 50,
            [Description("Dinner Side")]
            DinnerSide = 60,
            [Description("Other Entree")]
            OtherEntree = 70,
            [Description("Other Side")]
            OtherSide = 80,
            [Description("Child Entree")]
            ChildEntree = 90,
            [Description("Child Side")]
            ChildSide = 100,
            [Description("Salad")]
            Salad = 110,
            [Description("Soup")]
            Soup = 120,
            [Description("Dessert")]
            Dessert = 130,
            [Description("Beverage")]
            Beverage = 140,
            [Description("Snack")]
            Snack = 150,
            [Description("Supplement")]
            Supplement = 160,
            [Description("Goods")]
            Goods = 170,
            [Description("Miscellaneous")]
            Miscellaneous = 180
        }

        public enum MealTabItems
        {
            Breakfast,
            Lunch,
            Dinner,
            Child,
            Dessert,
            Other
        }

        public enum DeliveryTypes : int
        {
            [Description("Overnight Shipping")]
            Delivery = 1,
            [Description("Customer Pickup")]
            LocalPickUp = 2,
            [Description("Local Delivery")]
            LocalDelivery = 3

        }

        public enum CouponDiscountType : int
        {
            [Description("$ (Dollars)")]
            Monetary = 1,
            [Description("% (Percent)")]
            Percentage = 2
        }

        public enum CouponUsageType : int
        {
            [Description("Unlimited Use")]
            UnlimitedUse = 1,
            [Description("One Time Use")]
            OneTimeUse = 2,
            [Description("First Purchase Only")]
            FirstPurchaseOnly = 3
        }

        public enum UserProfileNoteTypes
        {
            [Description("Unknown")]
            Unknown = 0,
            [Description("Billing Note")]
            BillingNote = 1,
            [Description("Shipping Note")]
            ShippingNote = 2,
            [Description("General Note")]
            GeneralNote = 3,
            [Description("Preference Note")]
            PreferenceNote = 4,
            [Description("Allergen Note")]
            AllergenNote = 5
        }

        public enum CartStatus : int
        {
            [Description("Unfinalized")] // Any order that has been created, had items added to the list, but not been paid for.
            Unfinalized = 10,
            [Description("Paid")]   // Any order that has been created, had items added to the list, AND been paid for.
            Paid = 20,
            [Description("Fulfilled")] // Any order that has been created, had items added to the list, AND been paid for, and completely shipped.
            Fulfilled = 40,
            [Description("Cancelled")] // Any order that has been created, and then cancelled prior to fulfillment.
            Cancelled = 50
        }

        public enum CartItemType : int
        {
            [Description("À La Carte")]
            AlaCarte = 1,
            [Description("Defined Plan")]
            DefinedPlan = 2,
            [Description("Gift Card")]
            GiftCard = 3

        }

        public enum CartItemTypeAbbr : int
        {
            [Description("ALC")]
            AlaCarte = 1,
            [Description("PGM")]
            DefinedPlan = 2,
            [Description("GC")]
            GiftCard = 3
        }

        public enum CartItemSize : int
        {
            [Description("No Size")]
            NoSize = 0,
            [Description("Child")]
            ChildSize = 1,
            [Description("Small")]
            SmallSize = 2,
            [Description("Regular")]
            RegularSize = 3,
            [Description("Large")]
            LargeSize = 4
        }
    }
}