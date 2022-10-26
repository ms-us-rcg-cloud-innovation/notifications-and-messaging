using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.MessagingFunctions
{
    internal class ConfigurationConstants
    {
        public const string COSMOS_DB_CONFIG_KEY = "COSMOS_DB";
        public const string COSMOS_DEVICES_CONTAINER_CONFIG_KEY = "COSMOS_CONNECTION_STRING";
        public const string COSMOS_CONNECTIONSTRING_CONFIG_KEY = "COSMOS_CONNECTION_STRING";
        public const string DEVICES_CONTAINER_PARTITIONKEY = "/deviceType";
    }
}
