using HealthyChefWebAPI.Helpers;
using HealthyChefWebAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using HealthyChefWebAPI.CustomModels;

namespace HealthyChefWebAPI.Controllers
{
    //[Authorize(Roles = "Administrators")]
    public class ListManagementController : ApiController
    {
        [HttpGet]
        [ActionName("GetPrograms")]
        public HttpResponseMessage GetPrograms()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetAllPrograms();

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
        [ActionName("GetMealPrefs")]
        public HttpResponseMessage GetMealPrefs()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetMealPrefs();

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
        [ActionName("GetCustomerPrefs")]
        public HttpResponseMessage GetCustomerPrefs()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetCustomerPrefs();

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
        [ActionName("GetPlans")]
        public HttpResponseMessage GetPlans()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetPlans();

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
        [ActionName("GetAllAllergens")]
        public HttpResponseMessage GetAllAllergens()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetAllergens();

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
        [ActionName("GetAllCoupons")]
        public HttpResponseMessage GetAllCoupons()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetAllCoupons();

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
        [ActionName("GetAllMessageboxSizes")]
        public HttpResponseMessage GetAllMessageboxSizes()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetMessageboxSizes();

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
        [ActionName("GetAllShippingZone")]
        public HttpResponseMessage GetAllShippingZone()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetShippingZone();

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
        [ActionName("GetAllIngredient")]
        public HttpResponseMessage GetAllIngredient()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetAllIngredient();

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
        [ActionName("GetItems")]
        public HttpResponseMessage GetItems()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var _content = ListManagementRepository.GetItems();

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



        //Post

        [HttpPost]
        [ActionName("AddOrUpdateAllergen")]
        public HttpResponseMessage AddOrUpdateAllergen(Allergens allergen)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdateAllergen(allergen);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("AddOrUpdateCoupon")]
        public HttpResponseMessage AddOrUpdateCoupon(Coupon coupon)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdateCoupon(coupon);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }


        [HttpPost]
        [ActionName("AddOrUpdateShippingZone")]
        public HttpResponseMessage AddOrUpdateShippingZone(ShippingZone shippingzone)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdateShippingZone(shippingzone);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }


        [HttpPost]
        [ActionName("DeleteShippingZone")]
        public HttpResponseMessage DeleteShippingZone(int shippingzoneid)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.DeleteShippingZone(shippingzoneid);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("AddOrUpdateIngredient")]
        public HttpResponseMessage AddOrUpdateIngredient(Ingredients ingredient)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdateIngredient(ingredient);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("AddOrUpdateItem")]
        public HttpResponseMessage AddOrUpdateItem(ItemPost item)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdateItem(item);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }


        [HttpPost]
        [ActionName("AddOrUpdatePlan")]
        public HttpResponseMessage AddOrUpdatePlan(Plan item)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdatePlan(item);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("AddOrUpdatePreference")]
        public HttpResponseMessage AddOrUpdatePreference(MealPreferences pref)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdatePreference(pref);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("AddOrUpdateProgram")]
        public HttpResponseMessage AddOrUpdateProgram(Programs program)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);
            var _op = ListManagementRepository.AddOrUpdateProgram(program);
            response.StatusCode = _op.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(_op),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

    }
}
