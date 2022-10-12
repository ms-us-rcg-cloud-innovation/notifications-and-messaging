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
            //var channel = AzNH.NotificationHub.PushChannel;
            // Set the delegate for receiving messages
            AzNH.NotificationHub.SetListener(new NotificationListener());            
            //AzNH.NotificationHub.SetUserId("sbanjanovic@microsoft.com");
            //AzNH.NotificationHub.AddTag("sbanj");

            AzNH.NotificationHub.Start(Application, _hubName, _hubConnectionString);


        } 
    }
}
