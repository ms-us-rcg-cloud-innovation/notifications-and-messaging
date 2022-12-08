using Azure.Communication.Email;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
        // add cosmos client for message store
        services.AddSingleton(sp =>
        {
            var connectinString = context.Configuration.GetValue<string>("COSMOS_CONNECTION_STRING");
            CosmosClient cosmosClient = new(connectinString);

            return cosmosClient;
        });

        // add azure communication services connection string
        services.AddSingleton(sp =>
        {
            var acsConnectionString = context.Configuration.GetValue<string>("ACS_CONNECTION_STRING");
            EmailClient emailClient = new(acsConnectionString);

            return emailClient;
        });
    })
    .Build();

host.Run();
