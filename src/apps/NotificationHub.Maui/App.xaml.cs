using NotificationHub.Maui.Models;
using NotificationHub.Maui.Services;
using Plugin.FirebasePushNotification;

namespace NotificationHub.Maui;

public partial class App : Application
{
    private readonly INotificationHandler _notificationHandler;

    public App(INotificationHandler notificationHandler)
    {
        _notificationHandler = notificationHandler;
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override void OnStart()
    {
        base.OnStart();

        CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
        {
            var message = new NotificationMessage
            {
                Title = (string)p.Data["title"],
                Body = (string)p.Data["body"],
                Data = p.Data,
                TimeStamp = DateTime.Now
            };

            _notificationHandler.ReceiveNotificationAsync(message);

            System.Diagnostics.Debug.WriteLine("Received");

        };
    }
}
