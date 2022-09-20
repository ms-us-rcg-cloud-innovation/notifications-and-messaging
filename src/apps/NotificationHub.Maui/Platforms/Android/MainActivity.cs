using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using Plugin.FirebasePushNotification;
using WindowsAzure.Messaging;
using WindowsAzure.Messaging.NotificationHubs;
using NH = NotificationHub.Core.Maui.Constants;

namespace NotificationHub.Maui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static readonly string CHANNEL_ID = "my_notification_channel";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
           
            WindowsAzure.Messaging.NotificationHubs.NotificationHub.SetListener(new AzureListener());
            WindowsAzure.Messaging.NotificationHubs.NotificationHub.Start(this.Application, NH.NotificationHubName, NH.ListenConnectionString);
        }
    }
}
