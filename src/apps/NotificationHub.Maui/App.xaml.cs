using NotificationHub.Maui.Models;
using NotificationHub.Maui.Services;
using NotificationHub.Maui.ViewModels;
using Plugin.FirebasePushNotification;
using System.Diagnostics;

namespace NotificationHub.Maui;

public partial class App : Application
{
    public App(MainPageViewModel mainPageViewModel)
    {
        InitializeComponent();

        MainPage = new AppShell();
        MainPage.BindingContext = mainPageViewModel;
    }
}
