using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using Firebase;
using NotificationHub.Core.Maui.Platforms.Android.Services;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Services;
using Plugin.FirebasePushNotification;
using System.Security.Cryptography;
using WindowsAzure.Messaging;
using WindowsAzure.Messaging.NotificationHubs;

namespace NotificationHub.Maui.Platforms.Android
{
    [Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    public class MainActivity : MauiAppCompatActivity
    {
        public MainActivity()
        {

        } 
    }
}
