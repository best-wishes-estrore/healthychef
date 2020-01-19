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
    public class OrderManagementController : ApiController
    {
        [HttpPost]
        [ActionName("GetAllPurchases")]
        public HttpResponseMessage GetAllPurchases(SearchParamsForPurchases searchParams)
        {
            // searchParams = searchParams ?? new SearchParams();

            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = OrderManagementRepository.GetAllPurchases(searchParams);

            response.Content = new StringContent(
                                _content,
                                Encoding.UTF8,
                                "application/json");

            //if (string.IsNullOrEmpty(_content))
            //{
            //    response.StatusCode = HttpStatusCode.BadRequest;
            //}


            return response;
        }

        [HttpPost]
        [ActionName("GetOrderFullfillment")]
        public HttpResponseMessage GetOrderFullfillment(SearchParamsForOrderFulfillment searchParamsForOrderFulfillment)
        {

            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = OrderManagementRepository.GetOrderFullfillmentDetails(searchParamsForOrderFulfillment);

            response.Content = new StringContent(
                                _content,
                                Encoding.UTF8,
                                "application/json");

            //if (string.IsNullOrEmpty(_content))
            //{
            //    response.StatusCode = HttpStatusCode.BadRequest;
            //}


            return response;
        }


        [HttpGet]
        [ActionName("GetRecurringOrder")]
        public HttpResponseMessage GetRecurringOrder()
        {            
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = OrderManagementRepository.GetRecurringOrder();

            response.Content = new StringContent(
                                _content,
                                Encoding.UTF8,
                                "application/json");

            //if (string.IsNullOrEmpty(_content))
            //{
            //    response.StatusCode = HttpStatusCode.BadRequest;
            //}


            return response;
        }



        [HttpGet]
        [ActionName("GetCancellation")]
        public HttpResponseMessage GetCancellation(int PurchaseNumber)
        {
            
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = OrderManagementRepository.GetCancellation(PurchaseNumber);

            response.Content = new StringContent(
                                _content,
                                Encoding.UTF8,
                                "application/json");

            //if (string.IsNullOrEmpty(_content))
            //{
            //    response.StatusCode = HttpStatusCode.BadRequest;
            //}


            return response;
        }

        [HttpPost]
        [ActionName("CancelCartItems")]
        public HttpResponseMessage CancelCartItems(int PurchaseNumber, string OrderNumber, bool iscancel)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = OrderManagementRepository.CancelCartItems(PurchaseNumber, OrderNumber,iscancel);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpGet]
        [ActionName("CartItemsDetails")]
        public HttpResponseMessage CartItemsDetails(string OrderNumber)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            //var _op = OrderManagementRepository.CartItemsDetails(OrderNumber);

            response.Content = new StringContent(
                                OrderManagementRepository.CartItemsDetails(OrderNumber),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("CancelItemDetails")]
        public HttpResponseMessage CancelItemDetails(int cartitemid, bool iscancel)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = OrderManagementRepository.CancelItemDetails(cartitemid,iscancel);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("CancelAutorenew")]
        public HttpResponseMessage CancelAutorenew(int cartitemid, int cartid)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = OrderManagementRepository.CancelAutorenew(cartitemid, cartid);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }


        [HttpGet]
        [ActionName("PurchaseDetails")]
        public HttpResponseMessage PurchaseDetails(int Cartid)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = OrderManagementRepository.PurchaseDetails(Cartid);
            response.StatusCode = _op.StatusCode;

            response.StatusCode = _op.StatusCode;
            response.Content = new StringContent(
                                //_op.Message,
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpGet]
        [ActionName("PostponeOrderFullfilment")]
        public HttpResponseMessage PostponeOrderFullfilment(int cartItemId,string delDate)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = OrderManagementRepository.PostponeOrderFullfilment(cartItemId, delDate);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

    }
}
