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
        var appBuilder = MauiApp.CreateBuilder();
        appBuilder.Services.AddScoped<IDeviceInstallationService, AndroidDeviceInstallationService>();

        return MauiProgram.CreateMauiApp(appBuilder);
    }

}
