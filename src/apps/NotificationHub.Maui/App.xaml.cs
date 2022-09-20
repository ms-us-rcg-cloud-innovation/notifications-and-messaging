using Plugin.FirebasePushNotification;

namespace NotificationHub.Maui;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
        {
            System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
        };
        // Push message received event
        CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
        {

            System.Diagnostics.Debug.WriteLine("Received");

        };
        //Push message received event
        CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
        {
            System.Diagnostics.Debug.WriteLine("Opened");
            foreach (var data in p.Data)
            {
                System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
            }

        };

        MainPage = new AppShell();
    }
}
