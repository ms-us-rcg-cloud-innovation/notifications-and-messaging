namespace NotificationsAndMessaging.CoreLib.NotificationHub.Builders.Interfaces
{
    public interface INotificationPayloadBuilder
    {
        INotificationPayloadBuilder AddTitle(string value);
        INotificationPayloadBuilder AddBody(string value);

        string BuildAndroidPayload();
        string BuildApplePayload();

    }
}
