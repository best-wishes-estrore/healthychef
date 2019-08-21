using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class HomeViewModel
    {
        public string HtmlString { get; set; }


        public HomeViewModel()
        {
            this.HtmlString = string.Empty;
            try
            {
                StringBuilder _htmlStr = new StringBuilder();

                BayshoreSolutions.WebModules.ContentModule.Content content = null;
                content = BayshoreSolutions.WebModules.ContentModule.Content.GetActiveContentForDisplay(1, BayshoreSolutions.WebModules.CultureCode.Current);
                if (null != content)
                {
                    _htmlStr.Append(content.Text);
                }

                this.HtmlString = _htmlStr.ToString();
            }
            catch (Exception E)
            {

            }
        }
    }
}