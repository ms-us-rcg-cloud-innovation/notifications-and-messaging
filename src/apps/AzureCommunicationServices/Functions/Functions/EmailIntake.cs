using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;
using Azure.Core;
using Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;


namespace Functions.Functions
{


    public class EmailIntakeResponse
    {
        [ServiceBusOutput(queueOrTopicName: "%SB_SEND_EMAIL_QUEUE%"
                        , entityType: ServiceBusEntityType.Queue
                        , Connection = "SB_CONNECTION_STRING")]
        public SendEmailMessage ServiceBusData { get; set; }

        public HttpResponseData HttpResponse { get; set; }
    }

    public class EmailIntake
    {
        private readonly ILogger _logger;

        public EmailIntake(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<EmailIntake>();
        }


        [Function("EmailIntake")]

        public async Task<EmailIntakeResponse> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function
                       , methods: "post"
                       , Route = "submit")] HttpRequestData req
                       , CancellationToken cancellationToken)
        {
            EmailIntakeResponse multiResponse = new()
            {
                HttpResponse = req.CreateResponse(HttpStatusCode.Accepted)
            };

            var emailRequest = await req.ReadFromJsonAsync<SendEmailMessage>();

            if (!emailRequest.IsValid(out var errors))
            {
                multiResponse.HttpResponse.StatusCode = HttpStatusCode.BadRequest;
                await multiResponse.HttpResponse.WriteAsJsonAsync(new { Error = errors });

                _logger.LogError("Model validation failed: {Errors}", errors);
            }
            else
            {
                multiResponse.ServiceBusData = emailRequest;
                await multiResponse.HttpResponse.WriteAsJsonAsync(new { Message = "Email enqueued" });
            }


            return multiResponse;
        }
    }


}
