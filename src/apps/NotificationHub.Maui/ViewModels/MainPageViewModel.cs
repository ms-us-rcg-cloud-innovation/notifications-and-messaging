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
        private INotificationHandler _notificationHandler;

        public MainPageViewModel()
        {
            Init();
        }

        partial void Init();

        private string notificationMessage;
        public string NotificationMessage
        {
            get => notificationMessage;
            set
            {
                SetProperty(ref notificationMessage, value);
            }
        }
    }
}
