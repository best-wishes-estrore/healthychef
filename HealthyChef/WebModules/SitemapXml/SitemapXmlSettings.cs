using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Configuration;

namespace BayshoreSolutions.WebModules.SitemapXml
{
    public class xSettings
    {
        public bool Generate
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        public bool DefaultIncludePage
        {
            get;
            set;
        }

        public bool DefaultIncludeLastMod
        {
            get;
            set;
        }

        public DateTime? DefaultLastMod
        {
            get;
            set;
        }

        public bool DefaultIncludeChangefreq
        {
            get;
            set;
        }

        public string DefaultChangefreq
        {
            get;
            set;
        }

        public bool DefaultIncludePriority
        {
            get;
            set;
        }

        public decimal DefaultPriority
        {
            get;
            set;
        }

        protected bool IsSet(string strValue)
        {
            bool bSet = false;
            if (string.Compare(strValue, "1") == 0)
            {
                bSet = true;
            }
            else
            {
                if (string.Compare(strValue, "true", true) == 0)
                {
                    bSet = true;
                }
            }

            return bSet;
        }

        public void Load()
        {
            string strGenerate = ConfigurationManager.AppSettings["SitemapXml_Generate"];
            Generate = IsSet(strGenerate);

            string strFileName = ConfigurationManager.AppSettings["SitemapXml_FileName"];
            if (string.IsNullOrEmpty(strFileName))
            {
                strFileName = "/sitemap.xml";
            }
            FileName = strFileName;

            string strIncludePage = ConfigurationManager.AppSettings["SitemapXml_Default_IncludePage"];
            DefaultIncludePage = IsSet(strIncludePage);

            string strIncludeLastMod = ConfigurationManager.AppSettings["SitemapXml_Default_IncludeLastMod"];
            DefaultIncludeLastMod = IsSet(strIncludeLastMod);

            string strLastMod = ConfigurationManager.AppSettings["SitemapXml_Default_LastMod"];
            DateTime? dtLastMod = null;
            if (!string.IsNullOrEmpty(strLastMod))
            {
                DateTime dt;
                if (DateTime.TryParse(strLastMod, out dt))
                {
                    dtLastMod = dt;
                }
            }
            DefaultLastMod = dtLastMod;

            string strIncludeChangefreq = ConfigurationManager.AppSettings["SitemapXml_Default_IncludeChangefreq"];
            DefaultIncludeChangefreq = IsSet(strIncludeChangefreq);

            string strChangefreq = ConfigurationManager.AppSettings["SitemapXml_Default_Changefreq"];
            DefaultChangefreq = strChangefreq;

            string strIncludePriority = ConfigurationManager.AppSettings["SitemapXml_Default_IncludePriority"];
            DefaultIncludePriority = IsSet(strIncludePriority);

            string strPriority = ConfigurationManager.AppSettings["SitemapXml_Default_Priority"];
            decimal dPriority = 0.5M;
            decimal d;
            if (decimal.TryParse(strPriority, out d))
            {
                dPriority = d;
            }
            DefaultPriority = dPriority;
        }

        public void Save()
        {
            Configuration webConfig = WebConfigurationManager.OpenWebConfiguration("~");
            webConfig.AppSettings.Settings["SitemapXml_Generate"].Value = Generate.ToString();
            webConfig.AppSettings.Settings["SitemapXml_FileName"].Value = FileName.ToString();
            webConfig.AppSettings.Settings["SitemapXml_Default_IncludePage"].Value = DefaultIncludePage.ToString();
            webConfig.AppSettings.Settings["SitemapXml_Default_IncludeLastMod"].Value = DefaultIncludeLastMod.ToString();
            webConfig.AppSettings.Settings["SitemapXml_Default_LastMod"].Value = DefaultLastMod.ToString();
            webConfig.AppSettings.Settings["SitemapXml_Default_IncludeChangefreq"].Value = DefaultIncludeChangefreq.ToString();
            webConfig.AppSettings.Settings["SitemapXml_Default_Changefreq"].Value = DefaultChangefreq;
            webConfig.AppSettings.Settings["SitemapXml_Default_IncludePriority"].Value = DefaultIncludePriority.ToString();
            webConfig.AppSettings.Settings["SitemapXml_Default_Priority"].Value = DefaultPriority.ToString();

            webConfig.Save();
        }
    }
}
