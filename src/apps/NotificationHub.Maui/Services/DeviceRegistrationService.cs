using Microsoft.Extensions.Logging;
using NotificationHub.Maui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using UIKit;

namespace NotificationHub.Maui.Services
{
    public class DeviceRegistrationService
    {
        private const string REGISTER_DEVICE_ROUTE = "register-device";

        private readonly ILogger<DeviceRegistrationService> _logger;
        private readonly HttpClient _client;

        public DeviceRegistrationService(ILogger<DeviceRegistrationService> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<string> UpsertDeviceInstallationAsync(DeviceInstallation deviceInstallation)
        {            
            using var request = new HttpRequestMessage(HttpMethod.Post, REGISTER_DEVICE_ROUTE);
            request.Content = new StringContent(JsonSerializer.Serialize(deviceInstallation));
            using var response = await _client.SendAsync(request);

            var outcome = await response.Content.ReadAsStringAsync();

            return outcome;
        }
    }
}
