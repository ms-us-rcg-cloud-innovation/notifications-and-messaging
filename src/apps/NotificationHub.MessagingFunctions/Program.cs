using Microsoft.Extensions.Hosting;
using NotificationHub.Core.Builders.Interfaces;
using NotificationHub.Core.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.NotificationHubs;
using NotificationHub.Core.Services;
using Azure.Communication.Sms;
using Microsoft.Extensions.Azure;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {

        services.AddScoped<INotificationPayloadBuilder, NotificationPayloadBuilder>();

        // add azure notification hub service
        services.AddScoped(sp =>
        {
            var hubName = context.Configuration.GetValue<string>("NOTIFICATION_HUB_NAME");
            var connectionString = context.Configuration.GetConnectionString("NOTIFICATION_HUB_CS");

            var client = new NotificationHubClient(connectionString, hubName);

            return new NotificationHubService(client);
        });

        // add azure communication service
        services.AddScoped(sp =>
        {
            var connectionString = context.Configuration.GetConnectionString("AZURE_COMM_SERVICES_CS");

            var client = new SmsClient(connectionString);

            return new SmsCommunicationService(client);
        });
    })
    .Build();

host.Run();