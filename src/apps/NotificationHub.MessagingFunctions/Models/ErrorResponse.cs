using System.Net;

namespace NotificationHub.MessagingFunctions.Models
{
    public class ErrorResponse
    {
        public HttpStatusCode Status { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }

    }
}
