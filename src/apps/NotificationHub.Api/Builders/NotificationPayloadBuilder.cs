using NotificationHub.Api.Builders.Interfaces;

namespace NotificationHub.Api.Builders
{
    public class NotificationPayloadBuilder
        : INotificationPayloadBuilder
    {        

        public string? Body { get; private set; }
        public string? Title { get; private set; }

        public INotificationPayloadBuilder AddBody(string value)
        {
            Body = value;
            return this;
        }

        public INotificationPayloadBuilder AddTitle(string value)
        {
            Title = value;
            return this;
        }

        public string BuildAndroidPayload()
            => "{\"notification\":{\"title\":\"" + Title + "\",\"body\":\"" + Body + "\"}}";

        public string BuildApplePayload()
            => "{\"aps\":{\"alert\": {\"title\": \""+ Title +"\",\"body\" : \"" + Body + "\"}}}";

    }
}
