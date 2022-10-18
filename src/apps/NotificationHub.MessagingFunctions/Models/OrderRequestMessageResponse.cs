using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using NotificationHub.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.MessagingFunctions.Models
{
    public class OrderRequestMessageResponse
    {
        public OrderRequestMessageResponse(HttpResponseData httpResponse, OrderRequestMessage orderRequest = null)
        {
            OrderRequest = orderRequest;
            HttpResponse = httpResponse;
        }

        [CosmosDBOutput("%COSMOS_DB%"
                      , "%COSMOS_REQUEST_CONTAINER%"
                      , ConnectionStringSetting = "COSMOS_CONNECTION_STRING"
                      , PartitionKey = "/id"
                      , CreateIfNotExists = true)]
        public OrderRequestMessage OrderRequest { get; }

        public HttpResponseData HttpResponse { get;  }
    }
}
