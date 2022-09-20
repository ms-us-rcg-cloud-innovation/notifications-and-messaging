using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using AndroidX.Core.App;
using Firebase;
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

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
