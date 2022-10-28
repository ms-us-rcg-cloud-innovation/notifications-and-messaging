using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Models;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class SaveDeviceInformation
    {
        public record DeviceDetails(string DeviceId, string DeviceType, string OS, string OSVersion, string PNSPlatform, string PNSHandle, List<string> Tags);

        public record MultiResponse(
                            [property:CosmosDBOutput(
                                databaseName: $"%{ConfigurationConstants.COSMOS_DB_CONFIG_KEY}%"
                              , collectionName: $"%{ConfigurationConstants.COSMOS_DEVICES_CONTAINER_CONFIG_KEY}%"
                              , ConnectionStringSetting = ConfigurationConstants.COSMOS_CONNECTIONSTRING_CONFIG_KEY
                              , PartitionKey = ConfigurationConstants.DEVICES_CONTAINER_PARTITIONKEY
                              , CreateIfNotExists = true)]
                            DeviceInformation DeviceInfo
                          , HttpResponseData ResponseData);



        private readonly ILogger _logger;

        public SaveDeviceInformation(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveDeviceInformation>();
        }

        [Function(nameof(SaveDeviceInformation))]
        /// cosmos db output binding is on the DeviceInformation
        /// See: https://learn.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide#bindings
        public async Task<MultiResponse> RunAsync(
            [HttpTrigger(
                AuthorizationLevel.Function
              , "post"
              , Route = "save-device-information")] HttpRequestData request
              , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Saving DeviceInformation");

            var device = await request.ReadFromJsonAsync<DeviceDetails>(cancellationToken);
            DeviceInformation deviceInformation = new()
            {
                Id = Guid.NewGuid().ToString(),
                RegistrationDate = DateTime.Now,
                LastUpdatedHandleDate = DateTime.Now,
                OS = device.OS,
                OSVersion = device.OSVersion,
                PNSHandle = device.PNSHandle,
                PNSPlatform = device.PNSPlatform,
                DeviceId = device.DeviceId,
                DeviceType = device.DeviceType
            };

            if (device.Tags?.Count > 0)
            {
                foreach (var tag in device.Tags)
                {
                    deviceInformation.Tags.Add(tag);
                }
            }

            MultiResponse response = new(
                DeviceInfo: deviceInformation
              , ResponseData: await request.CreateOkResponseAsync(deviceInformation, cancellationToken));

            return response;
        }
    }
}

