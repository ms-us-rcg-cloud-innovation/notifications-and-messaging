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
    public class SendMessageResponseModel
    {
        public SendMessageResponseModel(HttpResponseData httpResponse, Notification notification = null)
        {
            PersistedDated = notification;
            HttpResponse = httpResponse;
        }

        [CosmosDBOutput("%COSMOS_DB%"
                      , "%COSMOS_CONTAINER%"
                      , ConnectionStringSetting = "COSMOS_CONNECTION_STRING"
                      , PartitionKey = "/id"
                      , CreateIfNotExists = true)]
        public Notification PersistedDated { get; }

        public HttpResponseData HttpResponse { get;  }
    }
}
