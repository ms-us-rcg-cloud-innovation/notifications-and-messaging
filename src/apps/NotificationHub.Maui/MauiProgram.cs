
using NotificationHub.Maui.Services;

namespace NotificationHub.Maui;


public static class MauiProgram
{
	public static MauiApp CreateMauiApp(MauiAppBuilder mauiAppBuilder)
		=> mauiAppBuilder
            .UseMauiApp<App>()			
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
            .Build();

}