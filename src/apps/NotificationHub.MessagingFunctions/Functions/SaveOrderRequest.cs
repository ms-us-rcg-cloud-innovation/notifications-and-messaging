using System.Collections.Generic;
using System.Net;
using Azure.Core;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.FunctionHelpers;
using NotificationHub.Core.Models;
using NotificationHub.MessagingFunctions.Models;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class SaveOrderRequest
    {
        private readonly ILogger _logger;

        public SaveOrderRequest(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<SaveOrderRequest>();
        }

        [Function(nameof(SaveOrderRequest))]
        public async Task<OrderRequestMessageResponse> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function
                         , "post"
                         , Route = "submit-request")] HttpRequestData request)
        {
            _logger.LogInformation("Http triggered function for processing notification submission");
            try
            {
                var broadcastMessage = await request.ReadFromJsonAsync<OrderRequestMessage>();

                if (broadcastMessage is null)
                {
                    return new(await request.CreateErrorResponseAsync("No content in request"));
                }

                return new(await request.CreateOkResponseAsync("Messaged persisted"), broadcastMessage);
            }
            catch (Exception e)
            {
                return new(await request.CreateErrorResponseAsync(e.Message, HttpStatusCode.InternalServerError));
            }
        }
    }
}
