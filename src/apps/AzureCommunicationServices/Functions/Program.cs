using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices((context, services) =>
    {
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
