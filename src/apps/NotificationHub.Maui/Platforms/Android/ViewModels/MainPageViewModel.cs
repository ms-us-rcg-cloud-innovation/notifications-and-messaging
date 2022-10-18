using Android.Gms.Common;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Platforms.Android;
using NotificationHub.Maui.Platforms.Android.Helpers;
using Plugin.FirebasePushNotification;
using System.Windows.Input;
using WindowsAzure.Messaging.NotificationHubs;
using static Android.Provider.Settings;
using AzNH = WindowsAzure.Messaging.NotificationHubs;

namespace NotificationHub.Maui.ViewModels
{
    public partial class MainPageViewModel
        : ObservableObject
    {
  
        private record DeviceDetails(string Id, string Channel, string Platform, IList<string> Tags);
        
        private InstallationTemplate _installationTemplate;

        partial void Init()
        {
            RegisterFcmTokenRefreshHandler();
            RegisterNotificationReceivedHandler();
            FirebasePushNotificationManager.Initialize(MainApplication.Context, true);

            RegisterUserCommand = new AsyncRelayCommand(RegisterUserAsync);
        }


        public ICommand RegisterUserCommand { get; private set; }

        private async Task RegisterUserAsync()
        {
            CurrentUser = UserId;
            TagList = Tags;

            var outcome = await RegisterDeviceInstallationAsync();
        }

        private async Task<string> RegisterDeviceInstallationAsync()
        {
            var installation = await _deviceInstallationService.GenerateDeviceInstallationAsync(Tags);
            var outcome = await _deviceRegistrationService.UpsertDeviceInstallationAsync(installation);

            return outcome;
        }

        private void RegisterFcmTokenRefreshHandler()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += async (sender, args) =>
            {
                await _deviceInstallationService.SetTokenAsync(args.Token);
                var outcome = await RegisterDeviceInstallationAsync();

                _logger.LogInformation($"Registration outcome: {outcome}");
            };
        }

        private void RegisterNotificationReceivedHandler()
        {
            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                var title = (string)p.Data["title"];
                var body = (string)p.Data["body"];

                var message = new NotificationMessage
                {
                    Title = title,
                    Body = body,
                    Data = p.Data,
                    TimeStamp = DateTime.Now
                };

                // capture notification while app is active 
                NotificationHelpers.RaiseSystemNotificationWhileInForeground(message);

                this.NotificationMessage = body;
            };
        }

    }
}
