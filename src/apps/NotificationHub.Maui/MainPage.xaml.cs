using NotificationHub.Maui.Models;
using Plugin.FirebasePushNotification;

namespace NotificationHub.Maui;

public partial class MainPage : ContentPage
{
    int count = 0;

    public MainPage()
    {
        InitializeComponent();

//#if ANDROID
//        CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
//        {
//            var message = new NotificationMessage
//            {
//                Title = (string)p.Data["title"],
//                Body = (string)p.Data["body"],
//                Data = p.Data,
//                TimeStamp = DateTime.Now
//            };

//            SetNotificationText(message);
//        };
//#endif

    }

    private void OnCounterClicked(object sender, EventArgs e)
    {
        count++;


        if (count == 1)
            CounterBtn.Text = $"Clicked {count} time";
        else
            CounterBtn.Text = $"Clicked {count} times";

        SemanticScreenReader.Announce(CounterBtn.Text);
    }
}

