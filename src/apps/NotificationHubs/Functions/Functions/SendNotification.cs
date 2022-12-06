using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using NotificationsAndMessaging.NotificationHubs.Functions.Extensions;
using NotificationsAndMessaging.CoreLib.NotificationHub.Builders.Interfaces;
using NotificationsAndMessaging.CoreLib.NotificationHub.Services;
using System.Net;

namespace NotificationsAndMessaging.NotificationHubs.Functions
{
    public class SendNotification
    {
        private readonly ILogger _logger;
        private readonly NotificationHubService _hubService;
        private readonly INotificationPayloadBuilder _payloadBuilder;

        public record NotificationRequest(string Title, string Body, string Platform, string[] Tags, string TagExpression);

        public SendNotification(ILogger<SendNotification> logger, NotificationHubService hubService, INotificationPayloadBuilder payloadBuilder)
        {
            _hubService = hubService;
            _payloadBuilder = payloadBuilder;
            _logger = logger;
        }

        [Function(nameof(SendNotification))]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(
                AuthorizationLevel.Function
              , "post"
              , Route = "nh/send-notification")] HttpRequestData request
              , CancellationToken cancellationToken)
        {
            _logger.LogInformation("Sending notification to targeted audiance");

            try
            {
                var notification = await request.ReadFromJsonAsync<NotificationRequest>();

                if (notification is null)
                {
                    return await request.CreateErrorResponseAsync("Incorrect notification message format");
                }
                bool validPlatform = RequestHelpers.PlatformEnumLookup.TryGetValue(notification.Platform.ToLower(), out var platform);

                if (!validPlatform)
                {

                    var message = $"Platform is invalid {notification.Platform}";
                    _logger.LogError(message);
                    return await request.CreateErrorResponseAsync(message);
                }

                var notificationPayload = CreateRawPayload(platform, notification.Title, notification.Body);
                var outcome = await _hubService.SendNotificationAsync(platform
                                                                    , notificationPayload
                                                                    , notification.Tags
                                                                    , notification.TagExpression
                                                                    , cancellationToken);

                _logger.LogInformation("Message sent to Notification Hub");

                return await request.CreateOkResponseAsync(outcome);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error during function execution time");
                return await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError);
            }
        }


        private string CreateRawPayload(NotificationPlatform platform, string title, string body)
        {
            _payloadBuilder
                .AddTitle(title)
                .AddBody(body);

            switch (platform)
            {
                case NotificationPlatform.Fcm:
                default:
                    return _payloadBuilder.BuildAndroidPayload();
            }
        }
    }
}
