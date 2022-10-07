using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.Models;
using NotificationHub.Core.Providers;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class ProcessNotificationRequest
    {
        private readonly AzureNotificationProvider _notificationProvider;
        private readonly ILogger _logger;

        public ProcessNotificationRequest(ILoggerFactory loggerFactory, AzureNotificationProvider notificationProvider)
        {
            _notificationProvider = notificationProvider;
            _logger = loggerFactory.CreateLogger<ProcessNotificationRequest>();
        }

        [Function("ProcessNotificationRequest")]
        public async Task RunAsync([CosmosDBTrigger(
            databaseName: "%COSMOS_DB%",
            collectionName: "%COSMOS_CONTAINER%",
            ConnectionStringSetting = "COSMOS_CONNECTION_STRING",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<Notification> input)
        {
            await Parallel.ForEachAsync(input, async (n, ct) =>
            {
                _logger.LogInformation("Sending message to Notification Hub");
                await _notificationProvider.SendNotificationAsync(n);
            });
        }
    }
}
