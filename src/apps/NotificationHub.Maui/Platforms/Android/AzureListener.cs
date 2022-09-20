using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.App;
using WindowsAzure.Messaging.NotificationHubs;
using AndroidX.Core.App;
using Application = Microsoft.Maui.Controls.Application;

namespace NotificationHub.Maui.Platforms.Android;

public class AzureListener: Java.Lang.Object, INotificationListener
{
    public void OnPushNotificationReceived(Context context, INotificationMessage message)
    {
        var intent = new Intent(context, typeof(MainActivity));
        intent.AddFlags(ActivityFlags.ClearTop);
        var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.OneShot);

        var notificationBuilder = new NotificationCompat.Builder(context, MainActivity.CHANNEL_ID);

        notificationBuilder.SetContentTitle(message.Title)
                    .SetSmallIcon(Resource.Drawable.ic_launcher)
                    .SetContentText(message.Body)
                    .SetAutoCancel(true)
                    .SetShowWhen(true)
                    .SetContentIntent(pendingIntent);


        var notification = notificationBuilder.Build();
        var notificationManager = NotificationManager.FromContext(context);

        notificationManager.Notify(42, notification);
    }
}
