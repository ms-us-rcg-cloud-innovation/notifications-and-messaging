using Functions.Functions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Functions.Models
{
    public class AcsEmailEvent
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("messageId")]
        public string MessageId { get; set; }

        [JsonPropertyName("eventTime")]
        public DateTime EventTime { get; set; }

        [JsonPropertyName("data")]
        public IDictionary<string, object> Data { get; set; }

        [JsonPropertyName("rawEvent")]
        public object RawEvent { get; set; }
    }

}
