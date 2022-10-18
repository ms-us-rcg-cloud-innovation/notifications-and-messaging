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
        private static readonly Dictionary<string, NotificationPlatform> _platformEnumLookup = new()
        {
            {"fcm", NotificationPlatform.Fcm },
            {"gcm", NotificationPlatform.Fcm },
            {"apns", NotificationPlatform.Apns }
        };

        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;

        public record DeviceDetails(string InstallationId, string PushChannel, string Platform, IList<string> Tags);

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
                bool validPlatform = _platformEnumLookup.TryGetValue(deviceDetails.Platform.ToLower(), out var platform);

                if (deviceDetails is null || !validPlatform)
                {

                    var message = "Invalid device details." + (!validPlatform ? $" {deviceDetails.Platform} is not a valid Platform" : "");
                    _logger.LogError(message);
                    return await request.CreateErrorResponseAsync(message);
                }

               

                if(validPlatform)
                {
                    await _hubService.UpsertDeviceRegistrationAsync(
                                                deviceDetails.InstallationId
                                              , deviceDetails.PushChannel
                                              , platform
                                              , cancellationToken
                                              , tags: deviceDetails.Tags);
                }

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
