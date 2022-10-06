using Microsoft.Azure.Functions.Worker.Http;
using NotificationHub.MessagingFunctions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Core.FunctionHelpers
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
