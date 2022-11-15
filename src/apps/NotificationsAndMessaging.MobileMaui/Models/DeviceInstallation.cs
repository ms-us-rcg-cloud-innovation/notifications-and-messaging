namespace NotificationsAndMessaging.MobileMaui.Models
{
    public class DeviceInstallation
    {
        public string Id { get; set; }
        public string Platform { get; set; }
        public string PushChannel { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
