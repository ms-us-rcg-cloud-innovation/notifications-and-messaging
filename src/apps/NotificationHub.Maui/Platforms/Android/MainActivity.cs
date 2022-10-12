using Android.App;
using Android.Content;
using Android.Content.PM;
//using Plugin.FirebasePushNotification;
using System.Security.Cryptography;
using WindowsAzure.Messaging;
using AzNH = WindowsAzure.Messaging.NotificationHubs;


namespace NotificationHub.Maui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        private string _hubConnectionString = "Endpoint=sb://rcgnsdefault.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=1F91fhCXulrcFLeTkwOqbY9Dxh6NMcEUYFkthtDFEYU=";
        private string _hubName = "rcg";

        public MainActivity()
        {
            //AzNH.NotificationHub.Start(this.Application, _hubName, _hubConnectionString);
        } 
    }
}
