using NotificationHub.Api.Builders.Interfaces;
using NotificationHub.Api.Models;
using NotificationHub.Api.Services;

namespace NotificationHub.Api.Providers
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
        {
            string payload = null;

            _payloadBuilder
                .AddTitle(notification.Title)
                .AddBody(notification.Message);

            switch(notification.Platform)
            {
                case "fcm":
                    payload = _payloadBuilder.BuildAndroidPayload();
                    break;
                case "aps":
                    payload = _payloadBuilder.BuildApplePayload();
                    break;
                default:
                    throw new Exception("Invalid platform");
            }

            return await _hubService.SendNotification(notification.Platform, payload);
        }
    }
}
