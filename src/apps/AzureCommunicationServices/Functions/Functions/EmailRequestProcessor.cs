using Azure.Communication.Email;
using Azure.Communication.Email.Models;
using Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace Functions.Functions
{
    public class EmailRequestProcessor
    {
        private readonly ILogger _logger;
        private readonly EmailClient _emailClient;
        private readonly IConfiguration _configuration;

        public EmailRequestProcessor(EmailClient emailClient, IConfiguration configuration)
        {
            _emailClient = emailClient;
            _configuration = configuration;
        }

        public record QueueMessage([Required] string[] To
                                 , [Required] string Subject
                                 , [Required] string Body
                                 , string Importance);

        [Function("EmailRequestProcessor")]
        [CosmosDBOutput(databaseName: "%COSMOS_COMM_DATABASE%"
                      , collectionName: "%COSMOS_EMAIL_COLLECTION%"
                      , PartitionKey = "/id"
                      , CreateIfNotExists = true
                      , ConnectionStringSetting = "COSMOS_CONNECTION_STRING")]
        public async Task<AcsEmail> RunAsync(
            [ServiceBusTrigger(queueName: "email-queue"
                             , Connection = "SB_CONNECTION_STRING")] QueueMessage emailQueueMessage
                             , CancellationToken cancellationToken)
        {
            var sender = _configuration.GetValue<string>("EMAIL_SENDER");
            var toAddresses = emailQueueMessage.To.Select(toAddr => new EmailAddress(toAddr));
            EmailRecipients recipients = new(toAddresses);
            EmailContent content = new(emailQueueMessage.Subject);
            content.Html = emailQueueMessage.Body;

            EmailMessage emailMessage = new(sender, content, recipients);
            emailMessage.Importance = emailQueueMessage.Importance;

            var emailResult = await _emailClient.SendAsync(emailMessage, cancellationToken);

            return new()
            {
                Id = emailResult.Value.MessageId,
                Recipients = emailQueueMessage.To,
                Subject = emailQueueMessage.Subject,
                Importance = emailQueueMessage.Importance,
                Body = emailQueueMessage.Body,
                CreationTimeStamp = DateTime.UtcNow
            };
        }
    }
}
