using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Platforms.Android;
using NotificationHub.Maui.Platforms.Android.Helpers;
using Plugin.FirebasePushNotification;
using System.Windows.Input;
using WindowsAzure.Messaging.NotificationHubs;

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
#if DEBUG
            FirebasePushNotificationManager.Initialize(MainApplication.Context, true);
#else
            FirebasePushNotificationManager.Initialize(MainApplication.Context, false);
#endif


            RegisterUserCommand = new AsyncRelayCommand(RegisterUserAsync);
        }


        public ICommand RegisterUserCommand { get; private set; }

        private async Task RegisterUserAsync()
        {
            var outcome = await RegisterDeviceInstallationAsync();
        }

        private async Task<string> RegisterDeviceInstallationAsync()
        {
            var tagList = Tags?.Split(",").Select(x => x.Trim()).ToArray() ?? Array.Empty<string>();

            var installation = await _deviceInstallationService.GenerateDeviceInstallationAsync(tagList);
            var outcome = await _deviceRegistrationService.UpsertDeviceInstallationAsync(installation);

            return outcome;
        }

        private void RegisterFcmTokenRefreshHandler()
        {
            CrossFirebasePushNotification.Current.OnTokenRefresh += async (sender, args) =>
            {
                await _deviceInstallationService.SetTokenAsync(args.Token);
                _logger.LogDebug($"Channel: {args.Token}");
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
