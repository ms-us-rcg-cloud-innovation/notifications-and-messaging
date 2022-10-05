using Microsoft.Extensions.Hosting;
using NotificationHub.Core.Builders.Interfaces;
using NotificationHub.Core.Builders;
using NotificationHub.Core.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.NotificationHubs;
using NotificationHub.Core.Services;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        services.AddScoped<AzureNotificationProvider>();
        services.AddScoped<INotificationPayloadBuilder, NotificationPayloadBuilder>();

        services.AddScoped(sp =>
        {
            // Retrieve values from configuration provider -- ours are stored in environment 
            // variables configured in our launchSettings.json (which is also in our .gitignore)
            var hubName = context.Configuration.GetValue<string>("NOTIFICATION_HUB_NAME");
            var connectionString = context.Configuration.GetValue<string>("NOTIFICATION_HUB_CS");

            var client = new NotificationHubClient(connectionString, hubName);

            return new NotificationHubService(client);
        });
    })
    .Build();

host.Run();