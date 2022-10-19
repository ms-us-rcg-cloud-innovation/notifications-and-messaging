using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.MessagingFunctions
{
    internal class RequestHelpers
    {
        public static readonly Dictionary<string, NotificationPlatform> PlatformEnumLookup = new()
        {
            { "fcm", NotificationPlatform.Fcm },
            { "gcm", NotificationPlatform.Fcm }
        };
    }
}
