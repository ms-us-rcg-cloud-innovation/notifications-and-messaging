using Android.Hardware.Usb;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AzNH = WindowsAzure.Messaging.NotificationHubs;

namespace NotificationHub.Maui.ViewModels
{
    public partial class MainPageViewModel
        : ObservableObject
    {
        partial void Init()
        {
            SetUserSettingsCommand = new RelayCommand(SetUserSettings);

            _notificationHandler = new DefaultAndroidNotificationHandler();
            _notificationHandler.NotificationReceived += (s, e) => { NotificationMessage = e.Message.Body; };
        }

        [ObservableProperty]
        private string _userId;

        [ObservableProperty]
        private string _tags;

        public ICommand SetUserSettingsCommand { get; private set; }

        private void SetUserSettings()
        {
            AzNH.NotificationHub.SetUserId(UserId);

            var tags = Tags.Split(",");
            AzNH.NotificationHub.AddTags(tags);
        }
    }
}
