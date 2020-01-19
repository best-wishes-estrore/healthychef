using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web.Configuration;
using System.Web.Security;

namespace HealthyChef.DAL
{
    public partial class hccUserProfile
    {
        //static healthychefEntities cont
        //{
        //    get
        //    {
        //        if (System.Web.HttpContext.Current != null)
        //            return healthychefEntities.Default;
        //        else
        //            return new healthychefEntities(WebConfigurationManager.ConnectionStrings["healthychefEntities"].ConnectionString);
        //    }
        //}

        /// <summary>
        /// Saves this user to the database.  If this user hasn't been created yet 
        /// this user will be added to the database.
        /// </summary>
        public void Save()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI(WebConfigurationManager.ConnectionStrings["healthychefEntities"].ConnectionString))
                {
                    System.Data.EntityKey key = cont.CreateEntityKey("hccUserProfiles", this);
                    object oldObj;

                    if (cont.TryGetObjectByKey(key, out oldObj))
                    {
                        cont.ApplyCurrentValues("hccUserProfiles", this);
                    }
                    else
                    {
                        cont.hccUserProfiles.AddObject(this);
                    }

                    cont.SaveChanges();
                    //cont.Refresh(System.Data.Objects.RefreshMode.StoreWins, this);
                }
            }
            catch
            {
                throw;
            }
        }

        public MembershipUser ASPUser
        {
            get
            {
                return Membership.GetUser(this.MembershipID);
            }
        }

        public bool IsChildProfile
        {
            get { return this.ParentProfileID.HasValue; }
        }

        public string ParentProfileName
        {
            get
            {
                hccUserProfile parent = hccUserProfile.GetParentProfileBy(this.MembershipID);
                if (parent == null)
                    return this.FullName;
                else
                    return parent.FullName;
            }
        }

        public string AllergensList
        {
            get
            {
                string allergens = this.GetAllergens()
                    .OrderBy(s => s.Name)
                    .Select(b => b.Name)
                    .Distinct()
                    .DefaultIfEmpty("None")
                    .Aggregate((c, d) => c + ", " + d);

                return allergens;
            }
        }

        public string FullName { get { return LastName + ", " + FirstName; } }


        public hccUserProfilePaymentProfile ActivePaymentProfile
        {
            get { return hccUserProfilePaymentProfile.GetBy(this.UserProfileID); }
        }

        /// <summary>
        /// Gets the hccUserProfile object with the specified id.  If there is no 
        /// profile to return then the return will be null.
        /// </summary>
        /// <param name="id">The id to use to get the profile.</param>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>This returns the user profile that matches the id.</returns>
        public static hccUserProfile GetById(int id)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfiles
                        .SingleOrDefault(a => a.UserProfileID == id);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the profile object with the specified membership id.  If there is no
        /// profile to return then the return will be null.  If there is more than one
        /// profile with the membership id an exception will be thrown.
        /// </summary>
        /// <param name="id">The guid id represented in membership.</param>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        /// <returns>This returns the the user profile with the matching guid, or null
        /// if there is no matching guid.</returns>
        public static List<hccUserProfile> GetBy(Guid userProviderKey)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfiles
                        .Where(a => a.MembershipID == userProviderKey)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccUserProfile GetBy(int cartItemId)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    var ci = cont.hccCartItems
                        .Where(a => a.CartItemID == cartItemId)
                        .SingleOrDefault();

                    if (ci != null)
                        return ci.UserProfile;
                    else
                        return null;
                }
            }
            catch
            {
                throw;
            }
        }


        public static List<hccUserProfile> GetBy(Guid userProviderKey, bool isActive)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfiles
                        .Where(a => a.MembershipID == userProviderKey
                            && a.IsActive == isActive)
                        .ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Get by account membershipID and profile name. Used to check for whethewr a profile name exists for an account.
        /// </summary>
        /// <param name="userProviderKey"></param>
        /// <param name="profileName"></param>
        /// <returns></returns>
        public static hccUserProfile GetBy(Guid userProviderKey, string profileName)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfiles
                        .SingleOrDefault(a => a.MembershipID == userProviderKey
                            && a.ProfileName == profileName);
                }
            }
            catch
            {
                throw;
            }
        }

        public static hccUserProfile GetParentProfileBy(Guid userProviderKey)
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfiles
                        .Where(a => a.MembershipID == userProviderKey && !a.ParentProfileID.HasValue && a.IsActive)
                        .OrderBy(a => a.CreatedDate).FirstOrDefault();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets all the root profiles in the system.
        /// </summary>
        /// <param name="isActive">Determines whether the method is to return records depending upon their IsActive flag; Active(true), Inactive(false), All(null)</param>
        /// <returns>This returns a list of all "Root Profiles".</returns>
        /// <exception cref="System.Exception">re-thrown exception.</exception>
        public static List<hccUserProfile> GetRootProfiles()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfiles
                        .Where(a => !a.ParentProfileID.HasValue).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Gets the child profiles of this profile.
        /// </summary>
        /// <returns>This returns a list of child profiles.</returns>
        /// <exception cref="System.Exception">re-thrown exception</exception>
        public List<hccUserProfile> GetSubProfiles()
        {
            try
            {
                using (var cont = new healthychefEntitiesAPI())
                {
                    return cont.hccUserProfiles
                        .Where(a => a.ParentProfileID == this.UserProfileID).ToList();
                }
            }
            catch
            {
                throw;
            }
        }

        public List<hccAllergen> GetAllergens()
        {
            try
            {
                List<hccAllergen> profAllgs = hccUserProfileAllergen.GetAllergensBy(this.UserProfileID, true);
                return profAllgs;

            }
            catch (Exception)
            {

                throw;
            }

        }

        public List<hccPreference> GetPreferences()
        {
            try
            {
                List<hccPreference> profPrefs = hccUserProfilePreference.GetPrefsBy(this.UserProfileID, true);
                return profPrefs;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Deactivates this user in the database.  If the user hasn't been
        /// created yet the user will be created in a deactivated state.
        /// </summary>
        /// <exception cref="System.Exception">re-throw exception.</exception>
        public void Activation(bool isActive)
        {
            try
            {
                this.IsActive = isActive;
                Save();
            }
            catch
            {
                throw;
            }
        }

        public bool HasShippingAddress
        {
            get { return this.ShippingAddressID.HasValue; }
        }

        public static List<MembershipUser> Search(string lastName, string email, string phone, int? purchaseNumber, DateTime? deliveryDate, string roles)
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
                       delegate(Guid? membershipUserId)
                       {
                           if (membershipUserId.HasValue)
                           {
                               MembershipUser user = Membership.GetUser(membershipUserId.Value);

                               if (user != null)
                                   retVals.Add(user);
                           }
                       });

                return retVals.OrderBy(a => a.Email).ToList();
            }
            catch
            {
                throw;
            }
        }
    }
}
