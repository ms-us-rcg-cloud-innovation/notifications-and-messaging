using NotificationsAndMessaging.MobileMaui.Services.NotificationHubFuns;
using NotificationsAndMessaging.MobileMaui.ViewModels;

#if ANDROID
using NotificationsAndMessaging.MobileMaui.Platforms.Android;
#endif

namespace NotificationsAndMessaging.MobileMaui;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp(MauiAppBuilder appBuilder)
        => appBuilder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .RegisterServices()
            .Build();


    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services
            .AddScoped<MainPageViewModel>()
            .AddScoped<DeviceRegistrationService>()
            .AddSingleton<HttpClient>(sp =>
            {
                HttpClient client = new();
                client.BaseAddress = new Uri(Local_Constants.NH_REGISTRATION_UPSERT_ENDPOINT);

                return client;
            });

        return mauiAppBuilder;
    }
}