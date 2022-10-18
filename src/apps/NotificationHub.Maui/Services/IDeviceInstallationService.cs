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
        string Token { get; set; }
        bool IsNotificationsSupported(out string error);
        string GetDeviceId();
        DeviceInstallation GetDeviceInstallation(params string[] tags);
    }
}
