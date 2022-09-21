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
        public NotificationMessage Message { get; set; }
        public string Platform { get; set; }
    }
}
