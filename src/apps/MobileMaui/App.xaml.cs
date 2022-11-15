using NotificationsAndMessaging.MobileMaui.ViewModels;
//using Plugin.FirebasePushNotification;

namespace NotificationsAndMessaging.MobileMaui;

public partial class App : Application
{
    public App(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();

        MainPage = new AppShell();
        MainPage.BindingContext = mainPageViewModel;

    }
}
