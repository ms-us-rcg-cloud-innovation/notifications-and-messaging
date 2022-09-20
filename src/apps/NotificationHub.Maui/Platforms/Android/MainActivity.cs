using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using Firebase;
using Plugin.FirebasePushNotification;
using System.Security.Cryptography;
using WindowsAzure.Messaging;
using WindowsAzure.Messaging.NotificationHubs;
using NH = NotificationHub.Core.Maui.Constants;

namespace NotificationHub.Maui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public static readonly string CHANNEL_ID = "notification_hub_channel";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
            };
            // Push message received event
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                var notificationManager = NotificationManager.FromContext(this);

                CreateNotificationChannel(notificationManager);

                var intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.ClearTop);
                var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);

                var notificationBuilder = new NotificationCompat.Builder(this, CHANNEL_ID);

                notificationBuilder.SetContentTitle((string)p.Data["title"])
                            .SetSmallIcon(Resource.Drawable.ic_launcher)
                            .SetContentText((string)p.Data["body"])
                            .SetAutoCancel(true)
                            .SetShowWhen(true)
                            .SetContentIntent(pendingIntent);


                var notification = notificationBuilder.Build();
                

                var nId = RandomNumberGenerator.GetInt32(10000);

                notificationManager.Notify(nId, notification);

                System.Diagnostics.Debug.WriteLine("Received");

            };
            //Push message received event
            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                System.Diagnostics.Debug.WriteLine("Opened");
                foreach (var data in p.Data)
                {
                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
                }

            };
        }  

        private void CreateNotificationChannel(NotificationManager notificationManager)
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.O)
            {
                // Notification channels are new in API 26 (and not a part of the
                // support library). There is no need to create a notification 
                // channel on older versions of Android.
                return;
            }

            var channel = new NotificationChannel(CHANNEL_ID, "FCM Notifications", NotificationImportance.Default)
            {
                Description = "Firebase Cloud Messages appear in this channel"
            };

            notificationManager.CreateNotificationChannel(channel);
        }
    }
}
