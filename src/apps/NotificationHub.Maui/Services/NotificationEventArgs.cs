using NotificationHub.Maui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.Services
{
    public class NotificationEventArgs
        : EventArgs
    {
        public NotificationEventArgs(NotificationMessage message, string platform)
            : base()
        {
            Message = message;
            Platform = platform;            
        }

        public NotificationMessage Message { get; }
        public string Platform { get; }
    }
}
