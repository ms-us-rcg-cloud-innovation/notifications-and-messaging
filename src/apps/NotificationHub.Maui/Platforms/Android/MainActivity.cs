using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
//using Plugin.FirebasePushNotification;
using System.Security.Cryptography;
using WindowsAzure.Messaging;
using WindowsAzure.Messaging.NotificationHubs;
using AzNH = WindowsAzure.Messaging.NotificationHubs;


namespace NotificationHub.Maui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // local constants is a git ignored file
            // create a class with those constants corresponding to your hub details
            AzNH.NotificationHub.Start(this.Application, Local_Constants.HUB_NAME, Local_Constants.HUB_CONNECTIONSTRING);

            AzNH.NotificationHub.SetUserId("ted@contoso.com");
            AzNH.NotificationHub.AddTags(new List<string> { "secret_demo", "tooling", "notify_me" });
        }
    }
}
