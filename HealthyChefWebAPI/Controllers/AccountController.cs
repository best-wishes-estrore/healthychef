using HealthyChefWebAPI.CustomModels;
using HealthyChefWebAPI.Helpers;
using HealthyChefWebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Security;

namespace HealthyChefWebAPI.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        [HttpPost]
        [ActionName("Login")]
        public HttpResponseMessage Login(LoginCredetails credetails)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _loginResponce = AccountRepository.LoginUser(credetails);
            response.StatusCode = _loginResponce.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_loginResponce),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }
    }
}



namespace HealthyChefWebAPI.Repository
{
    public class AccountRepository
    {
        public static LoginResponce LoginUser(LoginCredetails _creds)
        {
            _creds = _creds ?? new LoginCredetails();
            LoginResponce _resp = new LoginResponce();
            try
            {
                //string userName = _creds.UserName.Trim();
                string userName = Membership.GetUserNameByEmail(_creds.UserName.Trim());
                userName = userName ?? _creds.UserName.Trim();

                MembershipUser user = Membership.GetUser(userName);
                string _invalidLogin = "Login Attempt Failed.  Email/password combination not recognized.  Please re-enter your email address and account password.  If you have forgotten your password, please click the link below or call customer service at 866-575-2433 for assistance.";
                string _insufficientRoles = "The User does not have access to admin panel, Please login with admin credentials";

                if (user != null)
                {


                    string[] roles = Roles.GetRolesForUser(userName);

                    if (!Membership.ValidateUser(userName, _creds.Password.Trim()))
                    {
                        _resp.Message = _invalidLogin;
                    }
                    else if (!user.IsApproved)
                    {
                        _resp.Message = "That account has been deactivated. Please contact customer service at 866-575-2433 for assistance.";
                    }
                    else if (user.IsLockedOut)
                    {
                        _resp.Message = "That account is locked out. Please contact customer service at 866-575-2433 for assistance.";
                    }
                    else if(roles.Length == 0)
                    {
                        _resp.Message = _insufficientRoles;
                    }
                    else if (!roles.Contains("Administrators"))
                    {
                        _resp.Message = _insufficientRoles;
                    }
                    else
                    {
                        FormsAuthentication.SetAuthCookie(userName, true);

                        _resp.Message = string.Format("Login Successful as {0}",user.UserName);
                        _resp.IsSuccess = true;
                        _resp.IsAdmin = true;
                        _resp.StatusCode = HttpStatusCode.OK;
                    }
                }
                else
                {
                    _resp.Message = _invalidLogin;
                }

                return _resp;
            }
            catch (Exception Ex)
            {
                return _resp;
            }

        }
    }
}



namespace HealthyChefWebAPI.CustomModels
{
    public class LoginCredetails
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginCredetails()
        {
            UserName = Password = string.Empty;
        }
    }

    public class LoginResponce
    {
        public bool IsSuccess { get; set; }
        public bool IsAdmin { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public LoginResponce()
        {
            IsSuccess = IsAdmin = false;
            Message = "Invalid Request";
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}