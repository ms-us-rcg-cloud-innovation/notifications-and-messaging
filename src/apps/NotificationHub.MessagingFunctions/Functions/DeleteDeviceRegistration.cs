using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Services;
using static NotificationHub.MessagingFunctions.Functions.RegisterDevice;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class DeleteDeviceRegistration
    {
        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;

        public DeleteDeviceRegistration(ILogger<DeleteDeviceRegistration> logger, NotificationHubService hubService)
        {
            _logger = logger;
            _hubService = hubService;
        }

        [Function(nameof(DeleteDeviceRegistration))]
        public async Task<HttpResponseData> RunAsync(
                [HttpTrigger(
                        AuthorizationLevel.Function
                      , "post"
                      , Route = "delete-registration/{id}")] HttpRequestData request
                      , CancellationToken cancellationToken
                      , string id)
        {
            _logger.LogInformation("Registering device with Notification Hub");

            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return await request.CreateErrorResponseAsync("Registration Id is required");
                }

                await _hubService.DeleteDeviceInstallation(id, cancellationToken);

                return await request.CreateOkResponseAsync(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during function execution time");
                return await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
