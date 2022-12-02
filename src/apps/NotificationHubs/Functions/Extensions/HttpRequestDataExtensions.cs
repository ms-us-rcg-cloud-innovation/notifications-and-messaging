using Microsoft.Azure.Functions.Worker.Http;
using NotificationsAndMessaging.Models;
using System.Net;

namespace NotificationsAndMessaging.NotificationHubs.Functions.Extensions
{
    internal static class HttpRequestDataExtensions
    {
        public static async Task<HttpResponseData> CreateOkResponseAsync<T>(this HttpRequestData request, T content)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(content);

            return response;
        }

        public static async Task<HttpResponseData> CreateErrorResponseAsync(this HttpRequestData request, string message, HttpStatusCode status = HttpStatusCode.BadRequest)
        {
            var content = new ErrorResponse()
            {
                Status = status,
                Title = "Error during request processing",
                Message = message
            };
            var response = request.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(content);

            return response;
        }
    }
}
