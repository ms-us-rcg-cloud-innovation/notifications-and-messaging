using Microsoft.Azure.Functions.Worker.Http;
using NotificationHub.MessagingFunctions.Models;
using System.Net;

namespace NotificationHub.Core.FunctionHelpers
{
    internal static class HttpRequestDataExtensions
    {
        public static async Task<HttpResponseData> CreateOkResponseAsync<T>(this HttpRequestData request, T content, CancellationToken cancellationToken = default)
        {
            var response = request.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(content, cancellationToken);

            return response;
        }

        public static async Task<HttpResponseData> CreateErrorResponseAsync(this HttpRequestData request, string message, HttpStatusCode status = HttpStatusCode.BadRequest, CancellationToken cancellationToken = default)
        {
            var content = new ErrorResponse()
            {
                Status = status,
                Title = "Error during request processing",
                Message = message
            };
            var response = request.CreateResponse(HttpStatusCode.InternalServerError);
            await response.WriteAsJsonAsync(content, cancellationToken);

            return response;
        }
    }
}
