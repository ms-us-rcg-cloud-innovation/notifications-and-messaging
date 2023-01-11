using Azure;
using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Azure.Data.Tables;
using Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Functions.Functions
{
    public class EmailRequestProcessor
    {
        private readonly ILogger _logger;
        private readonly EmailClient _emailClient;
        private readonly IConfiguration _configuration;
        private readonly TableServiceClient _tableServiceClient;

        public EmailRequestProcessor(ILogger<EmailRequestProcessor> logger, EmailClient emailClient, TableServiceClient tableClient, IConfiguration configuration)
        {
            _logger = logger;
            _emailClient = emailClient;
            _configuration = configuration;
            _tableServiceClient = tableClient;
        }


        [Function("EmailRequestProcessor")]
        public async Task RunAsync(
            [ServiceBusTrigger(queueName: "%SB_SEND_EMAIL_QUEUE%"
                             , Connection = "SB_CONNECTION_STRING")] SendEmailMessage queueMessage
                             , CancellationToken cancellationToken
                             , FunctionContext context)
        {
            if(!queueMessage.IsValid(out var errors))
            {
                _logger.LogError("Model validation failed: {Errors}", errors);
            }
            else
            {
                var enqueuedDateTimeUtcValue = context.BindingContext.BindingData["EnqueuedTimeUtc"].ToString()[1..23]; // grab date time value only since it's wrapped in nested quotes

                var queueMessageId = context.BindingContext.BindingData["MessageId"].ToString();
                var enqueuedDateTimeUtc = DateTime.Parse(enqueuedDateTimeUtcValue);


                var sender = _configuration.GetValue<string>("EMAIL_SENDER");
                var toAddresses = queueMessage.To.Select(toAddr => new EmailAddress(toAddr));
                EmailRecipients recipients = new(toAddresses);
                EmailContent content = new(queueMessage.Subject);
                content.Html = queueMessage.Body;

                EmailMessage emailMessage = new(sender, content, recipients);
                emailMessage.Importance = queueMessage.Importance;

                var emailResult = await _emailClient.SendAsync(emailMessage, cancellationToken);

                _logger.LogInformation("Email request sent to Azure Communication Services. Message ID {messageId}", queueMessageId);

                var tableResponse = await SaveDataToStorageAccountAsync(emailResult.Value.MessageId, queueMessage);
            }
        }

        private async Task<Response> SaveDataToStorageAccountAsync(string messageId, SendEmailMessage queueMessage)
        {

            AcsEmailTableEntity email = new()
            {
                // required storage table properties
                PartitionKey = "email",
                RowKey = messageId,
                Importance = queueMessage.Importance,
                Subject= queueMessage.Subject,
                Data = JsonSerializer.Serialize(queueMessage, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })
            };

            var table = _configuration.GetValue<string>("SA_EMAIL_TABLE");
            await _tableServiceClient.CreateTableIfNotExistsAsync(table);
            TableClient tableClient = _tableServiceClient.GetTableClient(table);
            Response tableResponse = await tableClient.AddEntityAsync(email);

            return tableResponse;
        }
    }
}
