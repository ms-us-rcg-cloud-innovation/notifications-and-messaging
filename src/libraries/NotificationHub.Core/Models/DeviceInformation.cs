using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NotificationHub.Core.Models
{
    public class DeviceInformation
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("registrationDate")]
        public DateTime RegistrationDate { get; set; }

        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }
        
        [JsonPropertyName("deviceType")]
        public string DeviceType { get; set; }

        [JsonPropertyName("os")]
        public string OS { get; set; }
        
        [JsonPropertyName("osVersion")]
        public string OSVersion { get; set; }
        
        [JsonPropertyName("pnsPlatform")]
        public string PNSPlatform { get; set; }

        [JsonPropertyName("pnsHandle")]
        public string PNSHandle { get; set; }

        [JsonPropertyName("lastUpdatedHandleDate")]
        public DateTime LastUpdatedHandleDate { get; set; }

        [JsonPropertyName("tags ")]
        public List<string> Tags { get; set; } = new();
    }
}
