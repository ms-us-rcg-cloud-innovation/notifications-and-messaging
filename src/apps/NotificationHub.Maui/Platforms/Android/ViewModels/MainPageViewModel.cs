using Android.Hardware.Usb;
using Android.Nfc;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Javax.Xml.Transform;
using NotificationHub.Core.Maui.Platforms.Android.Services.Impl;
using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WindowsAzure.Messaging.NotificationHubs;
using AzNH = WindowsAzure.Messaging.NotificationHubs;

namespace NotificationHub.Maui.ViewModels
{
    public partial class MainPageViewModel
        : ObservableObject
    {
        private const string DEFAULT_INSTALLATION_TEMPLATE = "default";
        private InstallationTemplate _installationTemplate;

        partial void Init()
        {
            RegisterUsersCommand = new RelayCommand(RegisterUser);

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

        public ICommand RegisterUsersCommand { get; private set; }

        private void RegisterUser()
        {
            CurrentUser = UserId;
            InstallationId = CurrentUser;
            
            
        }

        private void Instance_InstanceInstallationSaved(object sender, InstallationAdapterEventArgs e, string[] tags)
        {

            AzNH.NotificationHub.RemoveTags(AzNH.NotificationHub.Tags.ToEnumerable<string>().ToList());
            AzNH.NotificationHub.AddTags(tags);
            TagList = string.Join("; ", _installationTemplate.Tags.ToEnumerable<string>());

            var sysTags = AzNH.NotificationHub.Tags.ToEnumerable<string>();
        }

        private void UpdateTags()
        {
            var tags = Tags?.Split("; ") ?? new string[] { "" };
            _installationTemplate.AddTags(tags);
            AzNH.NotificationHub.AddTags(tags);
            AzNH.NotificationHub.BeginInstallationUpdate();

        }
    }
}
