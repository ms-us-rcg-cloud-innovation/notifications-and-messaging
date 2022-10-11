namespace NotificationHub.Core.Models;

public class PushNotification
    : NotificationMessageBase
{
    public string Platform { get; set; }

    public string[] Tags { get; set; }
}

