using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Http;
using System.Net.Http.Headers;

namespace HealthyChef.Intercom.Controllers
{
    public class IntercomHelper
    {
        public static HttpClient CreateWebAPIRequest()
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri("https://api.intercom.io/");
            client.DefaultRequestHeaders.Accept.Clear();
            var byteArray = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["ktzghdfu"] + ":" + ConfigurationManager.AppSettings["PzEHxf-z-mt1ODRfqAp5gimMcW9l0YMra-MEZc81"]);
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }

        //public static async Task<HttpResponseMessage> GetIntercomAdmins()
        //{

        //    HttpResponseMessage response;
        //    var client = CreateWebAPIRequest();
        //    //response = await client.GetAsync("admins");
        //    return response;
        //}
        //public static async Task<HttpResponseMessage> GetIntercomTags()
        //{

        //    HttpResponseMessage response;
        //    var client = CreateWebAPIRequest();
        //   // response = await client.GetAsync("tags");
        //    return response;
        //}
    }
}