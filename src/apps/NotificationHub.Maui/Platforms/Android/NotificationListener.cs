using Android.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsAzure.Messaging.NotificationHubs;

namespace NotificationHub.Maui.Platforms.Android
{
    public class NotificationListener
        : Java.Lang.Object
        , INotificationListener
    {
        public void OnPushNotificationReceived(Context context, INotificationMessage message)
        {
            Console.WriteLine($"Message received with title {message.Title} and body {message.Body}");
        }
    }
}
