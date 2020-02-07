using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace HealthyChef.AuthNet
{
    public class AuthNetConfig
    {
        Configuration _config;
        public AuthNetConfigSection Settings;

        public AuthNetConfig()
        {
            _config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            Settings = (AuthNetConfigSection)_config.Sections.Get("AuthNetConfig");
        }
    }

    public class AuthNetConfigSection : ConfigurationSection
    {
        private const string NAME = "name";
        private const string APIKEY = "apiKey";
        private const string TRANSACTIONKEY = "transactionKey";
        private const string TESTMODE = "testMode";

        [ConfigurationProperty(NAME, IsRequired = true)]
        public string Name
        {
            get { return (string)this[NAME]; }
        }

        [ConfigurationProperty(APIKEY, IsRequired = false)]
        public string ApiKey
        {
            get { return (string)this[APIKEY]; }
            set { this[APIKEY] = value; }
        }

        [ConfigurationProperty(TRANSACTIONKEY, IsRequired = false)]
        public string TransactionKey
        {
            get { return (string)this[TRANSACTIONKEY]; }
            set { this[TRANSACTIONKEY] = value; }
        }

        [ConfigurationProperty(TESTMODE, IsRequired = false, DefaultValue = true)]
        public bool TestMode
        {
            get { return (bool)this[TESTMODE]; }
            set { this[TESTMODE] = value; }
        }       
    }
}
