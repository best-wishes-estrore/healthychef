using HealthyChef.DAL;
using HealthyChefCreationsMVC.CustomModels;
using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HealthyChef.DAL.Extensions;
using System.Data;
using HealthyChef.Common;
using AuthorizeNet;
using HealthyChef.AuthNet;
using AuthorizeNet.APICore;
using System.Web.Mvc;

namespace HealthyChefCreationsMVC.Repository
{
    public class RegisterAccount
    {        
        public static hccUserProfile GetUserProfileByID(string _userID)
        {
            hccUserProfile profile = new hccUserProfile();

            try
            {
                if (!string.IsNullOrEmpty(_userID))
                {
                    Guid aspNetId = Guid.Parse(_userID);
                    profile = hccUserProfile.GetParentProfileBy(aspNetId);

                }
            }
            catch (Exception E)
            {
                throw new Exception(E.Message);
            }

            return profile;
        }


        public static Enums.CreditCardType ValidateCardNumber(string sCardNumber)
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
    }
