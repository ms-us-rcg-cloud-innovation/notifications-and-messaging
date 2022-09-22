using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Firebase;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Platforms.Android.Helpers;
using NotificationHub.Maui.Services;
using Plugin.FirebasePushNotification;
using System.Security.Cryptography;

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
        var mauiAppBuilder = MauiApp.CreateBuilder();

        mauiAppBuilder.Services
            .AddScoped<INotificationHandler, DefaultAndroidNotificationHandler>();

        return MauiProgram.CreateMauiApp(mauiAppBuilder);
    }
}
