using System.Collections.Generic;
using System.Net;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Providers;
using NotificationHub.Core.Models;
using NotificationHub.MessagingFunctions.Models;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class PersistNotificationRequest
    {
        private readonly AzureNotificationProvider _notificationProvider;
        private readonly ILogger _logger;

        public PersistNotificationRequest(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<PersistNotificationRequest>();
        }

        [Function(nameof(PersistNotificationRequest))]
        public async Task<SendMessageResponseModel> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function
                         , "post"
                         , Route = "submit-request")] HttpRequestData request)
        {
            _logger.LogInformation("Http triggered function for processing notification submission");
            try
            {
                var notification = await request.ReadFromJsonAsync<Notification>();

                if (notification is null)
                {
                    return new(await request.CreateErrorResponseAsync("No content in request"));
                }

                return new(await request.CreateOkResponseAsync("Messaged persisted"), notification);
            }
            catch (Exception e)
            {
                return new(await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError));
            }
        }
    }
}
