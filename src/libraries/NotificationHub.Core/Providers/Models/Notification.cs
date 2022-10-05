namespace NotificationHub.Core.Providers.Models;

public class Notification
{
    public string Platform { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string[] Tags { get; set; }
}

