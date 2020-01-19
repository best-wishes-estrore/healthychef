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
using System.Text;

namespace HealthyChefWebAPI.Repository
{
    public class UserAccountsRepository
    {
        public static Guid CurrentLoggedUserId { get; set; }

        #region Get UserAccounts
        public static string SearchGetUserAccounts(SearchParams searchParameters)
        {
            List<UserAccount> retVals = new List<UserAccount>();
            List<Guid?> profileIDs = new List<Guid?>();
            CurrentLoggedUserId = searchParameters.CurrentLoggedUserId;
            int totalrecord = 0;
            try
            {

                //using (SqlConnection conn = new SqlConnection(WebConfigurationManager.ConnectionStrings["WebModulesAPI"].ConnectionString))
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("hcc_UserProfileSearch2", conn))
                    {


                        if (!string.IsNullOrEmpty(searchParameters.lastName))
                        {
                            SqlParameter prm1 = new SqlParameter("@LASTNAME", SqlDbType.NVarChar);
                            prm1.Value = searchParameters.lastName;
                            cmd.Parameters.Add(prm1);
                        }

                        if (!string.IsNullOrEmpty(searchParameters.email))
                        {
                            SqlParameter prm2 = new SqlParameter("@EMAIL", SqlDbType.NVarChar);
                            prm2.Value = searchParameters.email;
                            cmd.Parameters.Add(prm2);
                        }

                        if (!string.IsNullOrEmpty(searchParameters.phone))
                        {
                            SqlParameter prm3 = new SqlParameter("@PHONE", SqlDbType.NVarChar);
                            prm3.Value = searchParameters.phone;
                            cmd.Parameters.Add(prm3);
                        }

                        if (searchParameters.purchaseNumber != 0)
                        {
                            SqlParameter prm4 = new SqlParameter("@PURCHNUM", SqlDbType.BigInt);
                            prm4.Value = searchParameters.purchaseNumber;
                            cmd.Parameters.Add(prm4);
                        }

                        if (!string.IsNullOrEmpty(searchParameters.deliveryDate))
                        {
                            SqlParameter prm5 = new SqlParameter("@DELIVDATE", SqlDbType.NVarChar);
                            prm5.Value = searchParameters.deliveryDate;
                            cmd.Parameters.Add(prm5);
                        }

                        if (!string.IsNullOrEmpty(searchParameters.roles))
                        {
                            SqlParameter prm6 = new SqlParameter("@ROLES", SqlDbType.NVarChar);
                            prm6.Value = searchParameters.roles;
                            cmd.Parameters.Add(prm6);
                        }

                        SqlParameter prm7 = new SqlParameter("@PageNumber", SqlDbType.Int);
                        prm7.Value = searchParameters.pagenumber;
                        cmd.Parameters.Add(prm7);

                        SqlParameter prm8 = new SqlParameter("@PageSize", SqlDbType.Int);
                        prm8.Value = searchParameters.pagesize;
                        cmd.Parameters.Add(prm8);

                        SqlParameter prm9 = new SqlParameter("@totalrecord", SqlDbType.Int);
                        prm9.Value = searchParameters.totalrecords;
                        cmd.Parameters.Add(prm9);

                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();
                        SqlDataReader t = cmd.ExecuteReader();

                        if (t != null && t.HasRows)
                        {
                            while (t.Read())
                            {
                                profileIDs.Add(t.GetGuid(0));
                                totalrecord = DBUtil.GetIntField(t, "totalrecords");
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
                             {
                                 RoledUser rolesandnames = new RoledUser(user);
                                 if (rolesandnames == null)
                                 {
                                     rolesandnames.FullName = "";
                                 }
                                 UserAccount userAccount = new UserAccount
                                 {
                                     UserID = new Guid(user.ProviderUserKey.ToString()),
                                     Name = rolesandnames.FullName,
                                     Email = user.Email,
                                     Role = rolesandnames.UserRoles.Remove(rolesandnames.UserRoles.Length - 1),
                                     IsApproved = user.IsApproved,
                                     IsLockedOut = user.IsLockedOut,
                                     IsOnline = user.IsOnline,
                                     Totalrecords = totalrecord
                                 };
                                 retVals.Add(userAccount);
                             }
                         }
                     });
                if (retVals.Count != 0)
                {
                    if (retVals[0].Totalrecords == 0)
                    {
                        retVals[0].Totalrecords = retVals.Count;
                    }
                }
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public class RoledUser
        {
            public RoledUser(MembershipUser user)
            {
                StringBuilder roles = new StringBuilder();
                Roles.GetRolesForUser(user.UserName).ToList().ForEach(a => roles.Append(a + ","));

                ProviderUserKey = user.ProviderUserKey;
                Email = user.Email;
                IsApproved = user.IsApproved;
                IsLockedOut = user.IsLockedOut;
                IsOnline = user.IsOnline;
                UserRoles = roles.ToString();

                if (UserRoles.Contains("Customer"))
                {
                    hccUserProfile profile = hccUserProfile.GetParentProfileBy((Guid)user.ProviderUserKey);

                    //Create basic profile is user does not have one.
                    if (profile == null)
                    {
                        profile = new hccUserProfile
                        {
                            MembershipID = (Guid)user.ProviderUserKey,
                            CreatedBy = CurrentLoggedUserId,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                            AccountBalance = 0.00m,
                            ProfileName = "Main"
                        };
                        profile.Save();
                    }
                    else
                    {
                        this.FullName = profile.FullName;
                    }
                }
            }

            public RoledUser(hccUserProfile profile)
            {
                StringBuilder roles = new StringBuilder();
                Roles.GetRolesForUser(profile.ASPUser.UserName).ToList().ForEach(a => roles.Append(a + ","));

                //ParentUser = profile.ASPUser;
                ProviderUserKey = profile.ASPUser.ProviderUserKey;
                Email = profile.ASPUser.Email;
                IsApproved = profile.ASPUser.IsApproved;
                IsLockedOut = profile.ASPUser.IsLockedOut;
                IsOnline = profile.ASPUser.IsOnline;
                UserRoles = roles.ToString();
            }

            public object ProviderUserKey { get; set; }
            public string FullName { get; set; }
            public string Email { get; set; }
            public bool IsApproved { get; set; }
            public bool IsLockedOut { get; set; }
            public bool IsOnline { get; set; }
            public string UserRoles { get; set; }
        }


        public static string GetUserAccountDetails()
        {
            try
            {
                List<UserAccount> retVals = new List<UserAccount>();
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand(SPs.GETALLUSERS, conn))
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            var _userAccount = new UserAccount()
                            {
                                UserID = DBUtil.GetGuidField(t, "USERID"),
                                Name = DBUtil.GetCharField(t, "NAME"),
                                Email = DBUtil.GetCharField(t, "EMAIL"),
                                Role = DBUtil.GetCharField(t, "ROLENAME"),
                                IsActive = DBUtil.GetBoolField(t, "ISACTIVE"),
                                IsApproved = DBUtil.GetBoolField(t, "ISAPPROVED"),
                                IsLockedOut = DBUtil.GetBoolField(t, "ISLOCKEDOUT"),
                                DeliveryDate = DBUtil.GetCharField(t, "DELIVERYDATE"),
                                Phone = DBUtil.GetCharField(t, "PHONE"),
                            };

                            //logic to find is user online
                            var membershipUser = Membership.GetUser(DBUtil.GetGuidField(t, "USERID"));
                            if (membershipUser != null)
                            {
                                _userAccount.IsOnline = membershipUser.IsOnline;
                            }

                            retVals.Add(_userAccount);
                        }

                        //conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.UserID).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception E)
            {
                return E.Message;
            }

        }

