using Microsoft.Azure.NotificationHubs;
using NotificationHub.Core.Builders.Interfaces;
using System.Text.Json;

namespace NotificationHub.Core.Services;

public class NotificationHubService
{
    private readonly INotificationHubClient _client;

    public NotificationHubService(INotificationHubClient client)
    {
        _client = client;
    }

    public async Task<IList<NotificationOutcome>> SendNotificationAsync(NotificationPlatform platform, string payload, IList<string> tags, string tagExpressions, CancellationToken cancellationToken)
    {
        bool hasTags = tags?.Count > 0 == true;
        bool hasExpression = string.IsNullOrEmpty(tagExpressions);
        bool broadcastToAll = !hasTags && !hasExpression;


        List<NotificationOutcome> outcome = new();
        switch(platform)
        {
            case NotificationPlatform.Fcm:
                // default action is to send to all
                if (broadcastToAll)
                {
                    outcome.Add(await _client.SendFcmNativeNotificationAsync(payload, cancellationToken));
                }
                else
                {
                    // you can send notifcation using both
                    // tags and tag expression in one request
                    // they are not mutually exclusive
                    if (hasTags)
                    {// use tags for targeting
                        outcome.Add(await _client.SendFcmNativeNotificationAsync(payload, tags, cancellationToken));
                    }
                    if (hasExpression)
                    {// use exprssion for targeting
                        outcome.Add(await _client.SendFcmNativeNotificationAsync(payload, tagExpressions, cancellationToken));
                    }
                }
                break;
            default:
                throw new Exception($"Unsupported platform {platform}");
        }

        return outcome;
    }

    public async Task UpsertDeviceRegistrationAsync(string id, string channel, NotificationPlatform platform, CancellationToken cancellationToken, IList<string> tags = null)
    {
        var installation = CreateInstallation(id, channel, platform, tags);

        await _client.CreateOrUpdateInstallationAsync(installation, cancellationToken);
    }

    public async Task DeleteDeviceInstallation(string id, CancellationToken cancellationToken)
    {
        await _client.DeleteInstallationAsync(id, cancellationToken);
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

