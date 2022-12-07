using Azure;
using Azure.Communication.Email.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Functions.Models
{
    public class SentEmailMessage
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        public IEnumerable<string> Recipients { get; set; }

        public EmailImportance? Importance { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public string Status { get; set; }

        public List<SentEmailEvent> Events { get; set; } = new();
    }
}
