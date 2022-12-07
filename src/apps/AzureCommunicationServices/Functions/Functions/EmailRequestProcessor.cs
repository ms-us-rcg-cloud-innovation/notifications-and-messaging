using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Functions.Functions
{
    public class EmailRequestProcessor
    {
        private readonly ILogger _logger;
        private readonly EmailClient _emailClient;
        private readonly IConfiguration _configuration;

        public EmailRequestProcessor(ILogger<EmailRequestProcessor> logger, EmailClient emailClient, IConfiguration configuration)
        {
            _logger = logger;
            _emailClient = emailClient;
            _configuration = configuration;
        }

        [Function("EmailRequestProcessor")]
        [CosmosDBOutput(databaseName: "communication"
                      , collectionName: "emails"
                      , CreateIfNotExists = true
                      , PartitionKey = "/id"
                      , ConnectionStringSetting = "COSMOS_CONNECTION_STRING")]
        public async Task<SentEmailMessage> RunAsync(
            [ServiceBusTrigger(queueName: "email-queue"
                             , Connection = "SB_CONNECTION_STRING")] EmailQueueMessage emailQueueMessage)
        {
            var sender = _configuration.GetValue<string>("EMAIL_SENDER"); 
            var toAddresses = emailQueueMessage.To.Select(toAddr => new EmailAddress(toAddr));
            EmailRecipients recipients = new(toAddresses);
            EmailContent content = new(emailQueueMessage.Subject);
            content.Html = emailQueueMessage.Body;

            EmailMessage emailMessage = new(sender, content, recipients);
            emailMessage.Importance = emailQueueMessage.Importance;

            var emailResult = await _emailClient.SendAsync(emailMessage);

            SentEmailMessage message = new()
            {
                Id = emailResult.Value.MessageId, 
                Importance = emailMessage.Importance,
                Recipients = toAddresses.Select(x => x.Email),
                Subject = emailQueueMessage.Subject, 
                Body = emailQueueMessage.Body,
                Status = "Sent to ACS"
            };

            return message;
        }
    }
}
