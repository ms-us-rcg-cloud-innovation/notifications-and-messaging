
using NotificationHub.Maui.Services;
using NotificationHub.Maui.ViewModels;

#if ANDROID
using NotificationHub.Maui.Platforms.Android;
#endif

namespace NotificationHub.Maui;


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
			.RegisterViewModels()
            .Build();


	public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
	{
		mauiAppBuilder.Services.AddScoped<MainPageViewModel>(sp => new MainPageViewModel());

		return mauiAppBuilder;
	}
}