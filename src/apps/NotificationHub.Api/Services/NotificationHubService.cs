using Microsoft.Azure.NotificationHubs;
using NotificationHub.Api.Services.Models;
using System.Text.Json;

namespace NotificationHub.Api.Services;

public class NotificationHubService
{
    private readonly INotificationHubClient _client;

    public NotificationHubService(INotificationHubClient client)
    {
        _client = client;
    }

    public async Task<NotificationOutcome> SendPushNotification(NotificationPlatform platform, string title, string message)
    {
        var notification = new AndroidNotification
        {
            Notification = new Dictionary<string, string>
            {
                { "title", title },
                { "body", message }
            }
        };

        var jsonPayload = System.Text.Json.JsonSerializer.Serialize(notification, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });

        var outcome = await _client.SendFcmNativeNotificationAsync(jsonPayload);

        return outcome;
    }

    public async Task UpsertDeviceRegistrationAsync(string id, string channel, NotificationPlatform platform, IList<string>? tags = null)
    {
        var installation = CreateInstallation(id, channel, platform, tags);

        await _client.CreateOrUpdateInstallationAsync(installation);
    }

    private static Installation CreateInstallation(string id, string channel, NotificationPlatform platform, IList<string> tags)
        => new Installation
        {
            InstallationId = id,
            PushChannel = channel,
            Platform = platform,
            Tags = tags
        };
}

