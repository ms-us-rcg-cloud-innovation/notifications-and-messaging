using System;
using System.ComponentModel.DataAnnotations;
using Azure.Data.Tables;
using System.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Azure;
using Functions.Models;
using System.Text.Json;

namespace Functions.Functions
{
    public class HandleEngagementEventMessages
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly TableServiceClient _tableServiceClient;

        public record EmailEventMessage(string Sender
                                      , string MessageId
                                      , DateTime UserActionTimeStamp
                                      , string EngagementContext
                                      , string UserAgent
                                      , string EngagementType);

        public record QueueMessage(EmailEventMessage Data);

        public HandleEngagementEventMessages(ILoggerFactory loggerFactory, TableServiceClient tableClient, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<HandleEngagementEventMessages>();
            _configuration = configuration;
            _tableServiceClient = tableClient;
        }


        [Function("HandleEngagementEventMessages")]
        public async Task RunAsync(
            [ServiceBusTrigger(queueName: "%SB_ENGAGEMENT_EVENT_QUEUE%"
                             , Connection = "SB_CONNECTION_STRING")] QueueMessage queueMessage
                             , CancellationToken cancellationToken
                             , FunctionContext functionContext)
        {
            var eventMessage = queueMessage.Data;
            AcsEmailEventTableEntity email = new()
            {
                // required storage table properties
                PartitionKey = "event",
                RowKey = eventMessage.MessageId,

                EngagementContext = eventMessage.EngagementContext,
                EngagementType = eventMessage.EngagementType,
                Sender = eventMessage.Sender,
                UserActionTimeStamp = DateTime.SpecifyKind(eventMessage.UserActionTimeStamp, DateTimeKind.Utc),
                UserAgent = eventMessage.UserAgent,

                Data = JsonSerializer.Serialize(eventMessage, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };

            var table = _configuration.GetValue<string>("SA_EVENTS_TABLE");
            await _tableServiceClient.CreateTableIfNotExistsAsync(table);
            TableClient tableClient = _tableServiceClient.GetTableClient(table);
            Response tableResponse = await tableClient.AddEntityAsync(email);

            _logger.LogInformation("Table HTTP status for message {MessageId} - {Status}", eventMessage.MessageId, tableResponse.Status);
        }
    }
}
