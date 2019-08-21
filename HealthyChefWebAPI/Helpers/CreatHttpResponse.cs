using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace HealthyChefWebAPI.Helpers
{
    public class CreatHttpResponse
    {
        public static HttpResponseMessage CreateHttpResponse(HttpRequestMessage Request)
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }


    public class PostHttpResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public PostHttpResponse()
        {
            IsSuccess = false;
            Message = "Invalid Request";
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}