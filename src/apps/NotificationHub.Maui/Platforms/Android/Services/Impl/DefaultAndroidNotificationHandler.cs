using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Platforms.Android;
using NotificationHub.Maui.Services;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace NotificationHub.Core.Maui.Platforms.Android.Services.Impl;

public class DefaultAndroidNotificationHandler
    : INotificationHandler
{
    public event EventHandler<NotificationEventArgs> NotificationReceived;

    public DefaultAndroidNotificationHandler()
    {
        CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
        {

            var message = new NotificationMessage
            {
                Title = (string)p.Data["title"],
                Body = (string)p.Data["body"],
                Data = p.Data,
                TimeStamp = DateTime.Now
            };

            // handle the received notification while app is in foreground
            ReceiveNotificationAsync(message);

            var args = new NotificationEventArgs
            {
                Message = message,
                Platform = "Android"
            };

            // raise an abstracted event to the cross platform system
            OnNotificationReceived(args);
        };
    }

    protected void OnNotificationReceived(NotificationEventArgs e)
    {
        var handler = NotificationReceived;
        handler?.Invoke(this, e);
    }

    // Since android has specific behavior for handling notifications while in foregournd, use this 
    // method to raise a notification on device
    private Task ReceiveNotificationAsync(NotificationMessage message, IDictionary<string, object> properties = null)
    {
        var context = MainApplication.Context;
        var channelId = MainActivity.CHANNEL_ID;
        var intentType = typeof(MainActivity);

        var notificationManager = NotificationManager.FromContext(context);

        CreateNotificationChannel(notificationManager, channelId);

        var intent = new Intent(context, intentType);
        intent.AddFlags(ActivityFlags.ClearTop);
        var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.OneShot);

        var notificationBuilder = new NotificationCompat.Builder(context, channelId);

        notificationBuilder.SetContentTitle(message.Title)
                    .SetSmallIcon(NotificationHub.Maui.Resource.Drawable.ic_mtrl_checked_circle)
                    .SetContentText(message.Body)
                    .SetAutoCancel(true)
                    .SetShowWhen(true)
                    .SetContentIntent(pendingIntent);


        var notification = notificationBuilder.Build();


        var nId = RandomNumberGenerator.GetInt32(10000);

        notificationManager.Notify(nId, notification);

        return Task.CompletedTask;
    }

    private void CreateNotificationChannel(NotificationManager notificationManager, string channelId)
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        {
            // Notification channels are new in API 26 (and not a part of the
            // support library). There is no need to create a notification 
            // channel on older versions of Android.
            return;
        }

        var channel = new NotificationChannel(channelId, "FCM Notifications", NotificationImportance.Default)
        {
            Description = "Firebase Cloud Messages appear in this channel"
        };

        notificationManager.CreateNotificationChannel(channel);
    }
}

