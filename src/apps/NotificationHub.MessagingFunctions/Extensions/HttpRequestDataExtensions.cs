using Microsoft.Azure.Functions.Worker.Http;
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

        public static async Task<HttpResponseData> CreateErrorResponseAsync(this HttpRequestData request)
        {
            var response = request.CreateResponse(HttpStatusCode.InternalServerError);
            
        }
    }
}
