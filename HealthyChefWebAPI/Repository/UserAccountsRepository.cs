using HealthyChefWebAPI.CustomModels;
using HealthyChefWebAPI.Helpers;
using HealthyChefWebAPI.Constants;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using HealthyChef.DAL;
using HealthyChef.DAL.Extensions;
using AuthorizeNet;
using HealthyChef.AuthNet;
using HealthyChef.Common;
using AuthorizeNet.APICore;

namespace HealthyChefWebAPI.Repository
{
    public class UserAccountsRepository
    {

        #region Get UserAccounts
        public static string GetUserAccounts(string lastName, string email, string phone, int? purchaseNumber, DateTime? deliveryDate, string roles)
        {

            try
            {
                List<MembershipUser> retVals = new List<MembershipUser>();
                List<Guid?> profileIDs = new List<Guid?>();

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("hcc_UserProfileSearch", conn))
                    {


                        SqlParameter prm1 = new SqlParameter("@lastName", SqlDbType.NVarChar);
                        prm1.Value = lastName;
                        cmd.Parameters.Add(prm1);
                        SqlParameter prm2 = new SqlParameter("@email", SqlDbType.NVarChar);
                        prm2.Value = email;
                        cmd.Parameters.Add(prm2);
                        SqlParameter prm3 = new SqlParameter("@phone", SqlDbType.NVarChar);
                        prm3.Value = phone;
                        cmd.Parameters.Add(prm3);
                        SqlParameter prm4 = new SqlParameter("@purchNum", SqlDbType.Int);
                        prm4.Value = purchaseNumber;
                        cmd.Parameters.Add(prm4);
                        SqlParameter prm5 = new SqlParameter("@delivDate", SqlDbType.DateTime);
                        prm5.Value = deliveryDate;
                        cmd.Parameters.Add(prm5);
                        SqlParameter prm6 = new SqlParameter("@roles", SqlDbType.NVarChar);
                        prm6.Value = roles;
                        cmd.Parameters.Add(prm6);

                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();
                        SqlDataReader t = cmd.ExecuteReader();

                        if (t != null && t.HasRows)
                        {
                            while (t.Read())
                            {
                                profileIDs.Add(t.GetGuid(0));
                            }

                            t.Close();
                        }
                        conn.Close();
                    }
                }

                profileIDs.ForEach(
                       delegate (Guid? membershipUserId)
                       {
                           if (membershipUserId.HasValue)
                           {
                               MembershipUser user = Membership.GetUser(membershipUserId.Value);

                               if (user != null)
                                   retVals.Add(user);
                           }
                       });

                //return retVals.OrderBy(a => a.Email).ToList();
                retVals = retVals.OrderBy(a => a.Email).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch
            {
                throw;
            }

        }


