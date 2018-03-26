using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace UtilitiesManagers
{
    public sealed class ConfigurationHelper
    {
        public static string GetConnectionString(string lconnectionstring)
        {
            string str = string.Empty;

            str = ConfigurationManager.ConnectionStrings[lconnectionstring].ToString();
            if (str == null || (str != null && str.Length == 0))
                throw (new ApplicationException(lconnectionstring + " key is missing from your web.config"));
            return str;
        }

        public static string GetAppSettingValue(string lkey)
        {
            string str = string.Empty;
            str = ConfigurationManager.AppSettings[lkey];
            if (str == null || (str != null && str.Length == 0))
                throw (new ApplicationException(lkey + " key is missing from your web.config"));
            return str;
        }

        public static string connectionString
        {
            get { return GetConnectionString(ConfigKeys.connectionString); }
        }
    }
}
