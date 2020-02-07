using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class FacebookLoginModel
    {

        public string uid { get; set; }
        public string accessToken { get; set; }
        public string ReturnUrl { get; set; }
        public int CustomerType { get; set; }
        public FacebookLoginModel(string _returnUrl)
        {
            CustomerType = 1;
            this.ReturnUrl = _returnUrl;
        }
         
        public FacebookLoginModel()
        {
            CustomerType = 0;
        }


    }
}