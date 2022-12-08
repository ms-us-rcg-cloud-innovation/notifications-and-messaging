// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.Text.Json.Serialization;
using Functions.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Functions.Functions
{
    public class EmailEventGridHandler
    {
        private readonly ILogger _logger;
        private readonly CosmosClient _cosmosClient;

        public EmailEventGridHandler(ILogger<EmailEventGridHandler> logger)
        {
            _logger = logger;
        }

        public record EventGridMessage(string Id, string Topic, string Subject, string EventType, DateTime EventTime, IDictionary<string, object> Data);

        [Function("EmailEventGridHandler")]
        [CosmosDBOutput(databaseName: "%COSMOS_COMM_DATABASE%"
                      , collectionName: "%COSMOS_EMAIL_EVENT_COLLECTION%"
                      , PartitionKey = "/id"
                      , CreateIfNotExists = true
                      , ConnectionStringSetting = "COSMOS_CONNECTION_STRING")]
        public AcsEmailEvent Run([EventGridTrigger] EventGridMessage gridEvent)
        {
            string messageId = gridEvent.Data["messageId"].ToString();

            AcsEmailEvent acsEmailEvent = new()
            {
                Id = $"{gridEvent.Data["messageId"]}~{gridEvent.Id}",
                MessageId = messageId,
                EventTime = gridEvent.EventTime,
                Data = gridEvent.Data,
                RawEvent = gridEvent
            };

            return acsEmailEvent;
        }
    }
}
