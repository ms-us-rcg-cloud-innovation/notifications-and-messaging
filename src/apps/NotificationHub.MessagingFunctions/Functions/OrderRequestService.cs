using System;
using System.Collections.Generic;
using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.Builders;
using NotificationHub.Core.Builders.Interfaces;
using NotificationHub.Core.Models;
using NotificationHub.Core.Services;

namespace NotificationHub.MessagingFunctions.Functions
{
    public class OrderRequestService
    {
        public record StoreDetails(string Id, string StoreName, string TopicName);

        private readonly ILogger _logger;
        private readonly ServiceBusClient _servicebusClient;        

        public OrderRequestService(ILoggerFactory loggerFactory, ServiceBusClient serviceBusClient)
        {
            _logger = loggerFactory.CreateLogger<OrderRequestService>();
            _servicebusClient = serviceBusClient;
        }

        [Function(nameof(OrderRequestService))]
        public async Task RunAsync(
             [CosmosDBTrigger(
                  databaseName: "%COSMOS_DB%"
                , collectionName: "%COSMOS_REQUEST_CONTAINER%"
                , ConnectionStringSetting = "COSMOS_CONNECTION_STRING"
                , LeaseCollectionName = "leases"   
                , LeaseCollectionPrefix = $"{nameof(OrderRequestService)}__"
                , CreateLeaseCollectionIfNotExists = true)] IReadOnlyList<OrderRequestMessage> orderRequests
            , [CosmosDBInput(
                databaseName: "%COSMOS_DB%"
                , collectionName: "%COSMOS_STORE_CONTAINER%"
                , ConnectionStringSetting = "COSMOS_CONNECTION_STRING")] IReadOnlyList<StoreDetails> storeDetails)
        {
            _logger.LogInformation("Processing broadcast message in " + nameof(OrderRequestService));
            var storeDictionary = storeDetails.ToDictionary(k => k.Id);

            await Parallel.ForEachAsync(orderRequests, async (order, ct) =>
            {
                // TO DO: Create a SB Sender pool that will
                // pool senders by topic name to improve system performance
                // and stability


                // TO DO: Use sessions to link a user to a store and thus when a 
                // and order is complete they'll reply to the same session using
                // the ReplyToSessionId property

                if(storeDictionary.TryGetValue(order.StoreId, out var store))
                {
                    await using var sender = _servicebusClient.CreateSender(store.TopicName);
                                       
                    var sbMessage = new ServiceBusMessage
                    {
                        CorrelationId = order.Id,
                        Body = new BinaryData(order
                                            , options: new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }
                                            , type: typeof(OrderRequestMessage))
                        
                    }; 
                     await sender.SendMessageAsync(sbMessage);
                }
            });
        }
    }
}
