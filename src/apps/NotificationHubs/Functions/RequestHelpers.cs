using Microsoft.Azure.NotificationHubs;

namespace NotificationsAndMessaging.NotificationHubs.Functions
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
