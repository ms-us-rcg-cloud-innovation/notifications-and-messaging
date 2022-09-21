﻿using NotificationHub.Maui.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.Services
{
    public interface INotificationHandler
    {
        Task ReceiveNotificationAsync(NotificationMessage message, IDictionary<string, object> properties = null);
    }
}
