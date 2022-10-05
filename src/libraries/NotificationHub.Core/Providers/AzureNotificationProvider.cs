using NotificationHub.Api.Builders.Interfaces;
using NotificationHub.Core.Providers.Models;
using NotificationHub.Core.Services;

namespace NotificationHub.Core.Providers
{
    public class AzureNotificationProvider
    {
        private readonly NotificationHubService _hubService;
        private readonly INotificationPayloadBuilder _payloadBuilder;

        public AzureNotificationProvider(NotificationHubService hubService, INotificationPayloadBuilder payloadBuilder)
        {
            _hubService = hubService;
            _payloadBuilder = payloadBuilder;
        }

        public async Task<Microsoft.Azure.NotificationHubs.NotificationOutcome> SendNotification(Notification notification)
            => await _hubService.SendNotification(notification.Platform, CreateRawPayload(notification));
        

        private string CreateRawPayload(Notification notification)
        {
            _payloadBuilder
                .AddTitle(notification.Title)
                .AddBody(notification.Message);

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
