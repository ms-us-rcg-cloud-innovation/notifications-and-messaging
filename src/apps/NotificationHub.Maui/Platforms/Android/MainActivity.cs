using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using Firebase;
using NotificationHub.Core.Maui.Platforms.Android.Services;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Services;
using Plugin.FirebasePushNotification;
using System.Security.Cryptography;
using WindowsAzure.Messaging;
using WindowsAzure.Messaging.NotificationHubs;

namespace NotificationHub.Maui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static readonly string CHANNEL_ID = "notification_hub_channel";

        public MainActivity()
        {

        }

        //protected override void OnCreate(Bundle savedInstanceState)
        //{
        //    base.OnCreate(savedInstanceState);

        //    CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
        //    {                
        //        var message = new NotificationMessage
        //        {
        //            Title = (string)p.Data["title"],
        //            Body = (string)p.Data["body"],
        //            Data = p.Data,
        //            TimeStamp = DateTime.Now
        //        };

        //        var properties = new Dictionary<string, object>()
        //        {
        //            { "context", this },
        //            { "channel", CHANNEL_ID },
        //            { "intent", typeof(MainActivity) }
        //        };

        //        _notificationHandler.ReceiveNotificationAsync(message, properties); 

        //        System.Diagnostics.Debug.WriteLine("Received");

        //    };

        //}  
    }
}
