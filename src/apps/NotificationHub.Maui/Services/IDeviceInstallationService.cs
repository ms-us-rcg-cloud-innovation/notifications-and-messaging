using NotificationHub.Maui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.Services
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
