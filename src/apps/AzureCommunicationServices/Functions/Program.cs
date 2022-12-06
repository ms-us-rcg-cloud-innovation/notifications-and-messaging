using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()    
    .ConfigureServices((context, services) =>
    {
        services.AddScoped(sp =>
        {
            var acsConnectionString = context.Configuration.GetValue<string>("ACS_CONNECTION_STRING");
            var emailClient = new EmailClient(acsConnectionString);

            return emailClient;
        });        
    })
    .Build();

host.Run();
