using System.Text.Json.Serialization;

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

        [JsonPropertyName("importance")]
        public string Importance { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
