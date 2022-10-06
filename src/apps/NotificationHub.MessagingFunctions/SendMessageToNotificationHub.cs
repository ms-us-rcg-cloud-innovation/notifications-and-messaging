using System.Collections.Generic;
using System.Net;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Providers;
using NotificationHub.Core.Models;

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

            try
            {
                Notification? notification = await request.ReadFromJsonAsync<Notification>();

                if(notification is null)
                {
                    return await request.CreateErrorResponseAsync("No content in request");
                }

                var outcome = await _notificationProvider.SendNotification(notification);

                return await request.CreateOkResponseAsync(outcome);
                
            }
            catch (Exception e)
            {
                return await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError);
            }
        }
    }
}
