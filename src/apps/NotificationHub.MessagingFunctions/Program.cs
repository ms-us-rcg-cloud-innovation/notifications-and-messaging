using Azure.Communication.Sms;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationHub.Core.Builders;
using NotificationHub.Core.Builders.Interfaces;
using NotificationHub.Core.Services;
using NotificationHub.MessagingFunctions;

CosmosClient cosmosClient = default;
var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        cosmosClient = new CosmosClient(context.Configuration.GetConnectionString(ConfigurationConstants.COSMOS_CONNECTIONSTRING_CONFIG_KEY));
        services.AddScoped<INotificationPayloadBuilder, NotificationPayloadBuilder>();

        // add azure notification hub service
        services.AddScoped(sp =>
        {
            var hubName = context.Configuration.GetValue<string>("NOTIFICATION_HUB_NAME");
            var connectionString = context.Configuration.GetConnectionString("NOTIFICATION_HUB_CS");

            var client = new NotificationHubClient(connectionString, hubName);

            return new NotificationHubService(client);
        });
    })
    .Build();

host.Run();