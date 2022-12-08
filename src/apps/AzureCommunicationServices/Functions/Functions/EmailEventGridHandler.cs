// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using Functions.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Functions.Functions
{
    public class EmailEventGridHandler
    {
        public record EventGridMessage(string Id, string Topic, string Subject, string EventType, DateTime EventTime, IDictionary<string, object> Data);

        [Function("EmailEventGridHandler")]
        [CosmosDBOutput(databaseName: "%COSMOS_COMM_DATABASE%"
                      , collectionName: "%COSMOS_EMAIL_EVENT_COLLECTION%"
                      , PartitionKey = "/id"
                      , CreateIfNotExists = true
                      , ConnectionStringSetting = "COSMOS_CONNECTION_STRING")]
        public AcsEmailEvent Run([EventGridTrigger] EventGridMessage gridEvent)
            => new()
            {
                Id = gridEvent.Id,
                MessageId = gridEvent.Data["messageId"].ToString(),
                EventTime = gridEvent.EventTime,
                Data = gridEvent.Data,
                RawEvent = gridEvent
            };
    }
}
