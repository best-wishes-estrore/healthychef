using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;

namespace ZipToTaxService
{

    public class ZipToTaxConfig
    {
        Configuration _config;
        public ZipToTaxConfigSection Settings;

        public ZipToTaxConfig()
        {
            _config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            Settings = (ZipToTaxConfigSection)_config.Sections.Get("ZipToTaxConfig");
        }

        public string ConnString
        {
            get { return "Server=" + Settings.Server + "; Database=" + Settings.DBName + 
                "; User Id=" + Settings.DBUsername + "; password=" + Settings.DBPassword + ";"; }
        }
    }

    public class ZipToTaxConfigSection : ConfigurationSection
    {     
        const string server = "server";
        const string dbUsername = "dbUsername";
        const string dbPassword = "dbPassword";
        const string dbName = "dbName";
        const string loginUserName = "loginUserName";
        const string loginUserPassword = "loginUserPassword";
        
        

        [ConfigurationProperty(server, IsRequired = true)]
        public string Server
        {
            get { return this[server].ToString(); }
        }

        [ConfigurationProperty(dbUsername, IsRequired = true)]
        public string DBUsername
        {
            get { return this[dbUsername].ToString(); }
        }

        [ConfigurationProperty(dbPassword, IsRequired = true)]
        public string DBPassword
        {
            get { return this[dbPassword].ToString(); }
        }

        [ConfigurationProperty(dbName, IsRequired = true)]
        public string DBName
        {
            get { return this[dbName].ToString(); }
        }

        [ConfigurationProperty(loginUserName, IsRequired = true)]
        public string LoginUserName
        {
            get { return this[loginUserName].ToString(); }
        }

        [ConfigurationProperty(loginUserPassword, IsRequired = true)]
        public string LoginUserPassword
        {
            get { return this[loginUserPassword].ToString(); }
        }
    }

}
