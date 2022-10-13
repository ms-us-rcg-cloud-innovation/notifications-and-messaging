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
//using Plugin.FirebasePushNotification;
using System.Security.Cryptography;
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

        // local constants is a git ignored file
        // create a class with those constants corresponding to your hub details
        AzNH.NotificationHub.Start(this, Local_Constants.HUB_NAME, Local_Constants.HUB_CONNECTIONSTRING);

        AzNH.NotificationHub.SetUserId("ted@contoso.com");

        var tags = AzNH.NotificationHub.Tags.ToEnumerable<string>();

        if (tags.Count() == 0)
        {
            AzNH.NotificationHub.AddTags(new List<string> { "secret_demo", "tooling", "notify_me" });
        }

        return MauiProgram.CreateMauiApp();
    }
}
