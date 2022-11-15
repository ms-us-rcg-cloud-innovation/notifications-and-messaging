using NotificationsAndMessaging.MobileMaui.Models;

namespace NotificationsAndMessaging.MobileMaui.Services.NotificationHubFuns
{
    public interface IDeviceInstallationService
    {
        Task<string> GetTokenAsync();
        Task SetTokenAsync(string token);

        string GetDeviceId();

        bool AreNotificationSupported(out string error);

        Task<DeviceInstallation> GenerateDeviceInstallationAsync(params string[] tags);
    }
}
