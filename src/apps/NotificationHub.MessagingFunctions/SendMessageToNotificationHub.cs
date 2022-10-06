using System.Collections.Generic;
using System.Net;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.Providers;
using NotificationHub.Core.Providers.Models;

namespace NotificationHub.MessagingFunctions
{
    public class SendMessageToNotificationHub
    {
        private readonly AzureNotificationProvider _notificationProvider;
        private readonly ILogger _logger;

        public SendMessageToNotificationHub(ILoggerFactory loggerFactory, AzureNotificationProvider notificationProvider)
        {
            _notificationProvider = notificationProvider;
            _logger = loggerFactory.CreateLogger<SendMessageToNotificationHub>();
        }

        [Function(nameof(SendMessageToNotificationHub))]
        public async Task<HttpResponseData> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData request)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            var response = request.CreateResponse();
            try
            {
                Notification? notification = await request.ReadFromJsonAsync<Notification>();



                var outcome = await _notificationProvider.SendNotification(notification);

                response.StatusCode = HttpStatusCode.OK;
                await response.WriteAsJsonAsync(outcome);
                
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message);
            }

            return response;
        }
    }
}
