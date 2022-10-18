using Android.App;
using Android.Runtime;
using NotificationHub.Maui.Platforms.Android.Services;
using NotificationHub.Maui.Platforms.Android.Services.Impl;
using NotificationHub.Maui.Services;
using Plugin.FirebasePushNotification;
using AzNH = WindowsAzure.Messaging.NotificationHubs;


namespace NotificationHub.Maui.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {

    }

    protected override MauiApp CreateMauiApp()
    {
//        //If debug you should reset the token each time.
//#if DEBUG
//        FirebasePushNotificationManager.Initialize(this, true);
//#else
//        FirebasePushNotificationManager.Initialize(this,false);
//#endif
//        // initialize after refreshing refresh token
//        AzNH.NotificationHub.Start(MainApplication.Current, Local_Constants.HUB_NAME, Local_Constants.HUB_CONNECTIONSTRING);

        var appBuilder = MauiApp.CreateBuilder();
        appBuilder.Services.AddScoped<IDeviceInstallationService, AndroidDeviceInstallationService>();
        return MauiProgram.CreateMauiApp(appBuilder);
    }

}
