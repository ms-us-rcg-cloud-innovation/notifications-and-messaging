using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Services;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class RegisterDevice
    {
        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;

        public record DeviceDetails(string Id, string Channel, NotificationPlatform Platform, IList<string> Tags);

        public RegisterDevice(ILogger<RegisterDevice> logger, NotificationHubService hubService)
        {
            _logger = logger;
            _hubService = hubService;
        }

        [Function(nameof(RegisterDevice))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(
                AuthorizationLevel.Function
              , "post"
              , Route = "register-device")] HttpRequestData request
              , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering device with Notification Hub");

            try
            {
                var deviceDetails = await request.ReadFromJsonAsync<DeviceDetails>();

                if (deviceDetails is null)
                {
                    return await request.CreateErrorResponseAsync("Invalid device details provided");
                }

                await _hubService.UpsertDeviceRegistrationAsync(deviceDetails.Id
                                                              , deviceDetails.Channel
                                                              , deviceDetails.Platform
                                                              , cancellationToken
                                                              , tags: deviceDetails.Tags);

                return await request.CreateOkResponseAsync(deviceDetails);
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error during function execution time");
                return await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
