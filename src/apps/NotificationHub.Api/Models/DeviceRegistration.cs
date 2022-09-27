namespace NotificationHub.Api.Models;

public class DeviceRegistration
{
    public string DeviceId { get; set; }
    public string PushChannel { get; set; }
    public string Platform { get; set; }
    public string[] Tags { get; set; }
}

