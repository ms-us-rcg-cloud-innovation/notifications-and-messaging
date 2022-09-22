using CommunityToolkit.Mvvm.ComponentModel;
using NotificationHub.Maui.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationHub.Maui.ViewModels
{
    public partial class MainPageViewModel
        : ObservableObject
    {
        private readonly INotificationHandler _notificationHandler;

        [ObservableProperty]
        private string notificationMessage = "Notification message";

        public MainPageViewModel(INotificationHandler notificationHandler)
        {
            _notificationHandler = notificationHandler;
            _notificationHandler.NotificationReceived += (s, e) => { NotificationMessage = e.Message.Body; };
        }
    }
}
