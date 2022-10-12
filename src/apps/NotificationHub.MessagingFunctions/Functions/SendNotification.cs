using System.Collections.Generic;
using System.Net;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.Builders.Interfaces;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Models;
using NotificationHub.Core.Services;
using static NotificationHub.MessagingFunctions.Functions.OrderRequestService;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class SendNotification
    {
        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;
        private readonly INotificationPayloadBuilder _payloadBuilder;

        public SendNotification(ILogger<SendNotification> logger, NotificationHubService hubService, INotificationPayloadBuilder payloadBuilder)
        {
            _hubService = hubService;
            _payloadBuilder = payloadBuilder;      
            _logger = logger;
        }

        [Function(nameof(SendNotification))]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function
              , "post"
              , Route = "send-notification")] HttpRequestData request)
        {
            _logger.LogInformation("Triggering send-notification endpoint");

            try
            {
                var notification = await request.ReadFromJsonAsync<PushNotification>();

                if (notification is null)
                {
                    return await request.CreateErrorResponseAsync("No content in request");
                }

                var notificationPayload = CreateRawPayload(notification);
                var outcome = await _hubService.SendNotificationAsync(notification.Platform, notificationPayload, notification.Tags);

                _logger.LogInformation("Message sent to Notification Hub");

                return await request.CreateOkResponseAsync(outcome);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during function execution time");
                return await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError);
            }
        }


        private string CreateRawPayload(PushNotification notification)
        {
            _payloadBuilder
                .AddTitle(notification.Title)
                .AddBody(notification.Body);

            switch (notification.Platform)
            {
                case "fcm":
                    return _payloadBuilder.BuildAndroidPayload();
                case "aps":
                    return _payloadBuilder.BuildApplePayload();
                default:
                    throw new Exception("Invalid platform");
            }
        }
    }
}