        public static string GetUserAccountDetailsByRole(string roleid)
        {
            try
            {
                List<UserAccount> retVals = new List<UserAccount>();
                using (SqlConnection conn = new SqlConnection(DBUtil.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand(SPs.GETALLUSERS, conn);
                    if (!string.IsNullOrEmpty(roleid))
                    {
                        cmd = new SqlCommand("GETALLUSERSBYROLE", conn);
                        cmd.Parameters.AddWithValue("@roleid", roleid);
                    }

                    using (cmd)
                    {
                        cmd.CommandTimeout = 180;
                        cmd.CommandType = CommandType.StoredProcedure;

                        conn.Open();

                        IDataReader t = cmd.ExecuteReader();
                        while (t.Read())
                        {
                            var _userAccount = new UserAccount()
                            {
                                UserID = DBUtil.GetGuidField(t, "USERID"),
                                Name = DBUtil.GetCharField(t, "NAME"),
                                Email = DBUtil.GetCharField(t, "EMAIL"),
                                Role = DBUtil.GetCharField(t, "ROLENAME"),
                                IsActive = DBUtil.GetBoolField(t, "ISACTIVE"),
                                IsApproved = DBUtil.GetBoolField(t, "ISAPPROVED"),
                                IsLockedOut = DBUtil.GetBoolField(t, "ISLOCKEDOUT"),
                                DeliveryDate = DBUtil.GetCharField(t, "DELIVERYDATE"),
                                Phone = DBUtil.GetCharField(t, "PHONE"),
                            };

                            //logic to find is user online
                            var membershipUser = Membership.GetUser(DBUtil.GetGuidField(t, "USERID"));
                            if (membershipUser != null)
                            {
                                _userAccount.IsOnline = membershipUser.IsOnline;
                            }

                            retVals.Add(_userAccount);
                        }

                        //conn.Close();
                    }
                }

                retVals = retVals.OrderBy(a => a.UserID).ToList();
                return DBHelper.ConvertDataToJson(retVals);
            }
            catch (Exception E)
            {
                return E.Message;
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
        public static hccUserProfile GetUserProfileByProfileId(int _userID)
        {
            hccUserProfile profile = new hccUserProfile();
            try
            {
                if (_userID !=0)
                {
                    profile = hccUserProfile.GetById(_userID);
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
                MembershipUser user = null;

                if (_customerStatus.UserID != null)
                {
                    object providerUserKey = new Guid(_customerStatus.UserID);
                    user = Membership.GetUser(providerUserKey, false);
                }
                if (user != null) // create new profile
                {
                    if (_customerStatus.IsLockedOut)
                    {
                        HealthyChef.Common.Helpers.LockUser(user);
                    }
                    //HealthyChef.Common.Helpers.LockUser(user);
                    else
                    {
                        if (user.IsLockedOut)
                            user.UnlockUser();

                        if (!user.IsApproved)
                        {
                            user.IsApproved = true;
                            _customerStatus.IsActive = true;
                        }
                    }

                    user.IsApproved = _customerStatus.IsActive;
                    Membership.UpdateUser(user);

                    _res.IsSuccess = true;
                    _res.StatusCode = System.Net.HttpStatusCode.OK;
                    _res.Message = "Successfully Updated User status for user : " + _customerStatus.UserID;
                }
                else
                    _res.Message = "No user found to update with this id " + _customerStatus.UserID;
                //hccUserProfile _userToUpdate = GetUserProfileByID(_customerStatus.UserID);
                //if (_userToUpdate != null)
                //{
                //    userToUpdate.IsActive = customerStatus.IsActive;
                //    _userToUpdate.Save();
                //    if (_customerStatus.IsLockedOut)
                //    {
                //        object providerUserKey = new Guid(_customerStatus.UserID);
                //        var _membershipUser = Membership.GetUser(providerUserKey, false);
                //        HealthyChef.Common.Helpers.LockUser(_membershipUser);
                //    }

                //    _res.IsSuccess = true;
                //    _res.StatusCode = System.Net.HttpStatusCode.OK;
                //    res.Message = "Successfully Updated User status for user : " + customerStatus.UserID;
                //}
                //else
                //{
                //    res.Message = "No user found to update with this id " + customerStatus.UserID;
                //}
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
                if (_customerShippingInfo.City != "" && _customerShippingInfo.FirstName != "" && _customerShippingInfo.LastName != "" && _customerShippingInfo.Address1 != "" && _customerShippingInfo.State != "-1" && _customerShippingInfo.PostalCode != "")
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
                                _userToUpdate.ShippingAddressID = int.Parse(address.AddressID.ToString());
                                _userToUpdate.Save();
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
                            _userToUpdate.ShippingAddressID = int.Parse(address.AddressID.ToString());
                            _userToUpdate.Save();
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
                else
                {
                    if (_customerShippingInfo.City == "")
                    {
                        _res.Message = "City field is required for shipping";
                    }
                    if (_customerShippingInfo.FirstName == "")
                    {
                        _res.Message = "FirstName field is required for shipping";
                    }
                    if (_customerShippingInfo.LastName == "")
                    {
                        _res.Message = "LastName field is required for shipping";
                    }
                    if (_customerShippingInfo.Address1 == "")
                    {
                        _res.Message = "Address1 field is required for shipping";
                    }
                    if (_customerShippingInfo.State == "-1")
                    {
                        _res.Message = "State field is required for shipping";
                    }
                    if (_customerShippingInfo.PostalCode == "")
                    {
                        _res.Message = "PostalCode field is required for shipping";
                    }
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
                if (_customerBillingInfo.FirstName != "" && _customerBillingInfo.LastName != "" && _customerBillingInfo.Address1 != "" && _customerBillingInfo.City != "" && _customerBillingInfo.State != "-1" && _customerBillingInfo.PostalCode != "")
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
                        _userToUpdate.BillingAddressID = int.Parse(address.AddressID.ToString());
                        _userToUpdate.Save();


                        if (_customerBillingInfo.UpdateCreditCardInfo)
                        {
                            if (_customerBillingInfo.NameOnCard != "" && _customerBillingInfo.CardNumber != "" && _customerBillingInfo.ExipiresOnMonth != -1 && _customerBillingInfo.ExipiresOnYear != -1 && _customerBillingInfo.CardIdCode != "")
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
                                    catch (Exception ex)
                                    {
                                        _res.Message = "Error in updating credit card details " + ex.Message;
                                    }
                                }
                            }
                            else
                            {
                                if (_customerBillingInfo.State == "-1")
                                {
                                    _res.Message += "State field is requried for Billing \n\n";
                                }
                                if (_customerBillingInfo.NameOnCard == "")
                                {
                                    _res.Message += "*NameOnCard is requried for card information \n\n";
                                }
                                if (_customerBillingInfo.CardNumber == "")
                                {
                                    _res.Message += "*Cardnumber is requried for card information \n\n";
                                }
                                if (_customerBillingInfo.ExipiresOnMonth == -1)
                                {
                                    _res.Message += "*ExpiresOnMonth is requried for card information \n\n";
                                }
                                if (_customerBillingInfo.ExipiresOnYear == -1)
                                {
                                    _res.Message += "*ExpiresOnYear is requried for card information \n\n";
                                }
                                if (_customerBillingInfo.CardIdCode == "")
                                {
                                    _res.Message += "*CardIdCode is requried for card information \n\n";
                                }
                            }
                        }
                        else
                        {
                            _res.IsSuccess = true;
                            _res.StatusCode = System.Net.HttpStatusCode.OK;
                            _res.Message = "Billing address updated Succefully  ";
                        }


                    }
                    else
                    {
                        _res.Message = "No user found to update with this id " + _customerBillingInfo.UserID;
                    }

                }
                else
                {
                    if (_customerBillingInfo.FirstName == "")
                    {
                        _res.Message = "*First Name is Requried For Billing \n\n";
                    }
                    if (_customerBillingInfo.LastName == "")
                    {
                        _res.Message += "*Last Name is Requried For Billing \n\n";
                    }
                    if (_customerBillingInfo.Address1 == "")
                    {
                        _res.Message += "*Address1 is Requried For Billing \n\n";
                    }
                    if (_customerBillingInfo.City == "")
                    {
                        _res.Message += "*City is Requried For Billing \n\n";
                    }
                    if (_customerBillingInfo.State == "-1")
                    {
                        _res.Message += "*States is Requried For Billing \n\n";
                    }
                    if (_customerBillingInfo.PostalCode == "")
                    {
                        _res.Message += "*PostalCode is Requried For Billing \n\n";
                    }
                   
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
                    if (_customerPrefs.Preferences.Length != 0)
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

        public static PostHttpResponse UpdateShippingAddressforsubprofile(CustomerShippingAddress _customerShippingInfo)
        {
            var _res = new PostHttpResponse();

            try
            {
                if (_customerShippingInfo.City != "" && _customerShippingInfo.FirstName != "" && _customerShippingInfo.LastName != "" && _customerShippingInfo.Address1 != "" && _customerShippingInfo.State != "" && _customerShippingInfo.PostalCode != "")
                {
                    var parentprofileid = hccUserProfile.GetParentProfileBy(new Guid(_customerShippingInfo.UserID));
                    hccUserProfile _userToUpdate = GetUserProfileByProfileId(_customerShippingInfo.ShippingAddressID);
                    if (_userToUpdate != null)
                    {
                        var CurrentAddress = hccAddress.GetById(_customerShippingInfo.ShippingAddressID);
                        hccAddress address;

                        if (parentprofileid.ShippingAddressID == _userToUpdate.ShippingAddressID)
                        {
                           address = new hccAddress { Country = "US", AddressTypeID = 4 };
                        }
                        else
                        {
                            if (CurrentAddress == null)
                            {
                                address = new hccAddress { Country = "US", AddressTypeID = 4 };
                            }
                            else
                            {
                                address = CurrentAddress;
                            }
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
                                _userToUpdate.ShippingAddressID = int.Parse(address.AddressID.ToString());
                                _userToUpdate.Save();
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
                            _userToUpdate.ShippingAddressID = int.Parse(address.AddressID.ToString());
                            _userToUpdate.Save();
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
                else
                {
                    if (_customerShippingInfo.City == "")
                    {
                        _res.Message = "City field is required for shipping";
                    }
                    if (_customerShippingInfo.FirstName == "")
                    {
                        _res.Message = "FirstName field is required for shipping";
                    }
                    if (_customerShippingInfo.LastName == "")
                    {
                        _res.Message = "LastName field is required for shipping";
                    }
                    if (_customerShippingInfo.Address1 == "")
                    {
                        _res.Message = "Address1 field is required for shipping";
                    }
                    if (_customerShippingInfo.State == "")
                    {
                        _res.Message = "State field is required for shipping";
                    }
                    if (_customerShippingInfo.PostalCode == "")
                    {
                        _res.Message = "PostalCode field is required for shipping";
                    }
                }

            }
            catch (Exception ex)
            {
                _res.Message = "Error in updating user : " + Environment.NewLine + ex.Message;
            }

            return _res;
        }

        #endregion

    }
   

}