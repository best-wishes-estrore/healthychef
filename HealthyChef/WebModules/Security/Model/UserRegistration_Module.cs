using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Web.Security;

namespace BayshoreSolutions.WebModules.Cms.Security.Model
{
    public partial class UserRegistration_Module
    {
        private static readonly string _connectionString = BayshoreSolutions.WebModules.Settings.ConnectionString;

        /// <summary>Saves the entity to the information store.</summary>
        public int Save()
        {
            //pre-save code...

            //call the private Save_ function
            return Save_(this);

            //post-save code...
        }

        /// <summary>Physically deletes the specified entity from the information store.</summary>
        public static void Destroy(
            int moduleId
        )
        {
            //pre-destroy code...

            //call the private Destroy_ function
            Destroy_(
                moduleId
            );

            //post-destroy code...
        }

        public static string GetHumanStatusMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.InvalidPassword:
                    return "Password does not meet strength requirements, or does not meet the following criteria: "
                        + "<br/>minimum length: "
                        + Membership.MinRequiredPasswordLength
                        + "<br/>minimum non-alphanumeric characters: "
                        + Membership.MinRequiredNonAlphanumericCharacters;
                case MembershipCreateStatus.DuplicateEmail:
                    return " Email address is already in use.";
                case MembershipCreateStatus.DuplicateUserName:
                    return " Email address is already in use.";
                default:
                    return status.ToString();
            }
        }
    }

    public partial class UserRegistration_ModuleCollection : List<UserRegistration_Module>
    {
    }
}
