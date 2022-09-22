namespace NotificationHub.Api.Models;

public class Notification
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string[] Tags { get; set; }
}

