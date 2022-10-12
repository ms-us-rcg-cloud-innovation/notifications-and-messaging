using Microsoft.Azure.NotificationHubs;
using NotificationHub.Core.Builders.Interfaces;
using System.Text.Json;

namespace NotificationHub.Core.Services;

public class NotificationHubService
{
    private readonly INotificationHubClient _client;
    private readonly NotificationHubService _hubService;
    private readonly INotificationPayloadBuilder _payloadBuilder;

    public NotificationHubService(INotificationHubClient client)
    {
        _client = client;
    }

    public async Task<NotificationOutcome> SendNotificationAsync(string platform, string payload, IList<string> tags = null)
    {
        switch(platform)
        {
            case "fcm":
                return await _client.SendFcmNativeNotificationAsync(payload, tags);
            case "aps":
                return await _client.SendAppleNativeNotificationAsync(payload);
            default:
                throw new Exception("Invalid Platform");
        }
    }

    public async Task UpsertDeviceRegistrationAsync(string id, string channel, NotificationPlatform platform, IList<string> tags = null)
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

