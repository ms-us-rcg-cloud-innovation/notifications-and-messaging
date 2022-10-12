using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using AndroidX.Core.App;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Platforms.Android;
using NotificationHub.Maui.Platforms.Android.Helpers;
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
    public event EventHandler<NotificationEventArgs> NotificationTapped;

    public DefaultAndroidNotificationHandler()
    {
        CrossFirebasePushNotification.Current.RegisterForPushNotifications();

        CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
        {
            var message = new NotificationMessage
            {
                Title = (string)p.Data["title"],
                Body = (string)p.Data["body"],
                Data = p.Data,
                TimeStamp = DateTime.Now
            };

            // capture notification while app is active 
            NotificationHelpers.RaiseSystemNotificationWhileInForeground(message);

            // raise an abstracted event to the cross platform system
            OnNotificationReceived(new(message, "Android"));
        };
    }

    protected void OnNotificationReceived(NotificationEventArgs e)
    {
        var handler = NotificationReceived;
        handler?.Invoke(this, e);
    }
}

