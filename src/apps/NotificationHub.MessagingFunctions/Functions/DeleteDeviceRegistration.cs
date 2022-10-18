using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Services;
using System.Net;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class DeleteDeviceRegistration
    {
        public record Installation(string Id);

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
                      , Route = "delete-registration")] HttpRequestData request
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
