using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthyChefCreationsMVC.CustomModels
{
    public class SiteMapProviderXml
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public List<SiteMapProviderXml> listofSitemap { get; set; }

        public SiteMapProviderXml()
        {
            this.listofSitemap = new List<SiteMapProviderXml>();
        }
    }
}