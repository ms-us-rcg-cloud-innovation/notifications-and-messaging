using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common.ModuleInstall.Internal;
using Android.OS;
using AndroidX.Core.App;
//using Plugin.FirebasePushNotification;
using System.Security.Cryptography;
using WindowsAzure.Messaging;
using WindowsAzure.Messaging.NotificationHubs;
using AzNH = WindowsAzure.Messaging.NotificationHubs;


namespace NotificationsAndMessaging.MobileMaui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        
    }
}
