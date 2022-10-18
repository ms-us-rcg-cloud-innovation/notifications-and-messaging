using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using NotificationHub.Maui.Models;
using NotificationHub.Maui.Platforms.Android;
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

        private const string DEFAULT_INSTALLATION_TEMPLATE = "default";
        private InstallationTemplate _installationTemplate;

        partial void Init()
        {
            FirebasePushNotificationManager.Initialize(MainApplication.Context, true);

            CrossFirebasePushNotification.Current.OnTokenRefresh += (sender, args) =>
            {
                // update registration through API
                var deviceId = GetDeviceId();
                Secure.Set
                var installation = new DeviceInstallation
                {
                    InstallationId = GetDeviceId(),
                    Platform = "fcm",
                    PushChannel = args.Token
                };
            };


            RegisterUserCommand = new RelayCommand(RegisterUser);

            _notificationHandler = new DefaultAndroidNotificationHandler();
            _notificationHandler.NotificationReceived += (s, e) => { NotificationMessage = e.Message.Body; };
        }

        [ObservableProperty]
        private string _userId;

        [ObservableProperty]
        private string _installationId;

        [ObservableProperty]
        private string _tags;

        [ObservableProperty]
        private string _tagList;

        [ObservableProperty]
        private string _currentUser;

        public ICommand RegisterUserCommand { get; private set; }

        private void RegisterUser()
        {
            CurrentUser = UserId;
            TagList = Tags;
            // local constants is a git ignored file
            // create a class with those constants corresponding to your hub details
            
            var tags = Tags?.Split("; ") ?? new string[] { "" };
            AzNH.NotificationHub.AddTags(tags);
        }

        private string GetDeviceId()
        {
            var context = MainApplication.Context;

            var id = Secure.GetString(context.ContentResolver, Secure.AndroidId);

            return id;
        }
    }
}
