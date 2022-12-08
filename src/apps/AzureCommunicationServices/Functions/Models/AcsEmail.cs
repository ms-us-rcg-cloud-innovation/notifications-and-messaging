using Azure;
using Azure.Communication.Email.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Functions.Models
{
    public class AcsEmail
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("creationTimeStamp")]
        public DateTime CreationTimeStamp { get; set; }

        [JsonPropertyName("recipients")]
        public IEnumerable<string> Recipients { get; set; }

        [JsonPropertyName("subject")]
        public string Subject { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
