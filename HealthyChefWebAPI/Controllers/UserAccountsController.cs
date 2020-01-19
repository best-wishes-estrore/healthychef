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

namespace HealthyChefWebAPI.Controllers
{
    //[Authorize(Roles = "Administrators")]
    public class UserAccountsController : ApiController
    {
        [HttpPost]
        [ActionName("SearchGetUserAccounts")]
        public HttpResponseMessage SearchGetUserAccounts(SearchParams searchParameters)
        {

            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = UserAccountsRepository.SearchGetUserAccounts(searchParameters);

            response.Content = new StringContent(
                                _content,
                                Encoding.UTF8,
                                "application/json");
            return response;
        }


        [HttpGet]
        [ActionName("GetUserAccounts")]
        public HttpResponseMessage GetUserAccounts()
        {

            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            
            var _content = UserAccountsRepository.GetUserAccountDetails();

            response.Content = new StringContent(
                                _content,
                                Encoding.UTF8,
                                "application/json");

            //if (string.IsNullOrEmpty(_content))
            //{
            //    //response.StatusCode = HttpStatusCode.BadRequest;
            //}

            return response;
        }

        [HttpGet]
        [ActionName("GetUserAccountsByRole")]
        public HttpResponseMessage GetUserAccountsByRole(string roleid = null)
        {

            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = UserAccountsRepository.GetUserAccountDetailsByRole(roleid);

            response.Content = new StringContent(
                                _content,
                                Encoding.UTF8,
                                "application/json");

            //if (string.IsNullOrEmpty(_content))
            //{
            //    //response.StatusCode = HttpStatusCode.BadRequest;
            //}

            return response;
        }


        [HttpPost]
        [ActionName("UpdateStatusOfCustomer")]
        public HttpResponseMessage UpdateStatusOfCustomer(CustomerStatus _status)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = UserAccountsRepository.UpdateStatusOfCustomer(_status);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }


        [HttpPost]
        [ActionName("UpdateBasicInfo")]
        public HttpResponseMessage UpdateBasicInfo(CustomerBasicInfo _basicInfo)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = UserAccountsRepository.UpdateBasicInfo(_basicInfo);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("UpdateShippingAddress")]
        public HttpResponseMessage UpdateShippingAddress(CustomerShippingAddress _shippingInfo)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = UserAccountsRepository.UpdateShippingAddress(_shippingInfo);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("UpdateBillingInfo")]
        public HttpResponseMessage UpdateBillingInfo(CustomerBillingInfo _billingInfo)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = UserAccountsRepository.UpdateBillingInfo(_billingInfo);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("AddOrUpdateNotesForUser")]
        public HttpResponseMessage AddOrUpdateNotesForUser(CustomerNote _customerNote)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = UserAccountsRepository.AddOrUpdateNotesForUser(_customerNote);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("AddOrUpdatePreferenceForUser")]
        public HttpResponseMessage AddOrUpdatePreferenceForUser(CustomerPreferencesToUpdate _customerPreferences)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = UserAccountsRepository.AddOrUpdatePreferenceForUser(_customerPreferences);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }
        [HttpPost]
        [ActionName("UpdateShippingAddressforsubprofile")]
        public HttpResponseMessage UpdateShippingAddressforsubprofile(CustomerShippingAddress _shippingInfo)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = UserAccountsRepository.UpdateShippingAddressforsubprofile(_shippingInfo);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

    }
}
