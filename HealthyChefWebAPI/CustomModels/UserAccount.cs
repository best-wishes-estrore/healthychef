using HealthyChef.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefWebAPI.CustomModels
{
    public class UserAccount
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public string Role { get; set; }
        public string DeliveryDate { get; set; }
    }

    public class UpdateCardInfo
    {
        public string NameonCard { get; set; }
        public string CardNumber { get; set; }
        public int ExpireInMonth { get; set; }
        public int ExpireInYear { get; set; }
        public int CardIDCode { get; set; }
    }

    public class CustomerStatus
    {
        public string UserID { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockedOut { get; set; }
    }

    public class CustomerBasicInfo
    {
        public string UserID { get; set; }
        public string Email { get; set; }
        public string ProfileName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int DefaultCouponId { get; set; }

        public string[] Roles { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockedOut { get; set; }
        public bool CanyonRanchCustomer { get; set; }
    }

    public class CustomerShippingAddress
    {
        public string UserID { get; set; }
        public int ShippingAddressID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }
        public int  DefaultShippingTypeID { get; set; }
        public bool IsBusiness { get; set; }
    }


    public class CustomerBillingInfo
    {
        public string UserID { get; set; }
        public int BillingAddressID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string PostalCode { get; set; }

        public bool UpdateCreditCardInfo { get; set; }

        public string NameOnCard { get; set; }
        public string CardNumber { get; set; }
        public int ExipiresOnMonth { get; set; }
        public int ExipiresOnYear { get; set; }
        public string CardIdCode { get; set; }
        public Enums.CreditCardType CardType
        {
            get
                {
                return ValidateCardNumber(this.CardNumber);
                }
         }

        private Enums.CreditCardType ValidateCardNumber(string sCardNumber)
        {
            string cardNum = sCardNumber.Replace(" ", "");

            Enums.CreditCardType retVal = Enums.CreditCardType.Unknown;

            //validate the type of card is accepted
            if (cardNum.StartsWith("4") == true &&
                (cardNum.Length == 13
                    || cardNum.Length == 16))
            {
                //VISA
                retVal = Enums.CreditCardType.Visa;
            }
            else if ((cardNum.StartsWith("51") == true ||
                      cardNum.StartsWith("52") == true ||
                      cardNum.StartsWith("53") == true ||
                      cardNum.StartsWith("54") == true ||
                      cardNum.StartsWith("55") == true) &&
                     cardNum.Length == 16)
            {
                //MasterCard
                retVal = Enums.CreditCardType.MasterCard;
            }
            else if ((cardNum.StartsWith("34") == true ||
                      cardNum.StartsWith("37") == true) &&
                     cardNum.Length == 15)
            {
                //Amex
                retVal = Enums.CreditCardType.AmericanExpress;
            }
            //else if ((cardNum.StartsWith("300") == true ||
            //          cardNum.StartsWith("301") == true ||
            //          cardNum.StartsWith("302") == true ||
            //          cardNum.StartsWith("304") == true ||
            //          cardNum.StartsWith("305") == true ||
            //          cardNum.StartsWith("36") == true ||
            //          cardNum.StartsWith("38") == true) &&
            //         cardNum.Length == 14)
            //{
            //    //Diners Club/Carte Blanche
            //    retVal = Enums.CreditCardType.DinersClub;
            //}
            else if (cardNum.StartsWith("6011") == true &&
                     cardNum.Length == 16)
            {
                //Discover
                retVal = Enums.CreditCardType.Discover;
            }

            if (retVal != Enums.CreditCardType.Unknown)
            {
                int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
                int checksum = 0;
                char[] chars = cardNum.ToCharArray();
                for (int i = chars.Length - 1; i > -1; i--)
                {
                    int j = ((int)chars[i]) - 48;
                    checksum += j;
                    if (((i - chars.Length) % 2) == 0)
                        checksum += DELTAS[j];
                }

                if ((checksum % 10) != 0)
                    retVal = Enums.CreditCardType.Unknown;
            }

            return retVal;
        }

    }

    public class CustomerNote
    {
        public string UserID { get; set; }
        public int NoteId { get; set; }
        public string Note { get; set; }
        public bool DisplayToUser { get; set; }
        public int NotetypeId { get; set; }
    }

    public class CustomerPreferencesToUpdate
    {
        public string UserID { get; set; }
        public int[] Preferences { get; set; }
    }
}