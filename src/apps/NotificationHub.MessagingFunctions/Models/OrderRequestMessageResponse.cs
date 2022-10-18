using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

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

        public HttpResponseData HttpResponse { get; }
    }
}
