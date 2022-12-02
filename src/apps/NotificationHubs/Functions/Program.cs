using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationsAndMessaging.CoreLib.NotificationHub.Builders;
using NotificationsAndMessaging.CoreLib.NotificationHub.Builders.Interfaces;
using NotificationsAndMessaging.CoreLib.NotificationHub.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {

        services.AddScoped<INotificationPayloadBuilder, NotificationPayloadBuilder>();

        // add azure notification hub service
        services.AddScoped(sp =>
        {
            var hubName = context.Configuration.GetValue<string>("NOTIFICATION_HUB_NAME");
            var connectionString = context.Configuration.GetValue<string>("NOTIFICATION_HUB_CS");
            var client = new NotificationHubClient(connectionString, hubName);
            return new NotificationHubService(client);
        });
    })
    .Build();

host.Run();