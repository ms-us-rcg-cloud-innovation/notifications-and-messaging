using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationHub.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(NotificationFunctions.Startup))]

namespace NotificationFunctions
{
    public class Startup
        : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddAzureAppConfiguration();

            builder.Services.AddScoped(sp =>
            {
                var hubName = builder.GetContext().Configuration.GetValue<string>("NOTIFICATION_HUB_NAME");
                var connectionString = builder.GetContext().Configuration.GetValue<string>("NOTIFICATION_HUB_CS");

                var client = new NotificationHubClient(connectionString, hubName);

                return new NotificationHubService(client);
            });
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);

            builder.ConfigurationBuilder.AddEnvironmentVariables();
        }
    }
}
