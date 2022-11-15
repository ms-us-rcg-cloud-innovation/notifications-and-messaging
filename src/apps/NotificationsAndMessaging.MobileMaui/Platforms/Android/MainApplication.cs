using Android.App;
using Android.Runtime;
using NotificationsAndMessaging.MobileMaui.Platforms.Android.Services;
using NotificationsAndMessaging.MobileMaui.Platforms.Android.Services.Impl;
using NotificationsAndMessaging.MobileMaui.Services.NotificationHubFuns;
using Plugin.FirebasePushNotification;
using AzNH = WindowsAzure.Messaging.NotificationHubs;


namespace NotificationsAndMessaging.MobileMaui.Platforms.Android;

[Application]
public class MainApplication : MauiApplication
{
    public MainApplication(IntPtr handle, JniHandleOwnership ownership)
        : base(handle, ownership)
    {

    }

    protected override MauiApp CreateMauiApp()
    {
        var appBuilder = MauiApp.CreateBuilder();
        appBuilder.Services.AddScoped<IDeviceInstallationService, AndroidDeviceInstallationService>();

        return MauiProgram.CreateMauiApp(appBuilder);
    }

}
