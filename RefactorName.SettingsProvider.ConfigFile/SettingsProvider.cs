using RefactorName.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.SettingsProvider.ConfigFile
{
    public class SettingsProvider : ISettingsProvider
    {
        public string CacheProvider
        {
            get { return ConfigurationManager.AppSettings["CacheProvider"]; }
        }

        public string DbProvider
        {
            get { return ConfigurationManager.AppSettings["DbProvider"]; }
        }

        public string WebSvcProviderName
        {
            get { return ConfigurationManager.AppSettings["WebSvcProvider"]; }
        }

        public string RedisServerHost
        {
            get
            {
                string retVal = "localhost";

                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Redis-Server:Host"]) == false)
                    retVal = ConfigurationManager.AppSettings["Redis-Server:Host"];

                return retVal;
            }
        }

        public string RedisServerPassword
        {
            get
            {
                string retVal = null;

                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["Redis-Server:Password"]) == false)
                    retVal = ConfigurationManager.AppSettings["Redis-Server:Password"];

                return retVal;
            }
        }

        public int RedisServerPort
        {
            get
            {
                int retVal = 6379;

                int result;
                if (int.TryParse(ConfigurationManager.AppSettings["Redis-Server:Port"], out result))
                    retVal = result;

                return retVal;
            }
        }

        public bool RedisServerSSL
        {
            get
            {
                bool retVal = false;

                bool result;
                if (bool.TryParse(ConfigurationManager.AppSettings["Redis-Server:SSL"], out result))
                    retVal = result;

                return retVal;
            }
        }

    }
}
