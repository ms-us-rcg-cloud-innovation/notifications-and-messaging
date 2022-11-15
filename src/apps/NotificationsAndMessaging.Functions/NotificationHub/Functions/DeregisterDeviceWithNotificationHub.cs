using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationsAndMessaging.CoreLib.FunctionHelpers;
using NotificationsAndMessaging.CoreLib.NotificationHub.Services;
using System.Net;

namespace NotificationsAndMessaging.Functions.NotificationHub.Functions
{
    public class DeregisterDeviceWithNotificationHub
    {
        public record Installation(string Id);

        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;

        public DeregisterDeviceWithNotificationHub(ILogger<DeregisterDeviceWithNotificationHub> logger, NotificationHubService hubService)
        {
            _logger = logger;
            _hubService = hubService;
        }

        [Function(nameof(DeregisterDeviceWithNotificationHub))]
        public async Task<HttpResponseData> RunAsync(
                [HttpTrigger(
                        AuthorizationLevel.Function
                      , "post"
                      , Route = "nh/deregister-device")] HttpRequestData request
                      , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Registering device with Notification Hub");

            try
            {
                var installation = await request.ReadFromJsonAsync<Installation>();

                if (string.IsNullOrEmpty(installation.Id))
                {
                    return await request.CreateErrorResponseAsync("Registration Id is required");
                }

                await _hubService.DeleteDeviceInstallation(installation.Id, cancellationToken);

                return await request.CreateOkResponseAsync(installation.Id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during function execution time");
                return await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
