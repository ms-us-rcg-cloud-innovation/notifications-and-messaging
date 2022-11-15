namespace NotificationsAndMessaging.MobileMaui.Models;

public class NotificationMessage
{
    public string Title { get; set; }
    public string Body { get; set; }
    public DateTime TimeStamp { get; set; }
    public IDictionary<string, string> Tags { get; set; }
    public IDictionary<string, object> Data { get; set; }

}


