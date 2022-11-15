using Microsoft.Extensions.Logging;
using NotificationsAndMessaging.MobileMaui.Models;
using System.Text.Json;

namespace NotificationsAndMessaging.MobileMaui.Services.NotificationHubFuns
{
    public class DeviceRegistrationService
    {
        private readonly ILogger<DeviceRegistrationService> _logger;
        private readonly HttpClient _client;

        public DeviceRegistrationService(ILogger<DeviceRegistrationService> logger, HttpClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<string> UpsertDeviceInstallationAsync(DeviceInstallation deviceInstallation)
        {
<<<<<<< HEAD
            using var request = new HttpRequestMessage(HttpMethod.Post, $"?code={Local_Constants.NH_REGISTRATION_FUNC_TOKEN}");


=======
            using var request = new HttpRequestMessage(HttpMethod.Post, $"?code={Local_Constants.REGISTRATION_FUNC_TOKEN}");


>>>>>>> main
            request.Content = new StringContent(JsonSerializer.Serialize(deviceInstallation));
            using var response = await _client.SendAsync(request);

            var outcome = await response.Content.ReadAsStringAsync();

            return outcome;
        }
    }
}
