using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.Models
{
    public class DeviceInstallation
    {
        public string InstallationId { get; set; }
        public string Platform { get; set; }
        public string PushChannel { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
    }
}
