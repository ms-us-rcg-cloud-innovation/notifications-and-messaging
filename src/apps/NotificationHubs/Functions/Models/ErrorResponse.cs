using System.Net;

namespace NotificationsAndMessaging.Models
{
    public class ErrorResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

    }
}
