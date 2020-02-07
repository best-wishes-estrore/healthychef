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
    public class ProductionManagementController : ApiController
    {
        [HttpGet]
        [ActionName("GetIssuedCerts")]
        public HttpResponseMessage GetIssuedCerts()
        {         
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            response.Content = new StringContent(
                                ProductionManagementRepository.GetIssuedCerts(),
                                Encoding.UTF8,
                                "application/json");

            if (string.IsNullOrEmpty(response.Content.ToString()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }


        [HttpGet]
        [ActionName("GetRedeemdedCerts")]
        public HttpResponseMessage GetRedeemdedCerts()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            response.Content = new StringContent(
                                ProductionManagementRepository.GetRedeemdedCerts(),
                                Encoding.UTF8,
                                "application/json");

            if (string.IsNullOrEmpty(response.Content.ToString()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }


        [HttpGet]
        [ActionName("GetImported")]
        public HttpResponseMessage GetImported()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            response.Content = new StringContent(
                                ProductionManagementRepository.GetImported(),
                                Encoding.UTF8,
                                "application/json");

            if (string.IsNullOrEmpty(response.Content.ToString()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }


        [HttpGet]
        [ActionName("GetMenus")]
        public HttpResponseMessage GetMenus()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            response.Content = new StringContent(
                                ProductionManagementRepository.GetMenus(),
                                Encoding.UTF8,
                                "application/json");

            if (string.IsNullOrEmpty(response.Content.ToString()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;

        }

        [HttpGet]
        [ActionName("GetFutureCalender")]
        public HttpResponseMessage GetFutureCalender()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            response.Content = new StringContent(
                                ProductionManagementRepository.GetFutureCalender(),
                                Encoding.UTF8,
                                "application/json");

            if (string.IsNullOrEmpty(response.Content.ToString()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }


        [HttpGet]
        [ActionName("GetPastCalender")]
        public HttpResponseMessage GetPastCalender()
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            response.Content = new StringContent(
                                ProductionManagementRepository.GetPastCalender(),
                                Encoding.UTF8,
                                "application/json");

            if (string.IsNullOrEmpty(response.Content.ToString()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
            }

            return response;
        }

        //Post

        [HttpPost]
        [ActionName("AddorUpdateProductionCalendar")]
        public HttpResponseMessage AddorUpdateProductionCalendar(ProductionCalendar calendar)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var result = ProductionManagementRepository.AddorUpdateProductionCalendar(calendar);
            response.StatusCode = result.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(result),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("UpdateGiftCertificates")]
        public HttpResponseMessage UpdateGiftCertificates(GiftCertificates certificate)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var result = ProductionManagementRepository.UpdateGiftCertificates(certificate);
            response.StatusCode = result.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(result),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

        [HttpPost]
        [ActionName("UpdateImportedGiftCertificate")]
        public HttpResponseMessage UpdateImportedGiftCertificate(ImportedGiftCertificate certificate)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var result = ProductionManagementRepository.UpdateImportedGiftCertificate(certificate);
            response.StatusCode = result.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(result),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }


        [HttpPost]
        [ActionName("AddorUpdateMenu")]
        public HttpResponseMessage AddorUpdateMenu(Menu _menu)
        {
            var response = CreatHttpResponse.CreateHttpResponse(this.Request);

            var result = ProductionManagementRepository.AddorUpdateMenu(_menu);
            response.StatusCode = result.StatusCode;

            response.Content = new StringContent(
                                DBHelper.ConvertDataToJson(result),
                                Encoding.UTF8,
                                "application/json");

            return response;
        }

    }
}
