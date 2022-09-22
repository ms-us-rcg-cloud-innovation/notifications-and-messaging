using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.Platforms.Android.Helpers
{
    internal static class NotificationHelpers
    {

        // Since android has specific behavior for handling notifications while in foregournd, use this 
        // method to raise a notification on device
        public static void RaiseSystemNotificationWhileInForeground(NotificationMessage message)
        {
            var notificationManager = NotificationManager.FromContext(MainApplication.Context);

            CreateNotificationChannel(notificationManager, Constants.NOTIFICATION_CHANNEL_ID);

            var intent = new Intent(MainApplication.Context, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(MainApplication.Context, 0, intent, PendingIntentFlags.OneShot);

            var notificationBuilder = new NotificationCompat.Builder(MainApplication.Context, Constants.NOTIFICATION_CHANNEL_ID);

            notificationBuilder.SetContentTitle(message.Title)
                        .SetSmallIcon(NotificationHub.Maui.Resource.Drawable.ic_mtrl_checked_circle)
                        .SetContentText(message.Body)
                        .SetAutoCancel(true)
                        .SetShowWhen(true)
                        .SetContentIntent(pendingIntent);


            var notification = notificationBuilder.Build();


            var nId = RandomNumberGenerator.GetInt32(10000);

            notificationManager.Notify(nId, notification);
        }

        private static void CreateNotificationChannel(NotificationManager notificationManager, string channelId)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(channelId, channelId, NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            notificationManager.CreateNotificationChannel(channel);
        }

    }
}
