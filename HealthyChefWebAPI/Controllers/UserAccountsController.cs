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

        [HttpGet]
        [ActionName("GetUserAccounts")]
        public HttpResponseMessage GetUserAccounts(SearchParams searchParams)
        {           
            searchParams = searchParams ?? new SearchParams();

            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            //response.Content = new StringContent(
            //                    UserAccountsRepository.GetUserAccounts(
            //                        searchParams.lastName, 
            //                        searchParams.email, 
            //                        searchParams.phone, 
            //                        searchParams.purchaseNumber, 
            //                        searchParams.deliveryDate,
            //                        searchParams.roles), 
            //                    Encoding.UTF8, 
            //                    "application/json");

            response.Content = new StringContent(
                                UserAccountsRepository.GetUserAccountDetails(),
                                Encoding.UTF8,
                                "application/json");

            if(string.IsNullOrEmpty(response.Content.ToString()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

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

    }
}
