using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;


namespace Functions.Functions
{
    public record SendEmailMessage(
               string[] To
             , string Subject
             , string Body
             , string Importance);

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
                       , Route = "submit")] HttpRequestData  req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            EmailIntakeResponse multiResponse = new()
            {
                HttpResponse = req.CreateResponse(HttpStatusCode.Accepted)
            };

            var rawRequest = await req.ReadAsStringAsync();

            if(rawRequest == null) {
                multiResponse.HttpResponse.StatusCode = HttpStatusCode.BadRequest;
                await multiResponse.HttpResponse.WriteAsJsonAsync(new { Message = "No Content"});

                return multiResponse;
            }

            var email = JsonSerializer.Deserialize<SendEmailMessage>(rawRequest);
            multiResponse.ServiceBusData = email;
            await multiResponse.HttpResponse.WriteAsJsonAsync(new { Message = "Email enqueued" });

            return multiResponse;
        }
    }


}
