namespace NotificationHub.Core.Services.Models
{
    public class AndroidNotification
    {
        public IDictionary<string, string> Notification { get; set; }
        public IDictionary<string, string> Data { get; set; }
    }
}
