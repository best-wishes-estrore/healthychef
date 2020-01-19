using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Reflection;
using AuthorizeNet;
using System.Web.Security;
using HealthyChef.Common;
using System.Configuration;
using System.Data;
using System.IO;

namespace HealthyChef.DAL
{
    public partial class ImportedCustomer
    {
        //static healthychefEntities _cont;
        //static healthychefEntities cont
        //{
        //    get
        //    {
        //        if (_cont == null)
        //            _cont = new healthychefEntities(ConfigurationManager.ConnectionStrings["healthychefEntities"].ConnectionString);

        //        return _cont;
        //    }
        //}

        public bool IsValid
        {
            get
            {
                return (this.Email != null && !string.IsNullOrWhiteSpace(this.Email));
            }
        }

        public static void Import(out int count)
        {
            using (StreamWriter writer = new StreamWriter(@"C:\HCCCustomerOutput.txt"))
            {
                Console.SetOut(writer);

                count = 0;
                List<string> errorReport = new List<string>();

                try
                {
                    List<ImportedCustomer> impCusts = GetAll();

                    foreach (ImportedCustomer impCust in impCusts)
                    {
                        Console.WriteLine(count + " : ");
                        if (!impCust.IsValid)
                        {
                            impCust.Email = "admin" + count.ToString() + "@healthychefcreations.com";
                        }
                        else if (impCust.Email.Contains("info@healthychef") || impCust.Email.Contains("thehealthyassistant@earthlink"))
                        {
                            impCust.Email = "admin" + count.ToString() + "@healthychefcreations.com";
                        }

                        Console.WriteLine(impCust.Email);

                        if (impCust.IsValid)
                        {
                            count++;

                            string userName = impCust.Email.Trim().Split('@')[0] + DateTime.Now.ToString("yyyyMMddHHmmtt");
                            string password = OrderNumberGenerator.GenerateOrderNumber("?#?#?#?#");
                            string aspUserName = Membership.GetUserNameByEmail(impCust.Email.Trim());
                            MembershipUser newUser = null;

                            if (!string.IsNullOrWhiteSpace(aspUserName))
                                newUser = Membership.GetUser(aspUserName);

                            MembershipCreateStatus createResult = MembershipCreateStatus.UserRejected;

                            if (newUser == null)
                            {
                                newUser = Membership.CreateUser(userName, password, impCust.Email.Trim(), "import", "import", true, out createResult);

                                if (newUser != null)
                                    Console.WriteLine(newUser.UserName + "New user.");
                            }
                            else
                            {
                                Console.WriteLine(newUser.UserName + " Existing user.");
                                createResult = MembershipCreateStatus.Success;
                            }

                            if (newUser != null)
                            {
                                if (createResult == MembershipCreateStatus.Success)
                                {
                                    //Assign Customer role to newUser
                                    try
                                    {
                                        if (!Roles.IsUserInRole(newUser.UserName, "Customer"))
                                        {
                                            Roles.AddUserToRole(newUser.UserName, "Customer");
                                            Console.WriteLine(newUser.UserName + " Role assigned.");
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine(newUser.UserName + " =Assign role failed." + ex.Message + ex.StackTrace);
                                    }
                                    //Send E-mail notification to account user
                                    //HealthyChef.Email.EmailController ec = new HealthyChef.Email.EmailController();
                                    //ec.SendMail_NewUserConfirmation(email, password);

                                    //Create a Healthy Chef profile for this new user
                                    hccUserProfile newProfile = hccUserProfile.GetBy((Guid)newUser.ProviderUserKey).SingleOrDefault(a => !a.ParentProfileID.HasValue);

                                    if (newProfile == null)
                                    {
                                        try
                                        {
                                            newProfile = new hccUserProfile
                                            {
                                                MembershipID = (Guid)newUser.ProviderUserKey,
                                                CreatedBy = (Membership.GetUser() == null ? Guid.Empty : (Guid)Membership.GetUser().ProviderUserKey),
                                                CreatedDate = DateTime.Now,
                                                AccountBalance = 0.00m,
                                                IsActive = true,
                                                FirstName = impCust.FirstName.Trim(),
                                                LastName = impCust.LastName.Trim(),
                                                ProfileName = impCust.FirstName.Trim()
                                            };

                                            //Save all hccProfile information
                                            using (var cont = new healthychefEntitiesAPI())
                                            {
                                                System.Data.EntityKey key = cont.CreateEntityKey("hccUserProfiles", newProfile);
                                                object oldObj;

                                                if (cont.TryGetObjectByKey(key, out oldObj))
                                                {
                                                    cont.ApplyCurrentValues("hccUserProfiles", newProfile);
                                                }
                                                else
                                                {
                                                    cont.hccUserProfiles.AddObject(newProfile);
                                                }

                                                cont.SaveChanges();
                                            }
                                            //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, newProfile);
                                            Console.WriteLine(newUser.UserName + " New profile.");
                                        }
                                        catch (Exception)
                                        {
                                            Console.WriteLine("=" + newUser.UserName + " Save Profile failed.");
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine(newUser.UserName + " Existing profile.");
                                        createResult = MembershipCreateStatus.Success;
                                    }

                                    if (newProfile != null && newProfile.UserProfileID > 0)
                                    {
                                        if (impCust.OtherDeliveryInfo != null && !string.IsNullOrWhiteSpace(impCust.OtherDeliveryInfo))
                                        {
                                            hccUserProfileNote shipNote = new hccUserProfileNote
                                            {
                                                DateCreated = DateTime.Now,
                                                DisplayToUser = false,
                                                UserProfileID = newProfile.UserProfileID,
                                                IsActive = true,
                                                Note = impCust.OtherDeliveryInfo,
                                                NoteTypeID = (int)Enums.UserProfileNoteTypes.ShippingNote
                                            };

                                            using (var cont = new healthychefEntitiesAPI())
                                            {
                                                EntityKey key = cont.CreateEntityKey("hccUserProfileNotes", shipNote);
                                                object originalItem = null;

                                                if (cont.TryGetObjectByKey(key, out originalItem))
                                                {
                                                    cont.ApplyCurrentValues(key.EntitySetName, shipNote);
                                                }
                                                else
                                                {
                                                    cont.hccUserProfileNotes.AddObject(shipNote);
                                                }

                                                cont.SaveChanges();
                                            }
                                        }

                                        if (impCust.HowDidYouHear != null && !string.IsNullOrWhiteSpace(impCust.HowDidYouHear))
                                        {
                                            hccUserProfileNote hearNote = new hccUserProfileNote
                                            {
                                                DateCreated = DateTime.Now,
                                                DisplayToUser = false,
                                                UserProfileID = newProfile.UserProfileID,
                                                IsActive = true,
                                                Note = impCust.HowDidYouHear,
                                                NoteTypeID = (int)Enums.UserProfileNoteTypes.GeneralNote
                                            };

                                            using (var cont = new healthychefEntitiesAPI())
                                            {
                                                EntityKey key = cont.CreateEntityKey("hccUserProfileNotes", hearNote);
                                                object originalItem = null;

                                                if (cont.TryGetObjectByKey(key, out originalItem))
                                                {
                                                    cont.ApplyCurrentValues(key.EntitySetName, hearNote);
                                                }
                                                else
                                                {
                                                    cont.hccUserProfileNotes.AddObject(hearNote);
                                                }

                                                cont.SaveChanges();
                                            }
                                        }

                                        try
                                        {
                                            //save Shipping Address
                                            hccAddress shipAddr = null;

                                            if (newProfile.ShippingAddressID.HasValue)
                                                shipAddr = hccAddress.GetById(newProfile.ShippingAddressID.Value);

                                            if (shipAddr != null)
                                            {
                                                try
                                                {
                                                    newProfile.ShippingAddressID = null;
                                                    //Save all hccProfile information
                                                    using (var cont = new healthychefEntitiesAPI())
                                                    {
                                                        System.Data.EntityKey key1 = cont.CreateEntityKey("hccUserProfiles", newProfile);
                                                        object oldObj1;

                                                        if (cont.TryGetObjectByKey(key1, out oldObj1))
                                                        {
                                                            cont.ApplyCurrentValues("hccUserProfiles", newProfile);
                                                        }
                                                        else
                                                        {
                                                            cont.hccUserProfiles.AddObject(newProfile);
                                                        }
                                                        cont.SaveChanges();
                                                    }
                                                    //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, newProfile);
                                                    using (var cont = new healthychefEntitiesAPI())
                                                    {
                                                        EntityKey key = cont.CreateEntityKey("hccAddresses", shipAddr);
                                                        object originalItem = null;

                                                        if (cont.TryGetObjectByKey(key, out originalItem))
                                                        {
                                                            cont.AttachTo(shipAddr.EntityKey.EntitySetName, shipAddr);
                                                            cont.DeleteObject(shipAddr);
                                                        }
                                                        cont.SaveChanges();
                                                    }

                                                    shipAddr = null;
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(newUser.UserName + " =Delete old shipping address failed." + ex.Message + ex.StackTrace);
                                                }
                                            }

                                            if (shipAddr == null)
                                                shipAddr = new hccAddress();

                                            if (impCust.ShippingAddress1 != null)
                                                shipAddr.Address1 = (string.IsNullOrWhiteSpace(impCust.ShippingAddress1) ? "" : impCust.ShippingAddress1.Trim());
                                            else
                                                shipAddr.Address1 = "";

                                            if (impCust.ShippingAddress2 != null)
                                                shipAddr.Address2 = (string.IsNullOrWhiteSpace(impCust.ShippingAddress2) ? "" : impCust.ShippingAddress2.Trim());
                                            else
                                                shipAddr.Address2 = "";

                                            if (impCust.ShippingAddress3 != null)
                                                shipAddr.Address2 += " " + (string.IsNullOrWhiteSpace(impCust.ShippingAddress3) ? "" : impCust.ShippingAddress3.Trim());

                                            shipAddr.AddressTypeID = (int)Enums.AddressType.Shipping;

                                            if (impCust.ShippingCity != null)
                                                shipAddr.City = (string.IsNullOrWhiteSpace(impCust.ShippingCity) ? "" : impCust.ShippingCity.Trim());
                                            else
                                                shipAddr.City = "";

                                            shipAddr.Country = "US";

                                            if (impCust.FirstName != null)
                                                shipAddr.FirstName = (string.IsNullOrWhiteSpace(impCust.FirstName) ? "" : impCust.FirstName.Trim());
                                            else
                                                shipAddr.FirstName = "";

                                            shipAddr.IsBusiness = false;

                                            if (impCust.LastName != null)
                                                shipAddr.LastName = (string.IsNullOrWhiteSpace(impCust.LastName) ? "" : impCust.LastName.Trim());
                                            else
                                                shipAddr.LastName = "";

                                            if (impCust.Phone1 != null)
                                                shipAddr.Phone = (string.IsNullOrWhiteSpace(impCust.Phone1) ? "" : impCust.Phone1.Trim());
                                            else
                                                shipAddr.Phone = "";

                                            if (impCust.Phone1Ext != null)
                                                shipAddr.Phone += (string.IsNullOrWhiteSpace(impCust.Phone1Ext.Trim()) ? "" : " x" + impCust.Phone1Ext.Trim());
                                            else
                                                shipAddr.Phone = "";

                                            if (impCust.ShippingZipCode != null)
                                                shipAddr.PostalCode = (string.IsNullOrWhiteSpace(impCust.ShippingZipCode) ? "" : impCust.ShippingZipCode.Trim());
                                            else
                                                shipAddr.PostalCode = "";

                                            if (impCust.ShippingState != null)
                                                shipAddr.State = (string.IsNullOrWhiteSpace(impCust.ShippingState) ? "" : impCust.ShippingState.Trim());
                                            else
                                                shipAddr.State = "";

                                            if (impCust.ShipMethod == null)
                                                shipAddr.DefaultShippingTypeID = (int)Enums.DeliveryTypes.Delivery;
                                            else if (impCust.ShipMethod.Trim() == "F")
                                                shipAddr.DefaultShippingTypeID = (int)Enums.DeliveryTypes.Delivery;
                                            else if (impCust.ShipMethod.Trim() == "P")
                                                shipAddr.DefaultShippingTypeID = (int)Enums.DeliveryTypes.LocalPickUp;
                                            else if (impCust.ShipMethod.Trim() == "D")
                                                shipAddr.DefaultShippingTypeID = (int)Enums.DeliveryTypes.LocalDelivery;
                                            else
                                                shipAddr.DefaultShippingTypeID = (int)Enums.DeliveryTypes.Delivery;

                                            if (shipAddr != null)
                                            {
                                                try
                                                {
                                                    using (var cont = new healthychefEntitiesAPI())
                                                    {
                                                        EntityKey key = cont.CreateEntityKey("hccAddresses", shipAddr);
                                                        object originalItem = null;

                                                        if (cont.TryGetObjectByKey(key, out originalItem))
                                                        {
                                                            cont.hccAddresses.ApplyCurrentValues((hccAddress)originalItem);
                                                        }
                                                        else
                                                        {
                                                            cont.hccAddresses.AddObject(shipAddr);
                                                        }

                                                        cont.SaveChanges();
                                                    }
                                                    //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, shipAddr);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Console.WriteLine(newUser.UserName + " =Shipping address save failed." + ex.Message + ex.StackTrace);
                                                }
                                            }

                                            if (shipAddr != null && shipAddr.AddressID > 0)
                                                newProfile.ShippingAddressID = shipAddr.AddressID;
                                            else
                                                newProfile.ShippingAddressID = null;

                                            using (var cont = new healthychefEntitiesAPI())
                                            {
                                                System.Data.EntityKey upkey = cont.CreateEntityKey("hccUserProfiles", newProfile);
                                                object oldObj;

                                                if (cont.TryGetObjectByKey(upkey, out oldObj))
                                                {
                                                    cont.ApplyCurrentValues("hccUserProfiles", newProfile);
                                                }
                                                else
                                                {
                                                    cont.hccUserProfiles.AddObject(newProfile);
                                                }

                                                cont.SaveChanges();
                                            }

                                            Console.WriteLine(newUser.UserName + " Shipping address saved.");
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(newUser.UserName + " =Shipping address not created." + ex.Message + ex.StackTrace);
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("User Profile for user: " + newUser.UserName + " ID not created.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("=New user for user: " + newUser.UserName + " not created.");
                                }
                            }
                            else
                            {
                                Console.WriteLine(createResult.ToString() + " : " + impCust.Email);
                            }
                        }
                        else
                        { count++; Console.WriteLine("=Customer: " + impCust.FirstName + " " + impCust.LastName + " has no email address."); }
                    }
                }
                catch (Exception ex) { Console.WriteLine("=" + ex.Message + " : " + ex.StackTrace); }
            }
        }

        protected static List<ImportedCustomer> GetAll()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.ImportedCustomers.OrderBy(a => a.Email).ToList();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

        //protected static Enums.CreditCardType ValidateCardNumber(string sCardNumber)
        //{
        //    string cardNum = sCardNumber.Replace(" ", "");

        //    Enums.CreditCardType retVal = Enums.CreditCardType.Unknown;

        //    //validate the type of card is accepted
        //    if (cardNum.StartsWith("4") == true &&
        //        (cardNum.Length == 13
        //            || cardNum.Length == 16))
        //    {
        //        //VISA
        //        retVal = Enums.CreditCardType.Visa;
        //    }
        //    else if ((cardNum.StartsWith("51") == true ||
        //              cardNum.StartsWith("52") == true ||
        //              cardNum.StartsWith("53") == true ||
        //              cardNum.StartsWith("54") == true ||
        //              cardNum.StartsWith("55") == true) &&
        //             cardNum.Length == 16)
        //    {
        //        //MasterCard
        //        retVal = Enums.CreditCardType.MasterCard;
        //    }
        //    else if ((cardNum.StartsWith("34") == true ||
        //              cardNum.StartsWith("37") == true) &&
        //             cardNum.Length == 15)
        //    {
        //        //Amex
        //        retVal = Enums.CreditCardType.AmericanExpress;
        //    }
        //    //else if ((cardNum.StartsWith("300") == true ||
        //    //          cardNum.StartsWith("301") == true ||
        //    //          cardNum.StartsWith("302") == true ||
        //    //          cardNum.StartsWith("304") == true ||
        //    //          cardNum.StartsWith("305") == true ||
        //    //          cardNum.StartsWith("36") == true ||
        //    //          cardNum.StartsWith("38") == true) &&
        //    //         cardNum.Length == 14)
        //    //{
        //    //    //Diners Club/Carte Blanche
        //    //    retVal = Enums.CreditCardType.DinersClub;
        //    //}
        //    else if (cardNum.StartsWith("6011") == true &&
        //             cardNum.Length == 16)
        //    {
        //        //Discover
        //        retVal = Enums.CreditCardType.Discover;
        //    }

        //    if (retVal != Enums.CreditCardType.Unknown)
        //    {
        //        int[] DELTAS = new int[] { 0, 1, 2, 3, 4, -4, -3, -2, -1, 0 };
        //        int checksum = 0;
        //        char[] chars = cardNum.ToCharArray();
        //        for (int i = chars.Length - 1; i > -1; i--)
        //        {
        //            int j = ((int)chars[i]) - 48;
        //            checksum += j;
        //            if (((i - chars.Length) % 2) == 0)
        //                checksum += DELTAS[j];
        //        }

        //        if ((checksum % 10) != 0)
        //            retVal = Enums.CreditCardType.Unknown;
        //    }

        //    return retVal;
        //}

    }
}

//try
//{
//    //save Billing Address
//    hccAddress billAddr;

//    if (newProfile.BillingAddressID.HasValue)
//        billAddr = hccAddress.GetById(newProfile.BillingAddressID.Value);
//    else
//        billAddr = new hccAddress();

//    billAddr.Address1 = (string.IsNullOrWhiteSpace(impCust.BillingAddress1) ? "" : impCust.BillingAddress1.Trim());
//    billAddr.Address2 = (string.IsNullOrWhiteSpace(impCust.BillingAddress2) ? "" : impCust.BillingAddress2.Trim()) +
//        " " + (string.IsNullOrWhiteSpace(impCust.BillingAddress3) ? "" : impCust.BillingAddress3.Trim());
//    billAddr.AddressTypeID = (int)Enums.AddressType.Billing;
//    billAddr.City = (string.IsNullOrWhiteSpace(impCust.BillingCity) ? "" : impCust.BillingCity.Trim());
//    billAddr.Country = "US";
//    billAddr.FirstName = (string.IsNullOrWhiteSpace(impCust.FirstName) ? "" : impCust.FirstName.Trim());
//    billAddr.IsBusiness = false;
//    billAddr.LastName = (string.IsNullOrWhiteSpace(impCust.LastName) ? "" : impCust.LastName.Trim());
//    billAddr.Phone = (string.IsNullOrWhiteSpace(impCust.Phone1) ? "" : impCust.Phone1.Trim());

//    if (impCust.Phone2Ext != null)
//        billAddr.Phone += (string.IsNullOrWhiteSpace(impCust.Phone2Ext.Trim()) ? "" : " x" + impCust.Phone2Ext.Trim());

//    billAddr.PostalCode = (string.IsNullOrWhiteSpace(impCust.BillingZipCode) ? "" : impCust.BillingZipCode.Trim());
//    billAddr.State = (string.IsNullOrWhiteSpace(impCust.BillingState) ? "" : impCust.BillingState.Trim());
//    billAddr.Save();

//    newProfile.BillingAddressID = billAddr.AddressID;
//    newProfile.Save();
//}
//catch (Exception ex)
//{
//    Console.WriteLine("Billing Address for user: " + newUser.UserName + " could not be created." + ex.Message + ex.StackTrace);
//}
//Credit Card   
//hccUserProfilePaymentProfile newPaymentProfile = null;
//CardInfo card = null;
////Save CardInfo

//if (string.IsNullOrWhiteSpace(impCust.CCAcctNo))
//{
//    Console.WriteLine("Credit card information incomplete for user: " + newUser.UserName + ". No Card Number. Card record could not be created.");
//}
//else if (string.IsNullOrWhiteSpace(impCust.CCExpMonth))
//{
//    Console.WriteLine("Credit card information incomplete for user: " + newUser.UserName + ". No ExpMonth. Card record could not be created.");
//}
//else if (string.IsNullOrWhiteSpace(impCust.CCExpYear))
//{
//    Console.WriteLine("Credit card information incomplete for user: " + newUser.UserName + ". No ExpYear. Card record could not be created.");
//}
//else if (string.IsNullOrWhiteSpace(impCust.CVV))
//{
//    Console.WriteLine("Credit card information incomplete for user: " + newUser.UserName + ". No CVV. Card record could not be created.");
//}
//else
//{
//    try
//    {
//        card = new CardInfo
//        {
//            NameOnCard = newProfile.FirstName + " " + newProfile.LastName,
//            CardNumber = impCust.CCAcctNo.Trim(),
//            ExpMonth = int.Parse(impCust.CCExpMonth.Trim()),
//            ExpYear = int.Parse(impCust.CCExpYear.Trim()),
//            SecurityCode = impCust.CVV.Trim()
//        };
//    }
//    catch (Exception)
//    {
//        Console.WriteLine("Credit card information conversion error for user: " + newUser.UserName + " : "
//            + impCust.CCExpMonth + " : " + impCust.CCExpYear + ". Card record could not be created.");
//    }
//}

//if (card != null && card.HasValues)
//{
//    card.CardType = ValidateCardNumber(card.CardNumber);

//    if (card.CardType != Enums.CreditCardType.Unknown)
//    {
//        newPaymentProfile = new hccUserProfilePaymentProfile
//        {
//            CardTypeID = (int)card.CardType,
//            CCLast4 = card.CardNumber.Substring(card.CardNumber.Length - 4, 4),
//            ExpMon = card.ExpMonth,
//            ExpYear = card.ExpYear,
//            NameOnCard = card.NameOnCard,
//            UserProfileID = newProfile.UserProfileID,
//            IsActive = true
//        };
//    }
//    else
//    {
//        Console.WriteLine("Credit Card type for user: " + newUser.UserName + " unknown. Card not saved.");
//    }

//    try
//    {
//        //send card to Auth.net for Auth.net profile
//        CustomerInformationManager cim = new CustomerInformationManager();
//        Customer cust = null;
//        string autnetResult = string.Empty;

//        if (!string.IsNullOrWhiteSpace(newProfile.AuthNetProfileID))
//            cust = cim.GetCustomer(newProfile.AuthNetProfileID);

//        if (cust == null)
//            cust = cim.GetCustomerByEmail(newProfile.ASPUser.Email);

//        if (cust == null)
//            cust = cim.CreateCustomer(newProfile.ASPUser.Email, newProfile.ASPUser.UserName);

//        if (cust != null)
//        {
//            newProfile.AuthNetProfileID = cust.ProfileID;
//            newProfile.Save();

//            List<PaymentProfile> payProfiles = cust.PaymentProfiles.ToList();

//            if (payProfiles.Count == 0)
//            {   // create a new profile
//                autnetResult = cim.AddCreditCard(cust, card.CardNumber, card.ExpMonth,
//                        card.ExpYear, card.SecurityCode, newProfile.hccAddressBilling.ToAuthNetAddress());

//                if (!string.IsNullOrWhiteSpace(autnetResult))
//                {
//                    newPaymentProfile.AuthNetPaymentProfileID = autnetResult;
//                    newPaymentProfile.Save();
//                }
//            }
//            else
//            {
//                payProfiles.ForEach(a => cim.DeletePaymentProfile(newProfile.AuthNetProfileID, a.ProfileID));

//                string savePayProfileResult = cim.AddCreditCard(cust, card.CardNumber, card.ExpMonth,
//                    card.ExpYear, card.SecurityCode, newProfile.hccAddressBilling.ToAuthNetAddress());

//                newPaymentProfile.AuthNetPaymentProfileID = savePayProfileResult;
//                newPaymentProfile.Save();
//            }
//        }
//        else
//        {
//            Console.WriteLine("Authorize.net card profile could not be created for user: " + newUser.UserName + ".");
//        }
//    }
//    catch (Exception ex) { Console.WriteLine(ex.Message); }
//}
//else
//{
//    Console.WriteLine("Credit Card for user: " + newUser.UserName + " does not has required values. Card record not created.");
//}