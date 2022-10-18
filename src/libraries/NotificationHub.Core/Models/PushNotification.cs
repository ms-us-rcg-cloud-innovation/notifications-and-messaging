namespace NotificationHub.Core.Models;

public class PushNotification
    : NotificationMessageBase
{
    public string Platform { get; set; }

    public List<string> Tags { get; set; } = new List<string>();
}

