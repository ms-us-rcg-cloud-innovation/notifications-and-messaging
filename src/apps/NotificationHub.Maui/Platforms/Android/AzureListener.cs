using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.App;
using WindowsAzure.Messaging.NotificationHubs;
using AndroidX.Core.App;

namespace NotificationHub.Maui.Platforms.Android;

public class AzureListener: Java.Lang.Object, INotificationListener
{
    public void OnPushNotificationReceived(Context context, INotificationMessage message)
    {
        var intent = new Intent(MainApplication.Context, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.ClearTop);
        var pendingIntent = PendingIntent.GetActivity(MainApplication.Context, 0, intent, PendingIntentFlags.OneShot);

        var notificationBuilder = new NotificationCompat.Builder(MainApplication.Context, MainActivity.CHANNEL_ID);

        notificationBuilder.SetContentTitle(message.Title)
                    .SetSmallIcon(Resource.Drawable.ic_launcher)
                    .SetContentText(message.Body)
                    .SetAutoCancel(true)
                    .SetShowWhen(false)                   
                    .SetContentIntent(pendingIntent);

        var notificationManager = NotificationManager.FromContext(MainApplication.Context);

        notificationManager.Notify(0, notificationBuilder.Build());
    }
}