        public static string GetUserAccountDetails()
        {
            try
            {
                List<UserAccount> retVals = new List<UserAccount>();

                using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(SPs.GETALLUSERS, conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            retVals.Add(new UserAccount()
                            {
                                UserID = Convert.ToString(t["USERID"]),
                                Name = Convert.ToString(t["NAME"]),
                                Email = Convert.ToString(t["EMAIL"]),
                                Role = Convert.ToString(t["ROLENAME"]),
                                IsActive = Convert.ToBoolean(t["ISACTIVE"]),
                                IsApproved = Convert.ToBoolean(t["ISAPPROVED"]),
                                DeliveryDate = DBUtil.GetCharField(t, "DELIVERYDATE"),

                            });
                        }

                        conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.UserID).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception E)
            {
                return string.Empty;
            }

        }

        #endregion

        #region Update User Details

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


        public static PostHttpResponse UpdateStatusOfCustomer(CustomerStatus _customerStatus)
        {
            var _res = new PostHttpResponse();

            try
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(_customerStatus.UserID);
                if (_userToUpdate != null)
                {
                    _userToUpdate.IsActive = _customerStatus.IsActive;
                    _userToUpdate.Save();
                    if (_customerStatus.IsLockedOut)
                    {
                        object providerUserKey = new Guid(_customerStatus.UserID);
                        var _membershipUser = Membership.GetUser(providerUserKey,false);
                        HealthyChef.Common.Helpers.LockUser(_membershipUser);
                    }

                    _res.IsSuccess = true;
                    _res.StatusCode = System.Net.HttpStatusCode.OK;
                    _res.Message = "Successfully Updated User status for user : " + _customerStatus.UserID;
                }
                else
                {
                    _res.Message = "No user found to update with this id " + _customerStatus.UserID;
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in updating user : " + Environment.NewLine + ex.Message;
            }
            return _res;
        }


        public static PostHttpResponse UpdateBasicInfo(CustomerBasicInfo _customerBasicInfo)
        {
            var _res = new PostHttpResponse();

            try
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(_customerBasicInfo.UserID);
                if (_userToUpdate != null)
                {
                    _userToUpdate.FirstName = _customerBasicInfo.FirstName;
                    _userToUpdate.LastName = _customerBasicInfo.LastName;
                    _userToUpdate.ProfileName = _customerBasicInfo.ProfileName;
                    _userToUpdate.CanyonRanchCustomer = _customerBasicInfo.CanyonRanchCustomer;
                    _userToUpdate.DefaultCouponId = _customerBasicInfo.DefaultCouponId;
                    _userToUpdate.IsActive = _customerBasicInfo.IsActive;
                    _userToUpdate.Save();

                    object providerUserKey = new Guid(_customerBasicInfo.UserID);
                    var _membershipUser = Membership.GetUser(providerUserKey, false);
                    if (_customerBasicInfo.IsLockedOut)
                    {
                        HealthyChef.Common.Helpers.LockUser(_membershipUser);
                    }
                    _membershipUser.Email = _customerBasicInfo.Email;
                    foreach (var role in _customerBasicInfo.Roles)
                    {
                        //Roles.AddUserToRole(_membershipUser.UserName, role);
                    }
                    Membership.UpdateUser(_membershipUser);


                    _res.IsSuccess = true;
                    _res.StatusCode = System.Net.HttpStatusCode.OK;
                    _res.Message = "Successfully Updated basic info for user : " + _customerBasicInfo.UserID;
                }
                else
                {
                    _res.Message = "No user found to update with this id " + _customerBasicInfo.UserID;
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in updating user : " + Environment.NewLine + ex.Message;
            }

            return _res;
        }


        public static PostHttpResponse UpdateShippingAddress(CustomerShippingAddress _customerShippingInfo)
        {
            var _res = new PostHttpResponse();

            try
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(_customerShippingInfo.UserID);
                if (_userToUpdate != null)
                {
                    var CurrentAddress = hccAddress.GetById(_customerShippingInfo.ShippingAddressID);
                    hccAddress address;

                    if (CurrentAddress == null)
                    {
                        address = new hccAddress { Country = "US", AddressTypeID = 4 };
                    }
                    else
                    {
                        address = CurrentAddress;
                    }

                    int addrId = address.AddressID;
                    address.FirstName = _customerShippingInfo.FirstName;
                    address.LastName = _customerShippingInfo.LastName;
                    address.Address1 = _customerShippingInfo.Address1;
                    address.Address2 = _customerShippingInfo.Address2;
                    address.City = _customerShippingInfo.City;
                    address.State = _customerShippingInfo.State;
                    address.PostalCode = _customerShippingInfo.PostalCode;
                    address.Phone = _customerShippingInfo.Phone;
                    address.IsBusiness = _customerShippingInfo.IsBusiness;
                    address.DefaultShippingTypeID = _customerShippingInfo.DefaultShippingTypeID;

                    if (_customerShippingInfo.DefaultShippingTypeID == 2)
                    {
                        string ZipCode = _customerShippingInfo.PostalCode;
                        hccShippingZone hccshopin = new hccShippingZone();
                        DataSet ds = hccshopin.BindZoneByZipCodeNew(ZipCode);
                        int ZoneId = Convert.ToInt32(ds.Tables[0].Rows[0]["ZoneID"].ToString());
                        DataTable ds1 = hccshopin.BindGridShippingZoneByZoneID(ZoneId);
                        string IsPickup = ds1.Rows[0]["IsPickupShippingZone"].ToString();

                        if (IsPickup == "True")
                        {
                            address.Save();
                            _res.IsSuccess = true;
                            _res.StatusCode = System.Net.HttpStatusCode.OK;
                            _res.Message = "Customer shipping address is updated successfully";
                        }
                        else
                        {
                            _res.Message = "Customer pickup is not available at this Zip Code";
                        }
                    }
                    else
                    {
                        address.Save();
                        _res.IsSuccess = true;
                        _res.StatusCode = System.Net.HttpStatusCode.OK;
                        _res.Message = "Customer shipping address is updated successfully";
                    }

                }
                else
                {
                    _res.Message = "No user found to update with this id " + _customerShippingInfo.UserID;
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in updating user : " + Environment.NewLine + ex.Message;
            }

            return _res;
        }

        public static PostHttpResponse UpdateBillingInfo(CustomerBillingInfo _customerBillingInfo)
        {
            var _res = new PostHttpResponse();

            try
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(_customerBillingInfo.UserID);
                if (_userToUpdate != null)
                {
                    var CurrentAddress = hccAddress.GetById(_customerBillingInfo.BillingAddressID);
                    hccAddress address;

                    if (CurrentAddress == null)
                    {
                        address = new hccAddress { Country = "US", AddressTypeID = 2 };
                    }
                    else
                    {
                        address = CurrentAddress;
                    }

                    int addrId = address.AddressID;
                    address.FirstName = _customerBillingInfo.FirstName;
                    address.LastName = _customerBillingInfo.LastName;
                    address.Address1 = _customerBillingInfo.Address1;
                    address.Address2 = _customerBillingInfo.Address2;
                    address.City = _customerBillingInfo.City;
                    address.State = _customerBillingInfo.State;
                    address.PostalCode = _customerBillingInfo.PostalCode;
                    address.Phone = _customerBillingInfo.Phone;

                    address.Save();

                    if(_customerBillingInfo.UpdateCreditCardInfo)
                    {
                        CardInfo CurrentCardInfo = new CardInfo();

                        CurrentCardInfo.NameOnCard = _customerBillingInfo.NameOnCard;
                        CurrentCardInfo.CardNumber = _customerBillingInfo.CardNumber;
                        CurrentCardInfo.CardType = _customerBillingInfo.CardType;
                        CurrentCardInfo.ExpMonth = _customerBillingInfo.ExipiresOnMonth;
                        CurrentCardInfo.ExpYear = _customerBillingInfo.ExipiresOnYear;
                        CurrentCardInfo.SecurityCode = _customerBillingInfo.CardIdCode;

                        Address billAddr = null;
                        if (_customerBillingInfo.BillingAddressID != 0)
                        {
                            billAddr = hccAddress.GetById(_customerBillingInfo.BillingAddressID).ToAuthNetAddress();
                        }
                        if (CurrentCardInfo.HasValues && billAddr != null)
                        {
                            try
                            {
                                //send card to Auth.net for Auth.net profile
                                CustomerInformationManager cim = new CustomerInformationManager();
                                Customer cust = null;
                                string autnetResult = string.Empty;

                                if (!string.IsNullOrWhiteSpace(_userToUpdate.AuthNetProfileID))
                                    cust = cim.GetCustomer(_userToUpdate.AuthNetProfileID);

                                //Will Martinez - Commented out on 7/30/2013.
                                //This code scans all existing Profiles generated to check for duplicated email addresses, however the site registration prevents that
                                //commented out since this process had a significant performance effect on the site.
                                //if (cust == null)
                                //    cust = cim.GetCustomerByEmail(userProfile.ASPUser.Email);

                                if (cust == null)
                                    cust = cim.CreateCustomer(_userToUpdate.ASPUser.Email, _userToUpdate.ASPUser.UserName);

                                // had to add it back in, unable to create records with duplicate email addresses caused by IT desynching data.
                                // this should only be called infrequently since we try to create the account first.
                                if (cust.ProfileID == null)
                                    cust = cim.GetCustomerByEmail(_userToUpdate.ASPUser.Email, out autnetResult);
                                if (cust != null)
                                {
                                    if (_userToUpdate.AuthNetProfileID != cust.ProfileID)
                                    {
                                        _userToUpdate.AuthNetProfileID = cust.ProfileID;
                                        _userToUpdate.Save();
                                    }

                                    List<PaymentProfile> payProfiles = cust.PaymentProfiles.ToList();

                                    if (payProfiles.Count > 0)
                                        payProfiles.ForEach(a => cim.DeletePaymentProfile(_userToUpdate.AuthNetProfileID, a.ProfileID));

                                    // create new payment profile
                                    autnetResult = cim.AddCreditCard(cust, CurrentCardInfo.CardNumber, CurrentCardInfo.ExpMonth,
                                         CurrentCardInfo.ExpYear, CurrentCardInfo.SecurityCode, billAddr);

                                    if (!string.IsNullOrWhiteSpace(autnetResult))
                                    {
                                        // Validate card profile
                                        validateCustomerPaymentProfileResponse valProfile = cim.ValidateProfile(_userToUpdate.AuthNetProfileID,
                                            autnetResult, AuthorizeNet.ValidationMode.TestMode);

                                        if (valProfile.messages.resultCode == messageTypeEnum.Ok)
                                        {
                                            hccUserProfilePaymentProfile activePaymentProfile = null;
                                            activePaymentProfile = _userToUpdate.ActivePaymentProfile;

                                            if (_userToUpdate.ActivePaymentProfile == null)
                                                activePaymentProfile = new hccUserProfilePaymentProfile();

                                            activePaymentProfile.CardTypeID = (int)CurrentCardInfo.CardType;
                                            activePaymentProfile.CCLast4 = CurrentCardInfo.CardNumber.Substring(CurrentCardInfo.CardNumber.Length - 4, 4);
                                            activePaymentProfile.ExpMon = CurrentCardInfo.ExpMonth;
                                            activePaymentProfile.ExpYear = CurrentCardInfo.ExpYear;
                                            activePaymentProfile.NameOnCard = CurrentCardInfo.NameOnCard;
                                            activePaymentProfile.UserProfileID = _userToUpdate.UserProfileID;
                                            activePaymentProfile.IsActive = true;

                                            activePaymentProfile.AuthNetPaymentProfileID = autnetResult;
                                            activePaymentProfile.Save();

                                            _res.IsSuccess = true;
                                            _res.StatusCode = System.Net.HttpStatusCode.OK;
                                            _res.Message = "Billing address and credit card details are updated successfully for " + _customerBillingInfo.UserID;
                                        }
                                        else
                                        {
                                            _res.Message = "Payment Profile has been created, but validation failed.";
                                        }
                                    }
                                    else
                                    {
                                        _res.Message = "Authorize.Net response is empty.";
                                    }
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(autnetResult))
                                        _res.Message = autnetResult;
                                }
                            }
                            catch(Exception ex)
                            {
                                _res.Message = "Error in updating credit card details " + ex.Message;
                            }
                        }
                    }
                    else
                    {
                        _res.IsSuccess = true;
                        _res.StatusCode = System.Net.HttpStatusCode.OK;
                        _res.Message = "Billing address updated for " + _customerBillingInfo.UserID;
                    }
                }
                else
                {
                    _res.Message = "No user found to update with this id " + _customerBillingInfo.UserID;
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in updating user : " + Environment.NewLine + ex.Message;
            }

            return _res;
        }

        public static PostHttpResponse AddOrUpdateNotesForUser(CustomerNote _customerNote)
        {
            var _res = new PostHttpResponse();
            bool isupdate = false;
            try
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(_customerNote.UserID);
                if (_userToUpdate != null)
                {
                    hccUserProfileNote note = hccUserProfileNote.GetById(_customerNote.NoteId);
                    if (note == null)
                    {
                        note = new hccUserProfileNote
                        {
                            DateCreated = DateTime.Now,
                            UserProfileID = _userToUpdate.UserProfileID,
                            NoteTypeID = _customerNote.NotetypeId,
                            IsActive = true
                        };
                    }
                    else
                    {
                        isupdate = true;
                    }

                    note.DisplayToUser = _customerNote.DisplayToUser;
                    note.Note = _customerNote.Note;
                    note.Save();

                    _res.IsSuccess = true;
                    _res.StatusCode = System.Net.HttpStatusCode.OK;
                    _res.Message = isupdate ? "Successfully updated user notes with Id " + note.NoteID : "Successfully created new user notes with Id " + note.NoteID;
                }
                else
                {
                    _res.Message = "No user found to update with this id " + _customerNote.UserID;
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in updating user notes : " + Environment.NewLine + ex.Message;
            }

            return _res;
        }

        public static PostHttpResponse AddOrUpdatePreferenceForUser(CustomerPreferencesToUpdate _customerPrefs)
        {
            var _res = new PostHttpResponse();
            try
            {
                hccUserProfile _userToUpdate = GetUserProfileByID(_customerPrefs.UserID);
                if (_userToUpdate != null)
                {
                    if(_customerPrefs.Preferences.Length != 0)
                    {
                        List<hccUserProfilePreference> existingUserPrefs = hccUserProfilePreference.GetBy(_userToUpdate.UserProfileID, true);
                        existingUserPrefs.ForEach(delegate (hccUserProfilePreference userPref) { userPref.Delete(); });

                        foreach (var p in _customerPrefs.Preferences)
                        {
                            hccUserProfilePreference newPref = new hccUserProfilePreference
                            {
                                PreferenceID = p,
                                UserProfileID = _userToUpdate.UserProfileID,
                                IsActive = true
                            };
                            newPref.Save();
                        }

                        _res.IsSuccess = true;
                        _res.StatusCode = System.Net.HttpStatusCode.OK;
                        _res.Message = "Successfully updated user preferences for user " + _customerPrefs.UserID;
                    }
                    else
                    {
                        _res.Message = "No preferences found for the user " + _customerPrefs.UserID;
                    }
                }
                else
                {
                    _res.Message = "No user found to update with this id " + _customerPrefs.UserID;
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in updating user notes : " + Environment.NewLine + ex.Message;
            }

            return _res;
        }

        #endregion

    }

}