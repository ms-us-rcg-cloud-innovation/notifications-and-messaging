using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationsAndMessaging.NotificationHubs.Functions.Extensions;
using NotificationsAndMessaging.CoreLib.NotificationHub.Services;
using System.Net;

namespace NotificationsAndMessaging.NotificationHubs.Functions
{
    public class RegisterDevice
    {
        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;

        public record DeviceDetails(string Id, string PushChannel, string Platform, IList<string> Tags);

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
              , Route = "nh/register-device")] HttpRequestData request
              , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering device with Notification Hub");

            try
            {
                var deviceDetails = await request.ReadFromJsonAsync<DeviceDetails>();
                bool validPlatform = RequestHelpers.PlatformEnumLookup.TryGetValue(deviceDetails.Platform.ToLower(), out var platform);

                if (deviceDetails is null || !validPlatform)
                {

                    var message = "Invalid device details." + (!validPlatform ? $" {deviceDetails.Platform} is not a valid Platform" : "");
                    _logger.LogError(message);
                    return await request.CreateErrorResponseAsync(message);
                }

                if (validPlatform)
                {
                    await _hubService.UpsertDeviceRegistrationAsync(
                                                deviceDetails.Id
                                              , deviceDetails.PushChannel
                                              , platform
                                              , cancellationToken
                                              , tags: deviceDetails.Tags);
                }

                return await request.CreateOkResponseAsync(deviceDetails);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during function execution time");
                return await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
