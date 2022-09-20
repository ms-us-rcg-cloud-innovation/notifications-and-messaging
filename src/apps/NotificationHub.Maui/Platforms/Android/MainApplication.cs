using Android.App;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Firebase;
using Plugin.FirebasePushNotification;
using NH = NotificationHub.Core.Maui.Constants;

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
